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
    }
}
