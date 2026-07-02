using System.Collections.Generic;
using System.Linq;
using Game.Core;
using Game.Core.Battle;
using Game.Core.Localization;
using Game.Gameplay.Battle.Arenas;
using Game.Gameplay.Battle.AI;
using Game.Gameplay.Battle.Domain;
using Game.Gameplay.Battle.Feedback;
using Game.Gameplay.Battle.Projectiles;
using Game.Gameplay.Battle.Results;
using Game.Gameplay.Battle.Runtime;
using Game.Gameplay.Battle.Units;
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

    public sealed class BattleSceneController : MonoBehaviour, IProjectileSpawner
    {
        private readonly Color _playerColor = new(0.18f, 0.56f, 0.96f);
        private readonly Color _opponentColor = new(0.95f, 0.28f, 0.22f);
        private readonly List<BattleUnitPresenter> _unitPresenters = new();
        private readonly List<GameObject> _arenaObjects = new();
        private readonly Dictionary<string, WeaponDefinition> _weaponsById = new(System.StringComparer.Ordinal);

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
        private int _projectilesInFlight;
        private BattleResult _battleResult;
        private BattleResultScreenState _resultScreenState;
        private string _status;
        private IBattleHud _hud;
        private SimpleAiShooter _aiShooter;
        private AiDifficulty _aiDifficulty;
        private CharacterArchetype _playerArchetype;
        private CharacterArchetype _opponentArchetype;
        private float _aiDecisionTimer;
        private string _coachMessage = string.Empty;
        private float _coachTimer;
        private bool _aimHadInput;
        private bool HasProjectilesInFlight => _projectilesInFlight > 0;

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

            return new[] { "weapon.steady-orb", "weapon.pulse-shell" }
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

            if (_battleState.Turn.ActiveTeam == BattleTeamId.Opponent)
            {
                UpdateOpponentAi();
                return;
            }

            if (!HasProjectilesInFlight && Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
            {
                CancelAim();
                return;
            }

            if (!HasProjectilesInFlight && Keyboard.current != null && Keyboard.current.nKey.wasPressedThisFrame)
            {
                LoadArena(_arenaIndex + 1);
                return;
            }

            if (!HasProjectilesInFlight && Keyboard.current != null && Keyboard.current.tabKey.wasPressedThisFrame)
            {
                SelectWeapon(_inventory.SelectedIndex + 1);
                return;
            }

            if (!_battleResult.IsFinished && !HasProjectilesInFlight && Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                Fire();
                return;
            }

            if (HasProjectilesInFlight)
            {
                return;
            }

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
            _trajectoryLine.enabled = false;
            _aimHadInput = false;
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
            foreach (var block in arena.TerrainBlocks)
            {
                var blockObject = CreateBox(block.BlockId, block.Position, block.Size, block.Color, block.Solid, block.SortingOrder);
                _arenaObjects.Add(blockObject);
            }
        }

        private void BuildBattleState(ArenaDefinition arena)
        {
            _playerArchetype = PrototypeArchetypeLibrary.PlayerArchetype;
            _opponentArchetype = PrototypeArchetypeLibrary.ForDifficulty(_aiDifficulty);

            var playerUnit = new BattleUnitState($"player.{_playerArchetype.Id}.01", BattleTeamId.Player, arena.PlayerSpawn, _playerArchetype.MaxHealth);
            var opponentUnit = new BattleUnitState($"opponent.{_opponentArchetype.Id}.01", BattleTeamId.Opponent, arena.OpponentSpawn, _opponentArchetype.MaxHealth);

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
            }
        }

        private void ClearArenaRuntime()
        {
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
            unitObject.transform.localScale = new Vector3(0.72f, bodyHeight, 1f);

            var renderer = unitObject.AddComponent<SpriteRenderer>();
            renderer.sprite = _circleSprite;
            renderer.sortingOrder = 20;

            var collider = unitObject.AddComponent<CapsuleCollider2D>();
            collider.size = new Vector2(0.72f, bodyHeight + 0.05f);

            var body = unitObject.AddComponent<Rigidbody2D>();
            body.bodyType = RigidbodyType2D.Kinematic;

            return unitObject.AddComponent<BattleUnitView>();
        }

        private void BuildAimRenderers()
        {
            _aimLine = CreateLine("Aim Line", 0.08f, Color.white);
            _trajectoryLine = CreateLine("Trajectory Preview", 0.035f, new Color(1f, 1f, 1f, 0.55f));
        }

        private void UpdateAim()
        {
            var activePresenter = GetActivePresenter();
            if (activePresenter == null || !TryGetPointerWorldPosition(out var pointerWorld))
            {
                return;
            }

            _aimState = AimCalculator.Calculate(activePresenter.View.FirePoint.position, pointerWorld, _aimSettings);
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

        private void ReportTutorial(string triggerKey)
        {
            var message = BattleTutorialBridge.Report(triggerKey);
            if (!string.IsNullOrEmpty(message))
            {
                _coachMessage = message;
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
            if (!IsBattleReady() || _battleResult.IsFinished || HasProjectilesInFlight)
            {
                return;
            }

            var activePresenter = GetActivePresenter();
            if (activePresenter == null)
            {
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
            _turnService.SpendAction(_battleState);
            _status = GameText.ShotStatus(DescribeTeam(activePresenter.State.TeamId), DescribeWeapon(weapon));
            _aimLine.enabled = false;
            _trajectoryLine.enabled = false;
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
            ApplyProjectileVisualTransform(projectileObject.transform, request);

            var renderer = projectileObject.AddComponent<SpriteRenderer>();
            renderer.sprite = _circleSprite;
            renderer.color = ResolveProjectileVisualColor(request);
            renderer.sortingOrder = 30;

            projectileObject.AddComponent<CircleCollider2D>();
            projectileObject.AddComponent<Rigidbody2D>();

            var projectile = projectileObject.AddComponent<ProjectileController>();
            projectile.Impacted += OnProjectileImpacted;
            projectile.Launch(
                request.Definition,
                request.OwnerUnitId,
                request.Velocity,
                request.IgnoredOwnerCollider);
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

                return;
            }

            _turnService.AdvanceTurn(_battleState);
            ApplyAimSettingsForActiveTeam();
            _aimState = new AimState(Vector2.zero, Vector2.right, _aimSettings.MinPower, false);
            _aiDecisionTimer = 0f;
            _aimLine.enabled = false;
            _trajectoryLine.enabled = false;
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
            var allUnits = _battleState.Teams.SelectMany(team => team.Units).ToList();
            var selfDamageMultiplier = impactWeapon.Role == WeaponRole.ShortRangeBurst ? 0f : 0.65f;
            var settings = new RadialDamageSettings(impact.Damage, impact.ImpactRadius, selfDamageMultiplier);
            DamageResolver.ResolveRadialDamage(allUnits, impact.Point, impact.OwnerUnitId, impact.ProjectileId, settings);
        }

        private void UseInstantUtility(BattleUnitPresenter activePresenter, WeaponInventoryRuntime inventory, WeaponDefinition weapon)
        {
            if (!inventory.TryConsumeSelectedAmmo())
            {
                _status = GameText.AmmoDepleted(DescribeWeapon(weapon));
                return;
            }

            _activeShotWeapon = weapon;
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
            _trajectoryLine.enabled = false;
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

            SpawnImpactFlash(origin + direction * Mathf.Min(range, 0.95f), new Color(1f, 0.62f, 0.24f, 0.85f), 0.95f);
            _aimLine.enabled = false;
            _trajectoryLine.enabled = false;
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
                return;
            }

            _turnService.AdvanceTurn(_battleState);
            ApplyAimSettingsForActiveTeam();
            _aimState = new AimState(Vector2.zero, Vector2.right, _aimSettings.MinPower, false);
            _aiDecisionTimer = 0f;
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
                true,
                18);
            _arenaObjects.Add(cover);
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
            SpawnImpactFlash(owner.Position, new Color(0.66f, 0.42f, 0.96f, 0.8f), 1.05f);
            _status = GameText.Repositioned(DescribeTeam(owner.TeamId));
        }

        private Vector2 ResolveUnitLandingPosition(Vector2 desiredPoint, BattleTeamId teamId)
        {
            var bodyHeight = ArchetypeFor(teamId).BodyHeight;
            var hit = Physics2D.Raycast(desiredPoint + Vector2.up * 3f, Vector2.down, 8f);
            if (hit.collider != null)
            {
                return hit.point + Vector2.up * (bodyHeight * 0.5f);
            }

            return desiredPoint;
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

            var unlockText = string.IsNullOrWhiteSpace(GameSessionContext.SelectedMissionRewardUnlockWeaponId)
                ? string.Empty
                : GameText.UnlockAfterBattle();

            return new BattleResultScreenState(
                result,
                state.Title,
                state.Subtitle,
                new BattleRewardPreview(GameSessionContext.SelectedMissionRewardSoftCurrency, unlockText),
                state.RetryLabel,
                state.ContinueLabel);
        }

        private BattleUnitPresenter GetActivePresenter()
        {
            return _unitPresenters.FirstOrDefault(presenter => presenter.State.UnitId == _battleState.Turn.ActiveUnitId);
        }

        private void DrawAim()
        {
            if (!_aimState.HasInput)
            {
                _aimLine.enabled = false;
                return;
            }

            var activePresenter = GetActivePresenter();
            if (activePresenter == null)
            {
                return;
            }

            var origin = activePresenter.View.FirePoint.position;
            var end = origin + (Vector3)(_aimState.Direction * Mathf.Lerp(1.2f, 2.5f, Mathf.InverseLerp(_aimSettings.MinPower, _aimSettings.MaxPower, _aimState.Power)));

            _aimLine.enabled = true;
            _aimLine.SetPosition(0, origin);
            _aimLine.SetPosition(1, end);
        }

        private void DrawTrajectoryPreview()
        {
            if (!_aimState.HasInput)
            {
                _trajectoryLine.enabled = false;
                return;
            }

            var activePresenter = GetActivePresenter();
            if (activePresenter == null)
            {
                return;
            }

            if (_inventory.Selected.Role == WeaponRole.DefensiveTool)
            {
                _trajectoryLine.enabled = false;
                return;
            }

            if (_inventory.Selected.Role == WeaponRole.ShortRangeBurst)
            {
                _trajectoryLine.enabled = false;
                return;
            }

            if (_inventory.Selected.Role == WeaponRole.CoverTool)
            {
                DrawCoverPlacementPreview(activePresenter.View.FirePoint.position);
                return;
            }

            if (IsInstantUtility(_inventory.Selected.Role))
            {
                _trajectoryLine.enabled = false;
                return;
            }

            DrawBallisticTrajectory(activePresenter.View.FirePoint.position, _inventory.Selected);
        }

        private void DrawBallisticTrajectory(Vector2 firePoint, WeaponDefinition weapon)
        {
            const int steps = 24;
            _trajectoryLine.enabled = true;
            _trajectoryLine.positionCount = steps;
            for (var i = 0; i < steps; i++)
            {
                _trajectoryLine.SetPosition(i, GetTrajectoryPreviewPoint(firePoint, weapon, i));
            }
        }

        private Vector2 GetTrajectoryPreviewPoint(Vector2 firePoint, WeaponDefinition weapon, int stepIndex)
        {
            const float stepTime = 0.085f;
            var start = firePoint + (_aimState.Direction * 0.45f);
            var velocity = _aimState.Direction * _aimState.Power;
            var gravity = Physics2D.gravity * weapon.GravityScale;
            var t = stepIndex * stepTime;
            return start + velocity * t + 0.5f * gravity * (t * t);
        }

        private void DrawCoverPlacementPreview(Vector2 firePoint)
        {
            _trajectoryLine.enabled = true;
            _trajectoryLine.positionCount = 2;
            _trajectoryLine.SetPosition(0, firePoint);
            _trajectoryLine.SetPosition(1, GetCoverPlacementPoint(firePoint));
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

        private GameObject CreateBox(string objectName, Vector2 position, Vector2 size, Color color, bool solid, int sortingOrder)
        {
            var box = new GameObject(objectName);
            box.transform.position = position;
            box.transform.localScale = size;

            var renderer = box.AddComponent<SpriteRenderer>();
            renderer.sprite = _boxSprite;
            renderer.color = color;
            renderer.sortingOrder = sortingOrder;

            if (solid)
            {
                box.AddComponent<BoxCollider2D>();
            }

            return box;
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
            if (!IsBattleReady() || _battleResult.IsFinished || HasProjectilesInFlight)
            {
                return;
            }

            _aimState = new AimState(Vector2.zero, Vector2.right, _aimSettings.MinPower, false);
            _aimLine.enabled = false;
            _trajectoryLine.enabled = false;
            _status = GameText.AimReset();
        }

        private void RenderHud()
        {
            if (_hud == null || !IsBattleReady())
            {
                return;
            }

            var weapon = _inventory.Selected;
            var canFire = !_battleResult.IsFinished && !HasProjectilesInFlight && _inventory.CanFireSelected && _aimState.HasInput;
            var status = _coachTimer > 0f && !string.IsNullOrEmpty(_coachMessage) ? _coachMessage : _status;

            var state = new BattleHudState(
                status,
                _battleState.Turn.TurnNumber,
                DescribeTeam(_battleState.Turn.ActiveTeam),
                GetTeamHealth(BattleTeamId.Player),
                GetTeamMaxHealth(BattleTeamId.Player),
                GetTeamHealth(BattleTeamId.Opponent),
                GetTeamMaxHealth(BattleTeamId.Opponent),
                GameText.WeaponSlot(DescribeWeapon(weapon), _inventory.SelectedIndex + 1, _inventory.Count),
                DescribeAmmo(_inventory),
                _aimState.Power,
                _aimSettings.MinPower,
                _aimSettings.MaxPower,
                canFire,
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
    }
}
