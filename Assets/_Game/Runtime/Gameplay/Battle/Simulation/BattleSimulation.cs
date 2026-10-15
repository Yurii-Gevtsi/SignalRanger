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
    /// <summary>
    /// The battle rules: turn flow, firing, impact resolution, unit motion
    /// integration. No scene, no input, no status text - the scene controller
    /// drives this through commands and reacts to its events.
    /// </summary>
    public sealed partial class BattleSimulation : IProjectileSpawner, IDelayedProjectileSpawner
    {
        private const string TitaniumDiscWeaponId = "weapon.titanium-disc";
        private const float TitaniumDiscDamageBonusPerBounce = 0.25f;

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
        private const string RailRifleWeaponId = "weapon.rail-rifle";
        private const float RailPierceDamageStep = 0.25f;
        private const float RailPierceMinimumDamageFactor = 0.25f;
        // The rail bullet flies at 2 x V0 and goes as far through ground as the drill rocket does at V0.
        private const float RailBasePierceSeconds = 0.25f;
        private const float RailPierceStepPercent = 0.10f;
        private const float RailTunnelRadius = 0.12f;
        private const string C4WeaponId = "weapon.c4";
        private const string FireGunWeaponId = "weapon.fire-gun";
        private const string ToxicWeaponId = "weapon.toxic";
        private const string PoisonDamageSourceId = "status.poison";
        private const string BurnDamageSourceId = "status.burn";
        private const float EmpOverlayDurationSeconds = 0.75f;
        private const float ElementalStatusDamageRatio = 0.2f;
        private const int ToxicBaseDurationTurns = 2;
        private const float PoisonMovementDamageTickSeconds = 1f;
        private const string MineWeaponId = "weapon.mine";
        private const string GuardianDroneWeaponId = "weapon.guardian-drone";
        private const string LaserFenceWeaponId = "weapon.laser-fence";
        private const string MissileWeaponId = "weapon.missle";
        private const string TeleportWeaponId = "weapon.teleport";
        private const string SwapJumpWeaponId = "weapon.swap-jump";
        private const string GrounderWeaponId = "weapon.grounder";
        private const string SelfDestructWeaponId = "weapon.self-destruct";
        private const string BomberCopterProjectileId = "projectile.weapon.bomber";
        private const string BomberBombProjectileId = "projectile.weapon.bomber.bomb";
        private const string CarrierCopterProjectileId = "projectile.weapon.ufo-carrier";
        private const string CarrierFallProjectileId = "projectile.weapon.ufo-carrier.fall";
        private const float CarrierCaptureDistance = 0.62f;
        private const float CarrierCargoLift = 0.2f;
        private const float GrounderCaptureSpeed = 7f;
        private const int BomberBombCount = 4;
        private const float BomberBodyHalfSize = 0.32f;
        private const string SkyHookWeaponId = "weapon.sky-hook";
        // The rocket carves a channel a little wider than the rail bullet's.
        private const float SkyHookTunnelRadius = 0.18f;
        // The blast happens a little below the top of the arena so that it stays on the visible scene.
        private const float SkyHookApexMargin = 0.75f;
        private const string BoostWeaponId = "weapon.boost";
        private const string ForceFieldWeaponId = "weapon.force-field";
        // Owner decision 2026-10-09: the magnet's capture range and the force field's sphere are 1.5 x N0.
        private const float PlacedDeviceRadiusMultiplier = 1.5f;
        // The guardian drone notices a droid within 2 x N0 (its own art is about N0 wide).
        private const float GuardianDetectionMultiplier = 2f;
        private const float SkyHookArmingSeconds = 3f;
        // Half the rocket body's width and length: what touches a droid on the way up.
        private const float SkyHookBodyHalfWidth = 0.16f;
        private const float SkyHookBodyHalfLength = 0.38f;
        private const float MortarFragmentContactSeconds = 0.3f;
        private const string GrenadeWeaponId = "weapon.granata";
        private const string IonShotgunWeaponId = "weapon.ion-shotgun";
        private const string FpvWeaponId = "weapon.fpv-drone";
        private const string BomberWeaponId = "weapon.bomber";
        private const string CarrierWeaponId = "weapon.ufo-carrier";
        private const string FreezeWeaponId = "weapon.freeze";
        private const string CryoGrenadeWeaponId = "weapon.cryo-grenade";
        private const string EmpWeaponId = "weapon.emp";
        private const int CryoBaseFreezeTurns = 1;
        private const int CryoChainWindowTurnSequences = 2;
        private const string MineDeployProjectileId = "projectile.weapon.mine.deploy";
        private const string GuardianDroneDeployProjectileId = "projectile.weapon.guardian-drone.deploy";
        private const string GuardianDroneShotProjectileId = "projectile.weapon.guardian-drone.shot";
        private const string LaserFenceDeployProjectileId = "projectile.weapon.laser-fence.deploy";
        private const int MaxLaserFenceNodesPerTurn = 4;
        private const float LaserFenceGravityScale = 1.05f;
        /// <summary>Three times the old visual link of 1.8.</summary>
        public const float LaserFenceLinkRange = 5.4f;
        private const float LaserFenceLinkRangeStep = 0.10f;
        // Hull durability is a playtest number: any ordinary blast near a hull destroys it.
        // Any hit wrecks a hull: one blast of any strength or one pellet that touches it.
        private const int LaserFenceNodeHealth = 1;
        private const float LaserFenceHullHitRadius = 0.12f;
        private const float LaserFenceContactRadius = 0.42f;
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
        private const float GuardianDroneRiseSeconds = 0.25f;
        private const float GuardianDroneRiseHeight = 0.6f;
        private const float GuardianDroneShotSpeed = 22f;
        private const float C4FuseSeconds = 3f;
        private const float C4DropDurationSeconds = 0.35f;
        private const float C4DeployDistance = 0.82f;
        private const float DrillDurationSeconds = 3f;
        private const float DrillVisualGrowthSeconds = 1.2f;
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
        private const float DrillRocketTerrainSpeedRetain = 1f;
        private const float DrillRocketTunnelRadius = 0.24f;
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
        // Smooth, limited turning: the rocket arcs towards its target instead of snapping round.
        private const float MissileSteerDegreesPerSecond = 150f;
        // Without a purchased manoeuvre the rocket may still climb over up to this many obstacles.
        private const int MissileBaseObstacleBypasses = 2;
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
        private const string MortarCascadeProjectilePrefix = "projectile.weapon.mortira.cascade.";
        private const string RopeWeaponId = "weapon.rope";
        private const float RopeHookSpeed = 18f;
        private const float RopeBaseSwingAcceleration = 8f;
        private const string ElectricHarpoonWeaponId = "weapon.electric-harpoon";
        private const float ElectricHarpoonBaseLength = 8f;
        private const float ElectricHarpoonHookSpeed = 18f;
        // Twice the previous pulling speed; the target is dragged along the ground, not teleported.
        private const float ElectricHarpoonPullSpeed = 4.8f;
        private const float ElectricHarpoonPullWindowSeconds = 5f;
        private const float ElectricHarpoonContactX = 0.75f;
        private const float ElectricHarpoonContactY = 0.9f;
        private const float ElectricHarpoonHopSpeed = 2.4f;
        private const float ElectricHarpoonThrowAngleDegrees = 60f;
        // Prototype X (hit), Y (per second) and Z (final) shares of the weapon damage.
        private const float ElectricHarpoonHitShare = 0.35f;
        private const float ElectricHarpoonTickShare = 0.15f;
        private const float ElectricHarpoonFinalShare = 0.35f;
        private const float ElectricHarpoonDamageTickSeconds = 1f;
        private const float ElectricHarpoonTargetRadius = 0.18f;
        private const float ElectricHarpoonReleaseDistance = 0.62f;
        private const string PlasmaSwordWeaponId = "weapon.plasma-sword";
        private const string StickyGrenadeWeaponId = "weapon.sticky-grenade";
        private const float StickyGrenadeFuseSeconds = 1.5f;
        private const string HoppingGrenadeWeaponId = "weapon.hopping-grenade";
        private const float HoppingGrenadeLaunchSpeed = 5.5f;
        private const float HoppingGrenadeFuseSeconds = 3f;
        private const float HoppingGrenadeVerticalSpeed = 3.8f;
        private const string IncendiaryGrenadeWeaponId = "weapon.incendiary-grenade";
        private const int IncendiaryPuddleCount = 8;
        private const float IncendiaryPuddleSpreadRadius = 0.55f;
        private const float IncendiaryPuddleLifetimeSeconds = 2.5f;
        private const float IncendiaryDropSpeed = 7f;
        private const float IncendiaryDropGravityScale = 3f;
        private const string RepairJammerWeaponId = "weapon.repair-jammer";
        private const int RepairJammerBaseDurationTurns = 2;
        private const string OrbitalRocketWeaponId = "weapon.orbital-rocket";
        private const string NuclearChargeWeaponId = "weapon.nuclear-charge";
        private const float OrbitalRocketFallSpeed = 14f;
        private const float OrbitalRocketMinimumSpawnHeight = 4f;
        private const string VirusWeaponId = "weapon.virus";
        private const float AttachmentDeviceRange = 2.75f;
        private const float AttachmentDeviceSpeed = 9f;
        private const string SpiderMineWeaponId = "weapon.spider-mine";
        private const float SpiderSearchDurationSeconds = 5f;
        // Owner decision 2026-10-08: twice as fast, and it blasts at once when it reaches an enemy.
        private const float SpiderAttachFuseSeconds = 0f;
        private const float SpiderCrawlSpeed = 2.8f;
        private const float SpiderAttachDistance = 0.34f;
        // The crawler follows the ground: it climbs ledges up to this height and steps down up to the next.
        private const float SpiderClimbHeight = 0.45f;
        private const float SpiderDropHeight = 1.2f;

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
        private Texture2D _titaniumBarrierTexture;
        public Vector2 TitaniumBarrierBaseSize { get; private set; }
        private readonly List<ActiveMine> _activeMines = new();
        private readonly List<ActiveGrounder> _activeGrounders = new();
        private readonly List<ActiveForceField> _activeForceFields = new();
        private readonly List<ActiveLaserFenceNode> _activeLaserFenceNodes = new();
        private readonly List<CarryablePickup> _carryablePickups = new();
        private const string TurnTransferWeaponId = "weapon.turn-transfer";
        private TurnTransferSelection _turnTransfer;
        private readonly Dictionary<int, int> _guardianCopterShots = new();
        private readonly HashSet<int> _downedCopterProjectileIds = new();
        private List<LaserFenceLink> _currentLaserFenceLinks = new();
        private readonly HashSet<ulong> _spentLaserFenceLinks = new();
        private readonly List<ActiveCharge> _activeCharges = new();
        private readonly List<ActiveTimedEffect> _activeTimedEffects = new();
        private readonly List<ScheduledProjectileSpawn> _scheduledProjectileSpawns = new();
        private readonly List<ActiveFirePuddle> _activeFirePuddles = new();
        private int _hoppingGrenadeProjectileId = -1;
        private readonly Dictionary<string, float> _lavaOverlayRemainingSeconds = new(StringComparer.Ordinal);
        private readonly Dictionary<string, ActiveBurnEffect> _burnEffects = new(StringComparer.Ordinal);
        private readonly Dictionary<string, float> _electrifiedOverlayRemainingSeconds = new(StringComparer.Ordinal);
        private readonly Dictionary<string, ActiveRepairEffect> _repairEffects = new(StringComparer.Ordinal);
        private readonly Dictionary<string, int> _repairJammerTurns = new(StringComparer.Ordinal);
        private readonly Dictionary<string, VirusInfection> _virusInfections = new(StringComparer.Ordinal);
        private readonly Dictionary<string, SelfDestructMark> _selfDestructMarks = new(StringComparer.Ordinal);
        private const int VirusDurationTurns = 3;
        private readonly Dictionary<string, ActiveDisorientation> _disorientations = new(StringComparer.Ordinal);
        private readonly Dictionary<string, ActivePoisonEffect> _poisonEffects = new(StringComparer.Ordinal);
        // Shared two-turn elemental cycle: cold uses it now, fire and the others reuse the same rules.
        private readonly Dictionary<string, ElementalStatus> _chillEffects = new(StringComparer.Ordinal);
        private readonly Dictionary<string, ElementalStatus> _fireEffects = new(StringComparer.Ordinal);
        private readonly Dictionary<string, ElementalStatus> _electricEffects = new(StringComparer.Ordinal);
        private readonly Dictionary<string, ActiveDisorientation> _aimJams = new(StringComparer.Ordinal);
        private readonly Dictionary<string, Vector2> _poisonLastPositions = new(StringComparer.Ordinal);
        private const int ElementalBaseDurationTurns = 2;
        private const int ElementalRepeatBonusPercent = 25;
        private readonly Dictionary<string, ActiveDisorientation> _freezes = new(StringComparer.Ordinal);
        private readonly Dictionary<string, int> _lastFreezeApplicationTurn = new(StringComparer.Ordinal);
        private readonly Dictionary<string, ActiveDisorientation> _empStuns = new(StringComparer.Ordinal);
        private readonly Dictionary<string, ActiveEmpWeaken> _empWeakens = new(StringComparer.Ordinal);

        private WeaponDefinition _activeShotWeapon;
        private bool _activeShotEndsTurn = true;
        private int _blasterShotsThisTurn;
        private int _minesDeployedThisTurn;
        private int _laserFenceNodesDeployedThisTurn;
        private int _nextProjectileId;
        private float _simulationAccumulator;
        private readonly List<HarpoonCapture> _harpoonCaptures = new();
        private bool _harpoonHookFinished;
        private bool _harpoonImpactPull;
        private bool _harpoonDoubleGrab;
        private float _harpoonRopeLength;
        private int _walkIntentDirection;
        private int _controlledProjectileId = -1;
        private Vector2 _controlledProjectileInput;
        private float _retreatRemaining = -1f;
        private TurnAdvanceReason _pendingRetreatReason;
        private bool _playerTurnTimerPaused;
        private int _turnSequence;
        private RopeTraversal _ropeTraversal;
        private BoostSession _boost;
        private CopterSession _copter;
        private ElectricHarpoonTraversal _electricHarpoonTraversal;
        private Vector2? _orbitalTargetMarker;
        private PendingOrbitalStrike _pendingOrbitalStrike;

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
            CapMineStock(_playerInventory);
            CapMineStock(_opponentInventory);
        }

        private static void CapMineStock(WeaponInventoryRuntime inventory)
        {
            if (inventory == null) return;
            foreach (var weapon in inventory.Weapons)
                if (weapon.WeaponId == MineWeaponId && inventory.GetRemainingAmmo(weapon) > 4)
                    inventory.SetRemainingAmmo(weapon, 4);
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
        public event Action<GrounderDeployedEvent> GrounderDeployed;
        public event Action<int> GrounderRemoved;
        /// <summary>A placed magnet or force field device: id, health left, its full health.</summary>
        public event Action<int, int, int> PlacedDeviceHealthChanged;
        /// <summary>A mine, guardian or laser-fence node was picked up (true) or let go (false) by the carrier.</summary>
        public event Action<int, bool> GroundObjectCarried;
        public event Action<ForceFieldDeployedEvent> ForceFieldDeployed;
        public event Action<int> ForceFieldRemoved;
        /// <summary>A hostile energy carrier or beam was absorbed: field id and the absorption point.</summary>
        public event Action<int, Vector2> ForceFieldAbsorbed;
        public event Action<LaserFenceNodeDeployedEvent> LaserFenceNodeDeployed;
        /// <summary>A hull was destroyed: its beams disappear with it.</summary>
        public event Action<int> LaserFenceNodeRemoved;
        /// <summary>A pair fired once and its beam is gone for good.</summary>
        public event Action<int, int> LaserFenceLinkSpent;
        /// <summary>The rest of the turn went from the first droid to the second one.</summary>
        public event Action<string, string> TurnTransferred;
        public event Action<int> MineTriggered;
        public event Action<ChargeDeployedEvent> ChargeDeployed;
        public event Action<GroundObjectRepositionedEvent> GroundObjectRepositioned;
        public event Action<LinearWeaponEffectEvent> LinearWeaponEffectStarted;
        public event Action<FireBlobEvent> FireBlobSpawned;
        public event Action<SwordSwingEvent> SwordSwingStarted;
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
            || _activeMines.Exists(mine => mine.IsGuardian && mine.Triggered)
            || _activeMines.Exists(mine => mine.IsSpider && mine.OwnerUnitId == _state.Turn.ActiveUnitId)
            || _scheduledProjectileSpawns.Count > 0
            || _ropeTraversal != null && !_ropeTraversal.IsAttached
            || _electricHarpoonTraversal != null;
        /// <summary>True while the active droid is on rocket fuel (hovering).</summary>
        /// <summary>True while the active droid's copter is in the air.</summary>
        /// <summary>True while the active droid's carrier copter holds an item.</summary>
        public bool CopterHasCargo => _copter != null && _copter.HasCargo;
        public bool HasPlayerCopter => _copter != null && _copter.OwnerUnitId == _state.Turn.ActiveUnitId;
        public int BomberBombsRemaining => _copter?.BombsLeft ?? 0;

        /// <summary>Seconds left of the active droid's copter flight; 0 without a copter.</summary>
        public float CopterSecondsRemaining
        {
            get
            {
                if (!HasPlayerCopter)
                {
                    return 0f;
                }

                var copter = _projectiles.Find(value => value.Id == _copter.CopterProjectileId);
                return copter != null ? Mathf.Max(0f, copter.RemainingLifetime) : 0f;
            }
        }

        public bool HasPlayerBomberCopter => HasPlayerCopter && !_copter.IsCarrier;
        public bool HasPlayerBoost => _boost != null && _boost.OwnerUnitId == _state.Turn.ActiveUnitId;
        public float BoostSecondsRemaining => _boost?.Remaining ?? 0f;
        public bool HasPlayerRopeTraversal => _ropeTraversal != null
            && _ropeTraversal.OwnerUnitId == _state.Turn.ActiveUnitId;
        public bool IsPlayerRopeAttached => HasPlayerRopeTraversal && _ropeTraversal.IsAttached;
        public Vector2 RopeHookPosition => _ropeTraversal?.HookPosition ?? Vector2.zero;
        public Vector2 RopeHookDirection => _ropeTraversal?.Direction ?? Vector2.up;
        public Vector2 RopeAnchor => _ropeTraversal?.Anchor ?? Vector2.zero;
        public bool HasPlayerElectricHarpoon => _electricHarpoonTraversal != null
            && _electricHarpoonTraversal.OwnerUnitId == _state.Turn.ActiveUnitId;
        public bool IsPlayerElectricHarpoonAttached => HasPlayerElectricHarpoon
            && _electricHarpoonTraversal.IsAttached;
        public Vector2 ElectricHarpoonHookPosition => _electricHarpoonTraversal?.HookPosition ?? Vector2.zero;
        public Vector2 ElectricHarpoonOrigin => _electricHarpoonTraversal?.Origin ?? Vector2.zero;
        public Vector2 ElectricHarpoonDirection => _electricHarpoonTraversal?.Direction ?? Vector2.right;
        /// <summary>Positions of the additional captured droids (double grab), for the extra ropes.</summary>
        public IReadOnlyList<Vector2> ElectricHarpoonExtraTargets
        {
            get
            {
                var extra = new List<Vector2>();
                for (var i = 1; i < _harpoonCaptures.Count; i++)
                {
                    var unit = FindUnit(_harpoonCaptures[i].UnitId);
                    if (unit != null && !_harpoonCaptures[i].Finished)
                    {
                        extra.Add(unit.Position);
                    }
                }

                return extra;
            }
        }
        public int ElectricHarpoonActiveCaptures
        {
            get
            {
                var count = 0;
                foreach (var capture in _harpoonCaptures)
                {
                    if (!capture.Finished) count++;
                }

                return count;
            }
        }
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

        /// <summary>
        /// The player fired a turn-ending shot and is inside the short
        /// movement-only retreat window before the turn hands over.
        /// </summary>
        public bool IsPlayerRetreatActive => _retreatRemaining > 0f
            && _state.Turn.ActiveTeam == BattleTeamId.Player;

        /// <summary>
        /// The turn's attack is committed only after a turn-ending action. Mine
        /// placement deliberately keeps the action open so the player can place
        /// all available mines and then choose another weapon.
        /// </summary>
        public bool IsAttackCommitted => _state.Turn.ActionSpent;

        public WeaponInventoryRuntime ActiveInventory => GetInventoryFor(_state.Turn.ActiveTeam);

        /// <summary>
        /// Catalog entries with only a generic fallback must stay unavailable until
        /// their distinct combat action exists. A false action can heal, teleport,
        /// or explode when the weapon's specification calls for something else.
        /// </summary>
        public int ActiveForceFieldCount => _activeForceFields.Count;

        public bool HasOrbitalTargetMarker => _orbitalTargetMarker.HasValue;

        public Vector2 OrbitalTargetMarker => _orbitalTargetMarker.GetValueOrDefault();

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

        private int _teleportSphereProjectileId = -1;

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
                StepBoost(_settings.SimulationTickSeconds);
                StepElectricHarpoon(_settings.SimulationTickSeconds);
                StepScheduledProjectileSpawns(_settings.SimulationTickSeconds);
                StepProjectiles(_settings.SimulationTickSeconds);
                StepTimedWeaponEffects(_settings.SimulationTickSeconds);
                StepFirePuddles(_settings.SimulationTickSeconds);
                StepLavaOverlayStates(_settings.SimulationTickSeconds);
                StepElectrifiedOverlayStates(_settings.SimulationTickSeconds);
                StepVirusCarriers();
                StepMines(_settings.SimulationTickSeconds);
                StepPlacedDevices();
                StepCharges(_settings.SimulationTickSeconds);
                StepUnits();
                StepLaserFenceLinks();
                StepCarryablePickups();
                StepPoisonMovementDamage(_settings.SimulationTickSeconds);
            }

            UnitsMoved?.Invoke();

            if (HasProjectilesInFlight || HasActiveMotion || HasActiveWeaponEffect)
            {
                return;
            }

            ResolveBattleAfterMotion();
        }

        private const float LaserFenceBodyMargin = 0.06f;

        private readonly Dictionary<int, MortarSequence> _mortarSequences = new();

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

        /// <summary>True while the active droid's signal rocket still stands at its feet before lift-off.</summary>
        public bool IsActiveSkyHookArming => _projectiles.Count > 0
            && _projectiles.TrueForAll(projectile => projectile.IsSkyHookArming
                && projectile.OwnerUnitId == _state.Turn.ActiveUnitId);

        /// <summary>True while the active droid is choosing the ally that receives the rest of its turn.</summary>
        public bool HasTurnTransferSelection => _turnTransfer != null && _turnTransfer.OwnerUnitId == _state.Turn.ActiveUnitId;

        /// <summary>The ally currently chosen to receive the turn; empty without a selection.</summary>
        public string TurnTransferTargetUnitId => HasTurnTransferSelection ? _turnTransfer.TargetUnitId : string.Empty;

        private static readonly float[] SpiderFootOffsets = { 0f, -0.12f, 0.12f };

        private const float MovementPoisonCapPercent = 0.5f;
        private const float NanotoxicBaseDamageCapPercent = 16f;
        private const float TeleportPositionJump = 1f;

        private readonly Dictionary<(int, string), StreamHit> _streamHits = new();

        private const string ElementalDamageSourceId = "status.elemental";

        private const float ElementalSlowMovementFactor = 0.5f;

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
    }
}
