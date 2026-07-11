using System;
using System.Collections.Generic;
using Game.Gameplay.Battle.Domain;
using Game.Gameplay.Battle.Motion;
using Game.Gameplay.Battle.Projectiles;
using Game.Gameplay.Battle.Terrain;
using Game.Gameplay.Battle.Units;
using Game.Gameplay.Battle.Weapons;
using UnityEngine;

namespace Game.Gameplay.Battle.Simulation
{
    /// <summary>
    /// The battle rules: turn flow, firing, impact resolution, unit motion
    /// integration. No scene, no input, no status text - the scene controller
    /// drives this through commands and reacts to its events.
    /// </summary>
    public sealed class BattleSimulation : IProjectileSpawner
    {
        private const float ProjectileSubStepLength = 0.04f;

        private readonly BattleState _state;
        private readonly TurnService _turnService;
        private readonly BattleTerrain _terrain;
        private readonly IReadOnlyDictionary<string, UnitMotor> _motors;
        private readonly WeaponInventoryRuntime _playerInventory;
        private readonly WeaponInventoryRuntime _opponentInventory;
        private readonly WeaponExecutor _weaponExecutor;
        private readonly Vector2 _projectileGravityPerScale;
        private readonly BattleSimulationSettings _settings;
        private readonly IReadOnlyDictionary<string, WeaponDefinition> _weaponsById;
        private readonly CharacterArchetype _playerArchetype;
        private readonly CharacterArchetype _opponentArchetype;
        private readonly TurnMovementState _playerMovement;
        private readonly List<SimProjectile> _projectiles = new();

        private WeaponDefinition _activeShotWeapon;
        private int _nextProjectileId;
        private float _simulationAccumulator;

        public static BattleSimulation CreatePlayerFirst(
            BattleState state,
            BattleTerrain terrain,
            IReadOnlyDictionary<string, UnitMotor> motors,
            WeaponInventoryRuntime playerInventory,
            WeaponInventoryRuntime opponentInventory,
            WeaponExecutor weaponExecutor,
            Vector2 projectileGravityPerScale,
            BattleSimulationSettings settings,
            IReadOnlyDictionary<string, WeaponDefinition> weaponsById,
            CharacterArchetype playerArchetype,
            CharacterArchetype opponentArchetype)
        {
            var turnService = new TurnService();
            turnService.StartFirstTurn(state, BattleTeamId.Player);
            return new BattleSimulation(
                state,
                turnService,
                terrain,
                motors,
                playerInventory,
                opponentInventory,
                weaponExecutor,
                projectileGravityPerScale,
                settings,
                weaponsById,
                playerArchetype,
                opponentArchetype);
        }

        public BattleSimulation(
            BattleState state,
            TurnService turnService,
            BattleTerrain terrain,
            IReadOnlyDictionary<string, UnitMotor> motors,
            WeaponInventoryRuntime playerInventory,
            WeaponInventoryRuntime opponentInventory,
            WeaponExecutor weaponExecutor,
            Vector2 projectileGravityPerScale,
            BattleSimulationSettings settings,
            IReadOnlyDictionary<string, WeaponDefinition> weaponsById,
            CharacterArchetype playerArchetype,
            CharacterArchetype opponentArchetype)
        {
            _state = state;
            _turnService = turnService;
            _terrain = terrain;
            _motors = motors;
            _playerInventory = playerInventory;
            _opponentInventory = opponentInventory;
            _weaponExecutor = weaponExecutor;
            _projectileGravityPerScale = projectileGravityPerScale;
            _settings = settings;
            _weaponsById = weaponsById;
            _playerArchetype = playerArchetype;
            _opponentArchetype = opponentArchetype;
            _playerMovement = new TurnMovementState(settings.PlayerTurnDurationSeconds);
        }

        public event Action<BattleTeamId, WeaponDefinition, bool> ShotFired;
        public event Action<ImpactResolvedEvent> ImpactResolved;
        public event Action<UnitDefeatedEvent> UnitDefeated;
        public event Action<BattleTeamId, TurnAdvanceReason> TurnAdvanced;
        public event Action<BattleResult, BattleFinishReason> BattleFinished;
        public event Action<CoverDeployedEvent> CoverDeployed;
        public event Action<HealthRestoredEvent> HealthRestored;
        public event Action<string> OwnerRepositioned;
        public event Action<SimProjectile> ProjectileSpawned;
        public event Action<int, ProjectileImpact> ProjectileExploded;
        public event Action UnitsMoved;

