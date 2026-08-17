using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game.Core;
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
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void CreateBattleController()
        {
            if (SceneManager.GetActiveScene().name != GameSceneNames.Battle)
            {
                return;
            }

            EnsureCreated();
        }

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

    public sealed class BattleSceneController : MonoBehaviour
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
        // Battle-start framing already shows the whole map, so it is the
        // zoom-out floor: players can only zoom in from there, never further out.
        private const float CameraZoomOutMultiplier = 1f;
        private const float CameraZoomStep = 0.62f;
        private const float SpriteArenaBottomScreenOffset = 0.5f;
        private const float PinchZoomSensitivity = 0.008f;
        private const float DirectAimCenterDeadZone = 0.32f;
        private const float FirstBattleNavigationDistance = 1.2f;
        // High enough that the barrel visibly sticks to the finger (time
        // constant ~70 ms); the old 5.5 made straight-shot aiming feel laggy.
        private const float DirectAimSmoothingSpeed = 14f;
        // Barrels of these weapons visually match the aim only after this art
        // correction, and their resting pose tilts slightly toward the ground.
        private const float RestingWeaponTiltDegrees = 10f;
        private const float ControlledPointerDeadZonePixels = 2f;
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
        // asset has real alpha and is shared by every missile body variant.
        private const string RocketTrailResourcePath = "UI/Generated/missle_tail_clean";
        private const float RocketTrailAttachFraction = 0.32f;
        private static readonly HashSet<string> RocketWeaponIds = new(System.StringComparer.Ordinal)
        {
            "weapon.missle",
            "weapon.bur-rocket"
        };

        private readonly Color _playerColor = new(0.18f, 0.56f, 0.96f);
        private readonly Color _opponentColor = new(0.95f, 0.28f, 0.22f);
        private static readonly Color CoinRewardColor = new(1f, 0.8f, 0.22f, 1f);
        private static readonly Color WeaponRewardColor = new(0.48f, 0.9f, 1f, 1f);
        private static readonly Color BlueprintRewardColor = new(0.75f, 0.56f, 1f, 1f);
        private static readonly Color HealthRewardColor = new(0.32f, 0.96f, 0.64f, 1f);
        private readonly List<BattleUnitPresenter> _unitPresenters = new();
        private readonly List<GameObject> _arenaObjects = new();
        private readonly List<ResourcePickup> _resourcePickups = new();
        private readonly Dictionary<string, UnitMotor> _unitMotors = new(System.StringComparer.Ordinal);
        private readonly Dictionary<string, WeaponDefinition> _weaponsById = new(System.StringComparer.Ordinal);
        private readonly Dictionary<string, WeaponEvolutionProfile> _playerWeaponEvolutions = new(System.StringComparer.Ordinal);
        private readonly Dictionary<string, WeaponUpgradeModifiers> _playerWeaponUpgradeModifiers = new(System.StringComparer.Ordinal);
        private readonly Dictionary<string, Sprite> _spriteResourceCache = new(System.StringComparer.Ordinal);
        private readonly Dictionary<int, GameObject> _projectileVisuals = new();
        private readonly Dictionary<int, GameObject> _mineVisuals = new();
        private readonly Dictionary<int, (WeaponDefinition Weapon, BattleTeamId Team)> _mineOwners = new();
        private readonly Dictionary<int, FireBlobVisual> _firePuddleVisuals = new();
        private float _runtimeFallDeathY = FallDeathY;
        private Font _floatingRewardFont;
        private Sprite _topPivotSolidSprite;
        private Sprite[] _electroOverlayFrames;
        private BattleWeaponHudItem[] _arsenalHudItems = System.Array.Empty<BattleWeaponHudItem>();

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
        private readonly List<SpriteRenderer> _trajectoryDots = new();

        // Training-mission aim help: the dotted arc statically extends all the
        // way to the predicted impact point instead of stopping at preview length.
        private bool _impactGuideEnabled;
        private TrajectoryProbe _impactGuideProbe;
        private readonly List<TrajectoryUnitBody> _impactGuideBodies = new();
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
        private Vector2 _controlledPointerPreviousScreenPosition;
        private int _uiWalkInputDirection;
        private int _keyboardWalkInputDirection;
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
        }

        private bool HasProjectilesInFlight => _simulation != null && _simulation.HasProjectilesInFlight;

        private bool HasActiveMotion => _simulation != null && _simulation.HasActiveMotion;

        private bool HasActiveWeaponEffect => _simulation != null && _simulation.HasActiveWeaponEffect;

        private bool HasPendingGroundAction => _simulation != null && _simulation.HasPendingGroundAction;

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

            PopulatePlayerWeaponEvolutions(allWeapons);

            _weaponExecutor = new WeaponExecutor();
            _weaponExecutor.RegisterEffect(WeaponRole.ShortRangeBurst, new BurstProjectileEffect());
            _weaponExecutor.RegisterEffect("weapon.impulse", new ImpulseSalvoWeaponEffect());
            _aiDifficulty = MapDifficulty(GameSessionContext.SelectedAiDifficulty);
            _inventory = new WeaponInventoryRuntime(CreateMissionWeapons(allWeapons), 1);
            ApplyPlayerEvolutionAmmo(_inventory);
            _opponentInventory = new WeaponInventoryRuntime(CreateOpponentWeapons(allWeapons), 1);
            RefreshArsenalHudItems();
            _firstBattleGuide = new FirstBattleGuideState(GameSessionContext.SelectedMissionId);
            _impactGuideEnabled = FirstBattleGuideState.IsFirstMission(GameSessionContext.SelectedMissionId);
            SelectWeapon(_impactGuideEnabled ? 0 : _inventory.SelectedIndex);
            _playerWeaponSelected = false;

            LoadArena(GameSessionContext.SelectedArenaIndex);
        }

        private static WeaponDefinition[] CreateMissionWeapons(IEnumerable<WeaponDefinition> allWeapons)
        {
            return allWeapons
                .Where(IsBattleArsenalWeapon)
                .ToArray();
        }

        private void PopulatePlayerWeaponEvolutions(IEnumerable<WeaponDefinition> weapons)
        {
            _playerWeaponEvolutions.Clear();
            _playerWeaponUpgradeModifiers.Clear();
            if (weapons == null)
            {
                return;
            }

            foreach (var weapon in weapons)
            {
                if (weapon == null || string.IsNullOrWhiteSpace(weapon.WeaponId))
                {
                    continue;
                }

                var progress = WeaponProgressionBridge.Resolve(weapon.WeaponId);
                if (progress == null)
                {
                    continue;
                }

                _playerWeaponEvolutions[weapon.WeaponId] = WeaponEvolutionRuntimeResolver.Resolve(weapon, progress);
                _playerWeaponUpgradeModifiers[weapon.WeaponId] = WeaponUpgradeModifierResolver.Resolve(weapon, progress);
            }
        }

        private void ApplyPlayerEvolutionAmmo(WeaponInventoryRuntime inventory)
        {
            if (inventory == null)
            {
                return;
            }

            foreach (var weapon in inventory.Weapons)
            {
                if (weapon == null
                    || weapon.WeaponId != "weapon.blaster-caset"
                    || !_playerWeaponEvolutions.TryGetValue(weapon.WeaponId, out var evolution))
                {
                    continue;
                }

                inventory.SetRemainingAmmo(weapon, evolution.BlasterBurstShotCount);
            }
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
            _hud.MoveLeftStarted += () => SetUiWalkInput(-1);
            _hud.MoveLeftStopped += () => StopUiWalkInput(-1);
            _hud.MoveRightStarted += () => SetUiWalkInput(1);
            _hud.MoveRightStopped += () => StopUiWalkInput(1);
            _hud.JumpLeftRequested += () => TryStartJump(-1);
            _hud.JumpRightRequested += () => TryStartJump(1);
            _hud.RestartRequested += RestartScene;
            _hud.ContinueRequested += ContinueFromResult;
            _hud.MenuRequested += ExitToMenu;
        }

        private void OnDestroy()
        {
            RestoreBattleTimeScale();
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

        private void Update()
        {
            if (!IsBattleReady())
            {
                return;
            }

            _simulation.SetPlayerTurnTimerPaused(_firstBattleGuide != null && _firstBattleGuide.PausesPlayerTimer);

            if (_coachTimer > 0f && !_arsenalMenuOpen)
            {
                _coachTimer -= Time.deltaTime;
            }

            RenderHud();
            if (_arsenalMenuOpen)
            {
                ClearWalkInput();
                _cameraPanActive = false;
                return;
            }

            UpdateCameraZoom();
            UpdateCameraPan();

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

            if (HasActiveMotion || HasProjectilesInFlight || HasActiveWeaponEffect)
            {
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

            if (!_simulation.Result.IsFinished && !HasProjectilesInFlight && !HasActiveWeaponEffect && Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
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

        private void LoadArena(int arenaIndex)
        {
            if (_arenas == null || _arenas.Length == 0)
            {
                return;
            }

            ClearArenaRuntime();
            _opponentRingOut = false;
            _playerWeaponSelected = false;
            _cameraPanActive = false;

            _arenaIndex = (arenaIndex % _arenas.Length + _arenas.Length) % _arenas.Length;
            _currentArena = _arenas[_arenaIndex];
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
                _playerWeaponEvolutions,
                _playerWeaponUpgradeModifiers);
            SubscribeToSimulation(_simulation);
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

        private void SubscribeToSimulation(BattleSimulation simulation)
        {
            simulation.ShotFired += OnSimulationShotFired;
            simulation.ImpactResolved += OnSimulationImpactResolved;
            simulation.DamageApplied += OnSimulationDamageApplied;
            simulation.TerrainCellsDestroyed += OnSimulationTerrainCellsDestroyed;
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
            simulation.ChargeDeployed += OnSimulationChargeDeployed;
            simulation.GroundObjectRepositioned += OnSimulationGroundObjectRepositioned;
            simulation.LinearWeaponEffectStarted += OnSimulationLinearWeaponEffectStarted;
            simulation.FireBlobSpawned += OnSimulationFireBlobSpawned;
            simulation.FirePuddleMoved += OnSimulationFirePuddleMoved;
            simulation.FirePuddleExtinguished += OnSimulationFirePuddleExtinguished;
            simulation.UnitsMoved += OnSimulationUnitsMoved;
        }

        private void SelectWeapon(int weaponIndex)
        {
            if (_inventory == null || !_inventory.HasWeapons)
            {
                return;
            }

            _inventory.Select(weaponIndex);
            var weapon = _inventory.Selected;
            _aimSettings = weapon.CreateAimSettings(_baseAimSettings);
            ResetAimState();
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
            _camera.orthographicSize = arena.CameraSize;
            _minimumCameraSize = arena.CameraSize * CameraZoomInMultiplier;
            _maximumCameraSize = arena.CameraSize * CameraZoomOutMultiplier;
            _hasPreviousPinchDistance = false;
            _cameraWorldBounds = CalculateCameraWorldBounds(arena);
            ClampCameraToArena();
        }

        private void UpdateCameraZoom()
        {
            if (_camera == null || _minimumCameraSize <= 0f || _maximumCameraSize <= 0f)
            {
                return;
            }

            // Steering a controlled projectile (FPV drone) owns the pointer -
            // camera zoom would fight the piloting input.
            if (_simulation != null && _simulation.HasActiveControlledProjectile)
            {
                _hasPreviousPinchDistance = false;
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
                || _playerWeaponSelected
                || _battleState == null
                || _battleState.Turn.ActiveTeam != BattleTeamId.Player
                || _simulation == null
                || _simulation.Result.IsFinished
                || _simulation.HasActiveControlledProjectile
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

        private static Rect ExpandHorizontalCameraBounds(Rect bounds, Bounds visualBounds)
        {
            if (visualBounds.size.x <= 0f)
            {
                return bounds;
            }

            bounds.xMin = Mathf.Min(bounds.xMin, visualBounds.min.x);
            bounds.xMax = Mathf.Max(bounds.xMax, visualBounds.max.x);
            return bounds;
        }

        private static Rect ExpandCameraBounds(Rect bounds, Bounds visualBounds)
        {
            if (visualBounds.size.x <= 0f || visualBounds.size.y <= 0f)
            {
                return bounds;
            }

            bounds.xMin = Mathf.Min(bounds.xMin, visualBounds.min.x);
            bounds.xMax = Mathf.Max(bounds.xMax, visualBounds.max.x);
            bounds.yMin = Mathf.Min(bounds.yMin, visualBounds.min.y);
            bounds.yMax = Mathf.Max(bounds.yMax, visualBounds.max.y);
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
                _cameraWorldBounds,
                _camera.orthographicSize,
                _camera.aspect,
                fallbackCenter);
        }

        private void AnchorSpriteArenaToCameraBottom()
        {
            if (_camera == null || _currentArena == null || _currentArena.SpriteSurfaces.Count == 0
                || _cameraWorldBounds.height <= 0f)
            {
                return;
            }

            var position = _camera.transform.position;
            position.y = _cameraWorldBounds.yMin + _camera.orthographicSize + SpriteArenaBottomScreenOffset;
            _camera.transform.position = position;
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

        private void BuildWorld(ArenaDefinition arena)
        {
            _visualTheme = BattleVisualThemeLibrary.ForArena(arena);
            _battleTerrain = null;

            AddBackgroundPrefab();
            var solidLayer = AddVisualLayer("Arena Background", _visualTheme?.Background, null);
            var solidLayers = new List<SpriteRenderer>();
            var spriteSurfaces = new List<BattleTerrainSpriteSurface>();
            var solidBlocks = new List<ArenaTerrainBlock>();

            foreach (var block in arena.TerrainBlocks)
            {
                var showProceduralBlock = _visualTheme == null || !_visualTheme.HideProceduralTerrain;
                var solidProceduralBlock = block.Solid && (_visualTheme == null || _visualTheme.UseProceduralTerrainColliders);
                var blockObject = CreateBox(block.BlockId, block.Position, block.Size, block.Color, block.SortingOrder, showProceduralBlock);
                _arenaObjects.Add(blockObject);
                if (solidProceduralBlock)
                {
                    solidBlocks.Add(block);
                }
            }

            solidLayer = AddVisualLayer("Arena Foreground", _visualTheme?.Foreground, solidLayer);
            if (solidLayer != null)
            {
                solidLayers.Add(solidLayer);
                _cameraWorldBounds = ExpandHorizontalCameraBounds(_cameraWorldBounds, solidLayer.bounds);
                ClampCameraToArena();
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

            foreach (var surface in arena.SpriteSurfaces)
            {
                var runtimeSurface = AddArenaSpriteSurface(surface);
                if (runtimeSurface.Renderer == null)
                {
                    continue;
                }

                spriteSurfaces.Add(runtimeSurface);
                _cameraWorldBounds = ExpandCameraBounds(_cameraWorldBounds, runtimeSurface.Renderer.bounds);
            }

            ClampCameraToArena();
            AnchorSpriteArenaToCameraBottom();

            var bounds = ComputeArenaBounds(arena);
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

            // Must run before BuildUnits/CreateSimulationSettings: motors, the
            // simulation and the bot planners all take the kill height from
            // GetFallDeathY(). An arena's kill-zone art (laser barrier) wins
            // over the terrain-derived line so death matches what is on screen.
            _runtimeFallDeathY = _visualTheme?.KillZoneY
                ?? ComputeFallDeathY(_battleTerrain.Grid, FallDeathY);
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
            if (arena.SpriteSurfaces.Count > 0 && _cameraWorldBounds.width > 0f && _cameraWorldBounds.height > 0f)
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

            var playerSpawn = ResolveUnitLandingPosition(arena.PlayerSpawn, BattleTeamId.Player);
            var opponentSpawn = ResolveUnitLandingPosition(arena.OpponentSpawn, BattleTeamId.Opponent);
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

            var isStory = !string.IsNullOrWhiteSpace(GameSessionContext.SelectedMissionId);
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
            _unitMotors.Clear();
            _simulation = null;
            _botTurnController = null;
            _impactGuideProbe = null;
            _projectileVisuals.Clear();
            _mineVisuals.Clear();
            _firePuddleVisuals.Clear();
            _resourcePickups.Clear();
            _hasTrackedPlayerPosition = false;
            _activityBattleRecorded = false;
            _supportDropInProgress = false;
            _nextSupportDropTurn = int.MaxValue;
            _runtimeFallDeathY = FallDeathY;

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
            _battleState = null;
            _resultScreenState = default;
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

        private void UpdateControlledProjectileInput()
        {
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

            if (!TryReadPointer(out var pressed, out var pressStarted, out var released, out var screenPosition, out var pointerOverUi)
                || pointerOverUi)
            {
                _controlledPointerActive = false;
                _simulation.SetControlledProjectileInput(Vector2.zero);
                return;
            }

            if (pressStarted || (pressed && !_controlledPointerActive))
            {
                _controlledPointerActive = true;
                _controlledPointerPreviousScreenPosition = screenPosition;
                _simulation.SetControlledProjectileInput(Vector2.zero);
                return;
            }

            if (_controlledPointerActive && pressed)
            {
                var delta = screenPosition - _controlledPointerPreviousScreenPosition;
                _controlledPointerPreviousScreenPosition = screenPosition;
                _simulation.SetControlledProjectileInput(
                    delta.magnitude >= ControlledPointerDeadZonePixels ? delta.normalized : Vector2.zero);
                return;
            }

            if (released)
            {
                _controlledPointerActive = false;
            }

            _simulation.SetControlledProjectileInput(Vector2.zero);
        }

        private static bool UsesSmoothedDirectAim(WeaponDefinition weapon)
        {
            return weapon?.Role == WeaponRole.DirectProjectile
                && weapon.WeaponId != "weapon.fpv-drone";
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

            var result = _simulation.TryFire(_aimState);
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
            else if (weapon != null && RocketWeaponIds.Contains(weapon.WeaponId))
            {
                SpawnRocketTrail(visualObject.transform, renderer, weapon.WeaponId == "weapon.bur-rocket");
            }
        }

        /// <summary>
        /// Rockets (missile, drill rocket) don't get a fully baked "in flight" sprite
        /// per rank/branch body variant - instead the shared exhaust trail art is
        /// overlaid as a second layer behind the body sprite, parented under the same
        /// transform the body rotates on, so both move and turn together.
        /// </summary>
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
            if (impactWeapon?.WeaponId == "weapon.teleport")
            {
                SpawnTeleportMarkerVisual(impact.Point);
            }

            if (impactWeapon?.WeaponId != "weapon.teleport"
                && impact.ProjectileId != "projectile.weapon.mine.deploy")
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
            renderer.sprite = LoadWeaponIconSprite(ResolveMineSpriteId("mine_off", mine.Weapon, mine.Team));
            renderer.sortingOrder = 27;
            visual.transform.localScale = Vector3.one * 0.11f;
        }

        private void OnSimulationMineTriggered(int mineId)
        {
            if (_mineVisuals.TryGetValue(mineId, out var visual)
                && visual != null
                && visual.TryGetComponent<SpriteRenderer>(out var renderer))
            {
                var (weapon, team) = _mineOwners.TryGetValue(mineId, out var owner) ? owner : (null, BattleTeamId.None);
                renderer.sprite = LoadWeaponIconSprite(ResolveMineSpriteId("mine_on", weapon, team));
            }
        }

        /// <summary>
        /// Rank/branch-aware mine sprite (armed/triggered), same convention as the
        /// rocket body sprite - only the player's own saved progress is consulted,
        /// AI opponents' mines always render at the base rank-1 look.
        /// </summary>
        private static string ResolveMineSpriteId(string baseIconId, WeaponDefinition weapon, BattleTeamId team)
        {
            if (team == BattleTeamId.Player && weapon != null)
            {
                var progress = WeaponProgressionBridge.Resolve(weapon.WeaponId);
                if (progress != null && progress.rank >= 5)
                {
                    var branch = (WeaponUpgradeBranch)progress.branch;
                    var evolvedPath = WeaponEvolutionIconResolver.ResolveEvolutionIconResource(
                        baseIconId, progress.rank, branch);
                    if (!string.IsNullOrWhiteSpace(evolvedPath))
                    {
                        return evolvedPath.Substring(WeaponIconResourceRoot.Length);
                    }
                }
            }

            return baseIconId;
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

        private void OnSimulationGroundObjectRepositioned(GroundObjectRepositionedEvent groundObject)
        {
            if (!_mineVisuals.TryGetValue(groundObject.ObjectId, out var visual) || visual == null)
            {
                return;
            }

            if (visual.TryGetComponent<GroundDropVisual>(out var dropVisual))
            {
                Destroy(dropVisual);
            }

            visual.transform.position = groundObject.Point;
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

            projectileTransform.localScale = ResolveProjectileVisualScale(weapon, diameter);
            if (weapon.WeaponId != "weapon.fpv-drone")
            {
                projectileTransform.rotation = Quaternion.Euler(0f, 0f,
                    Mathf.Atan2(projectile.Velocity.y, projectile.Velocity.x) * Mathf.Rad2Deg);
            }
        }

        private static Vector3 ResolveProjectileVisualScale(WeaponDefinition weapon, float diameter)
        {
            diameter *= 0.5f;
            return weapon.WeaponId switch
            {
                "weapon.blaster-caset" => Vector3.one * (diameter * 0.42f),
                "weapon.impulse" => Vector3.one * (diameter * 0.40572f),
                "weapon.freeze" => Vector3.one * (diameter * 0.315f),
                "weapon.mortira" => Vector3.one * (diameter * 0.36f),
                "weapon.granata" => Vector3.one * (diameter * 0.35f),
                "weapon.teleport" => Vector3.one * (diameter * 0.72f),
                "weapon.toxic" => Vector3.one * (diameter * 1.05f),
                "weapon.emp" => new Vector3(diameter * 0.46f, diameter * 0.33f, 1f),
                "weapon.mine" => Vector3.one * (diameter * 0.55f),
                "weapon.bur-rocket" => Vector3.one * (diameter * 0.68f),
                "weapon.missle" => Vector3.one * (diameter * 1.25f),
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

            return ProjectileWeaponEffect.GetProjectileColor(weapon.Role);
        }

        private Sprite ResolveProjectileSprite(SimProjectile projectile, WeaponDefinition weapon)
        {
            if (weapon == null)
            {
                return null;
            }

            var spriteId = weapon.WeaponId switch
            {
                // These are explicit projectile sprites. Never substitute the held
                // weapon/evolution icon here: it makes the gun itself look as though
                // it was fired from the character.
                "weapon.blaster-caset" => "blaster_shot",
                "weapon.impulse" => "impulse_shot",
                "weapon.mortira" => "mortira_shot",
                "weapon.emp" => "emp_shot",
                "weapon.freeze" => "ice_shot",
                "weapon.toxic" => "toxic_shot",
                "weapon.missle" => "missle_fly",
                "weapon.bur-rocket" => "bur_rocket",
                "weapon.bur" => "bur_lazer",
                "weapon.fire-gun" => "fire_shot_1",
                // The following launch their actual placed/thrown object. Their
                // rank-aware art is therefore the correct in-flight representation.
                "weapon.granata" => ResolveOwnerAwareEvolvedIconId(weapon, projectile.OwnerUnitId),
                "weapon.mine" => ResolveOwnerAwareEvolvedIconId(weapon, projectile.OwnerUnitId),
                "weapon.c4" => ResolveOwnerAwareEvolvedIconId(weapon, projectile.OwnerUnitId),
                "weapon.heal" => ResolveOwnerAwareEvolvedIconId(weapon, projectile.OwnerUnitId),
                "weapon.teleport" => ResolveOwnerAwareEvolvedIconId(weapon, projectile.OwnerUnitId),
                "weapon.bonus-box" => ResolveOwnerAwareEvolvedIconId(weapon, projectile.OwnerUnitId),
                "weapon.fpv-drone" => ResolveOwnerAwareEvolvedIconId(weapon, projectile.OwnerUnitId),
                _ => null
            };
            return LoadWeaponIconSprite(spriteId);
        }

        /// <summary>
        /// Rank/branch-aware icon for a weapon's in-flight/held visual. Only the player's
        /// own saved progress is consulted - AI opponents never evolve their loadout
        /// (see WeaponEvolutionRuntimeResolver), so their projectiles always render at
        /// the base rank-1 art.
        /// </summary>
        private string ResolveOwnerAwareEvolvedIconId(WeaponDefinition weapon, string ownerUnitId)
        {
            if (!string.IsNullOrWhiteSpace(ownerUnitId)
                && ownerUnitId.StartsWith("player.", System.StringComparison.Ordinal))
            {
                var progress = WeaponProgressionBridge.Resolve(weapon.WeaponId);
                if (progress != null && progress.rank >= 5)
                {
                    var branch = (WeaponUpgradeBranch)progress.branch;
                    var evolvedPath = WeaponEvolutionIconResolver.ResolveEvolutionIconResource(
                        weapon.IconId, progress.rank, branch);
                    if (!string.IsNullOrWhiteSpace(evolvedPath))
                    {
                        return evolvedPath.Substring(WeaponIconResourceRoot.Length);
                    }
                }
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

            if (team == BattleTeamId.Player && HidesHeldWeaponAfterUse(weapon)
                && _inventory.GetSelectedRemainingAmmo() <= 0)
            {
                _playerWeaponSelected = false;
                RefreshPlayerWeaponVisual();
            }

            if (team != BattleTeamId.Player || !_simulation.CanContinueBlasterBurst)
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
                || _inventory.GetSelectedRemainingAmmo() > 0)
            {
                return;
            }

            _playerWeaponSelected = false;
            ResetAimState();
            RefreshPlayerWeaponVisual();
        }

        private static bool HidesHeldWeaponAfterUse(WeaponDefinition weapon)
        {
            return weapon?.WeaponId == "weapon.granata"
                || weapon?.WeaponId == "weapon.teleport"
                || weapon?.WeaponId == "weapon.mine"
                || weapon?.WeaponId == "weapon.bur-rocket"
                || weapon?.WeaponId == "weapon.missle"
                || weapon?.WeaponId == "weapon.fpv-drone";
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
                if (pickup.Visual == null || pickup.IsFalling)
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

        private void OnSimulationTurnAdvanced(BattleTeamId newActiveTeam, TurnAdvanceReason reason)
        {
            _playerWeaponSelected = false;
            _controlledPointerActive = false;
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
            _resourcePickups.Add(new ResourcePickup(pickup, groundedPosition, kind));
        }

        private void OnSimulationBattleFinished(BattleResult result, BattleFinishReason reason)
        {
            RecordFinishedBattleActivity(result);
            ReportObjectiveFacts(result);
            _resultScreenState = CreateResultScreenState(result, _battleState.Turn.TurnNumber);
            _status = GameText.ResultPrompt(_resultScreenState.Title);
            if (result.Outcome == BattleOutcome.PlayerWon && reason != BattleFinishReason.InstantActionCompleted)
            {
                ReportTutorial(BattleTutorialBridge.BattleWon);
            }

            _firstBattleGuide?.ObserveBattleFinished();
            EmitFeedback(BattleFeedbackEventType.BattleFinished);
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
            var isTraining = GameSessionContext.IsTrainingSkirmish;
            var isStoryBattle = !isTraining && !string.IsNullOrWhiteSpace(GameSessionContext.SelectedMissionId);
            var isArenaBattle = !isTraining && !isStoryBattle;
            BattleActivityBridge.ReportBattleFinished(
                result.Outcome == BattleOutcome.PlayerWon,
                isStoryBattle,
                isArenaBattle);
        }

        private void OnSimulationCoverDeployed(CoverDeployedEvent cover)
        {
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

                _resourcePickups.RemoveAt(index);
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
                    || WeaponProgressionBridge.IsWeaponUnlocked(candidate.WeaponId)
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
                    if (projectileObject.transform.childCount > 0
                        && projectile.Velocity.sqrMagnitude > 0.01f
                        && ResolveWeaponFromProjectileId(projectile.Definition.ProjectileId)?.WeaponId != "weapon.fpv-drone")
                    {
                        projectileObject.transform.GetChild(0).rotation = Quaternion.Euler(0f, 0f,
                            Mathf.Atan2(projectile.Velocity.y, projectile.Velocity.x) * Mathf.Rad2Deg);
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
        }

        private void StopUiWalkInput(int direction)
        {
            if (_uiWalkInputDirection == NormalizeDirection(direction))
            {
                _uiWalkInputDirection = 0;
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

        private void ClearWalkInput()
        {
            _uiWalkInputDirection = 0;
            _keyboardWalkInputDirection = 0;
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
                var isJumping = _unitMotors.TryGetValue(presenter.State.UnitId, out var motor) && motor.IsJumping;
                var jumpTrailScale = isJumping ? motor.JumpTrailScale : 0f;
                presenter.Refresh(presenter.State.UnitId == _battleState.Turn.ActiveUnitId, isJumping, jumpTrailScale);
                presenter.View.SetFreezeOverlay(freezeOverlay, _simulation.IsUnitFrozen(presenter.State.UnitId));
                presenter.View.SetPoisonOverlay(poisonOverlay, _simulation.IsUnitPoisoned(presenter.State.UnitId));
                presenter.View.SetLavaOverlay(lavaOverlay, _simulation.IsUnitLavaBurning(presenter.State.UnitId));
                presenter.View.SetElectroOverlay(electroOverlayFrames, _simulation.IsUnitElectrified(presenter.State.UnitId));
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
                ? new HeuristicShotPlanner(_aiDifficulty)
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

        private static BattleResultScreenState CreateResultScreenState(BattleResult result, int turnNumber)
        {
            var state = BattleResultScreenFactory.Create(result, turnNumber);
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

            var power01 = Mathf.Approximately(_aimSettings.MinPower, _aimSettings.MaxPower)
                ? 1f
                : Mathf.InverseLerp(_aimSettings.MinPower, _aimSettings.MaxPower, _aimState.Power);
            var previewLength = Mathf.Lerp(MinTrajectoryPreviewLength, MaxTrajectoryPreviewLength, Mathf.Sqrt(power01));
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

        private static bool IsLongDirectWeapon(WeaponDefinition weapon)
        {
            return weapon?.WeaponId == "weapon.blaster-caset"
                || weapon?.WeaponId == "weapon.emp";
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
                new TrajectoryObstacles(_impactGuideBodies, _aimState.Target));

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
            _trajectoryLine.enabled = true;
            _trajectoryLine.positionCount = 2;
            _trajectoryLine.SetPosition(0, firePoint);
            _trajectoryLine.SetPosition(1, GetCoverPlacementPoint(firePoint));
        }

        private void HideTrajectoryPreview()
        {
            _trajectoryLine.enabled = false;
            HideTrajectoryDots();
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

        private SpriteRenderer AddVisualLayer(string objectName, BattleVisualLayer? layer, SpriteRenderer currentSolidLayer)
        {
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
            visual.transform.position = new Vector3(layer.Value.Position.x, layer.Value.Position.y, 0f);
            visual.transform.localScale = new Vector3(layer.Value.Scale.x, layer.Value.Scale.y, 1f);

            var renderer = visual.AddComponent<SpriteRenderer>();
            renderer.sprite = sprite;
            renderer.sortingOrder = layer.Value.SortingOrder;

            if (layer.Value.CoverCamera)
            {
                ScaleSpriteToCoverCamera(visual.transform, sprite);
            }

            _arenaObjects.Add(visual);
            return layer.Value.Solid ? renderer : currentSolidLayer;
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
            visual.transform.position = new Vector3(surface.Position.x, surface.Position.y, 0f);
            visual.transform.localScale = Vector3.one * surface.Scale;
            var renderer = visual.AddComponent<SpriteRenderer>();
            renderer.sprite = sprite;
            renderer.sortingOrder = surface.SortingOrder;
            _arenaObjects.Add(visual);
            return new BattleTerrainSpriteSurface(renderer, mask);
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
                skyBackground.ConfigureCoverage(_camera, _maximumCameraSize, _cameraWorldBounds);

                // The sky art (station/clouds) is now placed in fixed world
                // space instead of following the camera every frame, so the
                // pan clamp must actually allow reaching up to it - otherwise
                // it would sit above the frustum at high zoom with no way to
                // pan into view, no matter how far up the player drags.
                var topWorldY = skyBackground.GetHighestDecorativeWorldY();
                if (topWorldY > _cameraWorldBounds.yMax)
                {
                    _cameraWorldBounds.yMax = topWorldY + 1f;
                }
            }

            _arenaObjects.Add(background);
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
            if (sprite != null)
            {
                _spriteResourceCache.Add(resourcePath, sprite);
            }

            return sprite;
        }

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

            if (!_playerWeaponSelected || _inventory?.Selected == null)
            {
                presenter.ClearEquippedWeapon();
                return;
            }

            var weapon = _inventory.Selected;
            var presentation = ResolveHeldWeaponPresentation(weapon.WeaponId);
            presenter.SetWeaponPresentation(
                presentation.Scale,
                presentation.Position,
                presentation.AimAngleOffset);
            var heldIconId = ResolveHudIconId(weapon);
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

        private static (float Scale, Vector2 Position, float AimAngleOffset) ResolveHeldWeaponPresentation(string weaponId)
        {
            return weaponId switch
            {
                "weapon.impulse" => (1f, new Vector2(-0.48f, 0.27f), 0f),
                "weapon.blaster-caset" => (1f, new Vector2(-0.56f, 0.34f), 0f),
                // Positive offsets counter the art's built-in downward barrel
                // tilt so the barrel axis matches the aim/trajectory exactly.
                "weapon.bur" => (1.5f, new Vector2(-0.54f, 0.42f), 15f),
                "weapon.mortira" => (1.12f, new Vector2(-0.55f, 0.40f), 7f),
                "weapon.granata" => (0.76f, new Vector2(-0.50f, 0.29f), 0f),
                "weapon.teleport" => (0.82f, new Vector2(-0.60f, 0.40f), 0f),
                "weapon.mine" => (0.82f, new Vector2(-0.58f, 0.36f), 0f),
                "weapon.toxic" => (1f, new Vector2(-0.68f, 0.42f), 11f),
                "weapon.emp" => (1f, new Vector2(-0.55f, 0.42f), 0f),
                "weapon.freeze" => (1f, new Vector2(-0.55f, 0.42f), 10f),
                "weapon.fire-gun" => (1.5f, new Vector2(-0.68f, 0.42f), 0f),
                "weapon.missle" => (1.5f, new Vector2(-0.54f, 0.30f), 0f),
                "weapon.heal" => (1f, new Vector2(-0.54f, 0.38f), 180f),
                "weapon.fpv-drone" => (1f, new Vector2(-0.28f, 1.08f), 0f),
                _ => (1f, new Vector2(-0.68f, 0.42f), 0f)
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
            if (!IsBattleReady() || !_simulation.Result.IsFinished)
            {
                return;
            }

            // Win: report completion so the menu advances. Loss: just return to menu.
            if (_simulation.Result.Outcome == BattleOutcome.PlayerWon)
            {
                GameSessionContext.ReportMissionCompleted();
            }

            // "Continue" drops the player back onto the campaign map with this mission's
            // planet already open, rather than straight into the next battle.
            GameSessionContext.RequestCampaignMissionView(GameSessionContext.SelectedMissionId);
            LoadMenuScene();
        }

        private void ExitToMenu()
        {
            LoadMenuScene();
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
            if (_hud == null || !IsBattleReady())
            {
                return;
            }

            var weapon = _inventory.Selected;
            var playerHasTime = _simulation.MovementTimeRemaining > 0f;
            var fireInputReady = !BattleSimulation.RequiresAim(weapon) || _aimState.HasInput;
            var canFire = !_simulation.Result.IsFinished
                && !HasProjectilesInFlight
                && !HasActiveMotion
                && !HasActiveWeaponEffect
                && !HasPendingGroundAction
                && playerHasTime
                && _playerWeaponSelected
                && _inventory.CanFireSelected
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
                _simulation.MovementTimeRemaining,
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
                _aimState.Power,
                _aimSettings.MinPower,
                _aimSettings.MaxPower,
                canFire && (!guideActive || _firstBattleGuide.AllowsFire),
                canUseMovement,
                _simulation.Result.IsFinished,
                _resultScreenState.Title,
                _resultScreenState.Subtitle,
                _simulation.Result.IsFinished ? DescribeOutcome(_resultScreenState.Result.Outcome) : string.Empty,
                _simulation.Result.IsFinished ? GameText.Reward(_resultScreenState.RewardPreview.SoftCurrency) : string.Empty,
                _simulation.Result.IsFinished && _resultScreenState.RewardPreview.HasUnlock ? _resultScreenState.RewardPreview.UnlockText : GameText.NoUnlocks,
                _simulation.Result.IsFinished && _resultScreenState.CanContinue,
                _simulation.Result.IsFinished ? _resultScreenState.RetryLabel : GameText.Restart,
                _resultScreenState.CanContinue ? _resultScreenState.ContinueLabel : GameText.Menu,
                _simulation.Result.Outcome == BattleOutcome.PlayerWon,
                GameSessionContext.SelectedMissionText,
                // Only a won battle banks the mission - a loss leaves it outstanding even
                // when its objective was met, matching what the campaign map will show.
                _simulation.Result.Outcome == BattleOutcome.PlayerWon);

            _hud.Render(state);
        }

        private void RefreshArsenalHudItems()
        {
            var items = new BattleWeaponHudItem[_inventory.Count];
            for (var index = 0; index < _inventory.Count; index++)
            {
                var weapon = _inventory.Weapons[index];
                var remainingAmmo = _inventory.GetRemainingAmmo(weapon);
                var ammoLabel = weapon.HasLimitedAmmo
                    ? GameText.AmmoLimited(remainingAmmo)
                    : GameText.AmmoUnlimited();
                items[index] = new BattleWeaponHudItem(
                    index,
                    DescribeWeapon(weapon),
                    ResolveHudIconId(weapon),
                    ammoLabel,
                    weapon.HasLimitedAmmo,
                    remainingAmmo,
                    !weapon.HasLimitedAmmo || remainingAmmo > 0);
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
