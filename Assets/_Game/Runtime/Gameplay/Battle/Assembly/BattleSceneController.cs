using System.Collections.Generic;
using System.Linq;
using Game.Core;
using Game.Core.Battle;
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

        private ArenaDefinition[] _arenas;
        private int _arenaIndex;
        private ArenaDefinition _currentArena;
        private WeaponInventoryRuntime _inventory;
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
        private bool _projectileInFlight;
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

        private void Awake()
        {
            BuildSprites();
            BuildCamera();
            BuildAimRenderers();
            BuildHud();
            _baseAimSettings = AimSettings.Default;
            _aimSettings = _baseAimSettings;
            _arenas = PrototypeArenaLibrary.CreateAll();
            _weaponExecutor = new WeaponExecutor();
            _aiShooter = new SimpleAiShooter();
            _aiDifficulty = MapDifficulty(GameSessionContext.SelectedAiDifficulty);
            _inventory = new WeaponInventoryRuntime(CreateMissionWeapons(), 1);
            SelectWeapon(_inventory.SelectedIndex);

            LoadArena(GameSessionContext.SelectedArenaIndex);
        }

        private static WeaponDefinition[] CreateMissionWeapons()
        {
            var allWeapons = PrototypeWeaponLibrary.CreateAll();
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

            return resolved.Length == 0 ? allWeapons : resolved;
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

            if (!_projectileInFlight && Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
            {
                CancelAim();
                return;
            }

            if (!_projectileInFlight && Keyboard.current != null && Keyboard.current.nKey.wasPressedThisFrame)
            {
                LoadArena(_arenaIndex + 1);
                return;
            }

            if (!_projectileInFlight && Keyboard.current != null && Keyboard.current.tabKey.wasPressedThisFrame)
            {
                SelectWeapon(_inventory.SelectedIndex + 1);
                return;
            }

            if (!_battleResult.IsFinished && !_projectileInFlight && Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                Fire();
                return;
            }

            if (_projectileInFlight)
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
            _projectileInFlight = false;
            _aiDecisionTimer = 0f;
            _status = $"{DescribeArena(_currentArena)}. Наведи постріл перетягуванням, потім натисни Вогонь.";
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
                ? $"Обрано зброю: {DescribeWeapon(weapon)}."
                : "Зброю обрано.";
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
            _projectileInFlight = false;
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
            if (!IsBattleReady() || _battleResult.IsFinished || _projectileInFlight)
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
                _status = "Спочатку наведи постріл перетягуванням у зоні бою.";
                return;
            }

            var weapon = _inventory.Selected;
            var context = new WeaponFireContext(
                weapon,
                activePresenter.State.UnitId,
                activePresenter.View.FirePoint.position,
                _aimState.Direction,
                _aimState.Power,
                activePresenter.View.GetComponent<Collider2D>(),
                this);

            var result = _weaponExecutor.TryFireSelected(_inventory, context);
            if (result != WeaponFireResult.Fired)
            {
                _status = result == WeaponFireResult.OutOfAmmo
                    ? $"Боєприпаси для {DescribeWeapon(weapon)} закінчилися. Перемкни зброю."
                    : "Зброю не обрано.";
                return;
            }

            _activeShotWeapon = weapon;
            _projectileInFlight = true;
            _turnService.SpendAction(_battleState);
            _status = $"{DescribeTeam(activePresenter.State.TeamId)} стріляє: {DescribeWeapon(weapon)}.";
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
            var projectileObject = new GameObject("Production Shell");
            _arenaObjects.Add(projectileObject);
            projectileObject.transform.position = request.Position;
            projectileObject.transform.localScale = Vector3.one * request.VisualDiameter;

            var renderer = projectileObject.AddComponent<SpriteRenderer>();
            renderer.sprite = _circleSprite;
            renderer.color = request.Color;
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

        private void OnProjectileImpacted(ProjectileImpact impact)
        {
            _projectileInFlight = false;
            SpawnImpactFlash(impact.Point);

            var damageWeapon = _activeShotWeapon ?? _inventory.Selected;
            var allUnits = _battleState.Teams.SelectMany(team => team.Units).ToList();
            var settings = new RadialDamageSettings(damageWeapon.Damage, damageWeapon.ImpactRadius, 0.65f);
            DamageResolver.ResolveRadialDamage(allUnits, impact.Point, impact.OwnerUnitId, impact.ProjectileId, settings);
            RefreshUnitViews();
            ReportTutorial(BattleTutorialBridge.ProjectileImpact);
            EmitFeedback(BattleFeedbackEventType.ProjectileImpact);

            _battleResult = BattleResultEvaluator.Evaluate(_battleState);
            if (_battleResult.IsFinished)
            {
                EmitFeedback(BattleFeedbackEventType.UnitDefeated);
                EmitFeedback(BattleFeedbackEventType.BattleFinished);
                _resultScreenState = CreateResultScreenState(_battleResult, _battleState.Turn.TurnNumber);
                _status = $"{_resultScreenState.Title}. Обери Повторити або Далі.";
                if (_battleResult.Outcome == BattleOutcome.PlayerWon)
                {
                    ReportTutorial(BattleTutorialBridge.BattleWon);
                }

                return;
            }

            _turnService.AdvanceTurn(_battleState);
            _aimState = new AimState(Vector2.zero, Vector2.right, _aimSettings.MinPower, false);
            _aiDecisionTimer = 0f;
            _aimLine.enabled = false;
            _trajectoryLine.enabled = false;
            _status = $"Хід: {DescribeTeam(_battleState.Turn.ActiveTeam)}.";
            RefreshUnitViews();
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
            if (_projectileInFlight)
            {
                return;
            }

            _aiDecisionTimer += Time.deltaTime;
            if (_aiDecisionTimer < 0.55f)
            {
                _status = "Суперник прицілюється.";
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
            var plan = _aiShooter.CreateShot(origin, target, _aimSettings, _aiDifficulty, _battleState.Turn.TurnNumber);
            _aimState = new AimState(origin, plan.Direction, plan.Power, true);
            _status = "Суперник стріляє.";
            Fire();
            _aiDecisionTimer = 0f;
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
                : "Нове спорядження відкрито після бою";

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

            const int steps = 24;
            const float stepTime = 0.085f;

            _trajectoryLine.enabled = true;
            _trajectoryLine.positionCount = steps;

            var start = (Vector2)activePresenter.View.FirePoint.position + (_aimState.Direction * 0.45f);
            var velocity = _aimState.Direction * _aimState.Power;
            // Match the actual projectile: Rigidbody2D scales global gravity by the weapon's gravityScale.
            var gravity = Physics2D.gravity * _inventory.Selected.GravityScale;

            for (var i = 0; i < steps; i++)
            {
                var t = i * stepTime;
                var position = start + velocity * t + 0.5f * gravity * (t * t);
                _trajectoryLine.SetPosition(i, position);
            }
        }

        private void SpawnImpactFlash(Vector2 position)
        {
            var flash = new GameObject("Impact Flash");
            _arenaObjects.Add(flash);
            flash.transform.position = new Vector3(position.x, position.y, -0.2f);
            flash.transform.localScale = Vector3.one * 1.2f;

            var renderer = flash.AddComponent<SpriteRenderer>();
            renderer.sprite = _circleSprite;
            renderer.color = new Color(1f, 0.74f, 0.18f, 0.85f);
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
            if (!IsBattleReady() || !_battleResult.IsFinished || !_resultScreenState.CanContinue)
            {
                return;
            }

            if (_battleResult.Outcome == BattleOutcome.PlayerWon)
            {
                GameSessionContext.ReportMissionCompleted();
            }

            SceneManager.LoadScene(GameSceneNames.Menu);
        }

        private void CancelAim()
        {
            if (!IsBattleReady() || _battleResult.IsFinished || _projectileInFlight)
            {
                return;
            }

            _aimState = new AimState(Vector2.zero, Vector2.right, _aimSettings.MinPower, false);
            _aimLine.enabled = false;
            _trajectoryLine.enabled = false;
            _status = "Приціл скинуто.";
        }

        private void RenderHud()
        {
            if (_hud == null || !IsBattleReady())
            {
                return;
            }

            var weapon = _inventory.Selected;
            var canFire = !_battleResult.IsFinished && !_projectileInFlight && _inventory.CanFireSelected && _aimState.HasInput;
            var status = _coachTimer > 0f && !string.IsNullOrEmpty(_coachMessage) ? _coachMessage : _status;

            var state = new BattleHudState(
                status,
                _battleState.Turn.TurnNumber,
                DescribeTeam(_battleState.Turn.ActiveTeam),
                GetTeamHealth(BattleTeamId.Player),
                GetTeamMaxHealth(BattleTeamId.Player),
                GetTeamHealth(BattleTeamId.Opponent),
                GetTeamMaxHealth(BattleTeamId.Opponent),
                $"{DescribeWeapon(weapon)} ({_inventory.SelectedIndex + 1}/{_inventory.Count})",
                DescribeAmmo(_inventory),
                _aimState.Power,
                _aimSettings.MinPower,
                _aimSettings.MaxPower,
                canFire,
                _battleResult.IsFinished,
                _resultScreenState.Title,
                _resultScreenState.Subtitle,
                _battleResult.IsFinished ? DescribeOutcome(_resultScreenState.Result.Outcome) : string.Empty,
                _battleResult.IsFinished ? $"Нагорода: {_resultScreenState.RewardPreview.SoftCurrency} монет" : string.Empty,
                _battleResult.IsFinished && _resultScreenState.RewardPreview.HasUnlock ? _resultScreenState.RewardPreview.UnlockText : "Нових відкриттів немає",
                _battleResult.IsFinished && _resultScreenState.CanContinue,
                _battleResult.IsFinished ? _resultScreenState.RetryLabel : "Повторити",
                _battleResult.IsFinished ? _resultScreenState.ContinueLabel : "Далі");

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
                ? $"Боєприпаси: {inventory.GetSelectedRemainingAmmo()}"
                : "Боєприпаси: безліміт";
        }

        private static string DescribeOutcome(BattleOutcome outcome)
        {
            return outcome switch
            {
                BattleOutcome.PlayerWon => "Ти переміг у бою.",
                BattleOutcome.OpponentWon => "Команда суперника перемогла.",
                BattleOutcome.Draw => "Обидві команди вибули.",
                _ => string.Empty
            };
        }

        private static string DescribeTeam(BattleTeamId teamId)
        {
            return teamId switch
            {
                BattleTeamId.Player => "Гравець",
                BattleTeamId.Opponent => "Суперник",
                _ => "Немає"
            };
        }

        private static string DescribeArena(ArenaDefinition arena)
        {
            return arena == null
                ? "Арена"
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
            return weapon == null
                ? "невідома зброя"
                : weapon.WeaponId switch
                {
                    "weapon.steady-orb" => "Рівна сфера",
                    "weapon.pulse-shell" => "Імпульсний снаряд",
                    "weapon.close-pop" => "Ближній хлопок",
                    "weapon.long-lance" => "Довгий спис",
                    "weapon.scatter-arc" => "Розсипна дуга",
                    _ => "Зброя"
                };
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
                && _inventory.HasWeapons
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
