using System;
using System.Collections.Generic;
using Game.Core;
using Game.Core.Localization;
using Game.Data.Save;
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
        private const string TrainingMissionId = "mission.training.01";

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
        private static readonly Vector2 CampaignCardSize = new(2340f, 858f);
        private static readonly Vector2 CampaignCardFrameSize = new(2150f, 902f);
        private static readonly Vector2 CampaignCardPosition = new(0f, 20f);
        private static readonly Vector2 CampaignCardScale = Vector2.one;
        private static readonly Vector2 CampaignCardCloseSize = new(112f, 112f);
        private static readonly Vector2 CampaignCardClosePosition = new(-280f, -120f);
        private static readonly Vector2 TrainingCardSize = new(480f, 648f);
        private static readonly Vector2 TrainingCardPosition = new(-640f, -30f);
        private static readonly Vector2 CampaignCardOptionSize = new(480f, 648f);
        private static readonly Vector2 CampaignCardOptionPosition = new(0f, -30f);
        private static readonly Vector2 SigmaCardPosition = new(640f, -30f);
        private static readonly Vector2 CampaignModeTitleSize = new(460f, 82f);
        private static readonly Vector2 CampaignModeTitlePosition = new(0f, 170f);
        private static readonly Vector4 CampaignCardSliceBorder = new(220f, 220f, 220f, 220f);

        private readonly Dictionary<string, Sprite> _sprites = new();

        private Sprite _whiteSprite;
        private GameObject _canvasObject;
        private GameObject _campaignCardOverlay;

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
            var playerName = CreateLabel("Player Name", profile.transform, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                46, TextAlignmentOptions.Center, ResolvePlayerName(saveData), GameTextRole.DisplayTitle);
            playerName.rectTransform.sizeDelta = new Vector2(360f, 90f);
            playerName.rectTransform.anchoredPosition = new Vector2(-100f, 0f);

            CreateImageButton("Settings Button", root, new Vector2(1f, 1f), new Vector2(1f, 1f), new Vector2(1f, 1f),
                SettingsSize, SettingsPosition, "settings_btn", OpenSettings);

            var coins = CreateImage("Coins Panel", root, new Vector2(1f, 1f), new Vector2(1f, 1f), new Vector2(1f, 1f),
                new Vector2(310f, 155f), new Vector2(-486f, -20f), "Coin_panel", true, null);
            CreateLabel("Coins Value", coins.transform, new Vector2(0.34f, 0.24f), new Vector2(0.76f, 0.80f),
                35, TextAlignmentOptions.Center, softCurrency.ToString());

            var premium = CreateImage("Premium Panel", root, new Vector2(1f, 1f), new Vector2(1f, 1f), new Vector2(1f, 1f),
                new Vector2(310f, 155f), new Vector2(-166f, -20f), "vip_coins_panel", true, null);
            CreateLabel("Premium Value", premium.transform, new Vector2(0.36f, 0.24f), new Vector2(0.76f, 0.80f),
                35, TextAlignmentOptions.Center, premiumCurrency.ToString());
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

            var rewards = CreateImage("Rewards Button", root, new Vector2(0.5f, 0f), new Vector2(0.5f, 0f),
                new Vector2(0.5f, 0f), RewardsSize, RewardsPosition, "rewards_btn", true, Color.white);
        }

        private void StartBattle()
        {
            AppServices.Session.SelectNextMission();
            AppServices.SceneLoader.Load(GameSceneNames.Battle);
        }

        private void OpenCampaign()
        {
            if (!IsTrainingMissionCompleted())
            {
                StartFirstCampaignMission();
                return;
            }

            ShowCampaignCard();
        }

        private void ShowCampaignCard()
        {
            if (_campaignCardOverlay != null || _canvasObject == null)
            {
                return;
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
            card.localScale = new Vector3(CampaignCardScale.x, CampaignCardScale.y, 1f);
            CreateSlicedImage("Campaign Card Frame", card, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                new Vector2(0.5f, 0.5f), CampaignCardFrameSize, Vector2.zero, "UI/Common/rect_card",
                CampaignCardSliceBorder);
            var trainingCard = CreateImageButton("Training Battle Card", card, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                new Vector2(0.5f, 0.5f), TrainingCardSize, TrainingCardPosition, "training_card", StartTrainingSkirmish);
            trainingCard.GetComponent<Image>().preserveAspect = false;
            var campaignBattleCard = CreateImage("Campaign Battle Card", card, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                new Vector2(0.5f, 0.5f), CampaignCardOptionSize, CampaignCardOptionPosition, "campaing_card", false, null);
            var sigmaBattleCard = CreateImage("Sigma Battle Card", card, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                new Vector2(0.5f, 0.5f), CampaignCardOptionSize, SigmaCardPosition, "sigma_card", false, null);
            CreateCampaignModeTitle(trainingCard, "campaign.trainingPlanet");
            CreateCampaignModeTitle(campaignBattleCard, "campaign.galaxyLiberation");
            CreateCampaignModeTitle(sigmaBattleCard, "campaign.invasionDefense");
            CreateCampaignCardCloseButton(card);
        }

        private void CreateCampaignModeTitle(Component parent, string localizationKey)
        {
            var title = CreateLabel($"{parent.name} Title", parent, new Vector2(0.5f, 0f), new Vector2(0.5f, 0f),
                44, TextAlignmentOptions.Center, T(localizationKey), GameTextRole.DisplayTitle);
            title.rectTransform.sizeDelta = CampaignModeTitleSize;
            title.rectTransform.anchoredPosition = CampaignModeTitlePosition;
            title.enableAutoSizing = true;
            title.fontSizeMin = 36f;
            title.fontSizeMax = 44f;
            title.raycastTarget = false;
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
        }

        private void StartTrainingSkirmish()
        {
            HideCampaignCard();
            AppServices.Session.SelectTrainingSkirmish();
            AppServices.SceneLoader.Load(GameSceneNames.Battle);
        }

        private static bool IsTrainingMissionCompleted()
        {
            var completedMissions = AppServices.SaveService?.Current?.campaign?.completedMissionIds;
            return completedMissions != null && completedMissions.Contains(TrainingMissionId);
        }

        private void StartFirstCampaignMission()
        {
            AppServices.Session.SelectMission(0);
            AppServices.SceneLoader.Load(GameSceneNames.Battle);
        }

        private void OpenLoadout()
        {
            WeaponsScreenController.Open(null);
        }

        private void OpenSettings()
        {
            SettingsScreenController.Open(null);
        }

        private void OpenShop()
        {
            ShopScreenController.Open(null);
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
