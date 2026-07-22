using System;
using System.Collections.Generic;
using Game.Core;
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

        private static readonly Color BackgroundColor = new(0.02f, 0.025f, 0.03f);
        private static readonly Color TextShadowColor = new(0.02f, 0.03f, 0.05f, 0.88f);
        // Layout uses the Canvas reference resolution (1920 x 1080).
        private static readonly Vector2 CharacterSize = new(560f, 560f);
        private static readonly Vector2 CharacterPosition = new(40f, -10f);
        private static readonly Vector2 PlayButtonPosition = new(0f, -39f);
        private static readonly Vector2 SideButtonSize = new(220f, 220f);
        private static readonly Vector2 InventoryButtonPosition = new(80f, -10f);
        private static readonly Vector2 ShopButtonPosition = new(-80f, -10f);
        private static readonly Vector2 RewardsSize = new(220f, 220f);
        private static readonly Vector2 RewardsPosition = new(480f, -10f);
        private static readonly Vector2 SettingsSize = new(140f, 140f);
        private static readonly Vector2 SettingsPosition = new(-26f, -31f);

        private readonly Dictionary<string, Sprite> _sprites = new();

        private Sprite _whiteSprite;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void CreateForMenuScene()
        {
            if (SceneManager.GetActiveScene().name != GameSceneNames.Menu)
            {
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
            BuildMenu();
        }

        private void Start()
        {
            if (AppServices.TryConsumeFirstSessionAutoStart())
            {
                StartBattle();
                return;
            }

            if (AppServices.TryConsumePendingLoadoutReview())
            {
                LoadoutScreenController.Open(null);
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

        private void BuildTopBar(Transform root, SaveData saveData)
        {
            var softCurrency = Mathf.Max(0, saveData?.economy.softCurrency ?? 0);

            var profile = CreateImage("Profile Panel", root, new Vector2(0f, 1f), new Vector2(0f, 1f), new Vector2(0f, 1f),
                new Vector2(560f, 110f), new Vector2(80f, -40f), "UI/Shop/info_panel", true, null);
            var playerName = CreateLabel("Player Name", profile.transform, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                46, TextAlignmentOptions.Center, "Гравець", GameTextRole.DisplayTitle);
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
                35, TextAlignmentOptions.Center, "0");
        }

        private void BuildBottomActions(Transform root)
        {
            var play = CreateImageButton("Play Button", root, new Vector2(0.5f, 0f), new Vector2(0.5f, 0f),
                new Vector2(0.5f, 0f), new Vector2(620f, 413f), PlayButtonPosition, "play_btn", StartBattle);
            CreateLabel("Play Label", play.transform, new Vector2(0.20f, 0.19f), new Vector2(0.80f, 0.49f),
                58, TextAlignmentOptions.Center, "Play");

            var inventory = CreateImageButton("Inventory Button", root, new Vector2(0f, 0f), new Vector2(0f, 0f),
                new Vector2(0f, 0f), SideButtonSize, InventoryButtonPosition, "inventory_btn", OpenLoadout);

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

        private void OpenLoadout()
        {
            LoadoutScreenController.Open(null);
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