        public BattleState State => _state;
        public BattleTerrain Terrain => _terrain;
        public BattleResult Result { get; private set; } = BattleResult.InProgress;
        public IReadOnlyList<SimProjectile> Projectiles => _projectiles;
        public int ProjectilesInFlight => _projectiles.Count;
        public bool HasProjectilesInFlight => _projectiles.Count > 0;
        public float MovementTimeRemaining => _playerMovement.RemainingTurnSeconds;

        public WeaponInventoryRuntime ActiveInventory => GetInventoryFor(_state.Turn.ActiveTeam);

        public bool HasActiveMotion
        {
            get
            {
                foreach (var team in _state.Teams)
                {
                    foreach (var unit in team.Units)
                    {
                        if (unit.IsDefeated)
                        {
                            continue;
                        }

                        if (_motors.TryGetValue(unit.UnitId, out var motor) && !motor.IsStable)
                        {
                            return true;
                        }
                    }
                }

                return false;
            }
        }

        public bool CanUseMovement()
        {
            return !Result.IsFinished
                && !HasProjectilesInFlight
                && !HasActiveMotion
                && _state.Turn.ActiveTeam == BattleTeamId.Player
                && !_state.Turn.ActionSpent
                && _playerMovement.HasTimeRemaining;
        }

        public BattleFireResult TryFire(AimState aim)
        {
            if (Result.IsFinished || HasProjectilesInFlight || HasActiveMotion)
            {
                return new BattleFireResult(BattleFireOutcome.Blocked, BattleTeamId.None, null);
            }

            var activeUnitId = _state.Turn.ActiveUnitId;
            if (string.IsNullOrEmpty(activeUnitId))
            {
                return new BattleFireResult(BattleFireOutcome.NoActiveUnit, BattleTeamId.None, null);
            }

            var unit = _state.GetUnit(activeUnitId);

            if (unit.TeamId == BattleTeamId.Player && !_playerMovement.HasTimeRemaining)
            {
                EndPlayerTurnFromTimer();
                return new BattleFireResult(BattleFireOutcome.TurnTimerExpired, unit.TeamId, null);
            }

            if (!aim.HasInput)
            {
                return new BattleFireResult(BattleFireOutcome.AimRequired, unit.TeamId, null);
            }

            var activeInventory = GetInventoryFor(unit.TeamId);
            var weapon = activeInventory.Selected;

            if (weapon.Role == WeaponRole.ShortRangeBurst)
            {
                return UseCloseRangeBurst(unit, activeInventory, weapon, aim);
            }

            if (IsInstantUtility(weapon.Role))
            {
                return UseInstantUtility(unit, activeInventory, weapon, aim);
            }

            var context = new WeaponFireContext(weapon, unit.UnitId, aim.Origin, aim.Direction, aim.Power, this);
            var fireResult = _weaponExecutor.TryFireSelected(activeInventory, context);
            if (fireResult != WeaponFireResult.Fired)
            {
                var outcome = fireResult == WeaponFireResult.OutOfAmmo ? BattleFireOutcome.OutOfAmmo : BattleFireOutcome.NoWeaponSelected;
                return new BattleFireResult(outcome, unit.TeamId, weapon);
            }

            _activeShotWeapon = weapon;
            _turnService.SpendAction(_state);
            ShotFired?.Invoke(unit.TeamId, weapon, true);
            return new BattleFireResult(BattleFireOutcome.Fired, unit.TeamId, weapon);
        }

