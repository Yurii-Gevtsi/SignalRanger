using System;
using System.Collections.Generic;
using Game.Core;
using Game.Data.Save;
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
        private static readonly Color DisabledTint = new(0.72f, 0.78f, 0.82f, 0.78f);

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
                new Vector2(0.5f, 0.5f), new Vector2(560f, 560f), new Vector2(0f, 80f),
                "Visuals/Characters/racoon-full", true, null);

            BuildTopBar(root, saveData);
            BuildBottomActions(root);
        }

        private void BuildTopBar(Transform root, SaveData saveData)
        {
            var playerName = string.IsNullOrWhiteSpace(saveData?.profile.playerName)
                ? "PLAYER NAME"
                : saveData.profile.playerName.ToUpperInvariant();
            var softCurrency = Mathf.Max(0, saveData?.economy.softCurrency ?? 0);

            var profile = CreateImage("Profile Panel", root, new Vector2(0f, 1f), new Vector2(0f, 1f), new Vector2(0f, 1f),
                new Vector2(560f, 170f), new Vector2(34f, -24f), "user_panel", true, null);
            CreateLabel("Player Name", profile.transform, new Vector2(0.36f, 0.28f), new Vector2(0.98f, 0.90f),
                35, TextAlignmentOptions.MidlineLeft, playerName);

            CreateImageButton("Settings Button", root, new Vector2(1f, 1f), new Vector2(1f, 1f), new Vector2(1f, 1f),
                new Vector2(124f, 124f), new Vector2(-28f, -25f), "settings_btn", OpenSettings);

            var coins = CreateImage("Coins Panel", root, new Vector2(1f, 1f), new Vector2(1f, 1f), new Vector2(1f, 1f),
                new Vector2(310f, 155f), new Vector2(-486f, -20f), "Coin_panel", true, null);
            CreateLabel("Coins Value", coins.transform, new Vector2(0.34f, 0.22f), new Vector2(0.76f, 0.78f),
                35, TextAlignmentOptions.Center, softCurrency.ToString());

            var premium = CreateImage("Premium Panel", root, new Vector2(1f, 1f), new Vector2(1f, 1f), new Vector2(1f, 1f),
                new Vector2(310f, 155f), new Vector2(-166f, -20f), "vip_coins_panel", true, null);
            CreateLabel("Premium Value", premium.transform, new Vector2(0.36f, 0.22f), new Vector2(0.76f, 0.78f),
                35, TextAlignmentOptions.Center, "0");
        }

        private void BuildBottomActions(Transform root)
        {
            var play = CreateImageButton("Play Button", root, new Vector2(0.5f, 0f), new Vector2(0.5f, 0f),
                new Vector2(0.5f, 0f), new Vector2(620f, 413f), new Vector2(0f, 54f), "play_btn", StartBattle);
            CreateLabel("Play Label", play.transform, new Vector2(0.20f, 0.17f), new Vector2(0.80f, 0.47f),
                58, TextAlignmentOptions.Center, "Play");

            var inventory = CreateImageButton("Inventory Button", root, new Vector2(0f, 0f), new Vector2(0f, 0f),
                new Vector2(0f, 0f), new Vector2(220f, 220f), new Vector2(80f, 72f), "inventory", OpenLoadout);
            CreateLabel("Inventory Label", inventory.transform, new Vector2(-0.20f, -0.07f), new Vector2(1.20f, 0.17f),
                35, TextAlignmentOptions.Center, "Inventory");

            var shop = CreateImageButton("Shop Button", root, new Vector2(1f, 0f), new Vector2(1f, 0f),
                new Vector2(1f, 0f), new Vector2(220f, 220f), new Vector2(-80f, 72f), "shop_btn_inactive", OpenShop);
            CreateLabel("Shop Label", shop.transform, new Vector2(-0.20f, -0.07f), new Vector2(1.20f, 0.17f),
                35, TextAlignmentOptions.Center, "Shop");

            var rewards = CreateImage("Rewards Button", root, new Vector2(0.5f, 1f), new Vector2(0.5f, 1f),
                new Vector2(0.5f, 1f), new Vector2(280f, 210f), new Vector2(0f, -18f), "rewards_btn", true, DisabledTint);
            CreateLabel("Rewards Label", rewards.transform, new Vector2(-0.20f, -0.10f), new Vector2(1.20f, 0.14f),
                35, TextAlignmentOptions.Center, "Rewards");
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
            int fontSize, TextAlignmentOptions alignment, string value)
        {
            var rect = CreateRect(name, parent, anchorMin, anchorMax, new Vector2(0.5f, 0.5f), Vector2.zero, Vector2.zero);
            var text = rect.gameObject.AddComponent<TextMeshProUGUI>();
            text.fontSize = fontSize;
            text.enableAutoSizing = true;
            text.fontSizeMin = Mathf.Max(16, fontSize - 14);
            text.fontSizeMax = fontSize;
            text.fontStyle = FontStyles.Bold;
            text.color = Color.white;
            text.alignment = alignment;
            text.textWrappingMode = TextWrappingModes.NoWrap;
            text.overflowMode = TextOverflowModes.Ellipsis;
            text.text = value;
            text.raycastTarget = false;

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
