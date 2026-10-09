using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game.Core;
using Game.Core.Modes;
using Game.Core.Battle;
using Game.Data.Progression;
using Game.Data.Save;
using Game.Core.Localization;
using Game.Gameplay.Battle.Arenas;
using Game.Gameplay.Battle.AI;
using Game.Gameplay.Battle.AI.Ballistics;
using Game.Gameplay.Battle.AI.Movement;
using Game.Gameplay.Battle.Domain;
using Game.Gameplay.Battle.Feedback;
using Game.Gameplay.Battle.Motion;
using Game.Gameplay.Battle.Net;
using Game.Gameplay.Battle.Projectiles;
using Game.Gameplay.Battle.Results;
using Game.Gameplay.Battle.Runtime;
using Game.Gameplay.Battle.Simulation;
using Game.Gameplay.Battle.Terrain;
using Game.Gameplay.Battle.Units;
using Game.Gameplay.Battle.Visuals;
using Game.Gameplay.Battle.Weapons;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Game.Gameplay.Battle.Assembly
{
    public static class BattleSceneBootstrap
    {
        public static void EnsureCreated()
        {
            if (Object.FindAnyObjectByType<BattleSceneController>() != null)
            {
                return;
            }

            var root = new GameObject("Battle Scene Controller");
            root.AddComponent<BattleSceneController>();
        }
    }

    public sealed class BattleSceneController : MonoBehaviour, IBattleCommandTarget
    {
        private const float GroundRayStartHeight = 3.5f;
        private const float GroundRayDistance = 8f;
        private const float OccupiedRadius = 0.72f;
        private const float KnockbackLandingSearchPadding = 0.46f;
        private const float UnitCollisionHalfWidth = 0.34f;
        private const float UnitCrawlHeight = 0.3f;
        private const float FallDeathY = -6.2f;
        private const float SimulationTickSeconds = 1f / 120f;
        private const float PlayerTurnDurationSeconds = 30f;
        // The dotted arc grows with shot power: a weak shot shows the minimum
        // count over the minimum length; a full-power shot shows more dots spread
        // over a longer line (so the spacing between dots widens too).
        private const int MinTrajectoryDotCount = 13;
        private const int MaxTrajectoryDotCount = 20;
        private const int MaxFullTrajectoryDotCount = 256;
        private const float MinTrajectoryPreviewLength = 2.15f;
        private const float MaxTrajectoryPreviewLength = 6.5f;
        private const float MaxTrajectoryDotSpacing = 0.55f;
        private const float ProjectileMuzzleOffset = 0.45f;
        private const string DroidRigResourcePath = "Visuals/Characters/DroidRig";
        private const float DroidBodyLocalY = 1.8f;
        private const float CameraZoomInMultiplier = 0.58f;
        private const float CameraZoomStep = 1.25f;
        private const float KillZoneBottomInset = 0.55f;
        private const float KillZoneVisualHeightScale = 0.9f;
        private const float ArenaSpriteSeamOverdrawPixels = 2f;
        private const string KillZoneResourcePath = "Visuals/Arenas/KillZones/LaserLine/laser_line";
        private const float PinchZoomSensitivity = 0.008f;
        private const float DirectAimCenterDeadZone = 0.32f;
        private const float FirstBattleNavigationDistance = 1.2f;
        // High enough that the barrel visibly sticks to the finger (time
        // constant ~70 ms); the old 5.5 made straight-shot aiming feel laggy.
        private const float DirectAimSmoothingSpeed = 14f;
        private const string RopeHookSpriteId = "rope_shot";
        private const float LaserFenceLinkRange = BattleSimulation.LaserFenceLinkRange;
        private static readonly Color LaserFenceLinkColor = new(0.20f, 1f, 0.30f, 0.96f);
        // Barrels of these weapons visually match the aim only after this art
        // correction, and their resting pose tilts slightly toward the ground.
        private const float RestingWeaponTiltDegrees = 10f;
        private const int MaxActiveSupportPickups = 2;
        private const float SupportPickupWorldWidth = 0.93f;
        private const float SupportPickupGroundOffset = 0.4f;
        private const float ResourceDropBeamWorldWidth = 2.4f;
        private const float ResourceDropBeamGroundOverlap = 0.65f;
        private const float ResourceDropBeamGrowSeconds = 0.65f;
        private const float ResourceDropBeamFadeSeconds = 0.28f;
        private const float FloatingRewardDurationSeconds = 1.15f;
        private const float FloatingRewardRiseDistance = 0.82f;
        private const float FloatingRewardIconWorldWidth = 0.84f;
        private const string FloatingRewardFontResourcePath = "Fonts/Orbitron";
        private const string WeaponIconResourceRoot = "UI/Weapons/";
        private const string FreezeOverlayResourcePath = "UI/Generated/freeze_overlay_ice_01";
        private const string PoisonOverlayResourcePath = "UI/Generated/toxic_overlay_01";
        private const string LavaOverlayResourcePath = "UI/Generated/lava_overlay_01";
        private static readonly string[] ElectroOverlayResourcePaths =
        {
            "UI/Generated/electro_overlay_01",
            "UI/Generated/electro_overlay_02",
            "UI/Generated/electro_overlay_03",
            "UI/Generated/electro_overlay_04"
        };
        // Generated from the art-source tail with its chroma canvas removed. This
        // asset has real alpha and is used by rocket bodies without baked exhaust.
        private const string RocketTrailResourcePath = "UI/Generated/missle_tail_clean";
        private const float RocketTrailAttachFraction = 0.32f;
        private const string MissileInFlightSpriteId = "missle_fly";
        private const float OrbitalTargetDoubleTapSeconds = 0.35f;
        private static readonly Color OrbitalMarkerColor = new Color(1f, 0.18f, 0.16f, 1f);
        private const float OrbitalTargetDoubleTapDistancePixels = 48f;

        private readonly Color _playerColor = new(0.18f, 0.56f, 0.96f);
        private readonly Color _opponentColor = new(0.95f, 0.28f, 0.22f);
        private static readonly Color CoinRewardColor = new(1f, 0.8f, 0.22f, 1f);
        private static readonly Color WeaponRewardColor = new(0.48f, 0.9f, 1f, 1f);
        private static readonly Color BlueprintRewardColor = new(0.75f, 0.56f, 1f, 1f);
        private static readonly Color HealthRewardColor = new(0.32f, 0.96f, 0.64f, 1f);
        private readonly List<BattleUnitPresenter> _unitPresenters = new();
        private readonly List<GameObject> _arenaObjects = new();
        // Full-screen backdrop sprites must follow the camera while the player
        // pans. Their scale is fixed for the maximum zoom-out view, so this
        // avoids exposing the camera clear colour at either arena edge.
        private readonly List<(Transform Transform, Vector2 Offset)> _cameraCoverVisuals = new();
        private readonly List<ResourcePickup> _resourcePickups = new();
        private readonly Dictionary<string, UnitMotor> _unitMotors = new(System.StringComparer.Ordinal);
        private readonly Dictionary<string, WeaponDefinition> _weaponsById = new(System.StringComparer.Ordinal);
        /// <summary>Identifies this battle so its reward transaction is idempotent.</summary>
        private string _battleId = string.Empty;

        /// <summary>
        /// Craters cut since the current turn began. Collected so the authoritative
        /// side can describe the turn's terrain damage in a few dozen bytes instead
        /// of shipping the whole destruction mask.
        /// </summary>
        private readonly List<BattleCraterSnapshot> _turnCraters = new();

        /// <summary>Duel networking; null in every mode but <see cref="BattleMode.Pvp"/>.</summary>
        private BattleDuelCoordinator _duel;
        private BattleArenaReward _arenaReward = BattleArenaReward.None;
        private BattleWeaponProgression _playerProgression = BattleWeaponProgression.Empty;
        private BattleWeaponProgression _opponentProgression = BattleWeaponProgression.Empty;
        private readonly Dictionary<string, Sprite> _spriteResourceCache = new(System.StringComparer.Ordinal);
        private readonly Dictionary<int, GameObject> _projectileVisuals = new();
        private readonly Dictionary<int, GameObject> _mineVisuals = new();
        private readonly Dictionary<int, GameObject> _grounderVisuals = new();
        private readonly Dictionary<int, GameObject> _forceFieldVisuals = new();
        private readonly Dictionary<int, GameObject> _laserFenceNodeVisuals = new();
        private readonly HashSet<(int, int)> _spentLaserFenceLinks = new();
        private readonly Dictionary<int, float> _forceFieldPulseUntil = new();
        private readonly Dictionary<int, (WeaponDefinition Weapon, BattleTeamId Team)> _mineOwners = new();
        private readonly Dictionary<int, LaserFenceNodeDeployedEvent> _laserFenceNodes = new();
        private readonly List<LineRenderer> _laserFenceLinks = new();
        private readonly Dictionary<int, FireBlobVisual> _firePuddleVisuals = new();
        private float _runtimeFallDeathY = FallDeathY;
        private Font _floatingRewardFont;
        private Sprite _topPivotSolidSprite;
        private Sprite[] _electroOverlayFrames;
        private BattleWeaponHudItem[] _arsenalHudItems = System.Array.Empty<BattleWeaponHudItem>();
        private bool _hideHeldWeaponForReusableThrow;
        private GameObject _orbitalTargetMarkerVisual;
        private float _orbitalLastTapTime = -10f;
        private Vector2 _orbitalLastTapScreenPosition;

        private ArenaDefinition[] _arenas;
        private int _arenaIndex;
        private ArenaDefinition _currentArena;
        private WeaponInventoryRuntime _inventory;
        private WeaponInventoryRuntime _opponentInventory;
        private WeaponExecutor _weaponExecutor;
        private BattleState _battleState;
        private BattleSimulation _simulation;
        private BattleSimulationSettings _simulationSettings;
        private AimSettings _baseAimSettings;
        private AimSettings _aimSettings;
        private AimState _aimState;
        private Camera _camera;
        private Sprite _boxSprite;
        private Sprite _circleSprite;
        private LineRenderer _aimLine;
        private LineRenderer _trajectoryLine;
        private GameObject _barrierPlacementPreview;
        private bool _barrierPlacementInitialized;
        private float _barrierPlacementScale = 1f;
        private LineRenderer _ropeLine;
        private GameObject _ropeHookVisual;
        private LineRenderer _electricHarpoonLine;
        private LineRenderer _electricHarpoonExtraLine;
        private GameObject _electricHarpoonHookVisual;
        private readonly List<SpriteRenderer> _trajectoryDots = new();

        // Training-mission aim help: the dotted arc statically extends all the
        // way to the predicted impact point instead of stopping at preview length.
        private bool _impactGuideEnabled;
        private TrajectoryProbe _impactGuideProbe;
        private readonly List<TrajectoryUnitBody> _impactGuideBodies = new();
        private readonly List<MovingArenaSurface> _movingArenaSurfaces = new();
        private const float MovingArenaTerrainRefreshInterval = 0.1f;
        private float _movingArenaTerrainRefreshTimer;
        private BattleResultScreenState _resultScreenState;
        private bool _opponentRingOut;
        private string _status;
        private IBattleHud _hud;
        private BotTurnController _botTurnController;
        private AiDifficulty _aiDifficulty;
        private CharacterArchetype _playerArchetype;
        private CharacterArchetype _opponentArchetype;
        private BattleVisualTheme _visualTheme;
        private BattleTerrain _battleTerrain;
        private FirstBattleGuideState _firstBattleGuide;
        private string _coachMessage = string.Empty;
        private string _coachTargetId = string.Empty;
        private float _coachTimer;
        private bool _aimHadInput;
        private bool _aimDragActive;
        private bool _aimDragHadValidPull;
        private Vector2 _aimAnchorWorld;
        private bool _controlledPointerActive;
        private int _uiWalkInputDirection;
        private int _keyboardWalkInputDirection;
        private int _controlledUiHorizontalInput;
        private int _ropeVerticalInput;
        private int _pendingTutorialWalkDirection;
        private float _pendingTutorialWalkStartX;
        private float _minimumCameraSize;
        private float _maximumCameraSize;
        private float _previousPinchDistance;
        private bool _hasPreviousPinchDistance;
        private bool _playerWeaponSelected;
        private bool _cameraPanActive;
        private Vector2 _previousCameraPanScreenPosition;
        private Rect _cameraWorldBounds;
        private Rect _cameraPanBounds;
        private Rect? _configuredCameraFrame;
        private Vector2 _arenaWorldOffset;
        private Vector2 _backgroundWorldOffset;
        private Vector2 _arenaScale = Vector2.one;
        private Vector2 _arenaScalePivot;
        private float _cameraTopPanAllowancePercent;
        private float _cameraBottomPanAllowance;
        private float _killZoneOffsetY;
        private SpriteRenderer _staticBackgroundRenderer;
        private AnimatedSkyBackground _animatedBackground;
        private Transform _killZoneVisual;
        private bool _battleSettingsOpen;
        private bool _arsenalMenuOpen;
        private float _timeScaleBeforeSettings = 1f;
        private Vector2 _lastTrackedPlayerPosition;
        private bool _hasTrackedPlayerPosition;
        private bool _activityBattleRecorded;
        private int _supportDropTurnInterval;
        private int _nextSupportDropTurn;
        private bool _supportDropInProgress;

        private enum ResourcePickupKind
        {
            SupplyBox,
            Medkit
        }

        private sealed class ResourcePickup
        {
            public ResourcePickup(GameObject visual, Vector2 position, ResourcePickupKind kind)
            {
                Visual = visual;
                Position = position;
                Kind = kind;
            }

            public GameObject Visual { get; }
            public Vector2 Position { get; set; }
            public ResourcePickupKind Kind { get; }
            public bool IsFalling { get; set; }
            /// <summary>Id of the same pickup in the simulation, so the carrier can lift it; -1 without one.</summary>
            public int SimulationId { get; set; } = -1;
            /// <summary>Height of the visual centre above the ground point the simulation knows.</summary>
            public float VisualLift { get; set; }
            public bool IsCarried { get; set; }
        }

        private sealed class MovingArenaSurface
        {
            public Transform Transform;
            public Vector3 Origin;
            public float Range;
            public float Speed;
            public float Elapsed;
        }

        private bool HasProjectilesInFlight => _simulation != null && _simulation.HasProjectilesInFlight;

        private bool HasActiveMotion => _simulation != null && _simulation.HasActiveMotion;

        private bool HasActiveWeaponEffect => _simulation != null && _simulation.HasActiveWeaponEffect;

        private bool HasPendingGroundAction => _simulation != null && _simulation.HasPendingGroundAction;

        private bool CanContinueMinePlacementInput()
        {
            // The sphere's throw already spent the action; the early trigger is the follow-up press.
            if (_battleState.Turn.ActiveTeam == BattleTeamId.Player
                && _inventory?.Selected?.WeaponId == "weapon.teleport"
                && _simulation.CanTriggerTeleportEarly())
            {
                return true;
            }

            if (_battleState.Turn.ActiveTeam != BattleTeamId.Player
                || _inventory == null
                || HasActiveMotion
                || HasActiveWeaponEffect
                || HasPendingGroundAction
                || _simulation.IsAttackCommitted)
            {
                return false;
            }

            return (_inventory.Selected?.WeaponId == "weapon.mine"
                    && _inventory.CanFireSelected)
                || (_inventory.Selected?.WeaponId == "weapon.hopping-grenade"
                    && _simulation.CanContinueHoppingGrenade())
                || (_inventory.Selected?.WeaponId == "weapon.teleport"
                    && _simulation.CanTriggerTeleportEarly());
        }

        private void Awake()
        {
            BuildSprites();
            BuildCamera();
            BuildAimRenderers();
            BuildHud();
            _baseAimSettings = AimSettings.Default;
            _aimSettings = _baseAimSettings;
            _arenas = PrototypeArenaLibrary.CreateAll();
            var allWeapons = PrototypeWeaponLibrary.CreateAll();
            foreach (var weapon in allWeapons)
            {
                if (weapon != null && !_weaponsById.ContainsKey(weapon.WeaponId))
                {
                    _weaponsById.Add(weapon.WeaponId, weapon);
                }
            }

            _battleId = System.Guid.NewGuid().ToString("N");
            _arenaReward = BattleArenaReward.None;
            _playerProgression = BattleWeaponProgression.FromResolver(
                allWeapons,
                WeaponProgressionBridge.Resolve);
            _opponentProgression = BattleWeaponProgression.FromRanks(
                _weaponsById,
                GameSessionContext.SelectedOpponentWeaponIds,
                GameSessionContext.SelectedOpponentWeaponRanks,
                GameSessionContext.SelectedOpponentWeaponBranches);

            _weaponExecutor = new WeaponExecutor();
            _weaponExecutor.RegisterEffect(WeaponRole.ShortRangeBurst, new BurstProjectileEffect());
            _weaponExecutor.RegisterEffect("weapon.impulse", new ImpulseSalvoWeaponEffect());
            _aiDifficulty = MapDifficulty(GameSessionContext.SelectedAiDifficulty);
            _inventory = new WeaponInventoryRuntime(CreateMissionWeapons(allWeapons), 1);
            ApplyProgressionAmmo(_inventory, _playerProgression);
            _opponentInventory = new WeaponInventoryRuntime(CreateOpponentWeapons(allWeapons), 1);
            ApplyProgressionAmmo(_opponentInventory, _opponentProgression);
            RefreshArsenalHudItems();
            var showFirstBattleGuide = FirstBattleGuideState.IsFirstMission(GameSessionContext.SelectedMissionId)
                && !BattleTutorialBridge.IsActiveMissionTutorialCompleted();
            _firstBattleGuide = new FirstBattleGuideState(showFirstBattleGuide ? GameSessionContext.SelectedMissionId : string.Empty);
            _impactGuideEnabled = showFirstBattleGuide;
            SelectWeapon(_impactGuideEnabled ? 0 : _inventory.SelectedIndex);
            _playerWeaponSelected = false;

            LoadArena(GameSessionContext.SelectedArenaIndex, GameSessionContext.SelectedArenaId);
        }

        private static WeaponDefinition[] CreateMissionWeapons(IEnumerable<WeaponDefinition> allWeapons)
        {
            return allWeapons
                .Where(IsBattleArsenalWeapon)
                .ToArray();
        }

        /// <summary>
        /// Apply battle ammo bonuses from the progression owned by each side.
        /// </summary>
        private static void ApplyProgressionAmmo(WeaponInventoryRuntime inventory, BattleWeaponProgression progression)
        {
            if (inventory == null || progression == null || progression.IsEmpty)
            {
                return;
            }

            foreach (var weapon in inventory.Weapons)
            {
                if (weapon == null)
                {
                    continue;
                }

                if (weapon.WeaponId == "weapon.blaster-caset")
                {
                    inventory.SetRemainingAmmo(weapon, progression.ResolveEvolution(weapon.WeaponId).BlasterBurstShotCount);
                }
                else if (weapon.WeaponId == "weapon.mine")
                {
                    inventory.SetRemainingAmmo(weapon, 4);
                }
                else
                {
                    var loadoutAmmo = ResolveLoadoutBattleAmmo(weapon, progression);
                    if (loadoutAmmo > 0)
                    {
                        inventory.SetRemainingAmmo(weapon, loadoutAmmo);
                    }
                }
            }
        }

        /// <summary>
        /// Battle stock of a limited weapon: its base stock plus one per paid Loadout purchase, within the
        /// catalog limit of that weapon. Returns 0 when the stock is fixed or unlimited.
        /// </summary>
        public static int ResolveLoadoutBattleAmmo(WeaponDefinition weapon, BattleWeaponProgression progression)
        {
            if (weapon == null || progression == null || !weapon.HasLimitedAmmo
                || !WeaponProgressionCatalog.TryGet(weapon.WeaponId, out var definition)
                || definition.LoadoutUpgradeLimit <= 0)
            {
                return 0;
            }

            var loadoutPurchases = Mathf.Clamp(progression.ResolveUpgradeCount(
                weapon.WeaponId, WeaponUpgradePath.Loadout), 0, definition.LoadoutUpgradeLimit);
            return weapon.AmmoCount + loadoutPurchases;
        }

        private static bool IsBattleArsenalWeapon(WeaponDefinition weapon)
        {
            return weapon != null
                && !WeaponPresentationText.IsDropOnly(weapon)
                && WeaponProgressionBridge.IsWeaponUnlocked(weapon.WeaponId);
        }

        private static WeaponDefinition[] CreateOpponentWeapons(IEnumerable<WeaponDefinition> allWeapons)
        {
            var byId = allWeapons
                .Where(weapon => weapon != null)
                .ToDictionary(weapon => weapon.WeaponId);

            var selectedIds = GameSessionContext.SelectedOpponentWeaponIds != null && GameSessionContext.SelectedOpponentWeaponIds.Length > 0
                ? GameSessionContext.SelectedOpponentWeaponIds
                : new[] { "weapon.impulse", "weapon.blaster-caset" };

            return selectedIds
                .Where(byId.ContainsKey)
                .Select(id => byId[id])
                .ToArray();
        }

        private void BuildHud()
        {
            _hud = BattleHudFactoryBridge.Create(transform);
            if (_hud == null)
            {
                Debug.LogWarning("BattleHudFactoryBridge.Factory not registered; HUD will be disabled.");
                return;
            }

            _hud.FireRequested += Fire;
            _hud.WeaponSelected += OnHudWeaponSelected;
            _hud.ArsenalVisibilityChanged += OnHudArsenalVisibilityChanged;
            _hud.WeaponResetRequested += ResetSelectedWeapon;
            _hud.SettingsRequested += OpenBattleSettings;
            _hud.MoveLeftStarted += () => SetUiHorizontalInput(-1);
            _hud.MoveLeftStopped += () => StopUiHorizontalInput(-1);
            _hud.MoveRightStarted += () => SetUiHorizontalInput(1);
            _hud.MoveRightStopped += () => StopUiHorizontalInput(1);
            _hud.JumpLeftRequested += () => TryStartJump(-1);
            _hud.JumpRightRequested += () => TryStartJump(1);
            _hud.RopeUpStarted += () => _ropeVerticalInput = -1;
            _hud.RopeUpStopped += () => _ropeVerticalInput = 0;
            _hud.RopeDownStarted += () => _ropeVerticalInput = 1;
            _hud.RopeDownStopped += () => _ropeVerticalInput = 0;
            _hud.RestartRequested += RestartScene;
            _hud.ContinueRequested += ContinueFromResult;
            _hud.MenuRequested += ExitToMenu;
            _hud.TutorialSkipRequested += SkipFirstBattleTutorial;
        }

        private void SkipFirstBattleTutorial()
        {
            if (_firstBattleGuide == null || !_firstBattleGuide.IsActive)
            {
                return;
            }

            ClearWalkInput();
            _firstBattleGuide.Skip();
            BattleTutorialBridge.SkipActiveMissionTutorial();
            GameSessionContext.ReportBattleFacts(playerWon: true, healthPercent: 100, opponentRingOut: false);
            GameSessionContext.ReportMissionCompleted();
            GameSessionContext.RequestCampaignMissionView(GameSessionContext.SelectedMissionId);
            LoadMenuScene();
        }

        private void OnDestroy()
        {
            RestoreBattleTimeScale();
            if (_duel != null)
            {
                _duel.OpponentLeft -= OnDuelOpponentLeft;
                _duel.Dispose();
                _duel = null;
                PvpSessionBridge.Resolve()?.Disconnect();
            }
        }

        private void OpenBattleSettings()
        {
            if (_battleSettingsOpen || !BattleSettingsBridge.IsConfigured)
            {
                return;
            }

            _battleSettingsOpen = true;
            _timeScaleBeforeSettings = Time.timeScale;
            Time.timeScale = 0f;
            BattleSettingsBridge.Open(
                RestoreBattleTimeScale,
                () =>
                {
                    RestoreBattleTimeScale();
                    RestartScene();
                },
                () =>
                {
                    RestoreBattleTimeScale();
                    ExitToMenu();
                });
        }

        private void RestoreBattleTimeScale()
        {
            if (!_battleSettingsOpen)
            {
                return;
            }

            Time.timeScale = _timeScaleBeforeSettings;
            _battleSettingsOpen = false;
        }

        private void LateUpdate()
        {
            // Craters cut during this frame's simulation ticks reach the GPU once, here.
            _battleTerrain?.FlushTextureUploads();
        }

        private void Update()
        {
            if (!IsBattleReady())
            {
                return;
            }

            // Pumped before input so a command that arrived this frame is applied
            // against the same turn the player is looking at.
            _duel?.Tick(Time.unscaledDeltaTime, _battleState.Turn.TurnNumber);

            _simulation.SetPlayerTurnTimerPaused(_firstBattleGuide != null && _firstBattleGuide.PausesPlayerTimer);
            UpdateForceFieldPulses();

            if (_coachTimer > 0f && !_arsenalMenuOpen)
            {
                _coachTimer -= Time.deltaTime;
            }

            RenderHud();
            UpdateRopeVisual();
            UpdateElectricHarpoonVisual();
            if (_arsenalMenuOpen)
            {
                ClearWalkInput();
                _cameraPanActive = false;
                return;
            }

            RefreshCameraFrameForViewport();
            UpdateCameraZoom();
            UpdateCameraPan();
            UpdateCameraCoverVisuals();
            UpdateMovingArenaSurfaces();

            if (Keyboard.current != null && Keyboard.current.rKey.wasPressedThisFrame)
            {
                RestartScene();
                return;
            }

            if (_simulation.Result.IsFinished)
            {
                return;
            }

            if (_simulation.HasActiveControlledProjectile)
            {
                UpdateControlledProjectileInput();
                _simulation.Tick(Time.deltaTime);
                return;
            }

            if (HasActiveMotion || HasActiveWeaponEffect
                || HasProjectilesInFlight && !CanContinueMinePlacementInput())
            {
                UpdateRopeTraversalInput();
                _simulation.Tick(Time.deltaTime);
                return;
            }

            if (_battleState.Turn.ActiveTeam == BattleTeamId.Opponent)
            {
                UpdateOpponentBot();
                return;
            }

            if (!HasProjectilesInFlight && !HasActiveWeaponEffect && Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
            {
                if (!IsCancelBlockedByGuide())
                {
                    CancelAim();
                }

                return;
            }

            if (!_simulation.Result.IsFinished
                && (!HasProjectilesInFlight || CanContinueMinePlacementInput())
                && !HasActiveWeaponEffect
                && Keyboard.current != null
                && Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                Fire();
                return;
            }

            if (Keyboard.current != null)
            {
                if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
                {
                    SetKeyboardWalkInput(-1);
                }
                else if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
                {
                    SetKeyboardWalkInput(1);
                }
                else
                {
                    SetKeyboardWalkInput(0);
                }

                if (!HasProjectilesInFlight && !HasActiveWeaponEffect && Keyboard.current.qKey.wasPressedThisFrame)
                {
                    TryStartJump(-1);
                    return;
                }

                if (!HasProjectilesInFlight && !HasActiveWeaponEffect && Keyboard.current.eKey.wasPressedThisFrame)
                {
                    TryStartJump(1);
                    return;
                }
            }

            UpdateWalking();
            _simulation.Tick(Time.deltaTime);
            CompletePendingTutorialWalkIfMoved();
            UpdateAim();
            DrawAim();
            DrawTrajectoryPreview();
        }

        private void BuildCamera()
        {
            _camera = Camera.main;
            if (_camera == null)
            {
                var cameraObject = new GameObject("Main Camera");
                _camera = cameraObject.AddComponent<Camera>();
                cameraObject.tag = "MainCamera";
                cameraObject.AddComponent<AudioListener>();
            }

            _camera.orthographic = true;
            _camera.orthographicSize = 5.6f;
            _camera.transform.position = new Vector3(0f, 1.2f, -10f);
            _camera.backgroundColor = new Color(0.54f, 0.78f, 0.95f);
        }

        private void LoadArena(int arenaIndex, string arenaId = null)
        {
            if (_arenas == null || _arenas.Length == 0)
            {
                return;
            }

            ClearArenaRuntime();
            _opponentRingOut = false;
            _playerWeaponSelected = false;
            _cameraPanActive = false;

            _arenaIndex = ResolveArenaIndex(arenaIndex, arenaId);
            _currentArena = _arenas[_arenaIndex];
            _visualTheme = BattleVisualThemeLibrary.ForArena(_currentArena);
            _backgroundWorldOffset = _visualTheme?.WorldOffset ?? Vector2.zero;
            _arenaWorldOffset = _visualTheme?.ArenaContentOffset ?? _backgroundWorldOffset;
            _arenaScale = _visualTheme?.ArenaScale ?? Vector2.one;
            _arenaScalePivot = _visualTheme?.ArenaScalePivot ?? Vector2.zero;
            _cameraTopPanAllowancePercent = _visualTheme?.CameraTopPanAllowancePercent ?? 0f;
            _cameraBottomPanAllowance = _visualTheme?.CameraBottomPanAllowance ?? 0f;
            _killZoneOffsetY = _visualTheme?.KillZoneOffsetY ?? 0f;
            ApplyArenaCamera(_currentArena);
            BuildWorld(_currentArena);
            BuildBattleState(_currentArena);
            BuildUnits();
            _simulationSettings = CreateSimulationSettings(GetFallDeathY());

            _simulation = BattleSimulation.CreatePlayerFirst(
                _battleState,
                _battleTerrain,
                _unitMotors,
                _inventory,
                _opponentInventory,
                _weaponExecutor,
                Physics2D.gravity,
                _simulationSettings,
                _weaponsById,
                _playerArchetype,
                _opponentArchetype,
                _playerProgression,
                _opponentProgression);
            _simulation.ConfigureTitaniumBarrier(Resources.Load<Texture2D>("UI/Weapons/titanium_barrier"),
                _currentArena.CameraSize);
            RefreshArsenalHudItems();
            SubscribeToSimulation(_simulation);
            SetUpDuelNetworking();
            ConfigureSupportDrops();
            _botTurnController = CreateBotTurnController();
            _impactGuideProbe = _impactGuideEnabled
                ? new TrajectoryProbe(
                    _battleTerrain.Grid,
                    Physics2D.gravity,
                    SimulationTickSeconds,
                    BattleSimulation.ProjectileSubStepLength,
                    GetFallDeathY())
                : null;

            _aimState = new AimState(Vector2.zero, Vector2.right, _aimSettings.MinPower, false);
            _resultScreenState = default;
            _arenaReward = BattleArenaReward.None;
            _battleId = System.Guid.NewGuid().ToString("N");
            _status = GameText.BattleIntro(DescribeArena(_currentArena));
            _aimLine.enabled = false;
            HideTrajectoryPreview();
            _aimHadInput = false;
            _aimDragActive = false;
            _uiWalkInputDirection = 0;
            _keyboardWalkInputDirection = 0;
            RefreshUnitViews();
            SeedActivityMovementPosition();
            ReportTutorial(BattleTutorialBridge.MissionStart);
        }

        private int ResolveArenaIndex(int fallbackIndex, string arenaId)
        {
            if (!string.IsNullOrWhiteSpace(arenaId))
            {
                for (var i = 0; i < _arenas.Length; i++)
                {
                    if (_arenas[i] != null && string.Equals(_arenas[i].ArenaId, arenaId, System.StringComparison.Ordinal))
                    {
                        return i;
                    }
                }
            }

            return (fallbackIndex % _arenas.Length + _arenas.Length) % _arenas.Length;
        }

        private void SubscribeToSimulation(BattleSimulation simulation)
        {
            simulation.ShotFired += OnSimulationShotFired;
            simulation.ImpactResolved += OnSimulationImpactResolved;
            simulation.DamageApplied += OnSimulationDamageApplied;
            simulation.TerrainCellsDestroyed += OnSimulationTerrainCellsDestroyed;
            simulation.TerrainCraterApplied += OnSimulationTerrainCraterApplied;
            simulation.UnitDefeated += OnSimulationUnitDefeated;
            simulation.TurnAdvanced += OnSimulationTurnAdvanced;
            simulation.BattleFinished += OnSimulationBattleFinished;
            simulation.CoverDeployed += OnSimulationCoverDeployed;
            simulation.HealthRestored += OnSimulationHealthRestored;
            simulation.OwnerRepositioned += OnSimulationOwnerRepositioned;
            simulation.ProjectileSpawned += OnSimulationProjectileSpawned;
            simulation.ProjectileExploded += OnSimulationProjectileExploded;
            simulation.MineDeployed += OnSimulationMineDeployed;
            simulation.MineTriggered += OnSimulationMineTriggered;
            simulation.GrounderDeployed += OnSimulationGrounderDeployed;
            simulation.GrounderRemoved += OnSimulationGrounderRemoved;
            simulation.GroundObjectCarried += OnSimulationGroundObjectCarried;
            simulation.LaserFenceLinkSpent += OnSimulationLaserFenceLinkSpent;
            simulation.LaserFenceNodeRemoved += OnSimulationLaserFenceNodeRemoved;
            simulation.ForceFieldDeployed += OnSimulationForceFieldDeployed;
            simulation.ForceFieldRemoved += OnSimulationForceFieldRemoved;
            simulation.ForceFieldAbsorbed += OnSimulationForceFieldAbsorbed;
            simulation.LaserFenceNodeDeployed += OnSimulationLaserFenceNodeDeployed;
            simulation.ChargeDeployed += OnSimulationChargeDeployed;
            simulation.GroundObjectRepositioned += OnSimulationGroundObjectRepositioned;
            simulation.LinearWeaponEffectStarted += OnSimulationLinearWeaponEffectStarted;
            simulation.FireBlobSpawned += OnSimulationFireBlobSpawned;
            simulation.SwordSwingStarted += OnSimulationSwordSwingStarted;
            simulation.FirePuddleMoved += OnSimulationFirePuddleMoved;
            simulation.FirePuddleExtinguished += OnSimulationFirePuddleExtinguished;
            simulation.UnitsMoved += OnSimulationUnitsMoved;
        }

        private void SelectWeapon(int weaponIndex)
        {
            EmitDuelCommand(BattleCommandKind.SelectWeapon, weaponIndex);
            if (_inventory == null || !_inventory.HasWeapons)
            {
                return;
            }

            ClearOrbitalTargetMarker();
            _simulation?.CancelTurnTransfer();
            _inventory.Select(weaponIndex);
            var weapon = _inventory.Selected;
            _aimSettings = weapon.CreateAimSettings(_baseAimSettings);
            ResetAimState();
            _barrierPlacementInitialized = false;
            _barrierPlacementScale = 1f;
            _hasPreviousPinchDistance = false;
            _playerWeaponSelected = true;
            RefreshPlayerWeaponVisual();
            _status = _currentArena != null
                ? GameText.WeaponSelected(DescribeWeapon(weapon))
                : GameText.WeaponSelectedGeneric();
        }

        private void OnHudWeaponSelected(int weaponIndex)
        {
            if (!IsBattleReady()
                || _battleState.Turn.ActiveTeam != BattleTeamId.Player
                || HasProjectilesInFlight
                || HasActiveMotion
                || HasActiveWeaponEffect
                || HasPendingGroundAction
                || _simulation.IsAttackCommitted)
            {
                return;
            }

            var weapon = weaponIndex >= 0 && weaponIndex < _inventory.Count ? _inventory.Weapons[weaponIndex] : null;
            if (weapon == null || (_firstBattleGuide != null && !_firstBattleGuide.AllowsWeapon(weapon.WeaponId)))
            {
                return;
            }

            SelectWeapon(weaponIndex);
            _firstBattleGuide?.ObserveWeaponSelected(weapon.WeaponId);
            _cameraPanActive = false;
        }

        private void OnHudArsenalVisibilityChanged(bool visible)
        {
            _arsenalMenuOpen = visible;
            if (visible)
            {
                ClearWalkInput();
                _cameraPanActive = false;
            }
        }

        private void ResetSelectedWeapon()
        {
            if (!IsBattleReady()
                || !_playerWeaponSelected
                || _battleState.Turn.ActiveTeam != BattleTeamId.Player
                || HasProjectilesInFlight
                || HasActiveMotion
                || HasActiveWeaponEffect
                || (_firstBattleGuide != null && _firstBattleGuide.IsActive))
            {
                return;
            }

            _playerWeaponSelected = false;
            ResetAimState();
            _status = GameText.WeaponReset();
        }

        private void ApplyArenaCamera(ArenaDefinition arena)
        {
            _camera.transform.position = arena.CameraPosition;
            _minimumCameraSize = arena.CameraSize * CameraZoomInMultiplier;
            _configuredCameraFrame = _visualTheme?.CameraFrame;
            _cameraWorldBounds = _configuredCameraFrame is Rect configuredFrame
                ? ResolveCameraFrame(configuredFrame, _camera.aspect)
                : CalculateCameraWorldBounds(arena);
            RefreshCameraPanBounds();
            _maximumCameraSize = Mathf.Max(
                _minimumCameraSize,
                CalculateCameraSizeToContainBounds(_cameraWorldBounds, _camera.aspect));
            _camera.orthographicSize = Mathf.Clamp(arena.CameraSize, _minimumCameraSize, _maximumCameraSize);
            _hasPreviousPinchDistance = false;
            ClampCameraToArena();
        }

        private void UpdateCameraZoom()
        {
            if (_camera == null || _minimumCameraSize <= 0f || _maximumCameraSize <= 0f)
            {
                return;
            }

            // Steering a controlled projectile (FPV drone) owns the pointer -
            // camera zoom would fight the piloting input. Since 2026-10-08 drones are piloted with the buttons
            // only, so the fingers are free to zoom and pan the camera during the flight.

            if (IsTitaniumBarrierPlacementActive())
            {
                if (TryGetPinchDistance(out var barrierDistance))
                {
                    if (_hasPreviousPinchDistance && _previousPinchDistance > 0.001f)
                        _barrierPlacementScale = _simulation.ResolveTitaniumBarrierScale(
                            _barrierPlacementScale * barrierDistance / _previousPinchDistance, BattleTeamId.Player);
                    _previousPinchDistance = barrierDistance;
                    _hasPreviousPinchDistance = true;
                    _aimDragActive = false;
                    _cameraPanActive = false;
                }
                else _hasPreviousPinchDistance = false;
                return;
            }

            if (TryGetPinchDistance(out var pinchDistance, out var pinchMidpoint))
            {
                if (_hasPreviousPinchDistance)
                {
                    AdjustCameraZoom(-(pinchDistance - _previousPinchDistance) * PinchZoomSensitivity, pinchMidpoint);
                }

                _previousPinchDistance = pinchDistance;
                _hasPreviousPinchDistance = true;
                _aimDragActive = false;
                _cameraPanActive = false;
                return;
            }

            _hasPreviousPinchDistance = false;

            var zoomDelta = 0f;
            if (Keyboard.current != null)
            {
                if (Keyboard.current.equalsKey.wasPressedThisFrame || Keyboard.current.numpadPlusKey.wasPressedThisFrame)
                {
                    zoomDelta -= CameraZoomStep;
                }

                if (Keyboard.current.minusKey.wasPressedThisFrame || Keyboard.current.numpadMinusKey.wasPressedThisFrame)
                {
                    zoomDelta += CameraZoomStep;
                }
            }

            if (zoomDelta != 0f)
            {
                AdjustCameraZoom(zoomDelta, null);
            }

            if (Mouse.current != null && !IsPointerOverUi())
            {
                var scroll = Mouse.current.scroll.ReadValue().y;
                if (Mathf.Abs(scroll) > 0.01f)
                {
                    AdjustCameraZoom(-Mathf.Sign(scroll) * CameraZoomStep, Mouse.current.position.ReadValue());
                }
            }
        }

        private void UpdateCameraPan()
        {
            if (_camera == null
                || _battleState == null
                || _battleState.Turn.ActiveTeam != BattleTeamId.Player
                || _simulation == null
                || _playerWeaponSelected && !_simulation.HasActiveControlledProjectile
                || _simulation.Result.IsFinished
                || TryGetPinchDistance(out _))
            {
                _cameraPanActive = false;
                return;
            }

            if (!TryReadPointer(out var pressed, out var pressStarted, out var released, out var screenPosition, out var pointerOverUi))
            {
                _cameraPanActive = false;
                return;
            }

            if (pressStarted && !pointerOverUi)
            {
                _cameraPanActive = true;
                _previousCameraPanScreenPosition = screenPosition;
            }

            if (_cameraPanActive && pressed)
            {
                var previousWorld = ScreenToWorld(_previousCameraPanScreenPosition);
                var currentWorld = ScreenToWorld(screenPosition);
                var delta = previousWorld - currentWorld;
                _camera.transform.position += new Vector3(delta.x, delta.y, 0f);
                _previousCameraPanScreenPosition = screenPosition;
                ClampCameraToArena();
            }

            if (released)
            {
                _cameraPanActive = false;
            }
        }

        private bool TryGetPinchDistance(out float distance)
        {
            return TryGetPinchDistance(out distance, out _);
        }

        private bool TryGetPinchDistance(out float distance, out Vector2 midpoint)
        {
            distance = 0f;
            midpoint = default;
            if (Touchscreen.current == null || Touchscreen.current.touches.Count < 2)
            {
                return false;
            }

            var firstTouch = Touchscreen.current.touches[0];
            var secondTouch = Touchscreen.current.touches[1];
            if (!firstTouch.press.isPressed || !secondTouch.press.isPressed)
            {
                return false;
            }

            var firstPosition = firstTouch.position.ReadValue();
            var secondPosition = secondTouch.position.ReadValue();
            distance = Vector2.Distance(firstPosition, secondPosition);
            midpoint = (firstPosition + secondPosition) * 0.5f;
            return true;
        }

        // Keeps the world point under focusScreenPosition fixed on screen
        // while the orthographic size changes, so zoom converges on the
        // cursor/pinch midpoint instead of always snapping the view down to
        // the bottom of the map. A null focus (keyboard zoom) zooms on the
        // current camera center.
        private void AdjustCameraZoom(float sizeDelta, Vector2? focusScreenPosition)
        {
            if (_camera == null || Mathf.Approximately(sizeDelta, 0f) || _minimumCameraSize <= 0f || _maximumCameraSize <= 0f)
            {
                return;
            }

            var newSize = Mathf.Clamp(_camera.orthographicSize + sizeDelta, _minimumCameraSize, _maximumCameraSize);
            if (Mathf.Approximately(newSize, _camera.orthographicSize))
            {
                return;
            }

            Vector2? worldFocusBefore = focusScreenPosition.HasValue
                ? ScreenToWorld(focusScreenPosition.Value)
                : null;

            _camera.orthographicSize = newSize;

            if (worldFocusBefore.HasValue)
            {
                var worldFocusAfter = ScreenToWorld(focusScreenPosition.Value);
                var shift = worldFocusBefore.Value - worldFocusAfter;
                _camera.transform.position += new Vector3(shift.x, shift.y, 0f);
            }

            ClampCameraToArena();
        }

        private static Rect CalculateCameraWorldBounds(ArenaDefinition arena)
        {
            var hasBounds = false;
            var bounds = new Rect(arena.CameraPosition.x, arena.CameraPosition.y, 0f, 0f);
            foreach (var block in arena.TerrainBlocks)
            {
                if (!block.Solid)
                {
                    continue;
                }

                var halfSize = block.Size * 0.5f;
                var minimum = block.Position - halfSize;
                var maximum = block.Position + halfSize;
                if (!hasBounds)
                {
                    bounds = Rect.MinMaxRect(minimum.x, minimum.y, maximum.x, maximum.y);
                    hasBounds = true;
                }
                else
                {
                    bounds.xMin = Mathf.Min(bounds.xMin, minimum.x);
                    bounds.yMin = Mathf.Min(bounds.yMin, minimum.y);
                    bounds.xMax = Mathf.Max(bounds.xMax, maximum.x);
                    bounds.yMax = Mathf.Max(bounds.yMax, maximum.y);
                }
            }

            if (!hasBounds)
            {
                return Rect.MinMaxRect(
                    arena.CameraPosition.x - 9f,
                    arena.CameraPosition.y - 5f,
                    arena.CameraPosition.x + 9f,
                    arena.CameraPosition.y + 5f);
            }

            bounds.xMin -= 2f;
            bounds.xMax += 2f;
            bounds.yMin -= 3f;
            bounds.yMax = Mathf.Max(bounds.yMax + 6f, arena.CameraPosition.y + arena.CameraSize);
            return bounds;
        }

        private void ClampCameraToArena()
        {
            if (_camera == null || _cameraWorldBounds.width <= 0f || _cameraWorldBounds.height <= 0f)
            {
                return;
            }

            var fallbackCenter = _currentArena != null
                ? (Vector2)_currentArena.CameraPosition
                : _cameraWorldBounds.center;
            _camera.transform.position = ClampCameraPositionToBounds(
                _camera.transform.position,
                SelectCameraClampBounds(
                    _cameraWorldBounds,
                    _cameraPanBounds,
                    _currentArena != null && _currentArena.LockCameraAtZoomOutLimit,
                    _camera.orthographicSize,
                    _maximumCameraSize),
                _camera.orthographicSize,
                _camera.aspect,
                fallbackCenter);
        }

        private static Vector3 ClampCameraPositionToBounds(
            Vector3 position,
            Rect bounds,
            float halfHeight,
            float aspect,
            Vector2 fallbackCenter)
        {
            halfHeight = Mathf.Max(0f, halfHeight);
            var halfWidth = halfHeight * Mathf.Max(0.01f, aspect);
            position.x = ClampCameraAxis(position.x, bounds.xMin, bounds.xMax, halfWidth, fallbackCenter.x);
            position.y = ClampCameraAxis(position.y, bounds.yMin, bounds.yMax, halfHeight, fallbackCenter.y);
            return position;
        }

        private static float ClampCameraAxis(float value, float minimum, float maximum, float halfExtent, float fallback)
        {
            var minimumCenter = minimum + halfExtent;
            var maximumCenter = maximum - halfExtent;
            return minimumCenter <= maximumCenter
                ? Mathf.Clamp(value, minimumCenter, maximumCenter)
                : Mathf.Clamp(fallback, minimum, maximum);
        }

        private static Rect SelectCameraClampBounds(
            Rect cameraFrame,
            Rect panBounds,
            bool lockCameraAtZoomOutLimit,
            float currentCameraSize,
            float maximumCameraSize)
        {
            return lockCameraAtZoomOutLimit
                   && Mathf.Abs(currentCameraSize - maximumCameraSize) <= 0.001f
                ? cameraFrame
                : panBounds;
        }

        private static float CalculateCameraSizeToContainBounds(Rect bounds, float aspect)
        {
            if (bounds.width <= 0f || bounds.height <= 0f)
            {
                return 0f;
            }

            return Mathf.Max(bounds.height * 0.5f, bounds.width / (2f * Mathf.Max(0.01f, aspect)));
        }

        private static Rect ResolveCameraFrame(Rect configuredFrame, float aspect)
        {
            aspect = Mathf.Max(0.01f, aspect);
            var requiredWidth = configuredFrame.height * aspect;
            if (requiredWidth > configuredFrame.width)
            {
                var horizontalPadding = (requiredWidth - configuredFrame.width) * 0.5f;
                configuredFrame.xMin -= horizontalPadding;
                configuredFrame.xMax += horizontalPadding;
                return configuredFrame;
            }

            var requiredHeight = configuredFrame.width / aspect;
            if (requiredHeight > configuredFrame.height)
            {
                configuredFrame.yMax = configuredFrame.yMin + requiredHeight;
            }

            return configuredFrame;
        }

        private void RefreshCameraPanBounds()
        {
            _cameraPanBounds = ExpandCameraPanBounds(
                _cameraWorldBounds,
                GetCameraTopPanAllowance(),
                _cameraBottomPanAllowance);
        }

        private static Rect ExpandCameraPanBounds(Rect cameraFrame, float topAllowance, float bottomAllowance)
        {
            cameraFrame.yMin -= Mathf.Max(0f, bottomAllowance);
            cameraFrame.yMax += Mathf.Max(0f, topAllowance);
            return cameraFrame;
        }

        private float GetCameraTopPanAllowance()
        {
            return _cameraWorldBounds.height * _cameraTopPanAllowancePercent;
        }

        private void RefreshCameraFrameForViewport()
        {
            if (_camera == null || !_configuredCameraFrame.HasValue)
            {
                return;
            }

            var resolvedFrame = ResolveCameraFrame(_configuredCameraFrame.Value, _camera.aspect);
            if (Mathf.Approximately(resolvedFrame.xMin, _cameraWorldBounds.xMin)
                && Mathf.Approximately(resolvedFrame.yMin, _cameraWorldBounds.yMin)
                && Mathf.Approximately(resolvedFrame.xMax, _cameraWorldBounds.xMax)
                && Mathf.Approximately(resolvedFrame.yMax, _cameraWorldBounds.yMax))
            {
                return;
            }

            _cameraWorldBounds = resolvedFrame;
            RefreshCameraPanBounds();
            _maximumCameraSize = Mathf.Max(
                _minimumCameraSize,
                CalculateCameraSizeToContainBounds(_cameraWorldBounds, _camera.aspect));
            _camera.orthographicSize = Mathf.Min(_camera.orthographicSize, _maximumCameraSize);
            FitStaticBackgroundToCameraFrame(_staticBackgroundRenderer);
            _animatedBackground?.ConfigureCoverage(_cameraPanBounds, _backgroundWorldOffset, 0f);
            UpdateKillZoneVisual();
            ClampCameraToArena();
        }

        private void BuildWorld(ArenaDefinition arena)
        {
            _battleTerrain = null;

            AddBackgroundPrefab();
            var solidLayer = AddVisualLayer("Arena Background", _visualTheme?.Background, null, out var backgroundRenderer, useBackgroundWorldOffset: true);
            _staticBackgroundRenderer = backgroundRenderer;
            if (backgroundRenderer != null && _visualTheme != null)
            {
                backgroundRenderer.color = _visualTheme.ArenaTint;
            }
            FitStaticBackgroundToCameraFrame(backgroundRenderer);
            var solidLayers = new List<SpriteRenderer>();
            var spriteSurfaces = new List<BattleTerrainSpriteSurface>();
            var solidBlocks = new List<ArenaTerrainBlock>();

            foreach (var block in arena.TerrainBlocks)
            {
                var showProceduralBlock = _visualTheme == null || !_visualTheme.HideProceduralTerrain;
                var solidProceduralBlock = block.Solid && (_visualTheme == null || _visualTheme.UseProceduralTerrainColliders);
                var transformedBlock = TransformArenaBlock(block);
                var blockObject = CreateBox(transformedBlock.BlockId, transformedBlock.Position, transformedBlock.Size, transformedBlock.Color, transformedBlock.SortingOrder, showProceduralBlock);
                _arenaObjects.Add(blockObject);
                if (solidProceduralBlock)
                {
                    solidBlocks.Add(transformedBlock);
                }
            }

            solidLayer = AddVisualLayer("Arena Foreground", _visualTheme?.Foreground, solidLayer);
            if (solidLayer != null)
            {
                solidLayers.Add(solidLayer);
            }

            if (_visualTheme != null)
            {
                for (var i = 0; i < _visualTheme.AdditionalDestructibleLayers.Count; i++)
                {
                    var additionalLayer = AddVisualLayer($"Arena Destructible {i + 1}", _visualTheme.AdditionalDestructibleLayers[i], null);
                    if (additionalLayer != null)
                    {
                        solidLayers.Add(additionalLayer);
                    }
                }
            }

            if (_visualTheme != null)
            {
                for (var i = 0; i < _visualTheme.OverlayLayers.Count; i++)
                {
                    AddVisualLayer($"Arena Overlay {i + 1}", _visualTheme.OverlayLayers[i], null);
                }
            }

            var hasKillZone = _visualTheme?.HasKillZone ?? true;
            var killZoneY = _cameraWorldBounds.yMin + KillZoneBottomInset + _arenaWorldOffset.y + _killZoneOffsetY;
            if (hasKillZone)
            {
                AddKillZoneVisual();
            }

            foreach (var surface in arena.SpriteSurfaces)
            {
                var runtimeSurface = AddArenaSpriteSurface(surface);
                if (runtimeSurface.Renderer == null)
                {
                    continue;
                }

                if (_visualTheme != null)
                {
                    runtimeSurface.Renderer.color = _visualTheme.ArenaTint;
                }

                spriteSurfaces.Add(runtimeSurface);
                if (surface.HasHorizontalMotion)
                {
                    _movingArenaSurfaces.Add(new MovingArenaSurface
                    {
                        Transform = runtimeSurface.Renderer.transform,
                        Origin = runtimeSurface.Renderer.transform.position,
                        Range = surface.HorizontalMotionRange,
                        Speed = surface.HorizontalMotionSpeed
                    });
                }
            }

            ClampCameraToArena();

            var bounds = ExpandArenaBoundsToIncludeSpriteSurfaces(ComputeArenaBounds(arena), spriteSurfaces);
            if (spriteSurfaces.Count > 0)
            {
                for (var i = 0; i < solidLayers.Count; i++)
                {
                    spriteSurfaces.Add(new BattleTerrainSpriteSurface(solidLayers[i], null));
                }

                _battleTerrain = BattleTerrain.CreateFromSpriteSurfaces(spriteSurfaces, bounds);
            }
            else if (solidLayers.Count > 0)
            {
                _battleTerrain = BattleTerrain.CreateFromSprites(solidLayers, bounds);
                if (_battleTerrain == null)
                {
                    Debug.LogWarning($"Destructible layer texture for arena '{arena.ArenaId}' is not readable; falling back to procedural terrain.");
                }
            }

            _battleTerrain ??= BattleTerrain.CreateProcedural(bounds);

            foreach (var block in solidBlocks)
            {
                _battleTerrain.StampSolidRect(block.Position, block.Size, block.Material);
            }

            if (_movingArenaSurfaces.Count > 0)
            {
                _battleTerrain.RefreshMovingSpriteSurfaces();
            }

            // Must run before BuildUnits/CreateSimulationSettings: motors, the
            // simulation and the bot planners all take the kill height from
            // GetFallDeathY(). Arenas without a visible kill zone use the
            // lowest solid terrain line as their out-of-world death boundary.
            _runtimeFallDeathY = hasKillZone
                ? killZoneY
                : ComputeFallDeathY(_battleTerrain.Grid, killZoneY);
        }

        private Rect ComputeArenaBounds(ArenaDefinition arena)
        {
            var cameraHeight = arena.CameraSize * 2f;
            var aspect = _camera != null ? _camera.aspect : 16f / 9f;
            var cameraWidth = cameraHeight * aspect;
            const float margin = 3f;
            var minX = arena.CameraPosition.x - cameraWidth * 0.5f - margin;
            var maxX = arena.CameraPosition.x + cameraWidth * 0.5f + margin;
            var maxY = arena.CameraPosition.y + cameraHeight * 0.5f + margin;
            var minY = Mathf.Min(arena.CameraPosition.y - cameraHeight * 0.5f, GetFallDeathY()) - margin;
            var bounds = new Rect(minX, minY, maxX - minX, maxY - minY);
            if (_cameraWorldBounds.width > 0f && _cameraWorldBounds.height > 0f)
            {
                bounds.xMin = Mathf.Min(bounds.xMin, _cameraWorldBounds.xMin - margin);
                bounds.xMax = Mathf.Max(bounds.xMax, _cameraWorldBounds.xMax + margin);
                bounds.yMin = Mathf.Min(bounds.yMin, _cameraWorldBounds.yMin - margin);
                bounds.yMax = Mathf.Max(bounds.yMax, _cameraWorldBounds.yMax + margin);
            }

            return bounds;
        }

        private void BuildBattleState(ArenaDefinition arena)
        {
            _playerArchetype = PrototypeArchetypeLibrary.PlayerArchetype;
            _opponentArchetype = ResolveOpponentArchetype(GameSessionContext.SelectedOpponentArchetypeId, _aiDifficulty);

            var playerSpawn = ResolveUnitLandingPosition(TransformArenaPosition(arena.PlayerSpawn), BattleTeamId.Player);
            var opponentSpawn = ResolveUnitLandingPosition(TransformArenaPosition(arena.OpponentSpawn), BattleTeamId.Opponent);
            var playerUnit = new BattleUnitState($"player.{_playerArchetype.Id}.01", BattleTeamId.Player, playerSpawn, _playerArchetype.MaxHealth, 1);
            var opponentUnit = new BattleUnitState($"opponent.{_opponentArchetype.Id}.01", BattleTeamId.Opponent, opponentSpawn, _opponentArchetype.MaxHealth, -1);

            var playerTeam = new BattleTeamState(BattleTeamId.Player, new[] { playerUnit });
            var opponentTeam = new BattleTeamState(BattleTeamId.Opponent, new[] { opponentUnit });
            _battleState = new BattleState(new[] { playerTeam, opponentTeam }, new TurnState(BattleTeamId.None, string.Empty, 0, false));
        }

        private CharacterArchetype ArchetypeFor(BattleTeamId teamId)
        {
            return teamId == BattleTeamId.Player ? _playerArchetype : _opponentArchetype;
        }

        private void BuildUnits()
        {
            foreach (var unit in _battleState.Teams.SelectMany(team => team.Units))
            {
                var view = CreateUnitView(unit);
                var presenter = new BattleUnitPresenter(unit, view);
                presenter.Initialize(unit.TeamId == BattleTeamId.Player ? _playerColor : _opponentColor);
                _unitPresenters.Add(presenter);
                _unitMotors[unit.UnitId] = new UnitMotor(
                    _battleTerrain.Grid,
                    CreateMotionProfile(ArchetypeFor(unit.TeamId)),
                    unit.Position,
                    GetFallDeathY());
            }
        }

        private void ConfigureSupportDrops()
        {
            _supportDropTurnInterval = ResolveSupportDropTurnInterval();
            _nextSupportDropTurn = _supportDropTurnInterval == int.MaxValue
                ? int.MaxValue
                : _battleState.Turn.TurnNumber + _supportDropTurnInterval;
            _supportDropInProgress = false;
        }

        private int ResolveSupportDropTurnInterval()
        {
            if (!GameSessionContext.SupportDropsEnabled)
            {
                return int.MaxValue;
            }

            if (GameSessionContext.IsTrainingSkirmish)
            {
                return 3;
            }

            var isStory = GameSessionContext.SelectedBattleMode.IsStory();
            var difficultyOffset = _aiDifficulty switch
            {
                AiDifficulty.Hard => 2,
                AiDifficulty.Normal => 1,
                _ => 0
            };
            return (isStory ? 3 : 4) + difficultyOffset;
        }

        private BattleSimulationSettings CreateSimulationSettings(float fallDeathY)
        {
            return new BattleSimulationSettings(
                fallDeathY,
                SimulationTickSeconds,
                PlayerTurnDurationSeconds,
                OccupiedRadius,
                UnitCollisionHalfWidth,
                KnockbackLandingSearchPadding,
                GroundRayStartHeight,
                GroundRayDistance);
        }

        private float GetFallDeathY()
        {
            return _runtimeFallDeathY;
        }

        /// <summary>
        /// Kill line derived from the loaded terrain: just below the deepest
        /// solid cell. The old fixed constant sat far below sprite arenas, so a
        /// unit knocked past the island's visible bottom kept falling
        /// off-screen without dying (or landed on unseen deep soil and lived).
        /// </summary>
        private static float ComputeFallDeathY(TerrainGrid grid, float fallback)
        {
            for (var y = 0; y < grid.Height; y++)
            {
                for (var x = 0; x < grid.Width; x++)
                {
                    if (grid.IsSolidCell(x, y))
                    {
                        return grid.Origin.y + y * grid.CellSize.y - 0.75f;
                    }
                }
            }

            return fallback;
        }

        private static UnitMotionProfile CreateMotionProfile(CharacterArchetype archetype)
        {
            return new UnitMotionProfile(
                halfWidth: UnitCollisionHalfWidth,
                halfHeight: archetype.BodyHeight * 0.5f,
                crawlHeight: UnitCrawlHeight,
                walkStepHeight: archetype.WalkStepHeight,
                walkSpeed: archetype.WalkSpeed,
                maxWalkSlopeDegrees: archetype.MaxWalkSlopeDegrees);
        }

        private void ClearArenaRuntime()
        {
            StopAllCoroutines();
            _battleTerrain?.Dispose();
            _battleTerrain = null;
            _movingArenaSurfaces.Clear();
            _movingArenaTerrainRefreshTimer = 0f;
            _unitMotors.Clear();
            _simulation = null;
            _botTurnController = null;
            _impactGuideProbe = null;
            _projectileVisuals.Clear();
            _mineVisuals.Clear();
            _grounderVisuals.Clear();
            _forceFieldVisuals.Clear();
            _laserFenceNodeVisuals.Clear();
            _spentLaserFenceLinks.Clear();
            _forceFieldPulseUntil.Clear();
            _laserFenceNodes.Clear();
            _laserFenceLinks.Clear();
            _firePuddleVisuals.Clear();
            _resourcePickups.Clear();
            _hasTrackedPlayerPosition = false;
            _activityBattleRecorded = false;
            _supportDropInProgress = false;
            _nextSupportDropTurn = int.MaxValue;
            _runtimeFallDeathY = FallDeathY;
            _configuredCameraFrame = null;
            _cameraPanBounds = default;
            _arenaWorldOffset = Vector2.zero;
            _backgroundWorldOffset = Vector2.zero;
            _arenaScale = Vector2.one;
            _arenaScalePivot = Vector2.zero;
            _cameraTopPanAllowancePercent = 0f;
            _cameraBottomPanAllowance = 0f;
            _killZoneOffsetY = 0f;
            _staticBackgroundRenderer = null;
            _animatedBackground = null;
            _killZoneVisual = null;

            foreach (var presenter in _unitPresenters)
            {
                if (presenter.View != null)
                {
                    Destroy(presenter.View.gameObject);
                }
            }

            _unitPresenters.Clear();

            foreach (var arenaObject in _arenaObjects)
            {
                if (arenaObject != null)
                {
                    Destroy(arenaObject);
                }
            }

            _arenaObjects.Clear();
            _cameraCoverVisuals.Clear();
            _battleState = null;
            _resultScreenState = default;
            _arenaReward = BattleArenaReward.None;
        }

        private BattleUnitView CreateUnitView(BattleUnitState state)
        {
            var bodyHeight = ArchetypeFor(state.TeamId).BodyHeight;
            var unitObject = new GameObject(state.UnitId);
            unitObject.transform.position = state.Position;

            var rigPrefab = GetUnitRigPrefab(state.TeamId);
            var skin = rigPrefab != null ? null : GetUnitSkin(state.TeamId);
            GameObject rigInstance = null;
            if (rigPrefab != null)
            {
                rigInstance = Object.Instantiate(rigPrefab, unitObject.transform);
                rigInstance.name = "Rig Visual";
                var spriteOffset = _visualTheme != null ? _visualTheme.UnitSpriteOffset : Vector2.zero;
                var spriteScale = _visualTheme != null ? _visualTheme.UnitSpriteScale : Vector2.one;
                rigInstance.transform.localPosition = new Vector3(spriteOffset.x, spriteOffset.y, 0f);
                rigInstance.transform.localScale = new Vector3(spriteScale.x, spriteScale.y, 1f);
                ApplyKnownRigLayoutOverrides(rigInstance.transform, state.TeamId);
            }
            else if (skin == null)
            {
                var bodyObject = new GameObject("Body Visual");
                bodyObject.transform.SetParent(unitObject.transform);
                bodyObject.transform.localScale = new Vector3(0.72f, bodyHeight, 1f);

                var renderer = bodyObject.AddComponent<SpriteRenderer>();
                renderer.sprite = _circleSprite;
                renderer.sortingOrder = 20;
            }
            else
            {
                var bodyObject = new GameObject("Body Visual");
                bodyObject.transform.SetParent(unitObject.transform);
                var spriteOffset = _visualTheme != null ? _visualTheme.UnitSpriteOffset : Vector2.zero;
                var spriteScale = _visualTheme != null ? _visualTheme.UnitSpriteScale : Vector2.one;
                bodyObject.transform.localPosition = new Vector3(spriteOffset.x, spriteOffset.y, 0f);
                bodyObject.transform.localScale = new Vector3(spriteScale.x, spriteScale.y, 1f);

                var renderer = bodyObject.AddComponent<SpriteRenderer>();
                renderer.sprite = skin;
                renderer.color = Color.white;
                renderer.sortingOrder = 20;
            }

            var view = unitObject.AddComponent<BattleUnitView>();
            view.SetPreserveSpriteColor(skin != null || rigInstance != null);
            if ((skin != null || rigInstance != null) && _visualTheme != null)
            {
                view.SetArtFacing(state.TeamId == BattleTeamId.Player
                    ? _visualTheme.PlayerSkinFacingDirectionX
                    : _visualTheme.OpponentSkinFacingDirectionX);
            }

            if (rigInstance != null)
            {
                view.SetVisualRoot(rigInstance.transform);
            }

            if (state.TeamId == BattleTeamId.Player)
            {
                view.SetHealthLabelPositionY(1.3f);
                view.SetHealthLabelScale(0.15f);
            }

            return view;
        }

        private void ApplyKnownRigLayoutOverrides(Transform rigRoot, BattleTeamId teamId)
        {
            if (rigRoot == null || _visualTheme == null)
            {
                return;
            }

            var resourcePath = teamId == BattleTeamId.Player
                ? _visualTheme.PlayerRigResourcePath
                : _visualTheme.OpponentRigResourcePath;
            if (resourcePath != DroidRigResourcePath)
            {
                return;
            }

            var body = rigRoot.Find("Body");
            if (body == null)
            {
                return;
            }

            var position = body.localPosition;
            body.localPosition = new Vector3(position.x, DroidBodyLocalY, position.z);
        }

        private void BuildAimRenderers()
        {
            _aimLine = CreateLine("Aim Line", 0.08f, Color.white);
            _trajectoryLine = CreateLine("Trajectory Preview", 0.035f, new Color(1f, 1f, 1f, 0.55f));
            _ropeLine = CreateLine("Rope Tether", 0.028f, new Color(0.88f, 0.82f, 0.68f, 1f));
            _ropeLine.sortingOrder = 31;
            _electricHarpoonLine = CreateLine("Electric Harpoon Tether", 0.035f, new Color(0.16f, 0.85f, 1f, 1f));
            _electricHarpoonLine.sortingOrder = 32;
            _electricHarpoonExtraLine = CreateLine("Electric Harpoon Second Tether", 0.035f, new Color(0.16f, 0.85f, 1f, 1f));
            _electricHarpoonExtraLine.sortingOrder = 32;
            _electricHarpoonExtraLine.enabled = false;
            BuildTrajectoryDots();
        }

        private void BuildTrajectoryDots()
        {
            EnsureTrajectoryDotCapacity(MaxTrajectoryDotCount);
        }

        private void EnsureTrajectoryDotCapacity(int requiredCount)
        {
            requiredCount = Mathf.Clamp(requiredCount, 0, MaxFullTrajectoryDotCount);
            for (var i = _trajectoryDots.Count; i < requiredCount; i++)
            {
                var dotObject = new GameObject($"Trajectory Dot {i + 1}");
                var renderer = dotObject.AddComponent<SpriteRenderer>();
                renderer.sprite = _circleSprite;
                renderer.sortingOrder = 52;
                renderer.color = Color.clear;
                renderer.enabled = false;
                _trajectoryDots.Add(renderer);
            }
        }

        private void UpdateAim()
        {
            if (_firstBattleGuide != null && _firstBattleGuide.IsActive && !_firstBattleGuide.AllowsAimInput)
            {
                _aimDragActive = false;
                return;
            }

            if (!_playerWeaponSelected)
            {
                _aimDragActive = false;
                return;
            }

            if (_inventory?.Selected?.WeaponId == "weapon.titanium-barrier")
            {
                UpdateTitaniumBarrierPlacement();
                return;
            }

            if (BattleSimulation.IsMarkedStrikeWeapon(_inventory?.Selected))
            {
                UpdateOrbitalTargetInput();
                return;
            }

            if (!BattleSimulation.RequiresAim(_inventory?.Selected))
            {
                _aimDragActive = false;
                _aimDragHadValidPull = false;
                return;
            }

            if (TryGetPinchDistance(out _))
            {
                _aimDragActive = false;
                return;
            }

            var activePresenter = GetActivePresenter();
            if (activePresenter == null)
            {
                _aimDragActive = false;
                return;
            }

            if (!TryReadPointer(out var pressed, out var pressStarted, out var released, out var screenPosition, out var pointerOverUi))
            {
                _aimDragActive = false;
                return;
            }

            var origin = (Vector2)activePresenter.View.FirePoint.position;

            // Slingshot: the press anchors anywhere on screen (away from the unit
            // so the finger never covers the arc), and dragging pulls the shot
            // back. The launch aims opposite the pull. Releasing only commits the
            // aim — the shot itself is fired by the Fire button, never on release.
            if (pressStarted && !pointerOverUi)
            {
                _aimDragActive = true;
                _aimDragHadValidPull = false;
                _aimAnchorWorld = ScreenToWorld(screenPosition);
            }

            if (_aimDragActive && pressed)
            {
                var pull = ScreenToWorld(screenPosition) - _aimAnchorWorld;
                var previousAim = _aimState;

                if (_aimDragHadValidPull
                    && UsesSmoothedDirectAim(_inventory.Selected)
                    && pull.magnitude < DirectAimCenterDeadZone)
                {
                    _aimState = new AimState(origin, previousAim.Direction, previousAim.Power, true);
                }
                else if (_aimDragHadValidPull)
                {
                    // Aim is live in this drag: keep tracking the finger everywhere,
                    // including through the dead zone around the anchor — no freeze,
                    // no arc blink (power rests at MinPower on short pulls).
                    _aimState = AimCalculator.ContinueFromPull(origin, pull, _aimSettings, _aimState);
                }
                else
                {
                    var candidate = AimCalculator.CalculateFromPull(origin, pull, _aimSettings);
                    if (candidate.HasInput)
                    {
                        _aimDragHadValidPull = true;
                        _aimState = candidate;
                    }
                    else if (!_aimState.HasInput)
                    {
                        // No committed aim at all — track the unit so the first
                        // valid pull springs from the right origin.
                        _aimState = candidate;
                    }

                    // Otherwise hold the previously committed aim: a fresh tap must
                    // not yank the frozen arc until it pulls past the dead zone.
                }

                if (UsesSmoothedDirectAim(_inventory.Selected)
                    && previousAim.HasInput
                    && _aimState.HasInput)
                {
                    var smoothing = 1f - Mathf.Exp(-DirectAimSmoothingSpeed * Time.deltaTime);
                    var direction = Vector2.Lerp(previousAim.Direction, _aimState.Direction, smoothing).normalized;
                    _aimState = new AimState(origin, direction, _aimState.Power, true);
                }

                _aimState = ClampRopeAimToUpwardSector(_aimState);

                FaceUnitTowardAim(activePresenter);

                if (_aimState.HasInput && !_aimHadInput)
                {
                    ReportTutorial(BattleTutorialBridge.AimStarted);
                    EmitFeedback(BattleFeedbackEventType.AimStarted);
                }

                _aimHadInput = _aimState.HasInput;
                return;
            }

            if (_aimDragActive && released)
            {
                // Freeze the pulled aim so the arc stays on screen for review;
                // the player fires it with the Fire button.
                _aimDragActive = false;
                _firstBattleGuide?.ObserveAimState(_aimState.HasInput);
            }
        }

        private void UpdateOrbitalTargetInput()
        {
            if (!TryReadPointer(out _, out var pressStarted, out _, out var screenPosition, out var pointerOverUi)
                || !pressStarted || pointerOverUi)
            {
                return;
            }

            var isSecondTap = Time.unscaledTime - _orbitalLastTapTime <= OrbitalTargetDoubleTapSeconds
                && (screenPosition - _orbitalLastTapScreenPosition).sqrMagnitude
                    <= OrbitalTargetDoubleTapDistancePixels * OrbitalTargetDoubleTapDistancePixels;
            _orbitalLastTapTime = Time.unscaledTime;
            _orbitalLastTapScreenPosition = screenPosition;
            if (!isSecondTap || _simulation == null)
            {
                return;
            }

            var target = ScreenToWorld(screenPosition);
            if (_simulation.TrySetOrbitalTarget(target))
            {
                ShowOrbitalTargetMarker(target);
            }
        }

        private void ShowOrbitalTargetMarker(Vector2 point)
        {
            ClearOrbitalTargetMarker(clearSimulation: false);
            if (_circleSprite == null)
            {
                return;
            }

            _orbitalTargetMarkerVisual = new GameObject("Orbital Strike Marker");
            _arenaObjects.Add(_orbitalTargetMarkerVisual);
            _orbitalTargetMarkerVisual.transform.position = point;
            // The same red "×" glyph the modal close buttons use, not a plus sign.
            foreach (var rotation in new[] { 45f, -45f })
            {
                var arm = new GameObject("Cross Arm");
                arm.transform.SetParent(_orbitalTargetMarkerVisual.transform, false);
                arm.transform.localScale = new Vector3(0.34f, 0.055f, 1f);
                arm.transform.localRotation = Quaternion.Euler(0f, 0f, rotation);
                var renderer = arm.AddComponent<SpriteRenderer>();
                renderer.sprite = _circleSprite;
                renderer.color = OrbitalMarkerColor;
                renderer.sortingOrder = 34;
            }
        }

        private static bool ShouldClearOrbitalTargetMarkerOnTurnAdvanced(TurnAdvanceReason reason)
        {
            return reason == TurnAdvanceReason.PlayerTimerExpired;
        }

        private void ClearOrbitalTargetMarker(bool clearSimulation = true)
        {
            _orbitalLastTapTime = -10f;
            if (clearSimulation)
            {
                _simulation?.ClearOrbitalTarget();
            }
            if (_orbitalTargetMarkerVisual == null)
            {
                return;
            }

            _arenaObjects.Remove(_orbitalTargetMarkerVisual);
            Destroy(_orbitalTargetMarkerVisual);
            _orbitalTargetMarkerVisual = null;
        }

        private void UpdateControlledProjectileInput()
        {
            if (_controlledUiHorizontalInput != 0 || _ropeVerticalInput != 0)
            {
                _controlledPointerActive = false;
                _simulation.SetControlledProjectileInput(new Vector2(_controlledUiHorizontalInput, -_ropeVerticalInput));
                return;
            }

            var keyboardDirection = Vector2.zero;
            if (Keyboard.current != null)
            {
                keyboardDirection = new Vector2(
                    (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed ? 1f : 0f)
                    - (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed ? 1f : 0f),
                    (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed ? 1f : 0f)
                    - (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed ? 1f : 0f));
            }

            if (keyboardDirection.sqrMagnitude > 0.01f)
            {
                _controlledPointerActive = false;
                _simulation.SetControlledProjectileInput(keyboardDirection);
                return;
            }

            // Owner decision 2026-10-08: a drone is piloted with the on-screen buttons (or the keyboard) only; a
            // finger on the arena moves and zooms the camera instead.
            _controlledPointerActive = false;
            _simulation.SetControlledProjectileInput(Vector2.zero);
        }

        private static bool UsesSmoothedDirectAim(WeaponDefinition weapon)
        {
            return weapon?.Role == WeaponRole.DirectProjectile
                && weapon.WeaponId != "weapon.fpv-drone";
        }

        private AimState ClampRopeAimToUpwardSector(in AimState aim)
        {
            if (!aim.HasInput || _inventory?.Selected?.WeaponId != "weapon.rope")
            {
                return aim;
            }

            var angle = Mathf.Atan2(aim.Direction.y, aim.Direction.x);
            var aimPurchases = _playerProgression?.ResolveUpgradeCount(
                "weapon.rope", WeaponUpgradePath.Aim) ?? 0;
            var halfSector = WeaponUpgradeChoiceResolver.ResolveRopeAimSectorDegrees(
                aimPurchases) * 0.5f * Mathf.Deg2Rad;
            var upward = Mathf.PI * 0.5f;
            var constrained = Mathf.Clamp(angle, upward - halfSector, upward + halfSector);
            var direction = new Vector2(Mathf.Cos(constrained), Mathf.Sin(constrained));
            return new AimState(aim.Origin, direction, aim.Power, true);
        }

        private void FaceUnitTowardAim(BattleUnitPresenter presenter)
        {
            if (presenter == null || !_aimState.HasInput)
            {
                return;
            }

            if (Mathf.Abs(_aimState.Direction.x) >= 0.01f)
            {
                presenter.SetFacing(_aimState.Direction.x > 0f ? 1 : -1);
            }

            presenter.SetWeaponAim(_aimState.Direction);
            var origin = presenter.View.WeaponMuzzlePosition - _aimState.Direction * ProjectileMuzzleOffset;
            _aimState = new AimState(
                origin,
                _aimState.Direction,
                _aimState.Power,
                true);
        }

        private static AiDifficulty MapDifficulty(int tier)
        {
            return tier switch
            {
                >= 2 => AiDifficulty.Hard,
                1 => AiDifficulty.Normal,
                _ => AiDifficulty.Easy
            };
        }

        private static CharacterArchetype ResolveOpponentArchetype(string archetypeId, AiDifficulty fallbackDifficulty)
        {
            return PrototypeArchetypeLibrary.TryGetById(archetypeId, out var archetype)
                ? archetype
                : PrototypeArchetypeLibrary.ForDifficulty(fallbackDifficulty);
        }

        private void ReportTutorial(string triggerKey)
        {
            if (GameSessionContext.IsTrainingSkirmish)
            {
                return;
            }

            // The first training battle owns its full lesson through
            // FirstBattleGuideState. Generic short-lived cues would otherwise
            // reopen the instruction card after its final required shot.
            if (FirstBattleGuideState.IsFirstMission(GameSessionContext.SelectedMissionId))
            {
                return;
            }

            var cue = BattleTutorialBridge.Report(triggerKey);
            if (!cue.IsEmpty)
            {
                _coachMessage = cue.Message;
                _coachTargetId = cue.UiTargetId;
                _coachTimer = 4.5f;
            }
        }

        private static void EmitFeedback(BattleFeedbackEventType type)
        {
            var key = type switch
            {
                BattleFeedbackEventType.AimStarted => BattleFeedbackBridge.Aim,
                BattleFeedbackEventType.ShotFired => BattleFeedbackBridge.Shot,
                BattleFeedbackEventType.ProjectileImpact => BattleFeedbackBridge.Impact,
                BattleFeedbackEventType.DamageResolved => BattleFeedbackBridge.Damage,
                BattleFeedbackEventType.UnitDefeated => BattleFeedbackBridge.Defeat,
                BattleFeedbackEventType.BattleFinished => BattleFeedbackBridge.Finished,
                _ => string.Empty
            };

            if (!string.IsNullOrEmpty(key))
            {
                BattleFeedbackBridge.Report(key);
            }
        }

        private bool TryReadPointer(
            out bool pressed,
            out bool pressStarted,
            out bool released,
            out Vector2 screenPosition,
            out bool pointerOverUi)
        {
            pressed = false;
            pressStarted = false;
            released = false;
            screenPosition = default;
            pointerOverUi = false;

            if (_camera == null)
            {
                return false;
            }

            if (Touchscreen.current != null)
            {
                var touch = Touchscreen.current.primaryTouch;
                pressed = touch.press.isPressed;
                pressStarted = touch.press.wasPressedThisFrame;
                released = touch.press.wasReleasedThisFrame;
                screenPosition = touch.position.ReadValue();
                pointerOverUi = IsPointerOverUi((int)touch.touchId.ReadValue());
                return true;
            }

            if (Mouse.current != null)
            {
                var button = Mouse.current.leftButton;
                pressed = button.isPressed;
                pressStarted = button.wasPressedThisFrame;
                released = button.wasReleasedThisFrame;
                screenPosition = Mouse.current.position.ReadValue();
                pointerOverUi = IsPointerOverUi();
                return true;
            }

            return false;
        }

        private Vector2 ScreenToWorld(Vector2 screenPosition)
        {
            return _camera.ScreenToWorldPoint(screenPosition);
        }

        private void Fire()
        {
            if (!IsBattleReady())
            {
                return;
            }

            if (!_playerWeaponSelected)
            {
                _status = GameText.NoWeaponSelected();
                return;
            }

            if (_firstBattleGuide != null && _firstBattleGuide.IsActive && !_firstBattleGuide.AllowsFire)
            {
                return;
            }

            FireWithAim(_aimState);
        }

        /// <summary>
        /// Fires with an explicit aim, whatever produced it - the local player's
        /// drag, the bot, or a command that arrived over the wire. The checks in
        /// <see cref="Fire"/> guard the local input path only; a remote command has
        /// already been through its own authority checks and must not be refused
        /// because of this client's UI state.
        /// </summary>
        private void FireWithAim(AimState aim)
        {
            if (!IsBattleReady())
            {
                return;
            }

            EmitDuelCommand(BattleCommandKind.Fire, 0, aim.Origin, aim.Direction, aim.Power);

            var result = _simulation.TryFire(aim);
            if (BattleSimulation.IsMarkedStrikeWeapon(result.Weapon)
                && result.Outcome == BattleFireOutcome.UtilityUsed)
            {
                ClearOrbitalTargetMarker();
            }

            // Opening the turn transfer choice shows the whole arena so every ally can be seen.
            if (_simulation.HasTurnTransferSelection && _camera != null && _maximumCameraSize > 0f)
            {
                _camera.orthographicSize = _maximumCameraSize;
                ClampCameraToArena();
            }

            switch (result.Outcome)
            {
                case BattleFireOutcome.NoActiveUnit:
                    HideTrajectoryPreview();
                    break;
                case BattleFireOutcome.AimRequired:
                    _status = GameText.AimRequired();
                    break;
                case BattleFireOutcome.OutOfAmmo:
                    _status = GameText.AmmoDepleted(DescribeWeapon(result.Weapon));
                    break;
                case BattleFireOutcome.NoWeaponSelected:
                    _status = GameText.NoWeaponSelected();
                    break;
                case BattleFireOutcome.Fired:
                    if (result.Team == BattleTeamId.Player)
                    {
                        _firstBattleGuide?.ObservePlayerShotFired();
                        ReportTutorial(BattleTutorialBridge.ShotFired);
                    }

                    break;
                case BattleFireOutcome.BurstFired:
                    SpawnImpactFlash(result.EffectPoint, new Color(1f, 0.62f, 0.24f, 0.85f), 0.95f);
                    if (result.HitCount > 0)
                    {
                        EmitFeedback(BattleFeedbackEventType.DamageResolved);
                    }

                    break;
            }
        }

        private void OnSimulationProjectileSpawned(SimProjectile projectile)
        {
            var projectileObject = new GameObject("Production Shell");
            _arenaObjects.Add(projectileObject);
            _projectileVisuals[projectile.Id] = projectileObject;
            projectileObject.transform.position = projectile.Position;

            var visualObject = new GameObject("Visual");
            visualObject.transform.SetParent(projectileObject.transform, false);
            ApplyProjectileVisualTransform(visualObject.transform, projectile);

            var weapon = ResolveWeaponFromProjectileId(projectile.Definition.ProjectileId);

            var renderer = visualObject.AddComponent<SpriteRenderer>();
            renderer.sprite = ResolveProjectileSprite(projectile, weapon) ?? _circleSprite;
            renderer.color = renderer.sprite == _circleSprite ? ResolveProjectileVisualColor(weapon) : Color.white;
            renderer.sortingOrder = 30;

            if (weapon?.WeaponId == "weapon.fpv-drone")
            {
                var animator = visualObject.AddComponent<WeaponProjectileSpriteAnimator>();
                animator.SetFrames(LoadWeaponIconSprite("fpv_0"), LoadWeaponIconSprite("fpv_1"),
                    LoadWeaponIconSprite("fpv_2"), LoadWeaponIconSprite("fpv_3"), LoadWeaponIconSprite("fpv_4"));
            }
            else if (weapon?.WeaponId == "weapon.bur-rocket")
            {
                SpawnRocketTrail(visualObject.transform, renderer, true);
            }
            else if (weapon?.WeaponId == "weapon.sky-hook")
            {
                SpawnSkyHookTail(visualObject.transform, renderer);
            }
        }

        /// <summary>
        /// Drill rockets don't get a fully baked "in flight" sprite per rank/branch
        /// body variant - instead the shared exhaust trail art is overlaid as a
        /// second layer behind the body sprite. The homing missile uses its own
        /// baked missle_fly sprite and must not receive this overlay.
        /// </summary>
        /// <summary>The shared exhaust is attached under the nose-up signal rocket body.</summary>
        private void SpawnSkyHookTail(Transform visualTransform, SpriteRenderer bodyRenderer)
        {
            var trailSprite = LoadCenteredTextureSprite(RocketTrailResourcePath, "Rocket Exhaust Trail");
            if (trailSprite == null || bodyRenderer.sprite == null)
            {
                return;
            }

            var trailObject = new GameObject("Rocket Trail");
            trailObject.transform.SetParent(visualTransform, false);
            // The exhaust art trails to the left; a quarter turn makes it trail downwards.
            trailObject.transform.localRotation = Quaternion.Euler(0f, 0f, 90f);
            var baseY = -bodyRenderer.sprite.bounds.extents.y * 0.92f;
            var trailLength = trailSprite.bounds.size.x;
            trailObject.transform.localPosition = new Vector3(0f, baseY - trailLength * 0.35f, 0f);
            var trailRenderer = trailObject.AddComponent<SpriteRenderer>();
            trailRenderer.sprite = trailSprite;
            trailRenderer.sortingOrder = 29;
            // The body is scaled down about 20x; the exhaust must follow it.
            trailObject.transform.localScale = Vector3.one * 1.0f;
        }

        private void SpawnRocketTrail(Transform visualTransform, SpriteRenderer bodyRenderer, bool isDrillRocket)
        {
            var trailSprite = LoadCenteredTextureSprite(RocketTrailResourcePath, "Rocket Exhaust Trail");
            if (trailSprite == null || bodyRenderer.sprite == null)
            {
                return;
            }

            var trailObject = new GameObject("Rocket Trail");
            trailObject.transform.SetParent(visualTransform, false);

            // Body sprites face +X (nose right); the engine sits inset from the
            // sprite's left edge rather than exactly on it. Best-effort placement -
            // nudge RocketTrailAttachFraction if the flame doesn't line up with the
            // engine once seen in the editor.
            var bodyHalfWidth = bodyRenderer.sprite.bounds.extents.x;
            var trailHalfWidth = trailSprite.bounds.extents.x;
            var attachX = -bodyHalfWidth * RocketTrailAttachFraction;
            trailObject.transform.localPosition = new Vector3(attachX - trailHalfWidth * 0.7f, 0f, 0f);

            var trailRenderer = trailObject.AddComponent<SpriteRenderer>();
            trailRenderer.sprite = trailSprite;
            // The drill rocket has a larger body and a heavier exhaust plume;
            // extend only its shared tail so the ordinary missile keeps its
            // established silhouette.
            trailObject.transform.localScale = isDrillRocket ? Vector3.one * 1.35f : Vector3.one;
            trailRenderer.sortingOrder = 29;
        }

        private void OnSimulationProjectileExploded(int projectileId, ProjectileImpact impact)
        {
            if (_projectileVisuals.TryGetValue(projectileId, out var projectileObject))
            {
                _projectileVisuals.Remove(projectileId);
                _arenaObjects.Remove(projectileObject);
                Destroy(projectileObject);
            }

            if (_mineVisuals.TryGetValue(projectileId, out var mineObject))
            {
                _mineVisuals.Remove(projectileId);
                _mineOwners.Remove(projectileId);
                _arenaObjects.Remove(mineObject);
                Destroy(mineObject);
            }

            var impactWeapon = ResolveWeaponFromProjectileId(impact.ProjectileId);
            if (impactWeapon?.WeaponId == "weapon.hopping-grenade")
            {
                _hideHeldWeaponForReusableThrow = false;
            }
            if (impactWeapon?.WeaponId == "weapon.teleport")
            {
                SpawnTeleportMarkerVisual(impact.Point);
            }

            if (impactWeapon?.WeaponId != "weapon.teleport"
                && impactWeapon?.WeaponId != "weapon.ion-shotgun"
                && impact.ProjectileId != "projectile.weapon.mine.deploy"
                && impact.ProjectileId != "projectile.weapon.laser-fence.deploy")
            {
                var isSmallMortarCharge = impact.ProjectileId != null
                    && (impact.ProjectileId.StartsWith("projectile.weapon.mortira.fragment", System.StringComparison.Ordinal)
                        || impact.ProjectileId.StartsWith("projectile.weapon.mortira.cascade.", System.StringComparison.Ordinal));
                var isMissileClusterCharge = impact.ProjectileId != null
                    && impact.ProjectileId.StartsWith("projectile.weapon.missle.cluster.", System.StringComparison.Ordinal);
                var flashScale = isSmallMortarCharge || isMissileClusterCharge ? 0.6f : 1.2f;
                SpawnImpactFlash(impact.Point, new Color(1f, 0.74f, 0.18f, 0.85f), flashScale);
            }
            if (!string.IsNullOrWhiteSpace(impact.OwnerUnitId)
                && impact.OwnerUnitId.StartsWith("player.", System.StringComparison.Ordinal))
            {
                _firstBattleGuide?.ObserveProjectileImpact();
                if (IsPlayerTurnShotSpent())
                {
                    // The turn's shot is spent: the next item stays out of the hands and there is nothing to aim.
                    if (_playerWeaponSelected)
                    {
                        _playerWeaponSelected = false;
                        ResetAimState();
                        RefreshPlayerWeaponVisual();
                    }
                }
                else if (ReturnsHeldWeaponAfterFlight(impactWeapon)
                    && _inventory?.Selected?.WeaponId == impactWeapon.WeaponId
                    && _inventory.GetSelectedRemainingAmmo() > 0)
                {
                    _playerWeaponSelected = true;
                    RefreshPlayerWeaponVisual();
                }
            }
        }

        private void OnSimulationMineDeployed(MineDeployedEvent mine)
        {
            var visual = new GameObject("Mine Armed");
            _arenaObjects.Add(visual);
            _mineVisuals[mine.MineId] = visual;
            _mineOwners[mine.MineId] = (mine.Weapon, mine.Team);
            visual.transform.position = mine.Point;
            var renderer = visual.AddComponent<SpriteRenderer>();
            if (mine.Weapon != null && mine.Weapon.WeaponId == "weapon.spider-mine")
            {
                var framePrefix = ResolveMineSpriteId("spider_mine", mine.Weapon, mine.Team) == "spider_mine_5A"
                    ? "spider_mine_5A_frame_"
                    : "spider_mine_frame_";
                var spiderAnimator = visual.AddComponent<WeaponProjectileSpriteAnimator>();
                spiderAnimator.SetFrames(
                    LoadWeaponIconSprite($"{framePrefix}00"),
                    LoadWeaponIconSprite($"{framePrefix}01"),
                    LoadWeaponIconSprite($"{framePrefix}02"),
                    LoadWeaponIconSprite($"{framePrefix}03"));
            }
            else if (mine.Weapon != null && mine.Weapon.WeaponId == "weapon.guardian-drone")
            {
                renderer.sprite = LoadWeaponIconSprite(ResolveMineSpriteId(mine.Weapon.IconId, mine.Weapon, mine.Team));
                // The drone is visible to everyone until the purchased stealth hides it from the
                // enemy; the owner always sees it, and it is revealed again when it fires.
                renderer.enabled = !(mine.ConcealedFromEnemy && mine.Team != BattleTeamId.Player);
            }
            else
            {
                renderer.sprite = LoadWeaponIconSprite(ResolveMineSpriteId("mine_off", mine.Weapon, mine.Team));
            }
            renderer.sortingOrder = 27;
            visual.transform.localScale = Vector3.one * GuardianPlacedVisualScale;
        }

        private const string GuardianDeployProjectileId = "projectile.weapon.guardian-drone.deploy";
        private const float GrounderPlacedVisualScale = 0.11f;
        private const float GuardianPlacedVisualScale = 0.11f;

        private void OnSimulationMineTriggered(int mineId)
        {
            if (_mineVisuals.TryGetValue(mineId, out var visual)
                && visual != null
                && visual.TryGetComponent<SpriteRenderer>(out var renderer))
            {
                var (weapon, team) = _mineOwners.TryGetValue(mineId, out var owner) ? owner : (null, BattleTeamId.None);
                if (weapon?.WeaponId == "weapon.guardian-drone")
                {
                    renderer.enabled = true;
                    _mineVisuals.Remove(mineId);
                    _mineOwners.Remove(mineId);
                    _arenaObjects.Remove(visual);
                    Destroy(visual, 0.28f);
                }
                else if (weapon == null || weapon.WeaponId != "weapon.spider-mine")
                {
                    renderer.sprite = LoadWeaponIconSprite(ResolveMineSpriteId("mine_on", weapon, team));
                }
            }
        }

        private void OnSimulationGrounderDeployed(GrounderDeployedEvent grounder)
        {
            var visual = new GameObject("Grounder Placed");
            _arenaObjects.Add(visual);
            _grounderVisuals[grounder.Id] = visual;
            visual.transform.localScale = Vector3.one * GrounderPlacedVisualScale;
            var renderer = visual.AddComponent<SpriteRenderer>();
            renderer.sprite = LoadWeaponIconSprite(grounder.Weapon?.IconId ?? "grounder");
            renderer.sortingOrder = 27;
            // The magnet's base stands on the ground instead of being half sunk into it.
            visual.transform.position = grounder.Point + Vector2.up * GroundStandingLift(renderer.sprite, GrounderPlacedVisualScale);
        }

        /// <summary>
        /// How far above a ground deploy point (which sits <see cref="GroundDeployPointLift"/> over the surface) a
        /// centre-pivoted device sprite of this world scale must be raised so its bottom edge rests on the surface.
        /// </summary>
        private static float GroundStandingLift(Sprite sprite, float worldScale)
        {
            return sprite == null ? 0f : -sprite.bounds.min.y * worldScale - GroundDeployPointLift;
        }

        private const float GroundDeployPointLift = 0.06f;

        private void OnSimulationForceFieldDeployed(ForceFieldDeployedEvent field)
        {
            var visual = new GameObject("Force Field Placed");
            _arenaObjects.Add(visual);
            _forceFieldVisuals[field.Id] = visual;
            visual.transform.position = field.Point;
            var renderer = visual.AddComponent<SpriteRenderer>();
            renderer.sprite = LoadWeaponIconSprite("force_field_sphere");
            renderer.sortingOrder = 26;
            if (renderer.sprite != null)
            {
                // The sphere art spans the field's diameter.
                visual.transform.localScale = Vector3.one * (field.Radius * 2f / renderer.sprite.bounds.size.x);
            }

            // The device itself stands on the ground at the centre of its sphere.
            var device = new GameObject("Force Field Device");
            device.transform.SetParent(visual.transform, false);
            var deviceRenderer = device.AddComponent<SpriteRenderer>();
            deviceRenderer.sprite = LoadWeaponIconSprite(field.Weapon?.IconId ?? "force_field");
            deviceRenderer.sortingOrder = 27;
            var parentScale = Mathf.Max(0.0001f, visual.transform.localScale.x);
            device.transform.localScale = Vector3.one * (GrounderPlacedVisualScale / parentScale);
            // Its bottom rests on the surface; the sphere stays centred on the deploy point around it.
            device.transform.localPosition = Vector3.up
                * (GroundStandingLift(deviceRenderer.sprite, GrounderPlacedVisualScale) / parentScale);
        }

        private void OnSimulationForceFieldRemoved(int fieldId)
        {
            _forceFieldPulseUntil.Remove(fieldId);
            if (!_forceFieldVisuals.TryGetValue(fieldId, out var visual))
            {
                return;
            }

            _forceFieldVisuals.Remove(fieldId);
            _arenaObjects.Remove(visual);
            Destroy(visual);
        }

        private void OnSimulationForceFieldAbsorbed(int fieldId, Vector2 point)
        {
            _forceFieldPulseUntil[fieldId] = Time.time + 0.3f;
            if (_forceFieldVisuals.TryGetValue(fieldId, out var visual) && visual != null
                && visual.TryGetComponent<SpriteRenderer>(out var renderer))
            {
                renderer.color = new Color(0.7f, 0.95f, 1f, 1f);
            }
        }

        private void UpdateForceFieldPulses()
        {
            if (_forceFieldPulseUntil.Count == 0)
            {
                return;
            }

            List<int> finished = null;
            foreach (var pair in _forceFieldPulseUntil)
            {
                if (Time.time < pair.Value)
                {
                    continue;
                }

                (finished ??= new List<int>()).Add(pair.Key);
            }

            if (finished == null)
            {
                return;
            }

            foreach (var id in finished)
            {
                _forceFieldPulseUntil.Remove(id);
                if (_forceFieldVisuals.TryGetValue(id, out var visual) && visual != null
                    && visual.TryGetComponent<SpriteRenderer>(out var renderer))
                {
                    renderer.color = Color.white;
                }
            }
        }

        private void OnSimulationGrounderRemoved(int grounderId)
        {
            if (!_grounderVisuals.TryGetValue(grounderId, out var visual))
            {
                return;
            }

            _grounderVisuals.Remove(grounderId);
            _arenaObjects.Remove(visual);
            Destroy(visual);
        }

        private void OnSimulationLaserFenceNodeDeployed(LaserFenceNodeDeployedEvent node)
        {
            var visual = new GameObject("Laser Fence Node");
            _arenaObjects.Add(visual);
            _laserFenceNodeVisuals[node.NodeId] = visual;
            visual.transform.position = node.Point;
            var renderer = visual.AddComponent<SpriteRenderer>();
            renderer.sprite = LoadWeaponIconSprite(node.Weapon?.IconId ?? "laser_fence");
            renderer.sortingOrder = 27;
            // Half of the former 0.11: the hulls are small marks on the ground.
            visual.transform.localScale = Vector3.one * 0.055f;

            _laserFenceNodes[node.NodeId] = node;
            RebuildLaserFenceLinks();
        }

        private void OnSimulationLaserFenceLinkSpent(int firstNodeId, int secondNodeId)
        {
            _spentLaserFenceLinks.Add((firstNodeId, secondNodeId));
            RebuildLaserFenceLinks();
        }

        private void OnSimulationLaserFenceNodeRemoved(int nodeId)
        {
            if (_laserFenceNodeVisuals.TryGetValue(nodeId, out var visual))
            {
                _laserFenceNodeVisuals.Remove(nodeId);
                _arenaObjects.Remove(visual);
                if (visual != null)
                {
                    Destroy(visual);
                }
            }

            _laserFenceNodes.Remove(nodeId);
            _spentLaserFenceLinks.RemoveWhere(pair => pair.Item1 == nodeId || pair.Item2 == nodeId);
            RebuildLaserFenceLinks();
        }

        private void RebuildLaserFenceLinks()
        {
            foreach (var line in _laserFenceLinks)
            {
                if (line == null)
                {
                    continue;
                }

                _arenaObjects.Remove(line.gameObject);
                Destroy(line.gameObject);
            }

            _laserFenceLinks.Clear();
            var links = LaserFenceLinkResolver.Resolve(_laserFenceNodes.Values.ToList(),
                owner => _simulation != null ? _simulation.GetLaserFenceLinkRange(owner) : LaserFenceLinkRange);
            foreach (var link in links)
            {
                if (_spentLaserFenceLinks.Contains((link.FirstNodeId, link.SecondNodeId))
                    || _spentLaserFenceLinks.Contains((link.SecondNodeId, link.FirstNodeId)))
                {
                    continue;
                }

                if (!_laserFenceNodes.TryGetValue(link.FirstNodeId, out var first)
                    || !_laserFenceNodes.TryGetValue(link.SecondNodeId, out var second))
                {
                    continue;
                }

                var line = CreateLine("Laser Fence Link", 0.035f, LaserFenceLinkColor);
                line.sortingOrder = 26;
                line.enabled = true;
                line.SetPosition(0, first.Point);
                line.SetPosition(1, second.Point);
                _laserFenceLinks.Add(line);
                _arenaObjects.Add(line.gameObject);
            }
        }

        /// <summary>
        /// Rank/branch-aware mine sprite (armed/triggered), resolved from whichever
        /// side laid it. A raid or duel opponent fielding an evolved mine must look
        /// like it: art that lies about what is on the ground is worse than no art.
        /// </summary>
        private string ResolveMineSpriteId(string baseIconId, WeaponDefinition weapon, BattleTeamId team)
        {
            return ResolveEvolvedIconId(baseIconId, weapon, ProgressionForTeam(team));
        }

        private BattleWeaponProgression ProgressionForTeam(BattleTeamId team)
        {
            return team == BattleTeamId.Player ? _playerProgression : _opponentProgression;
        }

        /// <summary>
        /// Evolved art for <paramref name="baseIconId"/> at the rank/branch
        /// <paramref name="progression"/> holds, or the base icon when the weapon has
        /// not evolved or has no evolved art authored.
        /// </summary>
        private static string ResolveEvolvedIconId(
            string baseIconId,
            WeaponDefinition weapon,
            BattleWeaponProgression progression)
        {
            if (weapon == null || progression == null || string.IsNullOrEmpty(baseIconId))
            {
                return baseIconId;
            }

            return progression.ResolveCurrentIconId(weapon, baseIconId);
        }

        private void OnSimulationChargeDeployed(ChargeDeployedEvent charge)
        {
            var visual = new GameObject("Ground Charge");
            _arenaObjects.Add(visual);
            _mineVisuals[charge.ChargeId] = visual;
            var renderer = visual.AddComponent<SpriteRenderer>();
            renderer.sprite = LoadWeaponIconSprite(charge.Weapon.IconId);
            renderer.sortingOrder = 27;
            // Same visual size as the charge held in the robot's hands
            // (weapon mount scale), so the drop does not "grow" the dynamite.
            visual.transform.localScale = Vector3.one * 0.085f;
            visual.AddComponent<GroundDropVisual>().Play(
                charge.Origin,
                charge.Point,
                charge.DropDurationSeconds);

            if (charge.Team == BattleTeamId.Player)
            {
                _playerWeaponSelected = false;
                RefreshPlayerWeaponVisual();
            }
        }

        /// <summary>A carried item is drawn over the copter's body; a released one returns to the ground layer.</summary>
        private readonly HashSet<int> _releasedCargoIds = new();

        private void OnSimulationGroundObjectCarried(int objectId, bool carried)
        {
            if (carried)
            {
                _releasedCargoIds.Remove(objectId);
            }
            else
            {
                _releasedCargoIds.Add(objectId);
            }

            GameObject visual = null;
            var pickup = FindResourcePickupBySimulationId(objectId);
            if (pickup != null)
            {
                pickup.IsCarried = carried;
                visual = pickup.Visual;
            }
            else if (!_mineVisuals.TryGetValue(objectId, out visual) && !_laserFenceNodeVisuals.TryGetValue(objectId, out visual))
            {
                return;
            }

            if (visual != null && visual.TryGetComponent<SpriteRenderer>(out var renderer))
            {
                renderer.sortingOrder = carried ? 31 : 27;
            }
        }

        private void OnSimulationGroundObjectRepositioned(GroundObjectRepositionedEvent groundObject)
        {
            var released = _releasedCargoIds.Remove(groundObject.ObjectId);
            var pickup = FindResourcePickupBySimulationId(groundObject.ObjectId);
            if (pickup != null)
            {
                var from = pickup.Visual != null ? (Vector2)pickup.Visual.transform.position : pickup.Position;
                pickup.Position = groundObject.Point + Vector2.up * pickup.VisualLift;
                if (pickup.Visual != null)
                {
                    MoveGroundObjectVisual(pickup.Visual, from, pickup.Position, released);
                }

                return;
            }

            if (_laserFenceNodeVisuals.TryGetValue(groundObject.ObjectId, out var nodeVisual) && nodeVisual != null
                && _laserFenceNodes.TryGetValue(groundObject.ObjectId, out var node))
            {
                MoveGroundObjectVisual(nodeVisual, nodeVisual.transform.position, groundObject.Point, released);
                _laserFenceNodes[groundObject.ObjectId] = new LaserFenceNodeDeployedEvent(
                    node.NodeId, node.OwnerUnitId, groundObject.Point, node.Weapon, node.Team);
                RebuildLaserFenceLinks();
                return;
            }

            if (!_mineVisuals.TryGetValue(groundObject.ObjectId, out var visual) || visual == null)
            {
                return;
            }

            if (visual.TryGetComponent<GroundDropVisual>(out var dropVisual))
            {
                Destroy(dropVisual);
            }

            MoveGroundObjectVisual(visual, visual.transform.position, groundObject.Point, released);
        }

        /// <summary>A released cargo falls visibly to its landing point; any other move is applied at once.</summary>
        private static void MoveGroundObjectVisual(GameObject visual, Vector2 from, Vector2 to, bool released)
        {
            if (visual.TryGetComponent<CargoFallVisual>(out var falling))
            {
                Destroy(falling);
            }

            if (released && to.y < from.y - 0.05f)
            {
                visual.AddComponent<CargoFallVisual>().Play(from, to);
                return;
            }

            visual.transform.position = to;
        }

        private void OnSimulationLinearWeaponEffectStarted(LinearWeaponEffectEvent effect)
        {
            var visual = new GameObject("Linear Weapon Effect");
            _arenaObjects.Add(visual);
            var renderer = visual.AddComponent<SpriteRenderer>();
            renderer.sprite = LoadWeaponIconSprite(effect.Weapon.WeaponId == "weapon.bur" ? "bur_lazer" : effect.Weapon.IconId);
            renderer.sortingOrder = 32;
            var animator = visual.AddComponent<LinearWeaponEffectVisual>();
            animator.SetThickness(effect.Weapon.WeaponId == "weapon.bur" ? 1.5f : 1f);
            animator.Play(effect.Origin, effect.Direction, effect.Range, effect.GrowthDurationSeconds);
            Destroy(visual, effect.LifetimeSeconds + 0.08f);
        }

        private void OnSimulationSwordSwingStarted(SwordSwingEvent swing)
        {
            var texture = Resources.Load<Texture2D>("UI/Weapons/plasma_sword");
            if (texture == null || texture.width <= 0 || texture.height <= 0)
            {
                return;
            }

            var visual = new GameObject("Plasma Sword Swing");
            _arenaObjects.Add(visual);
            visual.transform.position = swing.Origin;
            visual.transform.rotation = Quaternion.Euler(
                0f,
                0f,
                Mathf.Atan2(swing.Direction.y, swing.Direction.x) * Mathf.Rad2Deg);

            var hiltRenderer = visual.AddComponent<SpriteRenderer>();
            hiltRenderer.sprite = Sprite.Create(
                texture,
                // Unity imports this art at a different size than the original
                // source. These normalized crop bounds stay inside the texture.
                new Rect(texture.width * 0.0957f, 0f, texture.width * 0.5104f, texture.height * 0.5423f),
                new Vector2(0.53f, 0.71f),
                100f,
                0,
                SpriteMeshType.FullRect);
            hiltRenderer.sortingOrder = 33;
            // Match the selected sword's 2x hand presentation (0.085 * 2), so
            // the animated instance never shrinks when the held one is hidden.
            visual.transform.localScale = Vector3.one * 0.17f;

            var blade = new GameObject("Swinging Blade");
            blade.transform.SetParent(visual.transform, false);
            var bladeRenderer = blade.AddComponent<SpriteRenderer>();
            bladeRenderer.sprite = Sprite.Create(
                texture,
                new Rect(texture.width * 0.4226f, texture.height * 0.2871f, texture.width * 0.5774f, texture.height * 0.7129f),
                new Vector2(0.10f, 0.16f),
                100f,
                0,
                SpriteMeshType.FullRect);
            bladeRenderer.sortingOrder = 34;
            visual.AddComponent<WeaponSwingVisual>().Play(0.42f, 360f / 0.42f, blade.transform);

            if (swing.Team == BattleTeamId.Player)
            {
                _playerWeaponSelected = false;
                RefreshPlayerWeaponVisual();
                StartCoroutine(RestoreSwordWeaponVisualAfterSwing());
            }
        }

        private System.Collections.IEnumerator RestoreSwordWeaponVisualAfterSwing()
        {
            yield return new WaitForSeconds(0.43f);
            if (_inventory?.Selected?.WeaponId == "weapon.plasma-sword" && !IsPlayerTurnShotSpent())
            {
                _playerWeaponSelected = true;
                RefreshPlayerWeaponVisual();
            }
        }

        private void OnSimulationFireBlobSpawned(FireBlobEvent effect)
        {
            var visual = new GameObject("Fire Blob");
            _arenaObjects.Add(visual);
            visual.transform.position = effect.Origin;
            var renderer = visual.AddComponent<SpriteRenderer>();
            renderer.sprite = LoadWeaponIconSprite("fire_shot_1");
            renderer.sortingOrder = 32;
            var animator = visual.AddComponent<WeaponProjectileSpriteAnimator>();
            animator.SetFrames(LoadWeaponIconSprite("fire_shot_1"), LoadWeaponIconSprite("fire_shot_2"),
                LoadWeaponIconSprite("fire_shot_3"), LoadWeaponIconSprite("fire_shot_4"));
            var mover = visual.AddComponent<FireBlobVisual>();
            mover.Play(
                effect.Origin,
                effect.InitialVelocity,
                effect.Gravity,
                effect.ImpactPoint,
                effect.TravelSeconds,
                effect.LifetimeSeconds);
            visual.transform.localScale = Vector3.one * 0.072f;
            _firePuddleVisuals[effect.PuddleId] = mover;
            Destroy(visual, effect.TravelSeconds + effect.LifetimeSeconds + 0.25f);
        }

        private void OnSimulationFirePuddleMoved(int puddleId, Vector2 point)
        {
            if (_firePuddleVisuals.TryGetValue(puddleId, out var visual) && visual != null)
            {
                visual.SetBurnPoint(point);
            }
        }

        private void OnSimulationFirePuddleExtinguished(int puddleId)
        {
            if (_firePuddleVisuals.TryGetValue(puddleId, out var visual))
            {
                _firePuddleVisuals.Remove(puddleId);
                if (visual != null)
                {
                    Destroy(visual.gameObject);
                }
            }
        }

        private void ApplyProjectileVisualTransform(Transform projectileTransform, SimProjectile projectile)
        {
            var weapon = ResolveWeaponFromProjectileId(projectile.Definition.ProjectileId);
            var diameter = projectile.Definition.Radius * 2f;
            if (weapon == null)
            {
                projectileTransform.localScale = Vector3.one * diameter;
                return;
            }

            if (projectile.Definition.ProjectileId == GuardianDeployProjectileId)
            {
                projectileTransform.localScale = Vector3.one * GuardianPlacedVisualScale;
                return;
            }

            if (IsCopterBody(projectile.Definition.ProjectileId))
            {
                // The copter art is about 10 units across at 100 px/unit; it faces right and never rotates.
                projectileTransform.localScale = Vector3.one * 0.085f;
                projectileTransform.rotation = Quaternion.identity;
                return;
            }

            if (projectile.Definition.ProjectileId == "projectile.weapon.bomber.bomb")
            {
                // An enlarged blaster bolt standing vertically along the fall (about 0.35 long and 0.11 thick).
                projectileTransform.localScale = Vector3.one * 0.045f;
                projectileTransform.rotation = Quaternion.Euler(0f, 0f, -90f);
                return;
            }

            projectileTransform.localScale = ResolveProjectileVisualScale(weapon, diameter);
            if (weapon.WeaponId != "weapon.fpv-drone")
            {
                projectileTransform.rotation = Quaternion.Euler(0f, 0f,
                    Mathf.Atan2(projectile.Velocity.y, projectile.Velocity.x) * Mathf.Rad2Deg
                    + ResolveProjectileArtRotationOffset(weapon));
            }
        }

        /// <summary>The bomber and the carrier copters (and the carrier falling after its window) share one upright body look.</summary>
        private static bool IsCopterBody(string projectileId)
        {
            return projectileId == "projectile.weapon.bomber"
                || projectileId == "projectile.weapon.ufo-carrier"
                || projectileId == "projectile.weapon.ufo-carrier.fall";
        }

        /// <summary>Most flight skins face +X; the signal rocket art points up.</summary>
        private static float ResolveProjectileArtRotationOffset(WeaponDefinition weapon)
        {
            // The signal rocket points up; the nuclear bomb's nose points towards the lower left (225 degrees).
            return weapon?.WeaponId switch
            {
                "weapon.sky-hook" => -90f,
                "weapon.nuclear-charge" => 135f,
                _ => 0f
            };
        }

        private static Vector3 ResolveProjectileVisualScale(WeaponDefinition weapon, float diameter)
        {
            diameter *= 0.5f;
            return weapon.WeaponId switch
            {
                "weapon.blaster-caset" => Vector3.one * (diameter * 0.42f),
                "weapon.ion-shotgun" => Vector3.one * 0.02f,
                // blaster_shot is 1254 px wide (opaque bolt 781x241 px): this scale gives a bolt about 0.035 world units thick, close to a shotgun pellet.
                "weapon.rail-rifle" => Vector3.one * 0.0145f,
                "weapon.impulse" => Vector3.one * (diameter * 0.40572f),
                "weapon.freeze" => Vector3.one * (diameter * 0.315f),
                "weapon.mortira" => Vector3.one * (diameter * 0.36f),
                "weapon.guardian-drone" => Vector3.one * (diameter * 0.24f),
                // sky_hook is 1024 x 1536 px (15.4 world units tall at 100 px/unit); this makes the rocket about 0.75 tall.
                "weapon.sky-hook" => Vector3.one * 0.049f,
                "weapon.granata" => Vector3.one * (diameter * 0.35f),
                // The new grenade art has more empty canvas than the original
                // grenade. Its flight scale compensates for that so it matches
                // the intentionally smaller held presentation below.
                "weapon.sticky-grenade" => Vector3.one * (diameter * 0.455f),
                "weapon.hopping-grenade" => Vector3.one * (diameter * 0.455f),
                "weapon.cryo-grenade" => Vector3.one * (diameter * 0.455f),
                "weapon.incendiary-grenade" => Vector3.one * (diameter * 0.455f),
                "weapon.titanium-disc" => Vector3.one * (diameter * 0.85f),
                "weapon.laser-fence" => Vector3.one * (diameter * 0.35f),
                "weapon.teleport" => Vector3.one * (diameter * 0.72f),
                "weapon.swap-jump" => Vector3.one * (diameter * 0.72f),
                "weapon.toxic" => Vector3.one * (diameter * 1.05f),
                "weapon.emp" => new Vector3(diameter * 0.46f, diameter * 0.33f, 1f),
                "weapon.mine" => Vector3.one * (diameter * 0.55f),
                "weapon.bur-rocket" => Vector3.one * (diameter * 0.68f),
                "weapon.missle" => Vector3.one * (diameter * 1.25f),
                "weapon.orbital-rocket" => Vector3.one * (diameter * 1.1f),
                // nuclear_charge is 1254 px square (12.5 units at 100 px/unit); the bomb is drawn about 0.8 long.
                "weapon.nuclear-charge" => Vector3.one * 0.06f,
                "weapon.fpv-drone" => Vector3.one * (diameter * 1.4f),
                _ => Vector3.one * (diameter * 1.15f)
            };
        }

        private Color ResolveProjectileVisualColor(WeaponDefinition weapon)
        {
            if (weapon == null)
            {
                return Color.black;
            }

            return weapon.WeaponId == "weapon.ion-shotgun"
                ? Color.black
                : ProjectileWeaponEffect.GetProjectileColor(weapon.Role);
        }

        private Sprite ResolveProjectileSprite(SimProjectile projectile, WeaponDefinition weapon)
        {
            if (weapon == null)
            {
                return null;
            }

            if (projectile?.Definition != null && projectile.Definition.ProjectileId == "projectile.weapon.bomber.bomb")
            {
                return LoadWeaponIconSprite("blaster_shot");
            }

            // The thrown guardian body flies with the same skin it shows on the ground; only its bullet is small.
            if (projectile?.Definition != null && projectile.Definition.ProjectileId == GuardianDeployProjectileId)
            {
                return LoadWeaponIconSprite(ResolveOwnerAwareEvolvedIconId(weapon, projectile.OwnerUnitId));
            }

            var spriteId = weapon.WeaponId switch
            {
                // These are explicit projectile sprites. Never substitute the held
                // weapon/evolution icon here: it makes the gun itself look as though
                // it was fired from the character.
                "weapon.blaster-caset" => "blaster_shot",
                "weapon.rail-rifle" => "blaster_shot",
                "weapon.impulse" => "impulse_shot",
                "weapon.mortira" => "mortira_shot",
                "weapon.guardian-drone" => "mortira_shot",
                "weapon.emp" => "emp_shot",
                "weapon.freeze" => "ice_shot",
                "weapon.toxic" => "toxic_shot",
                "weapon.missle" => MissileInFlightSpriteId,
                "weapon.orbital-rocket" => MissileInFlightSpriteId,
                "weapon.nuclear-charge" => "nuclear_charge",
                "weapon.bur-rocket" => "bur_rocket",
                "weapon.bur" => "bur_lazer",
                "weapon.fire-gun" => "fire_shot_1",
                // The following launch their actual placed/thrown object. Their
                // rank-aware art is therefore the correct in-flight representation.
                "weapon.granata" => ResolveOwnerAwareEvolvedIconId(weapon, projectile.OwnerUnitId),
                "weapon.sticky-grenade" => ResolveOwnerAwareEvolvedIconId(weapon, projectile.OwnerUnitId),
                "weapon.hopping-grenade" => ResolveOwnerAwareEvolvedIconId(weapon, projectile.OwnerUnitId),
                "weapon.cryo-grenade" => ResolveOwnerAwareEvolvedIconId(weapon, projectile.OwnerUnitId),
                "weapon.incendiary-grenade" => ResolveOwnerAwareEvolvedIconId(weapon, projectile.OwnerUnitId),
                "weapon.titanium-disc" => ResolveOwnerAwareEvolvedIconId(weapon, projectile.OwnerUnitId),
                "weapon.laser-fence" => ResolveOwnerAwareEvolvedIconId(weapon, projectile.OwnerUnitId),
                "weapon.mine" => ResolveOwnerAwareEvolvedIconId(weapon, projectile.OwnerUnitId),
                "weapon.c4" => ResolveOwnerAwareEvolvedIconId(weapon, projectile.OwnerUnitId),
                "weapon.heal" => ResolveOwnerAwareEvolvedIconId(weapon, projectile.OwnerUnitId),
                 "weapon.teleport" => ResolveOwnerAwareEvolvedIconId(weapon, projectile.OwnerUnitId),
                 "weapon.swap-jump" => ResolveOwnerAwareEvolvedIconId(weapon, projectile.OwnerUnitId),
                "weapon.bonus-box" => ResolveOwnerAwareEvolvedIconId(weapon, projectile.OwnerUnitId),
                "weapon.fpv-drone" => ResolveOwnerAwareEvolvedIconId(weapon, projectile.OwnerUnitId),
                "weapon.repair-jammer" => ResolveOwnerAwareEvolvedIconId(weapon, projectile.OwnerUnitId),
                "weapon.virus" => ResolveOwnerAwareEvolvedIconId(weapon, projectile.OwnerUnitId),
                "weapon.sky-hook" => ResolveOwnerAwareEvolvedIconId(weapon, projectile.OwnerUnitId),
                "weapon.spider-mine" => ResolveOwnerAwareEvolvedIconId(weapon, projectile.OwnerUnitId),
                "weapon.bomber" => ResolveOwnerAwareEvolvedIconId(weapon, projectile.OwnerUnitId),
                "weapon.ufo-carrier" => ResolveOwnerAwareEvolvedIconId(weapon, projectile.OwnerUnitId),
                _ => null
            };
            return LoadWeaponIconSprite(spriteId);
        }

        /// <summary>
        /// Rank/branch-aware icon for a weapon's in-flight/held visual, resolved from
        /// the progression of whichever side fired it.
        /// </summary>
        private string ResolveOwnerAwareEvolvedIconId(WeaponDefinition weapon, string ownerUnitId)
        {
            var isPlayerOwned = !string.IsNullOrWhiteSpace(ownerUnitId)
                && ownerUnitId.StartsWith("player.", System.StringComparison.Ordinal);
            var evolved = ResolveEvolvedIconId(
                weapon?.IconId,
                weapon,
                isPlayerOwned ? _playerProgression : _opponentProgression);
            if (!string.Equals(evolved, weapon?.IconId, System.StringComparison.Ordinal))
            {
                return evolved;
            }

            return weapon.IconId;
        }

        private void SpawnTeleportMarkerVisual(Vector2 point)
        {
            var visual = new GameObject("Teleport Marker");
            _arenaObjects.Add(visual);
            visual.transform.position = point;
            var renderer = visual.AddComponent<SpriteRenderer>();
            renderer.sprite = LoadWeaponIconSprite("teleport");
            renderer.sortingOrder = 31;
            visual.transform.localScale = Vector3.one * 0.22f;
            Destroy(visual, 0.35f);
        }

        private WeaponDefinition ResolveWeaponFromProjectileId(string projectileId)
        {
            if (projectileId == "projectile.weapon.mine.deploy")
            {
                return _weaponsById.TryGetValue("weapon.mine", out var mine) ? mine : null;
            }

            if (projectileId == "projectile.weapon.laser-fence.deploy")
            {
                return _weaponsById.TryGetValue("weapon.laser-fence", out var laserFence) ? laserFence : null;
            }

            if (projectileId == "projectile.weapon.bomber.bomb")
            {
                return _weaponsById.TryGetValue("weapon.bomber", out var bomber) ? bomber : null;
            }

            if (projectileId == "projectile.weapon.ufo-carrier.fall")
            {
                return _weaponsById.TryGetValue("weapon.ufo-carrier", out var carrier) ? carrier : null;
            }

            // The thrown guardian body; without this it had no weapon and flew as a plain black ball.
            if (projectileId == GuardianDeployProjectileId)
            {
                return _weaponsById.TryGetValue("weapon.guardian-drone", out var guardian) ? guardian : null;
            }

            if (projectileId != null
                && (projectileId.StartsWith("projectile.weapon.mortira.fragment", System.StringComparison.Ordinal)
                    || projectileId.StartsWith("projectile.weapon.mortira.cascade.", System.StringComparison.Ordinal)))
            {
                return _weaponsById.TryGetValue("weapon.mortira", out var mortar) ? mortar : null;
            }

            if (projectileId != null
                && projectileId.StartsWith("projectile.weapon.missle.cluster.", System.StringComparison.Ordinal))
            {
                return _weaponsById.TryGetValue("weapon.missle", out var missile) ? missile : null;
            }

            if (!string.IsNullOrWhiteSpace(projectileId)
                && projectileId.StartsWith("projectile.", System.StringComparison.Ordinal))
            {
                var weaponId = projectileId.Substring("projectile.".Length);
                if (_weaponsById.TryGetValue(weaponId, out var weapon))
                {
                    return weapon;
                }
            }

            return null;
        }

        private void OnSimulationShotFired(BattleTeamId team, WeaponDefinition weapon, bool setsShotStatus)
        {
            ClearWalkInput();
            if (team == BattleTeamId.Player)
            {
                BattleActivityBridge.ReportShotFired(weapon?.WeaponId);
                RefreshArsenalHudItems();
                ClearSelectedWeaponIfDepleted(weapon);
            }
            if (setsShotStatus)
            {
                _status = GameText.ShotStatus(DescribeTeam(team), DescribeWeapon(weapon));
            }

            // Any weapon whose shot spends the turn's action hides its held skin and the aim afterwards.
            var spentTheTurnsShot = team == BattleTeamId.Player && _battleState.Turn.ActionSpent;
            if (team == BattleTeamId.Player
                && (spentTheTurnsShot
                    || HidesHeldWeaponAfterUse(weapon)
                        && (HidesHeldWeaponDuringFlight(weapon) || _inventory.GetSelectedRemainingAmmo() <= 0)))
            {
                var keepsHoppingControl = weapon?.WeaponId == "weapon.hopping-grenade" && !setsShotStatus;
                // The purchased early trigger needs the same weapon and aim for the second press.
                var keepsSphereControl = weapon?.WeaponId == "weapon.teleport" && _simulation.CanTriggerTeleportEarly();
                var keepsBomberControl = weapon?.WeaponId is "weapon.bomber" or "weapon.ufo-carrier" && _simulation.HasPlayerCopter;
                _hideHeldWeaponForReusableThrow = keepsHoppingControl || keepsSphereControl || keepsBomberControl;
                _playerWeaponSelected = keepsHoppingControl || keepsSphereControl || keepsBomberControl;
                RefreshPlayerWeaponVisual();
            }

            if (team != BattleTeamId.Player
                || (!_simulation.CanContinueBlasterBurst && weapon?.WeaponId != "weapon.hopping-grenade"
                    && !(weapon?.WeaponId == "weapon.teleport" && _simulation.CanTriggerTeleportEarly())))
            {
                ResetAimState();
            }
            EmitFeedback(BattleFeedbackEventType.ShotFired);
        }

        private void ClearSelectedWeaponIfDepleted(WeaponDefinition weapon)
        {
            if (weapon == null
                || !weapon.HasLimitedAmmo
                || _inventory?.Selected?.WeaponId != weapon.WeaponId
                || _inventory.GetSelectedRemainingAmmo() > 0
                // The copter's one charge is spent on launch, but its fire button drops the bombs.
                || (weapon.WeaponId is "weapon.bomber" or "weapon.ufo-carrier" && _simulation.HasPlayerCopter))
            {
                return;
            }

            _playerWeaponSelected = false;
            ResetAimState();
            RefreshPlayerWeaponVisual();
        }

        /// <summary>
        /// The player's shot of this turn is spent (only the retreat is left): the held skin and the aim stay hidden
        /// until the turn passes. The blaster burst, the teleport's early trigger and a flying copter still need
        /// their weapon for the follow-up press.
        /// </summary>
        private bool IsPlayerTurnShotSpent()
        {
            return _simulation != null
                && _battleState != null
                && _battleState.Turn.ActiveTeam == BattleTeamId.Player
                && _battleState.Turn.ActionSpent
                && !_simulation.CanContinueBlasterBurst
                && !_simulation.CanTriggerTeleportEarly()
                && !_simulation.HasPlayerCopter;
        }

        private static bool HidesHeldWeaponAfterUse(WeaponDefinition weapon)
        {
            return weapon?.WeaponId == "weapon.guardian-drone"
                || weapon?.WeaponId == "weapon.granata"
                || weapon?.WeaponId == "weapon.teleport"
                || weapon?.WeaponId == "weapon.mine"
                || weapon?.WeaponId == "weapon.bur-rocket"
                || weapon?.WeaponId == "weapon.titanium-disc"
                || weapon?.WeaponId == "weapon.missle"
                || weapon?.WeaponId == "weapon.fpv-drone"
                || weapon?.WeaponId == "weapon.sticky-grenade"
                || weapon?.WeaponId == "weapon.hopping-grenade"
                || weapon?.WeaponId == "weapon.cryo-grenade"
                || weapon?.WeaponId == "weapon.incendiary-grenade"
                || weapon?.WeaponId == "weapon.laser-fence"
                || weapon?.WeaponId == "weapon.repair-jammer"
                || weapon?.WeaponId == "weapon.virus"
                || weapon?.WeaponId == "weapon.sky-hook"
                || weapon?.WeaponId == "weapon.spider-mine"
                || weapon?.WeaponId == "weapon.bomber"
                || weapon?.WeaponId == "weapon.ufo-carrier";
        }

        private static bool HidesHeldWeaponDuringFlight(WeaponDefinition weapon)
        {
            return weapon?.WeaponId == "weapon.guardian-drone"
                || weapon?.WeaponId == "weapon.bur-rocket"
                || weapon?.WeaponId == "weapon.titanium-disc"
                || weapon?.WeaponId == "weapon.granata"
                || weapon?.WeaponId == "weapon.teleport"
                || weapon?.WeaponId == "weapon.missle"
                || weapon?.WeaponId == "weapon.sticky-grenade"
                || weapon?.WeaponId == "weapon.hopping-grenade"
                || weapon?.WeaponId == "weapon.cryo-grenade"
                || weapon?.WeaponId == "weapon.incendiary-grenade"
                || weapon?.WeaponId == "weapon.laser-fence"
                || weapon?.WeaponId == "weapon.repair-jammer"
                || weapon?.WeaponId == "weapon.virus"
                || weapon?.WeaponId == "weapon.sky-hook"
                || weapon?.WeaponId == "weapon.spider-mine"
                || weapon?.WeaponId == "weapon.fpv-drone"
                || weapon?.WeaponId == "weapon.bomber"
                || weapon?.WeaponId == "weapon.ufo-carrier";
        }

        private static bool ReturnsHeldWeaponAfterFlight(WeaponDefinition weapon)
        {
            // A spent instance remains hidden, but an arsenal slot with ammo
            // left immediately shows its next physical item after landing.
            return HidesHeldWeaponDuringFlight(weapon);
        }

        private void OnSimulationImpactResolved(ImpactResolvedEvent impact)
        {
            ReportTutorial(BattleTutorialBridge.ProjectileImpact);
            EmitFeedback(BattleFeedbackEventType.ProjectileImpact);
        }

        private void OnSimulationDamageApplied(BattleTeamId sourceTeam, DamageEvent damage)
        {
            if (sourceTeam == BattleTeamId.Player
                && damage.TargetTeam == BattleTeamId.Opponent
                && damage.Damage > 0)
            {
                BattleActivityBridge.ReportPlayerDamageDealt(damage.Damage);
            }
        }

        private void OnSimulationTerrainCellsDestroyed(BattleTeamId sourceTeam, int cells)
        {
            DropUnsupportedSupportPickups();

            if (sourceTeam == BattleTeamId.Player)
            {
                BattleActivityBridge.ReportTerrainDestroyed(cells);
            }
        }

        private void DropUnsupportedSupportPickups()
        {
            if (_battleTerrain?.Grid == null)
            {
                return;
            }

            var searchDistance = _battleTerrain.Grid.WorldMaxY - _battleTerrain.Grid.WorldMinY + 2f;
            foreach (var pickup in _resourcePickups)
            {
                if (pickup.Visual == null || pickup.IsFalling || pickup.IsCarried)
                {
                    continue;
                }

                var current = pickup.Position;
                if (_battleTerrain.Grid.IsSolidWorld(current + Vector2.down * 0.1f)
                    || !_battleTerrain.Grid.TryFindSurfaceBelow(current.x, current.y - 0.08f, searchDistance, out var surfaceY))
                {
                    continue;
                }

                var renderer = pickup.Visual.GetComponent<SpriteRenderer>();
                var spriteHeight = renderer?.sprite == null
                    ? 0f
                    : renderer.sprite.bounds.size.y * pickup.Visual.transform.localScale.y;
                var destination = new Vector2(
                    current.x,
                    surfaceY + spriteHeight * 0.5f - SupportPickupGroundOffset);
                if (destination.y >= current.y - 0.06f)
                {
                    continue;
                }

                StartCoroutine(AnimateSupportPickupFall(pickup, current, destination));
            }
        }

        private IEnumerator AnimateSupportPickupFall(ResourcePickup pickup, Vector2 origin, Vector2 destination)
        {
            pickup.IsFalling = true;
            const float duration = 0.24f;
            var elapsed = 0f;
            while (pickup.Visual != null && elapsed < duration)
            {
                elapsed += Time.deltaTime;
                pickup.Position = Vector2.Lerp(origin, destination, Mathf.SmoothStep(0f, 1f, elapsed / duration));
                pickup.Visual.transform.position = pickup.Position;
                yield return null;
            }

            if (pickup.Visual != null)
            {
                pickup.Position = destination;
                pickup.Visual.transform.position = destination;
            }

            if (pickup.SimulationId >= 0)
            {
                _simulation?.SetCarryablePickupPosition(pickup.SimulationId, destination - Vector2.up * pickup.VisualLift);
            }

            pickup.IsFalling = false;
        }

        private void OnSimulationUnitDefeated(UnitDefeatedEvent defeat)
        {
            // Falling out of bounds is the only non-weapon death the simulation reports,
            // and it covers both the map edge and the kill zone barrier.
            if (defeat.Reason == "fall.out-of-bounds"
                && !string.IsNullOrEmpty(defeat.UnitId)
                && defeat.UnitId.StartsWith("opponent.", System.StringComparison.Ordinal))
            {
                _opponentRingOut = true;
            }

            EmitFeedback(BattleFeedbackEventType.UnitDefeated);
        }

        void IBattleCommandTarget.SelectWeapon(int weaponIndex)
        {
            SelectWeapon(weaponIndex);
        }

        void IBattleCommandTarget.SetWalkIntent(int direction)
        {
            if (direction == 0)
            {
                ClearWalkInput();
                return;
            }

            SetUiWalkInput(direction > 0 ? 1 : -1);
        }

        void IBattleCommandTarget.Jump(int direction)
        {
            TryStartJump(direction);
        }

        void IBattleCommandTarget.Fire(Vector2 origin, Vector2 direction, float power)
        {
            FireWithAim(new AimState(origin, direction, power, true));
        }

        void IBattleCommandTarget.SetProjectileInput(Vector2 direction)
        {
            _simulation?.SetControlledProjectileInput(direction);
        }

        void IBattleCommandTarget.EndTurn()
        {
            _simulation?.EndActiveTurn();
        }

        /// <summary>
        /// Connects the duel transport and states who owns which unit. Both sides
        /// build their own side as <c>player.*</c>, so the authority map is what lets
        /// a command name a commander rather than a local id.
        /// </summary>
        /// <summary>
        /// Mirrors a local action onto the wire. No-op outside a networked duel, and
        /// suppressed while a remote command is being replayed so an incoming action
        /// is never echoed back at its sender.
        /// </summary>
        private void EmitDuelCommand(
            BattleCommandKind kind,
            int value = 0,
            Vector2 origin = default,
            Vector2 direction = default,
            float power = 0f)
        {
            if (_duel == null || _duel.IsApplyingRemote)
            {
                return;
            }

            var localUnit = GetPresenter(BattleTeamId.Player)?.State;
            if (localUnit != null)
            {
                _duel.SendLocalCommand(kind, localUnit.UnitId, value, origin, direction, power);
            }
        }

        private void SetUpDuelNetworking()
        {
            _duel = null;
            if (GameSessionContext.SelectedBattleMode != BattleMode.Pvp)
            {
                return;
            }

            var session = PvpSessionBridge.Resolve();
            var opponent = GameSessionContext.SelectedPvpOpponent;
            // No transport, or an opponent that never came from a live match: the
            // duel simply plays out locally against the profile it was handed.
            if (session == null || opponent == null || opponent.IsBotBackfill)
            {
                return;
            }

            var localPlayerId = GameSessionContext.LocalPlayerId;
            var authority = new BattleUnitAuthority();
            authority.SetLocalPlayer(localPlayerId);
            foreach (var unit in _battleState.Teams.SelectMany(team => team.Units))
            {
                authority.Assign(
                    unit.TeamId == BattleTeamId.Player ? localPlayerId : opponent.PlayerId,
                    unit.UnitId);
            }

            var coordinator = new BattleDuelCoordinator(
                session,
                this,
                authority,
                CaptureTurnSnapshot,
                ApplyAuthoritativeTurn);
            coordinator.OpponentLeft += OnDuelOpponentLeft;
            _duel = coordinator;

            session.Connect(
                GameSessionContext.SelectedMatchId,
                localPlayerId,
                PvpSessionRole.Guest,
                connected =>
                {
                    if (!connected)
                    {
                        Debug.LogWarning("Duel transport did not connect; the battle continues locally.");
                    }
                });
        }

        private void ApplyAuthoritativeTurn(BattleTurnSnapshot snapshot)
        {
            var result = BattleTurnReconciler.Apply(_battleState, _battleTerrain, snapshot);
            _duel?.ReportReconcileResult(result);
            if (result.CorrectedUnits > 0)
            {
                RefreshUnitViews();
            }
        }

        private void OnDuelOpponentLeft(string playerId)
        {
            // A commander who leaves mid-battle concedes it; the player is not left
            // staring at a turn that will never come.
            _status = GameText.OpponentLeftDuel();
        }

        private void OnSimulationTerrainCraterApplied(BattleTeamId sourceTeam, Vector2 center, float radius)
        {
            _turnCraters.Add(new BattleCraterSnapshot(center, radius));
        }

        /// <summary>
        /// The battle as the authoritative side sees it right now, for publishing at a
        /// turn boundary or for comparing against what another client believes.
        /// </summary>
        public BattleTurnSnapshot CaptureTurnSnapshot()
        {
            return BattleTurnSnapshot.Capture(_battleState, _turnCraters.ToArray());
        }

        private void OnSimulationTurnAdvanced(BattleTeamId newActiveTeam, TurnAdvanceReason reason)
        {
            _duel?.PublishTurnBoundary();
            // Craters belong to the turn that cut them; the next turn starts clean.
            _turnCraters.Clear();
            _playerWeaponSelected = false;
            _controlledPointerActive = false;
            if (ShouldClearOrbitalTargetMarkerOnTurnAdvanced(reason))
            {
                ClearOrbitalTargetMarker(clearSimulation: false);
            }

            if (reason == TurnAdvanceReason.PlayerTimerExpired)
            {
                _firstBattleGuide?.ObservePlayerTurnExpired();
            }
            else
            {
                _firstBattleGuide?.ObserveTurnAdvanced(newActiveTeam);
            }

            ApplyAimSettingsForActiveTeam();
            ResetAimState();
            ClearWalkInput();

            var turnText = GameText.TurnStatus(DescribeTeam(newActiveTeam));
            _status = reason == TurnAdvanceReason.InstantActionCompleted ? $"{_status} {turnText}" : turnText;
            TryScheduleSupportDrop();
        }

        private void TryScheduleSupportDrop()
        {
            if (!GameSessionContext.SupportDropsEnabled
                || _supportDropTurnInterval == int.MaxValue
                || _supportDropInProgress
                || _resourcePickups.Count >= MaxActiveSupportPickups
                || _battleState == null
                || _battleState.Turn.TurnNumber < _nextSupportDropTurn
                || (_simulation != null && _simulation.Result.IsFinished))
            {
                return;
            }

            _nextSupportDropTurn = _battleState.Turn.TurnNumber + _supportDropTurnInterval;
            if (!TryChooseSupportDropPosition(out var position))
            {
                return;
            }

            var kind = Random.value < ResolveMedkitDropChance()
                ? ResourcePickupKind.Medkit
                : ResourcePickupKind.SupplyBox;
            StartCoroutine(AnimateSupportDrop(position, kind));
        }

        private float ResolveMedkitDropChance()
        {
            return _aiDifficulty switch
            {
                AiDifficulty.Easy => 0.55f,
                AiDifficulty.Hard => 0.3f,
                _ => 0.42f
            };
        }

        private bool TryChooseSupportDropPosition(out Vector2 position)
        {
            position = default;
            var grid = _battleTerrain?.Grid;
            if (grid == null)
            {
                return false;
            }

            var horizontalPadding = Mathf.Max(0.8f, (grid.WorldMaxX - grid.WorldMinX) * 0.08f);
            var minX = grid.WorldMinX + horizontalPadding;
            var maxX = grid.WorldMaxX - horizontalPadding;
            var scanStartY = grid.WorldMaxY + 1f;
            var scanDistance = grid.WorldMaxY - grid.WorldMinY + 2f;
            for (var attempt = 0; attempt < 16; attempt++)
            {
                var x = Random.Range(minX, maxX);
                if (!grid.TryFindSurfaceBelow(x, scanStartY, scanDistance, out var surfaceY))
                {
                    continue;
                }

                var candidate = new Vector2(x, surfaceY + 0.28f);
                var tooCloseToUnit = _battleState.Teams
                    .SelectMany(team => team.Units)
                    .Any(unit => !unit.IsDefeated && Vector2.Distance(unit.Position, candidate) < 1.15f);
                var tooCloseToPickup = _resourcePickups
                    .Any(pickup => pickup.Visual != null && Vector2.Distance(pickup.Position, candidate) < 0.9f);
                if (tooCloseToUnit || tooCloseToPickup)
                {
                    continue;
                }

                position = candidate;
                return true;
            }

            return false;
        }

        private IEnumerator AnimateSupportDrop(Vector2 targetPosition, ResourcePickupKind kind)
        {
            _supportDropInProgress = true;
            // This copy lives under the managed generated-UI root, which locks it
            // to RGBA Default import instead of Unity's Single Channel mode.
            var beamSprite = LoadTopPivotTextureSprite("UI/Generated/resource_drop_beam_rgba", "Support Drop Beam");
            GameObject beam = null;
            SpriteRenderer beamRenderer = null;
            if (beamSprite != null)
            {
                var topY = Mathf.Max(_cameraWorldBounds.yMax + 0.4f, targetPosition.y + 4f);
                // The source sprite has transparent padding at its lower edge.
                // Extend past the pickup target so the visible beam reaches the
                // terrain instead of stopping slightly above it.
                var beamHeight = Mathf.Max(1f, topY - targetPosition.y + ResourceDropBeamGroundOverlap);
                beam = new GameObject("Support Drop Beam");
                beam.transform.position = new Vector3(targetPosition.x, topY, 0f);
                beamRenderer = beam.AddComponent<SpriteRenderer>();
                beamRenderer.sprite = beamSprite;
                // Keep the authored cyan/white beam pixels unchanged. Tinting this
                // source darkened its semi-transparent core and made it read black.
                beamRenderer.color = Color.white;
                beamRenderer.sortingOrder = 28;
                _arenaObjects.Add(beam);

                var fullScale = new Vector3(
                    ResourceDropBeamWorldWidth / Mathf.Max(0.001f, beamSprite.bounds.size.x),
                    beamHeight / Mathf.Max(0.001f, beamSprite.bounds.size.y),
                    1f);
                var elapsed = 0f;
                while (elapsed < ResourceDropBeamGrowSeconds && beam != null)
                {
                    elapsed += Time.deltaTime;
                    var progress = Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(elapsed / ResourceDropBeamGrowSeconds));
                    beam.transform.localScale = new Vector3(fullScale.x, fullScale.y * progress, 1f);
                    yield return null;
                }

                if (beam != null)
                {
                    beam.transform.localScale = fullScale;
                }
            }

            SpawnSupportPickup(targetPosition, kind);

            var fadeElapsed = 0f;
            while (beamRenderer != null && fadeElapsed < ResourceDropBeamFadeSeconds)
            {
                fadeElapsed += Time.deltaTime;
                var color = beamRenderer.color;
                color.a = 1f - Mathf.Clamp01(fadeElapsed / ResourceDropBeamFadeSeconds);
                beamRenderer.color = color;
                yield return null;
            }

            if (beam != null)
            {
                _arenaObjects.Remove(beam);
                Destroy(beam);
            }

            _supportDropInProgress = false;
        }

        private void SpawnSupportPickup(Vector2 position, ResourcePickupKind kind)
        {
            var iconId = kind == ResourcePickupKind.Medkit ? "heal" : "bonus_box";
            var sprite = LoadWeaponIconSprite(iconId);
            if (sprite == null)
            {
                return;
            }

            var pickup = new GameObject(kind == ResourcePickupKind.Medkit ? "Support Medkit" : "Support Supply Box");
            var scale = SupportPickupWorldWidth / Mathf.Max(0.001f, sprite.bounds.size.x);
            var groundedPosition = new Vector2(
                position.x,
                position.y + sprite.bounds.size.y * scale * 0.5f - SupportPickupGroundOffset);
            pickup.transform.position = groundedPosition;
            pickup.transform.localScale = Vector3.one * scale;
            var renderer = pickup.AddComponent<SpriteRenderer>();
            renderer.sprite = sprite;
            renderer.sortingOrder = 27;
            _arenaObjects.Add(pickup);
            var resourcePickup = new ResourcePickup(pickup, groundedPosition, kind)
            {
                VisualLift = groundedPosition.y - position.y
            };
            if (_simulation != null)
            {
                resourcePickup.SimulationId = _simulation.RegisterCarryablePickup(position);
            }

            _resourcePickups.Add(resourcePickup);
        }

        private ResourcePickup FindResourcePickupBySimulationId(int simulationId)
        {
            foreach (var pickup in _resourcePickups)
            {
                if (pickup.SimulationId == simulationId && simulationId >= 0)
                {
                    return pickup;
                }
            }

            return null;
        }

        private void OnSimulationBattleFinished(BattleResult result, BattleFinishReason reason)
        {
            RecordFinishedBattleActivity(result);
            ReportObjectiveFacts(result);
            _arenaReward = GrantArenaReward(result);
            _resultScreenState = CreateResultScreenState(result, _battleState.Turn.TurnNumber, _arenaReward);
            _status = GameText.ResultPrompt(_resultScreenState.Title);
            if (result.Outcome == BattleOutcome.PlayerWon && reason != BattleFinishReason.InstantActionCompleted)
            {
                ReportTutorial(BattleTutorialBridge.BattleWon);
            }

            _firstBattleGuide?.ObserveBattleFinished();
            EmitFeedback(BattleFeedbackEventType.BattleFinished);
        }

        /// <summary>
        /// Banks the payout for a finished raid or duel. Story and training pay
        /// nothing here: the campaign grants its first-completion reward when the menu
        /// banks the mission, and training is not an economy source at all.
        /// </summary>
        private BattleArenaReward GrantArenaReward(BattleResult result)
        {
            if (!GameSessionContext.SelectedBattleMode.IsArena() || result.Outcome == BattleOutcome.InProgress)
            {
                return BattleArenaReward.None;
            }

            var maxHealth = GetTeamMaxHealth(BattleTeamId.Player);
            var healthPercent = maxHealth > 0
                ? Mathf.RoundToInt(100f * GetTeamHealth(BattleTeamId.Player) / maxHealth)
                : 0;

            return BattleRewardBridge.GrantArenaReward(new BattleArenaRewardRequest(
                GameSessionContext.SelectedBattleMode,
                GameSessionContext.SelectedAiDifficulty,
                ResolveArenaOutcome(result.Outcome),
                healthPercent,
                _battleId));
        }

        /// <summary>
        /// Reward line for the result card: the full credits/XP/blueprints breakdown
        /// once a repeatable battle has actually paid, and the plain credit line for a
        /// campaign mission, which is still banked later on the map.
        /// </summary>
        private static string DescribeResultReward(BattleRewardPreview preview)
        {
            if (preview.CommanderExperience <= 0 && preview.Blueprints <= 0)
            {
                return GameText.Reward(preview.SoftCurrency);
            }

            var summary = GameText.RewardSummary(
                preview.SoftCurrency,
                preview.CommanderExperience,
                preview.Blueprints);
            return preview.IsReduced
                ? summary + System.Environment.NewLine + GameText.RewardDailyLimit()
                : summary;
        }

        private static BattleArenaOutcome ResolveArenaOutcome(BattleOutcome outcome)
        {
            return outcome switch
            {
                BattleOutcome.PlayerWon => BattleArenaOutcome.Victory,
                BattleOutcome.Draw => BattleArenaOutcome.Draw,
                _ => BattleArenaOutcome.Defeat
            };
        }

        /// <summary>
        /// Hands the app layer what the battle looked like, so the mission's objective can
        /// be judged when the menu banks the completion.
        /// </summary>
        private void ReportObjectiveFacts(BattleResult result)
        {
            var maxHealth = GetTeamMaxHealth(BattleTeamId.Player);
            var healthPercent = maxHealth > 0
                ? Mathf.RoundToInt(100f * GetTeamHealth(BattleTeamId.Player) / maxHealth)
                : 0;
            GameSessionContext.ReportBattleFacts(
                result.Outcome == BattleOutcome.PlayerWon,
                healthPercent,
                _opponentRingOut);
        }

        private void RecordFinishedBattleActivity(BattleResult result)
        {
            if (_activityBattleRecorded || result.Outcome == BattleOutcome.InProgress)
            {
                return;
            }

            _activityBattleRecorded = true;
            var mode = GameSessionContext.SelectedBattleMode;
            BattleActivityBridge.ReportBattleFinished(
                result.Outcome == BattleOutcome.PlayerWon,
                mode.IsStory(),
                mode.IsArena());
        }

        private void OnSimulationCoverDeployed(CoverDeployedEvent cover)
        {
            if (cover.Weapon?.WeaponId == "weapon.titanium-barrier")
            {
                var visual = new GameObject("Titanium Beam");
                visual.transform.position = cover.Position;
                visual.transform.rotation = Quaternion.Euler(0f, 0f, cover.RotationDegrees);
                var renderer = visual.AddComponent<SpriteRenderer>();
                renderer.sprite = cover.Surface.Sprite;
                renderer.color = Color.white;
                visual.transform.localScale = new Vector3(cover.Size.x / renderer.sprite.bounds.size.x,
                    cover.Size.y / renderer.sprite.bounds.size.y, 1f);
                renderer.sortingOrder = 18;
                _arenaObjects.Add(visual);
                _status = GameText.CoverDeployed(DescribeWeapon(cover.Weapon));
                return;
            }

            var box = CreateBox(
                $"Cover {cover.Weapon.WeaponId}",
                cover.Position,
                cover.Size,
                ProjectileWeaponEffect.GetProjectileColor(cover.Weapon.Role),
                18);
            _arenaObjects.Add(box);
            _status = GameText.CoverDeployed(DescribeWeapon(cover.Weapon));
        }

        private void OnSimulationHealthRestored(HealthRestoredEvent heal)
        {
            SpawnImpactFlash(heal.Point, new Color(0.26f, 0.84f, 0.62f, 0.85f), 1.45f);
            _status = GameText.ShieldPulse(DescribeTeam(heal.Team), heal.Amount);
        }

        private void OnSimulationOwnerRepositioned(string unitId)
        {
            if (string.IsNullOrEmpty(unitId))
            {
                _status = GameText.RepositionFailed();
                return;
            }

            var owner = _battleState.GetUnit(unitId);
            SpawnImpactFlash(owner.Position, new Color(0.66f, 0.42f, 0.96f, 0.8f), 1.05f);
            _status = GameText.Repositioned(DescribeTeam(owner.TeamId));
        }

        private void OnSimulationUnitsMoved()
        {
            TrackPlayerMovementActivity();
            CollectNearbyActivityResources();
            RefreshUnitViews();
            SyncProjectileVisuals();
            RefreshPlayerWeaponVisual();
        }

        private void SeedActivityMovementPosition()
        {
            var player = GetPresenter(BattleTeamId.Player)?.State;
            if (player == null)
            {
                return;
            }

            _lastTrackedPlayerPosition = player.Position;
            _hasTrackedPlayerPosition = true;
        }

        private void TrackPlayerMovementActivity()
        {
            var player = GetPresenter(BattleTeamId.Player)?.State;
            if (player == null)
            {
                return;
            }

            if (!_hasTrackedPlayerPosition)
            {
                _lastTrackedPlayerPosition = player.Position;
                _hasTrackedPlayerPosition = true;
                return;
            }

            var distance = Vector2.Distance(_lastTrackedPlayerPosition, player.Position);
            _lastTrackedPlayerPosition = player.Position;
            if (distance <= 0.0001f || !_unitMotors.TryGetValue(player.UnitId, out var motor))
            {
                return;
            }

            if (motor.IsJumping || motor.IsStable)
            {
                BattleActivityBridge.ReportPlayerMoved(distance, motor.IsJumping);
            }
        }

        private void CollectNearbyActivityResources()
        {
            var player = GetPresenter(BattleTeamId.Player)?.State;
            if (player == null)
            {
                return;
            }

            for (var index = _resourcePickups.Count - 1; index >= 0; index--)
            {
                var pickup = _resourcePickups[index];
                if (pickup.Visual == null)
                {
                    _resourcePickups.RemoveAt(index);
                    continue;
                }

                if (Vector2.Distance(player.Position, pickup.Position) > 0.7f)
                {
                    continue;
                }

                // The repair jammer blocks every source of healing, the medkit included: it stays on the ground.
                if (pickup.Kind == ResourcePickupKind.Medkit && _simulation != null
                    && _simulation.IsUnitRepairJammed(player.UnitId))
                {
                    continue;
                }

                _resourcePickups.RemoveAt(index);
                if (pickup.SimulationId >= 0)
                {
                    _simulation?.RemoveCarryablePickup(pickup.SimulationId);
                }
                _arenaObjects.Remove(pickup.Visual);
                Destroy(pickup.Visual);
                if (pickup.Kind == ResourcePickupKind.Medkit)
                {
                    var restored = player.RestoreHealth(Mathf.CeilToInt(player.MaxHealth * 0.25f));
                    if (restored > 0)
                    {
                        SpawnImpactFlash(player.Position, new Color(0.28f, 0.92f, 0.64f, 0.9f), 1.25f);
                        SpawnFloatingReward(player.Position, $"+{restored}", "heal", HealthRewardColor);
                    }
                }
                else
                {
                    GrantSupplyBoxReward();
                    SpawnImpactFlash(player.Position, new Color(1f, 0.82f, 0.32f, 0.9f), 1.25f);
                }

                BattleActivityBridge.ReportResourcesCollected(1);
            }
        }

        /// <summary>
        /// Supply box loot: one of three random bonuses. Chips reuse the same shared
        /// popup/selection flow as every other chip source in the game; the temporary
        /// weapon only ever exists in this battle's <see cref="_inventory"/> runtime,
        /// never touching the saved loadout, so it can't leak into future battles.
        /// </summary>
        private void GrantSupplyBoxReward()
        {
            var roll = Random.value;
            if (roll < 1f / 3f && TryPickTemporaryWeaponReward(out var weapon))
            {
                _inventory.TryAddTemporaryWeapon(weapon, 1);
                RefreshArsenalHudItems();
                _status = GameText.SupplyBoxWeapon(WeaponDisplayNames.Get(weapon.WeaponId));
                SpawnFloatingReward(GetPresenter(BattleTeamId.Player)?.State.Position ?? Vector2.zero,
                    "+1", weapon.IconId, WeaponRewardColor);
                return;
            }

            if (roll < 2f / 3f)
            {
                var chipAmount = Random.Range(5, 13);
                BattleRewardBridge.GrantWeaponBlueprints(chipAmount, "battle.supplyBox", GameSessionContext.SelectedMissionId ?? "battle");
                SpawnFloatingReward(GetPresenter(BattleTeamId.Player)?.State.Position ?? Vector2.zero,
                    $"+{chipAmount}", "UI/Common/chip_2", BlueprintRewardColor);
                return;
            }

            var coinAmount = Random.Range(60, 121);
            BattleRewardBridge.GrantCoins(coinAmount, "battle.supplyBox", "coins");
            _status = GameText.SupplyBoxCoins(coinAmount);
            SpawnFloatingReward(GetPresenter(BattleTeamId.Player)?.State.Position ?? Vector2.zero,
                $"+{coinAmount}", "UI/Shop/coin", CoinRewardColor);
        }

        private bool TryPickTemporaryWeaponReward(out WeaponDefinition weapon)
        {
            weapon = null;
            var commanderLevel = CommanderProgressionTable.GetLevel(BattleRewardBridge.CommanderExperience);
            var candidates = new List<WeaponDefinition>();
            foreach (var candidate in _weaponsById.Values)
            {
                if (candidate == null
                    || WeaponPresentationText.IsDropOnly(candidate)
                    || !WeaponProgressionBridge.IsWeaponUnlocked(candidate.WeaponId)
                    || !WeaponProgressionCatalog.TryGet(candidate.WeaponId, out var definition)
                    || definition.RequiredCommanderLevel > commanderLevel + 1)
                {
                    continue;
                }

                candidates.Add(candidate);
            }

            if (candidates.Count == 0)
            {
                return false;
            }

            weapon = candidates[Random.Range(0, candidates.Count)];
            return true;
        }

        private void SpawnFloatingReward(Vector2 worldPosition, string amountText, string iconReference, Color color)
        {
            if (string.IsNullOrWhiteSpace(amountText))
            {
                return;
            }

            var reward = new GameObject("Collected Reward");
            reward.transform.position = new Vector3(worldPosition.x, worldPosition.y + 0.62f, 0f);
            _arenaObjects.Add(reward);

            var icon = LoadFloatingRewardIcon(iconReference);
            if (icon != null)
            {
                var iconObject = new GameObject("Reward Icon");
                iconObject.transform.SetParent(reward.transform, false);
                iconObject.transform.localPosition = new Vector3(-0.52f, 0f, 0f);
                iconObject.transform.localScale = Vector3.one *
                    (FloatingRewardIconWorldWidth / Mathf.Max(0.001f, icon.bounds.size.x));
                var iconRenderer = iconObject.AddComponent<SpriteRenderer>();
                iconRenderer.sprite = icon;
                iconRenderer.color = color;
                iconRenderer.sortingOrder = 36;
            }

            var labelObject = new GameObject("Reward Amount");
            labelObject.transform.SetParent(reward.transform, false);
            labelObject.transform.localPosition = new Vector3(icon == null ? 0f : 0.24f, 0f, 0f);
            labelObject.transform.localScale = Vector3.one * 0.035f;
            var label = labelObject.AddComponent<TextMesh>();
            label.text = amountText;
            label.alignment = TextAlignment.Left;
            label.anchor = TextAnchor.MiddleLeft;
            label.fontSize = 116;
            label.fontStyle = FontStyle.Bold;
            label.color = color;
            _floatingRewardFont ??= Resources.Load<Font>(FloatingRewardFontResourcePath);
            if (_floatingRewardFont != null)
            {
                label.font = _floatingRewardFont;
                var textRenderer = labelObject.GetComponent<MeshRenderer>();
                if (textRenderer != null)
                {
                    textRenderer.sharedMaterial = _floatingRewardFont.material;
                    textRenderer.sortingOrder = 37;
                }
            }

            StartCoroutine(AnimateFloatingReward(reward, label, color));
        }

        private Sprite LoadFloatingRewardIcon(string iconReference)
        {
            return iconReference != null && iconReference.StartsWith("UI/", System.StringComparison.Ordinal)
                ? LoadCenteredTextureSprite(iconReference, "Reward Icon")
                : LoadWeaponIconSprite(iconReference);
        }

        private IEnumerator AnimateFloatingReward(GameObject reward, TextMesh label, Color color)
        {
            var spriteRenderers = reward.GetComponentsInChildren<SpriteRenderer>();
            var startPosition = reward.transform.position;
            var elapsed = 0f;
            while (reward != null && elapsed < FloatingRewardDurationSeconds)
            {
                elapsed += Time.deltaTime;
                var progress = Mathf.Clamp01(elapsed / FloatingRewardDurationSeconds);
                var alpha = 1f - Mathf.SmoothStep(0f, 1f, progress);
                reward.transform.position = startPosition + Vector3.up * (FloatingRewardRiseDistance * progress);
                foreach (var renderer in spriteRenderers)
                {
                    if (renderer == null)
                    {
                        continue;
                    }

                    var rendererColor = renderer.color;
                    rendererColor.a = color.a * alpha;
                    renderer.color = rendererColor;
                }

                if (label != null)
                {
                    var labelColor = color;
                    labelColor.a *= alpha;
                    label.color = labelColor;
                }

                yield return null;
            }

            if (reward != null)
            {
                _arenaObjects.Remove(reward);
                Destroy(reward);
            }
        }

        private void SyncProjectileVisuals()
        {
            if (_simulation == null)
            {
                return;
            }

            foreach (var projectile in _simulation.Projectiles)
            {
                if (_projectileVisuals.TryGetValue(projectile.Id, out var projectileObject) && projectileObject != null)
                {
                    projectileObject.transform.position = projectile.Position;
                    if (IsCopterBody(projectile.Definition.ProjectileId)
                        && projectileObject.transform.childCount > 0)
                    {
                        // The copter art faces right: it mirrors with the direction of travel.
                        var body = projectileObject.transform.GetChild(0);
                        var facing = projectile.Velocity.x < -0.05f ? -1f : projectile.Velocity.x > 0.05f ? 1f : Mathf.Sign(body.localScale.x);
                        body.localScale = new Vector3(Mathf.Abs(body.localScale.x) * facing, body.localScale.y, body.localScale.z);
                    }
                    else if (projectile.Definition.ProjectileId == "projectile.weapon.bomber.bomb"
                        || projectile.Definition.ProjectileId == GuardianDeployProjectileId)
                    {
                        // Keeps its pose (the bomb vertical, the thrown guardian upright); nothing to rotate.
                    }
                    else if (projectileObject.transform.childCount > 0
                        && projectile.Velocity.sqrMagnitude > 0.01f
                        && ResolveWeaponFromProjectileId(projectile.Definition.ProjectileId)?.WeaponId is not
                            ("weapon.fpv-drone" or "weapon.titanium-disc"))
                    {
                        var artOffset = ResolveProjectileArtRotationOffset(
                            ResolveWeaponFromProjectileId(projectile.Definition.ProjectileId));
                        projectileObject.transform.GetChild(0).rotation = Quaternion.Euler(0f, 0f,
                            Mathf.Atan2(projectile.Velocity.y, projectile.Velocity.x) * Mathf.Rad2Deg + artOffset);
                    }
                }
            }

            var activeIds = new HashSet<int>(_simulation.Projectiles.Select(projectile => projectile.Id));
            foreach (var projectileId in _projectileVisuals.Keys.Where(id => !activeIds.Contains(id)).ToArray())
            {
                var visual = _projectileVisuals[projectileId];
                _projectileVisuals.Remove(projectileId);
                _arenaObjects.Remove(visual);
                Destroy(visual);
            }
        }

        private Vector2 ResolveUnitLandingPosition(Vector2 desiredPoint, BattleTeamId teamId)
        {
            var profile = CreateMotionProfile(ArchetypeFor(teamId));
            var resolver = new BattleSpawnResolver(
                _battleTerrain?.Grid,
                profile,
                GetFallDeathY(),
                GroundRayStartHeight,
                GroundRayDistance,
                _battleTerrain == null
                    ? 2.5f
                    : _battleTerrain.Grid.WorldMaxX - _battleTerrain.Grid.WorldMinX);
            if (resolver.TryResolve(desiredPoint, out var safePosition))
            {
                return safePosition;
            }

            // Preserve the authored anchor as a last-resort fallback for a
            // malformed arena, but make the failure visible for content QA.
            Debug.LogWarning($"No safe spawn found near {desiredPoint} for {teamId} in arena '{_currentArena?.ArenaId}'.");
            return TryResolveGroundPosition(desiredPoint, teamId, out var landingPosition)
                ? landingPosition
                : desiredPoint;
        }

        private bool TryResolveGroundPosition(Vector2 desiredPoint, BattleTeamId teamId, out Vector2 landingPosition)
        {
            landingPosition = desiredPoint;
            if (_battleTerrain == null)
            {
                return false;
            }

            var bodyHeight = ArchetypeFor(teamId).BodyHeight;
            var scanTop = Mathf.Max(desiredPoint.y + GroundRayStartHeight, bodyHeight + GroundRayStartHeight);
            if (!_battleTerrain.Grid.TryFindSurfaceBelow(desiredPoint.x, scanTop, GroundRayDistance + GroundRayStartHeight, out var surfaceY))
            {
                return false;
            }

            landingPosition = new Vector2(desiredPoint.x, surfaceY + bodyHeight * 0.5f);
            return true;
        }

        private void SetUiHorizontalInput(int direction)
        {
            // While the turn transfer is choosing, left/right browse the allies instead of walking.
            if (_simulation != null && _simulation.HasTurnTransferSelection)
            {
                _simulation.SelectTurnTransferTarget(NormalizeDirection(direction));
                return;
            }

            if (_simulation != null && _simulation.HasActiveControlledProjectile)
            {
                _controlledUiHorizontalInput = NormalizeDirection(direction);
                return;
            }

            if (IsTitaniumBarrierPlacementActive())
            {
                _controlledUiHorizontalInput = NormalizeDirection(direction);
                return;
            }

            SetUiWalkInput(direction);
        }

        private void StopUiHorizontalInput(int direction)
        {
            if (_simulation != null && _simulation.HasActiveControlledProjectile)
            {
                if (_controlledUiHorizontalInput == NormalizeDirection(direction))
                {
                    _controlledUiHorizontalInput = 0;
                }

                return;
            }

            if (IsTitaniumBarrierPlacementActive())
            {
                if (_controlledUiHorizontalInput == NormalizeDirection(direction))
                {
                    _controlledUiHorizontalInput = 0;
                }

                return;
            }

            StopUiWalkInput(direction);
        }

        private void SetUiWalkInput(int direction)
        {
            if (_firstBattleGuide != null
                && _firstBattleGuide.IsActive
                && !_firstBattleGuide.AllowsWalkDirection(direction))
            {
                _uiWalkInputDirection = 0;
                return;
            }

            if (!CanPlayerAcceptMovementInput())
            {
                _uiWalkInputDirection = 0;
                if (IsMovementBlockedByGuide())
                {
                    _status = GameText.MovementGuideBlocked();
                }

                return;
            }

            _uiWalkInputDirection = NormalizeDirection(direction);
            EmitDuelCommand(BattleCommandKind.WalkIntent, _uiWalkInputDirection);
        }

        private void StopUiWalkInput(int direction)
        {
            if (_uiWalkInputDirection == NormalizeDirection(direction))
            {
                _uiWalkInputDirection = 0;
                EmitDuelCommand(BattleCommandKind.WalkIntent, 0);
            }
        }

        private void SetKeyboardWalkInput(int direction)
        {
            var normalizedDirection = NormalizeDirection(direction);
            _keyboardWalkInputDirection = CanPlayerAcceptMovementInput()
                && (_firstBattleGuide == null || !_firstBattleGuide.IsActive || _firstBattleGuide.AllowsWalkDirection(normalizedDirection))
                    ? normalizedDirection
                    : 0;
        }

        private static int NormalizeDirection(int direction)
        {
            return direction < 0 ? -1 : direction > 0 ? 1 : 0;
        }

        private bool CanPlayerUseMovement()
        {
            return IsBattleReady()
                && IsPlayerControlTurn(_battleState.Turn.ActiveTeam)
                && _simulation.CanUseMovement();
        }

        private bool CanPlayerAcceptMovementInput()
        {
            return IsBattleReady()
                && IsPlayerControlTurn(_battleState.Turn.ActiveTeam)
                && _simulation.CanAcceptMovementInput();
        }

        private static bool IsPlayerControlTurn(BattleTeamId activeTeam)
        {
            return activeTeam == BattleTeamId.Player;
        }

        private bool IsMovementBlockedByGuide()
        {
            return _firstBattleGuide != null && _firstBattleGuide.IsActive;
        }

        private void UpdateWalking()
        {
            if (_simulation != null && (_simulation.IsPlayerRopeAttached || _simulation.HasPlayerBoost))
            {
                UpdateRopeTraversalInput();
                return;
            }
            var walkDirection = _keyboardWalkInputDirection != 0 ? _keyboardWalkInputDirection : _uiWalkInputDirection;
            if (walkDirection == 0)
            {
                // During the two mandatory navigation steps, preserve an accepted
                // intent until at least one fixed simulation tick has moved the unit.
                if (_pendingTutorialWalkDirection == 0)
                {
                    _simulation?.Walk(0, 0f);
                }

                return;
            }

            if (!CanPlayerUseMovement())
            {
                // Preserve the held direction while the motor is airborne. The
                // command is submitted again automatically after landing.
                return;
            }

            var outcome = _simulation.Walk(walkDirection, 0f);
            switch (outcome)
            {
                case BattleWalkOutcome.Moved:
                    ResetAimState();
                    _status = GameText.Moved(DescribeTeam(BattleTeamId.Player));
                    if (_firstBattleGuide != null
                        && (_firstBattleGuide.CurrentPhase == FirstBattleGuideState.GuidePhase.MoveLeft
                            || _firstBattleGuide.CurrentPhase == FirstBattleGuideState.GuidePhase.MoveRight))
                    {
                        if (_pendingTutorialWalkDirection == 0)
                        {
                            _pendingTutorialWalkDirection = walkDirection;
                            _pendingTutorialWalkStartX = GetPresenter(BattleTeamId.Player)?.State.Position.x ?? 0f;
                        }
                    }
                    else
                    {
                        _firstBattleGuide?.ObserveWalk(walkDirection);
                    }
                    break;
                case BattleWalkOutcome.Blocked:
                    _status = GameText.MovementBlocked();
                    ClearWalkInput();
                    break;
                case BattleWalkOutcome.StartedFalling:
                    // Regular physics takes over from here (HasActiveMotion).
                    ResetAimState();
                    break;
            }
        }

        private void UpdateRopeTraversalInput()
        {
            if (_simulation == null)
            {
                return;
            }

            var horizontal = _keyboardWalkInputDirection != 0 ? _keyboardWalkInputDirection : _uiWalkInputDirection;
            if (_simulation.HasPlayerBoost)
            {
                // The shared rope buttons report "up" as -1; the fuel steering uses up = +1.
                _simulation.SetBoostInput(horizontal, -_ropeVerticalInput);
                return;
            }

            if (!_simulation.IsPlayerRopeAttached)
            {
                return;
            }

            _simulation.SetRopeTraversalInput(horizontal, _ropeVerticalInput);
        }

        private void UpdateRopeVisual()
        {
            if (_ropeLine == null || _simulation == null || !_simulation.HasPlayerRopeTraversal)
            {
                if (_ropeLine != null) _ropeLine.enabled = false;
                if (_ropeHookVisual != null) _ropeHookVisual.SetActive(false);
                return;
            }

            var presenter = GetPresenter(BattleTeamId.Player);
            if (presenter == null)
            {
                return;
            }

            var hookPosition = _simulation.RopeHookPosition;
            _ropeLine.enabled = true;
            _ropeLine.SetPosition(0, presenter.State.Position);
            _ropeLine.SetPosition(1, hookPosition);
            if (_ropeHookVisual == null)
            {
                _ropeHookVisual = new GameObject("Rope Hook");
                _arenaObjects.Add(_ropeHookVisual);
                _ropeHookVisual.AddComponent<SpriteRenderer>();
                var renderer = _ropeHookVisual.GetComponent<SpriteRenderer>();
                renderer.sortingOrder = 32;
                _ropeHookVisual.transform.localScale = Vector3.one * 0.045f;
            }

            var hookRenderer = _ropeHookVisual.GetComponent<SpriteRenderer>();
            var hookSprite = LoadWeaponIconSprite(RopeHookSpriteId);
            if (hookRenderer != null && hookSprite != null && hookRenderer.sprite != hookSprite)
            {
                hookRenderer.sprite = hookSprite;
            }

            _ropeHookVisual.SetActive(true);
            _ropeHookVisual.transform.position = hookPosition;
            var hookDirection = _simulation.RopeHookDirection;
            if (hookDirection.sqrMagnitude > 0.0001f)
            {
                _ropeHookVisual.transform.rotation = Quaternion.Euler(0f, 0f,
                    Mathf.Atan2(hookDirection.y, hookDirection.x) * Mathf.Rad2Deg);
            }
        }

        private void UpdateElectricHarpoonVisual()
        {
            if (_electricHarpoonLine == null || _simulation == null || !_simulation.HasPlayerElectricHarpoon)
            {
                if (_electricHarpoonLine != null) _electricHarpoonLine.enabled = false;
                if (_electricHarpoonExtraLine != null) _electricHarpoonExtraLine.enabled = false;
                if (_electricHarpoonHookVisual != null) _electricHarpoonHookVisual.SetActive(false);
                return;
            }

            var presenter = GetPresenter(BattleTeamId.Player);
            if (presenter == null)
            {
                return;
            }

            var hookPosition = _simulation.ElectricHarpoonHookPosition;
            _electricHarpoonLine.enabled = true;
            _electricHarpoonLine.SetPosition(0, presenter.State.Position);
            _electricHarpoonLine.SetPosition(1, hookPosition);
            var extraTargets = _simulation.ElectricHarpoonExtraTargets;
            if (_electricHarpoonExtraLine != null)
            {
                // The double grab holds a second droid on its own taut rope.
                _electricHarpoonExtraLine.enabled = extraTargets.Count > 0;
                if (extraTargets.Count > 0)
                {
                    _electricHarpoonExtraLine.SetPosition(0, presenter.State.Position);
                    _electricHarpoonExtraLine.SetPosition(1, extraTargets[0]);
                }
            }
            if (_electricHarpoonHookVisual == null)
            {
                _electricHarpoonHookVisual = new GameObject("Electric Harpoon Hook");
                _arenaObjects.Add(_electricHarpoonHookVisual);
                _electricHarpoonHookVisual.AddComponent<SpriteRenderer>();
                var renderer = _electricHarpoonHookVisual.GetComponent<SpriteRenderer>();
                renderer.sortingOrder = 33;
                _electricHarpoonHookVisual.transform.localScale = Vector3.one * 0.045f;
            }

            var hookRenderer = _electricHarpoonHookVisual.GetComponent<SpriteRenderer>();
            var evolvedHarpoonId = _weaponsById.TryGetValue("weapon.electric-harpoon", out var harpoon)
                ? ResolveEvolvedIconId(harpoon.IconId, harpoon, _playerProgression)
                : "electric_harpoon";
            var hookSprite = LoadWeaponIconSprite(
                evolvedHarpoonId == "electric_harpoon_5A"
                    ? "electric_harpoon_5A_shot"
                    : "electric_harpoon_shot");
            if (hookRenderer != null && hookSprite != null && hookRenderer.sprite != hookSprite)
            {
                hookRenderer.sprite = hookSprite;
            }

            _electricHarpoonHookVisual.SetActive(true);
            _electricHarpoonHookVisual.transform.position = hookPosition;
            var hookDirection = _simulation.ElectricHarpoonDirection;
            if (hookDirection.sqrMagnitude > 0.0001f)
            {
                _electricHarpoonHookVisual.transform.rotation = Quaternion.Euler(0f, 0f,
                    Mathf.Atan2(hookDirection.y, hookDirection.x) * Mathf.Rad2Deg);
            }
        }

        private void ClearWalkInput()
        {
            EmitDuelCommand(BattleCommandKind.WalkIntent, 0);
            _uiWalkInputDirection = 0;
            _keyboardWalkInputDirection = 0;
            _controlledUiHorizontalInput = 0;
            _ropeVerticalInput = 0;
            _pendingTutorialWalkDirection = 0;
            _simulation?.Walk(0, 0f);
        }

        private void CompletePendingTutorialWalkIfMoved()
        {
            if (_pendingTutorialWalkDirection == 0)
            {
                return;
            }

            var presenter = GetPresenter(BattleTeamId.Player);
            if (presenter == null
                || (presenter.State.Position.x - _pendingTutorialWalkStartX) * _pendingTutorialWalkDirection < FirstBattleNavigationDistance)
            {
                return;
            }

            var completedDirection = _pendingTutorialWalkDirection;
            _pendingTutorialWalkDirection = 0;
            _firstBattleGuide?.ObserveWalk(completedDirection);
            ClearWalkInput();
        }

        private void TryStartJump(int direction)
        {
            EmitDuelCommand(BattleCommandKind.Jump, direction);
            if (_firstBattleGuide != null && _firstBattleGuide.IsActive && !_firstBattleGuide.AllowsJumpDirection(direction))
            {
                return;
            }

            if (!CanPlayerUseMovement())
            {
                if (IsMovementBlockedByGuide())
                {
                    _status = GameText.MovementGuideBlocked();
                }

                return;
            }

            var outcome = _simulation.TryStartJump(direction);
            if (outcome == BattleJumpOutcome.Jumped)
            {
                ResetAimState();
                _status = GameText.Jumped(DescribeTeam(BattleTeamId.Player));
                _firstBattleGuide?.ObserveJump(direction);
            }
        }

        private void RefreshUnitViews()
        {
            var freezeOverlay = LoadCenteredTextureSprite(FreezeOverlayResourcePath, "Freeze Overlay Ice 01");
            var poisonOverlay = LoadCenteredTextureSprite(PoisonOverlayResourcePath, "Poison Overlay Toxic 01");
            var lavaOverlay = LoadCenteredTextureSprite(LavaOverlayResourcePath, "Lava Overlay 01");
            var electroOverlayFrames = LoadElectroOverlayFrames();
            foreach (var presenter in _unitPresenters)
            {
                var isJumping = _unitMotors.TryGetValue(presenter.State.UnitId, out var motor)
                    && (motor.IsJumping || motor.IsHovering);
                var jumpTrailScale = isJumping ? motor.JumpTrailScale : 0f;
                presenter.Refresh(presenter.State.UnitId == _battleState.Turn.ActiveUnitId, isJumping, jumpTrailScale);
                presenter.View.SetFreezeOverlay(freezeOverlay,
                    _simulation.IsUnitFrozen(presenter.State.UnitId) || _simulation.IsUnitChilled(presenter.State.UnitId));
                presenter.View.SetPoisonOverlay(poisonOverlay, _simulation.IsUnitPoisoned(presenter.State.UnitId));
                presenter.View.SetLavaOverlay(lavaOverlay, _simulation.IsUnitLavaBurning(presenter.State.UnitId));
                presenter.View.SetElectroOverlay(electroOverlayFrames, _simulation.IsUnitElectrified(presenter.State.UnitId));
                presenter.SetRepairJammerActive(_simulation.IsUnitRepairJammed(presenter.State.UnitId));
                presenter.SetVirusActive(_simulation.IsUnitInfected(presenter.State.UnitId));
                presenter.SetSelfDestructActive(_simulation.IsUnitSelfDestructing(presenter.State.UnitId));
            }
        }

        private Sprite[] LoadElectroOverlayFrames()
        {
            if (_electroOverlayFrames != null)
            {
                return _electroOverlayFrames;
            }

            _electroOverlayFrames = new Sprite[ElectroOverlayResourcePaths.Length];
            for (var index = 0; index < ElectroOverlayResourcePaths.Length; index++)
            {
                _electroOverlayFrames[index] = LoadCenteredTextureSprite(
                    ElectroOverlayResourcePaths[index],
                    $"Electro Overlay {index + 1}");
            }

            return _electroOverlayFrames;
        }

        private void UpdateOpponentBot()
        {
            // The bot plans and paces its own turn; ticking the simulation lets any
            // shot it fires (Stage 3: also movement) integrate this frame.
            _botTurnController?.Tick(Time.deltaTime);
            _simulation.Tick(Time.deltaTime);
            _status = _botTurnController?.Activity switch
            {
                BotActivity.Repositioning => GameText.Moved(DescribeTeam(BattleTeamId.Opponent)),
                BotActivity.Firing => GameText.OpponentFiring(),
                _ => GameText.OpponentAiming()
            };
        }

        private BotTurnController CreateBotTurnController()
        {
            var probe = new TrajectoryProbe(
                _battleTerrain.Grid,
                Physics2D.gravity,
                SimulationTickSeconds,
                BattleSimulation.ProjectileSubStepLength,
                GetFallDeathY());
            var profile = BotProfileProvider.Resolve(_aiDifficulty);
            var random = new SystemBotRandom(System.Environment.TickCount);
            IBotShotPlanner planner = _aiDifficulty == AiDifficulty.Easy
                ? new HeuristicShotPlanner(
                    _aiDifficulty,
                    new BallisticShotPlanner(probe, profile, random, Physics2D.gravity))
                : new BallisticShotPlanner(probe, profile, random, Physics2D.gravity);

            var opponentMotionProfile = CreateMotionProfile(_opponentArchetype);
            var scanner = new WalkReachabilityScanner(_battleTerrain.Grid, opponentMotionProfile, GetFallDeathY());
            var jumpGenerator = new JumpCandidateGenerator(_battleTerrain.Grid, opponentMotionProfile, _opponentArchetype, GetFallDeathY());
            var positionPlanner = new BotPositionPlanner(scanner, planner, profile, opponentMotionProfile.WalkSpeed, jumpGenerator);

            return new BotTurnController(
                _simulation,
                planner,
                positionPlanner,
                profile,
                random,
                UnitCollisionHalfWidth,
                _playerArchetype,
                _opponentArchetype,
                _baseAimSettings);
        }

        private void ApplyAimSettingsForActiveTeam()
        {
            var activeInventory = _simulation.ActiveInventory;
            var weapon = activeInventory?.Selected ?? _inventory.Selected;
            _aimSettings = weapon.CreateAimSettings(_baseAimSettings);
        }

        private static BattleResultScreenState CreateResultScreenState(
            BattleResult result,
            int turnNumber,
            BattleArenaReward arenaReward)
        {
            var state = BattleResultScreenFactory.Create(result, turnNumber);

            // A raid or duel has already been paid by the time the card appears, so it
            // shows the banked numbers - including on a loss, which still pays a
            // reduced amount and should say so.
            if (arenaReward.Granted)
            {
                return new BattleResultScreenState(
                    result,
                    state.Title,
                    state.Subtitle,
                    new BattleRewardPreview(
                        arenaReward.Credits,
                        string.Empty,
                        arenaReward.CommanderExperience,
                        arenaReward.Blueprints,
                        arenaReward.IsReduced),
                    state.RetryLabel,
                    state.ContinueLabel);
            }

            if (result.Outcome != BattleOutcome.PlayerWon)
            {
                return state;
            }

            var hasUnlock = !string.IsNullOrWhiteSpace(GameSessionContext.SelectedMissionRewardUnlockWeaponId);
            var unlockText = !hasUnlock
                ? string.Empty
                : GameText.UnlockAfterBattle(DescribeWeaponId(GameSessionContext.SelectedMissionRewardUnlockWeaponId));

            return new BattleResultScreenState(
                result,
                state.Title,
                state.Subtitle,
                new BattleRewardPreview(GameSessionContext.SelectedMissionRewardSoftCurrency, unlockText),
                state.RetryLabel,
                hasUnlock ? GameText.ContinueToLoadout() : state.ContinueLabel);
        }

        private BattleUnitPresenter GetActivePresenter()
        {
            return _unitPresenters.FirstOrDefault(presenter => presenter.State.UnitId == _battleState.Turn.ActiveUnitId);
        }

        private BattleUnitPresenter GetPresenter(BattleTeamId teamId)
        {
            return _unitPresenters.FirstOrDefault(presenter => presenter.State.TeamId == teamId && !presenter.State.IsDefeated);
        }

        private void DrawAim()
        {
            _aimLine.enabled = false;
        }

        private void DrawTrajectoryPreview()
        {
            if (!_aimState.HasInput)
            {
                HideTrajectoryPreview();
                return;
            }

            var activePresenter = GetActivePresenter();
            if (activePresenter == null)
            {
                HideTrajectoryPreview();
                return;
            }

            if (_inventory.Selected.Role == WeaponRole.DefensiveTool)
            {
                HideTrajectoryPreview();
                return;
            }

            if (_inventory.Selected.Role == WeaponRole.ShortRangeBurst
                && _inventory.Selected.WeaponId != "weapon.bur"
                && _inventory.Selected.WeaponId != "weapon.fire-gun")
            {
                HideTrajectoryPreview();
                return;
            }

            if (_inventory.Selected.Role == WeaponRole.CoverTool)
            {
                DrawCoverPlacementPreview(_aimState.Origin);
                return;
            }

            if (IsInstantUtility(_inventory.Selected.Role))
            {
                HideTrajectoryPreview();
                return;
            }

            DrawBallisticTrajectory(_aimState.Origin, _inventory.Selected);
        }

        private static bool IsInstantUtility(WeaponRole role)
        {
            return role == WeaponRole.CoverTool
                || role == WeaponRole.DefensiveTool;
        }

        private void DrawBallisticTrajectory(Vector2 firePoint, WeaponDefinition weapon)
        {
            _trajectoryLine.enabled = false;

            if (weapon?.WeaponId == "weapon.rail-rifle")
            {
                DrawRailRifleLaser(firePoint, weapon);
                return;
            }

            var power01 = Mathf.Approximately(_aimSettings.MinPower, _aimSettings.MaxPower)
                ? 1f
                : Mathf.InverseLerp(_aimSettings.MinPower, _aimSettings.MaxPower, _aimState.Power);
            var previewLength = ResolveTrajectoryPreviewLength(weapon,
                Mathf.Lerp(MinTrajectoryPreviewLength, MaxTrajectoryPreviewLength, Mathf.Sqrt(power01)));
            var previewEndTime = FindTrajectoryPreviewEndTime(firePoint, weapon, previewLength);
            var upwardAim01 = Mathf.InverseLerp(0.12f, 0.86f, _aimState.Direction.y);
            var weakShot01 = 1f - power01;
            var minimumArcTime = Mathf.Lerp(0.34f, 0.78f, upwardAim01 * weakShot01);
            previewEndTime = Mathf.Max(previewEndTime, minimumArcTime);

            // More dots at higher power. Because the length grows faster than the
            // count, the gap between dots widens with power as well.
            var isLongDirectWeapon = IsLongDirectWeapon(weapon);
            var activeDotCount = isLongDirectWeapon
                ? 10
                : Mathf.Clamp(
                    Mathf.RoundToInt(Mathf.Lerp(MinTrajectoryDotCount, MaxTrajectoryDotCount, power01)),
                    MinTrajectoryDotCount,
                    _trajectoryDots.Count);

            // Training mission: extend the arc all the way to the predicted
            // landing point so a new player sees exactly where the shell falls.
            Vector2? trajectoryEndPoint = null;
            if (!isLongDirectWeapon
                && _impactGuideEnabled
                && TryProbeTrajectoryEnd(firePoint, weapon, out var impactReport))
            {
                var impactArcLength = CalculateTrajectoryArcLength(firePoint, weapon, impactReport.FlightTime);
                activeDotCount = CalculateFullTrajectoryDotCount(impactArcLength);
                EnsureTrajectoryDotCapacity(activeDotCount);
                trajectoryEndPoint = impactReport.EndPoint;
                previewEndTime = Mathf.Max(previewEndTime, impactReport.FlightTime);
            }

            for (var i = 0; i < _trajectoryDots.Count; i++)
            {
                var dot = _trajectoryDots[i];
                if (i >= activeDotCount)
                {
                    dot.enabled = false;
                    continue;
                }

                var t01 = activeDotCount <= 1 ? 0f : (float)i / (activeDotCount - 1);
                var time01 = Mathf.Pow(t01, 0.86f);
                var point = GetTrajectoryPreviewPointAtTime(firePoint, weapon, previewEndTime * time01);
                if (trajectoryEndPoint.HasValue && i == activeDotCount - 1)
                {
                    // Pin the last dot to the probed impact, not the analytic
                    // curve, so the arc ends exactly where the shot will land.
                    point = trajectoryEndPoint.Value;
                }

                var scale = Mathf.Lerp(0.24f, 0.115f, t01);

                dot.enabled = true;
                dot.transform.position = new Vector3(point.x, point.y, -0.18f);
                dot.transform.localScale = new Vector3(scale, scale, 1f);
                dot.color = Color.white;
            }
        }

        private float ResolveTrajectoryPreviewLength(WeaponDefinition weapon, float baseLength)
        {
            if (weapon?.WeaponId != "weapon.granata"
                && weapon?.WeaponId != "weapon.impulse"
                && weapon?.WeaponId != "weapon.titanium-disc")
            {
                return baseLength;
            }

            var aimUpgrades = _playerProgression?.ResolveUpgradeCount(
                weapon.WeaponId, WeaponUpgradePath.Aim) ?? 0;
            return baseLength * WeaponUpgradeChoiceResolver.ResolveAimPreviewMultiplier(aimUpgrades);
        }

        private static bool IsLongDirectWeapon(WeaponDefinition weapon)
        {
            return weapon?.WeaponId == "weapon.blaster-caset"
                || weapon?.WeaponId == "weapon.emp";
        }

        private Vector2 TraceRailLaserEnd(Vector2 start, Vector2 direction)
        {
            var grid = _battleTerrain?.Grid;
            if (grid == null || direction.sqrMagnitude < 0.0001f)
            {
                return start + direction * MaxTrajectoryPreviewLength;
            }

            direction = direction.normalized;
            var step = Mathf.Max(0.02f, Mathf.Min(grid.CellSize.x, grid.CellSize.y));
            var point = start;
            for (var i = 0; i < 20000; i++)
            {
                var next = point + direction * step;
                if (next.x < grid.WorldMinX || next.x > grid.WorldMaxX || next.y < grid.WorldMinY || next.y > grid.WorldMaxY
                    || grid.IsSolidWorld(next))
                {
                    return point;
                }

                point = next;
            }

            return point;
        }

        private void DrawRailRifleLaser(Vector2 firePoint, WeaponDefinition weapon)
        {
            HideTrajectoryDots();
            var muzzle = firePoint + _aimState.Direction * ProjectileMuzzleOffset;
            // A sniper laser: the line runs straight on until it meets ground or leaves the arena.
            var endPoint = TraceRailLaserEnd(muzzle, _aimState.Direction);

            var laserColor = new Color(1f, 0.12f, 0.12f, 0.95f);
            _trajectoryLine.startColor = laserColor;
            _trajectoryLine.endColor = laserColor;
            _trajectoryLine.startWidth = 0.024f;
            _trajectoryLine.endWidth = 0.024f;
            _trajectoryLine.positionCount = 2;
            _trajectoryLine.SetPosition(0, muzzle);
            _trajectoryLine.SetPosition(1, endPoint);
            _trajectoryLine.enabled = true;
        }

        /// <summary>
        /// Probes the exact shot through the same physics the simulation runs.
        /// True when the shell hits terrain/a unit or falls out of the simulated
        /// world. Timeouts have no physical landing point to show.
        /// </summary>
        private bool TryProbeTrajectoryEnd(Vector2 firePoint, WeaponDefinition weapon, out TrajectoryReport report)
        {
            report = default;
            if (_impactGuideProbe == null)
            {
                return false;
            }

            var muzzle = firePoint + _aimState.Direction * ProjectileMuzzleOffset;
            BuildImpactGuideBodies();
            report = _impactGuideProbe.Simulate(
                muzzle,
                _aimState.Direction * _aimState.Power,
                weapon.GravityScale,
                weapon.ProjectileRadius,
                weapon.TimeoutSeconds,
                new TrajectoryObstacles(_impactGuideBodies, _aimState.Target),
                weapon);

            return report.EndKind != TrajectoryEndKind.Timeout;
        }

        private static int CalculateFullTrajectoryDotCount(float arcLength)
        {
            if (!float.IsFinite(arcLength) || arcLength <= 0f)
            {
                return MinTrajectoryDotCount;
            }

            return Mathf.Clamp(
                Mathf.CeilToInt(arcLength / MaxTrajectoryDotSpacing) + 1,
                MinTrajectoryDotCount,
                MaxFullTrajectoryDotCount);
        }

        private void BuildImpactGuideBodies()
        {
            _impactGuideBodies.Clear();
            var activeUnitId = _battleState.Turn.ActiveUnitId;
            foreach (var team in _battleState.Teams)
            {
                var archetype = team.TeamId == BattleTeamId.Player ? _playerArchetype : _opponentArchetype;
                var halfHeight = archetype.BodyHeight * 0.5f;
                foreach (var unit in team.Units)
                {
                    if (unit == null || unit.IsDefeated || unit.UnitId == activeUnitId)
                    {
                        continue;
                    }

                    _impactGuideBodies.Add(new TrajectoryUnitBody(
                        unit.Position,
                        UnitCollisionHalfWidth,
                        halfHeight,
                        isAlly: team.TeamId == BattleTeamId.Player,
                        isTarget: false));
                }
            }
        }

        private float FindTrajectoryPreviewEndTime(Vector2 firePoint, WeaponDefinition weapon, float targetLength)
        {
            const float sampleStepTime = 0.018f;
            const int maxSamples = 120;

            var distance = 0f;
            var previous = GetTrajectoryPreviewPointAtTime(firePoint, weapon, 0f);
            for (var i = 1; i <= maxSamples; i++)
            {
                var t = i * sampleStepTime;
                var current = GetTrajectoryPreviewPointAtTime(firePoint, weapon, t);
                distance += Vector2.Distance(previous, current);
                if (distance >= targetLength)
                {
                    return t;
                }

                previous = current;
            }

            return maxSamples * sampleStepTime;
        }

        private Vector2 GetTrajectoryPreviewPointAtTime(Vector2 firePoint, WeaponDefinition weapon, float time)
        {
            var start = firePoint + (_aimState.Direction * ProjectileMuzzleOffset);
            var velocity = _aimState.Direction * _aimState.Power;
            var gravity = Physics2D.gravity * weapon.GravityScale;
            return start + velocity * time + 0.5f * gravity * (time * time);
        }

        private float CalculateTrajectoryArcLength(Vector2 firePoint, WeaponDefinition weapon, float endTime)
        {
            if (!float.IsFinite(endTime) || endTime <= 0f)
            {
                return 0f;
            }

            const int sampleCount = 48;
            var length = 0f;
            var previous = GetTrajectoryPreviewPointAtTime(firePoint, weapon, 0f);
            for (var i = 1; i <= sampleCount; i++)
            {
                var current = GetTrajectoryPreviewPointAtTime(firePoint, weapon, endTime * i / sampleCount);
                length += Vector2.Distance(previous, current);
                previous = current;
            }

            return length;
        }

        private void DrawCoverPlacementPreview(Vector2 firePoint)
        {
            HideTrajectoryDots();
            if (_inventory.Selected?.WeaponId == "weapon.titanium-barrier")
            {
                _trajectoryLine.enabled = false;
                DrawTitaniumBarrierPlacementPreview(GetCoverPlacementPoint(firePoint));
                return;
            }

            _trajectoryLine.enabled = true;
            var placementColor = new Color(0.35f, 0.85f, 1f, 0.65f);
            _trajectoryLine.startColor = placementColor;
            _trajectoryLine.endColor = placementColor;
            _trajectoryLine.startWidth = 0.035f;
            _trajectoryLine.endWidth = 0.035f;
            _trajectoryLine.positionCount = 2;
            _trajectoryLine.SetPosition(0, firePoint);
            _trajectoryLine.SetPosition(1, GetCoverPlacementPoint(firePoint));
        }

        private bool IsTitaniumBarrierPlacementActive()
        {
            return _playerWeaponSelected
                && _inventory?.Selected?.WeaponId == "weapon.titanium-barrier"
                && _simulation != null
                && _battleState.Turn.ActiveTeam == BattleTeamId.Player
                && !_simulation.IsAttackCommitted;
        }

        private void UpdateTitaniumBarrierPlacement()
        {
            if (!IsTitaniumBarrierPlacementActive())
            {
                HideTrajectoryPreview();
                return;
            }
            var presenter = GetActivePresenter();
            if (presenter == null)
            {
                return;
            }

            if (!_barrierPlacementInitialized)
            {
                var facing = presenter.State.FacingDirectionX >= 0 ? 1f : -1f;
                var point = (Vector2)presenter.View.FirePoint.position + new Vector2(facing * 0.58f, 1.02f);
                for (var attempt = 0; attempt < 5 && _battleTerrain?.Grid.IsSolidWorld(point) == true; attempt++)
                {
                    point += Vector2.up * 0.35f;
                }

                SetTitaniumBarrierPlacement(point, Vector2.right);
                _barrierPlacementInitialized = true;
            }

            var pointNow = GetCoverPlacementPoint(_aimState.Origin);
            var move = new Vector2(_controlledUiHorizontalInput, -_ropeVerticalInput);
            if (move.sqrMagnitude > 0.01f)
            {
                var candidate = pointNow + move.normalized * (1.65f * Time.deltaTime);
                if (_battleTerrain?.Grid.IsSolidWorld(candidate) != true)
                {
                    SetTitaniumBarrierPlacement(candidate, _aimState.Direction);
                    pointNow = candidate;
                }
            }

            // Two touches own resizing; neither finger may rotate the same preview.
            SetTitaniumBarrierPlacement(pointNow, _aimState.Direction);
            if (TryGetPinchDistance(out _)) return;
            if (!TryReadPointer(out var pressed, out _, out _, out var screenPosition, out var pointerOverUi)
                || !pressed || pointerOverUi)
            {
                return;
            }

            var direction = ScreenToWorld(screenPosition) - pointNow;
            if (direction.sqrMagnitude > 0.04f)
            {
                SetTitaniumBarrierPlacement(pointNow, direction.normalized);
            }
        }

        private void SetTitaniumBarrierPlacement(Vector2 point, Vector2 direction)
        {
            var normalizedDirection = direction.sqrMagnitude > 0.001f ? direction.normalized : Vector2.right;
            _aimState = new AimState(
                point,
                normalizedDirection,
                _barrierPlacementScale,
                true);
            DrawTitaniumBarrierPlacementPreview(point);
        }

        private void HideTrajectoryPreview()
        {
            _trajectoryLine.enabled = false;
            HideTrajectoryDots();
            if (_barrierPlacementPreview != null)
            {
                _barrierPlacementPreview.SetActive(false);
            }
        }

        private void DrawTitaniumBarrierPlacementPreview(Vector2 point)
        {
            if (_barrierPlacementPreview == null)
            {
                _barrierPlacementPreview = new GameObject("Titanium Beam Placement Preview");
                _arenaObjects.Add(_barrierPlacementPreview);
                var renderer = _barrierPlacementPreview.AddComponent<SpriteRenderer>();
                renderer.sprite = LoadWeaponIconSprite("titanium_barrier");
                renderer.color = Color.white;
                renderer.sortingOrder = 29;
            }

            _barrierPlacementPreview.SetActive(true);
            _barrierPlacementPreview.transform.position = point;
            var previewSprite = _barrierPlacementPreview.GetComponent<SpriteRenderer>().sprite;
            var size = _simulation.TitaniumBarrierBaseSize * _barrierPlacementScale;
            _barrierPlacementPreview.transform.localScale = new Vector3(size.x / previewSprite.bounds.size.x,
                size.y / previewSprite.bounds.size.y, 1f);
            _barrierPlacementPreview.transform.rotation = Quaternion.Euler(
                0f,
                0f,
                Mathf.Atan2(_aimState.Direction.y, _aimState.Direction.x) * Mathf.Rad2Deg);
        }

        private void HideTrajectoryDots()
        {
            for (var i = 0; i < _trajectoryDots.Count; i++)
            {
                _trajectoryDots[i].enabled = false;
            }
        }

        private Vector2 GetCoverPlacementPoint(Vector2 firePoint)
        {
            if (_inventory?.Selected?.WeaponId == TitaniumBarrierRules.WeaponId) return _aimState.Origin;
            var power01 = Mathf.InverseLerp(_aimSettings.MinPower, _aimSettings.MaxPower, _aimState.Power);
            var placementDistance = Mathf.Lerp(1.25f, 3.1f, power01);
            return firePoint + _aimState.Direction.normalized * placementDistance;
        }

        private void SpawnImpactFlash(Vector2 position)
        {
            SpawnImpactFlash(position, new Color(1f, 0.74f, 0.18f, 0.85f), 1.2f);
        }

        private void SpawnImpactFlash(Vector2 position, Color color, float scale)
        {
            var flash = new GameObject("Impact Flash");
            _arenaObjects.Add(flash);
            flash.transform.position = new Vector3(position.x, position.y, -0.2f);
            flash.transform.localScale = Vector3.one * scale;

            var renderer = flash.AddComponent<SpriteRenderer>();
            renderer.sprite = _circleSprite;
            renderer.color = color;
            renderer.sortingOrder = 40;

            Destroy(flash, 0.25f);
        }

        private GameObject CreateBox(string objectName, Vector2 position, Vector2 size, Color color, int sortingOrder, bool visible = true)
        {
            var box = new GameObject(objectName);
            box.transform.position = position;
            box.transform.localScale = size;

            if (visible)
            {
                var renderer = box.AddComponent<SpriteRenderer>();
                renderer.sprite = _boxSprite;
                renderer.color = color;
                renderer.sortingOrder = sortingOrder;
            }

            return box;
        }

        private SpriteRenderer AddVisualLayer(string objectName, BattleVisualLayer? layer, SpriteRenderer currentSolidLayer, bool useBackgroundWorldOffset = false)
        {
            return AddVisualLayer(objectName, layer, currentSolidLayer, out _, useBackgroundWorldOffset);
        }

        private SpriteRenderer AddVisualLayer(
            string objectName,
            BattleVisualLayer? layer,
            SpriteRenderer currentSolidLayer,
            out SpriteRenderer addedRenderer,
            bool useBackgroundWorldOffset = false)
        {
            addedRenderer = null;
            if (layer == null || !layer.Value.HasSprite)
            {
                return currentSolidLayer;
            }

            var sprite = LoadSpriteResource(layer.Value.ResourcePath);
            if (sprite == null)
            {
                return currentSolidLayer;
            }

            var visual = new GameObject(objectName);
            var position = layer.Value.CoverCamera && _camera != null
                ? (Vector2)_camera.transform.position + layer.Value.Position
                : layer.Value.Position;
            var placementOffset = useBackgroundWorldOffset ? _backgroundWorldOffset : _arenaWorldOffset;
            visual.transform.position = new Vector3(
                position.x + placementOffset.x,
                position.y + placementOffset.y,
                0f);
            visual.transform.localScale = new Vector3(layer.Value.Scale.x, layer.Value.Scale.y, 1f);

            var renderer = visual.AddComponent<SpriteRenderer>();
            renderer.sprite = sprite;
            renderer.sortingOrder = layer.Value.SortingOrder;
            addedRenderer = renderer;

            if (layer.Value.CoverCamera)
            {
                ScaleSpriteToCoverCamera(visual.transform, sprite);
                _cameraCoverVisuals.Add((visual.transform, layer.Value.Position));
            }

            _arenaObjects.Add(visual);
            return layer.Value.Solid ? renderer : currentSolidLayer;
        }

        private Vector2 TransformArenaPosition(Vector2 position)
        {
            return _arenaScalePivot
                + Vector2.Scale(position - _arenaScalePivot, _arenaScale)
                + _arenaWorldOffset;
        }

        private ArenaTerrainBlock TransformArenaBlock(ArenaTerrainBlock block)
        {
            return new ArenaTerrainBlock(
                block.BlockId,
                TransformArenaPosition(block.Position),
                Vector2.Scale(block.Size, _arenaScale),
                block.Color,
                block.Solid,
                block.SortingOrder,
                block.Material);
        }

        private static Rect ExpandArenaBoundsToIncludeSpriteSurfaces(
            Rect bounds,
            IReadOnlyList<BattleTerrainSpriteSurface> surfaces)
        {
            for (var i = 0; i < surfaces.Count; i++)
            {
                var renderer = surfaces[i].Renderer;
                if (renderer == null || renderer.sprite == null)
                {
                    continue;
                }

                var surfaceBounds = renderer.bounds;
                bounds.xMin = Mathf.Min(bounds.xMin, surfaceBounds.min.x);
                bounds.xMax = Mathf.Max(bounds.xMax, surfaceBounds.max.x);
                bounds.yMin = Mathf.Min(bounds.yMin, surfaceBounds.min.y);
                bounds.yMax = Mathf.Max(bounds.yMax, surfaceBounds.max.y);
            }

            return bounds;
        }

        private void FitStaticBackgroundToCameraFrame(SpriteRenderer renderer)
        {
            if (renderer == null || renderer.sprite == null || _cameraPanBounds.width <= 0f || _cameraPanBounds.height <= 0f)
            {
                return;
            }

            var spriteSize = renderer.sprite.bounds.size;
            if (spriteSize.x <= 0f || spriteSize.y <= 0f)
            {
                return;
            }

            renderer.transform.position = new Vector3(
                _cameraPanBounds.center.x + _backgroundWorldOffset.x,
                _cameraPanBounds.center.y + _backgroundWorldOffset.y,
                0f);
            renderer.transform.localScale = new Vector3(
                _cameraPanBounds.width / spriteSize.x,
                _cameraPanBounds.height / spriteSize.y,
                1f);
        }

        private BattleTerrainSpriteSurface AddArenaSpriteSurface(ArenaSpriteSurface surface)
        {
            var sprite = LoadSpriteResource(surface.VisualResourcePath);
            var mask = Resources.Load<Texture2D>(surface.MaskResourcePath);
            if (sprite == null || mask == null)
            {
                Debug.LogWarning(
                    $"Arena surface '{surface.InstanceId}' is missing visual or mask resource " +
                    $"('{surface.VisualResourcePath}', '{surface.MaskResourcePath}').");
                return default;
            }

            var visual = new GameObject(surface.InstanceId);
            var position = TransformArenaPosition(surface.Position);
            visual.transform.position = new Vector3(position.x, position.y, 0f);
            var seamOverdraw = CalculateArenaSpriteSeamOverdraw(sprite.rect.size);
            visual.transform.localScale = new Vector3(
                surface.Scale * _arenaScale.x * seamOverdraw.x,
                surface.Scale * _arenaScale.y * seamOverdraw.y,
                1f);
            var renderer = visual.AddComponent<SpriteRenderer>();
            renderer.sprite = sprite;
            renderer.sortingOrder = surface.SortingOrder;
            _arenaObjects.Add(visual);
            return new BattleTerrainSpriteSurface(renderer, mask, surface.HasHorizontalMotion);
        }

        private static Vector2 CalculateArenaSpriteSeamOverdraw(Vector2 spritePixels)
        {
            return new Vector2(
                1f + ArenaSpriteSeamOverdrawPixels * 2f / Mathf.Max(1f, spritePixels.x),
                1f + ArenaSpriteSeamOverdrawPixels * 2f / Mathf.Max(1f, spritePixels.y));
        }

        private void AddBackgroundPrefab()
        {
            if (_visualTheme == null || !_visualTheme.HasBackgroundPrefab)
            {
                return;
            }

            var prefab = Resources.Load<GameObject>(_visualTheme.BackgroundPrefabResourcePath);
            if (prefab == null)
            {
                Debug.LogWarning($"Background prefab '{_visualTheme.BackgroundPrefabResourcePath}' was not found.");
                return;
            }

            var background = Instantiate(prefab);
            background.name = "Arena Animated Background";
            var skyBackground = background.GetComponent<AnimatedSkyBackground>();
            if (skyBackground != null)
            {
                _animatedBackground = skyBackground;
                skyBackground.ConfigureCoverage(_cameraPanBounds, _backgroundWorldOffset, 0f);
            }

            _arenaObjects.Add(background);
        }

        private void AddKillZoneVisual()
        {
            var sprite = LoadSpriteResource(KillZoneResourcePath);
            if (sprite == null || sprite.bounds.size.x <= 0f)
            {
                return;
            }

            var visual = new GameObject("Arena Kill Zone");
            _killZoneVisual = visual.transform;
            var renderer = visual.AddComponent<SpriteRenderer>();
            renderer.sprite = sprite;
            renderer.sortingOrder = 45;
            UpdateKillZoneVisual();
            _arenaObjects.Add(visual);
        }

        private void UpdateKillZoneVisual()
        {
            if (_killZoneVisual == null || !_killZoneVisual.TryGetComponent<SpriteRenderer>(out var renderer)
                || renderer.sprite == null || renderer.sprite.bounds.size.x <= 0f)
            {
                return;
            }

            _killZoneVisual.position = new Vector3(
                _cameraWorldBounds.center.x,
                _cameraWorldBounds.yMin + KillZoneBottomInset + _arenaWorldOffset.y + _killZoneOffsetY,
                0f);
            _killZoneVisual.localScale = new Vector3(
                _cameraWorldBounds.width / renderer.sprite.bounds.size.x,
                KillZoneVisualHeightScale,
                1f);
        }

        private void ScaleSpriteToCoverCamera(Transform visual, Sprite sprite)
        {
            if (_camera == null || visual == null || sprite == null)
            {
                return;
            }

            var spriteSize = sprite.bounds.size;
            if (spriteSize.x <= 0f || spriteSize.y <= 0f)
            {
                return;
            }

            var cameraHeight = Mathf.Max(_camera.orthographicSize, _maximumCameraSize) * 2f;
            var cameraWidth = cameraHeight * _camera.aspect;
            var scale = Mathf.Max(cameraWidth / spriteSize.x, cameraHeight / spriteSize.y);
            visual.localScale = new Vector3(scale, scale, 1f);
        }

        private void UpdateCameraCoverVisuals()
        {
            if (_camera == null)
            {
                return;
            }

            var cameraPosition = (Vector2)_camera.transform.position;
            for (var i = _cameraCoverVisuals.Count - 1; i >= 0; i--)
            {
                var entry = _cameraCoverVisuals[i];
                if (entry.Transform == null)
                {
                    _cameraCoverVisuals.RemoveAt(i);
                    continue;
                }

                var position = cameraPosition + entry.Offset;
                entry.Transform.position = new Vector3(position.x, position.y, entry.Transform.position.z);
            }
        }

        private void UpdateMovingArenaSurfaces()
        {
            if (_battleTerrain == null || _movingArenaSurfaces.Count == 0
                || _simulation == null || _simulation.Result.IsFinished)
            {
                return;
            }

            var moved = false;
            for (var i = 0; i < _movingArenaSurfaces.Count; i++)
            {
                var surface = _movingArenaSurfaces[i];
                if (surface?.Transform == null)
                {
                    continue;
                }

                surface.Elapsed += Time.deltaTime;
                var position = surface.Origin;
                position.x += Mathf.Sin(surface.Elapsed * surface.Speed) * surface.Range;
                if ((surface.Transform.position - position).sqrMagnitude > 0.000001f)
                {
                    surface.Transform.position = position;
                    moved = true;
                }
            }

            if (moved)
            {
                _movingArenaTerrainRefreshTimer -= Time.deltaTime;
                if (_movingArenaTerrainRefreshTimer <= 0f)
                {
                    // The sprite itself stays smooth at render-frame rate. The
                    // collision raster is expensive, so it only needs to follow
                    // the moving platform at a fixed 10 Hz rather than once per
                    // frame.
                    _battleTerrain.RefreshMovingSpriteSurfaces();
                    _movingArenaTerrainRefreshTimer = MovingArenaTerrainRefreshInterval;
                }
            }
        }

        private Sprite GetUnitSkin(BattleTeamId teamId)
        {
            if (_visualTheme == null)
            {
                return null;
            }

            var resourcePath = teamId == BattleTeamId.Player
                ? _visualTheme.PlayerSkinResourcePath
                : _visualTheme.OpponentSkinResourcePath;
            return LoadSpriteResource(resourcePath);
        }

        private GameObject GetUnitRigPrefab(BattleTeamId teamId)
        {
            if (_visualTheme == null)
            {
                return null;
            }

            var resourcePath = teamId == BattleTeamId.Player
                ? _visualTheme.PlayerRigResourcePath
                : _visualTheme.OpponentRigResourcePath;
            if (string.IsNullOrWhiteSpace(resourcePath))
            {
                return null;
            }

            return Resources.Load<GameObject>(resourcePath);
        }

        private Sprite LoadSpriteResource(string resourcePath)
        {
            if (string.IsNullOrWhiteSpace(resourcePath))
            {
                return null;
            }

            if (_spriteResourceCache.TryGetValue(resourcePath, out var cached))
            {
                return cached;
            }

            var sprite = Resources.Load<Sprite>(resourcePath);
            if (sprite == null)
            {
                var texture = Resources.Load<Texture2D>(resourcePath);
                if (texture != null)
                {
                    sprite = Sprite.Create(
                        texture,
                        new Rect(0f, 0f, texture.width, texture.height),
                        new Vector2(0.5f, 0.5f),
                        100f);
                }
            }

            if (sprite != null)
            {
                _spriteResourceCache.Add(resourcePath, sprite);
            }

            return sprite;
        }

        private GameObject _turnTransferMarker;

        /// <summary>The turn transfer art above the ally chosen to receive the turn, while the choice is open.</summary>
        private void UpdateTurnTransferMarker()
        {
            var targetId = _simulation != null ? _simulation.TurnTransferTargetUnitId : string.Empty;
            var target = string.IsNullOrEmpty(targetId)
                ? null
                : _unitPresenters.FirstOrDefault(presenter => presenter.State.UnitId == targetId);
            if (target == null)
            {
                if (_turnTransferMarker != null)
                {
                    _turnTransferMarker.SetActive(false);
                }

                return;
            }

            if (_turnTransferMarker == null)
            {
                var sprite = LoadWeaponIconSprite("turn_transfer");
                if (sprite == null)
                {
                    return;
                }

                _turnTransferMarker = new GameObject("Turn Transfer Marker");
                var renderer = _turnTransferMarker.AddComponent<SpriteRenderer>();
                renderer.sprite = sprite;
                renderer.sortingOrder = 35;
                _turnTransferMarker.transform.localScale = Vector3.one * (TurnTransferMarkerWorldWidth / Mathf.Max(0.001f, sprite.bounds.size.x));
                _arenaObjects.Add(_turnTransferMarker);
            }

            _turnTransferMarker.SetActive(true);
            _turnTransferMarker.transform.position = new Vector3(target.State.Position.x, target.State.Position.y + 1.05f, 0f);
        }

        private const float TurnTransferMarkerWorldWidth = 0.6f;

        private Sprite LoadWeaponIconSprite(string iconId)
        {
            if (string.IsNullOrWhiteSpace(iconId))
            {
                return null;
            }

            var resourcePath = $"UI/Weapons/{iconId}";
            if (_spriteResourceCache.TryGetValue(resourcePath, out var cached))
            {
                return cached;
            }

            var texture = Resources.Load<Texture2D>(resourcePath);
            if (texture == null)
            {
                return null;
            }

            var sprite = Sprite.Create(
                texture,
                new Rect(0f, 0f, texture.width, texture.height),
                new Vector2(0.5f, 0.5f),
                100f,
                0,
                SpriteMeshType.FullRect);
            sprite.name = $"{iconId} Battle Mount";
            _spriteResourceCache.Add(resourcePath, sprite);
            return sprite;
        }

        private Sprite LoadCenteredTextureSprite(string resourcePath, string spriteName)
        {
            if (string.IsNullOrWhiteSpace(resourcePath))
            {
                return null;
            }

            var cacheKey = resourcePath + "#center-pivot";
            if (_spriteResourceCache.TryGetValue(cacheKey, out var cached))
            {
                return cached;
            }

            var texture = Resources.Load<Texture2D>(resourcePath);
            if (texture == null)
            {
                return null;
            }

            var sprite = Sprite.Create(
                texture,
                new Rect(0f, 0f, texture.width, texture.height),
                new Vector2(0.5f, 0.5f),
                100f,
                0,
                SpriteMeshType.FullRect);
            sprite.name = spriteName;
            _spriteResourceCache.Add(cacheKey, sprite);
            return sprite;
        }

        private Sprite LoadTopPivotTextureSprite(string resourcePath, string spriteName)
        {
            if (string.IsNullOrWhiteSpace(resourcePath))
            {
                return null;
            }

            var cacheKey = resourcePath + "#top-pivot";
            if (_spriteResourceCache.TryGetValue(cacheKey, out var cached))
            {
                return cached;
            }

            var texture = Resources.Load<Texture2D>(resourcePath);
            if (texture == null)
            {
                return null;
            }

            var sprite = Sprite.Create(
                texture,
                new Rect(0f, 0f, texture.width, texture.height),
                new Vector2(0.5f, 1f),
                100f,
                0,
                // The imported texture already has alpha. A tight mesh prevents
                // transparent canvas pixels from being submitted by the renderer,
                // which was the source of the black rectangular beam in-game.
                SpriteMeshType.Tight);
            sprite.name = spriteName;
            _spriteResourceCache.Add(cacheKey, sprite);
            return sprite;
        }

        private Sprite GetTopPivotSolidSprite()
        {
            if (_topPivotSolidSprite != null)
            {
                return _topPivotSolidSprite;
            }

            _topPivotSolidSprite = Sprite.Create(
                Texture2D.whiteTexture,
                new Rect(0f, 0f, 1f, 1f),
                new Vector2(0.5f, 1f),
                1f,
                0,
                SpriteMeshType.FullRect);
            _topPivotSolidSprite.name = "Support Drop Beam Core";
            return _topPivotSolidSprite;
        }

        private void RefreshPlayerWeaponVisual()
        {
            var presenter = GetPresenter(BattleTeamId.Player);
            if (presenter == null)
            {
                return;
            }

            if (!_playerWeaponSelected || _inventory?.Selected == null || _hideHeldWeaponForReusableThrow
                || BattleSimulation.IsMarkedStrikeWeapon(_inventory.Selected)
                || IsPlayerTurnShotSpent())
            {
                presenter.ClearEquippedWeapon();
                return;
            }

            var weapon = _inventory.Selected;
            if (weapon.WeaponId == "weapon.titanium-barrier")
            {
                presenter.ClearEquippedWeapon();
                return;
            }
            var presentation = ResolveHeldWeaponPresentation(weapon.WeaponId);
            presenter.SetWeaponPresentation(
                presentation.Scale,
                presentation.Position,
                presentation.AimAngleOffset);
            var heldIconId = ResolveHudIconId(weapon);
            if (weapon.WeaponId == "weapon.electric-harpoon" && _simulation != null
                && _simulation.HasPlayerElectricHarpoon)
            {
                heldIconId = heldIconId.EndsWith("_5A", System.StringComparison.Ordinal)
                    ? $"{heldIconId}_shot"
                    : "electric_harpoon_shot";
            }
            presenter.SetEquippedWeapon(
                LoadWeaponIconSprite(heldIconId),
                WeaponIconFacesRight(weapon.IconId));
            var direction = _aimState.HasInput
                ? _aimState.Direction
                : RestingWeaponDirection(weapon.WeaponId, presenter.State.FacingDirectionX);
            presenter.SetWeaponAim(direction);
        }

        /// <summary>
        /// Barrel direction while no aim is committed: heavy tube weapons rest
        /// slightly tilted toward the ground; everything else sits horizontal.
        /// </summary>
        private static Vector2 RestingWeaponDirection(string weaponId, int facingDirectionX)
        {
            var facing = facingDirectionX >= 0 ? 1f : -1f;
            if (!HasRestingTilt(weaponId))
            {
                return Vector2.right * facing;
            }

            var radians = RestingWeaponTiltDegrees * Mathf.Deg2Rad;
            return new Vector2(facing * Mathf.Cos(radians), -Mathf.Sin(radians));
        }

        private static bool HasRestingTilt(string weaponId)
        {
            return weaponId == "weapon.bur"
                || weaponId == "weapon.mortira"
                || weaponId == "weapon.toxic"
                || weaponId == "weapon.freeze";
        }

        private static (Vector2 Scale, Vector2 Position, float AimAngleOffset) ResolveHeldWeaponPresentation(string weaponId)
        {
            return weaponId switch
            {
                "weapon.impulse" => (Vector2.one, new Vector2(-0.48f, 0.27f), 0f),
                "weapon.blaster-caset" => (Vector2.one, new Vector2(-0.56f, 0.34f), 0f),
                // Positive offsets counter the art's built-in downward barrel
                // tilt so the barrel axis matches the aim/trajectory exactly.
                "weapon.bur" => (Vector2.one * 1.5f, new Vector2(-0.54f, 0.42f), 15f),
                "weapon.mortira" => (Vector2.one * 1.12f, new Vector2(-0.55f, 0.40f), 7f),
                "weapon.granata" => (Vector2.one * 0.76f, new Vector2(-0.50f, 0.29f), 0f),
                "weapon.sticky-grenade" => (Vector2.one * 0.70f, new Vector2(-0.50f, 0.29f), 0f),
                "weapon.hopping-grenade" => (Vector2.one * 0.70f, new Vector2(-0.50f, 0.29f), 0f),
                "weapon.cryo-grenade" => (Vector2.one * 0.70f, new Vector2(-0.50f, 0.29f), 0f),
                "weapon.incendiary-grenade" => (Vector2.one * 0.70f, new Vector2(-0.50f, 0.29f), 0f),
                "weapon.teleport" => (Vector2.one * 0.82f, new Vector2(-0.60f, 0.40f), 0f),
                "weapon.mine" => (Vector2.one * 0.82f, new Vector2(-0.58f, 0.36f), 0f),
                "weapon.toxic" => (Vector2.one, new Vector2(-0.68f, 0.42f), 11f),
                "weapon.emp" => (Vector2.one, new Vector2(-0.55f, 0.42f), 0f),
                "weapon.freeze" => (Vector2.one, new Vector2(-0.55f, 0.42f), 10f),
                "weapon.fire-gun" => (Vector2.one * 1.5f, new Vector2(-0.68f, 0.42f), 0f),
                "weapon.missle" => (Vector2.one * 1.5f, new Vector2(-0.54f, 0.30f), 0f),
                "weapon.plasma-sword" => (Vector2.one * 2f, new Vector2(-0.56f, 0.45f), 0f),
                "weapon.rail-rifle" => (new Vector2(1f, 1.5f), new Vector2(-0.62f, 0.39f), 0f),
                "weapon.heal" => (Vector2.one, new Vector2(-0.54f, 0.38f), 180f),
                "weapon.fpv-drone" => (Vector2.one, new Vector2(-0.28f, 1.08f), 0f),
                _ => (Vector2.one, new Vector2(-0.68f, 0.42f), 0f)
            };
        }

        private static bool WeaponIconFacesRight(string iconId)
        {
            // The drill ("bur") art has its bit on the right - mirroring it made
            // the barrel point backwards against the aim line.
            return iconId != "bazooka_simple"
                && iconId != "bazooka"
                && iconId != "drobovik";
        }

        private LineRenderer CreateLine(string objectName, float width, Color color)
        {
            var lineObject = new GameObject(objectName);
            var line = lineObject.AddComponent<LineRenderer>();
            line.material = new Material(Shader.Find("Sprites/Default"));
            line.startColor = color;
            line.endColor = color;
            line.startWidth = width;
            line.endWidth = width;
            line.positionCount = 2;
            line.useWorldSpace = true;
            line.sortingOrder = 50;
            line.enabled = false;
            return line;
        }

        private void BuildSprites()
        {
            _boxSprite = Sprite.Create(Texture2D.whiteTexture, new Rect(0f, 0f, 1f, 1f), new Vector2(0.5f, 0.5f), 1f);
            _circleSprite = CreateCircleSprite(64);
        }

        private static Sprite CreateCircleSprite(int size)
        {
            var texture = new Texture2D(size, size, TextureFormat.RGBA32, false);
            texture.filterMode = FilterMode.Bilinear;
            var pixels = new Color[size * size];
            var center = (size - 1) * 0.5f;
            var radius = center - 1f;

            for (var y = 0; y < size; y++)
            {
                for (var x = 0; x < size; x++)
                {
                    var dx = x - center;
                    var dy = y - center;
                    var distance = Mathf.Sqrt(dx * dx + dy * dy);
                    var alpha = Mathf.Clamp01(radius + 0.5f - distance);
                    pixels[y * size + x] = new Color(1f, 1f, 1f, alpha);
                }
            }

            texture.SetPixels(pixels);
            texture.Apply();
            return Sprite.Create(texture, new Rect(0f, 0f, size, size), new Vector2(0.5f, 0.5f), size);
        }

        private void RestartScene()
        {
            // Go through the loading bridge like every other transition, so a retry shows
            // the loading screen instead of freezing on the result card.
            if (!SceneLoadingBridge.TryLoad(GameSceneNames.Battle))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }

        private void ContinueFromResult()
        {
            ExitToMenu();
        }

        private void ExitToMenu()
        {
            if (IsBattleReady() && _simulation.Result.IsFinished)
            {
                PrepareResultReturn();
            }

            LoadMenuScene();
        }

        private void PrepareResultReturn()
        {
            var mode = GameSessionContext.SelectedBattleMode;
            if (mode == BattleMode.Story
                && _simulation.Result.Outcome == BattleOutcome.PlayerWon)
            {
                GameSessionContext.ReportMissionCompleted();
                GameSessionContext.RequestCampaignMissionView(GameSessionContext.SelectedMissionId);
                return;
            }

            // Repeatable modes return to their own tab; training has no dedicated
            // tab and therefore returns to the map section.
            GameSessionContext.RequestBattleModeView(mode);
        }

        private static void LoadMenuScene()
        {
            if (!SceneLoadingBridge.TryLoad(GameSceneNames.Menu))
            {
                SceneManager.LoadScene(GameSceneNames.Menu);
            }
        }

        private void CancelAim()
        {
            if (!IsBattleReady() || _simulation.Result.IsFinished || HasProjectilesInFlight || HasActiveMotion || HasActiveWeaponEffect)
            {
                return;
            }

            ResetAimState();
            _status = GameText.AimReset();
        }

        private void ResetAimState()
        {
            _aimState = new AimState(Vector2.zero, Vector2.right, _aimSettings.MinPower, false);
            _aimDragActive = false;
            _aimDragHadValidPull = false;
            _aimHadInput = false;
            if (_aimLine != null)
            {
                _aimLine.enabled = false;
            }

            HideTrajectoryPreview();
            RefreshPlayerWeaponVisual();
        }

        private void RenderHud()
        {
            UpdateTurnTransferMarker();
            if (_hud == null || !IsBattleReady())
            {
                return;
            }

            var weapon = _inventory.Selected;
            var playerHasTime = _simulation.MovementTimeRemaining > 0f;
            var ropeAttached = _simulation.IsPlayerRopeAttached || _simulation.HasPlayerBoost;
            var directDroneControlActive = _simulation.HasActiveControlledProjectile;
            var fireInputReady = ropeAttached || !BattleSimulation.RequiresAim(weapon) || _aimState.HasInput;
            var bomberActive = _simulation.HasPlayerCopter;
            var canContinueMinePlacement = CanContinueMinePlacementInput();
            var canFire = !_simulation.Result.IsFinished
                && (ropeAttached || bomberActive || (!HasActiveMotion && !HasActiveWeaponEffect
                    && (!HasProjectilesInFlight || canContinueMinePlacement)))
                && !HasPendingGroundAction
                && playerHasTime
                && _playerWeaponSelected
                && (_inventory.CanFireSelected || _simulation.CanContinueHoppingGrenade()
                    || _simulation.CanTriggerTeleportEarly() || _simulation.HasPlayerBoost
                    || _simulation.HasPlayerCopter)
                && fireInputReady;
            var guideActive = _firstBattleGuide != null && _firstBattleGuide.IsActive;
            var isFirstTrainingBattle = FirstBattleGuideState.IsFirstMission(GameSessionContext.SelectedMissionId);
            // Instruction cards belong to the first training battle only. The old
            // condition inverted that and showed them in every later mission instead.
            var showTutorialCard = !GameSessionContext.IsTrainingSkirmish
                && isFirstTrainingBattle
                && (guideActive || _coachTimer > 0f);
            var status = guideActive
                ? _firstBattleGuide.Message
                : _coachTimer > 0f && !string.IsNullOrEmpty(_coachMessage) ? _coachMessage : _status;
            var tutorialTargetId = guideActive
                ? _firstBattleGuide.TargetId
                : _coachTimer > 0f ? _coachTargetId : string.Empty;
            var canUseMovement = IsPlayerControlTurn(_battleState.Turn.ActiveTeam)
                && _simulation.CanAcceptMovementInput()
                && (!guideActive || _firstBattleGuide.AllowsWalkDirection(-1) || _firstBattleGuide.AllowsWalkDirection(1)
                    || _firstBattleGuide.AllowsJumpDirection(-1) || _firstBattleGuide.AllowsJumpDirection(1));
            var mobilityLabel = GameText.MobilityLabel(_simulation.MovementTimeRemaining);
            var opponentWeapon = _opponentInventory?.Selected;
            var canOpenArsenal = !_simulation.Result.IsFinished
                && _battleState.Turn.ActiveTeam == BattleTeamId.Player
                && !HasProjectilesInFlight
                && !HasActiveMotion
                && !HasActiveWeaponEffect
                && !HasPendingGroundAction
                && !_simulation.IsAttackCommitted
                && playerHasTime
                && (!guideActive || _firstBattleGuide.AllowsArsenal);

            var state = new BattleHudState(
                status,
                _battleState.Turn.TurnNumber,
                DescribeTeam(_battleState.Turn.ActiveTeam),
                _battleState.Turn.ActiveTeam == BattleTeamId.Player ? _simulation.MovementTimeRemaining : 0f,
                !guideActive || _firstBattleGuide.ShowTurnTimer,
                GetTeamHealth(BattleTeamId.Player),
                GetTeamMaxHealth(BattleTeamId.Player),
                GetTeamHealth(BattleTeamId.Opponent),
                GetTeamMaxHealth(BattleTeamId.Opponent),
                GameText.WeaponSlot(DescribeWeapon(weapon), _inventory.SelectedIndex + 1, _inventory.Count),
                weapon != null ? ResolveHudIconId(weapon) : string.Empty,
                WeaponPresentationText.DescribeRole(weapon),
                DescribeAmmo(_inventory),
                WeaponPresentationText.DescribeBattleHint(weapon),
                mobilityLabel,
                _arsenalHudItems,
                _playerWeaponSelected ? _inventory.SelectedIndex : -1,
                _playerWeaponSelected,
                _playerWeaponSelected && fireInputReady,
                canOpenArsenal,
                tutorialTargetId,
                showTutorialCard,
                guideActive && _firstBattleGuide.BlocksWeaponSwitch,
                guideActive && _firstBattleGuide.BlocksCancelAim,
                guideActive && !(_firstBattleGuide.AllowsWalkDirection(-1) || _firstBattleGuide.AllowsWalkDirection(1)),
                guideActive && isFirstTrainingBattle,
                _aimState.Power,
                _aimSettings.MinPower,
                _aimSettings.MaxPower,
                canFire && (!guideActive || _firstBattleGuide.AllowsFire),
                canUseMovement,
                _simulation.Result.IsFinished,
                _resultScreenState.Title,
                _resultScreenState.Subtitle,
                _simulation.Result.IsFinished ? DescribeOutcome(_resultScreenState.Result.Outcome) : string.Empty,
                _simulation.Result.IsFinished ? DescribeResultReward(_resultScreenState.RewardPreview) : string.Empty,
                _simulation.Result.IsFinished && _resultScreenState.RewardPreview.HasUnlock ? _resultScreenState.RewardPreview.UnlockText : GameText.NoUnlocks,
                _simulation.Result.IsFinished && _resultScreenState.CanContinue,
                _simulation.Result.IsFinished ? _resultScreenState.RetryLabel : GameText.Restart,
                _resultScreenState.CanContinue ? _resultScreenState.ContinueLabel : GameText.Menu,
                _simulation.Result.Outcome == BattleOutcome.PlayerWon,
                GameSessionContext.SelectedMissionText,
                // Only a won battle banks the mission - a loss leaves it outstanding even
                // when its objective was met, matching what the campaign map will show.
                _simulation.Result.Outcome == BattleOutcome.PlayerWon,
                ResolveOpponentName(),
                DescribeWeapon(opponentWeapon),
                opponentWeapon != null ? _opponentProgression.ResolveRank(opponentWeapon.WeaponId) : 0,
                ropeAttached || directDroneControlActive || IsTitaniumBarrierPlacementActive(),
                ResolveControlledBombsForHud(),
                ResolveControlledSecondsForHud());

            _hud.Render(state);
        }

        /// <summary>Bomb counter of the player's own copter; hidden (-1) otherwise.</summary>
        private int ResolveControlledBombsForHud()
        {
            return _simulation != null && _battleState.Turn.ActiveTeam == BattleTeamId.Player
                && _simulation.HasPlayerBomberCopter
                    ? _simulation.BomberBombsRemaining
                    : -1;
        }

        /// <summary>Flight seconds of the player's copter or rocket fuel; hidden (-1) otherwise.</summary>
        private float ResolveControlledSecondsForHud()
        {
            if (_simulation == null || _battleState.Turn.ActiveTeam != BattleTeamId.Player)
            {
                return -1f;
            }

            if (_simulation.HasPlayerCopter)
            {
                return _simulation.CopterSecondsRemaining;
            }

            return _simulation.HasPlayerBoost ? _simulation.BoostSecondsRemaining : -1f;
        }

        /// <summary>
        /// Who the HUD names as the opponent. Story and training already say it
        /// through the mission briefing, so only the repeatable modes fill this in:
        /// the antagonist for a raid, the matched commander's call sign for a duel.
        /// </summary>
        private static string ResolveOpponentName()
        {
            return GameSessionContext.SelectedBattleMode switch
            {
                BattleMode.Raid => GameText.SigmaName(),
                BattleMode.Pvp => GameSessionContext.SelectedPvpOpponent?.DisplayName ?? string.Empty,
                _ => string.Empty
            };
        }

        private void RefreshArsenalHudItems()
        {
            var items = new BattleWeaponHudItem[_inventory.Count];
            for (var index = 0; index < _inventory.Count; index++)
            {
                var weapon = _inventory.Weapons[index];
                var remainingAmmo = _inventory.GetRemainingAmmo(weapon);
                var progress = WeaponProgressionBridge.Resolve(weapon.WeaponId);
                var hasProgression = WeaponProgressionCatalog.TryGet(weapon.WeaponId, out var progression);
                var description = hasProgression
                    ? WeaponUpgradePresentation.GetCurrentDescription(weapon, progression, progress)
                    : WeaponPresentationText.Describe(weapon);
                var damageValue = hasProgression
                    ? WeaponUpgradeChoiceResolver.ResolveValue(weapon, progression, progress, WeaponUpgradePath.Core)
                    : weapon.WeaponId == "weapon.grounder" ? 2f : weapon.Damage;
                var accuracyValue = hasProgression
                    ? WeaponUpgradeChoiceResolver.ResolveValue(weapon, progression, progress, WeaponUpgradePath.Aim)
                    : 70f;
                var ammoValue = hasProgression
                    ? WeaponUpgradeChoiceResolver.ResolveValue(weapon, progression, progress, WeaponUpgradePath.Loadout)
                    : (weapon.HasLimitedAmmo ? weapon.AmmoCount : -1f);
                var radiusValue = hasProgression
                    ? WeaponUpgradeChoiceResolver.ResolveValue(weapon, progression, progress, WeaponUpgradePath.Radius)
                    : weapon.ImpactRadius;
                var displayName = hasProgression && progress != null
                    ? WeaponUpgradePresentation.GetWeaponName(
                        progression,
                        progress.rank,
                        (WeaponUpgradeBranch)progress.branch,
                        DescribeWeapon(weapon))
                    : DescribeWeapon(weapon);
                var ammoLabel = weapon.HasLimitedAmmo
                    ? GameText.AmmoLimited(remainingAmmo)
                    : GameText.AmmoUnlimited();
                items[index] = new BattleWeaponHudItem(
                    index,
                    displayName,
                    ResolveHudIconId(weapon),
                    ammoLabel,
                    weapon.HasLimitedAmmo,
                    remainingAmmo,
                    _simulation != null && _simulation.CanUsePlayerWeapon(weapon),
                    progress?.rank ?? 1,
                    description,
                    damageValue,
                    accuracyValue,
                    ammoValue,
                    radiusValue,
                    weapon.WeaponId == "weapon.grounder"
                        ? "architect.detail.statDuration"
                        : "architect.detail.statDamage");
            }

            _arsenalHudItems = items;
        }

        /// <summary>
        /// Same rank/branch-aware icon as the workshop tree, arsenal shelf and shop
        /// listing - the HUD selector shows the evolved art once the weapon reaches
        /// rank 5/10, not just the base icon.
        /// </summary>
        private static string ResolveHudIconId(WeaponDefinition weapon)
        {
            var progress = WeaponProgressionBridge.Resolve(weapon.WeaponId);
            if (progress == null)
            {
                return weapon.IconId;
            }

            var resource = WeaponEvolutionIconResolver.ResolveCurrentIconResource(
                weapon.IconId, progress.rank, (WeaponUpgradeBranch)progress.branch);
            const string prefix = "UI/Weapons/";
            return resource.StartsWith(prefix, System.StringComparison.Ordinal)
                ? resource.Substring(prefix.Length)
                : weapon.IconId;
        }

        private bool IsWeaponSwitchBlockedByGuide()
        {
            return _firstBattleGuide != null && _firstBattleGuide.IsActive && _firstBattleGuide.BlocksWeaponSwitch;
        }

        private bool IsWeaponSwitchBlockedByGuideBeforeWeaponSelected()
        {
            return IsWeaponSwitchBlockedByGuide();
        }

        private bool IsCancelBlockedByGuide()
        {
            return _firstBattleGuide != null && _firstBattleGuide.IsActive && _firstBattleGuide.BlocksCancelAim;
        }

        private static bool IsPointerOverUi()
        {
            return EventSystem.current != null && EventSystem.current.IsPointerOverGameObject();
        }

        private static bool IsPointerOverUi(int pointerId)
        {
            return EventSystem.current != null && EventSystem.current.IsPointerOverGameObject(pointerId);
        }

        private static string DescribeAmmo(WeaponInventoryRuntime inventory)
        {
            var weapon = inventory.Selected;
            return weapon != null && weapon.HasLimitedAmmo
                ? GameText.AmmoLimited(inventory.GetSelectedRemainingAmmo())
                : GameText.AmmoUnlimited();
        }

        private static string DescribeOutcome(BattleOutcome outcome)
        {
            return outcome switch
            {
                BattleOutcome.PlayerWon => GameText.PlayerWon(),
                BattleOutcome.OpponentWon => GameText.OpponentWon(),
                BattleOutcome.Draw => GameText.Draw(),
                _ => string.Empty
            };
        }

        private static string DescribeTeam(BattleTeamId teamId)
        {
            return teamId switch
            {
                BattleTeamId.Player => GameText.PlayerTeam,
                BattleTeamId.Opponent => GameText.OpponentTeam,
                _ => GameText.NoTeam
            };
        }

        private static string DescribeArena(ArenaDefinition arena)
        {
            return arena == null
                ? GameText.ArenaFallback()
                : GameText.ArenaName(arena.ArenaId, arena.DisplayName);
        }

        private static string DescribeWeapon(WeaponDefinition weapon)
        {
            return weapon == null ? GameText.UnknownWeapon : WeaponDisplayNames.Get(weapon.WeaponId);
        }

        private static string DescribeWeaponId(string weaponId)
        {
            return string.IsNullOrWhiteSpace(weaponId) ? GameText.UnknownWeapon : WeaponDisplayNames.Get(weaponId);
        }

        private int GetTeamHealth(BattleTeamId teamId)
        {
            return _battleState.GetTeam(teamId).Units.Sum(unit => unit.Health);
        }

        private int GetTeamMaxHealth(BattleTeamId teamId)
        {
            return _battleState.GetTeam(teamId).Units.Sum(unit => unit.MaxHealth);
        }

        private bool IsBattleReady()
        {
            return _currentArena != null
                && _inventory != null
                && _opponentInventory != null
                && _inventory.HasWeapons
                && _opponentInventory.HasWeapons
                && _weaponExecutor != null
                && _arenas != null
                && _arenas.Length > 0
                && _battleState != null
                && _simulation != null
                && _aimLine != null
                && _trajectoryLine != null;
        }

    }
}
