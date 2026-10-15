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
        /// <summary>
        /// True while a thrown teleport sphere with the purchased early trigger is
        /// still flying for the active unit: a second fire press detonates it.
        /// </summary>
        public bool CanTriggerTeleportEarly()
        {
            if (_teleportSphereProjectileId < 0 || Result.IsFinished)
            {
                return false;
            }

            var sphere = _projectiles.Find(value => value.Id == _teleportSphereProjectileId);
            return sphere != null && sphere.OwnerUnitId == _state.Turn.ActiveUnitId;
        }

        private bool TryTriggerTeleportSphereEarly(out BattleFireResult result)
        {
            result = default;
            if (!CanTriggerTeleportEarly())
            {
                return false;
            }

            var index = _projectiles.FindIndex(value => value.Id == _teleportSphereProjectileId);
            var sphere = _projectiles[index];
            var weapon = ResolveWeaponFromProjectileId(sphere.Definition.ProjectileId);
            var team = _state.Turn.ActiveTeam;
            _teleportSphereProjectileId = -1;
            // The sphere is consumed where it is this frame; no further ammo is spent.
            ResolveProjectileAt(index, sphere.Position);
            result = new BattleFireResult(BattleFireOutcome.Fired, team, weapon, sphere.Position);
            return true;
        }

        public void SetRopeTraversalInput(int horizontalDirection, int verticalDirection)
        {
            if (_ropeTraversal == null || !_ropeTraversal.IsAttached)
            {
                return;
            }

            // A movement ban keeps the hook but blocks steering on the rope.
            if (IsUnitMovementLocked(_ropeTraversal.OwnerUnitId))
            {
                horizontalDirection = 0;
                verticalDirection = 0;
            }

            _ropeTraversal.SetInput(GetMotor(_ropeTraversal.OwnerUnitId), horizontalDirection, verticalDirection);
        }

        /// <summary>
        /// Owner decision 2026-10-09: the signal rocket is set up at the droid's feet, stands for three seconds so the
        /// droid can step away, then goes straight up at the shared speed.
        /// </summary>
        private AimState CreateSkyHookAim(BattleUnitState unit)
        {
            var ground = ResolveGroundDeployPoint(unit.Position + new Vector2(0f, 0.12f), unit.TeamId);
            // The rocket's centre is half its length above the ground point.
            return new AimState(ground + Vector2.up * 0.4f, Vector2.up, PrototypeWeaponLibrary.SharedProjectileSpeed, true);
        }

        private BattleFireResult StartBoost(
            BattleUnitState unit,
            WeaponInventoryRuntime inventory,
            WeaponDefinition weapon)
        {
            var motor = GetMotor(unit.UnitId);
            if (motor == null || !motor.IsStable)
            {
                return new BattleFireResult(BattleFireOutcome.Blocked, unit.TeamId, weapon);
            }

            if (!inventory.TryConsumeSelectedAmmo())
            {
                return new BattleFireResult(BattleFireOutcome.OutOfAmmo, unit.TeamId, weapon);
            }

            var radiusPurchases = ProgressionFor(unit.TeamId).ResolveUpgradeCount(
                weapon.WeaponId, WeaponUpgradePath.Radius);
            var archetype = ArchetypeFor(unit.TeamId);
            // Up to the top of an ordinary jump, then it hangs there.
            var apexHeight = Mathf.Lerp(archetype.FrontJumpHeight, archetype.BackJumpHeight, 0.5f);
            _boost = new BoostSession(
                unit.UnitId, weapon, WeaponUpgradeChoiceResolver.ResolveBoostSeconds(radiusPurchases));
            motor.StartHover(apexHeight, center => HoverBodyTouchesAnotherDroid(unit, center, motor.Profile.HalfHeight));
            ShotFired?.Invoke(unit.TeamId, weapon, false);
            UnitsMoved?.Invoke();
            return new BattleFireResult(BattleFireOutcome.Fired, unit.TeamId, weapon);
        }

        private bool HoverBodyTouchesAnotherDroid(BattleUnitState self, Vector2 center, float selfHalfHeight)
        {
            foreach (var team in _state.Teams)
            {
                foreach (var other in team.Units)
                {
                    if (other == null || other == self || other.IsDefeated)
                    {
                        continue;
                    }

                    var otherHalfHeight = GetMotor(other.UnitId)?.Profile.HalfHeight ?? selfHalfHeight;
                    if (Mathf.Abs(center.x - other.Position.x) < _settings.OccupiedRadius
                        && Mathf.Abs(center.y - other.Position.y) < selfHalfHeight + otherHalfHeight)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>Steers the hovering droid: horizontal and vertical (up positive) direction.</summary>
        public void SetBoostInput(int horizontalDirection, int verticalDirection)
        {
            if (_boost == null)
            {
                return;
            }

            _boost.HorizontalInput = horizontalDirection;
            _boost.VerticalInput = verticalDirection;
        }

        private void StepBoost(float tickSeconds)
        {
            if (_boost == null)
            {
                return;
            }

            var unit = FindUnit(_boost.OwnerUnitId);
            var motor = GetMotor(_boost.OwnerUnitId);
            if (unit == null || unit.IsDefeated || motor == null || !motor.IsHovering)
            {
                EndBoost();
                return;
            }

            // A frozen or disoriented droid keeps the automatic rise and hover but cannot steer.
            var canSteer = !IsUnitMovementLocked(unit.UnitId);
            motor.SetHoverInput(
                canSteer ? _boost.HorizontalInput : 0,
                canSteer ? _boost.VerticalInput : 0);
            if (canSteer && _boost.HorizontalInput != 0)
            {
                unit.SetFacing(_boost.HorizontalInput > 0 ? 1 : -1);
            }

            _boost.Remaining -= tickSeconds;
            if (_boost.Remaining <= 0f)
            {
                EndBoost();
            }
        }

        private void EndBoost()
        {
            if (_boost == null)
            {
                return;
            }

            var motor = GetMotor(_boost.OwnerUnitId);
            _boost = null;
            motor?.StopHover();
            UnitsMoved?.Invoke();
        }

        private BattleFireResult StartElectricHarpoon(
            BattleUnitState unit,
            WeaponInventoryRuntime inventory,
            WeaponDefinition weapon,
            AimState aim)
        {
            if (!inventory.TryConsumeSelectedAmmo())
            {
                return new BattleFireResult(BattleFireOutcome.OutOfAmmo, unit.TeamId, weapon);
            }

            var evolution = ResolveEvolutionFor(unit.TeamId, weapon);
            var radiusPurchases = ProgressionFor(unit.TeamId).ResolveUpgradeCount(weapon.WeaponId, WeaponUpgradePath.Radius);
            var direction = aim.Direction.sqrMagnitude > 0.01f ? aim.Direction.normalized : Vector2.right;
            // The rope length and its growth are those of the rope weapon.
            _harpoonRopeLength = WeaponUpgradeChoiceResolver.ResolveRopeLength(radiusPurchases);
            _harpoonImpactPull = evolution.HarpoonImpactPull;
            _harpoonDoubleGrab = evolution.HarpoonDoubleGrab;
            _harpoonHookFinished = false;
            _harpoonCaptures.Clear();
            _electricHarpoonTraversal = new ElectricHarpoonTraversal(
                unit.UnitId,
                aim.Origin,
                direction,
                _harpoonRopeLength,
                ElectricHarpoonHookSpeed,
                ElectricHarpoonPullSpeed,
                Mathf.Max(1f, Mathf.RoundToInt(weapon.Damage * ElectricHarpoonTickShare)));
            _activeShotWeapon = weapon;
            _activeShotEndsTurn = true;
            _turnService.SpendAction(_state);
            ShotFired?.Invoke(unit.TeamId, weapon, true);
            UnitsMoved?.Invoke();
            return new BattleFireResult(BattleFireOutcome.Fired, unit.TeamId, weapon);
        }

        private void RepositionOwner(
            Vector2 impactPoint,
            string ownerUnitId,
            WeaponDefinition impactWeapon,
            Vector2? sourceOrigin = null)
        {
            if (string.IsNullOrWhiteSpace(ownerUnitId))
            {
                return;
            }

            var owner = _state.GetUnit(ownerUnitId);
            if (owner == null)
            {
                return;
            }

            var evolution = ResolveEvolutionFor(owner.TeamId, impactWeapon);
            if (impactWeapon.WeaponId == TeleportWeaponId
                && TryFindTeleportExchangeTarget(owner, impactPoint, evolution, out var exchangeTarget))
            {
                ExchangeTeleportPositions(owner, exchangeTarget, evolution.TeleportDisorientationTurns);
                return;
            }

            if (impactWeapon.WeaponId == SwapJumpWeaponId
                && TryFindSwapJumpExchangeTarget(owner, impactPoint, out var swapTarget))
            {
                ExchangeTeleportPositions(
                    owner,
                    swapTarget,
                    disorientationTurns: 0,
                    ownerOriginOverride: sourceOrigin,
                    ownerDestinationOverride: ResolveTeleportLanding(impactPoint, owner.TeamId));
                return;
            }

            if (impactWeapon.WeaponId == TeleportWeaponId && evolution.TeleportInstantTouch)
            {
                // The sphere's own point is kept: only a physical overlap of the body with
                // the surface is resolved, by the smallest shift towards free space. A
                // droid with nothing underneath then falls under normal gravity.
                var freeCenter = ResolveFreeBodyCenter(impactPoint, owner.TeamId);
                owner.MoveTo(freeCenter);
                var arrivalMotor = GetMotor(owner.UnitId);
                arrivalMotor?.Teleport(freeCenter);
                arrivalMotor?.WakeIfUnsupported();
                OwnerRepositioned?.Invoke(owner.UnitId);
                return;
            }

            var target = ResolveUnitLandingPosition(impactPoint, owner.TeamId);
            if (impactWeapon.Role == WeaponRole.MovementTool)
            {
                target = ResolveTeleportLanding(impactPoint, owner.TeamId);

                owner.MoveTo(target);
                GetMotor(owner.UnitId)?.Teleport(target);
                OwnerRepositioned?.Invoke(owner.UnitId);
                ApplyTeleportArrivalImpulse(target, owner.UnitId, evolution);
                return;
            }

            var maxDistance = impactWeapon.Role == WeaponRole.MovementTool ? float.PositiveInfinity : impactWeapon.ImpactRadius;
            var deltaX = Mathf.Clamp(target.x - owner.Position.x, -maxDistance, maxDistance);
            if (Mathf.Abs(deltaX) < 0.1f)
            {
                OwnerRepositioned?.Invoke(string.Empty);
                return;
            }

            owner.MoveTo(ResolveUnitLandingPosition(new Vector2(owner.Position.x + deltaX, target.y), owner.TeamId));
            GetMotor(owner.UnitId)?.Teleport(owner.Position);
            OwnerRepositioned?.Invoke(owner.UnitId);
        }

        private Vector2 ResolveTeleportLanding(Vector2 impactPoint, BattleTeamId teamId)
        {
            var target = ResolveUnitLandingPosition(impactPoint, teamId);
            var bodyHalf = ArchetypeFor(teamId).BodyHeight * 0.5f;
            var desiredCenter = impactPoint + Vector2.up * bodyHalf;
            if (target.y > desiredCenter.y + 0.35f)
            {
                // The arrival displaces terrain when a surface snap would put the unit above the impact.
                CarveArrivalPocket(desiredCenter, bodyHalf, teamId);
                target = desiredCenter;
            }

            return target;
        }

        private bool TryFindTeleportExchangeTarget(
            BattleUnitState owner,
            Vector2 impactPoint,
            WeaponEvolutionProfile evolution,
            out TeleportExchangeTarget target)
        {
            target = null;
            if (!evolution.TeleportExchangesEnemy)
            {
                return false;
            }

            var bestDistanceSquared = TeleportExchangeTargetRadius * TeleportExchangeTargetRadius;
            foreach (var team in _state.Teams)
            {
                foreach (var unit in team.Units)
                {
                    if (unit == null || unit.IsDefeated || unit.UnitId == owner.UnitId)
                    {
                        continue;
                    }

                    if (unit.TeamId == owner.TeamId && !evolution.TeleportExchangesAlliesAndObjects)
                    {
                        continue;
                    }

                    TrySelectTeleportExchangeTarget(
                        new TeleportExchangeTarget(unit),
                        impactPoint,
                        ref target,
                        ref bestDistanceSquared);
                }
            }

            if (!evolution.TeleportExchangesAlliesAndObjects)
            {
                return target != null;
            }

            foreach (var mine in _activeMines)
            {
                if (!mine.Triggered)
                {
                    TrySelectTeleportExchangeTarget(
                        new TeleportExchangeTarget(mine),
                        impactPoint,
                        ref target,
                        ref bestDistanceSquared);
                }
            }

            foreach (var charge in _activeCharges)
            {
                TrySelectTeleportExchangeTarget(
                    new TeleportExchangeTarget(charge),
                    impactPoint,
                    ref target,
                    ref bestDistanceSquared);
            }

            return target != null;
        }

        private static void TrySelectTeleportExchangeTarget(
            TeleportExchangeTarget candidate,
            Vector2 impactPoint,
            ref TeleportExchangeTarget selected,
            ref float bestDistanceSquared)
        {
            var distanceSquared = (candidate.Position - impactPoint).sqrMagnitude;
            if (distanceSquared > bestDistanceSquared)
            {
                return;
            }

            selected = candidate;
            bestDistanceSquared = distanceSquared;
        }

        private bool TryFindSwapJumpExchangeTarget(
            BattleUnitState owner,
            Vector2 impactPoint,
            out TeleportExchangeTarget target)
        {
            target = null;
            // The shared "nearby" distance N0; every Radius purchase adds 10% of the base.
            var radiusPurchases = ProgressionFor(owner.TeamId).ResolveUpgradeCount(
                SwapJumpWeaponId, WeaponUpgradePath.Radius);
            var nearbyRadius = PrototypeWeaponLibrary.SharedNearbyRadius * (1f + 0.1f * radiusPurchases);
            var bestDistanceSquared = nearbyRadius * nearbyRadius;
            var bestUnitId = string.Empty;
            foreach (var team in _state.Teams)
            {
                if (team.TeamId == owner.TeamId)
                {
                    continue;
                }
                foreach (var unit in team.Units)
                {
                    if (unit == null || unit.IsDefeated)
                    {
                        continue;
                    }

                    var distanceSquared = (unit.Position - impactPoint).sqrMagnitude;
                    if (distanceSquared > nearbyRadius * nearbyRadius)
                    {
                        continue;
                    }

                    var isCloser = distanceSquared < bestDistanceSquared - 0.0001f;
                    var isStableTie = Mathf.Abs(distanceSquared - bestDistanceSquared) <= 0.0001f
                        && (string.IsNullOrEmpty(bestUnitId)
                            || string.CompareOrdinal(unit.UnitId, bestUnitId) < 0);
                    if (!isCloser && !isStableTie)
                    {
                        continue;
                    }

                    target = new TeleportExchangeTarget(unit);
                    bestDistanceSquared = distanceSquared;
                    bestUnitId = unit.UnitId;
                }
            }

            return target != null;
        }

        private void ExchangeTeleportPositions(
            BattleUnitState owner,
            TeleportExchangeTarget target,
            int disorientationTurns,
            Vector2? ownerOriginOverride = null,
            Vector2? ownerDestinationOverride = null)
        {
            var ownerPosition = ownerOriginOverride ?? owner.Position;
            owner.MoveTo(ownerDestinationOverride ?? target.Position);
            GetMotor(owner.UnitId)?.Teleport(owner.Position);
            MoveTeleportExchangeTarget(target, ownerPosition);

            ApplyDisorientation(owner.UnitId, disorientationTurns);
            if (target.Unit != null)
            {
                ApplyDisorientation(target.Unit.UnitId, disorientationTurns);
            }

            OwnerRepositioned?.Invoke(owner.UnitId);
            UnitsMoved?.Invoke();
        }

        private void MoveTeleportExchangeTarget(TeleportExchangeTarget target, Vector2 destination)
        {
            if (target.Unit != null)
            {
                target.Unit.MoveTo(destination);
                GetMotor(target.Unit.UnitId)?.Teleport(target.Unit.Position);
                return;
            }

            if (target.Mine != null)
            {
                target.Mine.MoveTo(destination);
                GroundObjectRepositioned?.Invoke(new GroundObjectRepositionedEvent(target.Mine.Id, destination));
                return;
            }

            if (target.Charge != null)
            {
                target.Charge.MoveTo(destination);
                GroundObjectRepositioned?.Invoke(new GroundObjectRepositionedEvent(target.Charge.Id, destination));
            }
        }

        private void ApplyTeleportArrivalImpulse(
            Vector2 center,
            string ownerUnitId,
            WeaponEvolutionProfile evolution)
        {
            if (evolution.TeleportImpulseRadius <= 0f || evolution.TeleportImpulseStrength <= 0f)
            {
                return;
            }

            var radius = evolution.TeleportImpulseRadius;
            var radiusSquared = radius * radius;
            foreach (var team in _state.Teams)
            {
                foreach (var unit in team.Units)
                {
                    if (unit == null || unit.IsDefeated || unit.UnitId == ownerUnitId)
                    {
                        continue;
                    }

                    var offset = unit.Position - center;
                    var distanceSquared = offset.sqrMagnitude;
                    if (distanceSquared > radiusSquared || distanceSquared < TeleportImpulseMinimumDistance * TeleportImpulseMinimumDistance)
                    {
                        continue;
                    }

                    var falloff = 1f - Mathf.Sqrt(distanceSquared) / radius;
                    GetMotor(unit.UnitId)?.AddImpulse(offset.normalized
                        * (evolution.TeleportImpulseStrength * Mathf.SmoothStep(0f, 1f, falloff)));
                }
            }

            foreach (var projectile in _projectiles)
            {
                if (projectile.IsRestingFuse)
                {
                    continue;
                }

                var offset = projectile.Position - center;
                var distanceSquared = offset.sqrMagnitude;
                if (distanceSquared > radiusSquared || distanceSquared < TeleportImpulseMinimumDistance * TeleportImpulseMinimumDistance)
                {
                    continue;
                }

                var falloff = 1f - Mathf.Sqrt(distanceSquared) / radius;
                var impulse = offset.normalized
                    * (evolution.TeleportImpulseStrength * Mathf.SmoothStep(0f, 1f, falloff));
                if (projectile.Id == _controlledProjectileId)
                {
                    projectile.AddExternalVelocity(impulse);
                }
                else
                {
                    projectile.SetVelocity(projectile.Velocity + impulse);
                }
            }
        }

        /// <summary>
        /// Blasts a body-sized capsule out of the terrain so a teleporting unit
        /// can materialise inside a spot it would not otherwise fit.
        /// </summary>
        private void CarveArrivalPocket(Vector2 center, float bodyHalfHeight, BattleTeamId teamId)
        {
            if (_terrain == null)
            {
                return;
            }

            var radius = Mathf.Max(0.5f, _settings.UnitCollisionHalfWidth * 1.6f);
            var offset = Vector2.up * Mathf.Max(0.12f, bodyHalfHeight * 0.45f);
            var changed = ApplyTerrainCrater(center - offset, radius, teamId);
            changed |= ApplyTerrainCrater(center + offset, radius, teamId);
            if (!changed)
            {
                return;
            }

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

        private void StepRopeTraversal(float tickSeconds)
        {
            if (_ropeTraversal == null)
            {
                return;
            }

            if (_ropeTraversal.IsAttached)
            {
                if (!_motors.TryGetValue(_ropeTraversal.OwnerUnitId, out var motor) || motor == null)
                {
                    _ropeTraversal = null;
                }

                return;
            }

            var hookStart = _ropeTraversal.HookPosition;
            if (!_ropeTraversal.StepHook(_terrain, tickSeconds))
            {
                _ropeTraversal = null;
                return;
            }

            var ropeOwner = FindUnit(_ropeTraversal.OwnerUnitId);
            if (ropeOwner != null && TryFindGrounderForHook(
                    ropeOwner.TeamId, hookStart, _ropeTraversal.HookPosition, out var ropeGrounder))
            {
                DamageGrounder(ropeGrounder, 1);
                _ropeTraversal = null;
                return;
            }

            if (_ropeTraversal.IsAttached)
            {
                _ropeTraversal.Attach(GetMotor(_ropeTraversal.OwnerUnitId));
            }
        }

        /// <summary>
        /// Flight of the hook, then up to five seconds of ground pulling per captured droid. Hit damage
        /// X is dealt at once, Y every second (with a small hop and the electric overlay); reaching the
        /// shooter inside the window deals Z and throws the droid away only with impact_pull.
        /// </summary>
        private void StepElectricHarpoon(float tickSeconds)
        {
            var traversal = _electricHarpoonTraversal;
            if (traversal == null)
            {
                return;
            }

            var owner = FindUnit(traversal.OwnerUnitId);
            if (owner == null || owner.IsDefeated)
            {
                EndElectricHarpoon(completeAction: false);
                return;
            }

            if (!_harpoonHookFinished)
            {
                StepElectricHarpoonHook(traversal, owner, tickSeconds);
                if (_electricHarpoonTraversal == null)
                {
                    return;
                }
            }

            var anyActive = false;
            foreach (var capture in _harpoonCaptures)
            {
                if (capture.Finished)
                {
                    continue;
                }

                StepHarpoonCapture(capture, owner, tickSeconds);
                anyActive |= !capture.Finished;
            }

            // While the hook is still flying (double grab) it must keep moving: only a finished hook rides the first droid.
            if (_harpoonHookFinished && _harpoonCaptures.Count > 0 && _harpoonCaptures[0].Finished == false)
            {
                var first = FindUnit(_harpoonCaptures[0].UnitId);
                if (first != null)
                {
                    traversal.Attach(first.UnitId, first.Position);
                }
            }

            UnitsMoved?.Invoke();
            if (_harpoonHookFinished && !anyActive)
            {
                EndElectricHarpoon(completeAction: true);
            }
        }

        private void StepElectricHarpoonHook(ElectricHarpoonTraversal traversal, BattleUnitState owner, float tickSeconds)
        {
            var hookStart = traversal.HookPosition;
            if (!traversal.StepHook(tickSeconds))
            {
                // The hook ran out of rope: with captured droids the pulling goes on, otherwise the shot ends.
                _harpoonHookFinished = true;
                if (_harpoonCaptures.Count == 0)
                {
                    EndElectricHarpoon(completeAction: true);
                }

                return;
            }

            if (TryFindGrounderForHook(owner.TeamId, hookStart, traversal.HookPosition, out var harpoonGrounder))
            {
                DamageGrounder(harpoonGrounder, 1);
                EndElectricHarpoon(completeAction: true);
                return;
            }

            var capacity = _harpoonDoubleGrab ? 2 : 1;
            foreach (var team in _state.Teams)
            {
                if (team.TeamId == owner.TeamId)
                {
                    continue;
                }

                foreach (var target in team.Units)
                {
                    if (_harpoonCaptures.Count >= capacity)
                    {
                        break;
                    }

                    if (target == null || target.IsDefeated
                        || _harpoonCaptures.Exists(capture => capture.UnitId == target.UnitId)
                        || !IsTouchingUnitHitbox(traversal.HookPosition, ElectricHarpoonTargetRadius, target))
                    {
                        continue;
                    }

                    var hit = Mathf.Max(1, Mathf.RoundToInt(
                        _weaponsById[ElectricHarpoonWeaponId].Damage * ElectricHarpoonHitShare));
                    ApplyDamage(target, ResolveOutgoingDamage(owner.UnitId, hit), ElectricHarpoonWeaponId, owner.TeamId);
                    ApplyElectrifiedOverlay(target.UnitId);
                    _harpoonCaptures.Add(new HarpoonCapture(target.UnitId, ElectricHarpoonPullWindowSeconds));
                }
            }

            if (_harpoonCaptures.Count >= capacity)
            {
                _harpoonHookFinished = true;
                if (_harpoonCaptures.Count > 0)
                {
                    var first = FindUnit(_harpoonCaptures[0].UnitId);
                    if (first != null)
                    {
                        traversal.Attach(first.UnitId, first.Position);
                    }
                }
            }
        }

        private void StepHarpoonCapture(HarpoonCapture capture, BattleUnitState owner, float tickSeconds)
        {
            var target = FindUnit(capture.UnitId);
            if (target == null || target.IsDefeated)
            {
                capture.Finished = true;
                return;
            }

            var weapon = _weaponsById[ElectricHarpoonWeaponId];
            capture.TimeRemaining -= tickSeconds;
            ApplyElectrifiedOverlay(target.UnitId);

            var motor = GetMotor(target.UnitId);
            var delta = owner.Position - target.Position;
            if (Mathf.Abs(delta.x) <= ElectricHarpoonContactX && Mathf.Abs(delta.y) <= ElectricHarpoonContactY)
            {
                // Reached the shooter inside the window: the final blow only exists with impact_pull.
                if (_harpoonImpactPull)
                {
                    var finalDamage = Mathf.Max(1, Mathf.RoundToInt(weapon.Damage * ElectricHarpoonFinalShare));
                    ApplyDamage(target, ResolveOutgoingDamage(owner.UnitId, finalDamage), ElectricHarpoonWeaponId, owner.TeamId);
                    if (!target.IsDefeated && motor != null)
                    {
                        var away = Mathf.Abs(delta.x) > 0.05f ? -Mathf.Sign(delta.x) : -owner.FacingDirectionX;
                        motor.AddImpulse(ComputeHarpoonFinalThrowImpulse(motor, away, _harpoonRopeLength));
                    }
                }

                capture.Finished = true;
                return;
            }

            if (motor != null && motor.IsStable)
            {
                var direction = delta.x >= 0f ? 1 : -1;
                var walkScale = ElectricHarpoonPullSpeed / Mathf.Max(0.1f, motor.WalkSpeed);
                motor.Walk(direction, tickSeconds * walkScale);
                target.MoveTo(motor.Position);
            }

            capture.TickAccumulator += tickSeconds;
            while (capture.TickAccumulator >= ElectricHarpoonDamageTickSeconds)
            {
                capture.TickAccumulator -= ElectricHarpoonDamageTickSeconds;
                var tick = Mathf.Max(1, Mathf.RoundToInt(weapon.Damage * ElectricHarpoonTickShare));
                ApplyDamage(target, ResolveOutgoingDamage(owner.UnitId, tick), ElectricHarpoonWeaponId, owner.TeamId);
                if (!target.IsDefeated && motor != null)
                {
                    // A small hop that can carry the droid over an obstacle.
                    var toward = delta.x >= 0f ? 1f : -1f;
                    motor.AddImpulse(new Vector2(toward * 0.9f, ElectricHarpoonHopSpeed));
                }
            }

            if (capture.TimeRemaining <= 0f)
            {
                // Not pulled in five seconds: the rope simply lets go, no final action.
                capture.Finished = true;
            }
        }

        private void EndElectricHarpoon(bool completeAction)
        {
            _electricHarpoonTraversal = null;
            _harpoonCaptures.Clear();
            _harpoonHookFinished = false;
            if (completeAction)
            {
                CompleteInstantAction();
            }
        }

        private sealed class HarpoonCapture
        {
            public HarpoonCapture(string unitId, float window)
            {
                UnitId = unitId;
                TimeRemaining = window;
            }

            public string UnitId { get; }
            public float TimeRemaining { get; set; }
            public float TickAccumulator { get; set; }
            public bool Finished { get; set; }
        }

        /// <summary>
        /// Vertical flight of the signal rocket: it erases ground without limit, picks up the first hostile
        /// droid its body touches and carries it at its own speed, and explodes slightly below the top of the
        /// arena. A carried droid that meets ground or a ceiling lets go and falls.
        /// </summary>
        private void StepSkyHook(int projectileIndex, SimProjectile projectile, float deltaTime)
        {
            if (projectile.IsSkyHookArming)
            {
                // It stands where it was set up; the owner may still move away.
                projectile.ReduceSkyHookArming(deltaTime);
                return;
            }

            projectile.ReduceLifetime(deltaTime);
            var weapon = ResolveWeaponFromProjectileId(projectile.Definition.ProjectileId);
            var sourceTeam = ResolveTeamForOwner(projectile.OwnerUnitId);
            var radius = Mathf.Max(0.05f, projectile.Definition.Radius);
            var apexY = _terrain != null ? _terrain.Grid.WorldMaxY - SkyHookApexMargin : projectile.Position.y + 12f;
            var velocity = projectile.Velocity;
            if (TryFindGrounderForProjectile(projectile, weapon, projectile.Position, out var attractor))
            {
                var toGrounder = attractor.Position - projectile.Position;
                if (toGrounder.sqrMagnitude > 0.0001f)
                {
                    // At twice the shared speed the rocket crosses the magnet's range in a few ticks, so a
                    // gradual pull cannot reach it: inside the range the magnet takes the heading over.
                    velocity = toGrounder.normalized * Mathf.Max(5f, velocity.magnitude);
                    projectile.SetVelocity(velocity);
                }
            }

            var position = projectile.Position;
            var displacement = velocity * deltaTime;
            var stepCount = Mathf.Max(1, Mathf.CeilToInt(displacement.magnitude / ProjectileSubStepLength));
            var step = displacement / stepCount;
            var carved = false;
            for (var stepIndex = 0; stepIndex < stepCount; stepIndex++)
            {
                position += step;
                if (TryFindGrounderForProjectile(projectile, weapon, position, out var grounder)
                    && Vector2.Distance(position, grounder.Position) <= Mathf.Max(0.35f, radius + 0.18f))
                {
                    // Intercepted: the carried droid is let go and simply falls; the rocket blasts at the magnet.
                    ReleaseSkyHookCarried(projectile, throwAway: false, position);
                    projectile.MoveTo(position);
                    ResolveGrounderProjectileContact(projectileIndex, projectile, weapon, grounder, position);
                    return;
                }

                var heading = velocity.sqrMagnitude > 0.0001f ? velocity.normalized : Vector2.up;
                var insideGround = _terrain != null
                    && (_terrain.Grid.IsSolidWorld(position)
                        || _terrain.Grid.IsSolidWorld(position + heading * (SkyHookTunnelRadius * 1.3f)));
                if (insideGround && projectile.ShouldCarveTunnelAt(position, DrillCarveSpacing))
                {
                    carved |= ApplyTerrainCrater(position, SkyHookTunnelRadius, sourceTeam, TerrainMaterial.Titanium);
                }

                if (string.IsNullOrEmpty(projectile.SkyHookCarriedUnitId))
                {
                    foreach (var team in _state.Teams)
                    {
                        if (team.TeamId == sourceTeam)
                        {
                            continue;
                        }

                        foreach (var unit in team.Units)
                        {
                            if (unit == null || unit.IsDefeated || !string.IsNullOrEmpty(projectile.SkyHookCarriedUnitId)
                                || !IsSkyHookBodyTouching(position, unit))
                            {
                                continue;
                            }

                            projectile.SetSkyHookCarried(unit.UnitId);
                        }
                    }
                }

                if (!string.IsNullOrEmpty(projectile.SkyHookCarriedUnitId))
                {
                    CarrySkyHookTarget(projectile, step);
                }

                if (position.y >= apexY)
                {
                    position.y = apexY;
                    projectile.MoveTo(position);
                    ReleaseSkyHookCarried(projectile, throwAway: true, position);
                    ResolveProjectileAt(projectileIndex, position);
                    return;
                }
            }

            projectile.MoveTo(position);
            if (carved)
            {
                WakeUnitsWithoutSupport();
            }

            if (projectile.RemainingLifetime <= 0f)
            {
                ReleaseSkyHookCarried(projectile, throwAway: true, position);
                ResolveProjectileAt(projectileIndex, position);
            }
        }

        /// <summary>The rocket's whole body, not just its tip, picks up a droid it touches.</summary>
        private bool IsSkyHookBodyTouching(Vector2 rocketCenter, BattleUnitState unit)
        {
            var halfHeight = ArchetypeFor(unit.TeamId).BodyHeight * 0.5f;
            return Mathf.Abs(rocketCenter.x - unit.Position.x) <= _settings.UnitCollisionHalfWidth + SkyHookBodyHalfWidth
                && Mathf.Abs(rocketCenter.y - unit.Position.y) <= halfHeight + SkyHookBodyHalfLength;
        }

        private void CarrySkyHookTarget(SimProjectile projectile, Vector2 step)
        {
            var unit = FindUnit(projectile.SkyHookCarriedUnitId);
            var motor = unit != null ? GetMotor(unit.UnitId) : null;
            if (unit == null || unit.IsDefeated || motor == null)
            {
                projectile.SetSkyHookCarried(string.Empty);
                return;
            }

            var next = unit.Position + step;
            if (BodyOverlapsTerrain(next, motor.Profile.HalfWidth, motor.Profile.HalfHeight))
            {
                // Pressed against ground or a ceiling: the droid comes off and falls on its own.
                ReleaseSkyHookCarried(projectile, throwAway: false, projectile.Position);
                return;
            }

            unit.MoveTo(next);
            motor.Teleport(next);
            UnitsMoved?.Invoke();
        }

        /// <summary>
        /// Lets the carried droid go. At the final point it is thrown sideways with the electric harpoon's
        /// final-throw force; otherwise it just falls.
        /// </summary>
        private void ReleaseSkyHookCarried(SimProjectile projectile, bool throwAway, Vector2 rocketPoint)
        {
            var unit = string.IsNullOrEmpty(projectile.SkyHookCarriedUnitId) ? null : FindUnit(projectile.SkyHookCarriedUnitId);
            projectile.SetSkyHookCarried(string.Empty);
            var motor = unit != null ? GetMotor(unit.UnitId) : null;
            if (unit == null || unit.IsDefeated || motor == null)
            {
                return;
            }

            if (throwAway)
            {
                var side = unit.Position.x - rocketPoint.x;
                var away = Mathf.Abs(side) > 0.05f ? Mathf.Sign(side) : projectile.SkyHookLaunchFacing;
                motor.AddImpulse(ComputeHarpoonFinalThrowImpulse(motor, away, WeaponUpgradeChoiceResolver.RopeBaseLength));
            }
            else
            {
                motor.WakeIfUnsupported();
                motor.AddImpulse(Vector2.zero);
            }

            UnitsMoved?.Invoke();
        }

        /// <summary>The fixed-angle throw of the electric harpoon's final blow, about two rope lengths in free space.</summary>
        private static Vector2 ComputeHarpoonFinalThrowImpulse(UnitMotor motor, float awayDirection, float ropeLength)
        {
            var angle = ElectricHarpoonThrowAngleDegrees * Mathf.Deg2Rad;
            var range = ropeLength * 2f;
            var speed = Mathf.Sqrt(range * motor.Gravity / Mathf.Sin(2f * angle));
            return new Vector2(awayDirection * Mathf.Cos(angle), Mathf.Sin(angle)) * speed;
        }

        private sealed class BoostSession
        {
            public BoostSession(string ownerUnitId, WeaponDefinition weapon, float seconds)
            {
                OwnerUnitId = ownerUnitId;
                Weapon = weapon;
                Remaining = seconds;
            }

            public string OwnerUnitId { get; }
            public WeaponDefinition Weapon { get; }
            public float Remaining { get; set; }
            public int HorizontalInput { get; set; }
            public int VerticalInput { get; set; }
        }

        private sealed class TeleportExchangeTarget
        {
            public TeleportExchangeTarget(BattleUnitState unit)
            {
                Unit = unit;
            }

            public TeleportExchangeTarget(ActiveMine mine)
            {
                Mine = mine;
            }

            public TeleportExchangeTarget(ActiveCharge charge)
            {
                Charge = charge;
            }

            public BattleUnitState Unit { get; }
            public ActiveMine Mine { get; }
            public ActiveCharge Charge { get; }
            public Vector2 Position => Unit != null
                ? Unit.Position
                : Mine != null
                    ? Mine.Position
                    : Charge.Position;
        }
    }
}
