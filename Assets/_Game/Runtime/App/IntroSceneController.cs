using System;
using System.Collections.Generic;
using Game.Core;
using Game.Core.Localization;
using Game.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.App
{
    public sealed class IntroSceneController : MonoBehaviour
    {
        private const string ResourceRoot = "UI/Menu/";
        private const string IntroBackground = "intro";
        private const string NextButton = "UI/Shop/btn_blue";

        private static readonly Color TextShadowColor = new(0.02f, 0.03f, 0.05f, 0.92f);
        private static readonly IntroPage[] Pages =
        {
            new(
                "intro.sigma.title",
                "intro.sigma.body"),
            new(
                "intro.uprising.title",
                "intro.uprising.body"),
            new(
                "intro.ranger.title",
                "intro.ranger.body")
        };

        private readonly Dictionary<string, Sprite> _sprites = new();
        private Sprite _whiteSprite;
        private int _pageIndex;
        private TMP_Text _title;
        private TMP_Text _body;
        private TMP_Text _nextLabel;
        private GameObject _introContentRoot;
        private static bool _shownThisSession;

        public static bool ShouldShow()
        {
            // Temporary QA mode: show the intro once on every app launch.
            return AppServices.IsInitialized && !_shownThisSession;
        }

        public static void EnsureCreated()
        {
            if (FindAnyObjectByType<IntroSceneController>() != null)
            {
                return;
            }

            _shownThisSession = true;
            var root = new GameObject("Intro Scene Controller");
            root.AddComponent<IntroSceneController>();
        }

        private void Awake()
        {
            AppServices.Initialize();
            _whiteSprite = Sprite.Create(Texture2D.whiteTexture, new Rect(0f, 0f, 1f, 1f),
                new Vector2(0.5f, 0.5f), 1f);
            BuildIntro();

            var settings = AppServices.SaveService?.Current?.settings;
            if (settings != null && !settings.hasSelectedLanguage)
            {
                _introContentRoot.SetActive(false);
                SettingsScreenController.OpenLanguagePicker(BeginIntro);
                return;
            }

            BeginIntro();
        }

        private void BeginIntro()
        {
            _introContentRoot.SetActive(true);
            ShowPage(0);
        }

        private void BuildIntro()
        {
            var canvasObject = new GameObject("Intro Canvas");
            canvasObject.transform.SetParent(transform, false);
            var canvas = canvasObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 1000;

            var scaler = canvasObject.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920f, 1080f);
            scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            scaler.matchWidthOrHeight = 0.5f;
            canvasObject.AddComponent<GraphicRaycaster>();

            var root = canvasObject.transform;
            CreateImage("Intro Background", root, Vector2.zero, Vector2.one, new Vector2(0.5f, 0.5f),
                Vector2.zero, Vector2.zero, IntroBackground, false, false);

            var introContent = CreateRect("Intro Content", canvasObject.transform, Vector2.zero, Vector2.one,
                new Vector2(0.5f, 0.5f), Vector2.zero, Vector2.zero);
            _introContentRoot = introContent.gameObject;

            _title = CreateText("Intro Title", introContent, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                new Vector2(0.5f, 1f), new Vector2(900f, 90f), new Vector2(0f, 400f),
                42, TextAlignmentOptions.Center, GameTextRole.ScreenTitle);

            _body = CreateText("Intro Body", introContent, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                new Vector2(0.5f, 0.5f), new Vector2(860f, 390f), new Vector2(0f, 100f),
                27, TextAlignmentOptions.TopLeft, GameTextRole.Body);
            _body.textWrappingMode = TextWrappingModes.Normal;
            _body.overflowMode = TextOverflowModes.Truncate;
            _body.enableAutoSizing = true;
            _body.fontSizeMin = 21f;
            _body.fontSizeMax = 27f;

            var next = CreateImageButton("Intro Next Button", introContent, new Vector2(0.5f, 0.5f),
                new Vector2(0.5f, 0.5f), new Vector2(1f, 0f), new Vector2(250f, 120f),
                new Vector2(520f, -120f), NextButton, Advance);
            _nextLabel = CreateText("Intro Next Label", next.transform, Vector2.zero, Vector2.one,
                new Vector2(0.5f, 0.5f), Vector2.zero, Vector2.zero, 30,
                TextAlignmentOptions.Center, GameTextRole.Button);
            _nextLabel.raycastTarget = false;
        }

        private void ShowPage(int index)
        {
            _pageIndex = Mathf.Clamp(index, 0, Pages.Length - 1);
            var page = Pages[_pageIndex];
            _title.text = T(page.TitleKey);
            _body.text = T(page.BodyKey);
            _nextLabel.text = T("ui.next");
        }

        private void Advance()
        {
            if (_pageIndex < Pages.Length - 1)
            {
                ShowPage(_pageIndex + 1);
                return;
            }

            var save = AppServices.SaveService?.Current;
            if (save?.profile != null)
            {
                save.profile.introCompleted = true;
                AppServices.SaveService.Save(save);
            }

            Destroy(gameObject);
            MenuSceneController.EnsureCreated();
        }

        private TMP_Text CreateText(string name, Component parent, Vector2 anchorMin, Vector2 anchorMax,
            Vector2 pivot, Vector2 sizeDelta, Vector2 anchoredPosition, int fontSize,
            TextAlignmentOptions alignment, GameTextRole role)
        {
            var rect = CreateRect(name, parent, anchorMin, anchorMax, pivot, sizeDelta, anchoredPosition);
            var text = rect.gameObject.AddComponent<TextMeshProUGUI>();
            SciFiTextStyle.Apply(text, role, fontSize, alignment);
            text.fontSizeMin = Mathf.Max(14, fontSize - 10);
            text.textWrappingMode = TextWrappingModes.Normal;
            text.overflowMode = TextOverflowModes.Truncate;
            text.raycastTarget = false;

            var shadow = rect.gameObject.AddComponent<Shadow>();
            shadow.effectColor = TextShadowColor;
            shadow.effectDistance = new Vector2(0f, -2f);
            return text;
        }

        private RectTransform CreateImageButton(string name, Component parent, Vector2 anchorMin, Vector2 anchorMax,
            Vector2 pivot, Vector2 sizeDelta, Vector2 anchoredPosition, string resourceName, Action onClick)
        {
            var rect = CreateImage(name, parent, anchorMin, anchorMax, pivot, sizeDelta, anchoredPosition,
                resourceName, true, true);
            var image = rect.GetComponent<Image>();
            image.raycastTarget = true;
            var button = rect.gameObject.AddComponent<Button>();
            button.targetGraphic = image;
            button.transition = Selectable.Transition.ColorTint;
            button.colors = new ColorBlock
            {
                normalColor = Color.white,
                highlightedColor = new Color(1.06f, 1.06f, 1.06f, 1f),
                pressedColor = new Color(0.88f, 0.92f, 0.95f, 1f),
                selectedColor = Color.white,
                disabledColor = new Color(0.58f, 0.62f, 0.66f, 0.65f),
                colorMultiplier = 1f,
                fadeDuration = 0.08f
            };
            button.onClick.AddListener(() => onClick?.Invoke());
            return rect;
        }

        private RectTransform CreateImage(string name, Component parent, Vector2 anchorMin, Vector2 anchorMax,
            Vector2 pivot, Vector2 sizeDelta, Vector2 anchoredPosition, string resourceName,
            bool preserveAspect, bool raycastTarget)
        {
            var rect = CreateRect(name, parent, anchorMin, anchorMax, pivot, sizeDelta, anchoredPosition);
            var image = rect.gameObject.AddComponent<Image>();
            image.sprite = LoadSprite(resourceName);
            image.preserveAspect = preserveAspect;
            image.raycastTarget = raycastTarget;
            return rect;
        }

        private Sprite LoadSprite(string resourceName)
        {
            var resourcePath = resourceName.Contains("/") ? resourceName : ResourceRoot + resourceName;
            if (_sprites.TryGetValue(resourcePath, out var cached))
            {
                return cached;
            }

            var texture = Resources.Load<Texture2D>(resourcePath);
            if (texture == null)
            {
                return _whiteSprite;
            }

            var sprite = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height),
                new Vector2(0.5f, 0.5f), 100f);
            _sprites[resourcePath] = sprite;
            return sprite;
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

        private readonly struct IntroPage
        {
            public IntroPage(string titleKey, string bodyKey)
            {
                TitleKey = titleKey;
                BodyKey = bodyKey;
            }

            public string TitleKey { get; }
            public string BodyKey { get; }
        }

        private static string T(string key) => AppServices.Localization?.Translate(key) ?? $"[{key}]";
    }
}
