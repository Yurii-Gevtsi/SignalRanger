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
        /// <summary>
        /// Max distance a projectile advances per collision sub-step. Public so
        /// the bot's <c>TrajectoryProbe</c> can mirror this exactly instead of
        /// hardcoding its own copy.
        /// </summary>
        public const float ProjectileSubStepLength = 0.04f;
        /// <summary>
        /// Fraction of walk speed the standard jump's horizontal launch is capped
        /// at. Public so the bot's jump prediction mirrors it from one source.
        /// </summary>
        public const float StandardJumpHorizontalSpeedRatio = 0.9f;
        private const string BlasterWeaponId = "weapon.blaster-caset";
        private const string BurWeaponId = "weapon.bur";
        private const string BurRocketWeaponId = "weapon.bur-rocket";
        private const string C4WeaponId = "weapon.c4";
        private const string FireGunWeaponId = "weapon.fire-gun";
        private const string MineWeaponId = "weapon.mine";
        private const string MissileWeaponId = "weapon.missle";
        private const string TeleportWeaponId = "weapon.teleport";
        private const string GrenadeWeaponId = "weapon.granata";
        private const string FpvWeaponId = "weapon.fpv-drone";
        private const string MineDeployProjectileId = "projectile.weapon.mine.deploy";
        private const int BlasterShotsPerTurn = 3;
        private const float MineThrowHorizontalSpeed = 1.75f;
        private const float MineThrowVerticalSpeed = 2.1f;
        private const float MineActivationDelaySeconds = 3f;
        private const float MineTriggerDelaySeconds = 3f;
        private const float C4FuseSeconds = 3f;
        private const float C4DropDurationSeconds = 0.35f;
        private const float C4DeployDistance = 0.82f;
        private const float DrillDurationSeconds = 3f;
        private const float DrillVisualGrowthSeconds = 1.2f;
        private const float DrillMaxRange = 3.25f;
        private const float DrillTickSeconds = 1f;
        private const float DrillTerrainTickSeconds = 0.18f;
        private const float FireStreamDurationSeconds = 3f;
        private const float FireStreamSpawnIntervalSeconds = 1f / 8f;
        private const float FireStreamRange = 3.15f;
        private const float FireBlobSpeed = 6.4f;
        private const float FireBlobGravityScale = 0.82f;
        private const float FireBlobMaxTravelSeconds = 3f;
        private const float FirePuddleLifetimeSeconds = 3f;
        private const float FirePuddleTickSeconds = 0.25f;
        private const float DrillRocketTerrainDurationSeconds = 3f;
        private const float DrillRocketTerrainSpeedRetain = 1f;
        private const float DrillRocketTunnelRadius = 0.16f;
        private const float BounceSpeedRetain = 0.46f;
        private const int MaxBounces = 5;
        private const float RestingFuseMinSpeed = 0.35f;
        private const float GrenadeRestingFuseSeconds = 3f;

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
        private readonly List<ActiveMine> _activeMines = new();
        private readonly List<ActiveCharge> _activeCharges = new();
        private readonly List<ActiveTimedEffect> _activeTimedEffects = new();
        private readonly List<ActiveFirePuddle> _activeFirePuddles = new();

        private WeaponDefinition _activeShotWeapon;
        private bool _activeShotEndsTurn = true;
        private int _blasterShotsThisTurn;
        private bool _mineDeployedThisTurn;
        private int _nextProjectileId;
        private float _simulationAccumulator;
        private int _walkIntentDirection;
        private int _controlledProjectileId = -1;
        private Vector2 _controlledProjectileInput;

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
        public event Action<MineDeployedEvent> MineDeployed;
        public event Action<int> MineTriggered;
        public event Action<ChargeDeployedEvent> ChargeDeployed;
        public event Action<LinearWeaponEffectEvent> LinearWeaponEffectStarted;
        public event Action<FireBlobEvent> FireBlobSpawned;
        public event Action UnitsMoved;

        public BattleState State => _state;
        public BattleTerrain Terrain => _terrain;
        public BattleResult Result { get; private set; } = BattleResult.InProgress;
        public IReadOnlyList<SimProjectile> Projectiles => _projectiles;
        public int ProjectilesInFlight => _projectiles.Count;
        public bool HasProjectilesInFlight => _projectiles.Count > 0;
        public bool HasActiveWeaponEffect => _activeTimedEffects.Count > 0;
        public bool HasPendingGroundAction => _activeCharges.Count > 0;
        public bool HasActiveControlledProjectile => _controlledProjectileId >= 0;
        public bool CanContinueBlasterBurst => _state.Turn.ActiveTeam == BattleTeamId.Player
            && _blasterShotsThisTurn > 0
            && _blasterShotsThisTurn < BlasterShotsPerTurn
            && _playerInventory.Selected?.WeaponId == BlasterWeaponId
            && _playerInventory.CanFireSelected;
        public float MovementTimeRemaining => _playerMovement.RemainingTurnSeconds;

        public WeaponInventoryRuntime ActiveInventory => GetInventoryFor(_state.Turn.ActiveTeam);

        public static bool RequiresAim(WeaponDefinition weapon)
        {
            if (weapon == null || weapon.Role == WeaponRole.DefensiveTool)
            {
                return false;
            }

            return weapon.WeaponId != MineWeaponId
                && weapon.WeaponId != C4WeaponId
                && weapon.WeaponId != FpvWeaponId;
        }

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
            return CanAcceptMovementInput() && !HasActiveMotion;
        }

        /// <summary>
        /// Whether player movement input may stay active. Unlike
        /// <see cref="CanUseMovement"/>, this remains true while the unit is in
        /// player-controlled motion so a held movement button is not released mid-jump or
        /// while dropping from a ledge.
        /// </summary>
        public bool CanAcceptMovementInput()
        {
            if (Result.IsFinished || HasProjectilesInFlight || HasActiveWeaponEffect || _state.Turn.ActionSpent)
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

        public BattleFireResult TryFire(AimState aim)
        {
            if (Result.IsFinished || HasProjectilesInFlight || HasActiveMotion || HasActiveWeaponEffect
                || HasPendingGroundAction || _state.Turn.ActionSpent)
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

            var activeInventory = GetInventoryFor(unit.TeamId);
            var weapon = activeInventory.Selected;
            if (weapon == null)
            {
                return new BattleFireResult(BattleFireOutcome.NoWeaponSelected, unit.TeamId, null);
            }

            if (!aim.HasInput && RequiresAim(weapon))
            {
                return new BattleFireResult(BattleFireOutcome.AimRequired, unit.TeamId, weapon);
            }

            var effectiveAim = aim.HasInput ? aim : CreateImmediateActionAim(unit);

            if (weapon.WeaponId == BlasterWeaponId)
            {
                return UseBlasterShot(unit, activeInventory, weapon, effectiveAim);
            }

            if (weapon.WeaponId == MineWeaponId)
            {
                return DeployMine(unit, activeInventory, weapon);
            }

            if (weapon.WeaponId == C4WeaponId)
            {
                return DeployC4(unit, activeInventory, weapon);
            }

            if (weapon.WeaponId == FpvWeaponId)
            {
                return LaunchControlledDrone(unit, activeInventory, weapon);
            }

            if (weapon.WeaponId == BurWeaponId)
            {
                return StartDrillLaser(unit, activeInventory, weapon, effectiveAim);
            }

            if (weapon.WeaponId == FireGunWeaponId)
            {
                return StartFireStream(unit, activeInventory, weapon, effectiveAim);
            }

            if (weapon.Role == WeaponRole.ShortRangeBurst)
            {
                return UseCloseRangeBurst(unit, activeInventory, weapon, effectiveAim);
            }

            if (IsInstantUtility(weapon.Role))
            {
                return UseInstantUtility(unit, activeInventory, weapon, effectiveAim);
            }

            var context = new WeaponFireContext(
                weapon,
                unit.UnitId,
                effectiveAim.Origin,
                effectiveAim.Direction,
                effectiveAim.Power,
                this);
            var fireResult = _weaponExecutor.TryFireSelected(activeInventory, context);
            if (fireResult != WeaponFireResult.Fired)
            {
                var outcome = fireResult == WeaponFireResult.OutOfAmmo ? BattleFireOutcome.OutOfAmmo : BattleFireOutcome.NoWeaponSelected;
                return new BattleFireResult(outcome, unit.TeamId, weapon);
            }

            _activeShotWeapon = weapon;
            _activeShotEndsTurn = true;
            _turnService.SpendAction(_state);
            ShotFired?.Invoke(unit.TeamId, weapon, true);
            return new BattleFireResult(BattleFireOutcome.Fired, unit.TeamId, weapon);
        }

        public void SetControlledProjectileInput(Vector2 direction)
        {
            _controlledProjectileInput = direction.sqrMagnitude > 0.01f
                ? direction.normalized
                : Vector2.zero;
        }

        private static AimState CreateImmediateActionAim(BattleUnitState unit)
        {
            var direction = unit.FacingDirectionX >= 0 ? Vector2.right : Vector2.left;
            return new AimState(unit.Position + Vector2.up * 0.12f, direction, 0f, true);
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
                StepPlayerTurnTimer(_settings.SimulationTickSeconds);
                StepWalkIntent(_settings.SimulationTickSeconds);
                StepProjectiles(_settings.SimulationTickSeconds);
                StepTimedWeaponEffects(_settings.SimulationTickSeconds);
                StepFirePuddles(_settings.SimulationTickSeconds);
                StepMines(_settings.SimulationTickSeconds);
                StepCharges(_settings.SimulationTickSeconds);
                StepUnits();
            }

            UnitsMoved?.Invoke();

            if (HasProjectilesInFlight || HasActiveMotion || HasActiveWeaponEffect)
            {
                return;
            }

            ResolveBattleAfterMotion();
        }

        private void ResolveProjectileImpact(ProjectileImpact impact)
        {
            var impactWeapon = ResolveImpactWeapon(impact);
            var impactForResolution = impact;
            if (impactWeapon?.WeaponId == "weapon.mortira"
                && impact.ProjectileId == "projectile.weapon.mortira")
            {
                SpawnMortarFragments(impact, impactWeapon);
            }
            else if (IsMortarFragment(impact.ProjectileId)
                && _weaponsById.TryGetValue("weapon.mortira", out var mortar))
            {
                impactWeapon = CreateMortarFragmentWeapon(mortar);
                impactForResolution = new ProjectileImpact(
                    impact.ProjectileId,
                    impact.OwnerUnitId,
                    impact.Point,
                    impact.Velocity,
                    impactWeapon.Damage,
                    impactWeapon.ImpactRadius);
            }

            var terrainChanged = ResolveImpact(impactForResolution, impactWeapon);
            ImpactResolved?.Invoke(new ImpactResolvedEvent(impact.Point, impactWeapon, terrainChanged));
            UnitsMoved?.Invoke();

            if (HasProjectilesInFlight)
            {
                return;
            }

            if (HasActiveWeaponEffect || !_activeShotEndsTurn)
            {
                _activeShotEndsTurn = true;
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

        private void SpawnMortarFragments(ProjectileImpact impact, WeaponDefinition weapon)
        {
            var fragmentWeapon = CreateMortarFragmentWeapon(weapon);
            var fragmentDefinition = ProjectileDefinition.CreateRuntime(
                "projectile.weapon.mortira.fragment",
                fragmentWeapon.ProjectileRadius,
                weapon.GravityScale,
                3f,
                fragmentWeapon.Damage,
                fragmentWeapon.ImpactRadius);
            var launchDirections = new[]
            {
                new Vector2(-0.72f, 1f).normalized,
                Vector2.up,
                new Vector2(0.72f, 1f).normalized
            };

            foreach (var direction in launchDirections)
            {
                Spawn(new ProjectileSpawnRequest(
                    fragmentDefinition,
                    impact.OwnerUnitId,
                    impact.Point + direction * 0.08f,
                    direction * 3.4f,
                    fragmentDefinition.Radius * 2f,
                    ProjectileWeaponEffect.GetProjectileColor(weapon.Role)));
            }
        }

        private static WeaponDefinition CreateMortarFragmentWeapon(WeaponDefinition weapon)
        {
            return WeaponDefinition.CreateRuntime(
                "weapon.mortira.fragment",
                weapon.DisplayKey,
                weapon.Role,
                Mathf.Max(1, Mathf.CeilToInt(weapon.Damage * 0.5f)),
                weapon.ImpactRadius * 0.5f,
                weapon.ProjectileRadius * 0.5f,
                weapon.GravityScale,
                3f,
                weapon.MinPower,
                weapon.MaxPower,
                -1,
                weapon.TutorialHintKey,
                CopyTags(weapon.Tags),
                iconId: weapon.IconId);
        }

        private static string[] CopyTags(IReadOnlyList<string> tags)
        {
            if (tags == null || tags.Count == 0)
            {
                return Array.Empty<string>();
            }

            var result = new string[tags.Count];
            for (var i = 0; i < tags.Count; i++)
            {
                result[i] = tags[i];
            }

            return result;
        }

        private static bool IsMortarFragment(string projectileId)
        {
            return projectileId != null
                && projectileId.StartsWith("projectile.weapon.mortira.fragment", StringComparison.Ordinal);
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
            _walkIntentDirection = 0;
            _controlledProjectileInput = Vector2.zero;
            if (_projectiles.Count == 0)
            {
                _controlledProjectileId = -1;
            }
            _blasterShotsThisTurn = 0;
            _mineDeployedThisTurn = false;
            _activeShotEndsTurn = true;
            foreach (var motor in _motors.Values)
            {
                motor.ResetWalk();
            }

            _playerMovement.Reset(_settings.PlayerTurnDurationSeconds);
        }

        private BattleFireResult UseBlasterShot(BattleUnitState unit, WeaponInventoryRuntime inventory, WeaponDefinition weapon, AimState aim)
        {
            if (_blasterShotsThisTurn >= BlasterShotsPerTurn || !inventory.TryConsumeSelectedAmmo())
            {
                return new BattleFireResult(BattleFireOutcome.OutOfAmmo, unit.TeamId, weapon);
            }

            var direction = aim.Direction.sqrMagnitude > 0.01f ? aim.Direction.normalized : Vector2.right;
            var request = new ProjectileSpawnRequest(
                weapon.CreateProjectileDefinition(),
                unit.UnitId,
                aim.Origin + direction * 0.45f,
                direction * aim.Power,
                weapon.ProjectileRadius * 2f,
                ProjectileWeaponEffect.GetProjectileColor(weapon.Role));

            _activeShotWeapon = weapon;
            _blasterShotsThisTurn++;
            _activeShotEndsTurn = _blasterShotsThisTurn >= BlasterShotsPerTurn;
            if (_activeShotEndsTurn)
            {
                _turnService.SpendAction(_state);
            }

            Spawn(request);
            ShotFired?.Invoke(unit.TeamId, weapon, true);
            return new BattleFireResult(BattleFireOutcome.Fired, unit.TeamId, weapon);
        }

        private BattleFireResult DeployMine(BattleUnitState unit, WeaponInventoryRuntime inventory, WeaponDefinition weapon)
        {
            if (_mineDeployedThisTurn)
            {
                return new BattleFireResult(BattleFireOutcome.OutOfAmmo, unit.TeamId, weapon);
            }

            if (!inventory.TryConsumeSelectedAmmo())
            {
                return new BattleFireResult(BattleFireOutcome.OutOfAmmo, unit.TeamId, weapon);
            }

            var facing = unit.FacingDirectionX >= 0 ? 1f : -1f;
            var origin = unit.Position + new Vector2(facing * 0.28f, 0.46f);
            var definition = ProjectileDefinition.CreateRuntime(
                MineDeployProjectileId,
                weapon.ProjectileRadius,
                weapon.GravityScale,
                weapon.TimeoutSeconds,
                0,
                weapon.ImpactRadius);
            Spawn(new ProjectileSpawnRequest(
                definition,
                unit.UnitId,
                origin,
                new Vector2(facing * MineThrowHorizontalSpeed, MineThrowVerticalSpeed),
                weapon.ProjectileRadius * 2f,
                ProjectileWeaponEffect.GetProjectileColor(weapon.Role)));
            _activeShotWeapon = weapon;
            _activeShotEndsTurn = false;
            _mineDeployedThisTurn = true;
            ShotFired?.Invoke(unit.TeamId, weapon, false);
            return new BattleFireResult(BattleFireOutcome.Fired, unit.TeamId, weapon, origin);
        }

        private BattleFireResult LaunchControlledDrone(
            BattleUnitState unit,
            WeaponInventoryRuntime inventory,
            WeaponDefinition weapon)
        {
            if (!inventory.TryConsumeSelectedAmmo())
            {
                return new BattleFireResult(BattleFireOutcome.OutOfAmmo, unit.TeamId, weapon);
            }

            var spawnPoint = unit.Position + Vector2.up * 1.05f;
            var definition = weapon.CreateProjectileDefinition();
            Spawn(new ProjectileSpawnRequest(
                definition,
                unit.UnitId,
                spawnPoint,
                Vector2.zero,
                weapon.ProjectileRadius * 2f,
                ProjectileWeaponEffect.GetProjectileColor(weapon.Role)));
            _controlledProjectileId = _projectiles[_projectiles.Count - 1].Id;
            _controlledProjectileInput = Vector2.zero;
            _activeShotWeapon = weapon;
            _activeShotEndsTurn = true;
            _turnService.SpendAction(_state);
            ShotFired?.Invoke(unit.TeamId, weapon, true);
            return new BattleFireResult(BattleFireOutcome.Fired, unit.TeamId, weapon, spawnPoint);
        }

        private BattleFireResult DeployC4(BattleUnitState unit, WeaponInventoryRuntime inventory, WeaponDefinition weapon)
        {
            if (!inventory.TryConsumeSelectedAmmo())
            {
                return new BattleFireResult(BattleFireOutcome.OutOfAmmo, unit.TeamId, weapon);
            }

            var facing = unit.FacingDirectionX >= 0 ? 1f : -1f;
            var origin = unit.Position + new Vector2(facing * 0.42f, 0.52f);
            var deployPoint = ResolveGroundDeployPoint(
                unit.Position + new Vector2(facing * C4DeployDistance, 0.08f),
                unit.TeamId);
            var charge = new ActiveCharge(
                ++_nextProjectileId,
                unit.UnitId,
                unit.TeamId,
                weapon,
                deployPoint,
                C4FuseSeconds);
            _activeCharges.Add(charge);
            var dropDistance = Mathf.Max(0f, origin.y - deployPoint.y);
            var dropDuration = Mathf.Clamp(C4DropDurationSeconds + dropDistance * 0.08f, C4DropDurationSeconds, 0.75f);
            ChargeDeployed?.Invoke(new ChargeDeployedEvent(
                charge.Id,
                origin,
                deployPoint,
                dropDuration,
                weapon,
                unit.TeamId));
            ShotFired?.Invoke(unit.TeamId, weapon, false);
            return new BattleFireResult(BattleFireOutcome.UtilityUsed, unit.TeamId, weapon, deployPoint);
        }

        private BattleFireResult StartDrillLaser(BattleUnitState unit, WeaponInventoryRuntime inventory, WeaponDefinition weapon, AimState aim)
        {
            if (!inventory.TryConsumeSelectedAmmo())
            {
                return new BattleFireResult(BattleFireOutcome.OutOfAmmo, unit.TeamId, weapon);
            }

            var direction = aim.Direction.sqrMagnitude > 0.01f ? aim.Direction.normalized : Vector2.right;
            _activeShotWeapon = weapon;
            _activeShotEndsTurn = true;
            _turnService.SpendAction(_state);
            _activeTimedEffects.Add(ActiveTimedEffect.Drill(unit.UnitId, unit.TeamId, weapon, aim.Origin, direction));
            LinearWeaponEffectStarted?.Invoke(new LinearWeaponEffectEvent(
                weapon,
                unit.TeamId,
                aim.Origin,
                direction,
                DrillMaxRange,
                DrillVisualGrowthSeconds,
                DrillDurationSeconds));
            ShotFired?.Invoke(unit.TeamId, weapon, true);
            return new BattleFireResult(BattleFireOutcome.Fired, unit.TeamId, weapon, aim.Origin + direction * DrillMaxRange);
        }

        private BattleFireResult StartFireStream(BattleUnitState unit, WeaponInventoryRuntime inventory, WeaponDefinition weapon, AimState aim)
        {
            if (!inventory.TryConsumeSelectedAmmo())
            {
                return new BattleFireResult(BattleFireOutcome.OutOfAmmo, unit.TeamId, weapon);
            }

            var direction = aim.Direction.sqrMagnitude > 0.01f ? aim.Direction.normalized : Vector2.right;
            _activeShotWeapon = weapon;
            _activeShotEndsTurn = true;
            _turnService.SpendAction(_state);
            _activeTimedEffects.Add(ActiveTimedEffect.FireStream(unit.UnitId, unit.TeamId, weapon, aim.Origin, direction));
            ShotFired?.Invoke(unit.TeamId, weapon, true);
            return new BattleFireResult(BattleFireOutcome.Fired, unit.TeamId, weapon, aim.Origin + direction * FireStreamRange);
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

            if (weapon.Role == WeaponRole.CoverTool)
            {
                _turnService.SpendAction(_state);
                DeployCover(GetCoverPlacementPoint(aim, weapon), weapon);
            }
            else if (weapon.Role == WeaponRole.DefensiveTool)
            {
                RestoreFriendlyHealth(unit.Position, unit.UnitId, weapon);
                ShotFired?.Invoke(unit.TeamId, weapon, false);
                UnitsMoved?.Invoke();
                return new BattleFireResult(BattleFireOutcome.UtilityUsed, unit.TeamId, weapon);
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

        private void StepTimedWeaponEffects(float deltaTime)
        {
            if (Result.IsFinished)
            {
                return;
            }

            for (var i = _activeTimedEffects.Count - 1; i >= 0; i--)
            {
                var effect = _activeTimedEffects[i];
                effect.Elapsed += deltaTime;

                if (effect.Kind == ActiveTimedEffectKind.Drill)
                {
                    effect.TerrainTickAccumulator += deltaTime;
                    while (effect.TerrainTickAccumulator >= DrillTerrainTickSeconds)
                    {
                        effect.TerrainTickAccumulator -= DrillTerrainTickSeconds;
                        var terrainRange01 = Mathf.Clamp01(effect.Elapsed / DrillVisualGrowthSeconds);
                        var terrainRange = Mathf.Lerp(1.05f, DrillMaxRange, terrainRange01);
                        ApplyLinearTerrain(effect, terrainRange, 0.34f);
                    }

                    effect.TickAccumulator += deltaTime;
                    while (effect.TickAccumulator >= DrillTickSeconds)
                    {
                        effect.TickAccumulator -= DrillTickSeconds;
                        var range01 = Mathf.Clamp01(effect.Elapsed / DrillVisualGrowthSeconds);
                        var range = Mathf.Lerp(1.05f, DrillMaxRange, range01);
                        ApplyLinearDamageAndTerrain(effect, range, Mathf.Max(1, Mathf.CeilToInt(effect.Weapon.Damage / 3f)), 0f, false);
                    }

                    if (effect.Elapsed < DrillDurationSeconds)
                    {
                        continue;
                    }

                    _activeTimedEffects.RemoveAt(i);
                    CompleteInstantAction();
                    continue;
                }

                if (effect.Kind == ActiveTimedEffectKind.FireStream)
                {
                    effect.TickAccumulator += deltaTime;
                    while (effect.TickAccumulator >= FireStreamSpawnIntervalSeconds)
                    {
                        effect.TickAccumulator -= FireStreamSpawnIntervalSeconds;
                        SpawnFireBlob(effect);
                    }

                    if (effect.Elapsed < FireStreamDurationSeconds)
                    {
                        continue;
                    }

                    _activeTimedEffects.RemoveAt(i);
                    CompleteInstantAction();
                }
            }
        }

        private void StepFirePuddles(float deltaTime)
        {
            if (Result.IsFinished)
            {
                return;
            }

            for (var i = _activeFirePuddles.Count - 1; i >= 0; i--)
            {
                var puddle = _activeFirePuddles[i];
                puddle.DelayRemaining = Mathf.Max(0f, puddle.DelayRemaining - deltaTime);
                if (puddle.DelayRemaining > 0f)
                {
                    continue;
                }

                puddle.Elapsed += deltaTime;
                puddle.TickAccumulator += deltaTime;
                while (puddle.TickAccumulator >= FirePuddleTickSeconds)
                {
                    puddle.TickAccumulator -= FirePuddleTickSeconds;
                    TryDropFirePuddleToSurface(puddle);
                    puddle.Point += Vector2.down * 0.045f;
                    var terrainChanged = _terrain?.ApplyCrater(puddle.Point + Vector2.down * 0.08f, 0.31f) ?? false;
                    if (terrainChanged)
                    {
                        TryDropFirePuddleToSurface(puddle);
                    }

                    ApplyPointDamage(puddle.Point, puddle.Weapon, puddle.OwnerUnitId, 0.48f, Mathf.Max(1, Mathf.CeilToInt(puddle.Weapon.Damage / 12f)));
                    UnitsMoved?.Invoke();
                }

                if (puddle.Elapsed >= FirePuddleLifetimeSeconds)
                {
                    _activeFirePuddles.RemoveAt(i);
                }
            }
        }

        private void TryDropFirePuddleToSurface(ActiveFirePuddle puddle)
        {
            if (_terrain == null)
            {
                return;
            }

            var probePoint = puddle.Point + Vector2.down * 0.12f;
            if (_terrain.Grid.IsSolidWorld(probePoint))
            {
                return;
            }

            if (_terrain.Grid.TryFindSurfaceBelow(
                    puddle.Point.x,
                    puddle.Point.y - 0.08f,
                    4f,
                    out var surfaceY)
                && puddle.Point.y - surfaceY > 0.12f)
            {
                puddle.Point = new Vector2(puddle.Point.x, surfaceY + 0.08f);
            }
        }

        private void StepMines(float deltaTime)
        {
            if (Result.IsFinished)
            {
                return;
            }

            for (var i = _activeMines.Count - 1; i >= 0; i--)
            {
                var mine = _activeMines[i];
                mine.ArmTimeRemaining = Mathf.Max(0f, mine.ArmTimeRemaining - deltaTime);
                if (mine.ArmTimeRemaining > 0f)
                {
                    continue;
                }

                if (!mine.Triggered && IsAnyUnitInside(mine.Position, mine.Weapon.ImpactRadius))
                {
                    mine.Triggered = true;
                    mine.TriggerTimeRemaining = MineTriggerDelaySeconds;
                    MineTriggered?.Invoke(mine.Id);
                }

                if (!mine.Triggered)
                {
                    continue;
                }

                mine.TriggerTimeRemaining = Mathf.Max(0f, mine.TriggerTimeRemaining - deltaTime);
                if (mine.TriggerTimeRemaining > 0f)
                {
                    continue;
                }

                _activeMines.RemoveAt(i);
                ResolveMineExplosion(mine);
            }
        }

        private void StepCharges(float deltaTime)
        {
            for (var i = _activeCharges.Count - 1; i >= 0; i--)
            {
                var charge = _activeCharges[i];
                charge.FuseRemaining = Mathf.Max(0f, charge.FuseRemaining - deltaTime);
                if (charge.FuseRemaining > 0f)
                {
                    continue;
                }

                _activeCharges.RemoveAt(i);
                var impact = new ProjectileImpact(
                    $"projectile.{charge.Weapon.WeaponId}",
                    charge.OwnerUnitId,
                    charge.Position,
                    Vector2.up,
                    charge.Weapon.Damage,
                    charge.Weapon.ImpactRadius);
                ProjectileExploded?.Invoke(charge.Id, impact);
                var terrainChanged = ResolveDamageImpact(impact, charge.Weapon);
                ImpactResolved?.Invoke(new ImpactResolvedEvent(impact.Point, charge.Weapon, terrainChanged));
                UnitsMoved?.Invoke();

                Result = BattleResultEvaluator.Evaluate(_state);
                if (Result.IsFinished)
                {
                    BattleFinished?.Invoke(Result, BattleFinishReason.InstantActionCompleted);
                    continue;
                }

                if (_state.Turn.ActiveUnitId == charge.OwnerUnitId && !_state.Turn.ActionSpent)
                {
                    CompleteInstantAction();
                }
            }
        }

        private void SpawnFireBlob(ActiveTimedEffect effect)
        {
            var initialVelocity = effect.Direction * FireBlobSpeed;
            var gravity = _projectileGravityPerScale * FireBlobGravityScale;
            TraceFireBlobTrajectory(
                effect.OwnerUnitId,
                effect.Origin,
                initialVelocity,
                gravity,
                out var endpoint,
                out var travelSeconds);
            FireBlobSpawned?.Invoke(new FireBlobEvent(
                effect.Weapon,
                effect.TeamId,
                effect.Origin,
                initialVelocity,
                gravity,
                endpoint,
                travelSeconds,
                FirePuddleLifetimeSeconds));
            _activeFirePuddles.Add(new ActiveFirePuddle(effect.OwnerUnitId, effect.Weapon, endpoint, travelSeconds));
        }

        private void TraceFireBlobTrajectory(
            string ownerUnitId,
            Vector2 origin,
            Vector2 initialVelocity,
            Vector2 gravity,
            out Vector2 impactPoint,
            out float travelSeconds)
        {
            const float traceStepSeconds = 1f / 60f;
            var position = origin;
            var velocity = initialVelocity;
            travelSeconds = 0f;

            while (travelSeconds < FireBlobMaxTravelSeconds)
            {
                velocity += gravity * traceStepSeconds;
                var next = position + velocity * traceStepSeconds;
                travelSeconds += traceStepSeconds;

                if (_terrain != null && _terrain.Grid.Raycast(position, next, out impactPoint))
                {
                    return;
                }

                if (TryHitUnit(next, 0.08f, ownerUnitId, out impactPoint))
                {
                    return;
                }

                position = next;
                if (position.y < _settings.FallDeathY)
                {
                    break;
                }
            }

            impactPoint = position;
        }

        private void ApplyLinearDamageAndTerrain(ActiveTimedEffect effect, float range, int damage, float craterRadius, bool canHitOwner)
        {
            var endpoint = ResolveLineEndpoint(effect.Origin, effect.Direction, range);
            if (_terrain != null && craterRadius > 0f)
            {
                _terrain.ApplyCrater(endpoint, craterRadius);
            }

            foreach (var team in _state.Teams)
            {
                foreach (var unit in team.Units)
                {
                    if (unit == null || unit.IsDefeated || (!canHitOwner && unit.UnitId == effect.OwnerUnitId))
                    {
                        continue;
                    }

                    var distance = DistanceToSegment(unit.Position, effect.Origin, endpoint);
                    if (distance > 0.42f)
                    {
                        continue;
                    }

                    if (damage > 0)
                    {
                        unit.ApplyDamage(damage, effect.Weapon.WeaponId);
                    }
                }
            }

            UnitsMoved?.Invoke();
        }

        private void ApplyLinearTerrain(ActiveTimedEffect effect, float range, float craterRadius)
        {
            if (_terrain == null)
            {
                return;
            }

            var endpoint = ResolveLineEndpoint(effect.Origin, effect.Direction, range);
            _terrain.ApplyCrater(endpoint, craterRadius);
            UnitsMoved?.Invoke();
        }

        private Vector2 ResolveLineEndpoint(Vector2 origin, Vector2 direction, float range)
        {
            var endpoint = origin + direction.normalized * range;
            if (_terrain != null && _terrain.Grid.Raycast(origin, endpoint, out var hitPoint))
            {
                return hitPoint;
            }

            return endpoint;
        }

        private void ApplyPointDamage(Vector2 point, WeaponDefinition weapon, string ownerUnitId, float radius, int damage)
        {
            foreach (var team in _state.Teams)
            {
                foreach (var unit in team.Units)
                {
                    if (unit == null || unit.IsDefeated)
                    {
                        continue;
                    }

                    if (Vector2.Distance(unit.Position, point) > radius)
                    {
                        continue;
                    }

                    unit.ApplyDamage(damage, weapon.WeaponId);
                }
            }

            Result = BattleResultEvaluator.Evaluate(_state);
            if (Result.IsFinished)
            {
                BattleFinished?.Invoke(Result, BattleFinishReason.ProjectileImpactResolved);
            }
        }

        private bool IsAnyUnitInside(Vector2 point, float radius)
        {
            foreach (var team in _state.Teams)
            {
                foreach (var unit in team.Units)
                {
                    if (unit != null && !unit.IsDefeated && Vector2.Distance(unit.Position, point) <= radius)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private void ResolveMineExplosion(ActiveMine mine)
        {
            var impact = new ProjectileImpact(
                $"projectile.{mine.Weapon.WeaponId}",
                mine.OwnerUnitId,
                mine.Position,
                Vector2.up,
                mine.Weapon.Damage,
                mine.Weapon.ImpactRadius);

            ProjectileExploded?.Invoke(mine.Id, impact);
            var terrainChanged = ResolveDamageImpact(impact, mine.Weapon);
            ImpactResolved?.Invoke(new ImpactResolvedEvent(impact.Point, mine.Weapon, terrainChanged));
            UnitsMoved?.Invoke();
            Result = BattleResultEvaluator.Evaluate(_state);
            if (Result.IsFinished)
            {
                BattleFinished?.Invoke(Result, BattleFinishReason.ProjectileImpactResolved);
            }
        }

        private static float DistanceToSegment(Vector2 point, Vector2 segmentStart, Vector2 segmentEnd)
        {
            var segment = segmentEnd - segmentStart;
            var lengthSq = segment.sqrMagnitude;
            if (lengthSq <= 0.0001f)
            {
                return Vector2.Distance(point, segmentStart);
            }

            var t = Mathf.Clamp01(Vector2.Dot(point - segmentStart, segment) / lengthSq);
            return Vector2.Distance(point, segmentStart + segment * t);
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
            if (owner == null)
            {
                return;
            }

            var target = ResolveUnitLandingPosition(impactPoint, owner.TeamId);
            if (impactWeapon.Role == WeaponRole.MovementTool)
            {
                owner.MoveTo(target);
                GetMotor(owner.UnitId)?.Teleport(target);
                OwnerRepositioned?.Invoke(owner.UnitId);
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

        private Vector2 ResolveGroundDeployPoint(Vector2 desiredPoint, BattleTeamId teamId)
        {
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

        private void EndPlayerTurnFromTimer()
        {
            _walkIntentDirection = 0;
            _turnService.AdvanceTurn(_state);
            ResetMotionForNewTurn();
            TurnAdvanced?.Invoke(_state.Turn.ActiveTeam, TurnAdvanceReason.PlayerTimerExpired);
            UnitsMoved?.Invoke();
        }

        private void StepPlayerTurnTimer(float tickSeconds)
        {
            if (_state.Turn.ActiveTeam != BattleTeamId.Player
                || _state.Turn.ActionSpent
                || HasProjectilesInFlight
                || Result.IsFinished)
            {
                return;
            }

            _playerMovement.Tick(tickSeconds);
            if (!_playerMovement.HasTimeRemaining)
            {
                EndPlayerTurnFromTimer();
            }
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

            var outcome = motor.Walk(direction, tickSeconds);
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
            if (ResolveWeaponFromProjectileId(request.Definition.ProjectileId)?.WeaponId == BurRocketWeaponId)
            {
                projectile.ConfigureTerrainDrill(DrillRocketTerrainDurationSeconds);
            }

            _projectiles.Add(projectile);
            ProjectileSpawned?.Invoke(projectile);
        }

        private void StepProjectiles(float deltaTime)
        {
            for (var i = _projectiles.Count - 1; i >= 0; i--)
            {
                var projectile = _projectiles[i];
                var projectileWeapon = ResolveWeaponFromProjectileId(projectile.Definition.ProjectileId);
                var isDrillRocket = projectileWeapon?.WeaponId == BurRocketWeaponId;
                var isControlledDrone = projectile.Id == _controlledProjectileId;

                if (projectile.IsRestingFuse)
                {
                    projectile.ReduceRestingFuse(deltaTime);
                    if (projectile.RestingFuseRemaining <= 0f)
                    {
                        ResolveProjectileAt(i, projectile.Position);
                    }

                    continue;
                }

                projectile.ReduceLifetime(deltaTime);
                if (projectile.RemainingLifetime <= 0f)
                {
                    ResolveProjectileAt(i, projectile.Position);
                    continue;
                }

                var velocity = isControlledDrone
                    ? _controlledProjectileInput * Mathf.Max(0.1f, projectileWeapon?.MaxPower ?? 1f)
                    : projectile.Velocity;
                if (!isControlledDrone && !(isDrillRocket && projectile.IsTraversingTerrain))
                {
                    velocity += _projectileGravityPerScale * projectile.Definition.GravityScale * deltaTime;
                }

                if (!isControlledDrone && !(isDrillRocket && projectile.IsTraversingTerrain))
                {
                    velocity = ApplyProjectileSteering(projectile, velocity, deltaTime);
                }
                if (isDrillRocket
                    && projectile.IsTraversingTerrain
                    && projectile.TerrainDrillSpeed > 0f
                    && projectile.TerrainDrillDirection.sqrMagnitude > 0.0001f)
                {
                    velocity = projectile.TerrainDrillDirection.normalized * projectile.TerrainDrillSpeed;
                }
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

                    if (isDrillRocket
                        && TryHitUnit(position, projectile.Definition.Radius, projectile.OwnerUnitId, out var drillHitPoint))
                    {
                        ResolveProjectileAt(i, drillHitPoint);
                        break;
                    }

                    var touchesTerrain = _terrain != null && _terrain.Grid.IsSolidWorld(position);
                    var traversesDrillableTerrain = isDrillRocket
                        && IsInsideDrillableTerrain(projectile, position, velocity, touchesTerrain);
                    if (traversesDrillableTerrain)
                    {
                        if (!projectile.IsTraversingTerrain)
                        {
                            projectile.EnterTerrain(DrillRocketTerrainSpeedRetain);
                            velocity = projectile.TerrainDrillDirection * projectile.TerrainDrillSpeed;
                            projectile.SetVelocity(velocity);
                            step = velocity * deltaTime / stepCount;
                        }

                        projectile.ConsumeTerrainDrillTime(deltaTime / stepCount);
                        _terrain.ApplyCrater(position, DrillRocketTunnelRadius);
                        projectile.MoveTo(position);
                        if (projectile.TerrainDrillTimeRemaining <= 0f)
                        {
                            ResolveProjectileAt(i, position);
                            break;
                        }

                        continue;
                    }

                    if (touchesTerrain)
                    {
                        if (TryBounceProjectile(projectile, position - step, velocity))
                        {
                            break;
                        }

                        if (projectileWeapon?.WeaponId == GrenadeWeaponId)
                        {
                            projectile.StartRestingFuse(position - step, GrenadeRestingFuseSeconds);
                            break;
                        }

                        if (projectileWeapon?.WeaponId == TeleportWeaponId)
                        {
                            ResolveProjectileAt(i, position - step);
                            break;
                        }

                        ResolveProjectileAt(i, position);
                        break;
                    }

                    if (isDrillRocket && projectile.IsTraversingTerrain)
                    {
                        projectile.ExitTerrain();
                    }

                    var ignoresUnitsInFlight = projectileWeapon?.WeaponId == GrenadeWeaponId
                        || projectileWeapon?.WeaponId == TeleportWeaponId
                        || projectile.Definition.ProjectileId == MineDeployProjectileId;
                    if (!isDrillRocket
                        && !ignoresUnitsInFlight
                        && TryHitUnit(position, projectile.Definition.Radius, projectile.OwnerUnitId, out var hitPoint))
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

        private bool IsInsideDrillableTerrain(
            SimProjectile projectile,
            Vector2 position,
            Vector2 velocity,
            bool centerIsSolid)
        {
            if (_terrain == null || (!centerIsSolid && !projectile.IsTraversingTerrain))
            {
                return false;
            }

            if (centerIsSolid)
            {
                return true;
            }

            var direction = velocity.sqrMagnitude > 0.01f ? velocity.normalized : Vector2.right;
            var normal = new Vector2(-direction.y, direction.x);
            var sampleDistance = DrillRocketTunnelRadius * 1.15f;
            return _terrain.Grid.IsSolidWorld(position + direction * sampleDistance)
                || _terrain.Grid.IsSolidWorld(position + normal * sampleDistance)
                || _terrain.Grid.IsSolidWorld(position - normal * sampleDistance);
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
            if (projectile.Id == _controlledProjectileId)
            {
                _controlledProjectileId = -1;
                _controlledProjectileInput = Vector2.zero;
            }

            if (projectile.Definition.ProjectileId == MineDeployProjectileId)
            {
                LandMine(projectile, point);
                return;
            }

            ResolveProjectileImpact(impact);
        }

        private void LandMine(SimProjectile projectile, Vector2 point)
        {
            var weapon = ResolveWeaponFromProjectileId(projectile.Definition.ProjectileId);
            if (weapon == null)
            {
                return;
            }

            var owner = _state.GetUnit(projectile.OwnerUnitId);
            var teamId = owner?.TeamId ?? BattleTeamId.None;
            var mine = new ActiveMine(projectile.Id, projectile.OwnerUnitId, teamId, weapon, point + Vector2.up * 0.04f);
            _activeMines.Add(mine);
            MineDeployed?.Invoke(new MineDeployedEvent(mine.Id, mine.Position, weapon, teamId));
            ImpactResolved?.Invoke(new ImpactResolvedEvent(point, weapon, false));
            UnitsMoved?.Invoke();
            if (!HasProjectilesInFlight)
            {
                _activeShotEndsTurn = true;
            }
        }

        private Vector2 ApplyProjectileSteering(SimProjectile projectile, Vector2 velocity, float deltaTime)
        {
            var weapon = ResolveWeaponFromProjectileId(projectile.Definition.ProjectileId);
            if (weapon?.WeaponId != MissileWeaponId || velocity.sqrMagnitude < 0.01f)
            {
                return velocity;
            }

            if (!TryFindTargetFor(projectile.OwnerUnitId, out var target))
            {
                return velocity;
            }

            var desired = target.Position - projectile.Position;
            if (desired.sqrMagnitude < 0.01f)
            {
                return velocity;
            }

            var speed = velocity.magnitude;
            var currentAngle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
            var desiredAngle = Mathf.Atan2(desired.y, desired.x) * Mathf.Rad2Deg;
            var steeredAngle = Mathf.MoveTowardsAngle(currentAngle, desiredAngle, 95f * deltaTime);
            var angleRad = steeredAngle * Mathf.Deg2Rad;
            return new Vector2(Mathf.Cos(angleRad), Mathf.Sin(angleRad)) * speed;
        }

        private bool TryBounceProjectile(SimProjectile projectile, Vector2 safePosition, Vector2 velocity)
        {
            var weapon = ResolveWeaponFromProjectileId(projectile.Definition.ProjectileId);
            if (weapon == null || (weapon.WeaponId != TeleportWeaponId && weapon.WeaponId != GrenadeWeaponId))
            {
                return false;
            }

            if (projectile.BounceCount >= MaxBounces || velocity.magnitude < RestingFuseMinSpeed)
            {
                return false;
            }

            var reflected = Mathf.Abs(velocity.y) >= Mathf.Abs(velocity.x)
                ? new Vector2(velocity.x, -velocity.y)
                : new Vector2(-velocity.x, velocity.y);
            projectile.RegisterBounce();
            projectile.MoveTo(safePosition);
            projectile.SetVelocity(reflected * BounceSpeedRetain);
            return true;
        }

        private bool TryFindTargetFor(string ownerUnitId, out BattleUnitState target)
        {
            target = null;
            if (string.IsNullOrWhiteSpace(ownerUnitId))
            {
                return false;
            }

            var owner = _state.GetUnit(ownerUnitId);
            if (owner == null)
            {
                return false;
            }

            var targetTeam = owner.TeamId == BattleTeamId.Player ? BattleTeamId.Opponent : BattleTeamId.Player;
            var bestDistance = float.PositiveInfinity;
            foreach (var unit in _state.GetTeam(targetTeam).Units)
            {
                if (unit == null || unit.IsDefeated)
                {
                    continue;
                }

                var distance = (unit.Position - owner.Position).sqrMagnitude;
                if (distance >= bestDistance)
                {
                    continue;
                }

                bestDistance = distance;
                target = unit;
            }

            return target != null;
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
            unit.ApplyDamage(unit.MaxHealth, "fall.out-of-bounds");
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

        private WeaponDefinition ResolveImpactWeapon(ProjectileImpact impact)
        {
            var weaponFromProjectile = ResolveWeaponFromProjectileId(impact.ProjectileId);
            return weaponFromProjectile ?? _activeShotWeapon ?? _playerInventory.Selected;
        }

        private WeaponDefinition ResolveWeaponFromProjectileId(string projectileId)
        {
            if (projectileId == MineDeployProjectileId)
            {
                return _weaponsById.TryGetValue(MineWeaponId, out var mine) ? mine : null;
            }

            if (projectileId != null
                && projectileId.StartsWith("projectile.weapon.mortira.fragment", StringComparison.Ordinal))
            {
                return _weaponsById.TryGetValue("weapon.mortira", out var mortar) ? mortar : null;
            }

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

        private enum ActiveTimedEffectKind
        {
            Drill,
            FireStream
        }

        private sealed class ActiveTimedEffect
        {
            private ActiveTimedEffect(
                ActiveTimedEffectKind kind,
                string ownerUnitId,
                BattleTeamId teamId,
                WeaponDefinition weapon,
                Vector2 origin,
                Vector2 direction,
                Vector2 targetPoint)
            {
                Kind = kind;
                OwnerUnitId = ownerUnitId;
                TeamId = teamId;
                Weapon = weapon;
                Origin = origin;
                Direction = direction.sqrMagnitude > 0.01f ? direction.normalized : Vector2.right;
                TargetPoint = targetPoint;
            }

            public ActiveTimedEffectKind Kind { get; }
            public string OwnerUnitId { get; }
            public BattleTeamId TeamId { get; }
            public WeaponDefinition Weapon { get; }
            public Vector2 Origin { get; }
            public Vector2 Direction { get; }
            public Vector2 TargetPoint { get; }
            public float Elapsed { get; set; }
            public float TickAccumulator { get; set; }
            public float TerrainTickAccumulator { get; set; }

            public static ActiveTimedEffect Drill(string ownerUnitId, BattleTeamId teamId, WeaponDefinition weapon, Vector2 origin, Vector2 direction)
            {
                return new ActiveTimedEffect(ActiveTimedEffectKind.Drill, ownerUnitId, teamId, weapon, origin, direction, Vector2.zero);
            }

            public static ActiveTimedEffect FireStream(string ownerUnitId, BattleTeamId teamId, WeaponDefinition weapon, Vector2 origin, Vector2 direction)
            {
                return new ActiveTimedEffect(ActiveTimedEffectKind.FireStream, ownerUnitId, teamId, weapon, origin, direction, Vector2.zero);
            }

        }

        private sealed class ActiveMine
        {
            public ActiveMine(int id, string ownerUnitId, BattleTeamId teamId, WeaponDefinition weapon, Vector2 position)
            {
                Id = id;
                OwnerUnitId = ownerUnitId;
                TeamId = teamId;
                Weapon = weapon;
                Position = position;
                ArmTimeRemaining = MineActivationDelaySeconds;
            }

            public int Id { get; }
            public string OwnerUnitId { get; }
            public BattleTeamId TeamId { get; }
            public WeaponDefinition Weapon { get; }
            public Vector2 Position { get; }
            public float ArmTimeRemaining { get; set; }
            public bool Triggered { get; set; }
            public float TriggerTimeRemaining { get; set; }
        }

        private sealed class ActiveCharge
        {
            public ActiveCharge(
                int id,
                string ownerUnitId,
                BattleTeamId teamId,
                WeaponDefinition weapon,
                Vector2 position,
                float fuseSeconds)
            {
                Id = id;
                OwnerUnitId = ownerUnitId;
                TeamId = teamId;
                Weapon = weapon;
                Position = position;
                FuseRemaining = fuseSeconds;
            }

            public int Id { get; }
            public string OwnerUnitId { get; }
            public BattleTeamId TeamId { get; }
            public WeaponDefinition Weapon { get; }
            public Vector2 Position { get; }
            public float FuseRemaining { get; set; }
        }

        private sealed class ActiveFirePuddle
        {
            public ActiveFirePuddle(string ownerUnitId, WeaponDefinition weapon, Vector2 point, float delaySeconds)
            {
                OwnerUnitId = ownerUnitId;
                Weapon = weapon;
                Point = point;
                DelayRemaining = Mathf.Max(0f, delaySeconds);
            }

            public string OwnerUnitId { get; }
            public WeaponDefinition Weapon { get; }
            public Vector2 Point { get; set; }
            public float DelayRemaining { get; set; }
            public float Elapsed { get; set; }
            public float TickAccumulator { get; set; }
        }
    }
}
