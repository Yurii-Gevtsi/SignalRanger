using System;
using System.Collections.Generic;
using Game.Data.Progression;
using Game.Gameplay.Battle.Domain;
using Game.Gameplay.Battle.Motion;
using Game.Gameplay.Battle.Projectiles;
using Game.Gameplay.Battle.Terrain;
using Game.Gameplay.Battle.Units;
using Game.Gameplay.Battle.Weapons;
using UnityEngine;

namespace Game.Gameplay.Battle.Simulation
{
    public sealed partial class BattleSimulation
    {
        public bool CanUseMovement()
        {
            return CanAcceptMovementInput() && !HasActiveMotion;
        }

        public bool CanAcceptMovementInput()
        {
            // A signal rocket standing at the feet leaves its owner free to step away before it lifts off.
            var skyHookArming = IsActiveSkyHookArming;
            if (Result.IsFinished || HasProjectilesInFlight && !skyHookArming || HasActiveWeaponEffect)
            {
                return false;
            }

            // A spent action normally freezes movement, except during the
            // retreat window: movement stays live while firing remains blocked.
            if (_state.Turn.ActionSpent && !IsPlayerRetreatActive && !skyHookArming)
            {
                return false;
            }

            if (IsActiveUnitDisoriented() || IsActiveUnitFrozen())
            {
                return false;
            }

            return _state.Turn.ActiveTeam switch
            {
                // The player is bounded by the on-screen turn timer.
                BattleTeamId.Player => _playerMovement.HasTimeRemaining,
                // The opponent bot paces itself through its own turn
                // controller, so the simulation imposes no movement time cap.
                BattleTeamId.Opponent => true,
                _ => false
            };
        }

        public BattleWalkOutcome Walk(int direction, float deltaTime)
        {
            if (direction == 0)
            {
                _walkIntentDirection = 0;
                return BattleWalkOutcome.None;
            }

            if (!CanUseMovement() || direction == 0)
            {
                _walkIntentDirection = 0;
                return BattleWalkOutcome.NotAllowed;
            }

            // The active unit always belongs to the active team; movement
            // permission is already gated per-team by CanUseMovement().
            var unit = GetActiveUnitOrNull();
            if (unit == null)
            {
                _walkIntentDirection = 0;
                return BattleWalkOutcome.NotAllowed;
            }

            var motor = GetMotor(unit.UnitId);
            if (motor == null)
            {
                _walkIntentDirection = 0;
                return BattleWalkOutcome.NotAllowed;
            }

            _walkIntentDirection = direction > 0 ? 1 : -1;
            return BattleWalkOutcome.Moved;
        }

        /// <summary>
        /// Starts the single player jump arc toward the requested screen side.
        /// Its horizontal launch speed is capped below running speed, keeping the
        /// jump useful for obstacles and pits without making repeated jumps faster.
        /// </summary>
        public BattleJumpOutcome TryStartJump(int directionX)
        {
            if (!TryGetJumpReadyMotor(out var unit, out var motor, out var blocked))
            {
                return blocked;
            }

            var archetype = ArchetypeFor(unit.TeamId);
            var direction = directionX == 0 ? 0 : directionX > 0 ? 1 : -1;
            if (direction == 0)
            {
                return BattleJumpOutcome.NotAllowed;
            }

            var apexHeight = Mathf.Lerp(archetype.FrontJumpHeight, archetype.BackJumpHeight, 0.5f);
            var desiredDistance = Mathf.Lerp(archetype.FrontJumpDistance, archetype.BackJumpDistance, 0.5f);
            var verticalSpeed = Mathf.Sqrt(2f * motor.Profile.Gravity * apexHeight);
            var flightTime = 2f * verticalSpeed / motor.Profile.Gravity;
            var runSpeedDistanceCap = motor.Profile.WalkSpeed * StandardJumpHorizontalSpeedRatio * flightTime;
            var horizontalDistance = Mathf.Min(desiredDistance, runSpeedDistanceCap) * direction;

            unit.SetFacing(direction);

            motor.StartJump(horizontalDistance, apexHeight);
            UnitsMoved?.Invoke();
            return BattleJumpOutcome.Jumped;
        }

        private bool TryGetJumpReadyMotor(out BattleUnitState unit, out UnitMotor motor, out BattleJumpOutcome blocked)
        {
            unit = null;
            motor = null;
            blocked = BattleJumpOutcome.NotAllowed;
            if (!CanUseMovement())
            {
                return false;
            }

            unit = GetActiveUnitOrNull();
            if (unit == null)
            {
                return false;
            }

            motor = GetMotor(unit.UnitId);
            if (motor == null || !motor.IsStable)
            {
                blocked = BattleJumpOutcome.NotStable;
                return false;
            }

            return true;
        }