        public BattleWalkOutcome Walk(int direction, float deltaTime)
        {
            if (!CanUseMovement() || direction == 0)
            {
                return BattleWalkOutcome.NotAllowed;
            }

            var unit = GetActiveUnitOrNull();
            if (unit == null || unit.TeamId != BattleTeamId.Player)
            {
                return BattleWalkOutcome.NotAllowed;
            }

            var motor = GetMotor(unit.UnitId);
            if (motor == null)
            {
                return BattleWalkOutcome.NotAllowed;
            }

            var outcome = motor.Walk(direction, deltaTime);
            if (outcome == WalkOutcome.None)
            {
                return BattleWalkOutcome.None;
            }

            unit.SetFacing(direction);
            unit.MoveTo(motor.Position);
            UnitsMoved?.Invoke();

            return outcome switch
            {
                WalkOutcome.Moved => BattleWalkOutcome.Moved,
                WalkOutcome.Blocked => BattleWalkOutcome.Blocked,
                WalkOutcome.StartedFalling => BattleWalkOutcome.StartedFalling,
                _ => BattleWalkOutcome.None
            };
        }

        public BattleJumpOutcome TryStartJump(bool forward)
        {
            if (!CanUseMovement())
            {
                return BattleJumpOutcome.NotAllowed;
            }

            var unit = GetActiveUnitOrNull();
            if (unit == null || unit.TeamId != BattleTeamId.Player)
            {
                return BattleJumpOutcome.NotAllowed;
            }

            var motor = GetMotor(unit.UnitId);
            if (motor == null || !motor.IsStable)
            {
                return BattleJumpOutcome.NotStable;
            }

            var archetype = ArchetypeFor(unit.TeamId);
            var direction = unit.FacingDirectionX;
            var horizontalDistance = (forward ? archetype.FrontJumpDistance : -archetype.BackJumpDistance) * direction;
            var apexHeight = (forward ? archetype.FrontJumpHeight : archetype.BackJumpHeight) + 0.2f;
            motor.StartJump(horizontalDistance, apexHeight);
            UnitsMoved?.Invoke();
            return BattleJumpOutcome.Jumped;
        }

        public bool TickPlayerTurnTimer(float deltaTime)
        {
            if (_state.Turn.ActiveTeam != BattleTeamId.Player
                || _state.Turn.ActionSpent
                || HasProjectilesInFlight
                || Result.IsFinished)
            {
                return false;
            }

            _playerMovement.Tick(deltaTime);
            if (_playerMovement.HasTimeRemaining || HasActiveMotion)
            {
                return false;
            }

            EndPlayerTurnFromTimer();
            return true;
        }

        public void AdvanceTurnDueToNoUsableWeapon()
        {
            _turnService.AdvanceTurn(_state);
            ResetMotionForNewTurn();
            TurnAdvanced?.Invoke(_state.Turn.ActiveTeam, TurnAdvanceReason.OpponentSkippedNoUsableWeapon);
            UnitsMoved?.Invoke();
        }

        public void Tick(float deltaTime)
        {
            _simulationAccumulator += Mathf.Min(deltaTime, 0.1f);
            while (_simulationAccumulator >= _settings.SimulationTickSeconds)
            {
                _simulationAccumulator -= _settings.SimulationTickSeconds;
                StepProjectiles(_settings.SimulationTickSeconds);
                StepUnits();
            }

            UnitsMoved?.Invoke();

            if (HasProjectilesInFlight || HasActiveMotion)
            {
                return;
            }

            _simulationAccumulator = 0f;
            ResolveBattleAfterMotion();
        }

        private void ResolveProjectileImpact(ProjectileImpact impact)
        {
            var impactWeapon = ResolveImpactWeapon(impact);
            var terrainChanged = ResolveImpact(impact, impactWeapon);
            ImpactResolved?.Invoke(new ImpactResolvedEvent(impact.Point, impactWeapon, terrainChanged));
            UnitsMoved?.Invoke();

            if (HasProjectilesInFlight)
            {
                return;
            }

            Result = BattleResultEvaluator.Evaluate(_state);
            if (Result.IsFinished)
            {
                UnitDefeated?.Invoke(new UnitDefeatedEvent(string.Empty, "battle-finished"));
                BattleFinished?.Invoke(Result, BattleFinishReason.ProjectileImpactResolved);
                return;
            }

            _turnService.AdvanceTurn(_state);
            ResetMotionForNewTurn();
            TurnAdvanced?.Invoke(_state.Turn.ActiveTeam, TurnAdvanceReason.ProjectileImpactResolved);
            UnitsMoved?.Invoke();
        }

        private BattleUnitState GetActiveUnitOrNull()
        {
            var activeUnitId = _state.Turn.ActiveUnitId;
            return string.IsNullOrEmpty(activeUnitId) ? null : _state.GetUnit(activeUnitId);
        }

