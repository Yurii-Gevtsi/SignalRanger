using System.Collections.Generic;
using System.Linq;
using Game.Core;
using Game.Core.Battle;
using Game.Core.Localization;
using Game.Gameplay.Battle.Arenas;
using Game.Gameplay.Battle.AI;
using Game.Gameplay.Battle.Domain;
using Game.Gameplay.Battle.Feedback;
using Game.Gameplay.Battle.Motion;
using Game.Gameplay.Battle.Projectiles;
using Game.Gameplay.Battle.Results;
using Game.Gameplay.Battle.Runtime;
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

    public sealed class BattleSceneController : MonoBehaviour, IProjectileSpawner, IProjectileWorld
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

        private readonly Color _playerColor = new(0.18f, 0.56f, 0.96f);
        private readonly Color _opponentColor = new(0.95f, 0.28f, 0.22f);
        private readonly List<BattleUnitPresenter> _unitPresenters = new();
        private readonly List<GameObject> _arenaObjects = new();
        private readonly Dictionary<string, UnitMotor> _unitMotors = new(System.StringComparer.Ordinal);
        private readonly Dictionary<string, WeaponDefinition> _weaponsById = new(System.StringComparer.Ordinal);
        private readonly Dictionary<string, Sprite> _spriteResourceCache = new(System.StringComparer.Ordinal);

        private ArenaDefinition[] _arenas;
        private int _arenaIndex;
        private ArenaDefinition _currentArena;
        private WeaponInventoryRuntime _inventory;
        private WeaponInventoryRuntime _opponentInventory;
        private WeaponExecutor _weaponExecutor;
        private WeaponDefinition _activeShotWeapon;
        private BattleState _battleState;
        private TurnService _turnService;
        private AimSettings _baseAimSettings;
        private AimSettings _aimSettings;
        private AimState _aimState;
        private Camera _camera;
        private Sprite _boxSprite;
        private Sprite _circleSprite;
        private LineRenderer _aimLine;
        private LineRenderer _trajectoryLine;
        private readonly List<SpriteRenderer> _trajectoryDots = new();
        private int _projectilesInFlight;
        private BattleResult _battleResult;
        private BattleResultScreenState _resultScreenState;
        private string _status;
        private IBattleHud _hud;
        private SimpleAiShooter _aiShooter;
        private AiDifficulty _aiDifficulty;
        private CharacterArchetype _playerArchetype;
        private CharacterArchetype _opponentArchetype;
        private BattleVisualTheme _visualTheme;
        private BattleTerrain _battleTerrain;
        private FirstBattleGuideState _firstBattleGuide;
        private float _aiDecisionTimer;
        private string _coachMessage = string.Empty;
        private string _coachTargetId = string.Empty;
        private float _coachTimer;
        private bool _aimHadInput;
        private TurnMovementState _playerMovement;
        private int _uiWalkInputDirection;
        private int _keyboardWalkInputDirection;
        private float _simulationAccumulator;
        private bool HasProjectilesInFlight => _projectilesInFlight > 0;

        private bool HasActiveMotion
        {
            get
            {
                foreach (var presenter in _unitPresenters)
                {
                    if (presenter.State.IsDefeated)
                    {
                        continue;
                    }

                    if (_unitMotors.TryGetValue(presenter.State.UnitId, out var motor) && !motor.IsStable)
                    {
                        return true;
                    }
                }

                return false;
            }
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

            _weaponExecutor = new WeaponExecutor();
            _weaponExecutor.RegisterEffect(WeaponRole.ShortRangeBurst, new BurstProjectileEffect());
            _weaponExecutor.RegisterEffect("weapon.scatter-arc", new BurstProjectileEffect(3, 20f));
            _aiShooter = new SimpleAiShooter();
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
            _hud.CancelRequested += CancelAim;
            _hud.NextWeaponRequested += () => SelectWeapon(_inventory.SelectedIndex + 1);
            _hud.MoveLeftStarted += () => SetWalkInput(-1);
            _hud.MoveLeftStopped += StopWalkInput;
            _hud.MoveRightStarted += () => SetWalkInput(1);
            _hud.MoveRightStopped += StopWalkInput;
            _hud.JumpForwardRequested += () => TryStartJump(true);
            _hud.JumpBackwardRequested += () => TryStartJump(false);
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

            if (Keyboard.current != null && Keyboard.current.rKey.wasPressedThisFrame)
            {
                RestartScene();
                return;
            }

            if (_battleResult.IsFinished)
            {
                return;
            }

            if (HasActiveMotion)
            {
                UpdateUnitPhysics();
                return;
            }

            if (_battleState.Turn.ActiveTeam == BattleTeamId.Opponent)
            {
                UpdateOpponentAi();
                return;
            }

            if (UpdatePlayerTurnTimer())
            {
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

            if (!_battleResult.IsFinished && !HasProjectilesInFlight && Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
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
                    TryStartJump(false);
                    return;
                }

                if (!HasProjectilesInFlight && Keyboard.current.eKey.wasPressedThisFrame)
                {
                    TryStartJump(true);
                    return;
                }
            }

            if (HasProjectilesInFlight)
            {
                return;
            }

            UpdateWalking();
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

            _turnService = new TurnService();
            _turnService.StartFirstTurn(_battleState, BattleTeamId.Player);
            _aimState = new AimState(Vector2.zero, Vector2.right, _aimSettings.MinPower, false);
            _battleResult = BattleResult.InProgress;
            _resultScreenState = default;
            _projectilesInFlight = 0;
            _aiDecisionTimer = 0f;
            _status = GameText.BattleIntro(DescribeArena(_currentArena));
            _aimLine.enabled = false;
            HideTrajectoryPreview();
            _aimHadInput = false;
            _uiWalkInputDirection = 0;
            _keyboardWalkInputDirection = 0;
            _simulationAccumulator = 0f;
            ResetPlayerMovementTurn();
            RefreshUnitViews();
            ReportTutorial(BattleTutorialBridge.MissionStart);
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
            _status = _currentArena != null
                ? GameText.WeaponSelected(DescribeWeapon(weapon))
                : GameText.WeaponSelectedGeneric();
        }

        private void ApplyArenaCamera(ArenaDefinition arena)
        {
            _camera.transform.position = arena.CameraPosition;
            _camera.orthographicSize = arena.CameraSize;
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
            var minY = Mathf.Min(arena.CameraPosition.y - cameraHeight * 0.5f, FallDeathY) - margin;
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
            _playerMovement = new TurnMovementState(PlayerTurnDurationSeconds);
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
                    FallDeathY);
            }
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

        private UnitMotor GetMotor(BattleUnitPresenter presenter)
        {
            return presenter != null && _unitMotors.TryGetValue(presenter.State.UnitId, out var motor)
                ? motor
                : null;
        }

        private void ClearArenaRuntime()
        {
            _battleTerrain?.Dispose();
            _battleTerrain = null;
            _unitMotors.Clear();
            _simulationAccumulator = 0f;

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
            _battleResult = BattleResult.InProgress;
            _resultScreenState = default;
            _projectilesInFlight = 0;
        }

        private BattleUnitView CreateUnitView(BattleUnitState state)
        {
            var bodyHeight = ArchetypeFor(state.TeamId).BodyHeight;
            var unitObject = new GameObject(state.UnitId);
            unitObject.transform.position = state.Position;

            var skin = GetUnitSkin(state.TeamId);
            if (skin == null)
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
            view.SetPreserveSpriteColor(skin != null);
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
            var activePresenter = GetActivePresenter();
            if (activePresenter == null || !TryGetPointerWorldPosition(out var pointerWorld))
            {
                return;
            }

            _aimState = AimCalculator.Calculate(activePresenter.View.FirePoint.position, pointerWorld, _aimSettings);
            _firstBattleGuide?.ObserveAimState(_aimState.HasInput);
            if (_aimState.HasInput && !_aimHadInput)
            {
                ReportTutorial(BattleTutorialBridge.AimStarted);
                EmitFeedback(BattleFeedbackEventType.AimStarted);
            }

            _aimHadInput = _aimState.HasInput;
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

        private bool TryGetPointerWorldPosition(out Vector2 pointerWorld)
        {
            pointerWorld = default;
            if (_camera == null)
            {
                return false;
            }

            if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed)
            {
                if (IsPointerOverUi((int)Touchscreen.current.primaryTouch.touchId.ReadValue()))
                {
                    return false;
                }

                pointerWorld = _camera.ScreenToWorldPoint(Touchscreen.current.primaryTouch.position.ReadValue());
                return true;
            }

            if (Mouse.current != null)
            {
                if (!Mouse.current.leftButton.isPressed)
                {
                    return false;
                }

                if (IsPointerOverUi())
                {
                    return false;
                }

                pointerWorld = _camera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
                return true;
            }

            return false;
        }

        private void Fire()
        {
            if (!IsBattleReady() || _battleResult.IsFinished || HasProjectilesInFlight || HasActiveMotion)
            {
                return;
            }

            var activePresenter = GetActivePresenter();
            if (activePresenter == null)
            {
                HideTrajectoryPreview();
                return;
            }

            if (activePresenter.State.TeamId == BattleTeamId.Player
                && _playerMovement != null
                && !_playerMovement.HasTimeRemaining)
            {
                EndPlayerTurnFromTimer();
                return;
            }

            if (!_aimState.HasInput)
            {
                _status = GameText.AimRequired();
                return;
            }

            var activeInventory = GetInventoryFor(activePresenter.State.TeamId);
            var weapon = activeInventory.Selected;
            if (weapon.Role == WeaponRole.ShortRangeBurst)
            {
                UseCloseRangeBurst(activePresenter, activeInventory, weapon);
                return;
            }

            var isUtilityWeapon = IsInstantUtility(weapon.Role);
            if (isUtilityWeapon)
            {
                UseInstantUtility(activePresenter, activeInventory, weapon);
                return;
            }

            var context = new WeaponFireContext(
                weapon,
                activePresenter.State.UnitId,
                activePresenter.View.FirePoint.position,
                _aimState.Direction,
                _aimState.Power,
                activePresenter.View.GetComponent<Collider2D>(),
                this);

            var result = _weaponExecutor.TryFireSelected(activeInventory, context);
            if (result != WeaponFireResult.Fired)
            {
                _status = result == WeaponFireResult.OutOfAmmo
                    ? GameText.AmmoDepleted(DescribeWeapon(weapon))
                    : GameText.NoWeaponSelected();
                return;
            }

            _activeShotWeapon = weapon;
            _projectilesInFlight = Mathf.Max(1, _projectilesInFlight);
            ClearWalkInput();
            _turnService.SpendAction(_battleState);
            _status = GameText.ShotStatus(DescribeTeam(activePresenter.State.TeamId), DescribeWeapon(weapon));
            _aimLine.enabled = false;
            HideTrajectoryPreview();
            if (activePresenter.State.TeamId == BattleTeamId.Player)
            {
                _firstBattleGuide?.ObservePlayerShotFired();
            }

            EmitFeedback(BattleFeedbackEventType.ShotFired);

            if (activePresenter.State.TeamId == BattleTeamId.Player)
            {
                ReportTutorial(BattleTutorialBridge.ShotFired);
            }
        }

        void IProjectileSpawner.Spawn(in ProjectileSpawnRequest request)
        {
            _projectilesInFlight++;
            var projectileObject = new GameObject("Production Shell");
            _arenaObjects.Add(projectileObject);
            projectileObject.transform.position = request.Position;

            var visualObject = new GameObject("Visual");
            visualObject.transform.SetParent(projectileObject.transform, false);
            ApplyProjectileVisualTransform(visualObject.transform, request);

            var renderer = visualObject.AddComponent<SpriteRenderer>();
            renderer.sprite = _circleSprite;
            renderer.color = ResolveProjectileVisualColor(request);
            renderer.sortingOrder = 30;

            var projectile = projectileObject.AddComponent<ProjectileController>();
            projectile.Impacted += OnProjectileImpacted;
            projectile.Launch(
                request.Definition,
                request.OwnerUnitId,
                request.Velocity,
                this);
        }

        private void ApplyProjectileVisualTransform(Transform projectileTransform, ProjectileSpawnRequest request)
        {
            var weapon = ResolveWeaponFromProjectileId(request.Definition.ProjectileId);
            var diameter = request.VisualDiameter;
            if (weapon == null)
            {
                projectileTransform.localScale = Vector3.one * diameter;
                return;
            }

            switch (weapon.WeaponId)
            {
                case "weapon.long-lance":
                    projectileTransform.localScale = new Vector3(diameter * 2.6f, diameter * 0.55f, 1f);
                    projectileTransform.rotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(request.Velocity.y, request.Velocity.x) * Mathf.Rad2Deg);
                    break;
                case "weapon.pulse-shell":
                    projectileTransform.localScale = Vector3.one * (diameter * 1.25f);
                    break;
                case "weapon.scatter-arc":
                    projectileTransform.localScale = Vector3.one * (diameter * 0.82f);
                    break;
                case "weapon.rampart-kit":
                    projectileTransform.localScale = new Vector3(diameter * 2.15f, diameter * 0.7f, 1f);
                    projectileTransform.rotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(request.Velocity.y, request.Velocity.x) * Mathf.Rad2Deg);
                    break;
                case "weapon.relay-step":
                    projectileTransform.localScale = Vector3.one * (diameter * 0.9f);
                    break;
                default:
                    projectileTransform.localScale = Vector3.one * diameter;
                    break;
            }
        }

        private Color ResolveProjectileVisualColor(ProjectileSpawnRequest request)
        {
            var weapon = ResolveWeaponFromProjectileId(request.Definition.ProjectileId);
            if (weapon == null)
            {
                return request.Color;
            }

            return weapon.WeaponId switch
            {
                "weapon.steady-orb" => new Color(0.36f, 0.78f, 1f, 0.95f),
                "weapon.pulse-shell" => new Color(1f, 0.49f, 0.18f, 0.96f),
                "weapon.long-lance" => new Color(0.92f, 0.98f, 1f, 0.98f),
                "weapon.scatter-arc" => new Color(1f, 0.84f, 0.32f, 0.94f),
                "weapon.rampart-kit" => new Color(0.54f, 0.9f, 0.55f, 0.96f),
                "weapon.relay-step" => new Color(0.68f, 0.46f, 1f, 0.94f),
                _ => request.Color
            };
        }

        private void OnProjectileImpacted(ProjectileImpact impact)
        {
            _projectilesInFlight = Mathf.Max(0, _projectilesInFlight - 1);
            SpawnImpactFlash(impact.Point);

            var impactWeapon = ResolveImpactWeapon(impact);
            ResolveImpact(impact, impactWeapon);
            RefreshUnitViews();
            ReportTutorial(BattleTutorialBridge.ProjectileImpact);
            if (!string.IsNullOrWhiteSpace(impact.OwnerUnitId)
                && impact.OwnerUnitId.StartsWith("player.", System.StringComparison.Ordinal))
            {
                _firstBattleGuide?.ObserveProjectileImpact();
            }

            EmitFeedback(BattleFeedbackEventType.ProjectileImpact);

            if (HasProjectilesInFlight)
            {
                return;
            }

            _battleResult = BattleResultEvaluator.Evaluate(_battleState);
            if (_battleResult.IsFinished)
            {
                EmitFeedback(BattleFeedbackEventType.UnitDefeated);
                EmitFeedback(BattleFeedbackEventType.BattleFinished);
                _resultScreenState = CreateResultScreenState(_battleResult, _battleState.Turn.TurnNumber);
                _status = GameText.ResultPrompt(_resultScreenState.Title);
                if (_battleResult.Outcome == BattleOutcome.PlayerWon)
                {
                    ReportTutorial(BattleTutorialBridge.BattleWon);
                }

                _firstBattleGuide?.ObserveBattleFinished();

                return;
            }

            _turnService.AdvanceTurn(_battleState);
            _firstBattleGuide?.ObserveTurnAdvanced(_battleState.Turn.ActiveTeam);
            ApplyAimSettingsForActiveTeam();
            _aimState = new AimState(Vector2.zero, Vector2.right, _aimSettings.MinPower, false);
            _aiDecisionTimer = 0f;
            ResetPlayerMovementTurn();
            _aimLine.enabled = false;
            HideTrajectoryPreview();
            _status = GameText.TurnStatus(DescribeTeam(_battleState.Turn.ActiveTeam));
            RefreshUnitViews();
        }

        private WeaponDefinition ResolveImpactWeapon(ProjectileImpact impact)
        {
            var weaponFromProjectile = ResolveWeaponFromProjectileId(impact.ProjectileId);
            return weaponFromProjectile ?? _activeShotWeapon ?? _inventory.Selected;
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

        private void ResolveImpact(ProjectileImpact impact, WeaponDefinition impactWeapon)
        {
            if (impactWeapon == null)
            {
                return;
            }

            switch (impactWeapon.Role)
            {
                case WeaponRole.CoverTool:
                    DeployCover(impact.Point, impactWeapon);
                    break;
                case WeaponRole.DefensiveTool:
                    RestoreFriendlyHealth(impact.Point, impact.OwnerUnitId, impactWeapon);
                    break;
                case WeaponRole.MovementTool:
                    RepositionOwner(impact.Point, impact.OwnerUnitId, impactWeapon);
                    break;
                default:
                    ResolveDamageImpact(impact, impactWeapon);
                    break;
            }
        }

        private void ResolveDamageImpact(ProjectileImpact impact, WeaponDefinition impactWeapon)
        {
            var terrainChanged = ApplyTerrainDestruction(impact.Point, impactWeapon);

            var allUnits = _battleState.Teams.SelectMany(team => team.Units).ToList();
            var selfDamageMultiplier = impactWeapon.Role == WeaponRole.ShortRangeBurst ? 0f : 0.65f;
            var settings = new RadialDamageSettings(impact.Damage, impact.ImpactRadius, selfDamageMultiplier);
            DamageResolver.ResolveRadialDamage(allUnits, impact.Point, impact.OwnerUnitId, impact.ProjectileId, settings);
            ApplyExplosionKnockback(impact.Point, impact.Velocity, impactWeapon, terrainChanged);
        }

        private bool ApplyTerrainDestruction(Vector2 impactPoint, WeaponDefinition impactWeapon)
        {
            if (_battleTerrain == null || impactWeapon == null)
            {
                return false;
            }

            var craterRadius = Mathf.Clamp(impactWeapon.ImpactRadius * 0.62f, 0.38f, 1.55f);
            return _battleTerrain.ApplyCrater(impactPoint, craterRadius);
        }

        private void ApplyExplosionKnockback(Vector2 impactPoint, Vector2 impactVelocity, WeaponDefinition impactWeapon, bool terrainChanged)
        {
            if (_battleState == null || impactWeapon == null)
            {
                return;
            }

            var radius = Mathf.Max(0.65f, impactWeapon.ImpactRadius + KnockbackLandingSearchPadding);
            var radiusScale = Mathf.InverseLerp(0.75f, 2.4f, impactWeapon.ImpactRadius);
            var incoming = impactVelocity.sqrMagnitude > 0.01f ? impactVelocity.normalized : Vector2.zero;
            var downwardImpact01 = Mathf.InverseLerp(-0.15f, -0.85f, incoming.y);

            foreach (var presenter in _unitPresenters)
            {
                var unit = presenter.State;
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

                GetMotor(presenter)?.AddImpulse(velocity);
            }

            if (!terrainChanged)
            {
                return;
            }

            // The crater may have removed the ground under units the blast
            // itself barely touched — let them re-check their footing.
            foreach (var presenter in _unitPresenters)
            {
                if (!presenter.State.IsDefeated)
                {
                    GetMotor(presenter)?.WakeIfUnsupported();
                }
            }
        }

        private void UseInstantUtility(BattleUnitPresenter activePresenter, WeaponInventoryRuntime inventory, WeaponDefinition weapon)
        {
            if (!inventory.TryConsumeSelectedAmmo())
            {
                _status = GameText.AmmoDepleted(DescribeWeapon(weapon));
                return;
            }

            _activeShotWeapon = weapon;
            ClearWalkInput();
            _turnService.SpendAction(_battleState);
            if (weapon.Role == WeaponRole.CoverTool)
            {
                var origin = (Vector2)activePresenter.View.FirePoint.position;
                DeployCover(GetCoverPlacementPoint(origin), weapon);
            }
            else if (weapon.Role == WeaponRole.DefensiveTool)
            {
                RestoreFriendlyHealth(activePresenter.State.Position, activePresenter.State.UnitId, weapon);
            }

            _aimLine.enabled = false;
            HideTrajectoryPreview();
            EmitFeedback(BattleFeedbackEventType.ShotFired);
            CompleteInstantAction(_status);
        }

        private void UseCloseRangeBurst(BattleUnitPresenter activePresenter, WeaponInventoryRuntime inventory, WeaponDefinition weapon)
        {
            if (!inventory.TryConsumeSelectedAmmo())
            {
                _status = GameText.AmmoDepleted(DescribeWeapon(weapon));
                return;
            }

            _activeShotWeapon = weapon;
            ClearWalkInput();
            _turnService.SpendAction(_battleState);

            var origin = (Vector2)activePresenter.View.FirePoint.position;
            var direction = _aimState.Direction.sqrMagnitude > 0.01f ? _aimState.Direction.normalized : Vector2.right;
            var range = Mathf.Max(1.2f, weapon.ImpactRadius * 1.75f);
            var hitCount = 0;
            var targetTeam = activePresenter.State.TeamId == BattleTeamId.Player ? BattleTeamId.Opponent : BattleTeamId.Player;

            foreach (var unit in _battleState.GetTeam(targetTeam).Units)
            {
                if (unit == null || unit.IsDefeated)
                {
                    continue;
                }

                var toUnit = unit.Position - origin;
                if (toUnit.magnitude > range || Vector2.Dot(direction, toUnit.normalized) < 0.25f)
                {
                    continue;
                }

                unit.ApplyDamage(weapon.Damage, weapon.WeaponId);
                hitCount++;
            }

            var burstPoint = origin + direction * Mathf.Min(range, 0.95f);
            var terrainChanged = ApplyTerrainDestruction(burstPoint, weapon);
            ApplyExplosionKnockback(burstPoint, direction * 8f, weapon, terrainChanged);
            SpawnImpactFlash(burstPoint, new Color(1f, 0.62f, 0.24f, 0.85f), 0.95f);
            _aimLine.enabled = false;
            HideTrajectoryPreview();
            EmitFeedback(BattleFeedbackEventType.ShotFired);
            if (hitCount > 0)
            {
                EmitFeedback(BattleFeedbackEventType.DamageResolved);
            }

            _status = GameText.ShotStatus(DescribeTeam(activePresenter.State.TeamId), DescribeWeapon(weapon));
            CompleteInstantAction(_status);
        }

        private void CompleteInstantAction(string actionStatus)
        {
            RefreshUnitViews();
            _battleResult = BattleResultEvaluator.Evaluate(_battleState);
            if (_battleResult.IsFinished)
            {
                EmitFeedback(BattleFeedbackEventType.BattleFinished);
                _resultScreenState = CreateResultScreenState(_battleResult, _battleState.Turn.TurnNumber);
                _status = GameText.ResultPrompt(_resultScreenState.Title);
                _firstBattleGuide?.ObserveBattleFinished();
                return;
            }

            _turnService.AdvanceTurn(_battleState);
            _firstBattleGuide?.ObserveTurnAdvanced(_battleState.Turn.ActiveTeam);
            ApplyAimSettingsForActiveTeam();
            _aimState = new AimState(Vector2.zero, Vector2.right, _aimSettings.MinPower, false);
            _aiDecisionTimer = 0f;
            ResetPlayerMovementTurn();
            _status = $"{actionStatus} {GameText.TurnStatus(DescribeTeam(_battleState.Turn.ActiveTeam))}";
            RefreshUnitViews();
        }

        private void DeployCover(Vector2 impactPoint, WeaponDefinition impactWeapon)
        {
            var width = Mathf.Clamp(impactWeapon.ImpactRadius * 1.2f, 1.15f, 2.1f);
            const float height = 0.6f;
            var position = impactPoint + Vector2.up * (height * 0.5f);
            var cover = CreateBox(
                $"Cover {impactWeapon.WeaponId}",
                position,
                new Vector2(width, height),
                ProjectileWeaponEffect.GetProjectileColor(impactWeapon.Role),
                18);
            _arenaObjects.Add(cover);
            _battleTerrain?.StampSolidRect(position, new Vector2(width, height));
            _status = GameText.CoverDeployed(DescribeWeapon(impactWeapon));
        }

        private void RestoreFriendlyHealth(Vector2 impactPoint, string ownerUnitId, WeaponDefinition impactWeapon)
        {
            if (string.IsNullOrWhiteSpace(ownerUnitId))
            {
                return;
            }

            var owner = _battleState.GetUnit(ownerUnitId);
            var restoredHealth = 0;
            foreach (var unit in _battleState.GetTeam(owner.TeamId).Units)
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

            SpawnImpactFlash(impactPoint, new Color(0.26f, 0.84f, 0.62f, 0.85f), 1.45f);
            _status = GameText.ShieldPulse(DescribeTeam(owner.TeamId), restoredHealth);
        }

        private void RepositionOwner(Vector2 impactPoint, string ownerUnitId, WeaponDefinition impactWeapon)
        {
            if (string.IsNullOrWhiteSpace(ownerUnitId))
            {
                return;
            }

            var owner = _battleState.GetUnit(ownerUnitId);
            var target = ResolveUnitLandingPosition(impactPoint, owner.TeamId);
            var maxDistance = impactWeapon.Role == WeaponRole.MovementTool ? float.PositiveInfinity : impactWeapon.ImpactRadius;
            var deltaX = Mathf.Clamp(target.x - owner.Position.x, -maxDistance, maxDistance);
            if (Mathf.Abs(deltaX) < 0.1f)
            {
                _status = GameText.RepositionFailed();
                return;
            }

            owner.MoveTo(ResolveUnitLandingPosition(new Vector2(owner.Position.x + deltaX, target.y), owner.TeamId));
            if (_unitMotors.TryGetValue(owner.UnitId, out var ownerMotor))
            {
                ownerMotor.Teleport(owner.Position);
            }

            SpawnImpactFlash(owner.Position, new Color(0.66f, 0.42f, 0.96f, 0.8f), 1.05f);
            _status = GameText.Repositioned(DescribeTeam(owner.TeamId));
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

        private void ResetPlayerMovementTurn()
        {
            _uiWalkInputDirection = 0;
            _keyboardWalkInputDirection = 0;
            foreach (var motor in _unitMotors.Values)
            {
                motor.ResetWalk();
            }

            _playerMovement?.Reset(PlayerTurnDurationSeconds);
        }

        private bool UpdatePlayerTurnTimer()
        {
            if (_playerMovement == null
                || _battleState.Turn.ActiveTeam != BattleTeamId.Player
                || _battleState.Turn.ActionSpent
                || HasProjectilesInFlight
                || _battleResult.IsFinished)
            {
                return false;
            }

            _playerMovement.Tick(Time.deltaTime);
            if (_playerMovement.HasTimeRemaining || HasActiveMotion)
            {
                return false;
            }

            EndPlayerTurnFromTimer();
            return true;
        }

        private void EndPlayerTurnFromTimer()
        {
            ClearWalkInput();
            _aimState = new AimState(Vector2.zero, Vector2.right, _aimSettings.MinPower, false);
            _aimLine.enabled = false;
            HideTrajectoryPreview();
            _turnService.AdvanceTurn(_battleState);
            _firstBattleGuide?.ObserveTurnAdvanced(_battleState.Turn.ActiveTeam);
            ApplyAimSettingsForActiveTeam();
            _aiDecisionTimer = 0f;
            ResetPlayerMovementTurn();
            _status = GameText.TurnStatus(DescribeTeam(_battleState.Turn.ActiveTeam));
            RefreshUnitViews();
        }

        private void SetWalkInput(int direction)
        {
            if (!CanUseMovement())
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

        private void StopWalkInput()
        {
            _uiWalkInputDirection = 0;
        }

        private void SetKeyboardWalkInput(int direction)
        {
            _keyboardWalkInputDirection = CanUseMovement() ? NormalizeDirection(direction) : 0;
        }

        private static int NormalizeDirection(int direction)
        {
            return direction < 0 ? -1 : direction > 0 ? 1 : 0;
        }

        private bool CanUseMovement()
        {
            return IsBattleReady()
                && !_battleResult.IsFinished
                && !HasProjectilesInFlight
                && !HasActiveMotion
                && _battleState.Turn.ActiveTeam == BattleTeamId.Player
                && !_battleState.Turn.ActionSpent
                && (_playerMovement == null || _playerMovement.HasTimeRemaining)
                && !IsMovementBlockedByGuide();
        }

        private bool IsMovementBlockedByGuide()
        {
            return _firstBattleGuide != null && _firstBattleGuide.IsActive;
        }

        private void UpdateWalking()
        {
            var walkDirection = _keyboardWalkInputDirection != 0 ? _keyboardWalkInputDirection : _uiWalkInputDirection;
            if (!CanUseMovement() || walkDirection == 0)
            {
                return;
            }

            var presenter = GetActivePresenter();
            if (presenter == null || presenter.State.TeamId != BattleTeamId.Player || _playerMovement == null)
            {
                return;
            }

            var motor = GetMotor(presenter);
            if (motor == null)
            {
                return;
            }

            var outcome = motor.Walk(walkDirection, Time.deltaTime);
            if (outcome == WalkOutcome.None)
            {
                return;
            }

            presenter.State.SetFacing(walkDirection);
            presenter.State.MoveTo(motor.Position);
            presenter.Refresh(true);

            switch (outcome)
            {
                case WalkOutcome.Moved:
                    _status = GameText.Moved(DescribeTeam(BattleTeamId.Player));
                    break;
                case WalkOutcome.Blocked:
                    _status = GameText.MovementBlocked();
                    ClearWalkInput();
                    break;
                case WalkOutcome.StartedFalling:
                    // Regular physics takes over from here (HasActiveMotion).
                    ClearWalkInput();
                    break;
            }
        }

        private void ClearWalkInput()
        {
            _uiWalkInputDirection = 0;
            _keyboardWalkInputDirection = 0;
        }

        private void TryStartJump(bool forward)
        {
            if (!CanUseMovement())
            {
                if (IsMovementBlockedByGuide())
                {
                    _status = GameText.MovementGuideBlocked();
                }

                return;
            }

            if (_playerMovement == null || !_playerMovement.HasTimeRemaining)
            {
                return;
            }

            var presenter = GetActivePresenter();
            if (presenter == null || presenter.State.TeamId != BattleTeamId.Player)
            {
                return;
            }

            StartJumpAttempt(presenter, forward);
        }

        private void StartJumpAttempt(BattleUnitPresenter presenter, bool forward)
        {
            var motor = GetMotor(presenter);
            if (motor == null || !motor.IsStable)
            {
                return;
            }

            var archetype = ArchetypeFor(presenter.State.TeamId);
            var direction = presenter.State.FacingDirectionX;
            var horizontalDistance = (forward ? archetype.FrontJumpDistance : -archetype.BackJumpDistance) * direction;
            var apexHeight = (forward ? archetype.FrontJumpHeight : archetype.BackJumpHeight) + 0.2f;
            motor.StartJump(horizontalDistance, apexHeight);
            _status = forward
                ? GameText.FrontJumped(DescribeTeam(presenter.State.TeamId))
                : GameText.BackJumped(DescribeTeam(presenter.State.TeamId));
        }

        private void UpdateUnitPhysics()
        {
            _simulationAccumulator += Mathf.Min(Time.deltaTime, 0.1f);
            while (_simulationAccumulator >= SimulationTickSeconds)
            {
                _simulationAccumulator -= SimulationTickSeconds;
                foreach (var presenter in _unitPresenters)
                {
                    var unit = presenter.State;
                    if (unit.IsDefeated || !_unitMotors.TryGetValue(unit.UnitId, out var motor) || motor.IsStable)
                    {
                        continue;
                    }

                    var outcome = motor.Step(SimulationTickSeconds);
                    unit.MoveTo(motor.Position);
                    if (outcome == MotorStepOutcome.FellOutOfBounds)
                    {
                        unit.MoveTo(new Vector2(motor.Position.x, FallDeathY));
                        motor.Teleport(unit.Position);
                        unit.ApplyDamage(unit.MaxHealth, "fall.out-of-bounds");
                        EmitFeedback(BattleFeedbackEventType.UnitDefeated);
                    }
                    else if (outcome == MotorStepOutcome.Landed)
                    {
                        NudgeApartFromOthers(presenter, motor);
                    }
                }
            }

            foreach (var presenter in _unitPresenters)
            {
                presenter.Refresh(presenter.State.UnitId == _battleState.Turn.ActiveUnitId);
            }

            if (HasActiveMotion)
            {
                return;
            }

            _simulationAccumulator = 0f;
            ResolveBattleAfterMotion();
        }

        private void NudgeApartFromOthers(BattleUnitPresenter presenter, UnitMotor motor)
        {
            foreach (var other in _unitPresenters)
            {
                var otherUnit = other.State;
                if (other == presenter || otherUnit.IsDefeated)
                {
                    continue;
                }

                var deltaX = motor.Position.x - otherUnit.Position.x;
                var verticalOverlap = Mathf.Abs(motor.Position.y - otherUnit.Position.y)
                    < ArchetypeFor(otherUnit.TeamId).BodyHeight * 0.55f;
                if (Mathf.Abs(deltaX) >= OccupiedRadius || !verticalOverlap)
                {
                    continue;
                }

                var pushDirection = deltaX >= 0f ? 1f : -1f;
                var desiredX = otherUnit.Position.x + pushDirection * OccupiedRadius;
                if (TryResolveGroundPosition(new Vector2(desiredX, motor.Position.y), presenter.State.TeamId, out var freeSpot)
                    && Mathf.Abs(freeSpot.y - motor.Position.y) <= 0.6f)
                {
                    motor.Teleport(freeSpot);
                    presenter.State.MoveTo(freeSpot);
                }

                return;
            }
        }

        private void ResolveBattleAfterMotion()
        {
            if (_battleState == null || _battleResult.IsFinished)
            {
                return;
            }

            _battleResult = BattleResultEvaluator.Evaluate(_battleState);
            RefreshUnitViews();
            if (!_battleResult.IsFinished)
            {
                return;
            }

            EmitFeedback(BattleFeedbackEventType.BattleFinished);
            _resultScreenState = CreateResultScreenState(_battleResult, _battleState.Turn.TurnNumber);
            _status = GameText.ResultPrompt(_resultScreenState.Title);
            if (_battleResult.Outcome == BattleOutcome.PlayerWon)
            {
                ReportTutorial(BattleTutorialBridge.BattleWon);
            }

            _firstBattleGuide?.ObserveBattleFinished();
        }

        private void RefreshUnitViews()
        {
            foreach (var presenter in _unitPresenters)
            {
                presenter.Refresh(presenter.State.UnitId == _battleState.Turn.ActiveUnitId);
            }
        }

        private void UpdateOpponentAi()
        {
            if (HasProjectilesInFlight)
            {
                return;
            }

            _aiDecisionTimer += Time.deltaTime;
            if (_aiDecisionTimer < 0.55f)
            {
                _status = GameText.OpponentAiming();
                return;
            }

            var activePresenter = GetActivePresenter();
            var targetPresenter = _unitPresenters.FirstOrDefault(presenter =>
                presenter.State.TeamId == BattleTeamId.Player && !presenter.State.IsDefeated);
            if (activePresenter == null || targetPresenter == null)
            {
                return;
            }

            if (!_opponentInventory.TrySelectNextUsable())
            {
                _turnService.AdvanceTurn(_battleState);
                ApplyAimSettingsForActiveTeam();
                _aimState = new AimState(Vector2.zero, Vector2.right, _aimSettings.MinPower, false);
                _aiDecisionTimer = 0f;
                ResetPlayerMovementTurn();
                _status = GameText.TurnStatus(DescribeTeam(_battleState.Turn.ActiveTeam));
                RefreshUnitViews();
                return;
            }

            var origin = (Vector2)activePresenter.View.FirePoint.position;
            var target = (Vector2)targetPresenter.View.transform.position;
            var opponentWeapon = _opponentInventory.Selected;
            _aimSettings = opponentWeapon.CreateAimSettings(_baseAimSettings);
            var plan = _aiShooter.CreateShot(origin, target, _aimSettings, _aiDifficulty, _battleState.Turn.TurnNumber);
            _aimState = new AimState(origin, plan.Direction, plan.Power, true);
            _status = GameText.OpponentFiring();
            Fire();
            _aiDecisionTimer = 0f;
        }

        private WeaponInventoryRuntime GetInventoryFor(BattleTeamId teamId)
        {
            return teamId == BattleTeamId.Opponent ? _opponentInventory : _inventory;
        }

        private static bool IsInstantUtility(WeaponRole role)
        {
            return role == WeaponRole.CoverTool
                || role == WeaponRole.DefensiveTool;
        }

        private void ApplyAimSettingsForActiveTeam()
        {
            var activeInventory = GetInventoryFor(_battleState.Turn.ActiveTeam);
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
            var dotColor = Color.Lerp(new Color(0.28f, 1f, 0.12f, 0.98f), new Color(0.98f, 0.05f, 0.02f, 0.98f), power01);

            for (var i = 0; i < _trajectoryDots.Count; i++)
            {
                var dot = _trajectoryDots[i];
                var t01 = _trajectoryDots.Count <= 1 ? 0f : (float)i / (_trajectoryDots.Count - 1);
                var time01 = Mathf.Pow(t01, 0.86f);
                var point = GetTrajectoryPreviewPointAtTime(firePoint, weapon, previewEndTime * time01);
                var scale = Mathf.Lerp(0.24f, 0.115f, t01);
                var alpha = Mathf.Lerp(1f, 0.62f, t01);

                dot.enabled = true;
                dot.transform.position = new Vector3(point.x, point.y, -0.18f);
                dot.transform.localScale = new Vector3(scale, scale, 1f);
                dot.color = new Color(dotColor.r, dotColor.g, dotColor.b, alpha);
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

            var cameraHeight = _camera.orthographicSize * 2f;
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
            if (!IsBattleReady() || !_battleResult.IsFinished)
            {
                return;
            }

            // Win: report completion so the menu advances. Loss: just return to menu.
            if (_battleResult.Outcome == BattleOutcome.PlayerWon)
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
            if (!IsBattleReady() || _battleResult.IsFinished || HasProjectilesInFlight || HasActiveMotion)
            {
                return;
            }

            _aimState = new AimState(Vector2.zero, Vector2.right, _aimSettings.MinPower, false);
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
            var playerHasTime = _playerMovement == null || _playerMovement.HasTimeRemaining;
            var canFire = !_battleResult.IsFinished
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
            var canUseMovement = !_battleResult.IsFinished
                && !HasProjectilesInFlight
                && !HasActiveMotion
                && _battleState.Turn.ActiveTeam == BattleTeamId.Player
                && !_battleState.Turn.ActionSpent
                && (_playerMovement == null || _playerMovement.HasTimeRemaining);
            var mobilityLabel = _playerMovement != null
                ? GameText.MobilityLabel(_playerMovement.RemainingTurnSeconds)
                : string.Empty;

            var state = new BattleHudState(
                status,
                _battleState.Turn.TurnNumber,
                DescribeTeam(_battleState.Turn.ActiveTeam),
                GetTeamHealth(BattleTeamId.Player),
                GetTeamMaxHealth(BattleTeamId.Player),
                GetTeamHealth(BattleTeamId.Opponent),
                GetTeamMaxHealth(BattleTeamId.Opponent),
                GameText.WeaponSlot(DescribeWeapon(weapon), _inventory.SelectedIndex + 1, _inventory.Count),
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
                canUseMovement,
                canUseMovement,
                canUseMovement,
                _battleResult.IsFinished,
                _resultScreenState.Title,
                _resultScreenState.Subtitle,
                _battleResult.IsFinished ? DescribeOutcome(_resultScreenState.Result.Outcome) : string.Empty,
                _battleResult.IsFinished ? GameText.Reward(_resultScreenState.RewardPreview.SoftCurrency) : string.Empty,
                _battleResult.IsFinished && _resultScreenState.RewardPreview.HasUnlock ? _resultScreenState.RewardPreview.UnlockText : GameText.NoUnlocks,
                _battleResult.IsFinished && _resultScreenState.CanContinue,
                _battleResult.IsFinished ? _resultScreenState.RetryLabel : GameText.Restart,
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
                && _turnService != null
                && _aimLine != null
                && _trajectoryLine != null;
        }

        float IProjectileWorld.KillY => FallDeathY - 2f;

        bool IProjectileWorld.IsSolid(Vector2 point)
        {
            return _battleTerrain != null && _battleTerrain.Grid.IsSolidWorld(point);
        }

        bool IProjectileWorld.TryHitUnit(Vector2 point, float radius, string ignoredUnitId, out Vector2 hitPoint)
        {
            hitPoint = point;
            foreach (var presenter in _unitPresenters)
            {
                var unit = presenter.State;
                if (unit.IsDefeated || unit.UnitId == ignoredUnitId)
                {
                    continue;
                }

                var halfHeight = ArchetypeFor(unit.TeamId).BodyHeight * 0.5f;
                if (Mathf.Abs(point.x - unit.Position.x) <= UnitCollisionHalfWidth + radius
                    && Mathf.Abs(point.y - unit.Position.y) <= halfHeight + radius)
                {
                    hitPoint = unit.Position + (point - unit.Position) * 0.5f;
                    return true;
                }
            }

            return false;
        }
    }
}