        /// <summary>
        /// A mine or planted charge is a physical ground object, not a world-space
        /// decal. When an explosion removes its support, settle it on the next
        /// surface below so it cannot remain suspended in an empty cavity.
        /// </summary>
        private void DropGroundObjectIfUnsupported(ActiveMine groundObject, int objectId)
        {
            if (!TryFindGroundObjectLanding(groundObject.Position, out var landingPosition))
            {
                return;
            }

            groundObject.MoveTo(landingPosition);
            GroundObjectRepositioned?.Invoke(new GroundObjectRepositionedEvent(objectId, landingPosition));
        }

        private void DropGroundObjectIfUnsupported(ActiveCharge groundObject, int objectId)
        {
            if (!TryFindGroundObjectLanding(groundObject.Position, out var landingPosition))
            {
                return;
            }

            groundObject.MoveTo(landingPosition);
            GroundObjectRepositioned?.Invoke(new GroundObjectRepositionedEvent(objectId, landingPosition));
        }

        private bool TryFindGroundObjectLanding(Vector2 position, out Vector2 landingPosition)
        {
            landingPosition = position;
            if (_terrain == null || _terrain.Grid.IsSolidWorld(position + Vector2.down * 0.1f))
            {
                return false;
            }

            if (!_terrain.Grid.TryFindSurfaceBelow(
                    position.x,
                    position.y - 0.08f,
                    FirePuddleDropSearchDistance,
                    out var surfaceY))
            {
                return false;
            }

            landingPosition = new Vector2(position.x, surfaceY + 0.04f);
            return landingPosition.y < position.y - 0.06f;
        }

