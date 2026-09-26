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
    public sealed class BattleSimulation : IProjectileSpawner, IDelayedProjectileSpawner
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
        private const string ToxicWeaponId = "weapon.toxic";
        private const string PoisonDamageSourceId = "status.poison";
        private const string BurnDamageSourceId = "status.burn";
        private const float EmpOverlayDurationSeconds = 0.75f;
        private const int ToxicBaseDamagePerTurn = 6;
        private const int ToxicBaseDurationTurns = 3;
        private const float PoisonMovementDamageTickSeconds = 1f;
        private const string MineWeaponId = "weapon.mine";
        private const string MissileWeaponId = "weapon.missle";
        private const string TeleportWeaponId = "weapon.teleport";
        private const string SwapJumpWeaponId = "weapon.swap-jump";
        private const string GrenadeWeaponId = "weapon.granata";
        private const string FpvWeaponId = "weapon.fpv-drone";
        private const string FreezeWeaponId = "weapon.freeze";
        private const string EmpWeaponId = "weapon.emp";
        private const int CryoBaseFreezeTurns = 1;
        private const int CryoChainWindowTurnSequences = 2;
        private const string MineDeployProjectileId = "projectile.weapon.mine.deploy";
        private const int BaseBlasterShotsPerTurn = 3;
        // Keep the armed mine in the same close placement zone as C4 instead of
        // lobbing it into the next platform.
        private const float MineThrowHorizontalSpeed = 0.95f;
        private const float MineThrowVerticalSpeed = 1.55f;
        // Reference feel: the mine arms shortly after it stops and pops fast
        // once someone steps into its radius - a long trigger delay let units
        // simply walk out of the blast.
        private const float MineActivationDelaySeconds = 2f;
        private const float MineTriggerDelaySeconds = 1f;
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
        private const float FirePuddleTickSeconds = 0.5f;
        private const float FirePuddleDamageTickSeconds = 0.25f;
        private const float LavaOverlayDurationSeconds = 0.45f;
        // A lava bead is roughly 10 px wide. Its contact burn spreads another
        // 8 px into terrain, while heat reaches twice that spread around it.
        private const float FirePuddleTerrainBurnRadius = 0.18f;
        private const float FirePuddleDamageRadius = 0.32f;
        private const float DrillRocketTerrainDurationSeconds = 1f;
        private const float DrillRocketTerrainSpeedRetain = 1f;
        private const float DrillRocketTunnelRadius = 0.24f;
        private const float BounceSpeedRetain = 0.46f;
        private const int MaxBounces = 5;
        private const float RestingFuseMinSpeed = 0.35f;
        private const float MagneticGrenadeSteerAcceleration = 15f;
        private const float GravityGrenadeMinimumDistance = 0.22f;
        private const float GravityGrenadeProjectilePullScale = 0.6f;
        private const float GravityGrenadeExternalVelocityDamping = 5f;
        private const float ProtectiveRepairDamageMultiplier = 0.5f;
        /// <summary>
        /// Movement-only window the player keeps after an attack: walk away from
        /// the blast, but no weapon switching or second shot. Capped by the turn
        /// timer - it never adds time on top of it.
        /// </summary>
        private const float RetreatSeconds = 3f;
        /// <summary>
        /// Minimum travel between tunnel craters. Each crater uploads the
        /// destructible texture to the GPU, so carving every sub-step made the
        /// drill rocket stutter; the tunnel radius comfortably covers this gap.
        /// </summary>
        private const float DrillCarveSpacing = 0.09f;
        private const float FirePuddleDropSearchDistance = 40f;
        private const float MissileHomingDelaySeconds = 0.45f;
        private const float MissileSteerDegreesPerSecond = 210f;
        private const float BallisticMissileHorizontalSteerSpeed = 10f;
        private const float BallisticMissileSplitDistance = 1.65f;
        private const float MissileDetourClearance = 0.6f;
        private const float MissileDetourWaypointArrivalDistance = 0.45f;
        private const float MissileDetourProbeStep = 0.1f;
        private const float MissileClusterSpreadDegrees = 18f;
        private const float MissileClusterSpeed = 5.2f;
        private const string MissileClusterProjectilePrefix = "projectile.weapon.missle.cluster.";
        private const float TeleportExchangeTargetRadius = 2.5f;
        private const float TeleportImpulseMinimumDistance = 0.08f;
        private const float MortarFragmentSpreadDegrees = 45f;
        private const float MortarFragmentSpeed = 3.4f;
        private const float MortarCascadeDelaySeconds = 0.12f;
        private const float MortarCascadeDamageScale = 0.5f;
        private const float MortarCascadeSpeed = 2.2f;
        private const string MortarCascadeProjectilePrefix = "projectile.weapon.mortira.cascade.";
        private const string RopeWeaponId = "weapon.rope";
        private const float RopeBaseLength = 8f;
        private const float RopeHookSpeed = 18f;
        private const float RopeBaseSectorDegrees = 45f;
        private const float RopeBaseSwingAcceleration = 8f;

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
        private readonly BattleWeaponProgression _playerProgression;
        private readonly BattleWeaponProgression _opponentProgression;
        private readonly CharacterArchetype _playerArchetype;
        private readonly CharacterArchetype _opponentArchetype;
        private readonly TurnMovementState _playerMovement;
        private readonly List<SimProjectile> _projectiles = new();
        private readonly List<ActiveMine> _activeMines = new();
        private readonly List<ActiveCharge> _activeCharges = new();
        private readonly List<ActiveTimedEffect> _activeTimedEffects = new();
        private readonly List<ScheduledProjectileSpawn> _scheduledProjectileSpawns = new();
        private readonly List<ActiveFirePuddle> _activeFirePuddles = new();
        private readonly Dictionary<string, float> _lavaOverlayRemainingSeconds = new(StringComparer.Ordinal);
        private readonly Dictionary<string, ActiveBurnEffect> _burnEffects = new(StringComparer.Ordinal);
        private readonly Dictionary<string, float> _electrifiedOverlayRemainingSeconds = new(StringComparer.Ordinal);
        private readonly Dictionary<string, ActiveRepairEffect> _repairEffects = new(StringComparer.Ordinal);
        private readonly Dictionary<string, ActiveDisorientation> _disorientations = new(StringComparer.Ordinal);
        private readonly Dictionary<string, ActivePoisonEffect> _poisonEffects = new(StringComparer.Ordinal);
        private readonly Dictionary<string, ActiveDisorientation> _freezes = new(StringComparer.Ordinal);
        private readonly Dictionary<string, int> _lastFreezeApplicationTurn = new(StringComparer.Ordinal);
        private readonly Dictionary<string, ActiveDisorientation> _empStuns = new(StringComparer.Ordinal);
        private readonly Dictionary<string, ActiveEmpWeaken> _empWeakens = new(StringComparer.Ordinal);

        private WeaponDefinition _activeShotWeapon;
        private bool _activeShotEndsTurn = true;
        private int _blasterShotsThisTurn;
        private int _minesDeployedThisTurn;
        private int _nextProjectileId;
        private float _simulationAccumulator;
        private int _walkIntentDirection;
        private int _controlledProjectileId = -1;
        private Vector2 _controlledProjectileInput;
        private float _retreatRemaining = -1f;
        private TurnAdvanceReason _pendingRetreatReason;
        private bool _playerTurnTimerPaused;
        private int _turnSequence;
        private RopeTraversal _ropeTraversal;

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
            CharacterArchetype opponentArchetype,
            BattleWeaponProgression playerProgression = null,
            BattleWeaponProgression opponentProgression = null)
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
                opponentArchetype,
                playerProgression,
                opponentProgression);
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
            CharacterArchetype opponentArchetype,
            BattleWeaponProgression playerProgression = null,
            BattleWeaponProgression opponentProgression = null)
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
            _playerProgression = playerProgression ?? BattleWeaponProgression.Empty;
            _opponentProgression = opponentProgression ?? BattleWeaponProgression.Empty;
            _playerArchetype = playerArchetype;
            _opponentArchetype = opponentArchetype;
            _playerMovement = new TurnMovementState(settings.PlayerTurnDurationSeconds);
        }

        public event Action<BattleTeamId, WeaponDefinition, bool> ShotFired;
        public event Action<ImpactResolvedEvent> ImpactResolved;
        public event Action<BattleTeamId, int> TerrainCellsDestroyed;

        /// <summary>
        /// Geometry of a hole cut in the terrain: centre and radius. The cell count
        /// above is enough for scoring, but replicating terrain to another client
        /// needs the shape that produced it - a crater replays exactly from these two
        /// numbers, while the resulting mask is far too large to send.
        /// </summary>
        public event Action<BattleTeamId, Vector2, float> TerrainCraterApplied;
        public event Action<BattleTeamId, DamageEvent> DamageApplied;
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
        public event Action<GroundObjectRepositionedEvent> GroundObjectRepositioned;
        public event Action<LinearWeaponEffectEvent> LinearWeaponEffectStarted;
        public event Action<FireBlobEvent> FireBlobSpawned;
        public event Action<int, Vector2> FirePuddleMoved;
        public event Action<int> FirePuddleExtinguished;
        public event Action UnitsMoved;

        public BattleState State => _state;
        public BattleTerrain Terrain => _terrain;
        public BattleResult Result { get; private set; } = BattleResult.InProgress;
        public IReadOnlyList<SimProjectile> Projectiles => _projectiles;
        public int ProjectilesInFlight => _projectiles.Count;
        public bool HasProjectilesInFlight => _projectiles.Count > 0;
        public bool HasActiveWeaponEffect => _activeTimedEffects.Count > 0
            || _scheduledProjectileSpawns.Count > 0
            || _ropeTraversal != null && !_ropeTraversal.IsAttached;
        public bool HasPlayerRopeTraversal => _ropeTraversal != null
            && _ropeTraversal.OwnerUnitId == _state.Turn.ActiveUnitId;
        public bool IsPlayerRopeAttached => HasPlayerRopeTraversal && _ropeTraversal.IsAttached;
        public Vector2 RopeHookPosition => _ropeTraversal?.HookPosition ?? Vector2.zero;
        public Vector2 RopeHookDirection => _ropeTraversal?.Direction ?? Vector2.up;
        public Vector2 RopeAnchor => _ropeTraversal?.Anchor ?? Vector2.zero;
        public bool HasPendingGroundAction => _activeCharges.Count > 0;
        public bool HasActiveControlledProjectile => _controlledProjectileId >= 0;
        public bool CanContinueBlasterBurst => _state.Turn.ActiveTeam == BattleTeamId.Player
            && _blasterShotsThisTurn > 0
            && _blasterShotsThisTurn < GetBlasterShotsPerTurn(BattleTeamId.Player)
            && _playerInventory.Selected?.WeaponId == BlasterWeaponId
            && _playerInventory.CanFireSelected;
        public float MovementTimeRemaining => IsPlayerRetreatActive
            ? Mathf.Max(0f, _retreatRemaining)
            : _playerMovement.RemainingTurnSeconds;

        public void SetPlayerTurnTimerPaused(bool paused)
        {
            _playerTurnTimerPaused = paused;
        }

        /// <summary>
        /// The player fired a turn-ending shot and is inside the short
        /// movement-only retreat window before the turn hands over.
        /// </summary>
        public bool IsPlayerRetreatActive => _retreatRemaining > 0f
            && _state.Turn.ActiveTeam == BattleTeamId.Player;

        /// <summary>
        /// The turn's attack is already committed, unless the active player can
        /// still place another mine granted by the workshop evolution.
        /// </summary>
        public bool IsAttackCommitted => _state.Turn.ActionSpent || !CanContinueMineDeploymentThisTurn();

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
        /// <summary>
        /// False while the active unit is EMP-stunned (cannot fire this turn). The bot
        /// consults this before planning a shot so it falls back to
        /// <see cref="AdvanceTurnDueToNoUsableWeapon"/> instead of repeatedly attempting
        /// a <see cref="TryFire"/> that would just be blocked.
        /// </summary>
        public bool CanActiveUnitFire()
        {
            return !IsActiveUnitEmpStunned();
        }

        public bool CanAcceptMovementInput()
        {
            if (Result.IsFinished || HasProjectilesInFlight || HasActiveWeaponEffect)
            {
                return false;
            }

            // A spent action normally freezes movement, except during the
            // retreat window: movement stays live while firing remains blocked.
            if (_state.Turn.ActionSpent && !IsPlayerRetreatActive)
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

        public BattleFireResult TryFire(AimState aim)
        {
            if (_ropeTraversal != null && _ropeTraversal.IsAttached
                && _ropeTraversal.OwnerUnitId == _state.Turn.ActiveUnitId)
            {
                _ropeTraversal.Detach(GetMotor(_ropeTraversal.OwnerUnitId));
                _ropeTraversal = null;
                UnitsMoved?.Invoke();
                return new BattleFireResult(BattleFireOutcome.Fired, _state.Turn.ActiveTeam, _weaponsById[RopeWeaponId]);
            }

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

            if (IsActiveUnitEmpStunned())
            {
                return new BattleFireResult(BattleFireOutcome.Blocked, unit.TeamId, null);
            }

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

            if (_minesDeployedThisTurn > 0 && weapon.WeaponId != MineWeaponId)
            {
                return new BattleFireResult(BattleFireOutcome.Blocked, unit.TeamId, weapon);
            }

            if (weapon.WeaponId == MineWeaponId
                && _minesDeployedThisTurn >= GetMineDeployLimitPerTurn(unit.TeamId, weapon))
            {
                return new BattleFireResult(BattleFireOutcome.Blocked, unit.TeamId, weapon);
            }

            if (!aim.HasInput && RequiresAim(weapon))
            {
                return new BattleFireResult(BattleFireOutcome.AimRequired, unit.TeamId, weapon);
            }

            var effectiveAim = aim.HasInput ? aim : CreateImmediateActionAim(unit);

            if (weapon.WeaponId == RopeWeaponId)
            {
                var rank = ResolveRankFor(unit.TeamId, weapon);
                _ropeTraversal = new RopeTraversal(
                    unit.UnitId,
                    effectiveAim.Origin,
                    effectiveAim.Direction,
                    RopeBaseLength + (rank - 1) * 0.75f,
                    RopeHookSpeed,
                    RopeBaseSectorDegrees + (rank - 1) * 2f,
                    RopeBaseSwingAcceleration + (rank - 1) * 0.65f);
                ShotFired?.Invoke(unit.TeamId, weapon, false);
                UnitsMoved?.Invoke();
                return new BattleFireResult(BattleFireOutcome.Fired, unit.TeamId, weapon);
            }

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
                this,
                ResolveEvolutionFor(unit.TeamId, weapon),
                ResolveModifiersFor(unit.TeamId, weapon));
            var fireResult = _weaponExecutor.TryFireSelected(activeInventory, context);
            if (fireResult != WeaponFireResult.Fired)
            {
                var outcome = fireResult == WeaponFireResult.OutOfAmmo ? BattleFireOutcome.OutOfAmmo : BattleFireOutcome.NoWeaponSelected;
                return new BattleFireResult(outcome, unit.TeamId, weapon);
            }

            _activeShotWeapon = weapon;
            _activeShotEndsTurn = weapon.WeaponId != TeleportWeaponId;
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

        public void SetRopeTraversalInput(int horizontalDirection, int verticalDirection)
        {
            if (_ropeTraversal == null || !_ropeTraversal.IsAttached)
            {
                return;
            }

            _ropeTraversal.SetInput(GetMotor(_ropeTraversal.OwnerUnitId), horizontalDirection, verticalDirection);
        }

        /// <summary>
        /// Rank 5/10 branch mechanics for whichever side is firing. Both sides are
        /// looked up the same way: a raid or duel opponent is developed exactly as
        /// far as its own <see cref="BattleWeaponProgression"/> says.
        /// </summary>
        private WeaponEvolutionProfile ResolveEvolutionFor(BattleTeamId teamId, WeaponDefinition weapon)
        {
            return weapon == null
                ? WeaponEvolutionProfile.Base
                : ProgressionFor(teamId).ResolveEvolution(weapon.WeaponId);
        }

        /// <summary>
        /// Flat % Damage/ImpactRadius bonuses from ranks 2-4 and 6-9, resolved per
        /// side exactly like <see cref="ResolveEvolutionFor"/>.
        /// </summary>
        private WeaponUpgradeModifiers ResolveModifiersFor(BattleTeamId teamId, WeaponDefinition weapon)
        {
            return weapon == null
                ? default
                : ProgressionFor(teamId).ResolveModifiers(weapon.WeaponId);
        }

        private int ResolveRankFor(BattleTeamId teamId, WeaponDefinition weapon)
        {
            return weapon == null
                ? 1
                : Mathf.Max(1, (teamId == BattleTeamId.Player ? _playerProgression : _opponentProgression)
                    .ResolveRank(weapon.WeaponId));
        }

        private BattleWeaponProgression ProgressionFor(BattleTeamId teamId)
        {
            return teamId == BattleTeamId.Player ? _playerProgression : _opponentProgression;
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

        /// <summary>
        /// Ends the active turn without an action, the same way the turn timer does.
        /// Used when a combatant gives their turn up explicitly - a remote player
        /// passing, or a battle being resynchronised past a turn nobody completed.
        /// </summary>
        public void EndActiveTurn()
        {
            if (Result.IsFinished || GetActiveUnitOrNull() == null)
            {
                return;
            }

            EndPlayerTurnFromTimer();
        }

        public void AdvanceTurnDueToNoUsableWeapon()
        {
            ConsumeDisorientationForActiveTurn();
            ConsumeFreezeForActiveTurn();
            ConsumeEmpStunForActiveTurn();
            _turnService.AdvanceTurn(_state);
            _turnSequence++;
            ResetMotionForNewTurn();
            ProcessRepairEffectsForActiveTurn();
            ProcessPoisonForActiveTurn();
            ProcessBurnForActiveTurn();
            ProcessEmpWeakenForActiveTurn();
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
                StepRetreat(_settings.SimulationTickSeconds);
                StepWalkIntent(_settings.SimulationTickSeconds);
                StepRopeTraversal(_settings.SimulationTickSeconds);
                StepScheduledProjectileSpawns(_settings.SimulationTickSeconds);
                StepProjectiles(_settings.SimulationTickSeconds);
                StepTimedWeaponEffects(_settings.SimulationTickSeconds);
                StepFirePuddles(_settings.SimulationTickSeconds);
                StepLavaOverlayStates(_settings.SimulationTickSeconds);
                StepElectrifiedOverlayStates(_settings.SimulationTickSeconds);
                StepMines(_settings.SimulationTickSeconds);
                StepCharges(_settings.SimulationTickSeconds);
                StepUnits();
                StepPoisonMovementDamage(_settings.SimulationTickSeconds);
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
                var evolution = ResolveEvolutionFor(ResolveTeamForOwner(impact.OwnerUnitId), impactWeapon);
                if (evolution.MortarFragmentCount > 0)
                {
                    SpawnMortarFragments(impact, impactWeapon, evolution.MortarFragmentCount);
                }
                else if (evolution.MortarCascadeCount > 0)
                {
                    SpawnMortarCascade(impact, impactWeapon, 1);
                }
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
            else if (IsMortarCascade(impact.ProjectileId)
                && _weaponsById.TryGetValue("weapon.mortira", out var cascadeMortar))
            {
                var evolution = ResolveEvolutionFor(ResolveTeamForOwner(impact.OwnerUnitId), cascadeMortar);
                var stage = GetMortarCascadeStage(impact.ProjectileId);
                if (stage < evolution.MortarCascadeCount)
                {
                    SpawnMortarCascade(impact, cascadeMortar, stage + 1);
                }
            }
            else if (IsMissileCluster(impact.ProjectileId)
                && _weaponsById.TryGetValue(MissileWeaponId, out var missile))
            {
                impactWeapon = CreateMissileClusterWeapon(missile, GetMissileClusterCount(impact.ProjectileId));
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

            AdvanceTurnOrStartRetreat(TurnAdvanceReason.ProjectileImpactResolved);
        }

        /// <summary>
        /// Ends the acting team's turn. The player gets the movement-only
        /// retreat window first (bounded by the turn timer); the bot hands the
        /// turn over immediately.
        /// </summary>
        private void AdvanceTurnOrStartRetreat(TurnAdvanceReason reason)
        {
            if (_state.Turn.ActiveTeam == BattleTeamId.Player)
            {
                var retreat = Mathf.Min(RetreatSeconds, _playerMovement.RemainingTurnSeconds);
                if (retreat > 0.01f)
                {
                    // Instant utilities (C4, heal) reach here without a spent
                    // action; spending it here keeps TryFire blocked while
                    // movement stays allowed via IsPlayerRetreatActive.
                    _turnService.SpendAction(_state);
                    _retreatRemaining = retreat;
                    _pendingRetreatReason = reason;
                    UnitsMoved?.Invoke();
                    return;
                }
            }

            AdvanceTurnNow(reason);
        }

        private void AdvanceTurnNow(TurnAdvanceReason reason)
        {
            ConsumeDisorientationForActiveTurn();
            ConsumeFreezeForActiveTurn();
            ConsumeEmpStunForActiveTurn();
            _turnService.AdvanceTurn(_state);
            _turnSequence++;
            ResetMotionForNewTurn();
            ProcessRepairEffectsForActiveTurn();
            ProcessPoisonForActiveTurn();
            ProcessBurnForActiveTurn();
            ProcessEmpWeakenForActiveTurn();
            TurnAdvanced?.Invoke(_state.Turn.ActiveTeam, reason);
            UnitsMoved?.Invoke();
        }

        private void StepRetreat(float deltaTime)
        {
            if (_retreatRemaining <= 0f || Result.IsFinished)
            {
                return;
            }

            _retreatRemaining -= deltaTime;
            if (_retreatRemaining <= 0f)
            {
                _retreatRemaining = -1f;
                AdvanceTurnNow(_pendingRetreatReason);
            }
        }

        /// <summary>
        /// Spawns the selected fragment-mortar evolution in a deterministic fan.
        /// </summary>
        private void SpawnMortarFragments(ProjectileImpact impact, WeaponDefinition weapon, int count)
        {
            var fragmentWeapon = CreateMortarFragmentWeapon(weapon);
            var fragmentDefinition = ProjectileDefinition.CreateRuntime(
                "projectile.weapon.mortira.fragment",
                fragmentWeapon.ProjectileRadius,
                weapon.GravityScale,
                3f,
                fragmentWeapon.Damage,
                fragmentWeapon.ImpactRadius);

            var incoming = impact.Velocity.sqrMagnitude > 0.01f ? impact.Velocity.normalized : Vector2.down;
            var baseAngle = Mathf.Atan2(-incoming.y, -incoming.x) * Mathf.Rad2Deg;
            for (var i = 0; i < count; i++)
            {
                var t = count > 1 ? i / (float)(count - 1) : 0.5f;
                var angle = (baseAngle + Mathf.Lerp(-MortarFragmentSpreadDegrees, MortarFragmentSpreadDegrees, t)) * Mathf.Deg2Rad;
                var direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
                Spawn(new ProjectileSpawnRequest(
                    fragmentDefinition,
                    impact.OwnerUnitId,
                    impact.Point + direction * 0.08f,
                    direction * MortarFragmentSpeed,
                    fragmentDefinition.Radius * 2f,
                    ProjectileWeaponEffect.GetProjectileColor(weapon.Role)));
            }
        }

        /// <summary>
        /// The cascade branch drops a progressively smaller charge into the new
        /// crater. Damage and impact radius halve per stage, while terrain
        /// destruction stays tied to the base mortar in ResolveImpact.
        /// </summary>
        private void SpawnMortarCascade(ProjectileImpact impact, WeaponDefinition weapon, int stage)
        {
            var scale = Mathf.Pow(MortarCascadeDamageScale, Mathf.Max(1, stage));
            var definition = ProjectileDefinition.CreateRuntime(
                MortarCascadeProjectilePrefix + stage,
                weapon.ProjectileRadius * scale,
                weapon.GravityScale,
                1.5f,
                Mathf.Max(1, Mathf.CeilToInt(weapon.Damage * scale)),
                weapon.ImpactRadius * scale);

            var horizontalDirection = Mathf.Abs(impact.Velocity.x) > 0.01f
                ? Mathf.Sign(impact.Velocity.x)
                : 0f;
            var velocity = new Vector2(horizontalDirection * MortarCascadeSpeed * 0.18f, -MortarCascadeSpeed);
            Schedule(new ProjectileSpawnRequest(
                    definition,
                    impact.OwnerUnitId,
                    impact.Point + Vector2.up * 0.12f,
                    velocity,
                    definition.Radius * 2f,
                    ProjectileWeaponEffect.GetProjectileColor(weapon.Role)),
                MortarCascadeDelaySeconds);
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

        private static WeaponDefinition CreateC4EvolutionWeapon(
            WeaponDefinition weapon,
            WeaponEvolutionProfile evolution,
            WeaponUpgradeModifiers modifiers = default)
        {
            var damageMultiplier = (evolution.C4DamageMultiplier > 0f ? evolution.C4DamageMultiplier : 1f) * modifiers.DamageMultiplier;
            var radiusMultiplier = (evolution.C4ImpactRadiusMultiplier > 0f ? evolution.C4ImpactRadiusMultiplier : 1f) * modifiers.ImpactRadiusMultiplier;
            if (Mathf.Approximately(damageMultiplier, 1f) && Mathf.Approximately(radiusMultiplier, 1f))
            {
                return weapon;
            }

            return WeaponDefinition.CreateRuntime(
                weapon.WeaponId,
                weapon.DisplayKey,
                weapon.Role,
                Mathf.Max(1, Mathf.CeilToInt(weapon.Damage * damageMultiplier)),
                weapon.ImpactRadius * radiusMultiplier,
                weapon.ProjectileRadius,
                weapon.GravityScale,
                weapon.TimeoutSeconds,
                weapon.MinPower,
                weapon.MaxPower,
                weapon.AmmoCount,
                weapon.TutorialHintKey,
                CopyTags(weapon.Tags),
                iconId: weapon.IconId);
        }

        private static WeaponDefinition CreateFpvEvolutionWeapon(
            WeaponDefinition weapon,
            WeaponEvolutionProfile evolution,
            WeaponUpgradeModifiers modifiers = default)
        {
            var damageMultiplier = (evolution.FpvDamageMultiplier > 0f ? evolution.FpvDamageMultiplier : 1f) * modifiers.DamageMultiplier;
            var radiusMultiplier = (evolution.FpvImpactRadiusMultiplier > 0f ? evolution.FpvImpactRadiusMultiplier : 1f) * modifiers.ImpactRadiusMultiplier;
            if (Mathf.Approximately(damageMultiplier, 1f) && Mathf.Approximately(radiusMultiplier, 1f))
            {
                return weapon;
            }

            return WeaponDefinition.CreateRuntime(
                weapon.WeaponId,
                weapon.DisplayKey,
                weapon.Role,
                Mathf.Max(1, Mathf.CeilToInt(weapon.Damage * damageMultiplier)),
                weapon.ImpactRadius * radiusMultiplier,
                weapon.ProjectileRadius,
                weapon.GravityScale,
                weapon.TimeoutSeconds,
                weapon.MinPower,
                weapon.MaxPower,
                weapon.AmmoCount,
                weapon.TutorialHintKey,
                CopyTags(weapon.Tags),
                iconId: weapon.IconId);
        }

        private static WeaponDefinition CreateMissileClusterWeapon(WeaponDefinition weapon, int chargeCount)
        {
            var count = Mathf.Max(2, chargeCount);
            return WeaponDefinition.CreateRuntime(
                "weapon.missle.cluster",
                weapon.DisplayKey,
                weapon.Role,
                Mathf.Max(1, Mathf.CeilToInt(weapon.Damage / (float)count)),
                weapon.ImpactRadius * 0.58f,
                weapon.ProjectileRadius * 0.6f,
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

        private static bool IsMortarCascade(string projectileId)
        {
            return projectileId != null
                && projectileId.StartsWith(MortarCascadeProjectilePrefix, StringComparison.Ordinal);
        }

        private static int GetMortarCascadeStage(string projectileId)
        {
            if (!IsMortarCascade(projectileId))
            {
                return 0;
            }

            return int.TryParse(projectileId.Substring(MortarCascadeProjectilePrefix.Length), out var stage)
                ? Mathf.Max(0, stage)
                : 0;
        }

        private static bool IsMissileCluster(string projectileId)
        {
            return projectileId != null
                && projectileId.StartsWith(MissileClusterProjectilePrefix, StringComparison.Ordinal);
        }

        private static int GetMissileClusterCount(string projectileId)
        {
            if (!IsMissileCluster(projectileId))
            {
                return 0;
            }

            return int.TryParse(projectileId.Substring(MissileClusterProjectilePrefix.Length), out var count)
                ? Mathf.Max(0, count)
                : 0;
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
            if (_ropeTraversal != null)
            {
                _ropeTraversal.Detach(GetMotor(_ropeTraversal.OwnerUnitId));
                _ropeTraversal = null;
            }
            _walkIntentDirection = 0;
            _retreatRemaining = -1f;
            _controlledProjectileInput = Vector2.zero;
            if (_projectiles.Count == 0)
            {
                _controlledProjectileId = -1;
            }
            _blasterShotsThisTurn = 0;
            _minesDeployedThisTurn = 0;
            _activeShotEndsTurn = true;
            foreach (var motor in _motors.Values)
            {
                motor.ResetWalk();
            }

            _playerMovement.Reset(_settings.PlayerTurnDurationSeconds);
        }

        private BattleFireResult UseBlasterShot(BattleUnitState unit, WeaponInventoryRuntime inventory, WeaponDefinition weapon, AimState aim)
        {
            var shotsPerTurn = GetBlasterShotsPerTurn(unit.TeamId);
            if (_blasterShotsThisTurn >= shotsPerTurn || !inventory.TryConsumeSelectedAmmo())
            {
                return new BattleFireResult(BattleFireOutcome.OutOfAmmo, unit.TeamId, weapon);
            }

            var direction = aim.Direction.sqrMagnitude > 0.01f ? aim.Direction.normalized : Vector2.right;
            var evolution = ResolveEvolutionFor(unit.TeamId, weapon);
            var request = new ProjectileSpawnRequest(
                weapon.CreateProjectileDefinition(weapon.Damage, 1f, evolution.TerrainBounceLimit),
                unit.UnitId,
                aim.Origin + direction * 0.45f,
                direction * aim.Power,
                weapon.ProjectileRadius * 2f,
                ProjectileWeaponEffect.GetProjectileColor(weapon.Role));

            _activeShotWeapon = weapon;
            _blasterShotsThisTurn++;
            // The turn ends on the final shot, when the magazine runs dry
            // mid-burst (otherwise the turn would never resolve), and always for
            // the bot - it fires a single planned shot per turn.
            _activeShotEndsTurn = _blasterShotsThisTurn >= shotsPerTurn
                || !inventory.CanFireSelected
                || unit.TeamId == BattleTeamId.Opponent;
            if (_activeShotEndsTurn)
            {
                _turnService.SpendAction(_state);
            }

            Spawn(request);
            ShotFired?.Invoke(unit.TeamId, weapon, true);
            return new BattleFireResult(BattleFireOutcome.Fired, unit.TeamId, weapon);
        }

        private int GetBlasterShotsPerTurn(BattleTeamId teamId)
        {
            if (teamId != BattleTeamId.Player || _playerInventory.Selected?.WeaponId != BlasterWeaponId)
            {
                return BaseBlasterShotsPerTurn;
            }

            return Mathf.Max(
                BaseBlasterShotsPerTurn,
                ResolveEvolutionFor(teamId, _playerInventory.Selected).BlasterBurstShotCount);
        }

        private int GetMineDeployLimitPerTurn(BattleTeamId teamId, WeaponDefinition weapon)
        {
            return Mathf.Max(1, ResolveEvolutionFor(teamId, weapon).MineDeployLimitPerTurn);
        }

        private bool CanContinueMineDeploymentThisTurn()
        {
            if (_minesDeployedThisTurn == 0)
            {
                return true;
            }

            if (!_weaponsById.TryGetValue(MineWeaponId, out var mineWeapon))
            {
                return false;
            }

            var activeTeam = _state.Turn.ActiveTeam;
            var inventory = GetInventoryFor(activeTeam);
            return _minesDeployedThisTurn < GetMineDeployLimitPerTurn(activeTeam, mineWeapon)
                && inventory.GetRemainingAmmo(mineWeapon) > 0;
        }

        private BattleFireResult DeployMine(BattleUnitState unit, WeaponInventoryRuntime inventory, WeaponDefinition weapon)
        {
            if (_minesDeployedThisTurn >= GetMineDeployLimitPerTurn(unit.TeamId, weapon))
            {
                return new BattleFireResult(BattleFireOutcome.OutOfAmmo, unit.TeamId, weapon);
            }

            PrepareMineAmmoForPlacement(unit.TeamId, inventory, weapon);
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
            _minesDeployedThisTurn++;
            ShotFired?.Invoke(unit.TeamId, weapon, false);
            return new BattleFireResult(BattleFireOutcome.Fired, unit.TeamId, weapon, origin);
        }

        private void PrepareMineAmmoForPlacement(
            BattleTeamId teamId,
            WeaponInventoryRuntime inventory,
            WeaponDefinition weapon)
        {
            var placementsRemaining = GetMineDeployLimitPerTurn(teamId, weapon) - _minesDeployedThisTurn;
            if (placementsRemaining <= 1 || inventory.GetRemainingAmmo(weapon) >= placementsRemaining)
            {
                return;
            }

            inventory.SetRemainingAmmo(weapon, placementsRemaining);
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

            var evolution = ResolveEvolutionFor(unit.TeamId, weapon);
            var evolvedWeapon = CreateFpvEvolutionWeapon(weapon, evolution, ResolveModifiersFor(unit.TeamId, weapon));
            var controlDuration = weapon.TimeoutSeconds + evolution.FpvControlBonusSeconds;
            var spawnPoint = unit.Position + Vector2.up * 1.05f;
            var definition = evolvedWeapon.CreateProjectileDefinition(evolvedWeapon.Damage, 1f, 0, controlDuration);
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
            // In an enclosed pocket the surface scan starts above the roof and
            // "teleports" the charge on top of it. Keep it at the unit's feet.
            var feetY = unit.Position.y - ArchetypeFor(unit.TeamId).BodyHeight * 0.5f;
            if (deployPoint.y > feetY + 0.6f)
            {
                deployPoint = new Vector2(unit.Position.x + facing * 0.3f, feetY + 0.06f);
            }
            var evolution = ResolveEvolutionFor(unit.TeamId, weapon);
            var evolvedWeapon = CreateC4EvolutionWeapon(weapon, evolution, ResolveModifiersFor(unit.TeamId, weapon));
            var charge = new ActiveCharge(
                ++_nextProjectileId,
                unit.UnitId,
                unit.TeamId,
                evolvedWeapon,
                deployPoint,
                C4FuseSeconds,
                evolution.KnockbackMultiplier);
            _activeCharges.Add(charge);
            var dropDistance = Mathf.Max(0f, origin.y - deployPoint.y);
            var dropDuration = Mathf.Clamp(C4DropDurationSeconds + dropDistance * 0.08f, C4DropDurationSeconds, 0.75f);
            ChargeDeployed?.Invoke(new ChargeDeployedEvent(
                charge.Id,
                origin,
                deployPoint,
                dropDuration,
                evolvedWeapon,
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
            var evolution = ResolveEvolutionFor(unit.TeamId, weapon);
            var drillRange = evolution.DrillRange > 0f ? evolution.DrillRange : DrillMaxRange;
            _activeShotWeapon = weapon;
            _activeShotEndsTurn = true;
            _turnService.SpendAction(_state);
            _activeTimedEffects.Add(ActiveTimedEffect.Drill(
                unit.UnitId,
                unit.TeamId,
                weapon,
                aim.Origin,
                direction,
                drillRange,
                evolution.DrillPenetration));
            LinearWeaponEffectStarted?.Invoke(new LinearWeaponEffectEvent(
                weapon,
                unit.TeamId,
                aim.Origin,
                direction,
                drillRange,
                DrillVisualGrowthSeconds,
                DrillDurationSeconds));
            ShotFired?.Invoke(unit.TeamId, weapon, true);
            return new BattleFireResult(BattleFireOutcome.Fired, unit.TeamId, weapon, aim.Origin + direction * drillRange);
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
            var evolution = ResolveEvolutionFor(unit.TeamId, weapon);
            var streamDuration = FireStreamDurationSeconds * evolution.FireGunStreamDurationMultiplier;
            _activeTimedEffects.Add(ActiveTimedEffect.FireStream(unit.UnitId, unit.TeamId, weapon, aim.Origin, direction, streamDuration));
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

            foreach (var team in _state.Teams)
            {
                foreach (var target in team.Units)
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

                    ApplyDamage(target, weapon.Damage, weapon.WeaponId, unit.TeamId);
                    hitCount++;
                }
            }

            var burstPoint = origin + direction * Mathf.Min(range, 0.95f);
            var terrainChanged = ApplyTerrainDestruction(burstPoint, weapon, unit.TeamId);
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
                // Healing is a completed action too: it enters the universal
                // movement-only retreat window instead of leaving a free turn.
                CompleteInstantAction();
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

            AdvanceTurnOrStartRetreat(TurnAdvanceReason.InstantActionCompleted);
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
                        // Track the beam visual exactly (same 0.25->max growth
                        // curve), so destruction never outruns what is drawn.
                        var terrainRange01 = Mathf.Clamp01(effect.Elapsed / DrillVisualGrowthSeconds);
                        var terrainRange = Mathf.Lerp(0.25f, effect.DrillRange, terrainRange01);
                        ApplyLinearTerrain(effect, terrainRange, 0.6f);
                    }

                    effect.TickAccumulator += deltaTime;
                    while (effect.TickAccumulator >= DrillTickSeconds)
                    {
                        effect.TickAccumulator -= DrillTickSeconds;
                        var range01 = Mathf.Clamp01(effect.Elapsed / DrillVisualGrowthSeconds);
                        var range = Mathf.Lerp(0.25f, effect.DrillRange, range01);
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

                    if (effect.Elapsed < effect.StreamDurationSeconds)
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
                puddle.DamageTickAccumulator += deltaTime;
                var startPoint = puddle.Point;
                var hasSupport = true;
                while (puddle.TickAccumulator >= FirePuddleTickSeconds)
                {
                    puddle.TickAccumulator -= FirePuddleTickSeconds;
                    hasSupport &= TryDropFirePuddleToSurface(puddle);
                    puddle.Point += Vector2.down * 0.045f;
                    // Small scorch bites: about a third of the flame sprite's
                    // area per bite - the fire chews through slowly instead of
                    // demolishing the ground. A deterministic sideways wobble
                    // (hashed from puddle id + burn time) keeps the burn holes
                    // ragged instead of drilling perfectly vertical shafts.
                    var wobble = Mathf.Sin(puddle.Id * 12.9898f + puddle.Elapsed * 9.7f) * 0.09f;
                    var terrainChanged = ApplyTerrainCrater(
                        puddle.Point + new Vector2(wobble, -0.08f),
                        FirePuddleTerrainBurnRadius,
                        ResolveTeamForOwner(puddle.OwnerUnitId));
                    if (terrainChanged)
                    {
                        // The fire just burned through its own floor - chase the
                        // new surface below instead of burning in mid-air.
                        hasSupport &= TryDropFirePuddleToSurface(puddle);
                    }

                    UnitsMoved?.Invoke();
                }

                while (puddle.DamageTickAccumulator >= FirePuddleDamageTickSeconds)
                {
                    puddle.DamageTickAccumulator -= FirePuddleDamageTickSeconds;
                    var tickDamage = GetFirePuddleDamagePerTick(puddle);
                    if (tickDamage > 0)
                    {
                        ApplyPointDamage(
                            puddle.Point,
                            puddle.Weapon,
                            puddle.OwnerUnitId,
                            FirePuddleDamageRadius,
                            tickDamage,
                            applyLavaOverlay: true);
                    }

                    UnitsMoved?.Invoke();
                }

                if (!hasSupport || puddle.Point.y < _settings.FallDeathY)
                {
                    // Nothing left underneath anywhere - the fire drains out of
                    // the world instead of hovering.
                    _activeFirePuddles.RemoveAt(i);
                    FirePuddleExtinguished?.Invoke(puddle.Id);
                    continue;
                }

                if ((puddle.Point - startPoint).sqrMagnitude > 0.0004f)
                {
                    FirePuddleMoved?.Invoke(puddle.Id, puddle.Point);
                }

                if (puddle.Elapsed >= puddle.LifetimeSeconds)
                {
                    _activeFirePuddles.RemoveAt(i);
                    FirePuddleExtinguished?.Invoke(puddle.Id);
                }
            }
        }

        /// <summary>
        /// Keeps a burning puddle glued to the terrain surface. Returns false
        /// when no surface exists anywhere below - the puddle has nothing to
        /// burn on and should be removed.
        /// </summary>
        private bool TryDropFirePuddleToSurface(ActiveFirePuddle puddle)
        {
            if (_terrain == null)
            {
                return true;
            }

            var probePoint = puddle.Point + Vector2.down * 0.12f;
            if (_terrain.Grid.IsSolidWorld(probePoint))
            {
                return true;
            }

            if (_terrain.Grid.TryFindSurfaceBelow(
                    puddle.Point.x,
                    puddle.Point.y - 0.08f,
                    FirePuddleDropSearchDistance,
                    out var surfaceY))
            {
                if (puddle.Point.y - surfaceY > 0.12f)
                {
                    puddle.Point = new Vector2(puddle.Point.x, surfaceY + 0.08f);
                }

                return true;
            }

            return false;
        }

        private static int GetFirePuddleDamagePerTick(ActiveFirePuddle puddle)
        {
            return puddle.ConsumeDamageForTick();
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
                DropGroundObjectIfUnsupported(mine, mine.Id);
                mine.ArmTimeRemaining = Mathf.Max(0f, mine.ArmTimeRemaining - deltaTime);
                if (mine.ArmTimeRemaining > 0f)
                {
                    continue;
                }

                if (!mine.Triggered && IsMineTriggerTargetInside(mine))
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
                DropGroundObjectIfUnsupported(charge, charge.Id);
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
                    charge.Weapon.ImpactRadius,
                    charge.KnockbackMultiplier);
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

        private void SpawnFireBlob(ActiveTimedEffect effect)
        {
            var puddleId = ++_nextProjectileId;
            // Reference flame thrower ramps projectile power over the burst and
            // wobbles the angle slightly, so the fire lays down a strip instead
            // of pooling in a single point. Deterministic per blob index.
            var blobIndex = effect.SpawnedBlobs++;
            var totalBlobs = Mathf.Max(2, Mathf.RoundToInt(effect.StreamDurationSeconds / FireStreamSpawnIntervalSeconds));
            var ramp01 = Mathf.Clamp01(blobIndex / (float)(totalBlobs - 1));
            var speed = FireBlobSpeed * Mathf.Lerp(0.65f, 1.35f, ramp01);
            var jitterRadians = ((blobIndex % 3) - 1) * 2.5f * Mathf.Deg2Rad;
            var cos = Mathf.Cos(jitterRadians);
            var sin = Mathf.Sin(jitterRadians);
            var direction = new Vector2(
                effect.Direction.x * cos - effect.Direction.y * sin,
                effect.Direction.x * sin + effect.Direction.y * cos);
            var initialVelocity = direction * speed;
            var gravity = _projectileGravityPerScale * FireBlobGravityScale;
            TraceFireBlobTrajectory(
                effect.OwnerUnitId,
                effect.Origin,
                initialVelocity,
                gravity,
                out var endpoint,
                out var travelSeconds);
            var evolution = ResolveEvolutionFor(effect.TeamId, effect.Weapon);
            var puddleLifetime = FirePuddleLifetimeSeconds * evolution.FireGunBurnDurationMultiplier;
            FireBlobSpawned?.Invoke(new FireBlobEvent(
                puddleId,
                effect.Weapon,
                effect.TeamId,
                effect.Origin,
                initialVelocity,
                gravity,
                endpoint,
                travelSeconds,
                puddleLifetime));
            var baseDamageBudget = effect.Weapon.Damage / totalBlobs;
            var remainingDamageBudget = effect.Weapon.Damage % totalBlobs;
            var damageBudget = baseDamageBudget + (blobIndex < remainingDamageBudget ? 1 : 0);
            _activeFirePuddles.Add(new ActiveFirePuddle(
                puddleId,
                effect.OwnerUnitId,
                effect.Weapon,
                endpoint,
                travelSeconds,
                puddleLifetime,
                damageBudget));
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
                ApplyTerrainCrater(endpoint, craterRadius, effect.TeamId);
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
                    if (distance > 0.7f)
                    {
                        continue;
                    }

                    if (damage > 0)
                    {
                        ApplyDamage(unit, damage, effect.Weapon.WeaponId, effect.TeamId);
                    }
                }
            }

            UnitsMoved?.Invoke();
        }

        /// <summary>
        /// Burns a channel along the whole beam. Crater centers are spaced so
        /// the farthest crater's edge lands exactly on the beam tip: terrain
        /// never crumbles ahead of the laser, and destruction stops together
        /// with the beam (the beam is drawn through walls at its full length,
        /// so everything it overlaps must burn).
        /// </summary>
        private void ApplyLinearTerrain(ActiveTimedEffect effect, float beamLength, float craterRadius)
        {
            if (_terrain == null)
            {
                return;
            }

            var carveEnd = beamLength - craterRadius;
            for (var distance = 0.45f; distance <= carveEnd; distance += craterRadius)
            {
                ApplyTerrainCrater(
                    effect.Origin + effect.Direction * distance,
                    craterRadius,
                    effect.TeamId,
                    effect.DrillPenetration);
            }

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

        private void ApplyPointDamage(
            Vector2 point,
            WeaponDefinition weapon,
            string ownerUnitId,
            float radius,
            int damage,
            bool applyLavaOverlay = false)
        {
            var sourceTeam = ResolveTeamForOwner(ownerUnitId);
            var fireEvolution = applyLavaOverlay ? ResolveEvolutionFor(sourceTeam, weapon) : default;
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

                    var resolvedDamage = damage;
                    if (applyLavaOverlay
                        && ApplyBurn(unit, sourceTeam, fireEvolution, out var burnWasAlreadyActive)
                        && burnWasAlreadyActive)
                    {
                        resolvedDamage = Mathf.CeilToInt(
                            resolvedDamage * (1f + fireEvolution.FireGunChainDamagePercent / 100f));
                    }

                    ApplyDamage(unit, resolvedDamage, weapon.WeaponId, sourceTeam);
                    if (applyLavaOverlay)
                    {
                        _lavaOverlayRemainingSeconds[unit.UnitId] = LavaOverlayDurationSeconds;
                    }
                }
            }

            Result = BattleResultEvaluator.Evaluate(_state);
            if (Result.IsFinished)
            {
                BattleFinished?.Invoke(Result, BattleFinishReason.ProjectileImpactResolved);
            }
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
            var terrainChanged = ResolveDamageImpact(
                impact,
                mine.Weapon,
                mine.AlliesImmuneToExplosion ? mine.TeamId : BattleTeamId.None);
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

                var restored = unit.RestoreHealth(impactWeapon.Damage);
                restoredHealth += restored;
                if (restored > 0)
                {
                    ApplyRepairAftereffect(unit, owner.TeamId, impactWeapon);
                }
            }

            HealthRestored?.Invoke(new HealthRestoredEvent(impactPoint, restoredHealth, owner.TeamId));
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
            var bestDistanceSquared = TeleportExchangeTargetRadius * TeleportExchangeTargetRadius;
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
                    if (distanceSquared > TeleportExchangeTargetRadius * TeleportExchangeTargetRadius)
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
                    RepositionOwner(impact.Point, impact.OwnerUnitId, impactWeapon, impact.SourceOrigin);
                    return false;
                default:
                    return ResolveDamageImpact(impact, impactWeapon);
            }
        }

        private bool ResolveDamageImpact(
            ProjectileImpact impact,
            WeaponDefinition impactWeapon,
            BattleTeamId ignoredTeam = BattleTeamId.None)
        {
            var terrainChanged = ApplyTerrainDestruction(
                impact.Point,
                impactWeapon,
                ResolveTeamForOwner(impact.OwnerUnitId));

            var allUnits = new List<BattleUnitState>();
            foreach (var team in _state.Teams)
            {
                if (team.TeamId == ignoredTeam)
                {
                    continue;
                }

                allUnits.AddRange(team.Units);
            }

            var sourceTeam = ResolveTeamForOwner(impact.OwnerUnitId);
            var effectiveDamage = impact.Damage;
            if (_empWeakens.TryGetValue(impact.OwnerUnitId, out var empWeaken) && empWeaken.DamagePercent > 0)
            {
                effectiveDamage = Mathf.Max(0, Mathf.FloorToInt(effectiveDamage * (1f - empWeaken.DamagePercent / 100f)));
            }

            var appliesPoison = impactWeapon.WeaponId == ToxicWeaponId;
            var poisonEvolution = appliesPoison ? ResolveEvolutionFor(sourceTeam, impactWeapon) : default;
            var appliesEmpStun = impactWeapon.WeaponId == EmpWeaponId;
            var empEvolution = appliesEmpStun ? ResolveEvolutionFor(sourceTeam, impactWeapon) : default;
            var appliesFreeze = impactWeapon.WeaponId == FreezeWeaponId;
            var freezeEvolution = appliesFreeze ? ResolveEvolutionFor(sourceTeam, impactWeapon) : default;

            const float selfDamageMultiplier = 1f;
            var settings = new RadialDamageSettings(effectiveDamage, impact.ImpactRadius, selfDamageMultiplier);
            var damageEvents = DamageResolver.ResolveRadialDamage(
                allUnits,
                impact.Point,
                impact.OwnerUnitId,
                impact.ProjectileId,
                settings,
                (unit, amount) => ResolveFreezeChainDamage(ResolveIncomingDamage(unit, amount), unit, appliesFreeze, freezeEvolution));
            foreach (var damageEvent in damageEvents)
            {
                DamageApplied?.Invoke(sourceTeam, damageEvent);
                var target = FindUnit(damageEvent.TargetUnitId);
                if (appliesPoison)
                {
                    ApplyPoison(target, sourceTeam, poisonEvolution);
                }

                if (appliesEmpStun)
                {
                    ApplyElectrifiedOverlay(target?.UnitId);
                    if (empEvolution.EmpStunTurns > 0)
                    {
                        ApplyEmpStun(target?.UnitId, empEvolution.EmpStunTurns);
                        if (empEvolution.EmpStunBlocksMovement)
                        {
                            ApplyDisorientation(target?.UnitId, empEvolution.EmpStunTurns);
                        }
                    }

                    if (empEvolution.EmpWeakenDamagePercent > 0)
                    {
                        ApplyEmpWeaken(target?.UnitId, empEvolution.EmpWeakenDamagePercent, empEvolution.EmpWeakenTurns);
                    }
                }

                if (appliesFreeze)
                {
                    var freezeTurns = freezeEvolution.CryoFreezeTurns > 0f
                        ? Mathf.RoundToInt(freezeEvolution.CryoFreezeTurns)
                        : CryoBaseFreezeTurns;
                    ApplyFreeze(target?.UnitId, freezeTurns);
                    if (target != null)
                    {
                        _lastFreezeApplicationTurn[target.UnitId] = _turnSequence;
                    }
                }
            }
            ApplyExplosionKnockback(
                impact.Point,
                impact.Velocity,
                impactWeapon,
                terrainChanged,
                impact.KnockbackMultiplier,
                ignoredTeam);
            return terrainChanged;
        }

        private void ApplyRepairAftereffect(
            BattleUnitState unit,
            BattleTeamId teamId,
            WeaponDefinition repairWeapon)
        {
            var evolution = ResolveEvolutionFor(teamId, repairWeapon);
            if (evolution.RepairProtectionTurns <= 0 && evolution.RepairRegenerationTurns <= 0)
            {
                return;
            }

            _repairEffects[unit.UnitId] = new ActiveRepairEffect(
                unit.UnitId,
                teamId,
                repairWeapon.Damage,
                evolution.RepairProtectionTurns,
                evolution.RepairRegenerationTurns);
        }

        /// <summary>
        /// Cryo's B branch: a cryo hit landing on a target this attacker already
        /// froze on their previous turn (still within <see cref="CryoChainWindowTurnSequences"/>
        /// turn-sequence steps) deals bonus damage - rewards refreezing the same
        /// target instead of a persistent multi-turn debuff.
        /// </summary>
        private int ResolveFreezeChainDamage(int amount, BattleUnitState unit, bool appliesFreeze, WeaponEvolutionProfile freezeEvolution)
        {
            if (!appliesFreeze || amount <= 0 || unit == null || freezeEvolution.CryoChainDamagePercent <= 0)
            {
                return amount;
            }

            if (_lastFreezeApplicationTurn.TryGetValue(unit.UnitId, out var lastTurn)
                && _turnSequence - lastTurn <= CryoChainWindowTurnSequences)
            {
                return Mathf.CeilToInt(amount * (1f + freezeEvolution.CryoChainDamagePercent / 100f));
            }

            return amount;
        }

        private int ResolveIncomingDamage(BattleUnitState unit, int amount)
        {
            if (unit == null || amount <= 0)
            {
                return 0;
            }

            var resolvedAmount = amount;
            if (_repairEffects.TryGetValue(unit.UnitId, out var effect))
            {
                if (effect.ProtectionTurnsRemaining > 0)
                {
                    resolvedAmount = Mathf.CeilToInt(resolvedAmount * ProtectiveRepairDamageMultiplier);
                }

                if (resolvedAmount > 0)
                {
                    effect.DamageTakenSinceLastCheck = true;
                }
            }

            if (_poisonEffects.TryGetValue(unit.UnitId, out var poison)
                && poison.ArmorWeakenTurnsRemaining > 0
                && poison.ArmorWeakenPercent > 0)
            {
                resolvedAmount = Mathf.CeilToInt(resolvedAmount * (1f + poison.ArmorWeakenPercent / 100f));
            }

            return Mathf.Max(0, resolvedAmount);
        }

        private void ApplyPoison(BattleUnitState unit, BattleTeamId sourceTeam, WeaponEvolutionProfile evolution)
        {
            if (unit == null || unit.IsDefeated)
            {
                return;
            }

            var duration = ToxicBaseDurationTurns + evolution.ToxicDurationBonusTurns;
            if (_poisonEffects.TryGetValue(unit.UnitId, out var existing))
            {
                existing.SourceTeam = sourceTeam;
                existing.DamagePerTurn = Mathf.Max(existing.DamagePerTurn, ToxicBaseDamagePerTurn);
                existing.TurnsRemaining = Mathf.Max(existing.TurnsRemaining, duration);
                existing.ArmorWeakenPercent = Mathf.Max(existing.ArmorWeakenPercent, evolution.ToxicArmorWeakenPercent);
                existing.ArmorWeakenTurnsRemaining = Mathf.Max(existing.ArmorWeakenTurnsRemaining, evolution.ToxicArmorWeakenTurns);
                existing.MovementDamagePerSecond = Mathf.Max(existing.MovementDamagePerSecond, evolution.ToxicMovementDamagePerSecond);
                return;
            }

            _poisonEffects[unit.UnitId] = new ActivePoisonEffect(
                unit.UnitId,
                sourceTeam,
                ToxicBaseDamagePerTurn,
                duration,
                evolution.ToxicArmorWeakenPercent,
                evolution.ToxicArmorWeakenTurns,
                evolution.ToxicMovementDamagePerSecond);
        }

        private void ProcessPoisonForActiveTurn()
        {
            if (_poisonEffects.Count == 0)
            {
                return;
            }

            var activeTeam = _state.Turn.ActiveTeam;
            var completed = new List<string>();
            foreach (var pair in _poisonEffects)
            {
                var effect = pair.Value;
                var unit = FindUnit(effect.UnitId);
                if (unit == null || unit.IsDefeated)
                {
                    completed.Add(pair.Key);
                    continue;
                }

                if (unit.TeamId != activeTeam)
                {
                    continue;
                }

                if (effect.TurnsRemaining > 0)
                {
                    if (effect.DamagePerTurn > 0)
                    {
                        ApplyDamage(unit, effect.DamagePerTurn, PoisonDamageSourceId, effect.SourceTeam);
                    }

                    effect.TurnsRemaining--;
                    if (effect.TurnsRemaining <= 0)
                    {
                        effect.MovementDamagePerSecond = 0f;
                    }
                }

                if (effect.ArmorWeakenTurnsRemaining > 0)
                {
                    effect.ArmorWeakenTurnsRemaining--;
                }

                if (effect.TurnsRemaining <= 0 && effect.ArmorWeakenTurnsRemaining <= 0)
                {
                    completed.Add(pair.Key);
                }
            }

            foreach (var unitId in completed)
            {
                _poisonEffects.Remove(unitId);
            }
        }

        private void StepPoisonMovementDamage(float deltaTime)
        {
            if (_poisonEffects.Count == 0)
            {
                return;
            }

            foreach (var pair in _poisonEffects)
            {
                var effect = pair.Value;
                if (effect.MovementDamagePerSecond <= 0f)
                {
                    continue;
                }

                var unit = FindUnit(effect.UnitId);
                if (unit == null || unit.IsDefeated)
                {
                    continue;
                }

                var motor = GetMotor(effect.UnitId);
                if (motor == null || motor.IsStable)
                {
                    continue;
                }

                effect.MovementSecondsAccumulator += deltaTime;
                while (effect.MovementSecondsAccumulator >= PoisonMovementDamageTickSeconds)
                {
                    effect.MovementSecondsAccumulator -= PoisonMovementDamageTickSeconds;
                    var damage = Mathf.Max(1, Mathf.RoundToInt(effect.MovementDamagePerSecond));
                    ApplyDamage(unit, damage, PoisonDamageSourceId, effect.SourceTeam);
                }
            }
        }

        private void ProcessRepairEffectsForActiveTurn()
        {
            if (_repairEffects.Count == 0)
            {
                return;
            }

            var activeTeam = _state.Turn.ActiveTeam;
            var completed = new List<string>();
            foreach (var pair in _repairEffects)
            {
                var effect = pair.Value;
                if (effect.TeamId != activeTeam)
                {
                    continue;
                }

                var unit = FindUnit(effect.UnitId);
                if (unit == null || unit.IsDefeated)
                {
                    completed.Add(pair.Key);
                    continue;
                }

                if (effect.RegenerationChecksRemaining > 0)
                {
                    if (!effect.DamageTakenSinceLastCheck)
                    {
                        var restored = unit.RestoreHealth(effect.HealAmount);
                        if (restored > 0)
                        {
                            HealthRestored?.Invoke(new HealthRestoredEvent(unit.Position, restored, effect.TeamId));
                        }
                    }

                    effect.RegenerationChecksRemaining--;
                    effect.DamageTakenSinceLastCheck = false;
                }

                if (effect.ProtectionTurnsRemaining > 0)
                {
                    effect.ProtectionTurnsRemaining--;
                }

                if (effect.ProtectionTurnsRemaining <= 0 && effect.RegenerationChecksRemaining <= 0)
                {
                    completed.Add(pair.Key);
                }
            }

            foreach (var unitId in completed)
            {
                _repairEffects.Remove(unitId);
            }
        }

        private bool IsActiveUnitDisoriented()
        {
            var activeUnitId = _state.Turn.ActiveUnitId;
            return !string.IsNullOrWhiteSpace(activeUnitId)
                && _disorientations.TryGetValue(activeUnitId, out var effect)
                && effect.FirstAffectedTurnSequence <= _turnSequence
                && effect.TurnsRemaining > 0;
        }

        private void ApplyDisorientation(string unitId, int turns)
        {
            if (string.IsNullOrWhiteSpace(unitId) || turns <= 0)
            {
                return;
            }

            var firstAffectedTurn = _turnSequence + 1;
            if (_disorientations.TryGetValue(unitId, out var existing))
            {
                existing.TurnsRemaining = Mathf.Max(existing.TurnsRemaining, turns);
                existing.FirstAffectedTurnSequence = Mathf.Min(existing.FirstAffectedTurnSequence, firstAffectedTurn);
                return;
            }

            _disorientations[unitId] = new ActiveDisorientation(firstAffectedTurn, turns);
        }

        private void ConsumeDisorientationForActiveTurn()
        {
            var activeUnitId = _state.Turn.ActiveUnitId;
            if (string.IsNullOrWhiteSpace(activeUnitId)
                || !_disorientations.TryGetValue(activeUnitId, out var effect)
                || effect.FirstAffectedTurnSequence > _turnSequence)
            {
                return;
            }

            effect.TurnsRemaining--;
            if (effect.TurnsRemaining <= 0)
            {
                _disorientations.Remove(activeUnitId);
            }
        }

        private bool IsActiveUnitFrozen()
        {
            var activeUnitId = _state.Turn.ActiveUnitId;
            return !string.IsNullOrWhiteSpace(activeUnitId)
                && _freezes.TryGetValue(activeUnitId, out var effect)
                && effect.FirstAffectedTurnSequence <= _turnSequence
                && effect.TurnsRemaining > 0;
        }

        public bool IsUnitFrozen(string unitId)
        {
            return !string.IsNullOrWhiteSpace(unitId)
                && _freezes.TryGetValue(unitId, out var effect)
                && effect.TurnsRemaining > 0;
        }

        public bool IsUnitPoisoned(string unitId)
        {
            return !string.IsNullOrWhiteSpace(unitId)
                && _poisonEffects.TryGetValue(unitId, out var effect)
                && (effect.TurnsRemaining > 0 || effect.ArmorWeakenTurnsRemaining > 0);
        }

        public bool IsUnitLavaBurning(string unitId)
        {
            if (string.IsNullOrWhiteSpace(unitId))
            {
                return false;
            }

            return (_lavaOverlayRemainingSeconds.TryGetValue(unitId, out var secondsRemaining)
                    && secondsRemaining > 0f)
                || (_burnEffects.TryGetValue(unitId, out var burn)
                    && burn.TurnsRemaining > 0);
        }

        public bool IsUnitElectrified(string unitId)
        {
            if (string.IsNullOrWhiteSpace(unitId))
            {
                return false;
            }

            return (_electrifiedOverlayRemainingSeconds.TryGetValue(unitId, out var secondsRemaining)
                    && secondsRemaining > 0f)
                || (_empStuns.TryGetValue(unitId, out var stun)
                    && stun.TurnsRemaining > 0)
                || (_empWeakens.TryGetValue(unitId, out var weaken)
                    && weaken.TurnsRemaining > 0);
        }

        private void ApplyElectrifiedOverlay(string unitId)
        {
            if (!string.IsNullOrWhiteSpace(unitId))
            {
                _electrifiedOverlayRemainingSeconds[unitId] = EmpOverlayDurationSeconds;
            }
        }

        private void StepElectrifiedOverlayStates(float deltaTime)
        {
            if (_electrifiedOverlayRemainingSeconds.Count == 0)
            {
                return;
            }

            var expiredUnitIds = new List<string>();
            foreach (var pair in _electrifiedOverlayRemainingSeconds)
            {
                var secondsRemaining = pair.Value - Mathf.Max(0f, deltaTime);
                if (secondsRemaining <= 0f)
                {
                    expiredUnitIds.Add(pair.Key);
                }
                else
                {
                    _electrifiedOverlayRemainingSeconds[pair.Key] = secondsRemaining;
                }
            }

            foreach (var unitId in expiredUnitIds)
            {
                _electrifiedOverlayRemainingSeconds.Remove(unitId);
            }
        }

        private bool ApplyBurn(
            BattleUnitState unit,
            BattleTeamId sourceTeam,
            WeaponEvolutionProfile evolution,
            out bool wasAlreadyActive)
        {
            wasAlreadyActive = false;
            if (unit == null
                || unit.IsDefeated
                || evolution.FireGunIgniteTurns <= 0
                || evolution.FireGunIgniteDamagePerTurn <= 0)
            {
                return false;
            }

            if (_burnEffects.TryGetValue(unit.UnitId, out var existing))
            {
                wasAlreadyActive = existing.TurnsRemaining > 0;
                if (existing.LastApplicationTurnSequence == _turnSequence)
                {
                    return false;
                }

                existing.SourceTeam = sourceTeam;
                existing.DamagePerTurn = Mathf.Max(existing.DamagePerTurn, evolution.FireGunIgniteDamagePerTurn);
                existing.TurnsRemaining = Mathf.Max(existing.TurnsRemaining, evolution.FireGunIgniteTurns);
                existing.LastApplicationTurnSequence = _turnSequence;
                return true;
            }

            _burnEffects[unit.UnitId] = new ActiveBurnEffect(
                unit.UnitId,
                sourceTeam,
                evolution.FireGunIgniteDamagePerTurn,
                evolution.FireGunIgniteTurns,
                _turnSequence);
            return true;
        }

        private void ProcessBurnForActiveTurn()
        {
            if (_burnEffects.Count == 0)
            {
                return;
            }

            var activeTeam = _state.Turn.ActiveTeam;
            var expiredUnitIds = new List<string>();
            foreach (var pair in _burnEffects)
            {
                var burn = pair.Value;
                var unit = FindUnit(burn.UnitId);
                if (unit == null || unit.IsDefeated)
                {
                    expiredUnitIds.Add(pair.Key);
                    continue;
                }

                if (unit.TeamId != activeTeam)
                {
                    continue;
                }

                ApplyDamage(unit, burn.DamagePerTurn, BurnDamageSourceId, burn.SourceTeam);
                burn.TurnsRemaining--;
                if (burn.TurnsRemaining <= 0)
                {
                    expiredUnitIds.Add(pair.Key);
                }
            }

            foreach (var unitId in expiredUnitIds)
            {
                _burnEffects.Remove(unitId);
            }
        }

        private void StepLavaOverlayStates(float deltaTime)
        {
            if (_lavaOverlayRemainingSeconds.Count == 0)
            {
                return;
            }

            var expiredUnitIds = new List<string>();
            foreach (var pair in _lavaOverlayRemainingSeconds)
            {
                var secondsRemaining = pair.Value - Mathf.Max(0f, deltaTime);
                if (secondsRemaining <= 0f)
                {
                    expiredUnitIds.Add(pair.Key);
                }
                else
                {
                    _lavaOverlayRemainingSeconds[pair.Key] = secondsRemaining;
                }
            }

            foreach (var unitId in expiredUnitIds)
            {
                _lavaOverlayRemainingSeconds.Remove(unitId);
            }
        }

        private void ApplyFreeze(string unitId, int turns)
        {
            if (string.IsNullOrWhiteSpace(unitId) || turns <= 0)
            {
                return;
            }

            var firstAffectedTurn = _turnSequence + 1;
            if (_freezes.TryGetValue(unitId, out var existing))
            {
                existing.TurnsRemaining = Mathf.Max(existing.TurnsRemaining, turns);
                existing.FirstAffectedTurnSequence = Mathf.Min(existing.FirstAffectedTurnSequence, firstAffectedTurn);
                return;
            }

            _freezes[unitId] = new ActiveDisorientation(firstAffectedTurn, turns);
        }

        private void ConsumeFreezeForActiveTurn()
        {
            var activeUnitId = _state.Turn.ActiveUnitId;
            if (string.IsNullOrWhiteSpace(activeUnitId)
                || !_freezes.TryGetValue(activeUnitId, out var effect)
                || effect.FirstAffectedTurnSequence > _turnSequence)
            {
                return;
            }

            effect.TurnsRemaining--;
            if (effect.TurnsRemaining <= 0)
            {
                _freezes.Remove(activeUnitId);
            }
        }

        private bool IsActiveUnitEmpStunned()
        {
            var activeUnitId = _state.Turn.ActiveUnitId;
            return !string.IsNullOrWhiteSpace(activeUnitId)
                && _empStuns.TryGetValue(activeUnitId, out var effect)
                && effect.FirstAffectedTurnSequence <= _turnSequence
                && effect.TurnsRemaining > 0;
        }

        private void ApplyEmpStun(string unitId, int turns)
        {
            if (string.IsNullOrWhiteSpace(unitId) || turns <= 0)
            {
                return;
            }

            var firstAffectedTurn = _turnSequence + 1;
            if (_empStuns.TryGetValue(unitId, out var existing))
            {
                existing.TurnsRemaining = Mathf.Max(existing.TurnsRemaining, turns);
                existing.FirstAffectedTurnSequence = Mathf.Min(existing.FirstAffectedTurnSequence, firstAffectedTurn);
                return;
            }

            _empStuns[unitId] = new ActiveDisorientation(firstAffectedTurn, turns);
        }

        private void ConsumeEmpStunForActiveTurn()
        {
            var activeUnitId = _state.Turn.ActiveUnitId;
            if (string.IsNullOrWhiteSpace(activeUnitId)
                || !_empStuns.TryGetValue(activeUnitId, out var effect)
                || effect.FirstAffectedTurnSequence > _turnSequence)
            {
                return;
            }

            effect.TurnsRemaining--;
            if (effect.TurnsRemaining <= 0)
            {
                _empStuns.Remove(activeUnitId);
            }
        }

        private void ApplyEmpWeaken(string unitId, int damagePercent, int turns)
        {
            if (string.IsNullOrWhiteSpace(unitId) || damagePercent <= 0 || turns <= 0)
            {
                return;
            }

            if (_empWeakens.TryGetValue(unitId, out var existing))
            {
                existing.DamagePercent = Mathf.Max(existing.DamagePercent, damagePercent);
                existing.TurnsRemaining = Mathf.Max(existing.TurnsRemaining, turns);
                return;
            }

            _empWeakens[unitId] = new ActiveEmpWeaken(damagePercent, turns);
        }

        private void ProcessEmpWeakenForActiveTurn()
        {
            if (_empWeakens.Count == 0)
            {
                return;
            }

            var activeTeam = _state.Turn.ActiveTeam;
            var completed = new List<string>();
            foreach (var pair in _empWeakens)
            {
                var unit = FindUnit(pair.Key);
                if (unit == null || unit.IsDefeated)
                {
                    completed.Add(pair.Key);
                    continue;
                }

                if (unit.TeamId != activeTeam)
                {
                    continue;
                }

                var effect = pair.Value;
                effect.TurnsRemaining--;
                if (effect.TurnsRemaining <= 0)
                {
                    completed.Add(pair.Key);
                }
            }

            foreach (var unitId in completed)
            {
                _empWeakens.Remove(unitId);
            }
        }

        private BattleUnitState FindUnit(string unitId)
        {
            foreach (var team in _state.Teams)
            {
                foreach (var unit in team.Units)
                {
                    if (unit != null && unit.UnitId == unitId)
                    {
                        return unit;
                    }
                }
            }

            return null;
        }

        private void ApplyDamage(
            BattleUnitState target,
            int amount,
            string sourceId,
            BattleTeamId sourceTeam,
            bool canBeProtected = true)
        {
            if (target == null || target.IsDefeated || amount <= 0)
            {
                return;
            }

            var resolvedAmount = canBeProtected ? ResolveIncomingDamage(target, amount) : amount;
            if (resolvedAmount <= 0)
            {
                return;
            }

            var damageEvent = target.ApplyDamage(resolvedAmount, sourceId);
            DamageApplied?.Invoke(sourceTeam, damageEvent);
        }

        private bool ApplyTerrainDestruction(Vector2 impactPoint, WeaponDefinition impactWeapon, BattleTeamId sourceTeam)
        {
            if (_terrain == null || impactWeapon == null)
            {
                return false;
            }

            var craterRadius = Mathf.Clamp(impactWeapon.ImpactRadius * 0.62f, 0.38f, 1.55f);
            return ApplyTerrainCrater(impactPoint, craterRadius, sourceTeam);
        }

        private bool ApplyTerrainCrater(
            Vector2 point,
            float radius,
            BattleTeamId sourceTeam,
            TerrainMaterial maximumMaterial = TerrainMaterial.Titanium)
        {
            if (_terrain == null)
            {
                return false;
            }

            var destroyedCellCount = _terrain.ApplyCraterAndGetDestroyedCellCount(point, radius, maximumMaterial);
            if (destroyedCellCount <= 0)
            {
                return false;
            }

            TerrainCellsDestroyed?.Invoke(sourceTeam, destroyedCellCount);
            TerrainCraterApplied?.Invoke(sourceTeam, point, radius);
            return true;
        }

        private BattleTeamId ResolveTeamForOwner(string ownerUnitId)
        {
            if (string.IsNullOrWhiteSpace(ownerUnitId))
            {
                return BattleTeamId.None;
            }

            foreach (var team in _state.Teams)
            {
                foreach (var unit in team.Units)
                {
                    if (unit != null && unit.UnitId == ownerUnitId)
                    {
                        return team.TeamId;
                    }
                }
            }

            return BattleTeamId.None;
        }

        private void ApplyExplosionKnockback(
            Vector2 impactPoint,
            Vector2 impactVelocity,
            WeaponDefinition impactWeapon,
            bool terrainChanged,
            float knockbackMultiplier = 1f,
            BattleTeamId ignoredTeam = BattleTeamId.None)
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
                    if (team.TeamId == ignoredTeam
                        || unit == null
                        || unit.IsDefeated
                        || Mathf.Abs(unit.Position.x - impactPoint.x) > radius)
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

                    velocity *= Mathf.Max(0f, knockbackMultiplier);

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
            ConsumeDisorientationForActiveTurn();
            ConsumeFreezeForActiveTurn();
            ConsumeEmpStunForActiveTurn();
            _turnService.AdvanceTurn(_state);
            _turnSequence++;
            ResetMotionForNewTurn();
            ProcessRepairEffectsForActiveTurn();
            ProcessPoisonForActiveTurn();
            ProcessBurnForActiveTurn();
            ProcessEmpWeakenForActiveTurn();
            TurnAdvanced?.Invoke(_state.Turn.ActiveTeam, TurnAdvanceReason.PlayerTimerExpired);
            UnitsMoved?.Invoke();
        }

        private void StepPlayerTurnTimer(float tickSeconds)
        {
            // HasPendingGroundAction: a burning C4 fuse must resolve within the
            // owner's turn - if the timer expired mid-fuse the turn would pass
            // to the bot while TryFire stays blocked, deadlocking its turn.
            if (_playerTurnTimerPaused
                || _state.Turn.ActiveTeam != BattleTeamId.Player
                || _state.Turn.ActionSpent
                || HasProjectilesInFlight
                || HasPendingGroundAction
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

            if (!_ropeTraversal.StepHook(_terrain, tickSeconds))
            {
                _ropeTraversal = null;
                return;
            }

            if (_ropeTraversal.IsAttached)
            {
                _ropeTraversal.Attach(GetMotor(_ropeTraversal.OwnerUnitId));
            }
        }

        public void Spawn(in ProjectileSpawnRequest request)
        {
            if (request.Definition == null)
            {
                return;
            }

            var spawnedWeapon = ResolveWeaponFromProjectileId(request.Definition.ProjectileId);
            var sourceOrigin = spawnedWeapon?.WeaponId == SwapJumpWeaponId
                ? _state.GetUnit(request.OwnerUnitId)?.Position
                : (Vector2?)null;
            var projectile = new SimProjectile(
                ++_nextProjectileId,
                request.Definition,
                request.OwnerUnitId,
                request.Position,
                request.Velocity,
                request.Definition.TimeoutSeconds,
                sourceOrigin: sourceOrigin);
            if (spawnedWeapon?.WeaponId == BurRocketWeaponId)
            {
                var evolution = ResolveEvolutionFor(ResolveTeamForOwner(request.OwnerUnitId), spawnedWeapon);
                var duration = evolution.RocketDrillDurationSeconds > 0f
                    ? evolution.RocketDrillDurationSeconds
                    : DrillRocketTerrainDurationSeconds;
                projectile.ConfigureTerrainDrill(duration, evolution.RocketDrillPenetration);
            }
            else if (spawnedWeapon?.WeaponId == MissileWeaponId
                && !IsMissileCluster(request.Definition.ProjectileId))
            {
                var evolution = ResolveEvolutionFor(ResolveTeamForOwner(request.OwnerUnitId), spawnedWeapon);
                projectile.ConfigureMissileEvolution(
                    evolution.MissileSplitChargeCount,
                    evolution.MissileObstacleBypassCount);
            }

            _projectiles.Add(projectile);
            ProjectileSpawned?.Invoke(projectile);
        }

        public void Schedule(in ProjectileSpawnRequest request, float delaySeconds)
        {
            if (request.Definition == null)
            {
                return;
            }

            if (delaySeconds <= 0f)
            {
                Spawn(request);
                return;
            }

            _scheduledProjectileSpawns.Add(new ScheduledProjectileSpawn(request, delaySeconds));
        }

        private void StepScheduledProjectileSpawns(float deltaTime)
        {
            for (var i = _scheduledProjectileSpawns.Count - 1; i >= 0; i--)
            {
                var scheduled = _scheduledProjectileSpawns[i];
                scheduled.RemainingDelay -= deltaTime;
                if (scheduled.RemainingDelay > 0f)
                {
                    continue;
                }

                _scheduledProjectileSpawns.RemoveAt(i);
                Spawn(scheduled.Request);
            }
        }

        private void StepProjectiles(float deltaTime)
        {
            for (var i = _projectiles.Count - 1; i >= 0; i--)
            {
                var projectile = _projectiles[i];
                var projectileWeapon = ResolveWeaponFromProjectileId(projectile.Definition.ProjectileId);
                var isDrillRocket = projectileWeapon?.WeaponId == BurRocketWeaponId;
                var isControlledDrone = projectile.Id == _controlledProjectileId;
                var grenadeEvolution = projectileWeapon?.WeaponId == GrenadeWeaponId
                    ? ResolveEvolutionFor(ResolveTeamForOwner(projectile.OwnerUnitId), projectileWeapon)
                    : WeaponEvolutionProfile.Base;
                var isStickyGrenade = grenadeEvolution.GrenadeSticks;

                if (projectile.IsRestingFuse)
                {
                    UpdateStickyGrenadeAttachment(projectile);
                    if (grenadeEvolution.GrenadeGravityPullRadius > 0f)
                    {
                        ApplyGravityGrenadePull(
                            projectile,
                            grenadeEvolution.GrenadeGravityPullRadius,
                            grenadeEvolution.GrenadePullAcceleration,
                            deltaTime);
                    }

                    projectile.ReduceRestingFuse(deltaTime);
                    if (projectile.RestingFuseRemaining <= 0f)
                    {
                        ResolveProjectileAt(i, projectile.Position);
                    }

                    continue;
                }

                var isMissile = projectileWeapon?.WeaponId == MissileWeaponId
                    && !IsMissileCluster(projectile.Definition.ProjectileId);
                if (!isMissile)
                {
                    projectile.ReduceLifetime(deltaTime);
                    if (projectile.RemainingLifetime <= 0f)
                    {
                        ResolveProjectileAt(i, projectile.Position);
                        continue;
                    }
                }

                // The homing missile mirrors the reference: a short ballistic
                // free-flight phase, then gravity switches off and it steers
                // toward the target at constant speed.
                var isHomingMissile = isMissile;
                var isBallisticSplitMissile = isHomingMissile && projectile.MissileSplitChargeCount > 0;
                var homingActive = isHomingMissile;

                var velocity = isControlledDrone
                    ? _controlledProjectileInput * Mathf.Max(0.1f, projectileWeapon?.MaxPower ?? 1f)
                        + projectile.ExternalVelocity
                    : projectile.Velocity;
                if (!isControlledDrone && !homingActive)
                {
                    // Gravity also keeps acting inside terrain for the drill
                    // rocket, so its tunnels curve like in the reference.
                    velocity += _projectileGravityPerScale * projectile.Definition.GravityScale * deltaTime;
                }

                if (grenadeEvolution.GrenadeMagneticSeekRadius > 0f
                    && TryFindMagneticGrenadeTarget(
                        projectile,
                        grenadeEvolution.GrenadeMagneticSeekRadius,
                        out var magneticTarget))
                {
                    velocity = SteerGrenadeToward(velocity, projectile.Position, magneticTarget, deltaTime);
                }

                if (homingActive)
                {
                    velocity = isBallisticSplitMissile
                        ? ApplyBallisticMissileGuidance(projectile, velocity, deltaTime)
                        : ApplyProjectileSteering(projectile, velocity, deltaTime);
                }
                projectile.SetVelocity(velocity);
                if (isControlledDrone)
                {
                    projectile.DampenExternalVelocity(deltaTime, GravityGrenadeExternalVelocityDamping);
                }

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

                    // The owner is only shielded until the projectile clears
                    // their hitbox once; after that a clipping shot hurts them
                    // too (reference behaviour).
                    if (!projectile.HasClearedOwner
                        && !IsTouchingUnit(position, projectile.Definition.Radius, projectile.OwnerUnitId))
                    {
                        projectile.MarkClearedOwner();
                    }

                    var ignoredUnitId = projectile.HasClearedOwner ? null : projectile.OwnerUnitId;

                    if (isStickyGrenade
                        && TryFindHitUnit(
                            position,
                            projectile.Definition.Radius,
                            ignoredUnitId,
                            out var stuckUnit,
                            out var stickyHitPoint))
                    {
                        projectile.MoveTo(stickyHitPoint);
                        projectile.StartRestingFuse(stickyHitPoint, projectile.RemainingLifetime);
                        projectile.AttachToUnit(stuckUnit.UnitId, stuckUnit.Position);
                        break;
                    }

                    if (isBallisticSplitMissile
                        && projectile.IsBallisticMissileDescending
                        && TryFindTargetFor(projectile.OwnerUnitId, out var ballisticTarget)
                        && (ballisticTarget.Position - position).sqrMagnitude
                            <= BallisticMissileSplitDistance * BallisticMissileSplitDistance)
                    {
                        SplitBallisticMissile(i, projectile, projectileWeapon);
                        break;
                    }

                    if (isDrillRocket
                        && TryHitUnit(position, projectile.Definition.Radius, ignoredUnitId, out var drillHitPoint))
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
                        }

                        projectile.ConsumeTerrainDrillTime(deltaTime / stepCount);
                        // Carving uploads the destructible texture to the GPU;
                        // spacing the craters out keeps the tunnel continuous
                        // (radius >> spacing) without a hitch every sub-step.
                        if (projectile.ShouldCarveTunnelAt(position, DrillCarveSpacing))
                        {
                            ApplyTerrainCrater(
                                position,
                                DrillRocketTunnelRadius,
                                ResolveTeamForOwner(projectile.OwnerUnitId),
                                projectile.TerrainDrillPenetration);
                        }

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
                        if (isStickyGrenade)
                        {
                            projectile.StartRestingFuse(position - step, projectile.RemainingLifetime);
                            break;
                        }

                        if (TryBounceProjectile(projectile, position - step, velocity))
                        {
                            break;
                        }

                        if (projectileWeapon?.WeaponId == GrenadeWeaponId)
                        {
                            projectile.StartRestingFuse(position - step, projectile.RemainingLifetime);
                            break;
                        }

                        // Resolve at the last free sub-step point, matching the
                        // reference which explodes at the pre-collision position:
                        // craters bite the surface and radial damage is not
                        // attenuated by originating inside solid ground.
                        ResolveProjectileAt(i, position - step);
                        break;
                    }

                    if (isDrillRocket && projectile.IsTraversingTerrain)
                    {
                        projectile.ExitTerrain();
                    }

                    var ignoresUnitsInFlight = projectileWeapon?.WeaponId == GrenadeWeaponId
                        || projectileWeapon?.WeaponId == TeleportWeaponId
                        || projectileWeapon?.WeaponId == SwapJumpWeaponId
                        || projectile.Definition.ProjectileId == MineDeployProjectileId;
                    if (!isDrillRocket
                        && !ignoresUnitsInFlight
                        && TryHitUnit(position, projectile.Definition.Radius, ignoredUnitId, out var hitPoint))
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
                return _terrain.Grid.GetMaterialWorld(position) <= projectile.TerrainDrillPenetration;
            }

            var direction = velocity.sqrMagnitude > 0.01f ? velocity.normalized : Vector2.right;
            var normal = new Vector2(-direction.y, direction.x);
            var sampleDistance = DrillRocketTunnelRadius * 1.15f;
            return IsDrillableTerrainAt(position + direction * sampleDistance, projectile.TerrainDrillPenetration)
                || IsDrillableTerrainAt(position + normal * sampleDistance, projectile.TerrainDrillPenetration)
                || IsDrillableTerrainAt(position - normal * sampleDistance, projectile.TerrainDrillPenetration);
        }

        private bool IsDrillableTerrainAt(Vector2 position, TerrainMaterial penetration)
        {
            return _terrain != null
                && _terrain.Grid.IsSolidWorld(position)
                && _terrain.Grid.GetMaterialWorld(position) <= penetration;
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
                projectile.Definition.ImpactRadius,
                projectile.Definition.KnockbackMultiplier,
                projectile.SourceOrigin);

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
            var evolution = ResolveEvolutionFor(teamId, weapon);
            var mine = new ActiveMine(
                projectile.Id,
                projectile.OwnerUnitId,
                teamId,
                weapon,
                point + Vector2.up * 0.04f,
                evolution.MineTriggersEnemiesOnly,
                evolution.MineAlliesImmuneToExplosion);
            _activeMines.Add(mine);
            MineDeployed?.Invoke(new MineDeployedEvent(mine.Id, mine.Position, weapon, teamId));
            ImpactResolved?.Invoke(new ImpactResolvedEvent(point, weapon, false));
            UnitsMoved?.Invoke();
            if (!HasProjectilesInFlight)
            {
                _activeShotEndsTurn = true;
            }

            // The mine sequence remains open only for the multi-mine evolution.
            if (_state.Turn.ActiveUnitId == projectile.OwnerUnitId && !_state.Turn.ActionSpent)
            {
                if (teamId == BattleTeamId.Opponent)
                {
                    CompleteInstantAction();
                }
                else if (!CanContinueMineDeploymentThisTurn())
                {
                    _playerMovement.CapRemaining(RetreatSeconds);
                }
            }
        }

        private void SplitBallisticMissile(
            int projectileIndex,
            SimProjectile projectile,
            WeaponDefinition weapon)
        {
            var chargeCount = Mathf.Max(2, projectile.MissileSplitChargeCount);
            _projectiles.RemoveAt(projectileIndex);
            var damage = Mathf.Max(1, Mathf.CeilToInt(weapon.Damage / (float)chargeCount));
            var impactRadius = weapon.ImpactRadius * 0.58f;
            var definition = ProjectileDefinition.CreateRuntime(
                MissileClusterProjectilePrefix + chargeCount,
                weapon.ProjectileRadius * 0.6f,
                weapon.GravityScale,
                3f,
                damage,
                impactRadius);

            for (var index = 0; index < chargeCount; index++)
            {
                var t = chargeCount > 1 ? index / (float)(chargeCount - 1) : 0.5f;
                var angle = Mathf.Lerp(-90f - MissileClusterSpreadDegrees, -90f + MissileClusterSpreadDegrees, t)
                    * Mathf.Deg2Rad;
                var direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
                Spawn(new ProjectileSpawnRequest(
                    definition,
                    projectile.OwnerUnitId,
                    projectile.Position + direction * 0.08f,
                    direction * MissileClusterSpeed,
                    definition.Radius * 2f,
                    ProjectileWeaponEffect.GetProjectileColor(weapon.Role)));
            }
        }

        private Vector2 ApplyBallisticMissileGuidance(SimProjectile projectile, Vector2 velocity, float deltaTime)
        {
            if (velocity.sqrMagnitude < 0.01f || !TryFindTargetFor(projectile.OwnerUnitId, out var target))
            {
                return velocity;
            }

            var speed = velocity.magnitude;
            if (projectile.IsBallisticMissileDescending)
            {
                var horizontal = Mathf.MoveTowards(velocity.x, target.Position.x - projectile.Position.x, BallisticMissileHorizontalSteerSpeed * deltaTime);
                return new Vector2(horizontal, -Mathf.Max(speed, MissileClusterSpeed));
            }

            var apexY = target.Position.y + 12f;
            if (projectile.Position.y >= apexY)
            {
                projectile.BeginBallisticMissileDescent();
                projectile.MoveTo(new Vector2(target.Position.x, apexY));
                return Vector2.down * Mathf.Max(speed, MissileClusterSpeed);
            }

            return Vector2.up * Mathf.Max(speed, MissileClusterSpeed);
        }

        private Vector2 ApplyProjectileSteering(SimProjectile projectile, Vector2 velocity, float deltaTime)
        {
            if (velocity.sqrMagnitude < 0.01f)
            {
                return velocity;
            }

            if (!TryFindTargetFor(projectile.OwnerUnitId, out var target))
            {
                return velocity;
            }

            var guidancePoint = ResolveMissileGuidancePoint(projectile, target.Position);
            var desired = guidancePoint - projectile.Position;
            if (desired.sqrMagnitude < 0.01f)
            {
                return velocity;
            }

            var speed = velocity.magnitude;
            var currentAngle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
            var desiredAngle = Mathf.Atan2(desired.y, desired.x) * Mathf.Rad2Deg;
            var steeredAngle = Mathf.MoveTowardsAngle(currentAngle, desiredAngle, MissileSteerDegreesPerSecond * deltaTime);
            var angleRad = steeredAngle * Mathf.Deg2Rad;
            return new Vector2(Mathf.Cos(angleRad), Mathf.Sin(angleRad)) * speed;
        }

        private Vector2 ResolveMissileGuidancePoint(SimProjectile projectile, Vector2 targetPoint)
        {
            if (projectile.HasMissileWaypoint)
            {
                if ((projectile.MissileWaypoint - projectile.Position).sqrMagnitude
                    > MissileDetourWaypointArrivalDistance * MissileDetourWaypointArrivalDistance)
                {
                    return projectile.MissileWaypoint;
                }

                projectile.ClearMissileWaypoint();
            }

            if (projectile.MissileObstacleBypassesRemaining > 0
                && TryFindTerrainOnLine(projectile.Position, targetPoint, out var obstaclePoint)
                && TryBuildMissileDetourWaypoint(projectile.Position, targetPoint, obstaclePoint, out var waypoint))
            {
                projectile.TrySetMissileWaypoint(waypoint);
                return waypoint;
            }

            return targetPoint;
        }

        private bool TryFindTerrainOnLine(Vector2 start, Vector2 end, out Vector2 hitPoint)
        {
            hitPoint = default;
            if (_terrain == null)
            {
                return false;
            }

            var delta = end - start;
            var distance = delta.magnitude;
            if (distance <= MissileDetourProbeStep)
            {
                return false;
            }

            var steps = Mathf.CeilToInt(distance / MissileDetourProbeStep);
            var step = delta / steps;
            for (var index = 1; index <= steps; index++)
            {
                var point = start + step * index;
                if (_terrain.Grid.IsSolidWorld(point))
                {
                    hitPoint = point;
                    return true;
                }
            }

            return false;
        }

        private bool IsMineTriggerTargetInside(ActiveMine mine)
        {
            foreach (var team in _state.Teams)
            {
                if (mine.TriggersEnemiesOnly && team.TeamId == mine.TeamId)
                {
                    continue;
                }

                foreach (var unit in team.Units)
                {
                    if (unit != null
                        && !unit.IsDefeated
                        && Vector2.Distance(unit.Position, mine.Position) <= mine.Weapon.ImpactRadius)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private bool TryBuildMissileDetourWaypoint(
            Vector2 start,
            Vector2 target,
            Vector2 obstaclePoint,
            out Vector2 waypoint)
        {
            waypoint = default;
            if (_terrain == null)
            {
                return false;
            }

            var forward = Mathf.Sign(target.x - start.x);
            if (Mathf.Abs(forward) < 0.01f)
            {
                forward = 1f;
            }

            var clearancePoint = obstaclePoint;
            for (var clearance = MissileDetourClearance;
                 clearance <= 8f;
                 clearance += MissileDetourClearance)
            {
                var candidate = obstaclePoint + Vector2.up * clearance;
                if (_terrain.Grid.IsSolidWorld(candidate))
                {
                    continue;
                }

                clearancePoint = candidate + Vector2.up * MissileDetourClearance;
                waypoint = new Vector2(clearancePoint.x - forward * MissileDetourClearance, clearancePoint.y);
                return !_terrain.Grid.IsSolidWorld(waypoint);
            }

            return false;
        }

        private bool TryBounceProjectile(SimProjectile projectile, Vector2 safePosition, Vector2 velocity)
        {
            var weapon = ResolveWeaponFromProjectileId(projectile.Definition.ProjectileId);
            var terrainBounceLimit = projectile.Definition.TerrainBounceLimit;
            var usesDefaultBounceRules = weapon != null
                && (weapon.WeaponId == TeleportWeaponId || weapon.WeaponId == GrenadeWeaponId);
            if (!usesDefaultBounceRules && terrainBounceLimit <= 0)
            {
                return false;
            }

            var maxBounces = terrainBounceLimit > 0 ? terrainBounceLimit : MaxBounces;
            if (projectile.BounceCount >= maxBounces || velocity.magnitude < RestingFuseMinSpeed)
            {
                return false;
            }

            var reflected = ReflectOffTerrain(safePosition, velocity, projectile.Definition.Radius);
            projectile.RegisterBounce();
            projectile.MoveTo(safePosition);
            projectile.SetVelocity(reflected * BounceSpeedRetain);
            return true;
        }

        /// <summary>
        /// Reflects a bouncing projectile by sampling which sides of the contact
        /// point are solid (left/right/top/bottom), mirroring the reference
        /// grenade model: each axis is inverted independently based on the hit
        /// sides, so corner and slope bounces behave naturally instead of
        /// guessing from the dominant velocity component.
        /// </summary>
        private Vector2 ReflectOffTerrain(Vector2 contactPoint, Vector2 velocity, float projectileRadius)
        {
            var reflected = velocity;
            if (_terrain == null)
            {
                return new Vector2(velocity.x, -velocity.y);
            }

            var sample = Mathf.Max(projectileRadius, 0.06f) + ProjectileSubStepLength;
            var grid = _terrain.Grid;
            var hitLeft = grid.IsSolidWorld(contactPoint + Vector2.left * sample);
            var hitRight = grid.IsSolidWorld(contactPoint + Vector2.right * sample);
            var hitTop = grid.IsSolidWorld(contactPoint + Vector2.up * sample);
            var hitBottom = grid.IsSolidWorld(contactPoint + Vector2.down * sample);

            if (hitLeft && !hitRight)
            {
                reflected.x = Mathf.Abs(reflected.x);
            }
            else if (hitRight && !hitLeft)
            {
                reflected.x = -Mathf.Abs(reflected.x);
            }

            if (hitBottom && !hitTop)
            {
                reflected.y = Mathf.Abs(reflected.y);
            }
            else if (hitTop && !hitBottom)
            {
                reflected.y = -Mathf.Abs(reflected.y);
            }

            if (reflected == velocity)
            {
                // Corner or thin sliver the axis probes missed: fall back to a
                // velocity-based reflection so the projectile never tunnels.
                reflected = Mathf.Abs(velocity.y) >= Mathf.Abs(velocity.x)
                    ? new Vector2(velocity.x, -velocity.y)
                    : new Vector2(-velocity.x, velocity.y);
            }

            return reflected;
        }

        private void UpdateStickyGrenadeAttachment(SimProjectile projectile)
        {
            if (!projectile.IsAttachedToUnit)
            {
                return;
            }

            foreach (var team in _state.Teams)
            {
                foreach (var unit in team.Units)
                {
                    if (unit.UnitId != projectile.AttachedUnitId)
                    {
                        continue;
                    }

                    if (!unit.IsDefeated)
                    {
                        projectile.FollowAttachedUnit(unit.Position);
                        return;
                    }
                }
            }

            projectile.DetachFromUnit();
        }

        private void ApplyGravityGrenadePull(
            SimProjectile source,
            float radius,
            float acceleration,
            float deltaTime)
        {
            if (radius <= 0f || acceleration <= 0f)
            {
                return;
            }

            var center = source.Position;
            var radiusSquared = radius * radius;
            foreach (var team in _state.Teams)
            {
                foreach (var unit in team.Units)
                {
                    if (unit == null || unit.IsDefeated)
                    {
                        continue;
                    }

                    var offset = center - unit.Position;
                    var distanceSquared = offset.sqrMagnitude;
                    if (distanceSquared > radiusSquared || distanceSquared < GravityGrenadeMinimumDistance * GravityGrenadeMinimumDistance)
                    {
                        continue;
                    }

                    var falloff = 1f - Mathf.Sqrt(distanceSquared) / radius;
                    var impulse = offset.normalized * (acceleration * Mathf.SmoothStep(0f, 1f, falloff) * deltaTime);
                    GetMotor(unit.UnitId)?.AddImpulse(impulse);
                }
            }

            foreach (var projectile in _projectiles)
            {
                if (projectile == source || projectile.IsRestingFuse)
                {
                    continue;
                }

                var offset = center - projectile.Position;
                var distanceSquared = offset.sqrMagnitude;
                if (distanceSquared > radiusSquared || distanceSquared < GravityGrenadeMinimumDistance * GravityGrenadeMinimumDistance)
                {
                    continue;
                }

                var falloff = 1f - Mathf.Sqrt(distanceSquared) / radius;
                var pullVelocity = offset.normalized
                    * (acceleration * GravityGrenadeProjectilePullScale * Mathf.SmoothStep(0f, 1f, falloff) * deltaTime);
                if (projectile.Id == _controlledProjectileId)
                {
                    projectile.AddExternalVelocity(pullVelocity);
                }
                else
                {
                    projectile.SetVelocity(projectile.Velocity + pullVelocity);
                }
            }
        }

        private Vector2 SteerGrenadeToward(Vector2 velocity, Vector2 position, Vector2 target, float deltaTime)
        {
            var desired = target - position;
            if (desired.sqrMagnitude < 0.0001f || velocity.sqrMagnitude < 0.0001f)
            {
                return velocity;
            }

            return Vector2.MoveTowards(
                velocity,
                desired.normalized * velocity.magnitude,
                MagneticGrenadeSteerAcceleration * deltaTime);
        }

        private bool TryFindMagneticGrenadeTarget(SimProjectile projectile, float radius, out Vector2 target)
        {
            target = default;
            var bestDistanceSquared = radius * radius;
            foreach (var team in _state.Teams)
            {
                foreach (var unit in team.Units)
                {
                    if (unit == null || unit.IsDefeated || unit.UnitId == projectile.OwnerUnitId)
                    {
                        continue;
                    }

                    var distanceSquared = (unit.Position - projectile.Position).sqrMagnitude;
                    if (distanceSquared >= bestDistanceSquared)
                    {
                        continue;
                    }

                    bestDistanceSquared = distanceSquared;
                    target = unit.Position;
                }
            }

            if (_terrain == null)
            {
                return bestDistanceSquared < radius * radius;
            }

            var grid = _terrain.Grid;
            var minimum = grid.WorldToCell(projectile.Position - Vector2.one * radius);
            var maximum = grid.WorldToCell(projectile.Position + Vector2.one * radius);
            for (var y = Mathf.Max(0, minimum.y); y <= Mathf.Min(grid.Height - 1, maximum.y); y++)
            {
                for (var x = Mathf.Max(0, minimum.x); x <= Mathf.Min(grid.Width - 1, maximum.x); x++)
                {
                    var material = grid.GetMaterialCell(x, y);
                    if (!grid.IsSolidCell(x, y)
                        || (material != TerrainMaterial.LightMetal && material != TerrainMaterial.Titanium))
                    {
                        continue;
                    }

                    var point = grid.CellToWorld(x, y);
                    var distanceSquared = (point - projectile.Position).sqrMagnitude;
                    if (distanceSquared >= bestDistanceSquared)
                    {
                        continue;
                    }

                    bestDistanceSquared = distanceSquared;
                    target = point;
                }
            }

            return bestDistanceSquared < radius * radius;
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

        /// <summary>Box-overlap test against one specific unit (used to detect
        /// when a projectile leaves its owner's hitbox after launch).</summary>
        private bool IsTouchingUnit(Vector2 point, float radius, string unitId)
        {
            if (string.IsNullOrEmpty(unitId))
            {
                return false;
            }

            var unit = _state.GetUnit(unitId);
            if (unit == null || unit.IsDefeated)
            {
                return false;
            }

            return IsTouchingUnitHitbox(point, radius, unit);
        }

        private bool TryHitUnit(Vector2 point, float radius, string ignoredUnitId, out Vector2 hitPoint)
        {
            return TryFindHitUnit(point, radius, ignoredUnitId, out _, out hitPoint);
        }

        private bool TryFindHitUnit(
            Vector2 point,
            float radius,
            string ignoredUnitId,
            out BattleUnitState hitUnit,
            out Vector2 hitPoint)
        {
            hitUnit = null;
            hitPoint = point;
            foreach (var team in _state.Teams)
            {
                foreach (var unit in team.Units)
                {
                    if (unit.IsDefeated || unit.UnitId == ignoredUnitId)
                    {
                        continue;
                    }

                    if (IsTouchingUnitHitbox(point, radius, unit))
                    {
                        hitUnit = unit;
                        hitPoint = unit.Position + (point - unit.Position) * 0.5f;
                        return true;
                    }
                }
            }

            return false;
        }

        private bool IsTouchingUnitHitbox(Vector2 point, float radius, BattleUnitState unit)
        {
            var halfHeight = ArchetypeFor(unit.TeamId).BodyHeight * 0.5f;
            var lowerY = unit.Position.y - halfHeight - radius;
            var upperY = unit.Position.y + halfHeight + _settings.UnitCollisionTopPadding + radius;
            return Mathf.Abs(point.x - unit.Position.x) <= _settings.UnitCollisionHalfWidth + radius
                && point.y >= lowerY
                && point.y <= upperY;
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
                && (projectileId.StartsWith("projectile.weapon.mortira.fragment", StringComparison.Ordinal)
                    || projectileId.StartsWith(MortarCascadeProjectilePrefix, StringComparison.Ordinal)))
            {
                return _weaponsById.TryGetValue("weapon.mortira", out var mortar) ? mortar : null;
            }

            if (IsMissileCluster(projectileId))
            {
                return _weaponsById.TryGetValue(MissileWeaponId, out var missile) ? missile : null;
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

        private sealed class ScheduledProjectileSpawn
        {
            public ScheduledProjectileSpawn(ProjectileSpawnRequest request, float delaySeconds)
            {
                Request = request;
                RemainingDelay = delaySeconds;
            }

            public ProjectileSpawnRequest Request { get; }
            public float RemainingDelay { get; set; }
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
                Vector2 targetPoint,
                float drillRange = 0f,
                TerrainMaterial drillPenetration = TerrainMaterial.Soil)
            {
                Kind = kind;
                OwnerUnitId = ownerUnitId;
                TeamId = teamId;
                Weapon = weapon;
                Origin = origin;
                Direction = direction.sqrMagnitude > 0.01f ? direction.normalized : Vector2.right;
                TargetPoint = targetPoint;
                DrillRange = Mathf.Max(0.25f, drillRange);
                DrillPenetration = drillPenetration;
            }

            public ActiveTimedEffectKind Kind { get; }
            public string OwnerUnitId { get; }
            public BattleTeamId TeamId { get; }
            public WeaponDefinition Weapon { get; }
            public Vector2 Origin { get; }
            public Vector2 Direction { get; }
            public Vector2 TargetPoint { get; }
            public float DrillRange { get; }
            public TerrainMaterial DrillPenetration { get; }
            public float Elapsed { get; set; }
            public float TickAccumulator { get; set; }
            public float TerrainTickAccumulator { get; set; }
            public int SpawnedBlobs { get; set; }
            public float StreamDurationSeconds { get; set; } = FireStreamDurationSeconds;

            public static ActiveTimedEffect Drill(
                string ownerUnitId,
                BattleTeamId teamId,
                WeaponDefinition weapon,
                Vector2 origin,
                Vector2 direction,
                float drillRange,
                TerrainMaterial drillPenetration)
            {
                return new ActiveTimedEffect(
                    ActiveTimedEffectKind.Drill,
                    ownerUnitId,
                    teamId,
                    weapon,
                    origin,
                    direction,
                    Vector2.zero,
                    drillRange,
                    drillPenetration);
            }

            public static ActiveTimedEffect FireStream(
                string ownerUnitId,
                BattleTeamId teamId,
                WeaponDefinition weapon,
                Vector2 origin,
                Vector2 direction,
                float streamDurationSeconds = FireStreamDurationSeconds)
            {
                return new ActiveTimedEffect(ActiveTimedEffectKind.FireStream, ownerUnitId, teamId, weapon, origin, direction, Vector2.zero)
                {
                    StreamDurationSeconds = streamDurationSeconds > 0f ? streamDurationSeconds : FireStreamDurationSeconds
                };
            }

        }

        private sealed class ActiveMine
        {
            public ActiveMine(
                int id,
                string ownerUnitId,
                BattleTeamId teamId,
                WeaponDefinition weapon,
                Vector2 position,
                bool triggersEnemiesOnly,
                bool alliesImmuneToExplosion)
            {
                Id = id;
                OwnerUnitId = ownerUnitId;
                TeamId = teamId;
                Weapon = weapon;
                Position = position;
                TriggersEnemiesOnly = triggersEnemiesOnly;
                AlliesImmuneToExplosion = alliesImmuneToExplosion;
                ArmTimeRemaining = MineActivationDelaySeconds;
            }

            public int Id { get; }
            public string OwnerUnitId { get; }
            public BattleTeamId TeamId { get; }
            public WeaponDefinition Weapon { get; }
            public Vector2 Position { get; private set; }
            public bool TriggersEnemiesOnly { get; }
            public bool AlliesImmuneToExplosion { get; }
            public float ArmTimeRemaining { get; set; }
            public bool Triggered { get; set; }
            public float TriggerTimeRemaining { get; set; }

            public void MoveTo(Vector2 position)
            {
                Position = position;
            }
        }

        private sealed class ActiveRepairEffect
        {
            public ActiveRepairEffect(
                string unitId,
                BattleTeamId teamId,
                int healAmount,
                int protectionTurns,
                int regenerationChecks)
            {
                UnitId = unitId;
                TeamId = teamId;
                HealAmount = Mathf.Max(0, healAmount);
                ProtectionTurnsRemaining = Mathf.Max(0, protectionTurns);
                RegenerationChecksRemaining = Mathf.Max(0, regenerationChecks);
            }

            public string UnitId { get; }
            public BattleTeamId TeamId { get; }
            public int HealAmount { get; }
            public int ProtectionTurnsRemaining { get; set; }
            public int RegenerationChecksRemaining { get; set; }
            public bool DamageTakenSinceLastCheck { get; set; }
        }

        private sealed class ActivePoisonEffect
        {
            public ActivePoisonEffect(
                string unitId,
                BattleTeamId sourceTeam,
                int damagePerTurn,
                int turnsRemaining,
                int armorWeakenPercent,
                int armorWeakenTurns,
                float movementDamagePerSecond)
            {
                UnitId = unitId;
                SourceTeam = sourceTeam;
                DamagePerTurn = Mathf.Max(0, damagePerTurn);
                TurnsRemaining = Mathf.Max(0, turnsRemaining);
                ArmorWeakenPercent = Mathf.Max(0, armorWeakenPercent);
                ArmorWeakenTurnsRemaining = Mathf.Max(0, armorWeakenTurns);
                MovementDamagePerSecond = Mathf.Max(0f, movementDamagePerSecond);
            }

            public string UnitId { get; }
            public BattleTeamId SourceTeam { get; set; }
            public int DamagePerTurn { get; set; }
            public int TurnsRemaining { get; set; }
            public int ArmorWeakenPercent { get; set; }
            public int ArmorWeakenTurnsRemaining { get; set; }
            public float MovementDamagePerSecond { get; set; }
            public float MovementSecondsAccumulator { get; set; }
        }

        private sealed class ActiveBurnEffect
        {
            public ActiveBurnEffect(
                string unitId,
                BattleTeamId sourceTeam,
                int damagePerTurn,
                int turnsRemaining,
                int lastApplicationTurnSequence)
            {
                UnitId = unitId;
                SourceTeam = sourceTeam;
                DamagePerTurn = Mathf.Max(0, damagePerTurn);
                TurnsRemaining = Mathf.Max(0, turnsRemaining);
                LastApplicationTurnSequence = Mathf.Max(0, lastApplicationTurnSequence);
            }

            public string UnitId { get; }
            public BattleTeamId SourceTeam { get; set; }
            public int DamagePerTurn { get; set; }
            public int TurnsRemaining { get; set; }
            public int LastApplicationTurnSequence { get; set; }
        }

        private sealed class ActiveEmpWeaken
        {
            public ActiveEmpWeaken(int damagePercent, int turnsRemaining)
            {
                DamagePercent = Mathf.Clamp(damagePercent, 0, 100);
                TurnsRemaining = Mathf.Max(0, turnsRemaining);
            }

            public int DamagePercent { get; set; }
            public int TurnsRemaining { get; set; }
        }

        private sealed class ActiveCharge
        {
            public ActiveCharge(
                int id,
                string ownerUnitId,
                BattleTeamId teamId,
                WeaponDefinition weapon,
                Vector2 position,
                float fuseSeconds,
                float knockbackMultiplier)
            {
                Id = id;
                OwnerUnitId = ownerUnitId;
                TeamId = teamId;
                Weapon = weapon;
                Position = position;
                FuseRemaining = fuseSeconds;
                KnockbackMultiplier = Mathf.Max(0f, knockbackMultiplier);
            }

            public int Id { get; }
            public string OwnerUnitId { get; }
            public BattleTeamId TeamId { get; }
            public WeaponDefinition Weapon { get; }
            public Vector2 Position { get; private set; }
            public float FuseRemaining { get; set; }
            public float KnockbackMultiplier { get; }

            public void MoveTo(Vector2 position)
            {
                Position = position;
            }
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

        private sealed class ActiveDisorientation
        {
            public ActiveDisorientation(int firstAffectedTurnSequence, int turnsRemaining)
            {
                FirstAffectedTurnSequence = Mathf.Max(0, firstAffectedTurnSequence);
                TurnsRemaining = Mathf.Max(0, turnsRemaining);
            }

            public int FirstAffectedTurnSequence { get; set; }
            public int TurnsRemaining { get; set; }
        }

        private sealed class ActiveFirePuddle
        {
            public ActiveFirePuddle(
                int id,
                string ownerUnitId,
                WeaponDefinition weapon,
                Vector2 point,
                float delaySeconds,
                float lifetimeSeconds = FirePuddleLifetimeSeconds,
                int damageBudget = 0)
            {
                Id = id;
                OwnerUnitId = ownerUnitId;
                Weapon = weapon;
                Point = point;
                DelayRemaining = Mathf.Max(0f, delaySeconds);
                LifetimeSeconds = lifetimeSeconds > 0f ? lifetimeSeconds : FirePuddleLifetimeSeconds;
                DamageBudget = Mathf.Max(0, damageBudget);
            }

            public int Id { get; }
            public string OwnerUnitId { get; }
            public WeaponDefinition Weapon { get; }
            public Vector2 Point { get; set; }
            public float DelayRemaining { get; set; }
            public float LifetimeSeconds { get; }
            public float Elapsed { get; set; }
            public float TickAccumulator { get; set; }
            public float DamageTickAccumulator { get; set; }
            public int DamageBudget { get; }
            public int DamageTicksProcessed { get; private set; }
            public int DamageApplied { get; private set; }

            public int ConsumeDamageForTick()
            {
                var totalTicks = Mathf.Max(1, Mathf.CeilToInt(LifetimeSeconds / FirePuddleDamageTickSeconds));
                DamageTicksProcessed = Mathf.Min(totalTicks, DamageTicksProcessed + 1);
                var cumulativeBudget = Mathf.CeilToInt(
                    DamageBudget * DamageTicksProcessed / (float)totalTicks);
                var targetDamage = DamageTicksProcessed >= totalTicks
                    ? DamageBudget
                    : cumulativeBudget;
                var tickDamage = Mathf.Max(0, targetDamage - DamageApplied);
                DamageApplied += tickDamage;
                return tickDamage;
            }
        }
    }
}