        private CharacterArchetype ArchetypeFor(BattleTeamId teamId)
        {
            return teamId == BattleTeamId.Player ? _playerArchetype : _opponentArchetype;
        }

        private UnitMotor GetMotor(string unitId)
        {
            return unitId != null && _motors.TryGetValue(unitId, out var motor) ? motor : null;
        }

        private WeaponInventoryRuntime GetInventoryFor(BattleTeamId teamId)
        {
            return teamId == BattleTeamId.Opponent ? _opponentInventory : _playerInventory;
        }

        private static bool IsInstantUtility(WeaponRole role)
        {
            return role == WeaponRole.CoverTool || role == WeaponRole.DefensiveTool;
        }

        private void ResetMotionForNewTurn()
        {
            foreach (var motor in _motors.Values)
            {
                motor.ResetWalk();
            }

            _playerMovement.Reset(_settings.PlayerTurnDurationSeconds);
        }

        private BattleFireResult UseCloseRangeBurst(BattleUnitState unit, WeaponInventoryRuntime inventory, WeaponDefinition weapon, AimState aim)
        {
            if (!inventory.TryConsumeSelectedAmmo())
            {
                return new BattleFireResult(BattleFireOutcome.OutOfAmmo, unit.TeamId, weapon);
            }

            _activeShotWeapon = weapon;
            _turnService.SpendAction(_state);

            var origin = aim.Origin;
            var direction = aim.Direction.sqrMagnitude > 0.01f ? aim.Direction.normalized : Vector2.right;
            var range = Mathf.Max(1.2f, weapon.ImpactRadius * 1.75f);
            var hitCount = 0;
            var targetTeam = unit.TeamId == BattleTeamId.Player ? BattleTeamId.Opponent : BattleTeamId.Player;

            foreach (var target in _state.GetTeam(targetTeam).Units)
            {
                if (target == null || target.IsDefeated)
                {
                    continue;
                }

                var toUnit = target.Position - origin;
                if (toUnit.magnitude > range || Vector2.Dot(direction, toUnit.normalized) < 0.25f)
                {
                    continue;
                }

                target.ApplyDamage(weapon.Damage, weapon.WeaponId);
                hitCount++;
            }

            var burstPoint = origin + direction * Mathf.Min(range, 0.95f);
            var terrainChanged = ApplyTerrainDestruction(burstPoint, weapon);
            ApplyExplosionKnockback(burstPoint, direction * 8f, weapon, terrainChanged);
            ShotFired?.Invoke(unit.TeamId, weapon, true);
            CompleteInstantAction();
            return new BattleFireResult(BattleFireOutcome.BurstFired, unit.TeamId, weapon, burstPoint, hitCount);
        }

        private BattleFireResult UseInstantUtility(BattleUnitState unit, WeaponInventoryRuntime inventory, WeaponDefinition weapon, AimState aim)
        {
            if (!inventory.TryConsumeSelectedAmmo())
            {
                return new BattleFireResult(BattleFireOutcome.OutOfAmmo, unit.TeamId, weapon);
            }

            _activeShotWeapon = weapon;
            _turnService.SpendAction(_state);

            if (weapon.Role == WeaponRole.CoverTool)
            {
                DeployCover(GetCoverPlacementPoint(aim, weapon), weapon);
            }
            else if (weapon.Role == WeaponRole.DefensiveTool)
            {
                RestoreFriendlyHealth(unit.Position, unit.UnitId, weapon);
            }

            ShotFired?.Invoke(unit.TeamId, weapon, false);
            CompleteInstantAction();
            return new BattleFireResult(BattleFireOutcome.UtilityUsed, unit.TeamId, weapon);
        }

        private static Vector2 GetCoverPlacementPoint(AimState aim, WeaponDefinition weapon)
        {
            var aimSettings = weapon.CreateAimSettings(AimSettings.Default);
            var power01 = Mathf.InverseLerp(aimSettings.MinPower, aimSettings.MaxPower, aim.Power);
            var placementDistance = Mathf.Lerp(1.25f, 3.1f, power01);
            return aim.Origin + aim.Direction.normalized * placementDistance;
        }

