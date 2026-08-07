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
    /// <summary>
    /// Visual campaign galaxy map. The first cluster is the currently available
    /// entry point; later clusters will be unlocked by campaign progression.
    /// </summary>
    public sealed class CampaignScreenController : MonoBehaviour
    {
        private const string MenuResourceRoot = "UI/Menu/";
        private const string ShopResourceRoot = "UI/Shop/";

        private static readonly Color TextShadowColor = new(0.02f, 0.03f, 0.05f, 0.9f);
        private static readonly Vector2 BackButtonSize = new(118f, 118f);
        private static readonly Vector2 BackButtonPosition = new(30f, 0f);
        private static readonly Vector2 TitlePanelSize = new(560f, 110f);
        private static readonly Vector2 TitlePanelPosition = new(128f, 0f);
        private static readonly Vector2 SettingsButtonSize = new(104f, 104f);
        private static readonly Vector2 SettingsButtonPosition = new(-26f, -10f);
        private static readonly Vector2 LockSize = new(118f, 118f);
        private static readonly Vector2 StarsSize = new(320f, 214f);
        private static readonly Vector2 FirstClusterPosition = new(-750f, -250f);

        private readonly Dictionary<string, Sprite> _sprites = new(StringComparer.Ordinal);
        private Sprite _whiteSprite;
        private readonly struct ClusterLayout
        {
            public ClusterLayout(int number, Vector2 position)
            {
                Number = number;
                Position = position;
            }

            public int Number { get; }
            public Vector2 Position { get; }
        }

        public static void Open()
        {
            if (FindAnyObjectByType<CampaignScreenController>() != null)
            {
                return;
            }

            var root = new GameObject("Campaign Screen");
            root.AddComponent<CampaignScreenController>();
        }

        private void Awake()
        {
            _whiteSprite = Sprite.Create(Texture2D.whiteTexture, new Rect(0f, 0f, 1f, 1f),
                new Vector2(0.5f, 0.5f), 1f);
            Build();
        }

        private void Build()
        {
            var canvasObject = new GameObject("Campaign Canvas");
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
            CreateImage("Campaign Background", root, Vector2.zero, Vector2.one, new Vector2(0.5f, 0.5f),
                Vector2.zero, Vector2.zero, MenuResourceRoot + "campaign_menu", false, Color.white);

            CreateImage("Campaign Open Cluster Stars", root, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                new Vector2(0.5f, 0.5f), StarsSize, FirstClusterPosition, MenuResourceRoot + "stars1", true, Color.white);

            BuildClusters(root);
            var backButton = CreateImageButton("Campaign Back", root, new Vector2(0f, 1f), new Vector2(0f, 1f), new Vector2(0f, 1f),
                BackButtonSize, BackButtonPosition, ShopResourceRoot + "back_btn", Close);
            EnlargeForPhone(backButton);

            var titlePanel = CreateImage("Campaign Title Panel", root, new Vector2(0f, 1f), new Vector2(0f, 1f),
                new Vector2(0f, 1f), TitlePanelSize, TitlePanelPosition, ShopResourceRoot + "info_panel", true, Color.white);
            EnlargeForPhone(titlePanel);
            CreateText("Campaign Title", titlePanel, Vector2.zero, Vector2.one, new Vector2(0.5f, 0.5f),
                new Vector2(-60f, -12f), new Vector2(-70f, 0f), 38, TextAlignmentOptions.Center, GameText.CampaignMapTitle());

            var settingsButton = CreateImageButton("Campaign Settings", root, new Vector2(1f, 1f), new Vector2(1f, 1f), new Vector2(1f, 1f),
                SettingsButtonSize, SettingsButtonPosition, ShopResourceRoot + "settings_btn", () => SettingsScreenController.Open(null));
            EnlargeForPhone(settingsButton);
        }

        private void BuildClusters(Transform root)
        {
            var clusters = new[]
            {
                new ClusterLayout(1, FirstClusterPosition),
                new ClusterLayout(2, new Vector2(-550f, 0f)),
                new ClusterLayout(3, new Vector2(-500f, 250f)),
                new ClusterLayout(4, new Vector2(-40f, 250f)),
                new ClusterLayout(5, new Vector2(400f, 250f)),
                new ClusterLayout(6, new Vector2(545f, 100f)),
                new ClusterLayout(7, new Vector2(600f, -150f)),
                new ClusterLayout(8, new Vector2(240f, -140f)),
                new ClusterLayout(9, new Vector2(-225f, -150f)),
                new ClusterLayout(10, new Vector2(0f, 0f))
            };

            for (var i = 0; i < clusters.Length; i++)
            {
                var cluster = clusters[i];
                var node = CreateClusterButton(root, cluster);
                if (cluster.Number > 1)
                {
                    CreateImage($"Campaign Lock {cluster.Number}", node, new Vector2(0.5f, 0.5f),
                        new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), LockSize,
                        Vector2.zero, ShopResourceRoot + "lock", true, Color.white);
                }

                var label = CreateText($"Campaign Cluster Number {cluster.Number}", node,
                    new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(0.5f, 0.5f),
                    new Vector2(90f, 60f), new Vector2(0f, 28f), 44,
                    TextAlignmentOptions.Center, cluster.Number.ToString());
                label.color = cluster.Number == 1 ? SciFiTextStyle.GoldText : SciFiTextStyle.CyanText;
            }
        }

        private RectTransform CreateClusterButton(Transform root, ClusterLayout cluster)
        {
            var rect = CreateRect($"Campaign Cluster {cluster.Number}", root, new Vector2(0.5f, 0.5f),
                new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(200f, 180f), cluster.Position);
            var image = rect.gameObject.AddComponent<Image>();
            image.sprite = _whiteSprite;
            image.color = new Color(1f, 1f, 1f, 0f);
            image.raycastTarget = true;
            var button = rect.gameObject.AddComponent<Button>();
            button.targetGraphic = image;
            button.transition = Selectable.Transition.None;
            button.interactable = false;

            return rect;
        }

        private void Close()
        {
            Destroy(gameObject);
        }

        private RectTransform CreateImage(string name, Component parent, Vector2 anchorMin, Vector2 anchorMax,
            Vector2 pivot, Vector2 sizeDelta, Vector2 anchoredPosition, string resourceName, bool preserveAspect,
            Color tint)
        {
            var rect = CreateRect(name, parent, anchorMin, anchorMax, pivot, sizeDelta, anchoredPosition);
            var image = rect.gameObject.AddComponent<Image>();
            image.sprite = LoadSprite(resourceName);
            image.color = tint;
            image.preserveAspect = preserveAspect;
            image.raycastTarget = false;
            return rect;
        }

        private RectTransform CreateImageButton(string name, Component parent, Vector2 anchorMin, Vector2 anchorMax,
            Vector2 pivot, Vector2 sizeDelta, Vector2 anchoredPosition, string resourceName, Action onClick)
        {
            var rect = CreateImage(name, parent, anchorMin, anchorMax, pivot, sizeDelta, anchoredPosition,
                resourceName, true, Color.white);
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
            TextAlignmentOptions alignment, string value)
        {
            var rect = CreateRect(name, parent, anchorMin, anchorMax, pivot, sizeDelta, anchoredPosition);
            var text = rect.gameObject.AddComponent<TextMeshProUGUI>();
            SciFiTextStyle.Apply(text, GameTextRole.DisplayTitle, fontSize, alignment);
            text.enableAutoSizing = false;
            text.textWrappingMode = TextWrappingModes.NoWrap;
            text.overflowMode = TextOverflowModes.Truncate;
            text.text = value;
            var shadow = text.gameObject.AddComponent<Shadow>();
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

        private static void EnlargeForPhone(RectTransform element)
        {
            element.localScale = Vector3.one * 1.2f;
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
