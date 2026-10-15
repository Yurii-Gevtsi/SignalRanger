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
    public sealed partial class BattleSceneController
    {
        private sealed class MovingArenaSurface
        {
            public Transform Transform;
            public Vector3 Origin;
            public float Range;
            public float Speed;
            public float Elapsed;
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
            _deviceHealthLabels.Clear();
            _deviceTeams.Clear();
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
    }
}