        private void CompleteInstantAction()
        {
            UnitsMoved?.Invoke();
            Result = BattleResultEvaluator.Evaluate(_state);
            if (Result.IsFinished)
            {
                BattleFinished?.Invoke(Result, BattleFinishReason.InstantActionCompleted);
                return;
            }

            _turnService.AdvanceTurn(_state);
            ResetMotionForNewTurn();
            TurnAdvanced?.Invoke(_state.Turn.ActiveTeam, TurnAdvanceReason.InstantActionCompleted);
            UnitsMoved?.Invoke();
        }

        private void DeployCover(Vector2 impactPoint, WeaponDefinition impactWeapon)
        {
            var width = Mathf.Clamp(impactWeapon.ImpactRadius * 1.2f, 1.15f, 2.1f);
            const float height = 0.6f;
            var position = impactPoint + Vector2.up * (height * 0.5f);
            _terrain?.StampSolidRect(position, new Vector2(width, height));
            CoverDeployed?.Invoke(new CoverDeployedEvent(position, new Vector2(width, height), impactWeapon));
        }

        private void RestoreFriendlyHealth(Vector2 impactPoint, string ownerUnitId, WeaponDefinition impactWeapon)
        {
            if (string.IsNullOrWhiteSpace(ownerUnitId))
            {
                return;
            }

            var owner = _state.GetUnit(ownerUnitId);
            var restoredHealth = 0;
            foreach (var unit in _state.GetTeam(owner.TeamId).Units)
            {
                if (unit == null || unit.IsDefeated)
                {
                    continue;
                }

                if (Vector2.Distance(unit.Position, impactPoint) > impactWeapon.ImpactRadius)
                {
                    continue;
                }

                restoredHealth += unit.RestoreHealth(impactWeapon.Damage);
            }

            HealthRestored?.Invoke(new HealthRestoredEvent(impactPoint, restoredHealth, owner.TeamId));
        }

