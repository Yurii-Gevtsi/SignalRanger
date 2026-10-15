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

    public sealed partial class BattleSceneController : MonoBehaviour, IBattleCommandTarget
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
        private readonly Dictionary<int, TextMesh> _deviceHealthLabels = new();
        private readonly Dictionary<int, BattleTeamId> _deviceTeams = new();
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
                || HasProjectilesInFlight && !CanContinueMinePlacementInput() && !_simulation.IsActiveSkyHookArming)
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

        private const string GuardianDeployProjectileId = "projectile.weapon.guardian-drone.deploy";
        private const float GrounderPlacedVisualScale = 0.11f;
        private const float GuardianPlacedVisualScale = 0.11f;

        private const float GroundDeployPointLift = 0.06f;

        /// <summary>A carried item is drawn over the copter's body; a released one returns to the ground layer.</summary>
        private readonly HashSet<int> _releasedCargoIds = new();

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

        void IBattleCommandTarget.SetProjectileInput(Vector2 direction)
        {
            _simulation?.SetControlledProjectileInput(direction);
        }

        void IBattleCommandTarget.EndTurn()
        {
            _simulation?.EndActiveTurn();
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

        private GameObject _turnTransferMarker;

        private const float TurnTransferMarkerWorldWidth = 0.6f;

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
