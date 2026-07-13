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
        private const int TrajectoryDotCount = 13;
        private const float CameraZoomInMultiplier = 0.58f;
        private const float CameraZoomOutMultiplier = 1.25f;
        private const float CameraZoomStep = 0.62f;
        private const float PinchZoomSensitivity = 0.008f;

        private readonly Color _playerColor = new(0.18f, 0.56f, 0.96f);
        private readonly Color _opponentColor = new(0.95f, 0.28f, 0.22f);
        private readonly List<BattleUnitPresenter> _unitPresenters = new();
        private readonly List<GameObject> _arenaObjects = new();
        private readonly Dictionary<string, UnitMotor> _unitMotors = new(System.StringComparer.Ordinal);
        private readonly Dictionary<string, WeaponDefinition> _weaponsById = new(System.StringComparer.Ordinal);
        private readonly Dictionary<string, Sprite> _spriteResourceCache = new(System.StringComparer.Ordinal);
        private readonly Dictionary<int, GameObject> _projectileVisuals = new();

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
        private Vector2 _aimAnchorWorld;
        private int _uiWalkInputDirection;
        private int _keyboardWalkInputDirection;
        private float _minimumCameraSize;
        private float _maximumCameraSize;
        private float _previousPinchDistance;
        private bool _hasPreviousPinchDistance;

        private bool HasProjectilesInFlight => _simulation != null && _simulation.HasProjectilesInFlight;

        private bool HasActiveMotion => _simulation != null && _simulation.HasActiveMotion;

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
            _weaponExecutor.RegisterEffect("weapon.scatter-arc", new BurstProjectileEffect(3, 20f));
            _aiDifficulty = MapDifficulty(GameSessionContext.SelectedAiDifficulty);
            _inventory = new WeaponInventoryRuntime(CreateMissionWeapons(allWeapons), 1);
            _opponentInventory = new WeaponInventoryRuntime(CreateOpponentWeapons(allWeapons), 1);
            _firstBattleGuide = new FirstBattleGuideState(GameSessionContext.SelectedMissionId);
            SelectWeapon(_inventory.SelectedIndex);

            LoadArena(GameSessionContext.SelectedArenaIndex);
        }

        private static WeaponDefinition[] CreateMissionWeapons(IEnumerable<WeaponDefinition> allWeapons)
        {
            var byId = allWeapons
                .Where(weapon => weapon != null)
                .ToDictionary(weapon => weapon.WeaponId);

            var effectiveIds = BattleLoadoutResolver.Resolve(
                GameSessionContext.SelectedLoadoutWeaponIds,
                GameSessionContext.SelectedAllowedWeaponIds,
                allWeapons.Select(weapon => weapon.WeaponId).ToArray());

            var resolved = effectiveIds
                .Where(byId.ContainsKey)
                .Select(id => byId[id])
                .ToArray();

            return resolved.Length == 0 ? allWeapons.ToArray() : resolved;
        }

        private static WeaponDefinition[] CreateOpponentWeapons(IEnumerable<WeaponDefinition> allWeapons)
        {
            var byId = allWeapons
                .Where(weapon => weapon != null)
                .ToDictionary(weapon => weapon.WeaponId);

            var selectedIds = GameSessionContext.SelectedOpponentWeaponIds != null && GameSessionContext.SelectedOpponentWeaponIds.Length > 0
                ? GameSessionContext.SelectedOpponentWeaponIds
                : new[] { "weapon.steady-orb", "weapon.pulse-shell" };

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
            _hud.MoveLeftStarted += () => SetUiWalkInput(-1);
            _hud.MoveLeftStopped += () => StopUiWalkInput(-1);
            _hud.MoveRightStarted += () => SetUiWalkInput(1);
            _hud.MoveRightStopped += () => StopUiWalkInput(1);
            _hud.JumpLeftRequested += () => TryStartJump(-1);
            _hud.JumpRightRequested += () => TryStartJump(1);
            _hud.ZoomInRequested += () => AdjustCameraZoom(-CameraZoomStep);
            _hud.ZoomOutRequested += () => AdjustCameraZoom(CameraZoomStep);
            _hud.RestartRequested += RestartScene;
            _hud.ContinueRequested += ContinueFromResult;
            _hud.MenuRequested += ExitToMenu;
        }

        private void Update()
        {
            if (!IsBattleReady())
            {
                return;
            }

            if (_coachTimer > 0f)
            {
                _coachTimer -= Time.deltaTime;
            }

            RenderHud();
            UpdateCameraZoom();

            if (Keyboard.current != null && Keyboard.current.rKey.wasPressedThisFrame)
            {
                RestartScene();
                return;
            }

            if (_simulation.Result.IsFinished)
            {
                return;
            }

            if (HasActiveMotion || HasProjectilesInFlight)
            {
                _simulation.Tick(Time.deltaTime);
                return;
            }

            if (_battleState.Turn.ActiveTeam == BattleTeamId.Opponent)
            {
                UpdateOpponentBot();
                return;
            }

            if (!HasProjectilesInFlight && Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
            {
                if (!IsCancelBlockedByGuide())
                {
                    CancelAim();
                }

                return;
            }

            if (!HasProjectilesInFlight && Keyboard.current != null && Keyboard.current.nKey.wasPressedThisFrame)
            {
                LoadArena(_arenaIndex + 1);
                return;
            }

            if (!HasProjectilesInFlight && Keyboard.current != null && Keyboard.current.tabKey.wasPressedThisFrame)
            {
                if (!IsWeaponSwitchBlockedByGuide())
                {
                    SelectWeapon(_inventory.SelectedIndex + 1);
                }

                return;
            }

            if (!_simulation.Result.IsFinished && !HasProjectilesInFlight && Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
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

                if (!HasProjectilesInFlight && Keyboard.current.qKey.wasPressedThisFrame)
                {
                    TryStartJump(-1);
                    return;
                }

                if (!HasProjectilesInFlight && Keyboard.current.eKey.wasPressedThisFrame)
                {
                    TryStartJump(1);
                    return;
                }
            }

            UpdateWalking();
            _simulation.Tick(Time.deltaTime);
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
            _aimState = new AimState(Vector2.zero, Vector2.right, _aimSettings.MinPower, false);
            _aimDragActive = false;
            _status = _currentArena != null
                ? GameText.WeaponSelected(DescribeWeapon(weapon))
                : GameText.WeaponSelectedGeneric();
        }

        private void ApplyArenaCamera(ArenaDefinition arena)
        {
            _camera.transform.position = arena.CameraPosition;
            _camera.orthographicSize = arena.CameraSize;
            _minimumCameraSize = arena.CameraSize * CameraZoomInMultiplier;
            _maximumCameraSize = arena.CameraSize * CameraZoomOutMultiplier;
            _hasPreviousPinchDistance = false;
        }

        private void UpdateCameraZoom()
        {
            if (_camera == null || _minimumCameraSize <= 0f || _maximumCameraSize <= 0f)
            {
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
        }

        private void BuildWorld(ArenaDefinition arena)
        {
            _visualTheme = BattleVisualThemeLibrary.ForArena(arena);
            _battleTerrain = null;

            var solidLayer = AddVisualLayer("Arena Background", _visualTheme?.Background, null);
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
            AddKillZoneVisual();

            var bounds = ComputeArenaBounds(arena);
            if (solidLayer != null)
            {
                _battleTerrain = BattleTerrain.CreateFromSprite(solidLayer, solidLayer.sprite, bounds);
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
            return _visualTheme?.KillZoneDeathY ?? FallDeathY;
        }

        private void AddKillZoneVisual()
        {
            if (_visualTheme == null || !_visualTheme.HasKillZonePrefab)
            {
                return;
            }

            var prefab = Resources.Load<GameObject>(_visualTheme.KillZonePrefabResourcePath);
            if (prefab == null)
            {
                Debug.LogWarning($"Kill-zone prefab '{_visualTheme.KillZonePrefabResourcePath}' was not found.");
                return;
            }

            var killZone = Object.Instantiate(prefab);
            ScaleKillZoneToCamera(killZone);
            _arenaObjects.Add(killZone);
        }

        private void ScaleKillZoneToCamera(GameObject killZone)
        {
            if (_camera == null || killZone == null)
            {
                return;
            }

            var visuals = killZone.transform.Find("Visuals");
            var waveRenderer = visuals?.Find("SurfaceWaves")?.GetComponent<SpriteRenderer>();
            if (waveRenderer == null || waveRenderer.bounds.size.x <= 0f)
            {
                return;
            }

            var cameraHeight = Mathf.Max(_camera.orthographicSize, _maximumCameraSize) * 2f;
            var cameraWidth = cameraHeight * _camera.aspect;
            var horizontalScale = cameraWidth / waveRenderer.bounds.size.x;
            visuals.localScale = new Vector3(horizontalScale, 1f, 1f);
        }

        private static UnitMotionProfile CreateMotionProfile(CharacterArchetype archetype)
        {
            return new UnitMotionProfile(
                halfWidth: UnitCollisionHalfWidth,
                halfHeight: archetype.BodyHeight * 0.5f,
                crawlHeight: UnitCrawlHeight,
                walkStepHeight: archetype.WalkStepHeight,
                walkSpeed: archetype.WalkSpeed);
        }

        private void ClearArenaRuntime()
        {
            _battleTerrain?.Dispose();
            _battleTerrain = null;
            _unitMotors.Clear();
            _simulation = null;
            _botTurnController = null;
            _projectileVisuals.Clear();

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
            }
            else if (skin == null)
            {
                unitObject.transform.localScale = new Vector3(0.72f, bodyHeight, 1f);

                var renderer = unitObject.AddComponent<SpriteRenderer>();
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

            return view;
        }

        private void BuildAimRenderers()
        {
            _aimLine = CreateLine("Aim Line", 0.08f, Color.white);
            _trajectoryLine = CreateLine("Trajectory Preview", 0.035f, new Color(1f, 1f, 1f, 0.55f));
            BuildTrajectoryDots();
        }

        private void BuildTrajectoryDots()
        {
            for (var i = 0; i < TrajectoryDotCount; i++)
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
                _aimAnchorWorld = ScreenToWorld(screenPosition);
            }

            if (_aimDragActive && pressed)
            {
                var pull = ScreenToWorld(screenPosition) - _aimAnchorWorld;
                _aimState = AimCalculator.CalculateFromPull(origin, pull, _aimSettings);
                _firstBattleGuide?.ObserveAimState(_aimState.HasInput);
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
            }
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
            renderer.sprite = _circleSprite;
            renderer.color = ResolveProjectileVisualColor(projectile);
            renderer.sortingOrder = 30;
        }

        private void OnSimulationProjectileExploded(int projectileId, ProjectileImpact impact)
        {
            if (_projectileVisuals.TryGetValue(projectileId, out var projectileObject))
            {
                _projectileVisuals.Remove(projectileId);
                _arenaObjects.Remove(projectileObject);
                Destroy(projectileObject);
            }

            SpawnImpactFlash(impact.Point);
            if (!string.IsNullOrWhiteSpace(impact.OwnerUnitId)
                && impact.OwnerUnitId.StartsWith("player.", System.StringComparison.Ordinal))
            {
                _firstBattleGuide?.ObserveProjectileImpact();
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

            switch (weapon.WeaponId)
            {
                case "weapon.long-lance":
                    projectileTransform.localScale = new Vector3(diameter * 2.6f, diameter * 0.55f, 1f);
                    projectileTransform.rotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(projectile.Velocity.y, projectile.Velocity.x) * Mathf.Rad2Deg);
                    break;
                case "weapon.pulse-shell":
                    projectileTransform.localScale = Vector3.one * (diameter * 1.25f);
                    break;
                case "weapon.scatter-arc":
                    projectileTransform.localScale = Vector3.one * (diameter * 0.82f);
                    break;
                case "weapon.rampart-kit":
                    projectileTransform.localScale = new Vector3(diameter * 2.15f, diameter * 0.7f, 1f);
                    projectileTransform.rotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(projectile.Velocity.y, projectile.Velocity.x) * Mathf.Rad2Deg);
                    break;
                case "weapon.relay-step":
                    projectileTransform.localScale = Vector3.one * (diameter * 0.9f);
                    break;
                default:
                    projectileTransform.localScale = Vector3.one * diameter;
                    break;
            }
        }

        private Color ResolveProjectileVisualColor(SimProjectile projectile)
        {
            var weapon = ResolveWeaponFromProjectileId(projectile.Definition.ProjectileId);
            if (weapon == null)
            {
                return Color.black;
            }

            return weapon.WeaponId switch
            {
                "weapon.steady-orb" => new Color(0.36f, 0.78f, 1f, 0.95f),
                "weapon.pulse-shell" => new Color(1f, 0.49f, 0.18f, 0.96f),
                "weapon.long-lance" => new Color(0.92f, 0.98f, 1f, 0.98f),
                "weapon.scatter-arc" => new Color(1f, 0.84f, 0.32f, 0.94f),
                "weapon.rampart-kit" => new Color(0.54f, 0.9f, 0.55f, 0.96f),
                "weapon.relay-step" => new Color(0.68f, 0.46f, 1f, 0.94f),
                _ => ProjectileWeaponEffect.GetProjectileColor(weapon.Role)
            };
        }

        private WeaponDefinition ResolveWeaponFromProjectileId(string projectileId)
        {
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
            if (setsShotStatus)
            {
                _status = GameText.ShotStatus(DescribeTeam(team), DescribeWeapon(weapon));
            }

            _aimDragActive = false;
            _aimLine.enabled = false;
            HideTrajectoryPreview();
            EmitFeedback(BattleFeedbackEventType.ShotFired);
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
            if (reason != TurnAdvanceReason.OpponentSkippedNoUsableWeapon)
            {
                _firstBattleGuide?.ObserveTurnAdvanced(newActiveTeam);
            }

            ApplyAimSettingsForActiveTeam();
            _aimState = new AimState(Vector2.zero, Vector2.right, _aimSettings.MinPower, false);
            _aimDragActive = false;
            ClearWalkInput();
            _aimLine.enabled = false;
            HideTrajectoryPreview();

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
            if (!CanPlayerAcceptMovementInput())
            {
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
            _keyboardWalkInputDirection = CanPlayerAcceptMovementInput() ? NormalizeDirection(direction) : 0;
        }

        private static int NormalizeDirection(int direction)
        {
            return direction < 0 ? -1 : direction > 0 ? 1 : 0;
        }

        private bool CanPlayerUseMovement()
        {
            return IsBattleReady()
                && !IsMovementBlockedByGuide()
                && _simulation.CanUseMovement();
        }

        private bool CanPlayerAcceptMovementInput()
        {
            return IsBattleReady()
                && !IsMovementBlockedByGuide()
                && _simulation.CanAcceptMovementInput();
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
                _simulation?.Walk(0, 0f);
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
                    _status = GameText.Moved(DescribeTeam(BattleTeamId.Player));
                    break;
                case BattleWalkOutcome.Blocked:
                    _status = GameText.MovementBlocked();
                    ClearWalkInput();
                    break;
                case BattleWalkOutcome.StartedFalling:
                    // Regular physics takes over from here (HasActiveMotion).
                    break;
            }
        }

        private void ClearWalkInput()
        {
            _uiWalkInputDirection = 0;
            _keyboardWalkInputDirection = 0;
            _simulation?.Walk(0, 0f);
        }

        private void TryStartJump(int direction)
        {
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
                _status = GameText.Jumped(DescribeTeam(BattleTeamId.Player));
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

            if (_inventory.Selected.Role == WeaponRole.ShortRangeBurst)
            {
                HideTrajectoryPreview();
                return;
            }

            if (_inventory.Selected.Role == WeaponRole.CoverTool)
            {
                DrawCoverPlacementPreview(activePresenter.View.FirePoint.position);
                return;
            }

            if (IsInstantUtility(_inventory.Selected.Role))
            {
                HideTrajectoryPreview();
                return;
            }

            DrawBallisticTrajectory(activePresenter.View.FirePoint.position, _inventory.Selected);
        }

        private static bool IsInstantUtility(WeaponRole role)
        {
            return role == WeaponRole.CoverTool
                || role == WeaponRole.DefensiveTool;
        }

        private void DrawBallisticTrajectory(Vector2 firePoint, WeaponDefinition weapon)
        {
            _trajectoryLine.enabled = false;

            var power01 = Mathf.InverseLerp(_aimSettings.MinPower, _aimSettings.MaxPower, _aimState.Power);
            var previewLength = Mathf.Lerp(2.15f, 4.15f, Mathf.Sqrt(power01));
            var previewEndTime = FindTrajectoryPreviewEndTime(firePoint, weapon, previewLength);
            var upwardAim01 = Mathf.InverseLerp(0.12f, 0.86f, _aimState.Direction.y);
            var weakShot01 = 1f - power01;
            var minimumArcTime = Mathf.Lerp(0.34f, 0.78f, upwardAim01 * weakShot01);
            previewEndTime = Mathf.Max(previewEndTime, minimumArcTime);

            for (var i = 0; i < _trajectoryDots.Count; i++)
            {
                var dot = _trajectoryDots[i];
                var t01 = _trajectoryDots.Count <= 1 ? 0f : (float)i / (_trajectoryDots.Count - 1);
                var time01 = Mathf.Pow(t01, 0.86f);
                var point = GetTrajectoryPreviewPointAtTime(firePoint, weapon, previewEndTime * time01);
                var scale = Mathf.Lerp(0.24f, 0.115f, t01);

                dot.enabled = true;
                dot.transform.position = new Vector3(point.x, point.y, -0.18f);
                dot.transform.localScale = new Vector3(scale, scale, 1f);
                dot.color = Color.white;
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
            var start = firePoint + (_aimState.Direction * 0.45f);
            var velocity = _aimState.Direction * _aimState.Power;
            var gravity = Physics2D.gravity * weapon.GravityScale;
            return start + velocity * time + 0.5f * gravity * (time * time);
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

            SceneManager.LoadScene(GameSceneNames.Menu);
        }

        private void ExitToMenu()
        {
            SceneManager.LoadScene(GameSceneNames.Menu);
        }

        private void CancelAim()
        {
            if (!IsBattleReady() || _simulation.Result.IsFinished || HasProjectilesInFlight || HasActiveMotion)
            {
                return;
            }

            _aimState = new AimState(Vector2.zero, Vector2.right, _aimSettings.MinPower, false);
            _aimDragActive = false;
            _aimLine.enabled = false;
            HideTrajectoryPreview();
            _status = GameText.AimReset();
        }

        private void RenderHud()
        {
            if (_hud == null || !IsBattleReady())
            {
                return;
            }

            var weapon = _inventory.Selected;
            var playerHasTime = _simulation.MovementTimeRemaining > 0f;
            var canFire = !_simulation.Result.IsFinished
                && !HasProjectilesInFlight
                && !HasActiveMotion
                && playerHasTime
                && _inventory.CanFireSelected
                && _aimState.HasInput;
            var guideActive = _firstBattleGuide != null && _firstBattleGuide.IsActive;
            var status = guideActive
                ? _firstBattleGuide.Message
                : _coachTimer > 0f && !string.IsNullOrEmpty(_coachMessage) ? _coachMessage : _status;
            var tutorialTargetId = guideActive
                ? _firstBattleGuide.TargetId
                : _coachTimer > 0f ? _coachTargetId : string.Empty;
            var canUseMovement = _simulation.CanAcceptMovementInput();
            var mobilityLabel = GameText.MobilityLabel(_simulation.MovementTimeRemaining);

            var state = new BattleHudState(
                status,
                _battleState.Turn.TurnNumber,
                DescribeTeam(_battleState.Turn.ActiveTeam),
                _simulation.MovementTimeRemaining,
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
                tutorialTargetId,
                guideActive && _firstBattleGuide.BlocksWeaponSwitch,
                guideActive && _firstBattleGuide.BlocksCancelAim,
                guideActive,
                _aimState.Power,
                _aimSettings.MinPower,
                _aimSettings.MaxPower,
                canFire,
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

        private bool IsWeaponSwitchBlockedByGuide()
        {
            return _firstBattleGuide != null && _firstBattleGuide.IsActive && _firstBattleGuide.BlocksWeaponSwitch;
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
                : arena.ArenaId switch
                {
                    "arena.training-meadow" => "Тренувальна галявина",
                    "arena.split-ridge" => "Розділений хребет",
                    "arena.central-cover" => "Центральне укриття",
                    _ => arena.DisplayName
                };
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