        private void RepositionOwner(Vector2 impactPoint, string ownerUnitId, WeaponDefinition impactWeapon)
        {
            if (string.IsNullOrWhiteSpace(ownerUnitId))
            {
                return;
            }

            var owner = _state.GetUnit(ownerUnitId);
            var target = ResolveUnitLandingPosition(impactPoint, owner.TeamId);
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

        private bool ResolveImpact(ProjectileImpact impact, WeaponDefinition impactWeapon)
        {
            if (impactWeapon == null)
            {
                return false;
            }

            switch (impactWeapon.Role)
            {
                case WeaponRole.CoverTool:
                    DeployCover(impact.Point, impactWeapon);
                    return false;
                case WeaponRole.DefensiveTool:
                    RestoreFriendlyHealth(impact.Point, impact.OwnerUnitId, impactWeapon);
                    return false;
                case WeaponRole.MovementTool:
                    RepositionOwner(impact.Point, impact.OwnerUnitId, impactWeapon);
                    return false;
                default:
                    return ResolveDamageImpact(impact, impactWeapon);
            }
        }

        private bool ResolveDamageImpact(ProjectileImpact impact, WeaponDefinition impactWeapon)
        {
            var terrainChanged = ApplyTerrainDestruction(impact.Point, impactWeapon);

            var allUnits = new List<BattleUnitState>();
            foreach (var team in _state.Teams)
            {
                allUnits.AddRange(team.Units);
            }

            var selfDamageMultiplier = impactWeapon.Role == WeaponRole.ShortRangeBurst ? 0f : 0.65f;
            var settings = new RadialDamageSettings(impact.Damage, impact.ImpactRadius, selfDamageMultiplier);
            DamageResolver.ResolveRadialDamage(allUnits, impact.Point, impact.OwnerUnitId, impact.ProjectileId, settings);
            ApplyExplosionKnockback(impact.Point, impact.Velocity, impactWeapon, terrainChanged);
            return terrainChanged;
        }

        private bool ApplyTerrainDestruction(Vector2 impactPoint, WeaponDefinition impactWeapon)
        {
            if (_terrain == null || impactWeapon == null)
            {
                return false;
            }

            var craterRadius = Mathf.Clamp(impactWeapon.ImpactRadius * 0.62f, 0.38f, 1.55f);
            return _terrain.ApplyCrater(impactPoint, craterRadius);
        }

        private void ApplyExplosionKnockback(Vector2 impactPoint, Vector2 impactVelocity, WeaponDefinition impactWeapon, bool terrainChanged)
        {
            if (impactWeapon == null)
            {
                return;
            }

            var radius = Mathf.Max(0.65f, impactWeapon.ImpactRadius + _settings.KnockbackLandingSearchPadding);
            var radiusScale = Mathf.InverseLerp(0.75f, 2.4f, impactWeapon.ImpactRadius);
            var incoming = impactVelocity.sqrMagnitude > 0.01f ? impactVelocity.normalized : Vector2.zero;
            var downwardImpact01 = Mathf.InverseLerp(-0.15f, -0.85f, incoming.y);

            foreach (var team in _state.Teams)
            {
                foreach (var unit in team.Units)
                {
                    if (unit == null || unit.IsDefeated || Mathf.Abs(unit.Position.x - impactPoint.x) > radius)
                    {
                        continue;
                    }

                    var offset = unit.Position - impactPoint;
                    var distance = offset.magnitude;
                    if (distance > radius)
                    {
                        continue;
                    }

                    var falloff = Mathf.Clamp01(1f - (distance / radius));
                    falloff = Mathf.SmoothStep(0f, 1f, falloff);
                    if (falloff <= 0.03f && !terrainChanged)
                    {
                        continue;
                    }

                    var radial = offset.sqrMagnitude > 0.0001f ? offset.normalized : Vector2.up;
                    var side = Mathf.Abs(radial.x) > 0.08f
                        ? Mathf.Sign(radial.x)
                        : (Mathf.Abs(incoming.x) > 0.08f ? Mathf.Sign(incoming.x) : unit.FacingDirectionX);
                    var horizontalSpeed = Mathf.Lerp(1.05f, 3.65f, falloff) * Mathf.Lerp(0.86f, 1.16f, radiusScale);
                    var verticalFromBlast = Mathf.Clamp(radial.y, -0.52f, Mathf.Lerp(0.72f, 0.22f, downwardImpact01));
                    var verticalSpeed = verticalFromBlast
                        * Mathf.Lerp(0.65f, 2.55f, falloff)
                        * Mathf.Lerp(0.88f, 1.08f, radiusScale);
                    if (verticalSpeed > 0f)
                    {
                        verticalSpeed *= Mathf.Lerp(1f, 0.28f, downwardImpact01);
                    }

                    var velocity = new Vector2(side * horizontalSpeed, verticalSpeed);
                    if (incoming.sqrMagnitude > 0.01f)
                    {
                        velocity.x += incoming.x * Mathf.Lerp(0.08f, 0.45f, falloff);
                    }

                    if (velocity.sqrMagnitude < 0.02f)
                    {
                        continue;
                    }

                    GetMotor(unit.UnitId)?.AddImpulse(velocity);
                }
            }

            if (!terrainChanged)
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

        private Vector2 ResolveUnitLandingPosition(Vector2 desiredPoint, BattleTeamId teamId)
        {
            return TryResolveGroundPosition(desiredPoint, teamId, out var landingPosition) ? landingPosition : desiredPoint;
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

        private void EndPlayerTurnFromTimer()
        {
            _turnService.AdvanceTurn(_state);
            ResetMotionForNewTurn();
            TurnAdvanced?.Invoke(_state.Turn.ActiveTeam, TurnAdvanceReason.PlayerTimerExpired);
            UnitsMoved?.Invoke();
        }

        public void Spawn(in ProjectileSpawnRequest request)
        {
            if (request.Definition == null)
            {
                return;
            }

            var projectile = new SimProjectile(
                ++_nextProjectileId,
                request.Definition,
                request.OwnerUnitId,
                request.Position,
                request.Velocity,
                request.Definition.TimeoutSeconds);
            _projectiles.Add(projectile);
            ProjectileSpawned?.Invoke(projectile);
        }

        private void StepProjectiles(float deltaTime)
        {
            for (var i = _projectiles.Count - 1; i >= 0; i--)
            {
                var projectile = _projectiles[i];
                projectile.ReduceLifetime(deltaTime);
                if (projectile.RemainingLifetime <= 0f)
                {
                    ResolveProjectileAt(i, projectile.Position);
                    continue;
                }

                var velocity = projectile.Velocity + _projectileGravityPerScale * projectile.Definition.GravityScale * deltaTime;
                projectile.SetVelocity(velocity);

                var position = projectile.Position;
                var displacement = velocity * deltaTime;
                var stepCount = Mathf.Max(1, Mathf.CeilToInt(displacement.magnitude / ProjectileSubStepLength));
                var step = displacement / stepCount;

                for (var stepIndex = 0; stepIndex < stepCount; stepIndex++)
                {
                    position += step;

                    if (position.y < _settings.FallDeathY - 2f)
                    {
                        ResolveProjectileAt(i, position);
                        break;
                    }

                    if (_terrain != null && _terrain.Grid.IsSolidWorld(position))
                    {
                        ResolveProjectileAt(i, position);
                        break;
                    }

                    if (TryHitUnit(position, projectile.Definition.Radius, projectile.OwnerUnitId, out var hitPoint))
                    {
                        ResolveProjectileAt(i, hitPoint);
                        break;
                    }

                    if (stepIndex == stepCount - 1)
                    {
                        projectile.MoveTo(position);
                    }
                }
            }
        }

        private void ResolveProjectileAt(int projectileIndex, Vector2 point)
        {
            var projectile = _projectiles[projectileIndex];
            _projectiles.RemoveAt(projectileIndex);

            var impact = new ProjectileImpact(
                projectile.Definition.ProjectileId,
                projectile.OwnerUnitId,
                point,
                projectile.Velocity,
                projectile.Definition.Damage,
                projectile.Definition.ImpactRadius);

            ProjectileExploded?.Invoke(projectile.Id, impact);
            ResolveProjectileImpact(impact);
        }

        private bool TryHitUnit(Vector2 point, float radius, string ignoredUnitId, out Vector2 hitPoint)
        {
            hitPoint = point;
            foreach (var team in _state.Teams)
            {
                foreach (var unit in team.Units)
                {
                    if (unit.IsDefeated || unit.UnitId == ignoredUnitId)
                    {
                        continue;
                    }

                    var halfHeight = ArchetypeFor(unit.TeamId).BodyHeight * 0.5f;
                    if (Mathf.Abs(point.x - unit.Position.x) <= _settings.UnitCollisionHalfWidth + radius
                        && Mathf.Abs(point.y - unit.Position.y) <= halfHeight + radius)
                    {
                        hitPoint = unit.Position + (point - unit.Position) * 0.5f;
                        return true;
                    }
                }
            }

            return false;
        }

        private void StepUnits()
        {
            foreach (var team in _state.Teams)
            {
                foreach (var unit in team.Units)
                {
                    if (unit.IsDefeated || !_motors.TryGetValue(unit.UnitId, out var motor) || motor.IsStable)
                    {
                        continue;
                    }

                    var outcome = motor.Step(_settings.SimulationTickSeconds);
                    unit.MoveTo(motor.Position);
                    if (outcome == MotorStepOutcome.FellOutOfBounds)
                    {
                        unit.MoveTo(new Vector2(motor.Position.x, _settings.FallDeathY));
                        motor.Teleport(unit.Position);
                        unit.ApplyDamage(unit.MaxHealth, "fall.out-of-bounds");
                        UnitDefeated?.Invoke(new UnitDefeatedEvent(unit.UnitId, "fall.out-of-bounds"));
                    }
                    else if (outcome == MotorStepOutcome.Landed)
                    {
                        NudgeApartFromOthers(unit, motor);
                    }
                }
            }
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

        private WeaponDefinition ResolveImpactWeapon(ProjectileImpact impact)
        {
            var weaponFromProjectile = ResolveWeaponFromProjectileId(impact.ProjectileId);
            return weaponFromProjectile ?? _activeShotWeapon ?? _playerInventory.Selected;
        }

        private WeaponDefinition ResolveWeaponFromProjectileId(string projectileId)
        {
            if (!string.IsNullOrWhiteSpace(projectileId)
                && projectileId.StartsWith("projectile.", StringComparison.Ordinal))
            {
                var weaponId = projectileId.Substring("projectile.".Length);
                if (_weaponsById.TryGetValue(weaponId, out var weapon))
                {
                    return weapon;
                }
            }

            return null;
        }
    }
}