        private bool BodyOverlapsTerrain(Vector2 center, float halfWidth, float halfHeight)
        {
            if (_terrain == null)
            {
                return false;
            }

            for (var column = -1; column <= 1; column++)
            {
                for (var row = -2; row <= 2; row++)
                {
                    var sample = center + new Vector2(column * halfWidth, row * halfHeight * 0.5f);
                    if (_terrain.Grid.IsSolidWorld(sample))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Smallest shift of a body centre that leaves the body clear of solid ground.
        /// </summary>
        private Vector2 ResolveFreeBodyCenter(Vector2 point, BattleTeamId teamId)
        {
            var halfHeight = ArchetypeFor(teamId).BodyHeight * 0.5f;
            var halfWidth = _settings.UnitCollisionHalfWidth;
            if (!BodyOverlapsTerrain(point, halfWidth, halfHeight))
            {
                return point;
            }

            const int directions = 24;
            for (var radius = 0.04f; radius <= 3f; radius += 0.04f)
            {
                for (var direction = 0; direction < directions; direction++)
                {
                    var angle = direction * Mathf.PI * 2f / directions;
                    var candidate = point + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;
                    if (!BodyOverlapsTerrain(candidate, halfWidth, halfHeight))
                    {
                        return candidate;
                    }
                }
            }

            return point;
        }

        private Vector2 ResolveUnitLandingPosition(Vector2 desiredPoint, BattleTeamId teamId)
        {
            return TryResolveGroundPosition(desiredPoint, teamId, out var landingPosition) ? landingPosition : desiredPoint;
        }

        private Vector2 ResolveGroundDeployPoint(Vector2 desiredPoint, BattleTeamId teamId)
        {
            // The ground is searched from the droid's own height downwards: a branch or an overhang above the droid
            // must not catch a device it sets down at its feet. A point inside a step is first lifted out of it.
            if (_terrain != null)
            {
                var grid = _terrain.Grid;
                var start = desiredPoint;
                var maxLift = ArchetypeFor(teamId).BodyHeight;
                for (var lift = 0f; lift < maxLift && grid.IsSolidWorld(start); lift += grid.CellSize.y)
                {
                    start.y += grid.CellSize.y;
                }

                if (!grid.IsSolidWorld(start)
                    && grid.TryFindSurfaceBelow(start.x, start.y, _settings.GroundRayDistance, out var surfaceY))
                {
                    return new Vector2(start.x, surfaceY + 0.06f);
                }
            }

            var unitLanding = ResolveUnitLandingPosition(desiredPoint, teamId);
            return unitLanding + Vector2.down * (ArchetypeFor(teamId).BodyHeight * 0.5f - 0.06f);
        }

        private bool TryResolveGroundPosition(Vector2 desiredPoint, BattleTeamId teamId, out Vector2 landingPosition)
        {
            landingPosition = desiredPoint;
            if (_terrain == null)
            {
                return false;
            }

            var bodyHeight = ArchetypeFor(teamId).BodyHeight;
            var scanTop = Mathf.Max(desiredPoint.y + _settings.GroundRayStartHeight, bodyHeight + _settings.GroundRayStartHeight);
            if (!_terrain.Grid.TryFindSurfaceBelow(desiredPoint.x, scanTop, _settings.GroundRayDistance + _settings.GroundRayStartHeight, out var surfaceY))
            {
                return false;
            }

            landingPosition = new Vector2(desiredPoint.x, surfaceY + bodyHeight * 0.5f);
            return true;
        }

        private void StepWalkIntent(float tickSeconds)
        {
            var direction = _walkIntentDirection;
            if (direction == 0 || !CanUseMovement())
            {
                return;
            }

            var unit = GetActiveUnitOrNull();
            if (unit == null)
            {
                _walkIntentDirection = 0;
                return;
            }

            var motor = GetMotor(unit.UnitId);
            if (motor == null)
            {
                _walkIntentDirection = 0;
                return;
            }

            // The shared cold and fire halve the droid's own walking while the status lasts.
            var outcome = motor.Walk(direction, tickSeconds * ResolveManualMovementFactor(unit.UnitId));
            if (outcome == WalkOutcome.None)
            {
                return;
            }

            unit.SetFacing(direction);
            unit.MoveTo(motor.Position);
            UnitsMoved?.Invoke();

            if (outcome != WalkOutcome.Moved)
            {
                _walkIntentDirection = 0;
            }
        }

        private void WakeUnitsWithoutSupport()
        {
            foreach (var team in _state.Teams)
            {
                foreach (var unit in team.Units)
                {
                    if (!unit.IsDefeated)
                    {
                        GetMotor(unit.UnitId)?.WakeIfUnsupported();
                    }
                }
            }
        }

        private void StepUnits()
        {
            foreach (var team in _state.Teams)
            {
                foreach (var unit in team.Units)
                {
                    if (unit.IsDefeated || !_motors.TryGetValue(unit.UnitId, out var motor))
                    {
                        continue;
                    }

                    if (HasFallenOutOfBounds(unit))
                    {
                        DefeatFromFall(unit, motor);
                        continue;
                    }

                    if (motor.IsStable)
                    {
                        continue;
                    }

                    var outcome = motor.Step(_settings.SimulationTickSeconds);
                    unit.MoveTo(motor.Position);
                    if (outcome == MotorStepOutcome.FellOutOfBounds)
                    {
                        DefeatFromFall(unit, motor);
                    }
                    else if (outcome == MotorStepOutcome.Landed)
                    {
                        NudgeApartFromOthers(unit, motor);
                    }
                }
            }
        }

        private bool HasFallenOutOfBounds(BattleUnitState unit)
        {
            var halfHeight = ArchetypeFor(unit.TeamId).BodyHeight * 0.5f;
            return unit.Position.y - halfHeight < _settings.FallDeathY;
        }

        private void DefeatFromFall(BattleUnitState unit, UnitMotor motor)
        {
            unit.MoveTo(new Vector2(motor.Position.x, _settings.FallDeathY));
            motor.Teleport(unit.Position);
            ApplyDamage(unit, unit.MaxHealth, "fall.out-of-bounds", BattleTeamId.None, canBeProtected: false);
            UnitDefeated?.Invoke(new UnitDefeatedEvent(unit.UnitId, "fall.out-of-bounds"));
        }

        private void NudgeApartFromOthers(BattleUnitState unit, UnitMotor motor)
        {
            foreach (var team in _state.Teams)
            {
                foreach (var other in team.Units)
                {
                    if (other == unit || other.IsDefeated)
                    {
                        continue;
                    }

                    var deltaX = motor.Position.x - other.Position.x;
                    var verticalOverlap = Mathf.Abs(motor.Position.y - other.Position.y)
                        < ArchetypeFor(other.TeamId).BodyHeight * 0.55f;
                    if (Mathf.Abs(deltaX) >= _settings.OccupiedRadius || !verticalOverlap)
                    {
                        continue;
                    }

                    var pushDirection = deltaX >= 0f ? 1f : -1f;
                    var desiredX = other.Position.x + pushDirection * _settings.OccupiedRadius;
                    if (TryResolveGroundPosition(new Vector2(desiredX, motor.Position.y), unit.TeamId, out var freeSpot)
                        && Mathf.Abs(freeSpot.y - motor.Position.y) <= 0.6f)
                    {
                        motor.Teleport(freeSpot);
                        unit.MoveTo(freeSpot);
                    }

                    return;
                }
            }
        }

        private void ResolveBattleAfterMotion()
        {
            if (Result.IsFinished)
            {
                return;
            }

            Result = BattleResultEvaluator.Evaluate(_state);
            UnitsMoved?.Invoke();
            if (!Result.IsFinished)
            {
                return;
            }

            BattleFinished?.Invoke(Result, BattleFinishReason.UnitMotionSettled);
        }
    }
}
