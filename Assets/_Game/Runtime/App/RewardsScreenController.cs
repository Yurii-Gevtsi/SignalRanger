using System;
using System.Collections.Generic;
using Game.Core.Localization;
using Game.Progression;
using Game.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.App
{
    /// <summary>
    /// Daily and weekly task hub with a separate seven-day login reward track.
    /// Reward persistence and authoritative grants remain the responsibility of the reward service.
    /// </summary>
    public sealed class RewardsScreenController : MonoBehaviour
    {
        private enum RewardPeriod
        {
            Daily,
            Weekly
        }

        private enum RewardKind
        {
            Credits,
            Crystals,
            Chip,
            Chest,
            SuperChest
        }

        private sealed class QuestRowView
        {
            public RectTransform Root;
            public string QuestId;
            public TMP_Text Number;
            public TMP_Text Title;
            public TMP_Text Progress;
            public TMP_Text RewardAmount;
            public Image TaskIcon;
            public Image RewardIcon;
            public Image ProgressFill;
            public RectTransform ClaimRoot;
            public Image ClaimBackground;
            public Button ClaimButton;
            public TMP_Text ClaimLabel;
            public Image Check;
        }

        private const string MenuResourceRoot = "UI/Menu/";
        private const string ShopResourceRoot = "UI/Shop/";
        private const string RewardIconRoot = "UI/Menu/Rewards/Icons/";
        private const string QuestIconRoot = "UI/Menu/Rewards/QuestIcons/";
        private const int RewardRowCapacity = 10;
        private const int VisibleRewardRowCount = 5;
        private const float RewardRowStep = 128f;
        private const int DailyLoginRewardDayCount = 7;

        private static readonly Vector2 BackButtonSize = new(118f, 118f);
        private static readonly Vector2 BackButtonPosition = new(30f, 0f);
        private static readonly Vector2 TitlePanelSize = new(560f, 110f);
        private static readonly Vector2 TitlePanelPosition = new(128f, 0f);
        private static readonly Vector2 SettingsButtonSize = new(104f, 104f);
        private static readonly Vector2 SettingsButtonPosition = new(-26f, -10f);
        private static readonly Vector2 MainPanelSize = new(1440f, 750f);
        private static readonly Vector2 MainPanelPosition = new(-200f, -25f);
        private static readonly Vector2 LongCardSize = new(500f, 750f);
        private static readonly Vector2 LongCardPosition = new(720f, -25f);
        private static readonly Vector2 RewardRowSize = new(1320f, 110f);
        private static readonly Vector2 RewardViewportSize = new(1340f, VisibleRewardRowCount * RewardRowStep - 2f);
        private static readonly Vector2 RewardViewportPosition = new(0f, -15f);
        private static readonly Vector2 RewardTabSize = new(300f, 92f);
        private static readonly Vector2 DailyTabPosition = new(-160f, 350f);
        private static readonly Vector2 WeeklyTabPosition = new(145f, 350f);

        private readonly Dictionary<string, Sprite> _sprites = new(StringComparer.Ordinal);
        private readonly List<QuestRowView> _questRows = new();
        private RectTransform _questContent;
        private ScrollRect _questScrollRect;
        private Sprite _whiteSprite;
        private Sprite _circleSprite;
        private Sprite _roundedRectSprite;
        private Action _onClosed;
        private Image _dailyTabImage;
        private Image _weeklyTabImage;
        private TMP_Text _dailyTabLabel;
        private TMP_Text _weeklyTabLabel;
        private RewardPeriod _selectedRewardPeriod;
        private Image _dailyLoginDayRewardIcon;
        private TMP_Text _dailyLoginDayRewardAmount;
        private Image _dailyLoginDayRewardCheck;
        private Button _dailyLoginClaimButton;
        private TMP_Text _dailyLoginClaimLabel;
        private readonly TMP_Text[] _dailyLoginDayLabels = new TMP_Text[DailyLoginRewardDayCount + 1];
        private readonly TMP_Text[] _dailyLoginDayNumbers = new TMP_Text[DailyLoginRewardDayCount + 1];
        private bool _dailyLoginClaimed;

        public static void Open(Action onClosed)
        {
            if (FindAnyObjectByType<RewardsScreenController>() != null)
            {
                return;
            }

            var root = new GameObject("Rewards Screen");
            root.AddComponent<RewardsScreenController>()._onClosed = onClosed;
        }

        private void Awake()
        {
            _whiteSprite = Sprite.Create(Texture2D.whiteTexture, new Rect(0f, 0f, 1f, 1f),
                new Vector2(0.5f, 0.5f), 1f);
            _circleSprite = CreateCircleSprite();
            _roundedRectSprite = CreateRoundedRectSprite();
            Build();
        }

        private void Build()
        {
            var canvasObject = new GameObject("Rewards Canvas");
            canvasObject.transform.SetParent(transform, false);
            var canvas = canvasObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 125;

            var scaler = canvasObject.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920f, 1080f);
            scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            scaler.matchWidthOrHeight = 0.5f;
            canvasObject.AddComponent<GraphicRaycaster>();

            var root = canvasObject.transform;
            CreateImage("Rewards Background", root, Vector2.zero, Vector2.one,
                new Vector2(0.5f, 0.5f), Vector2.zero, Vector2.zero, MenuResourceRoot + "rewards_menu", false);

            var mainPanel = CreateImage("Rewards Main Panel", root, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                new Vector2(0.5f, 0.5f), MainPanelSize, MainPanelPosition,
                MenuResourceRoot + "Rewards/rewards_main_panel", false);
            BuildRewardRows(mainPanel);
            BuildLongCard(root);
            BuildRewardTabs(root);
            BuildTopNavigation(root);
        }

        private void BuildTopNavigation(Transform root)
        {
            var back = CreateImageButton("Rewards Back", root, new Vector2(0f, 1f), new Vector2(0f, 1f),
                new Vector2(0f, 1f), BackButtonSize, BackButtonPosition, ShopResourceRoot + "back_btn", Close);
            back.localScale = Vector3.one * 1.2f;

            var titlePanel = CreateImage("Rewards Title Panel", root, new Vector2(0f, 1f), new Vector2(0f, 1f),
                new Vector2(0f, 1f), TitlePanelSize, TitlePanelPosition, ShopResourceRoot + "info_panel", true);
            titlePanel.localScale = Vector3.one * 1.2f;
            CreateText("Rewards Title", titlePanel, Vector2.zero, Vector2.one, new Vector2(0.5f, 0.5f),
                new Vector2(-60f, -12f), new Vector2(-70f, 0f), 38, TextAlignmentOptions.Center,
                GameText.RewardsTitle(), GameTextRole.DisplayTitle);

            var saveData = AppServices.SaveService.Current;
            var credits = Mathf.Max(0, saveData?.economy.softCurrency ?? 0);
            var crystals = Mathf.Max(0, saveData?.economy.premiumCurrency ?? 0);

            var coins = CreateImage("Rewards Coins", root, new Vector2(1f, 1f), new Vector2(1f, 1f),
                new Vector2(1f, 1f), new Vector2(310f, 155f), new Vector2(-486f, 0f),
                ShopResourceRoot + "Coin_panel", true);
            CreateText("Rewards Coins Value", coins, new Vector2(0.34f, 0.24f), new Vector2(0.76f, 0.80f),
                new Vector2(0.5f, 0.5f), Vector2.zero, Vector2.zero, 35, TextAlignmentOptions.Center,
                credits.ToString(), GameTextRole.CurrencyGold);

            var premium = CreateImage("Rewards Crystals", root, new Vector2(1f, 1f), new Vector2(1f, 1f),
                new Vector2(1f, 1f), new Vector2(310f, 155f), new Vector2(-166f, 0f),
                ShopResourceRoot + "vip_coins_panel", true);
            CreateText("Rewards Crystals Value", premium, new Vector2(0.36f, 0.24f), new Vector2(0.76f, 0.80f),
                new Vector2(0.5f, 0.5f), Vector2.zero, Vector2.zero, 35, TextAlignmentOptions.Center,
                crystals.ToString(), GameTextRole.CurrencyCrystal);

            var settings = CreateImageButton("Rewards Settings", root, new Vector2(1f, 1f), new Vector2(1f, 1f),
                new Vector2(1f, 1f), SettingsButtonSize, SettingsButtonPosition, ShopResourceRoot + "settings_btn",
                () => SettingsScreenController.Open(null));
            settings.localScale = Vector3.one * 1.2f;
        }

        private void BuildRewardRows(RectTransform panel)
        {
            var viewport = CreateRect("Quest Scroll Viewport", panel, new Vector2(0.5f, 0.5f),
                new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), RewardViewportSize,
                RewardViewportPosition);
            var viewportImage = viewport.gameObject.AddComponent<Image>();
            viewportImage.sprite = _whiteSprite;
            viewportImage.color = new Color(1f, 1f, 1f, 0f);
            viewportImage.raycastTarget = true;
            viewport.gameObject.AddComponent<RectMask2D>();

            _questContent = CreateRect("Quest Scroll Content", viewport, new Vector2(0.5f, 1f),
                new Vector2(0.5f, 1f), new Vector2(0.5f, 1f),
                new Vector2(RewardRowSize.x, RewardRowCapacity * RewardRowStep - 2f), Vector2.zero);

            _questScrollRect = viewport.gameObject.AddComponent<ScrollRect>();
            _questScrollRect.content = _questContent;
            _questScrollRect.viewport = viewport;
            _questScrollRect.horizontal = false;
            _questScrollRect.vertical = true;
            _questScrollRect.movementType = ScrollRect.MovementType.Clamped;
            _questScrollRect.inertia = true;
            _questScrollRect.decelerationRate = 0.12f;
            _questScrollRect.scrollSensitivity = 48f;

            BuildQuestScrollbar(panel);

            for (var i = 0; i < RewardRowCapacity; i++)
            {
                var row = CreateImage($"Reward Row {i + 1}", _questContent, new Vector2(0.5f, 1f),
                    new Vector2(0.5f, 1f), new Vector2(0.5f, 0.5f), RewardRowSize,
                    new Vector2(0f, -RewardRowSize.y * 0.5f - i * RewardRowStep),
                    MenuResourceRoot + "Rewards/reward_card_row_ui_v6", false);
                _questRows.Add(BuildQuestRowContent(row, i));
            }
        }

        private void BuildQuestScrollbar(RectTransform panel)
        {
            var track = CreateSolidImage("Quest Scrollbar", panel, new Vector2(10f, RewardViewportSize.y),
                RewardViewportPosition + new Vector2(680f, 0f), new Color32(0x09, 0x34, 0x5D, 0xB8),
                _roundedRectSprite);
            track.raycastTarget = true;

            var slidingArea = CreateRect("Sliding Area", track.rectTransform, Vector2.zero, Vector2.one,
                new Vector2(0.5f, 0.5f), new Vector2(-2f, -4f), Vector2.zero);
            var handle = CreateRect("Handle", slidingArea, Vector2.zero, Vector2.one,
                new Vector2(0.5f, 0.5f), Vector2.zero, Vector2.zero);
            var handleImage = handle.gameObject.AddComponent<Image>();
            handleImage.sprite = _roundedRectSprite;
            handleImage.type = Image.Type.Sliced;
            handleImage.fillCenter = true;
            handleImage.color = new Color32(0x29, 0xD8, 0xFF, 0xFF);
            handleImage.raycastTarget = true;

            var scrollbar = track.gameObject.AddComponent<Scrollbar>();
            scrollbar.handleRect = handle;
            scrollbar.targetGraphic = handleImage;
            scrollbar.direction = Scrollbar.Direction.BottomToTop;
            scrollbar.numberOfSteps = 0;

            _questScrollRect.verticalScrollbar = scrollbar;
            _questScrollRect.verticalScrollbarVisibility = ScrollRect.ScrollbarVisibility.AutoHide;
            _questScrollRect.verticalScrollbarSpacing = 0f;
        }

        private QuestRowView BuildQuestRowContent(RectTransform row, int index)
        {
            var view = new QuestRowView
            {
                Root = row
            };

            var numberCircle = CreateSolidImage("Number Circle", row, new Vector2(46f, 46f), new Vector2(-600f, 0f),
                new Color32(0x0B, 0x46, 0x77, 0xFF), _circleSprite);
            view.Number = CreateText("Number", numberCircle.rectTransform, Vector2.zero, Vector2.one,
                new Vector2(0.5f, 0.5f), Vector2.zero, Vector2.zero, 24, TextAlignmentOptions.Center,
                (index + 1).ToString(), GameTextRole.ScreenTitle);

            view.TaskIcon = CreateImage("Task Icon", row, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                new Vector2(0.5f, 0.5f), new Vector2(88f, 88f), new Vector2(-535f, 0f),
                QuestIconRoot + "quest_login", true)
                .GetComponent<Image>();

            view.Title = CreateText("Task Title", row, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                new Vector2(0.5f, 0.5f), new Vector2(630f, 44f), new Vector2(-150f, 18f), 28,
                TextAlignmentOptions.MidlineLeft, string.Empty, GameTextRole.Body);
            view.Title.color = new Color32(0x0A, 0x2B, 0x51, 0xFF);
            view.Title.fontStyle = FontStyles.Bold;

            CreateSolidImage("Progress Outer", row, new Vector2(480f, 17f), new Vector2(-230f, -20f),
                new Color32(0x12, 0x2F, 0x49, 0xFF), _roundedRectSprite);
            CreateSolidImage("Progress Background", row, new Vector2(476f, 13f), new Vector2(-230f, -20f),
                new Color32(0x8A, 0xA1, 0xB5, 0xFF), _roundedRectSprite);
            var fill = CreateSolidImage("Progress Fill", row, new Vector2(476f, 13f), new Vector2(-468f, -20f),
                new Color32(0x29, 0xCE, 0x62, 0xFF), _roundedRectSprite);
            fill.rectTransform.anchorMin = fill.rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            fill.rectTransform.pivot = new Vector2(0f, 0.5f);
            view.ProgressFill = fill;
            view.Progress = CreateText("Task Progress", row, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                new Vector2(0.5f, 0.5f), new Vector2(132f, 34f), new Vector2(85f, -20f), 22,
                TextAlignmentOptions.Center, string.Empty, GameTextRole.Caption);
            view.Progress.color = new Color32(0x0A, 0x2B, 0x51, 0xFF);

            view.RewardIcon = CreateImage("Reward Icon", row, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                new Vector2(0.5f, 0.5f), new Vector2(72f, 72f), new Vector2(295f, 0f),
                ShopResourceRoot + "coin", true).GetComponent<Image>();
            view.RewardAmount = CreateText("Reward Amount", row, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                new Vector2(0.5f, 0.5f), new Vector2(110f, 64f), new Vector2(376f, 0f), 32,
                TextAlignmentOptions.Center, string.Empty, GameTextRole.Numeric);
            view.RewardAmount.color = Color.white;

            view.ClaimRoot = CreateImageButton("Quest Action", row, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                new Vector2(0.5f, 0.5f), new Vector2(174f, 84f), new Vector2(555f, 0f),
                RewardIconRoot + "quest_button_go", () => HandleQuestAction(view));
            view.ClaimBackground = view.ClaimRoot.GetComponent<Image>();
            view.ClaimBackground.preserveAspect = true;
            view.ClaimButton = view.ClaimRoot.GetComponent<Button>();
            view.ClaimLabel = CreateText("Claim Label", view.ClaimRoot, Vector2.zero, Vector2.one,
                new Vector2(0.5f, 0.5f), Vector2.zero, Vector2.zero, 25,
                TextAlignmentOptions.Center, GameText.ClaimReward(), GameTextRole.Button);
            view.Check = CreateImage("Claimed Check", row, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                new Vector2(0.5f, 0.5f), new Vector2(66f, 66f), new Vector2(494f, 0f),
                RewardIconRoot + "check", true).GetComponent<Image>();
            return view;
        }

        private void BuildLongCard(Transform root)
        {
            var card = CreateImage("Rewards Long Card", root, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                new Vector2(0.5f, 0.5f), LongCardSize, LongCardPosition,
                MenuResourceRoot + "Rewards/rewards_long_card", false);

            var description = CreateText("Login Reward Description", card, new Vector2(0.5f, 1f), new Vector2(0.5f, 1f),
                new Vector2(0.5f, 1f), new Vector2(440f, 64f), new Vector2(0f, -48f), 20,
                TextAlignmentOptions.Center, GameText.DailyLoginDescription(), GameTextRole.Caption);
            description.textWrappingMode = TextWrappingModes.Normal;
            description.overflowMode = TextOverflowModes.Overflow;

            var dayRewards = new[]
            {
                (RewardKind.Credits, 500), (RewardKind.Crystals, 10), (RewardKind.Chest, 1),
                (RewardKind.Credits, 500), (RewardKind.Crystals, 20), (RewardKind.Chip, 20),
                (RewardKind.SuperChest, 1)
            };

            BuildLoginDay(card, 7, dayRewards[6], new Vector2(430f, 250f), new Vector2(15f, 190f), true);
            BuildLoginDay(card, 6, dayRewards[5], new Vector2(202f, 202f), new Vector2(-90f, 60f), false);
            BuildLoginDay(card, 5, dayRewards[4], new Vector2(202f, 202f), new Vector2(100f, 60f), false);
            BuildLoginDay(card, 4, dayRewards[3], new Vector2(202f, 202f), new Vector2(-90f, -58f), false);
            BuildLoginDay(card, 3, dayRewards[2], new Vector2(202f, 202f), new Vector2(100f, -58f), false);
            BuildLoginDay(card, 2, dayRewards[1], new Vector2(202f, 202f), new Vector2(-90f, -176f), false);
            BuildLoginDay(card, 1, dayRewards[0], new Vector2(202f, 202f), new Vector2(100f, -176f), false);

            var claimRoot = CreateImageButton("Login Reward Claim", card, new Vector2(0.5f, 0.5f),
                new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(410f, 88f),
                new Vector2(0f, -275f), RewardIconRoot + "claim_button_yellow", null);
            _dailyLoginClaimButton = claimRoot.GetComponent<Button>();
            _dailyLoginClaimButton.interactable = false;
            _dailyLoginClaimLabel = CreateText("Login Reward Claim Label", claimRoot, Vector2.zero, Vector2.one,
                new Vector2(0.5f, 0.5f), Vector2.zero, Vector2.zero, 30,
                TextAlignmentOptions.Center, GameText.ClaimReward(), GameTextRole.Button);

            _dailyLoginClaimed = IsDailyLoginRewardClaimed();
            RefreshDailyLoginState();
        }

        private void BuildLoginDay(
            RectTransform card,
            int day,
            (RewardKind Kind, int Amount) reward,
            Vector2 size,
            Vector2 position,
            bool isFinalReward)
        {
            var isCurrent = day == 1;
            var row = CreateRect($"Login Day {day}", card, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                new Vector2(0.5f, 0.5f), size, position);
            CreateImage("Login Reward Card", row, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                new Vector2(0.5f, 0.5f), size, Vector2.zero, "UI/Common/card2", false);
            var dayLabelPosition = isFinalReward ? new Vector2(110f, -70f) : new Vector2(30f, -50f);
            var dayLabelFontSize = isFinalReward ? 28 : 25;
            var dayLabel = CreateText("Day Label", row, new Vector2(0.5f, 1f), new Vector2(0.5f, 1f),
                new Vector2(0.5f, 1f), new Vector2(size.x - 24f, 34f), dayLabelPosition, dayLabelFontSize,
                TextAlignmentOptions.Center, $"{GameText.RewardDayLabel()} {day}", GameTextRole.Body);
            _dailyLoginDayLabels[day] = dayLabel;
            dayLabel.color = SciFiTextStyle.MainText;

            var iconSize = isFinalReward
                ? new Vector2(200f, 200f)
                : reward.Kind == RewardKind.Chip
                    ? new Vector2(70f, 70f)
                    : new Vector2(80f, 80f);
            var iconPosition = isFinalReward ? new Vector2(-20f, 0f) : new Vector2(-45f, 0f);
            var rewardIcon = CreateImage("Day Reward Icon", row, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                new Vector2(0.5f, 0.5f), iconSize, iconPosition,
                isFinalReward ? RewardIconRoot + "super_chest_clean" : RewardSpritePath(reward.Kind), true).GetComponent<Image>();
            if (isFinalReward)
            {
                return;
            }

            var amountPosition = isFinalReward ? new Vector2(96f, -14f) : new Vector2(35f, -12f);
            var rewardAmount = CreateText("Day Reward Amount", row, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                new Vector2(0.5f, 0.5f), new Vector2(64f, 46f), amountPosition, 28,
                TextAlignmentOptions.Center, reward.Amount.ToString(), GameTextRole.Numeric);
            if (!isCurrent)
            {
                return;
            }

            _dailyLoginDayRewardIcon = rewardIcon;
            _dailyLoginDayRewardAmount = rewardAmount;
            _dailyLoginDayRewardCheck = CreateImage("Current Day Reward Claimed", row,
                new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                new Vector2(58f, 58f), new Vector2(0f, -10f), RewardIconRoot + "check", true).GetComponent<Image>();
        }

        private void BuildRewardTabs(Transform root)
        {
            var daily = CreateImageButton("Daily Rewards Tab", root, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                new Vector2(0.5f, 0.5f), RewardTabSize, DailyTabPosition, "UI/Common/reward_tab_active",
                () => SelectRewardPeriod(RewardPeriod.Daily));
            _dailyTabImage = daily.GetComponent<Image>();
            CreateImage("Daily Calendar", daily, new Vector2(0f, 0.5f), new Vector2(0f, 0.5f),
                new Vector2(0f, 0.5f), new Vector2(48f, 48f), new Vector2(38f, 0f), RewardIconRoot + "calendar", true);
            _dailyTabLabel = CreateText("Daily Rewards Tab Label", daily, Vector2.zero, Vector2.one,
                new Vector2(0.5f, 0.5f), new Vector2(-58f, -12f), new Vector2(24f, 0f), 28,
                TextAlignmentOptions.Center, GameText.DailyRewards(), GameTextRole.ScreenTitle);

            var weekly = CreateImageButton("Weekly Rewards Tab", root, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                new Vector2(0.5f, 0.5f), RewardTabSize, WeeklyTabPosition, "UI/Common/reward_tab_inactive",
                () => SelectRewardPeriod(RewardPeriod.Weekly));
            _weeklyTabImage = weekly.GetComponent<Image>();
            CreateImage("Weekly Calendar", weekly, new Vector2(0f, 0.5f), new Vector2(0f, 0.5f),
                new Vector2(0f, 0.5f), new Vector2(48f, 48f), new Vector2(38f, 0f), RewardIconRoot + "calendar", true);
            _weeklyTabLabel = CreateText("Weekly Rewards Tab Label", weekly, Vector2.zero, Vector2.one,
                new Vector2(0.5f, 0.5f), new Vector2(-58f, -12f), new Vector2(24f, 0f), 28,
                TextAlignmentOptions.Center, GameText.WeeklyRewards(), GameTextRole.ScreenTitle);

            SelectRewardPeriod(RewardPeriod.Daily);
        }

        private void SelectRewardPeriod(RewardPeriod period)
        {
            _selectedRewardPeriod = period;
            var dailyIsActive = period == RewardPeriod.Daily;
            if (_dailyTabImage != null)
            {
                _dailyTabImage.sprite = LoadSprite(dailyIsActive ? "UI/Common/reward_tab_active" : "UI/Common/reward_tab_inactive");
            }

            if (_weeklyTabImage != null)
            {
                _weeklyTabImage.sprite = LoadSprite(dailyIsActive ? "UI/Common/reward_tab_inactive" : "UI/Common/reward_tab_active");
            }

            if (_dailyTabLabel != null)
            {
                _dailyTabLabel.color = dailyIsActive ? SciFiTextStyle.GoldText : SciFiTextStyle.CyanText;
            }

            if (_weeklyTabLabel != null)
            {
                _weeklyTabLabel.color = dailyIsActive ? SciFiTextStyle.CyanText : SciFiTextStyle.GoldText;
            }

            RefreshQuestRows();
        }

        private void RefreshQuestRows()
        {
            var period = ToActivityPeriod(_selectedRewardPeriod);
            var progress = AppServices.ActivityProgress;
            if (progress == null)
            {
                return;
            }

            var quests = ActivityQuestCatalog.GetVisibleQuests(period, progress.GetPeriodStartUtcTicks(period));
            var orderedQuests = OrderQuestsForDisplay(quests, period, progress);
            var visibleCount = Mathf.Min(_questRows.Count, orderedQuests.Count);
            if (_questContent != null)
            {
                var contentHeight = Mathf.Max(RewardViewportSize.y, visibleCount * RewardRowStep - 2f);
                _questContent.sizeDelta = new Vector2(RewardRowSize.x, contentHeight);
            }

            for (var i = 0; i < _questRows.Count; i++)
            {
                var view = _questRows[i];
                var isVisible = i < visibleCount;
                view.Root.gameObject.SetActive(isVisible);
                if (!isVisible)
                {
                    view.QuestId = string.Empty;
                    continue;
                }

                var quest = orderedQuests[i];
                var current = progress.GetProgress(quest.Metric, period);
                var completed = current >= quest.Target;
                var claimed = progress.IsClaimed(period, quest.Id);
                view.QuestId = quest.Id;
                view.Number.text = (i + 1).ToString();
                view.Title.text = GameText.RewardQuest(quest.Id, quest.FallbackTitle);
                view.Progress.text = $"{Mathf.Min(current, quest.Target)}/{quest.Target}";
                var progressWidth = 476f * Mathf.Clamp01((float)current / quest.Target);
                view.ProgressFill.rectTransform.sizeDelta = new Vector2(progressWidth, 13f);
                view.ProgressFill.enabled = progressWidth > 0f;
                view.TaskIcon.sprite = LoadSprite(QuestIconRoot + quest.IconId);
                view.RewardIcon.sprite = LoadSprite(RewardSpritePath(quest.RewardKind));
                view.RewardAmount.text = quest.RewardAmount.ToString();
                view.ClaimRoot.gameObject.SetActive(!claimed);
                view.Check.gameObject.SetActive(claimed);
                view.RewardIcon.gameObject.SetActive(!claimed);
                view.RewardAmount.gameObject.SetActive(!claimed);
                view.ClaimButton.interactable = true;
                view.ClaimBackground.sprite = LoadSprite(RewardIconRoot + (completed ? "quest_button_claim" : "quest_button_go"));
                view.ClaimLabel.text = completed ? GameText.ClaimReward() : GameText.GoToQuest();
                view.ClaimLabel.color = Color.white;
            }

            if (_questScrollRect != null)
            {
                Canvas.ForceUpdateCanvases();
                _questScrollRect.StopMovement();
                _questScrollRect.verticalNormalizedPosition = 1f;
            }
        }

        private static List<ActivityQuestDefinition> OrderQuestsForDisplay(
            IReadOnlyList<ActivityQuestDefinition> quests,
            ActivityPeriod period,
            ActivityProgressService progress)
        {
            var ordered = new List<ActivityQuestDefinition>(quests.Count);
            for (var i = 0; i < quests.Count; i++)
            {
                if (!progress.IsClaimed(period, quests[i].Id))
                {
                    ordered.Add(quests[i]);
                }
            }

            for (var i = 0; i < quests.Count; i++)
            {
                if (progress.IsClaimed(period, quests[i].Id))
                {
                    ordered.Add(quests[i]);
                }
            }

            return ordered;
        }

        private void HandleQuestAction(QuestRowView view)
        {
            var period = ToActivityPeriod(_selectedRewardPeriod);
            var progress = AppServices.ActivityProgress;
            if (progress == null || view == null || string.IsNullOrWhiteSpace(view.QuestId))
            {
                return;
            }

            var quests = ActivityQuestCatalog.GetVisibleQuests(period, progress.GetPeriodStartUtcTicks(period));
            if (!TryFindQuest(quests, view.QuestId, out var quest))
            {
                return;
            }

            if (progress.GetProgress(quest.Metric, period) >= quest.Target)
            {
                ClaimQuest(quest.Id);
                return;
            }

            OpenQuestTarget(quest);
        }

        private void OpenQuestTarget(ActivityQuestDefinition quest)
        {
            if (quest.Metric == ActivityMetric.RewardedAdsWatched)
            {
                AppServices.Ads?.ShowRewardedAd("reward.default");
                return;
            }

            Close();
            MenuSceneController.OpenBattleModeSelection();
        }

        private void ClaimQuest(string questId)
        {
            var period = ToActivityPeriod(_selectedRewardPeriod);
            var progress = AppServices.ActivityProgress;
            if (progress == null || string.IsNullOrWhiteSpace(questId))
            {
                return;
            }

            var quests = ActivityQuestCatalog.GetVisibleQuests(period, progress.GetPeriodStartUtcTicks(period));
            if (!TryFindQuest(quests, questId, out var quest))
            {
                return;
            }

            if (!progress.TryClaim(period, quest.Id, quest.Metric, quest.Target))
            {
                return;
            }

            AppServices.SaveService?.Save(AppServices.SaveService.Current);
            RefreshQuestRows();
        }

        private static bool TryFindQuest(
            IReadOnlyList<ActivityQuestDefinition> quests,
            string questId,
            out ActivityQuestDefinition quest)
        {
            for (var i = 0; i < quests.Count; i++)
            {
                if (string.Equals(quests[i].Id, questId, StringComparison.Ordinal))
                {
                    quest = quests[i];
                    return true;
                }
            }

            quest = default;
            return false;
        }

        private void RefreshDailyLoginState()
        {
            if (_dailyLoginDayRewardIcon != null)
            {
                _dailyLoginDayRewardIcon.gameObject.SetActive(!_dailyLoginClaimed);
            }

            if (_dailyLoginDayRewardAmount != null)
            {
                _dailyLoginDayRewardAmount.gameObject.SetActive(!_dailyLoginClaimed);
            }

            if (_dailyLoginClaimButton != null)
            {
                _dailyLoginClaimButton.interactable = !_dailyLoginClaimed;
            }

            if (_dailyLoginClaimLabel != null)
            {
                _dailyLoginClaimLabel.text = _dailyLoginClaimed
                    ? GameText.RewardClaimed()
                    : GameText.ClaimReward();
                _dailyLoginClaimLabel.color = _dailyLoginClaimed
                    ? SciFiTextStyle.SuccessText
                    : Color.white;
            }

            if (_dailyLoginDayRewardCheck != null)
            {
                _dailyLoginDayRewardCheck.gameObject.SetActive(_dailyLoginClaimed);
            }

            for (var day = 1; day < _dailyLoginDayLabels.Length; day++)
            {
                if (_dailyLoginDayLabels[day] != null)
                {
                    _dailyLoginDayLabels[day].color = _dailyLoginClaimed && day == 1
                        ? SciFiTextStyle.SuccessText
                        : SciFiTextStyle.MainText;
                }

                if (_dailyLoginDayNumbers[day] != null)
                {
                    _dailyLoginDayNumbers[day].color = _dailyLoginClaimed && day == 1
                        ? SciFiTextStyle.SuccessText
                        : SciFiTextStyle.MainText;
                }
            }
        }

        private static bool IsDailyLoginRewardClaimed()
        {
            var saveData = AppServices.SaveService?.Current;
            var activity = saveData?.activity;
            return activity != null
                && activity.lastLoginDailyPeriodStartUtcTicks == activity.dailyPeriodStartUtcTicks;
        }

        private static string RewardSpritePath(RewardKind kind)
        {
            return kind switch
            {
                RewardKind.Crystals => RewardIconRoot + "reward_crystal",
                RewardKind.Chip => RewardIconRoot + "reward_chip",
                RewardKind.Chest => "UI/Weapons/bonus_box",
                RewardKind.SuperChest => RewardIconRoot + "super_chest",
                _ => ShopResourceRoot + "coin"
            };
        }

        private static string RewardSpritePath(ActivityRewardKind kind)
        {
            return kind switch
            {
                ActivityRewardKind.Crystals => RewardIconRoot + "reward_crystal",
                ActivityRewardKind.Chip => RewardIconRoot + "reward_chip",
                ActivityRewardKind.Chest => "UI/Weapons/bonus_box",
                _ => ShopResourceRoot + "coin"
            };
        }

        private static ActivityPeriod ToActivityPeriod(RewardPeriod period)
        {
            return period == RewardPeriod.Daily ? ActivityPeriod.Daily : ActivityPeriod.Weekly;
        }

        private void Close()
        {
            var callback = _onClosed;
            Destroy(gameObject);
            callback?.Invoke();
        }

        private RectTransform CreateImage(string name, Component parent, Vector2 anchorMin, Vector2 anchorMax,
            Vector2 pivot, Vector2 sizeDelta, Vector2 anchoredPosition, string resourceName, bool preserveAspect)
        {
            var rect = CreateRect(name, parent, anchorMin, anchorMax, pivot, sizeDelta, anchoredPosition);
            var image = rect.gameObject.AddComponent<Image>();
            image.sprite = LoadSprite(resourceName);
            image.color = Color.white;
            image.preserveAspect = preserveAspect;
            image.raycastTarget = false;
            return rect;
        }

        private Image CreateSolidImage(string name, Component parent, Vector2 size, Vector2 position, Color color, Sprite sprite)
        {
            var rect = CreateRect(name, parent, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                new Vector2(0.5f, 0.5f), size, position);
            var image = rect.gameObject.AddComponent<Image>();
            image.sprite = sprite;
            if (sprite == _roundedRectSprite)
            {
                image.type = Image.Type.Sliced;
                image.fillCenter = true;
            }

            image.color = color;
            image.raycastTarget = false;
            return image;
        }

        private RectTransform CreateImageButton(string name, Component parent, Vector2 anchorMin, Vector2 anchorMax,
            Vector2 pivot, Vector2 sizeDelta, Vector2 anchoredPosition, string resourceName, Action onClick)
        {
            var rect = CreateImage(name, parent, anchorMin, anchorMax, pivot, sizeDelta, anchoredPosition,
                resourceName, true);
            var image = rect.GetComponent<Image>();
            image.raycastTarget = true;
            var button = rect.gameObject.AddComponent<Button>();
            button.targetGraphic = image;
            button.transition = Selectable.Transition.ColorTint;
            button.colors = CreateButtonColors();
            button.onClick.AddListener(() => onClick?.Invoke());
            return rect;
        }

        private TMP_Text CreateText(string name, Component parent, Vector2 anchorMin, Vector2 anchorMax,
            Vector2 pivot, Vector2 sizeDelta, Vector2 anchoredPosition, int fontSize,
            TextAlignmentOptions alignment, string value, GameTextRole role = GameTextRole.DisplayTitle)
        {
            var rect = CreateRect(name, parent, anchorMin, anchorMax, pivot, sizeDelta, anchoredPosition);
            var text = rect.gameObject.AddComponent<TextMeshProUGUI>();
            SciFiTextStyle.Apply(text, role, fontSize, alignment);
            text.enableAutoSizing = false;
            text.textWrappingMode = TextWrappingModes.NoWrap;
            text.overflowMode = TextOverflowModes.Truncate;
            text.text = value;
            text.raycastTarget = false;
            return text;
        }

        private Sprite LoadSprite(string resourceName)
        {
            if (_sprites.TryGetValue(resourceName, out var cached))
            {
                return cached;
            }

            var texture = Resources.Load<Texture2D>(resourceName);
            if (texture == null)
            {
                return _whiteSprite;
            }

            var sprite = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height),
                new Vector2(0.5f, 0.5f), 100f);
            _sprites[resourceName] = sprite;
            return sprite;
        }

        private static Sprite CreateCircleSprite()
        {
            const int size = 96;
            var texture = new Texture2D(size, size, TextureFormat.RGBA32, false);
            var pixels = new Color32[size * size];
            var center = (size - 1) * 0.5f;
            var radius = center - 1f;
            for (var y = 0; y < size; y++)
            {
                for (var x = 0; x < size; x++)
                {
                    var distance = Vector2.Distance(new Vector2(x, y), new Vector2(center, center));
                    var alpha = (byte)Mathf.RoundToInt(Mathf.Clamp01(radius + 1f - distance) * 255f);
                    pixels[y * size + x] = new Color32(255, 255, 255, alpha);
                }
            }

            texture.SetPixels32(pixels);
            texture.Apply(false, true);
            return Sprite.Create(texture, new Rect(0f, 0f, size, size), new Vector2(0.5f, 0.5f), 100f);
        }

        private static Sprite CreateRoundedRectSprite()
        {
            const int width = 64;
            const int height = 32;
            const float radius = 8f;
            const float border = radius + 1f;
            var texture = new Texture2D(width, height, TextureFormat.RGBA32, false);
            var pixels = new Color32[width * height];
            var center = new Vector2((width - 1) * 0.5f, (height - 1) * 0.5f);
            var halfSize = new Vector2(width * 0.5f - 1f, height * 0.5f - 1f);

            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    var local = new Vector2(Mathf.Abs(x - center.x), Mathf.Abs(y - center.y));
                    var corner = local - (halfSize - Vector2.one * radius);
                    var outside = new Vector2(Mathf.Max(corner.x, 0f), Mathf.Max(corner.y, 0f)).magnitude;
                    var inside = Mathf.Min(Mathf.Max(corner.x, corner.y), 0f);
                    var signedDistance = outside + inside - radius;
                    var alpha = (byte)Mathf.RoundToInt(Mathf.Clamp01(0.5f - signedDistance) * 255f);
                    pixels[y * width + x] = new Color32(255, 255, 255, alpha);
                }
            }

            texture.SetPixels32(pixels);
            texture.Apply(false, true);
            return Sprite.Create(texture, new Rect(0f, 0f, width, height), new Vector2(0.5f, 0.5f),
                100f, 0u, SpriteMeshType.FullRect, new Vector4(border, border, border, border));
        }

        private static ColorBlock CreateButtonColors()
        {
            return new ColorBlock
            {
                normalColor = Color.white,
                highlightedColor = new Color(1.06f, 1.06f, 1.06f, 1f),
                pressedColor = new Color(0.88f, 0.92f, 0.95f, 1f),
                selectedColor = Color.white,
                disabledColor = new Color(0.58f, 0.62f, 0.66f, 0.65f),
                colorMultiplier = 1f,
                fadeDuration = 0.08f
            };
        }

        private static RectTransform CreateRect(string name, Component parent, Vector2 anchorMin, Vector2 anchorMax,
            Vector2 pivot, Vector2 sizeDelta, Vector2 anchoredPosition)
        {
            var rectObject = new GameObject(name, typeof(RectTransform));
            var rect = (RectTransform)rectObject.transform;
            rect.SetParent(parent.transform, false);
            rect.anchorMin = anchorMin;
            rect.anchorMax = anchorMax;
            rect.pivot = pivot;
            rect.sizeDelta = sizeDelta;
            rect.anchoredPosition = anchoredPosition;
            return rect;
        }
    }
}
