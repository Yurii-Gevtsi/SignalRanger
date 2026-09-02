using System;
using System.Collections.Generic;
using Game.Core;
using Game.Core.Localization;
using Game.Data.Progression;
using Game.Data.Save;
using Game.Progression.Missions;
using Game.Progression.Modes;
using Game.UI;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Game.App
{
    public sealed class MenuSceneController : MonoBehaviour
    {
        private const string UiResourceRoot = "UI/Menu/";
        private static readonly Color BackgroundColor = new(0.02f, 0.025f, 0.03f);
        private static readonly Color TextShadowColor = new(0.02f, 0.03f, 0.05f, 0.88f);
        // Layout uses the Canvas reference resolution (1920 x 1080).
        private static readonly Vector2 CharacterSize = new(560f, 560f);
        private static readonly Vector2 CharacterPosition = new(40f, -10f);
        private static readonly Vector2 PlayButtonPosition = new(0f, -39f);
        private static readonly Vector2 SideButtonSize = new(220f, 220f);
        private static readonly Vector2 ArchitectButtonSize = new(270f, 270f);
        private static readonly Vector2 InventoryButtonPosition = new(80f, -10f);
        private static readonly Vector2 ArchitectButtonPosition = new(450f, 24f);
        private static readonly Vector2 ArchitectButtonScale = new(0.9f, 1.2f);
        private static readonly Vector2 ShopButtonPosition = new(-80f, -10f);
        private static readonly Vector2 RewardsSize = new(220f, 220f);
        private static readonly Vector2 RewardsPosition = new(480f, -10f);
        private static readonly Vector2 SettingsSize = new(140f, 140f);
        private static readonly Vector2 SettingsPosition = new(-26f, -31f);
        private static readonly Vector2 CampaignCardSize = new(2050f, 1180f);
        private static readonly Vector2 CampaignCardPosition = new(0f, -50f);
        private static readonly Vector2 CampaignCardCloseSize = new(92f, 92f);
        private static readonly Vector2 CampaignCardClosePosition = new(-108f, -170f);
        private static readonly Vector2 CampaignModeTabSize = new(320f, 94f);
        private static readonly Vector2 CampaignModeTabsLineSize = new(1750f, 5f);
        private static readonly Vector2 CampaignModeContentSize = new(1770f, 900f);
        private static readonly Vector2 CampaignModeLeftSize = new(1200f, 640f);
        private static readonly Vector2 CampaignModeLeftPosition = new(-320f, -8f);
        private static readonly Vector2 CampaignSectorBackgroundSize = new(1195f, 630f);
        private static readonly Vector2 CampaignModeRightSize = new(520f, 650f);
        private static readonly Vector2 CampaignModeRightPosition = new(600f, -8f);
        private static readonly Vector2 CampaignMapPlanetPanelSize = CampaignModeLeftSize;
        private static readonly Vector2 CampaignMapPlanetPanelPosition = CampaignModeLeftPosition;
        private static readonly Vector2 CampaignMapMissionContentPosition = CampaignModeRightPosition;
        private static readonly Vector2 CampaignStartButtonSize = new(490f, 100f);
        private const float CampaignRightTextWidth = 492f;
        private static readonly Vector2 CampaignClusterArrowSize = new(150f, 180f);
        private static readonly Vector2 CampaignClusterLeftArrowPosition = new(-880f, 0f);
        private static readonly Vector2 CampaignClusterRightArrowPosition = new(230f, 0f);
        private const int CampaignClusterCount = 4;
        private const int CampaignSectorPaginatorCount = 10;
        private static readonly Vector2 CampaignSectorProgressCardSize = new(200f, 82f);
        private static readonly Vector2 CampaignSectorProgressCardPosition = new(500f, 270f);
        private static readonly Vector2 CampaignMissionCardSize = new(650f, 104f);
        private static readonly Vector4 CampaignCardSliceBorder = new(220f, 220f, 220f, 220f);

        private enum CampaignModeTab
        {
            Map,
            Raid,
            Arena,
            Bosses
        }

        private readonly Dictionary<string, Sprite> _sprites = new();

        private Sprite _whiteSprite;
        private GameObject _canvasObject;
        private GameObject _campaignCardOverlay;
        private GameObject _campaignSectorProgressDialog;
        private RectTransform _campaignModeContent;
        private readonly List<(Image Image, TMP_Text Label)> _campaignModeTabs = new();
        private CampaignModeTab _selectedCampaignMode;
        private RaidDifficulty _selectedRaidDifficulty = RaidDifficulty.Normal;
        private int _selectedPlanetIndex;
        private int _selectedCampaignClusterIndex;
        private string _selectedCampaignMissionId = string.Empty;
        private RectTransform _campaignMapDetails;
        private readonly List<RectTransform> _campaignPlanetButtons = new();

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void CreateForMenuScene()
        {
            if (SceneManager.GetActiveScene().name != GameSceneNames.Menu)
            {
                return;
            }

            if (IntroSceneController.ShouldShow())
            {
                IntroSceneController.EnsureCreated();
                return;
            }

            EnsureCreated();
        }

        public static void EnsureCreated()
        {
            if (FindAnyObjectByType<MenuSceneController>() != null)
            {
                return;
            }

            var root = new GameObject("Menu Scene Controller");
            root.AddComponent<MenuSceneController>();
        }

        private void Awake()
        {
            AppServices.Initialize();
            _whiteSprite = Sprite.Create(Texture2D.whiteTexture, new Rect(0f, 0f, 1f, 1f), new Vector2(0.5f, 0.5f), 1f);

            BuildCamera();
            EnsureEventSystem();
            if (AppServices.Localization != null)
            {
                AppServices.Localization.LanguageChanged += RefreshForLanguage;
            }
            BuildMenu();
        }

        private void OnDestroy()
        {
            if (AppServices.Localization != null)
            {
                AppServices.Localization.LanguageChanged -= RefreshForLanguage;
            }
        }

        private void Start()
        {
            // Arriving from a finished battle via "continue": go straight back to the
            // campaign map with that mission's planet open. Runs here rather than in
            // Awake so the bootstrapper has already banked the mission completion and
            // the card shows the freshly cleared mission as done.
            var focusMissionId = GameSessionContext.ConsumePendingCampaignMissionView();
            if (!string.IsNullOrWhiteSpace(focusMissionId))
            {
                OpenCampaignCardForMission(focusMissionId);
                return;
            }

            if (AppServices.TryConsumeFirstLoginToday())
            {
                RewardsScreenController.Open(() =>
                {
                    if (AppServices.TryConsumePendingLoadoutReview())
                    {
                        WeaponsScreenController.Open(null);
                    }
                });
                return;
            }

            if (AppServices.TryConsumePendingLoadoutReview())
            {
                WeaponsScreenController.Open(null);
            }
        }

        private static void EnsureEventSystem()
        {
            if (FindAnyObjectByType<EventSystem>() != null)
            {
                return;
            }

            var eventSystem = new GameObject("Menu EventSystem");
            eventSystem.AddComponent<EventSystem>();
            eventSystem.AddComponent<InputSystemUIInputModule>();
        }

        private static void BuildCamera()
        {
            var cameraObject = new GameObject("Menu Camera");
            var camera = cameraObject.AddComponent<Camera>();
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = BackgroundColor;
            camera.orthographic = true;
            camera.orthographicSize = 5f;
            cameraObject.tag = "MainCamera";
        }

        private void BuildMenu()
        {
            var saveData = AppServices.SaveService.Current;

            var canvasObject = new GameObject("Menu Canvas");
            _canvasObject = canvasObject;
            var canvas = canvasObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 50;

            var scaler = canvasObject.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920f, 1080f);
            scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            scaler.matchWidthOrHeight = 0.5f;
            canvasObject.AddComponent<GraphicRaycaster>();

            var root = canvasObject.transform;
            CreateImage("Main Background", root, Vector2.zero, Vector2.one, new Vector2(0.5f, 0.5f),
                Vector2.zero, Vector2.zero, "main_scr_albom", false, null);
            CreateImage("Menu Character", root, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                new Vector2(0.5f, 0.5f), CharacterSize, CharacterPosition,
                "Visuals/Characters/racoon-full", true, null);

            BuildTopBar(root, saveData);
            BuildBottomActions(root);
        }

        private void RefreshForLanguage()
        {
            if (_canvasObject != null)
            {
                Destroy(_canvasObject);
            }

            _campaignCardOverlay = null;
            BuildMenu();
        }

        private void BuildTopBar(Transform root, SaveData saveData)
        {
            var softCurrency = Mathf.Max(0, saveData?.economy.softCurrency ?? 0);
            var premiumCurrency = Mathf.Max(0, saveData?.economy.premiumCurrency ?? 0);

            var profile = CreateImage("Profile Panel", root, new Vector2(0f, 1f), new Vector2(0f, 1f), new Vector2(0f, 1f),
                new Vector2(560f, 110f), new Vector2(80f, -40f), "UI/Shop/info_panel", true, null);
            var profileImage = profile.GetComponent<Image>();
            profileImage.raycastTarget = true;
            var profileButton = profile.gameObject.AddComponent<Button>();
            profileButton.targetGraphic = profileImage;
            profileButton.onClick.AddListener(OpenNameChange);
            var playerName = CreateLabel("Player Name", profile.transform, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                46, TextAlignmentOptions.Center, ResolvePlayerName(saveData), GameTextRole.DisplayTitle);
            playerName.rectTransform.sizeDelta = new Vector2(310f, 90f);
            playerName.rectTransform.anchoredPosition = new Vector2(-145f, 0f);
            var commanderLevel = CommanderProgressionTable.GetLevel(saveData?.profile?.commanderExperience ?? 0);
            var levelLabel = CreateLabel("Player Level", profile.transform, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                24, TextAlignmentOptions.Center, GameText.PlayerLevel(commanderLevel), GameTextRole.Caption);
            levelLabel.color = SciFiTextStyle.CyanText;
            levelLabel.rectTransform.sizeDelta = new Vector2(130f, 60f);
            levelLabel.rectTransform.anchoredPosition = new Vector2(90f, 0f);

            CreateImageButton("Settings Button", root, new Vector2(1f, 1f), new Vector2(1f, 1f), new Vector2(1f, 1f),
                SettingsSize, SettingsPosition, "settings_btn", OpenSettings);

            var coins = CreateImage("Coins Panel", root, new Vector2(1f, 1f), new Vector2(1f, 1f), new Vector2(1f, 1f),
                new Vector2(372f, 155f), new Vector2(-486f, -20f), "Coin_panel", true, null);
            var coinsValue = CreateLabel("Coins Value", coins.transform, new Vector2(0.40f, 0.24f), new Vector2(0.80f, 0.80f),
                35, TextAlignmentOptions.Center, softCurrency.ToString(), GameTextRole.CurrencyGold);
            CurrencyBalanceText.Bind(coinsValue, false);

            var premium = CreateImage("Premium Panel", root, new Vector2(1f, 1f), new Vector2(1f, 1f), new Vector2(1f, 1f),
                new Vector2(372f, 155f), new Vector2(-166f, -20f), "vip_coins_panel", true, null);
            var premiumValue = CreateLabel("Premium Value", premium.transform, new Vector2(0.40f, 0.24f), new Vector2(0.80f, 0.80f),
                35, TextAlignmentOptions.Center, premiumCurrency.ToString(), GameTextRole.CurrencyCrystal);
            CurrencyBalanceText.Bind(premiumValue, true);
        }

        private void BuildBottomActions(Transform root)
        {
            var play = CreateImageButton("Play Button", root, new Vector2(0.5f, 0f), new Vector2(0.5f, 0f),
                new Vector2(0.5f, 0f), new Vector2(620f, 413f), PlayButtonPosition, "play_btn", OpenCampaign);
            CreateLabel("Play Label", play.transform, new Vector2(0.20f, 0.19f), new Vector2(0.80f, 0.49f),
                58, TextAlignmentOptions.Center, T("ui.play"));

            var inventory = CreateImageButton("Inventory Button", root, new Vector2(0f, 0f), new Vector2(0f, 0f),
                new Vector2(0f, 0f), SideButtonSize, InventoryButtonPosition, "inventory_btn", OpenLoadout);

            var architect = CreateImageButton("Architect Button", root, new Vector2(0f, 0f), new Vector2(0f, 0f),
                new Vector2(0f, 0f), ArchitectButtonSize, ArchitectButtonPosition, "UI/Common/architect_btn",
                () => ArchitectScreenController.Open(null));
            architect.localScale = new Vector3(ArchitectButtonScale.x, ArchitectButtonScale.y, 1f);

            var shop = CreateImageButton("Shop Button", root, new Vector2(1f, 0f), new Vector2(1f, 0f),
                new Vector2(1f, 0f), SideButtonSize, ShopButtonPosition, "shop_btn", OpenShop);

            CreateImageButton("Rewards Button", root, new Vector2(0.5f, 0f), new Vector2(0.5f, 0f),
                new Vector2(0.5f, 0f), RewardsSize, RewardsPosition, "rewards_btn", OpenRewards);
        }

        private void StartBattle()
        {
            AppServices.Session.SelectNextMission();
            AppServices.SceneLoader.Load(GameSceneNames.Battle);
        }

        private void OpenCampaign()
        {
            ShowCampaignCard();
        }

        public static void OpenBattleModeSelection()
        {
            var controller = FindAnyObjectByType<MenuSceneController>();
            controller?.ShowCampaignCard();
        }

        private void OpenCampaignCardForMission(string missionId)
        {
            if (AppServices.Missions != null)
            {
                for (var missionIndex = 0; missionIndex < AppServices.Missions.Length; missionIndex++)
                {
                    var mission = AppServices.Missions[missionIndex];
                    if (mission == null || !string.Equals(mission.MissionId, missionId, StringComparison.Ordinal))
                    {
                        continue;
                    }

                    for (var sectorIndex = 0; sectorIndex < CampaignPlanetCatalog.Sectors.Length; sectorIndex++)
                    {
                        var planets = CampaignPlanetCatalog.Sectors[sectorIndex].Planets;
                        for (var planetIndex = 0; planetIndex < planets.Length; planetIndex++)
                        {
                            if (!string.Equals(planets[planetIndex].ChapterId, mission.ChapterId, StringComparison.Ordinal))
                            {
                                continue;
                            }

                            _selectedCampaignClusterIndex = sectorIndex;
                            _selectedPlanetIndex = planetIndex;
                            _selectedCampaignMissionId = string.Empty;
                            ShowCampaignCard(restoreLastPlayedPlanet: false);
                            return;
                        }
                    }

                    break;
                }
            }

            ShowCampaignCard(restoreLastPlayedPlanet: false);
        }

        private void ShowCampaignCard(bool restoreLastPlayedPlanet = true)
        {
            if (_campaignCardOverlay != null || _canvasObject == null)
            {
                return;
            }

            if (restoreLastPlayedPlanet)
            {
                RestoreLastPlayedCampaignPlanet();
            }

            var overlay = CreateRect("Campaign Card Overlay", _canvasObject.transform, Vector2.zero, Vector2.one,
                new Vector2(0.5f, 0.5f), Vector2.zero, Vector2.zero);
            _campaignCardOverlay = overlay.gameObject;
            var blocker = overlay.gameObject.AddComponent<Image>();
            blocker.sprite = _whiteSprite;
            blocker.color = new Color(0f, 0f, 0f, 0.84f);
            blocker.raycastTarget = true;

            var card = CreateRect("Campaign Card", overlay, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                new Vector2(0.5f, 0.5f), CampaignCardSize, CampaignCardPosition);
            var frame = CreateImage("Campaign Card Frame", card, Vector2.zero, Vector2.one,
                new Vector2(0.5f, 0.5f), Vector2.zero, Vector2.zero, "UI/Common/rect_card_long", false, null);
            frame.GetComponent<Image>().raycastTarget = true;
            BuildCampaignModeTabs(card);
            _campaignModeContent = CreateRect("Campaign Mode Content", card, new Vector2(0.5f, 0.5f),
                new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), CampaignModeContentSize, Vector2.zero);
            CreateCampaignCardCloseButton(card);
            SelectCampaignMode(CampaignModeTab.Map);
        }

        private void BuildCampaignModeTabs(Component parent)
        {
            _campaignModeTabs.Clear();
            var line = CreateRect("Campaign Mode Tabs Line", parent, new Vector2(0.5f, 0.5f),
                new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), CampaignModeTabsLineSize, new Vector2(-50f, 362f));
            var lineImage = line.gameObject.AddComponent<Image>();
            lineImage.sprite = _whiteSprite;
            lineImage.color = new Color32(0x4C, 0xCE, 0xF9, 0xFF);
            lineImage.raycastTarget = false;

            var labels = new[] { GameText.ModeMap(), GameText.RaidTitle(), GameText.ModeArena(), GameText.ModeBosses() };
            for (var i = 0; i < labels.Length; i++)
            {
                var tab = (CampaignModeTab)i;
                var tabRect = CreateImageButton($"Campaign Mode Tab {tab}", parent,
                    new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                    CampaignModeTabSize, new Vector2((i - 1.5f) * 340f - 90f, 405f), "UI/Generated/campaign_mode_tab_inactive",
                    () => SelectCampaignMode(tab));
                var label = CreateLabel($"Campaign Mode Tab {tab} Label", tabRect, Vector2.zero, Vector2.one,
                    40, TextAlignmentOptions.Center, labels[i], GameTextRole.ScreenTitle);
                label.fontWeight = FontWeight.Bold;
                label.rectTransform.anchoredPosition = new Vector2(0f, -10f);
                label.rectTransform.sizeDelta = new Vector2(-32f, -16f);
                _campaignModeTabs.Add((tabRect.GetComponent<Image>(), label));
            }
        }

        private void SelectCampaignMode(CampaignModeTab mode)
        {
            if (mode == CampaignModeTab.Map)
            {
                _selectedCampaignMissionId = string.Empty;
            }

            _selectedCampaignMode = mode;
            for (var i = 0; i < _campaignModeTabs.Count; i++)
            {
                var active = i == (int)mode;
                _campaignModeTabs[i].Image.sprite = LoadSprite(active ? "UI/Generated/campaign_mode_tab_active" : "UI/Generated/campaign_mode_tab_inactive");
                _campaignModeTabs[i].Label.color = active ? SciFiTextStyle.GoldText : SciFiTextStyle.CyanText;
            }

            if (_campaignModeContent == null)
            {
                return;
            }

            for (var i = _campaignModeContent.childCount - 1; i >= 0; i--)
            {
                var previousContent = _campaignModeContent.GetChild(i).gameObject;
                previousContent.SetActive(false);
                Destroy(previousContent);
            }

            switch (mode)
            {
                case CampaignModeTab.Map:
                    BuildMapModeContent(_campaignModeContent);
                    break;
                case CampaignModeTab.Raid:
                    BuildRaidModeContent(_campaignModeContent);
                    break;
                case CampaignModeTab.Arena:
                    BuildArenaModeContent(_campaignModeContent);
                    break;
                case CampaignModeTab.Bosses:
                    BuildBossModeContent(_campaignModeContent);
                    break;
            }
        }

        private void BuildMapModeContent(Transform parent)
        {
            var left = CreateModePanel(parent, "Campaign Planet Panel", CampaignMapPlanetPanelPosition, CampaignMapPlanetPanelSize);
            var sector = CampaignPlanetCatalog.Sectors[Mathf.Clamp(_selectedCampaignClusterIndex, 0, CampaignPlanetCatalog.Sectors.Length - 1)];
            CreateModeImage(left.transform, "Campaign Sector Background", $"UI/Generated/Campaign/{sector.BackgroundResource}", Vector2.zero, CampaignSectorBackgroundSize);
            BuildCampaignSectorHeader(left.transform, sector);
            var planets = sector.Planets;
            _campaignPlanetButtons.Clear();
            var planetPositions = new Vector2[planets.Length];
            for (var i = 0; i < planets.Length; i++)
            {
                planetPositions[i] = ScaleCampaignMapPosition(planets[i].Position);
            }
            for (var i = 0; i < planetPositions.Length - 1; i++)
            {
                var connectorWidth = Vector2.Distance(planetPositions[i], planetPositions[i + 1]);
                CreateCampaignMapConnector(left.transform, planetPositions[i], planetPositions[i + 1], connectorWidth);
            }

            for (var i = 0; i < planets.Length; i++)
            {
                var planetIndex = i;
                var planet = planets[i];
                var position = planetPositions[i];
                var planetButton = CreateImageButton($"Campaign Planet {i + 1}", left.transform,
                    new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                    new Vector2(180f, 180f), position, $"UI/Generated/Campaign/{planet.MapResource}",
                    () => SelectCampaignPlanet(planetIndex));
                var planetSelectable = planetButton.GetComponent<Button>();
                var isSectorLocked = !IsCampaignSectorUnlocked(_selectedCampaignClusterIndex);
                var isReachable = !isSectorLocked && IsPlanetReachable(planetIndex);
                planetSelectable.interactable = true;
                planetButton.GetComponent<Image>().preserveAspect = true;
                planetButton.GetComponent<Image>().color = isSectorLocked ? new Color(0.38f, 0.42f, 0.48f, 1f) : Color.white;
                _campaignPlanetButtons.Add(planetButton);
                ApplyCampaignPlanetSelection(planetButton, i == _selectedPlanetIndex);
                CreateModeText(planetButton, $"Planet {i + 1} Label", GameText.Localized(planet.LocalizationKey, string.Empty), new Vector2(0f, -125f), 19,
                    TextAlignmentOptions.Center, isReachable ? SciFiTextStyle.GoldText : SciFiTextStyle.MainText);
                if (isSectorLocked || !isReachable)
                {
                    var lockIcon = CreateImage($"Campaign Planet {i + 1} Lock", planetButton, new Vector2(0.5f, 0.5f),
                        new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(72f, 72f), Vector2.zero,
                        "UI/Shop/lock", true, Color.white);
                    lockIcon.GetComponent<Image>().raycastTarget = false;
                }
            }

            _campaignMapDetails = CreateRect("Campaign Map Details", parent, new Vector2(0.5f, 0.5f),
                new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), CampaignModeRightSize, CampaignMapMissionContentPosition);
            BuildCampaignMapDetails(_campaignMapDetails, planets);
            BuildCampaignClusterNavigation(parent);
            BuildCampaignSectorPaginator(parent);
        }

        private void BuildCampaignSectorHeader(Transform parent, CampaignPlanetCatalog.Sector sector)
        {
            CreateModeText(parent, "Campaign Sector Header", GameText.CampaignSectorName(_selectedCampaignClusterIndex + 1),
                new Vector2(-320f, 278f), 30, TextAlignmentOptions.MidlineLeft, SciFiTextStyle.GoldText, 470f);

            var card = CreateImage("Campaign Sector Progress Card", parent, new Vector2(0.5f, 0.5f),
                new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), CampaignSectorProgressCardSize,
                CampaignSectorProgressCardPosition, "UI/Common/card2", false, Color.white);
            card.GetComponent<Image>().raycastTarget = false;

            var completed = GetCompletedCampaignSectorMissionCount(_selectedCampaignClusterIndex);
            var total = GetCampaignSectorMissionCount(_selectedCampaignClusterIndex);
            var star = CreateRect("Campaign Sector Progress Star", card, new Vector2(0.5f, 0.5f),
                new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(42f, 42f), new Vector2(-50f, 0f));
            var starImage = star.gameObject.AddComponent<Image>();
            starImage.sprite = LoadSprite("UI/Generated/Campaign/campaign_progress_star");
            starImage.preserveAspect = true;
            starImage.raycastTarget = false;
            CreateModeText(card, "Campaign Sector Progress Value", GameText.CampaignSectorMissionProgress(completed, total),
                new Vector2(0f, 0f), 22, TextAlignmentOptions.Center, SciFiTextStyle.MainText, 76f);

            var info = CreateImageButton("Campaign Sector Progress Info", card, new Vector2(0.5f, 0.5f),
                new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(40f, 40f), new Vector2(50f, 0f),
                "UI/Generated/Campaign/campaign_progress_info", ShowCampaignSectorProgressDialog);
            info.GetComponent<Image>().color = Color.white;
        }

        private static Vector2 ScaleCampaignMapPosition(Vector2 position)
        {
            return new Vector2(
                position.x * CampaignMapPlanetPanelSize.x / 1106f,
                position.y * CampaignMapPlanetPanelSize.y / 560f);
        }

        private void BuildCampaignMapDetails(Transform parent, CampaignPlanetCatalog.Planet[] planets)
        {
            for (var i = parent.childCount - 1; i >= 0; i--)
            {
                var old = parent.GetChild(i).gameObject;
                old.SetActive(false);
                Destroy(old);
            }

            var selected = planets[Mathf.Clamp(_selectedPlanetIndex, 0, planets.Length - 1)];
            var isSectorLocked = !IsCampaignSectorUnlocked(_selectedCampaignClusterIndex);
            var isReachable = !isSectorLocked && IsPlanetReachable(_selectedPlanetIndex);
            if (!isReachable || string.IsNullOrWhiteSpace(selected.ChapterId))
            {
                CreateModeText(parent, "Campaign Planet Lock Message", GameText.CampaignPreviousPlanetLockedFromSigma(),
                    Vector2.zero, 26, TextAlignmentOptions.Center, SciFiTextStyle.SecondaryText, CampaignRightTextWidth);
                return;
            }

            CreateModeText(parent, "Campaign Missions Heading", GameText.CampaignMissionsForPlanet(
                    GameText.Localized(selected.LocalizationKey, string.Empty)), new Vector2(20f, 310f),
                24, TextAlignmentOptions.MidlineLeft, SciFiTextStyle.GoldText, CampaignMissionCardSize.x);

            var hasIncompleteMission = EnsureSelectedCampaignMission(selected.ChapterId);
            var missionCount = 0;
            if (AppServices.Missions != null)
            {
                for (var i = 0; i < AppServices.Missions.Length && missionCount < 4; i++)
                {
                    var mission = AppServices.Missions[i];
                    if (mission == null || !string.Equals(mission.ChapterId, selected.ChapterId, System.StringComparison.Ordinal))
                    {
                        continue;
                    }

                    var completed = IsCampaignMissionCompleted(mission.MissionId);
                    BuildCampaignMissionCard(parent, mission, missionCount, completed,
                        string.Equals(mission.MissionId, _selectedCampaignMissionId, StringComparison.Ordinal));
                    missionCount++;
                }
            }

            if (missionCount == 0)
            {
                CreateModeText(parent, "Campaign Planet Lock Message", GameText.CampaignPreviousPlanetLockedFromSigma(),
                    Vector2.zero, 26, TextAlignmentOptions.Center, SciFiTextStyle.SecondaryText, CampaignRightTextWidth);
                return;
            }

            if (hasIncompleteMission && IsSelectedCampaignMissionAvailable(selected.ChapterId))
            {
                CreateModeButton(parent, "Open Selected Mission", GameText.EnterBattle(),
                    new Vector2(0f, -275f), StartSelectedCampaignMission, true);
            }
        }

        private void BuildCampaignMissionCard(Transform parent, MissionDefinition mission, int missionIndex, bool completed, bool selected)
        {
            var card = CreateImage($"Campaign Mission Card {missionIndex + 1}", parent, new Vector2(0.5f, 0.5f),
                new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), CampaignMissionCardSize,
                new Vector2(20f, 225f - missionIndex * 120f), "UI/Menu/Rewards/reward_card_row_ui_v6", false, Color.white);
            var cardImage = card.GetComponent<Image>();
            cardImage.raycastTarget = true;
            var cardButton = card.gameObject.AddComponent<Button>();
            cardButton.targetGraphic = cardImage;
            cardButton.transition = Selectable.Transition.None;
            cardButton.onClick.AddListener(() => SelectCampaignMission(mission.MissionId));
            if (selected)
            {
                var outline = card.gameObject.AddComponent<Outline>();
                outline.effectColor = new Color(0.2f, 0.9f, 1f, 1f);
                outline.effectDistance = new Vector2(6f, 6f);
                card.localScale = new Vector3(1.02f, 1.02f, 1f);
            }
            else
            {
                card.localScale = new Vector3(0.98f, 0.98f, 1f);
            }

            var darkCardText = new Color(0.04f, 0.10f, 0.16f, 1f);
            var number = CreateModeText(card, $"Campaign Mission {missionIndex + 1} Number", (missionIndex + 1).ToString(),
                new Vector2(-285f, 0f), 32, TextAlignmentOptions.Center, darkCardText, 54f);
            number.fontWeight = FontWeight.Bold;

            CreateModeText(card, $"Campaign Mission {missionIndex + 1} Description",
                GameText.MissionDescription(mission.MissionId, string.Empty), new Vector2(-20f, -5f), 20,
                TextAlignmentOptions.TopLeft, darkCardText, 470f);

            if (completed)
            {
                var check = CreateImage($"Campaign Mission {missionIndex + 1} Completed", card, new Vector2(0.5f, 0.5f),
                    new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(35f, 35f), new Vector2(280f, -5f),
                    "UI/Menu/Rewards/Icons/check", true, SciFiTextStyle.SuccessText);
                check.GetComponent<Image>().raycastTarget = false;
            }
        }

        private void CreateCampaignMapConnector(Transform parent, Vector2 start, Vector2 end, float width)
        {
            var direction = end - start;
            var connector = CreateImage("Campaign Planet Connector", parent, new Vector2(0.5f, 0.5f),
                new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(width, 34f),
                (start + end) * 0.5f, "UI/Generated/Campaign/campaign_map_connector", false, Color.white);
            connector.localEulerAngles = new Vector3(0f, 0f, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
            connector.GetComponent<Image>().raycastTarget = false;

            var dot = CreateImage("Campaign Planet Connector Dot", parent, new Vector2(0.5f, 0.5f),
                new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(42f, 42f),
                (start + end) * 0.5f, "UI/Generated/Campaign/campaign_map_dot", true, Color.white);
            dot.GetComponent<Image>().raycastTarget = false;
        }

        private void BuildCampaignClusterNavigation(Transform parent)
        {
            var leftArrow = CreateImageButton("Campaign Cluster Left", parent,
                new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                CampaignClusterArrowSize, CampaignClusterLeftArrowPosition, "UI/Generated/Campaign/campaign_map_chevron",
                () => ChangeCampaignCluster(-1));
            leftArrow.GetComponent<Image>().preserveAspect = true;
            leftArrow.localScale = new Vector3(-1f, 1f, 1f);
            leftArrow.GetComponent<Button>().interactable = _selectedCampaignClusterIndex > 0;

            var rightArrow = CreateImageButton("Campaign Cluster Right", parent,
                new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                CampaignClusterArrowSize, CampaignClusterRightArrowPosition, "UI/Generated/Campaign/campaign_map_chevron",
                () => ChangeCampaignCluster(1));
            rightArrow.GetComponent<Image>().preserveAspect = true;
            rightArrow.localScale = Vector3.one;
            rightArrow.GetComponent<Button>().interactable = _selectedCampaignClusterIndex < CampaignClusterCount - 1;

        }

        private void BuildCampaignSectorPaginator(Transform parent)
        {
            const float spacing = 38f;
            var start = -((CampaignSectorPaginatorCount - 1) * spacing) * 0.5f;
            var activeSectorIndex = GetActiveCampaignSectorIndex();
            for (var i = 0; i < CampaignSectorPaginatorCount; i++)
            {
                var isCompleted = i < CampaignClusterCount && IsCampaignSectorComplete(i);
                var isViewed = i == _selectedCampaignClusterIndex;
                var isActive = i == activeSectorIndex;
                var dot = CreateRect($"Campaign Sector Page {i + 1}", parent, new Vector2(0.5f, 0.5f),
                    new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), isViewed ? new Vector2(36f, 36f) : new Vector2(18f, 18f),
                    new Vector2(CampaignMapPlanetPanelPosition.x + start + i * spacing, -310f));
                var image = dot.gameObject.AddComponent<Image>();
                image.sprite = LoadSprite("UI/Generated/Campaign/campaign_map_dot");
                image.color = isCompleted ? SciFiTextStyle.SuccessText : isActive ? SciFiTextStyle.CyanText : new Color(0.18f, 0.38f, 0.52f, 0.8f);
                image.raycastTarget = false;
            }
        }

        private int GetActiveCampaignSectorIndex()
        {
            for (var i = 0; i < CampaignClusterCount; i++)
            {
                if (!IsCampaignSectorComplete(i))
                {
                    return i;
                }
            }

            return CampaignClusterCount - 1;
        }

        private int GetCompletedCampaignSectorMissionCount(int sectorIndex)
        {
            var sectors = CampaignPlanetCatalog.Sectors;
            if (sectorIndex < 0 || sectorIndex >= sectors.Length || AppServices.Missions == null)
            {
                return 0;
            }

            var chapters = new HashSet<string>();
            for (var i = 0; i < sectors[sectorIndex].Planets.Length; i++)
            {
                var chapterId = sectors[sectorIndex].Planets[i].ChapterId;
                if (!string.IsNullOrWhiteSpace(chapterId))
                {
                    chapters.Add(chapterId);
                }
            }

            var completed = 0;
            for (var i = 0; i < AppServices.Missions.Length; i++)
            {
                var mission = AppServices.Missions[i];
                if (mission != null && chapters.Contains(mission.ChapterId) && IsCampaignMissionCompleted(mission.MissionId))
                {
                    completed++;
                }
            }

            return completed;
        }

        private int GetCampaignSectorMissionCount(int sectorIndex)
        {
            var sectors = CampaignPlanetCatalog.Sectors;
            if (sectorIndex < 0 || sectorIndex >= sectors.Length || AppServices.Missions == null)
            {
                return 0;
            }

            var chapters = new HashSet<string>();
            for (var i = 0; i < sectors[sectorIndex].Planets.Length; i++)
            {
                var chapterId = sectors[sectorIndex].Planets[i].ChapterId;
                if (!string.IsNullOrWhiteSpace(chapterId))
                {
                    chapters.Add(chapterId);
                }
            }

            var total = 0;
            for (var i = 0; i < AppServices.Missions.Length; i++)
            {
                var mission = AppServices.Missions[i];
                if (mission != null && chapters.Contains(mission.ChapterId))
                {
                    total++;
                }
            }

            return total;
        }

        private void ShowCampaignSectorProgressDialog()
        {
            if (_campaignCardOverlay == null || _campaignSectorProgressDialog != null)
            {
                return;
            }

            var dialog = CreateRect("Campaign Sector Progress Dialog", _campaignCardOverlay.transform, Vector2.zero, Vector2.one,
                new Vector2(0.5f, 0.5f), Vector2.zero, Vector2.zero);
            _campaignSectorProgressDialog = dialog.gameObject;
            var blocker = dialog.gameObject.AddComponent<Image>();
            blocker.sprite = _whiteSprite;
            blocker.color = new Color(0f, 0f, 0f, 0.6f);
            blocker.raycastTarget = true;
            var close = dialog.gameObject.AddComponent<Button>();
            close.targetGraphic = blocker;
            close.onClick.AddListener(HideCampaignSectorProgressDialog);

            var panel = CreateImage("Campaign Sector Progress Dialog Panel", dialog, new Vector2(0.5f, 0.5f),
                new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(520f, 210f), Vector2.zero,
                "UI/Common/card2", false, Color.white);
            panel.GetComponent<Image>().raycastTarget = true;
            var total = GetCampaignSectorMissionCount(_selectedCampaignClusterIndex);
            CreateModeText(panel, "Campaign Sector Progress Dialog Text",
                GameText.CampaignSectorProgressHint(GameText.CampaignSectorName(_selectedCampaignClusterIndex + 1), total),
                Vector2.zero, 25, TextAlignmentOptions.Center, SciFiTextStyle.MainText, 430f);
        }

        private void HideCampaignSectorProgressDialog()
        {
            if (_campaignSectorProgressDialog == null)
            {
                return;
            }

            Destroy(_campaignSectorProgressDialog);
            _campaignSectorProgressDialog = null;
        }

        private void BuildLockedCampaignCluster(Transform parent)
        {
            var panel = CreateModePanel(parent, "Locked Campaign Cluster", Vector2.zero, new Vector2(1100f, 430f));
            CreateModeText(panel.transform, "Locked Campaign Cluster Title", GameText.ModeCluster(_selectedCampaignClusterIndex + 1),
                new Vector2(0f, 100f), 40, TextAlignmentOptions.Center, SciFiTextStyle.GoldText);
            CreateModeText(panel.transform, "Locked Campaign Cluster Hint", GameText.ModeClusterLocked(),
                new Vector2(0f, -20f), 27, TextAlignmentOptions.Center, SciFiTextStyle.SecondaryText);
        }

        private void ChangeCampaignCluster(int direction)
        {
            var next = Mathf.Clamp(_selectedCampaignClusterIndex + direction, 0, CampaignClusterCount - 1);

            _selectedCampaignClusterIndex = next;
            _selectedPlanetIndex = 0;
            _selectedCampaignMissionId = string.Empty;
            SelectCampaignMode(CampaignModeTab.Map);
        }

        private bool IsPlanetReachable(int planetIndex)
        {
            if (AppServices.IsAdminModeEnabled)
            {
                return true;
            }

            if (planetIndex <= 0)
            {
                return true;
            }

            var sector = CampaignPlanetCatalog.Sectors[Mathf.Clamp(_selectedCampaignClusterIndex, 0, CampaignPlanetCatalog.Sectors.Length - 1)];
            var previous = sector.Planets[planetIndex - 1];
            if (string.IsNullOrWhiteSpace(previous.ChapterId) || AppServices.Missions == null)
            {
                return false;
            }

            return HasCompletedMissionForChapter(previous.ChapterId)
                || HasUnlockedMissionForChapter(sector.Planets[planetIndex].ChapterId);
        }

        private static bool HasCompletedMissionForChapter(string chapterId)
        {
            for (var i = 0; i < AppServices.Missions.Length; i++)
            {
                var mission = AppServices.Missions[i];
                if (mission != null && string.Equals(mission.ChapterId, chapterId, StringComparison.Ordinal)
                    && IsCampaignMissionCompleted(mission.MissionId))
                {
                    return true;
                }
            }

            return false;
        }

        private static bool HasUnlockedMissionForChapter(string chapterId)
        {
            var highestUnlockedMissionIndex = AppServices.SaveService?.Current?.campaign?.highestUnlockedMissionIndex ?? -1;
            for (var i = 0; i < AppServices.Missions.Length; i++)
            {
                var mission = AppServices.Missions[i];
                if (mission != null && string.Equals(mission.ChapterId, chapterId, StringComparison.Ordinal))
                {
                    return highestUnlockedMissionIndex >= i;
                }
            }

            return false;
        }

        private bool IsCampaignSectorUnlocked(int sectorIndex)
        {
            if (AppServices.IsAdminModeEnabled)
            {
                return true;
            }

            if (sectorIndex <= 0)
            {
                return true;
            }

            return IsCampaignSectorComplete(sectorIndex - 1);
        }

        private bool IsCampaignClusterComplete()
        {
            return IsCampaignSectorComplete(0);
        }

        private bool IsCampaignSectorComplete(int sectorIndex)
        {
            var planets = CampaignPlanetCatalog.Sectors[Mathf.Clamp(sectorIndex, 0, CampaignPlanetCatalog.Sectors.Length - 1)].Planets;
            for (var planetIndex = 0; planetIndex < planets.Length; planetIndex++)
            {
                var chapterId = planets[planetIndex].ChapterId;
                if (string.IsNullOrWhiteSpace(chapterId) || AppServices.Missions == null)
                {
                    return false;
                }

                var hasMission = false;
                for (var i = 0; i < AppServices.Missions.Length; i++)
                {
                    var mission = AppServices.Missions[i];
                    if (mission == null || !string.Equals(mission.ChapterId, chapterId, StringComparison.Ordinal))
                    {
                        continue;
                    }

                    hasMission = true;
                    if (!IsCampaignMissionCompleted(mission.MissionId))
                    {
                        return false;
                    }
                }

                if (!hasMission)
                {
                    return false;
                }
            }

            return true;
        }

        private static bool IsCampaignMissionCompleted(string missionId)
        {
            var completed = AppServices.SaveService?.Current?.campaign?.completedMissionIds;
            return completed != null && completed.Contains(missionId);
        }

        private void BuildRaidModeContent(Transform parent)
        {
            CreateModeImage(parent, "Raid Preview", "UI/Menu/sigma_card", CampaignModeLeftPosition, CampaignModeLeftSize);
            CreateModeText(parent, "Raid Preview Title", GameText.ModePreview(), new Vector2(CampaignModeLeftPosition.x, 190f), 34, TextAlignmentOptions.Center, SciFiTextStyle.GoldText);
            CreateModeText(parent, "Raid Preview Hint", GameText.ModeRaidHint(), new Vector2(CampaignModeLeftPosition.x, -190f), 23, TextAlignmentOptions.Center, SciFiTextStyle.SecondaryText);
            BuildDifficultyModePanel(parent, "Raid Difficulty Panel", GameText.ModeDifficultyHint(), StartSelectedRaid);
        }

        private void BuildArenaModeContent(Transform parent)
        {
            CreateModeImage(parent, "Arena Preview", "UI/Menu/sigma_card", CampaignModeLeftPosition, CampaignModeLeftSize);
            CreateModeText(parent, "Arena Preview Title", GameText.ModeOnline(), new Vector2(CampaignModeLeftPosition.x, 190f), 34, TextAlignmentOptions.Center, SciFiTextStyle.GoldText);
            CreateModeText(parent, "Arena Preview Hint", GameText.ModeArenaHint(), new Vector2(CampaignModeLeftPosition.x, -190f), 23, TextAlignmentOptions.Center, SciFiTextStyle.SecondaryText);
            var right = CreateModePanel(parent, "Arena Action Panel", CampaignModeRightPosition, CampaignModeRightSize);
            CreateModeText(right.transform, "Arena Action Title", GameText.DuelTitle(), new Vector2(0f, 110f), 38, TextAlignmentOptions.Center, SciFiTextStyle.GoldText, CampaignRightTextWidth);
            CreateModeText(right.transform, "Arena Action Hint", GameText.ModeArenaHint(), new Vector2(0f, 25f), 25, TextAlignmentOptions.Center, SciFiTextStyle.SecondaryText, CampaignRightTextWidth);
            CreateModeButton(right.transform, "Find Arena Match", GameText.EnterBattle(), new Vector2(0f, -130f), OpenDuel, true);
        }

        private void BuildBossModeContent(Transform parent)
        {
            var panel = CreateModePanel(parent, "Bosses Panel", Vector2.zero, new Vector2(1060f, 430f));
            CreateModeText(panel.transform, "Bosses Title", GameText.ModeBosses(), new Vector2(0f, 100f), 40, TextAlignmentOptions.Center, SciFiTextStyle.GoldText);
            CreateModeText(panel.transform, "Bosses Hint", GameText.ModeBossesHint(), new Vector2(0f, -20f), 27, TextAlignmentOptions.Center, SciFiTextStyle.SecondaryText);
        }

        private void BuildDifficultyModePanel(Transform parent, string name, string hint, Action startAction)
        {
            var right = CreateModePanel(parent, name, CampaignModeRightPosition, CampaignModeRightSize);
            CreateModeText(right.transform, "Difficulty Hint", hint, new Vector2(0f, 135f), 23, TextAlignmentOptions.Center, SciFiTextStyle.SecondaryText, CampaignRightTextWidth);
            CreateModeText(right.transform, "Difficulty Label", GameText.RaidDifficultyLabel(), new Vector2(0f, 50f), 28, TextAlignmentOptions.Center, SciFiTextStyle.GoldText, CampaignRightTextWidth);
            CreateDifficultyButton(right.transform, "Raid Easy", GameText.RaidDifficultyEasy(), new Vector2(-150f, -20f), _selectedRaidDifficulty == RaidDifficulty.Easy, () => SelectRaidDifficulty(RaidDifficulty.Easy));
            CreateDifficultyButton(right.transform, "Raid Normal", GameText.RaidDifficultyNormal(), new Vector2(0f, -20f), _selectedRaidDifficulty == RaidDifficulty.Normal, () => SelectRaidDifficulty(RaidDifficulty.Normal));
            CreateDifficultyButton(right.transform, "Raid Hard", GameText.RaidDifficultyHard(), new Vector2(150f, -20f), _selectedRaidDifficulty == RaidDifficulty.Hard, () => SelectRaidDifficulty(RaidDifficulty.Hard));
            CreateModeButton(right.transform, "Start Mode", GameText.EnterBattle(), new Vector2(0f, -145f), startAction, true);
        }

        private void CreateDifficultyButton(Transform parent, string name, string label, Vector2 position, bool selected, Action onClick)
        {
            var button = CreateModeButton(parent, name, label, position, onClick);
            button.sizeDelta = new Vector2(150f, 52f);
            var image = button.GetComponent<Image>();
            image.sprite = LoadSprite(selected ? "UI/Generated/campaign_mode_tab_active" : "UI/Generated/campaign_mode_tab_inactive");
            image.color = Color.white;
            image.preserveAspect = false;
            var text = button.GetComponentInChildren<TMP_Text>();
            text.color = selected ? SciFiTextStyle.GoldText : SciFiTextStyle.CyanText;
        }

        private RectTransform CreateModePanel(Transform parent, string name, Vector2 position, Vector2 size)
        {
            var panel = CreateRect(name, parent, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                new Vector2(0.5f, 0.5f), size, position);
            var image = panel.gameObject.AddComponent<Image>();
            image.sprite = _whiteSprite;
            image.color = new Color(1f, 1f, 1f, 0f);
            image.raycastTarget = false;
            AddModeSectionBorder(panel.gameObject);
            return panel;
        }

        private RectTransform CreateModeImage(Transform parent, string name, string resource, Vector2 position, Vector2 size)
        {
            var image = CreateImage(name, parent, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                new Vector2(0.5f, 0.5f), size, position, resource, false, Color.white);
            image.GetComponent<Image>().raycastTarget = false;
            AddModeSectionBorder(image.gameObject);
            return image;
        }

        private static void AddModeSectionBorder(GameObject target)
        {
            const float thickness = 2f;
            var color = new Color(0.31f, 0.85f, 1f, 0.92f);
            var sprite = target.GetComponent<Image>()?.sprite;
            var parent = target.transform;

            CreateModeBorderLine("Border Top", parent, new Vector2(0f, 1f), new Vector2(1f, 1f),
                new Vector2(0.5f, 1f), new Vector2(0f, thickness), Vector2.zero, sprite, color);
            CreateModeBorderLine("Border Bottom", parent, Vector2.zero, new Vector2(1f, 0f),
                new Vector2(0.5f, 0f), new Vector2(0f, thickness), Vector2.zero, sprite, color);
            CreateModeBorderLine("Border Left", parent, Vector2.zero, new Vector2(0f, 1f),
                new Vector2(0f, 0.5f), new Vector2(thickness, 0f), Vector2.zero, sprite, color);
            CreateModeBorderLine("Border Right", parent, new Vector2(1f, 0f), Vector2.one,
                new Vector2(1f, 0.5f), new Vector2(thickness, 0f), Vector2.zero, sprite, color);
        }

        private static void CreateModeBorderLine(string name, Transform parent, Vector2 anchorMin, Vector2 anchorMax,
            Vector2 pivot, Vector2 size, Vector2 position, Sprite sprite, Color color)
        {
            var line = CreateRect(name, parent, anchorMin, anchorMax, pivot, size, position);
            var image = line.gameObject.AddComponent<Image>();
            image.sprite = sprite;
            image.color = color;
            image.raycastTarget = false;
        }

        private TMP_Text CreateModeText(Transform parent, string name, string value, Vector2 position, int fontSize,
            TextAlignmentOptions alignment, Color color, float width = 560f)
        {
            var text = CreateLabel(name, parent, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), fontSize, alignment, value);
            text.fontSize = fontSize + 8;
            text.rectTransform.sizeDelta = new Vector2(width, 78f);
            text.rectTransform.anchoredPosition = position;
            text.color = color;
            text.textWrappingMode = TextWrappingModes.Normal;
            text.overflowMode = TextOverflowModes.Overflow;
            text.raycastTarget = false;
            return text;
        }

        private RectTransform CreateModeButton(Transform parent, string name, string label, Vector2 position, Action onClick, bool green = false)
        {
            var button = CreateRect(name, parent, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                new Vector2(0.5f, 0.5f), green ? CampaignStartButtonSize : new Vector2(310f, 88f), position);
            var image = button.gameObject.AddComponent<Image>();
            image.sprite = green ? LoadSprite("UI/Shop/btn_green") : _whiteSprite;
            image.color = Color.white;
            image.raycastTarget = true;
            var selectable = button.gameObject.AddComponent<Button>();
            selectable.targetGraphic = image;
            selectable.onClick.AddListener(() => onClick?.Invoke());
            var text = CreateLabel($"{name} Label", button, Vector2.zero, Vector2.one, 25, TextAlignmentOptions.Center, label);
            text.fontSize = green ? 34 : 27;
            if (green)
            {
                text.fontWeight = FontWeight.Bold;
            }
            text.color = Color.white;
            text.raycastTarget = false;
            return button;
        }

        private void StartSelectedCampaignMission()
        {
            if (AppServices.Session == null)
            {
                return;
            }

            var sector = CampaignPlanetCatalog.Sectors[Mathf.Clamp(_selectedCampaignClusterIndex, 0, CampaignPlanetCatalog.Sectors.Length - 1)];
            var selectedPlanet = sector.Planets[Mathf.Clamp(_selectedPlanetIndex, 0, sector.Planets.Length - 1)];
            if (string.IsNullOrWhiteSpace(_selectedCampaignMissionId))
            {
                return;
            }

            var missionIndex = -1;
            if (AppServices.Missions != null)
            {
                for (var i = 0; i < AppServices.Missions.Length; i++)
                {
                    var mission = AppServices.Missions[i];
                    if (mission != null && string.Equals(mission.MissionId, _selectedCampaignMissionId, StringComparison.Ordinal)
                        && string.Equals(mission.ChapterId, selectedPlanet.ChapterId, StringComparison.Ordinal)
                        && !IsCampaignMissionCompleted(mission.MissionId)
                        && IsCampaignMissionAvailable(selectedPlanet.ChapterId, mission.MissionId))
                    {
                        missionIndex = i;
                        break;
                    }
                }
            }

            if (missionIndex < 0)
            {
                return;
            }

            RememberLastPlayedCampaignPlanet(selectedPlanet.ChapterId);
            AppServices.Session.SelectMission(missionIndex);
            HideCampaignCard();
            AppServices.SceneLoader.Load(GameSceneNames.Battle);
        }

        private void RestoreLastPlayedCampaignPlanet()
        {
            var chapterId = AppServices.SaveService?.Current?.campaign?.lastPlayedPlanetChapterId;
            if (string.IsNullOrWhiteSpace(chapterId))
            {
                return;
            }

            for (var sectorIndex = 0; sectorIndex < CampaignPlanetCatalog.Sectors.Length; sectorIndex++)
            {
                var planets = CampaignPlanetCatalog.Sectors[sectorIndex].Planets;
                for (var planetIndex = 0; planetIndex < planets.Length; planetIndex++)
                {
                    if (!string.Equals(planets[planetIndex].ChapterId, chapterId, StringComparison.Ordinal))
                    {
                        continue;
                    }

                    _selectedCampaignClusterIndex = sectorIndex;
                    _selectedPlanetIndex = planetIndex;
                    _selectedCampaignMissionId = string.Empty;
                    return;
                }
            }
        }

        private static void RememberLastPlayedCampaignPlanet(string chapterId)
        {
            var saveService = AppServices.SaveService;
            var saveData = saveService?.Current;
            if (saveData?.campaign == null || string.IsNullOrWhiteSpace(chapterId)
                || string.Equals(saveData.campaign.lastPlayedPlanetChapterId, chapterId, StringComparison.Ordinal))
            {
                return;
            }

            saveData.campaign.lastPlayedPlanetChapterId = chapterId;
            saveService.Save(saveData);
        }

        private bool EnsureSelectedCampaignMission(string chapterId)
        {
            if (AppServices.Missions == null || string.IsNullOrWhiteSpace(chapterId))
            {
                _selectedCampaignMissionId = string.Empty;
                return false;
            }

            var hasSelectedMissionOnPlanet = false;
            var firstIncompleteMissionId = string.Empty;
            for (var i = 0; i < AppServices.Missions.Length; i++)
            {
                var mission = AppServices.Missions[i];
                if (mission == null || !string.Equals(mission.ChapterId, chapterId, StringComparison.Ordinal))
                {
                    continue;
                }

                if (string.Equals(mission.MissionId, _selectedCampaignMissionId, StringComparison.Ordinal))
                {
                    hasSelectedMissionOnPlanet = true;
                }

                if (string.IsNullOrWhiteSpace(firstIncompleteMissionId) && !IsCampaignMissionCompleted(mission.MissionId))
                {
                    firstIncompleteMissionId = mission.MissionId;
                }
            }

            if (!hasSelectedMissionOnPlanet)
            {
                _selectedCampaignMissionId = firstIncompleteMissionId;
            }

            return !string.IsNullOrWhiteSpace(firstIncompleteMissionId);
        }

        private bool IsSelectedCampaignMissionAvailable(string chapterId)
        {
            if (AppServices.Missions == null || string.IsNullOrWhiteSpace(_selectedCampaignMissionId))
            {
                return false;
            }

            for (var i = 0; i < AppServices.Missions.Length; i++)
            {
                var mission = AppServices.Missions[i];
                if (mission != null && string.Equals(mission.MissionId, _selectedCampaignMissionId, StringComparison.Ordinal)
                    && string.Equals(mission.ChapterId, chapterId, StringComparison.Ordinal))
                {
                    return !IsCampaignMissionCompleted(mission.MissionId)
                        && IsCampaignMissionAvailable(chapterId, mission.MissionId);
                }
            }

            return false;
        }

        private bool IsCampaignMissionAvailable(string chapterId, string missionId)
        {
            if (AppServices.Missions == null || string.IsNullOrWhiteSpace(chapterId)
                || string.IsNullOrWhiteSpace(missionId))
            {
                return false;
            }

            var firstMissionId = string.Empty;
            for (var i = 0; i < AppServices.Missions.Length; i++)
            {
                var mission = AppServices.Missions[i];
                if (mission == null || !string.Equals(mission.ChapterId, chapterId, StringComparison.Ordinal))
                {
                    continue;
                }

                if (string.IsNullOrWhiteSpace(firstMissionId))
                {
                    firstMissionId = mission.MissionId;
                }

                if (!string.Equals(mission.MissionId, missionId, StringComparison.Ordinal))
                {
                    continue;
                }

                return string.Equals(missionId, firstMissionId, StringComparison.Ordinal)
                    || IsCampaignMissionCompleted(firstMissionId);
            }

            return false;
        }

        private void SelectCampaignPlanet(int index)
        {
            var planets = CampaignPlanetCatalog.Sectors[Mathf.Clamp(_selectedCampaignClusterIndex, 0, CampaignPlanetCatalog.Sectors.Length - 1)].Planets;
            _selectedPlanetIndex = Mathf.Clamp(index, 0, planets.Length - 1);
            _selectedCampaignMissionId = string.Empty;
            for (var planetIndex = 0; planetIndex < _campaignPlanetButtons.Count; planetIndex++)
            {
                ApplyCampaignPlanetSelection(_campaignPlanetButtons[planetIndex], planetIndex == _selectedPlanetIndex);
            }
            if (_campaignMapDetails != null)
            {
                BuildCampaignMapDetails(_campaignMapDetails, planets);
            }
        }

        private static void ApplyCampaignPlanetSelection(RectTransform planetButton, bool selected)
        {
            if (planetButton == null)
            {
                return;
            }

            planetButton.localScale = selected ? new Vector3(1.14f, 1.14f, 1f) : Vector3.one;
            var outline = planetButton.GetComponent<Outline>();
            if (outline != null)
            {
                UnityEngine.Object.Destroy(outline);
            }
        }

        private void SelectCampaignMission(string missionId)
        {
            if (string.IsNullOrWhiteSpace(missionId) || string.Equals(_selectedCampaignMissionId, missionId, StringComparison.Ordinal))
            {
                return;
            }

            _selectedCampaignMissionId = missionId;
            if (_campaignMapDetails != null)
            {
                var planets = CampaignPlanetCatalog.Sectors[Mathf.Clamp(_selectedCampaignClusterIndex, 0, CampaignPlanetCatalog.Sectors.Length - 1)].Planets;
                BuildCampaignMapDetails(_campaignMapDetails, planets);
            }
        }

        private void SelectRaidDifficulty(RaidDifficulty difficulty)
        {
            _selectedRaidDifficulty = difficulty;
            if (_campaignModeContent != null && _selectedCampaignMode == CampaignModeTab.Raid)
            {
                BuildRaidModeContent(_campaignModeContent);
            }
        }

        private void StartSelectedRaid()
        {
            var session = AppServices.Session;
            if (session == null)
            {
                return;
            }

            var setup = session.PrepareRaid(_selectedRaidDifficulty, Guid.NewGuid().ToString("N"));
            if (!session.SelectRaid(setup))
            {
                return;
            }

            HideCampaignCard();
            AppServices.SceneLoader.Load(GameSceneNames.Battle);
        }

        private void CreateCampaignCardCloseButton(Component parent)
        {
            var close = CreateRect("Close Campaign Card", parent, new Vector2(1f, 1f), new Vector2(1f, 1f),
                new Vector2(0.5f, 0.5f), CampaignCardCloseSize, CampaignCardClosePosition);
            var hitArea = close.gameObject.AddComponent<Image>();
            hitArea.sprite = _whiteSprite;
            hitArea.color = new Color(1f, 1f, 1f, 0f);
            hitArea.raycastTarget = true;
            var button = close.gameObject.AddComponent<Button>();
            button.targetGraphic = hitArea;
            button.transition = Selectable.Transition.None;
            button.onClick.AddListener(HideCampaignCard);

            var label = CreateLabel("Close Campaign Card Label", close, Vector2.zero, Vector2.one,
                72, TextAlignmentOptions.Center, "\u00D7", GameTextRole.DisplayTitle);
            label.rectTransform.localScale = new Vector3(1.5f, 1.5f, 1f);
            label.color = new Color(1f, 0.18f, 0.16f, 1f);
            label.raycastTarget = false;
        }

        private void HideCampaignCard()
        {
            if (_campaignCardOverlay == null)
            {
                return;
            }

            Destroy(_campaignCardOverlay);
            _campaignCardOverlay = null;
            _campaignSectorProgressDialog = null;
        }

        private void OpenCampaignMap()
        {
            ShowCampaignCard();
        }

        private void OpenRaid()
        {
            HideCampaignCard();
            RaidScreenController.Open();
        }

        private void OpenDuel()
        {
            HideCampaignCard();
            DuelScreenController.Open();
        }

        private void OpenLoadout()
        {
            WeaponsScreenController.Open(null);
        }

        private void OpenSettings()
        {
            SettingsScreenController.Open(null);
        }

        private void OpenNameChange()
        {
            PlayerNameScreenController.Open(RefreshAfterNameChange);
        }

        private void RefreshAfterNameChange()
        {
            if (_canvasObject == null)
            {
                return;
            }

            Destroy(_canvasObject);
            _campaignCardOverlay = null;
            BuildMenu();
        }

        private void OpenShop()
        {
            ShopScreenController.Open(null);
        }

        private void OpenRewards()
        {
            RewardsScreenController.Open(null);
        }

        private RectTransform CreateImage(string name, Component parent, Vector2 anchorMin, Vector2 anchorMax,
            Vector2 pivot, Vector2 sizeDelta, Vector2 anchoredPosition, string resourceName, bool preserveAspect,
            Color? tint)
        {
            var rect = CreateRect(name, parent, anchorMin, anchorMax, pivot, sizeDelta, anchoredPosition);
            var image = rect.gameObject.AddComponent<Image>();
            image.sprite = LoadSprite(resourceName);
            image.color = tint ?? Color.white;
            image.preserveAspect = preserveAspect;
            image.raycastTarget = false;
            return rect;
        }

        private RectTransform CreateSlicedImage(string name, Component parent, Vector2 anchorMin, Vector2 anchorMax,
            Vector2 pivot, Vector2 sizeDelta, Vector2 anchoredPosition, string resourceName, Vector4 border)
        {
            var rect = CreateRect(name, parent, anchorMin, anchorMax, pivot, sizeDelta, anchoredPosition);
            var image = rect.gameObject.AddComponent<Image>();
            image.sprite = LoadSlicedSprite(resourceName, border);
            image.type = Image.Type.Sliced;
            image.fillCenter = true;
            image.color = Color.white;
            image.raycastTarget = false;
            return rect;
        }

        private RectTransform CreateImageButton(string name, Component parent, Vector2 anchorMin, Vector2 anchorMax,
            Vector2 pivot, Vector2 sizeDelta, Vector2 anchoredPosition, string resourceName, Action onClick)
        {
            var rect = CreateImage(name, parent, anchorMin, anchorMax, pivot, sizeDelta, anchoredPosition, resourceName, true, null);
            var image = rect.GetComponent<Image>();
            image.raycastTarget = true;

            var button = rect.gameObject.AddComponent<Button>();
            button.targetGraphic = image;
            button.transition = Selectable.Transition.ColorTint;
            button.colors = CreateButtonColors();
            button.onClick.AddListener(() => onClick?.Invoke());
            return rect;
        }

        private TMP_Text CreateLabel(string name, Component parent, Vector2 anchorMin, Vector2 anchorMax,
            int fontSize, TextAlignmentOptions alignment, string value, GameTextRole? roleOverride = null)
        {
            var rect = CreateRect(name, parent, anchorMin, anchorMax, new Vector2(0.5f, 0.5f), Vector2.zero, Vector2.zero);
            var text = rect.gameObject.AddComponent<TextMeshProUGUI>();
            var role = roleOverride ?? (fontSize >= 50 ? GameTextRole.DisplayTitle : GameTextRole.Button);
            SciFiTextStyle.Apply(text, role, fontSize, alignment);
            text.fontSizeMin = Mathf.Max(16, fontSize - 14);
            text.textWrappingMode = TextWrappingModes.NoWrap;
            text.overflowMode = TextOverflowModes.Truncate;
            text.text = value;

            var shadow = rect.gameObject.AddComponent<Shadow>();
            shadow.effectColor = TextShadowColor;
            shadow.effectDistance = new Vector2(0f, -3f);
            return text;
        }

        private Sprite LoadSprite(string resourceName)
        {
            if (_sprites.TryGetValue(resourceName, out var cached))
            {
                return cached;
            }

            var resourcePath = resourceName.Contains("/")
                ? resourceName
                : UiResourceRoot + resourceName;
            var importedSprite = Resources.Load<Sprite>(resourcePath);
            if (importedSprite != null)
            {
                _sprites[resourceName] = importedSprite;
                return importedSprite;
            }

            var texture = Resources.Load<Texture2D>(resourcePath);
            if (texture == null)
            {
                return _whiteSprite;
            }

            var sprite = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height),
                new Vector2(0.5f, 0.5f), 100f);
            _sprites[resourceName] = sprite;
            return sprite;
        }

        private Sprite LoadSlicedSprite(string resourceName, Vector4 border)
        {
            var cacheKey = $"{resourceName}#sliced:{border.x}:{border.y}:{border.z}:{border.w}";
            if (_sprites.TryGetValue(cacheKey, out var cached))
            {
                return cached;
            }

            var resourcePath = resourceName.Contains("/")
                ? resourceName
                : UiResourceRoot + resourceName;
            var texture = Resources.Load<Texture2D>(resourcePath);
            if (texture == null)
            {
                return _whiteSprite;
            }

            var sprite = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height),
                new Vector2(0.5f, 0.5f), 100f, 0u, SpriteMeshType.FullRect, border);
            _sprites[cacheKey] = sprite;
            return sprite;
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

        private static string T(string key) => AppServices.Localization?.Translate(key) ?? $"[{key}]";

        private static string ResolvePlayerName(SaveData saveData)
        {
            var name = saveData?.profile?.playerName;
            return string.IsNullOrWhiteSpace(name) || name == "Гравець" ? T("profile.player") : name;
        }
    }
}
