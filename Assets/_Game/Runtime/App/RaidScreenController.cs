using System;
using System.Collections.Generic;
using System.Text;
using Game.Core;
using Game.Core.Localization;
using Game.Data.Progression;
using Game.Gameplay.Battle.Weapons;
using Game.Progression.Modes;
using Game.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.App
{
    /// <summary>
    /// Raid setup screen. The player picks a difficulty and nothing else: the arena
    /// comes from the liberated pool by the screen's seed, and the opponent is always
    /// Sigma. Everything shown here is resolved before the battle loads, so the card
    /// promises exactly what the battle will deliver.
    /// </summary>
    public sealed class RaidScreenController : MonoBehaviour
    {
        private static readonly Color OverlayColor = new(0.04f, 0.06f, 0.08f, 0.92f);
        private static readonly Color SelectedDifficultyColor = new(0.22f, 0.62f, 0.40f);
        private static readonly Color UnselectedDifficultyColor = new(0.20f, 0.26f, 0.32f);
        private static readonly Vector2 PanelSize = new(880f, 1220f);
        private static readonly Vector4 CardSliceBorder = new(220f, 220f, 220f, 220f);

        private readonly List<(RaidDifficulty Difficulty, Image Image)> _difficultyButtons = new();

        private Sprite _whiteSprite;
        private Action _onClosed;
        private string _seed = string.Empty;
        private RaidDifficulty _difficulty = RaidDifficulty.Normal;
        private RaidBattleSetup _setup;
        private TMP_Text _arenaText;
        private TMP_Text _opponentText;
        private TMP_Text _rewardText;

        public static void Open(Action onClosed = null)
        {
            if (FindAnyObjectByType<RaidScreenController>() != null)
            {
                return;
            }

            var root = new GameObject("Raid Screen");
            root.AddComponent<RaidScreenController>()._onClosed = onClosed;
        }

        private void Awake()
        {
            _whiteSprite = Sprite.Create(Texture2D.whiteTexture, new Rect(0f, 0f, 1f, 1f), new Vector2(0.5f, 0.5f), 1f);
            // One seed per visit: reopening the screen offers a different arena and a
            // different Sigma loadout, while switching difficulty compares like for
            // like on the same ground.
            _seed = Guid.NewGuid().ToString("N");
            Build();
        }

        private void Build()
        {
            var canvasObject = new GameObject("Raid Canvas");
            canvasObject.transform.SetParent(transform, false);
            var canvas = canvasObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 120;

            var scaler = canvasObject.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1080f, 1920f);
            scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            scaler.matchWidthOrHeight = 0.5f;
            canvasObject.AddComponent<GraphicRaycaster>();

            var root = canvasObject.transform;
            var overlay = CreatePanel("Overlay", root, Vector2.zero, Vector2.one, new Vector2(0.5f, 0.5f),
                Vector2.zero, Vector2.zero, OverlayColor, _whiteSprite);
            overlay.raycastTarget = true;

            var panel = CreateSlicedPanel("Raid Panel", root, PanelSize, Vector2.zero);

            CreateText("Title", panel.transform, new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(0.5f, 1f),
                new Vector2(700f, 90f), new Vector2(0f, -120f), 52, TextAlignmentOptions.Center,
                GameText.RaidTitle(), GameTextRole.DisplayTitle);

            if (AppServices.Session == null || !AppServices.Session.AreRepeatableModesUnlocked)
            {
                BuildLockedState(panel);
                return;
            }

            BuildSetupState(panel);
        }

        /// <summary>
        /// Before the first planet is liberated the mode is visible but closed, and
        /// the screen says what opens it rather than hiding the card entirely.
        /// </summary>
        private void BuildLockedState(Component panel)
        {
            CreateText("Locked", panel.transform, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                new Vector2(660f, 300f), new Vector2(0f, 60f), 30, TextAlignmentOptions.Center,
                GameText.RaidLocked());

            CreateImageButton("Open Campaign", panel.transform, "UI/Shop/btn_green", new Vector2(0f, -280f),
                GameText.RaidOpenCampaign(), () =>
                {
                    Close();
                    MenuSceneController.OpenBattleModeSelection();
                });
            CreateImageButton("Back", panel.transform, "UI/Shop/btn_blue", new Vector2(0f, -420f),
                GameText.Back, Close);
        }

        private void BuildSetupState(Component panel)
        {
            _arenaText = CreateText("Arena", panel.transform, new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(0.5f, 1f),
                new Vector2(700f, 60f), new Vector2(0f, -220f), 28, TextAlignmentOptions.Center, string.Empty);

            CreateText("Difficulty Label", panel.transform, new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(0.5f, 1f),
                new Vector2(700f, 50f), new Vector2(0f, -300f), 26, TextAlignmentOptions.Center,
                GameText.RaidDifficultyLabel());

            var difficultyY = -380f;
            CreateDifficultyButton(panel, RaidDifficulty.Easy, new Vector2(-240f, difficultyY), GameText.RaidDifficultyEasy());
            CreateDifficultyButton(panel, RaidDifficulty.Normal, new Vector2(0f, difficultyY), GameText.RaidDifficultyNormal());
            CreateDifficultyButton(panel, RaidDifficulty.Hard, new Vector2(240f, difficultyY), GameText.RaidDifficultyHard());

            CreateText("Opponent Label", panel.transform, new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(0.5f, 1f),
                new Vector2(700f, 50f), new Vector2(0f, -480f), 26, TextAlignmentOptions.Center,
                GameText.RaidOpponentLabel());

            _opponentText = CreateText("Opponent", panel.transform, new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(0.5f, 1f),
                new Vector2(700f, 220f), new Vector2(0f, -540f), 26, TextAlignmentOptions.Top, string.Empty);

            _rewardText = CreateText("Reward", panel.transform, new Vector2(0.5f, 0f), new Vector2(0.5f, 0f), new Vector2(0.5f, 0f),
                new Vector2(700f, 110f), new Vector2(0f, 320f), 24, TextAlignmentOptions.Center, string.Empty);

            CreateImageButton("Start", panel.transform, "UI/Shop/btn_green", new Vector2(0f, -520f),
                GameText.EnterBattle(), StartRaid);
            CreateImageButton("Back", panel.transform, "UI/Shop/btn_blue", new Vector2(0f, -660f),
                GameText.Back, Close);

            SelectDifficulty(_difficulty);
        }

        private void CreateDifficultyButton(Component panel, RaidDifficulty difficulty, Vector2 position, string label)
        {
            var image = CreatePanel($"Difficulty {difficulty}", panel.transform, new Vector2(0.5f, 1f), new Vector2(0.5f, 1f),
                new Vector2(0.5f, 0.5f), new Vector2(220f, 96f), position, UnselectedDifficultyColor, _whiteSprite);
            var button = image.gameObject.AddComponent<Button>();
            button.targetGraphic = image;
            button.onClick.AddListener(() => SelectDifficulty(difficulty));
            CreateText($"Difficulty {difficulty} Label", image.transform, Vector2.zero, Vector2.one, new Vector2(0.5f, 0.5f),
                Vector2.zero, Vector2.zero, 26, TextAlignmentOptions.Center, label);
            _difficultyButtons.Add((difficulty, image));
        }

        private void SelectDifficulty(RaidDifficulty difficulty)
        {
            _difficulty = difficulty;
            for (var i = 0; i < _difficultyButtons.Count; i++)
            {
                var entry = _difficultyButtons[i];
                entry.Image.color = entry.Difficulty == difficulty
                    ? SelectedDifficultyColor
                    : UnselectedDifficultyColor;
            }

            RefreshSetup();
        }

        private void RefreshSetup()
        {
            _setup = AppServices.Session.PrepareRaid(_difficulty, _seed);
            if (!_setup.IsAvailable)
            {
                return;
            }

            if (_arenaText != null)
            {
                _arenaText.text = GameText.ArenaName(_setup.ArenaId, GameText.ArenaFallback());
            }

            if (_opponentText != null)
            {
                _opponentText.text = DescribeOpponent(_setup);
            }

            if (_rewardText != null)
            {
                _rewardText.text = DescribeReward(_setup);
            }
        }

        /// <summary>
        /// Sigma's loadout with the rank each weapon fights at, so the player sees
        /// what the difficulty actually bought before committing to it.
        /// </summary>
        private static string DescribeOpponent(RaidBattleSetup setup)
        {
            var builder = new StringBuilder();
            builder.AppendLine(LocalizationRuntime.Get(setup.OpponentNameKey, "Sigma"));
            for (var i = 0; i < setup.OpponentWeaponIds.Length; i++)
            {
                var weaponId = setup.OpponentWeaponIds[i];
                var rank = i < setup.OpponentWeaponRanks.Length ? setup.OpponentWeaponRanks[i] : 1;
                var name = WeaponDisplayNames.Get(weaponId);
                builder.AppendLine(rank > 1 ? GameText.WeaponWithRank(name, rank) : name);
            }

            return builder.ToString().TrimEnd();
        }

        /// <summary>
        /// What a clean victory pays right now, including the reduced rate once the
        /// day's budget is spent - the player should know that before the battle, not
        /// after it.
        /// </summary>
        private static string DescribeReward(RaidBattleSetup setup)
        {
            var save = AppServices.SaveService?.Current;
            var ordinal = (save?.activity?.daily?.arenaBattlesPlayed ?? 0) + 1;
            var scalePercent = RepeatableBattleRewardPolicy.ResolveScalePercent(ordinal);
            var reward = AppServices.BattleRewards.CalculateArenaReward(
                setup.Rules.RewardDifficulty,
                BattleRewardOutcome.Victory,
                RepeatableBattleRewardPolicy.ResolveCreditPerformanceBonusPercent(100, true),
                RepeatableBattleRewardPolicy.ResolveBlueprintPerformanceBonusPercent(100, true),
                "preview",
                scalePercent);

            var summary = GameText.RewardSummary(reward.Credits, reward.CommanderExperience, reward.Blueprints);
            return RepeatableBattleRewardPolicy.IsReduced(ordinal)
                ? summary + Environment.NewLine + GameText.RewardDailyLimit()
                : summary;
        }

        private void StartRaid()
        {
            if (!_setup.IsAvailable || !AppServices.Session.SelectRaid(_setup))
            {
                return;
            }

            _onClosed = null;
            AppServices.SceneLoader.Load(GameSceneNames.Battle);
        }

        private void Close()
        {
            var callback = _onClosed;
            _onClosed = null;
            Destroy(gameObject);
            callback?.Invoke();
        }

        private Image CreateSlicedPanel(string name, Transform parent, Vector2 size, Vector2 position)
        {
            var rect = CreateRect(name, parent, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                new Vector2(0.5f, 0.5f), size, position);
            var image = rect.gameObject.AddComponent<Image>();
            var texture = Resources.Load<Texture2D>("UI/Common/rect_card");
            image.sprite = texture != null
                ? Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height),
                    new Vector2(0.5f, 0.5f), 100f, 0u, SpriteMeshType.FullRect, CardSliceBorder)
                : _whiteSprite;
            image.type = Image.Type.Sliced;
            image.raycastTarget = true;
            return image;
        }

        private Image CreatePanel(string name, Component parent, Vector2 anchorMin, Vector2 anchorMax, Vector2 pivot,
            Vector2 sizeDelta, Vector2 anchoredPosition, Color color, Sprite sprite)
        {
            var rect = CreateRect(name, parent.transform, anchorMin, anchorMax, pivot, sizeDelta, anchoredPosition);
            var image = rect.gameObject.AddComponent<Image>();
            image.sprite = sprite ?? _whiteSprite;
            image.color = color;
            return image;
        }

        private Button CreateImageButton(string name, Transform parent, string resourcePath, Vector2 position,
            string label, Action onClick)
        {
            var rect = CreateRect(name, parent, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                new Vector2(0.5f, 0.5f), new Vector2(360f, 120f), position);
            var image = rect.gameObject.AddComponent<Image>();
            var texture = Resources.Load<Texture2D>(resourcePath);
            image.sprite = texture != null
                ? Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100f)
                : _whiteSprite;
            image.type = Image.Type.Simple;
            image.preserveAspect = true;
            var button = image.gameObject.AddComponent<Button>();
            button.targetGraphic = image;
            button.onClick.AddListener(() => onClick?.Invoke());
            CreateText($"{name} Label", image.transform, Vector2.zero, Vector2.one, new Vector2(0.5f, 0.5f),
                Vector2.zero, Vector2.zero, 30, TextAlignmentOptions.Center, label);
            return button;
        }

        private static TMP_Text CreateText(string name, Transform parent, Vector2 anchorMin, Vector2 anchorMax,
            Vector2 pivot, Vector2 sizeDelta, Vector2 anchoredPosition, int fontSize, TextAlignmentOptions alignment,
            string value, GameTextRole? roleOverride = null)
        {
            var rect = CreateRect(name, parent, anchorMin, anchorMax, pivot, sizeDelta, anchoredPosition);
            var text = rect.gameObject.AddComponent<TextMeshProUGUI>();
            var role = roleOverride ?? (fontSize >= 40 ? GameTextRole.ScreenTitle : GameTextRole.Body);
            SciFiTextStyle.Apply(text, role, fontSize, alignment);
            text.textWrappingMode = TextWrappingModes.Normal;
            text.overflowMode = TextOverflowModes.Overflow;
            text.raycastTarget = false;
            text.text = value;
            return text;
        }

        private static RectTransform CreateRect(string name, Transform parent, Vector2 anchorMin, Vector2 anchorMax,
            Vector2 pivot, Vector2 sizeDelta, Vector2 anchoredPosition)
        {
            var rectObject = new GameObject(name, typeof(RectTransform));
            var rect = (RectTransform)rectObject.transform;
            rect.SetParent(parent, false);
            rect.anchorMin = anchorMin;
            rect.anchorMax = anchorMax;
            rect.pivot = pivot;
            rect.sizeDelta = sizeDelta;
            rect.anchoredPosition = anchoredPosition;
            return rect;
        }
    }
}
