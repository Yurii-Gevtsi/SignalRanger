using System.Collections.Generic;
using System.Linq;
using Game.Core;
using Game.Core.Battle;
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
        private const float CameraZoomOutMultiplier = 1.25f;
        private const float CameraZoomStep = 0.62f;
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

        private readonly Color _playerColor = new(0.18f, 0.56f, 0.96f);
        private readonly Color _opponentColor = new(0.95f, 0.28f, 0.22f);
        private readonly List<BattleUnitPresenter> _unitPresenters = new();
        private readonly List<GameObject> _arenaObjects = new();
        private readonly Dictionary<string, UnitMotor> _unitMotors = new(System.StringComparer.Ordinal);
        private readonly Dictionary<string, WeaponDefinition> _weaponsById = new(System.StringComparer.Ordinal);
        private readonly Dictionary<string, Sprite> _spriteResourceCache = new(System.StringComparer.Ordinal);
        private readonly Dictionary<int, GameObject> _projectileVisuals = new();
        private readonly Dictionary<int, GameObject> _mineVisuals = new();
        private readonly Dictionary<int, FireBlobVisual> _firePuddleVisuals = new();
        private float _runtimeFallDeathY = FallDeathY;
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

            _weaponExecutor = new WeaponExecutor();
            _weaponExecutor.RegisterEffect(WeaponRole.ShortRangeBurst, new BurstProjectileEffect());
            _aiDifficulty = MapDifficulty(GameSessionContext.SelectedAiDifficulty);
            _inventory = new WeaponInventoryRuntime(CreateMissionWeapons(allWeapons), 1);
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

        private static bool IsBattleArsenalWeapon(WeaponDefinition weapon)
        {
            return weapon != null
                && !WeaponPresentationText.IsDropOnly(weapon);
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

            if (!HasProjectilesInFlight && !HasActiveWeaponEffect && Keyboard.current != null && Keyboard.current.nKey.wasPressedThisFrame)
            {
                LoadArena(_arenaIndex + 1);
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
                _opponentArchetype);
            SubscribeToSimulation(_simulation);
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
            ReportTutorial(BattleTutorialBridge.MissionStart);
        }

        private void SubscribeToSimulation(BattleSimulation simulation)
        {
            simulation.ShotFired += OnSimulationShotFired;
            simulation.ImpactResolved += OnSimulationImpactResolved;
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

            if (TryGetPinchDistance(out var pinchDistance))
            {
                if (_hasPreviousPinchDistance)
                {
                    AdjustCameraZoom(-(pinchDistance - _previousPinchDistance) * PinchZoomSensitivity);
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

            if (Mouse.current != null && !IsPointerOverUi())
            {
                var scroll = Mouse.current.scroll.ReadValue().y;
                if (Mathf.Abs(scroll) > 0.01f)
                {
                    zoomDelta -= Mathf.Sign(scroll) * CameraZoomStep;
                }
            }

            AdjustCameraZoom(zoomDelta);
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
            distance = 0f;
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

            distance = Vector2.Distance(firstTouch.position.ReadValue(), secondTouch.position.ReadValue());
            return true;
        }

        private void AdjustCameraZoom(float sizeDelta)
        {
            if (_camera == null || Mathf.Approximately(sizeDelta, 0f) || _minimumCameraSize <= 0f || _maximumCameraSize <= 0f)
            {
                return;
            }

            _camera.orthographicSize = Mathf.Clamp(_camera.orthographicSize + sizeDelta, _minimumCameraSize, _maximumCameraSize);
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

            var bounds = ComputeArenaBounds(arena);
            if (solidLayers.Count > 0)
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
                _battleTerrain.StampSolidRect(block.Position, block.Size);
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
            return new Rect(minX, minY, maxX - minX, maxY - minY);
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
            _battleTerrain?.Dispose();
            _battleTerrain = null;
            _unitMotors.Clear();
            _simulation = null;
            _botTurnController = null;
            _impactGuideProbe = null;
            _projectileVisuals.Clear();
            _mineVisuals.Clear();
            _firePuddleVisuals.Clear();
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

            var renderer = visualObject.AddComponent<SpriteRenderer>();
            renderer.sprite = ResolveProjectileSprite(projectile) ?? _circleSprite;
            renderer.color = renderer.sprite == _circleSprite ? ResolveProjectileVisualColor(projectile) : Color.white;
            renderer.sortingOrder = 30;

            if (ResolveWeaponFromProjectileId(projectile.Definition.ProjectileId)?.WeaponId == "weapon.fpv-drone")
            {
                var animator = visualObject.AddComponent<WeaponProjectileSpriteAnimator>();
                animator.SetFrames(LoadWeaponIconSprite("fpv_0"), LoadWeaponIconSprite("fpv_1"),
                    LoadWeaponIconSprite("fpv_2"), LoadWeaponIconSprite("fpv_3"), LoadWeaponIconSprite("fpv_4"));
            }
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
                var flashScale = impact.ProjectileId != null
                    && impact.ProjectileId.StartsWith("projectile.weapon.mortira.fragment", System.StringComparison.Ordinal)
                        ? 0.6f
                        : 1.2f;
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
            visual.transform.position = mine.Point;
            var renderer = visual.AddComponent<SpriteRenderer>();
            renderer.sprite = LoadWeaponIconSprite("mine_off");
            renderer.sortingOrder = 27;
            visual.transform.localScale = Vector3.one * 0.11f;
        }

        private void OnSimulationMineTriggered(int mineId)
        {
            if (_mineVisuals.TryGetValue(mineId, out var visual)
                && visual != null
                && visual.TryGetComponent<SpriteRenderer>(out var renderer))
            {
                renderer.sprite = LoadWeaponIconSprite("mine_on");
            }
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

        private void OnSimulationLinearWeaponEffectStarted(LinearWeaponEffectEvent effect)
        {
            var visual = new GameObject("Linear Weapon Effect");
            _arenaObjects.Add(visual);
            var renderer = visual.AddComponent<SpriteRenderer>();
            renderer.sprite = LoadWeaponIconSprite(effect.Weapon.WeaponId == "weapon.bur" ? "bur_lazer" : effect.Weapon.IconId);
            renderer.sortingOrder = 32;
            var animator = visual.AddComponent<LinearWeaponEffectVisual>();
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
                "weapon.blaster-caset" => Vector3.one * (diameter * 0.936f),
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

        private Color ResolveProjectileVisualColor(SimProjectile projectile)
        {
            var weapon = ResolveWeaponFromProjectileId(projectile.Definition.ProjectileId);
            if (weapon == null)
            {
                return Color.black;
            }

            return ProjectileWeaponEffect.GetProjectileColor(weapon.Role);
        }

        private Sprite ResolveProjectileSprite(SimProjectile projectile)
        {
            var weapon = ResolveWeaponFromProjectileId(projectile.Definition.ProjectileId);
            if (weapon == null)
            {
                return null;
            }

            var spriteId = weapon.WeaponId switch
            {
                "weapon.blaster-caset" => "blaster_shot",
                "weapon.bur" => "bur_lazer",
                "weapon.impulse" => "impulse_shot",
                "weapon.mortira" => "mortira_shot",
                "weapon.granata" => "granata",
                "weapon.teleport" => "teleport",
                "weapon.freeze" => "ice_shot",
                "weapon.toxic" => "toxic_shot",
                "weapon.emp" => "emp_shot",
                "weapon.missle" => "missle_fly",
                "weapon.fire-gun" => "fire_shot_1",
                _ => weapon.IconId
            };
            return LoadWeaponIconSprite(spriteId);
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
                && projectileId.StartsWith("projectile.weapon.mortira.fragment", System.StringComparison.Ordinal))
            {
                return _weaponsById.TryGetValue("weapon.mortira", out var mortar) ? mortar : null;
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
                RefreshArsenalHudItems();
                ClearSelectedWeaponIfDepleted(weapon);
            }
            if (setsShotStatus)
            {
                _status = GameText.ShotStatus(DescribeTeam(team), DescribeWeapon(weapon));
            }

            if (team == BattleTeamId.Player && HidesHeldWeaponAfterUse(weapon))
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

        private void OnSimulationUnitDefeated(UnitDefeatedEvent defeat)
        {
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
        }

        private void OnSimulationBattleFinished(BattleResult result, BattleFinishReason reason)
        {
            _resultScreenState = CreateResultScreenState(result, _battleState.Turn.TurnNumber);
            _status = GameText.ResultPrompt(_resultScreenState.Title);
            if (result.Outcome == BattleOutcome.PlayerWon && reason != BattleFinishReason.InstantActionCompleted)
            {
                ReportTutorial(BattleTutorialBridge.BattleWon);
            }

            _firstBattleGuide?.ObserveBattleFinished();
            EmitFeedback(BattleFeedbackEventType.BattleFinished);
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
            RefreshUnitViews();
            SyncProjectileVisuals();
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
            foreach (var presenter in _unitPresenters)
            {
                presenter.Refresh(presenter.State.UnitId == _battleState.Turn.ActiveUnitId);
            }
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
            presenter.SetEquippedWeapon(
                LoadWeaponIconSprite(weapon.IconId),
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
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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
            var showTutorialCard = !GameSessionContext.IsTrainingSkirmish
                && (!isFirstTrainingBattle || guideActive || _coachTimer > 0f);
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
                weapon != null ? weapon.IconId : string.Empty,
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
                _resultScreenState.CanContinue ? _resultScreenState.ContinueLabel : GameText.Menu);

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
                    weapon.IconId,
                    ammoLabel,
                    weapon.HasLimitedAmmo,
                    remainingAmmo,
                    !weapon.HasLimitedAmmo || remainingAmmo > 0);
            }

            _arsenalHudItems = items;
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
