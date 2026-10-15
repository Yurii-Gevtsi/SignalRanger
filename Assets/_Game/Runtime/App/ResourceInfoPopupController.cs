using System;
using System.Collections.Generic;
using Game.Core.Localization;
using Game.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.App
{
    public enum ResourceInfoKind
    {
        Chips,
        Coins,
        Crystals,
        TaskReward
    }

    /// <summary>
    /// Shared resource and requirement explanation popup for non-battle menus.
    /// The popup is owned by the screen canvas so it is destroyed with that screen.
    /// </summary>
    public sealed class ResourceInfoPopupController : MonoBehaviour
    {
        private static readonly Vector2 CardSize = new(820f, 620f);
        private static readonly Vector2 IconSize = new(150f, 150f);

        private readonly Dictionary<string, Sprite> _sprites = new(StringComparer.Ordinal);
        private Sprite _whiteSprite;
        private GameObject _overlay;
        private RectTransform _card;
        private Image _icon;
        private TMP_Text _title;
        private TMP_Text _description;
        private RectTransform _titleRect;
        private RectTransform _descriptionRect;
        private RectTransform _closeRect;

        public static ResourceInfoPopupController Create(Transform owner)
        {
            var root = new GameObject("Resource Info Popup", typeof(RectTransform));
            root.transform.SetParent(owner, false);
            var rootRect = (RectTransform)root.transform;
            rootRect.anchorMin = Vector2.zero;
            rootRect.anchorMax = Vector2.one;
            rootRect.pivot = new Vector2(0.5f, 0.5f);
            rootRect.sizeDelta = Vector2.zero;
            rootRect.anchoredPosition = Vector2.zero;
            var popup = root.AddComponent<ResourceInfoPopupController>();
            var canvas = root.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.overrideSorting = true;
            canvas.sortingOrder = 240;
            var scaler = root.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920f, 1080f);
            scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            scaler.matchWidthOrHeight = 0.5f;
            root.AddComponent<GraphicRaycaster>();
            return popup;
        }

        private void Awake()
        {
            _whiteSprite = Sprite.Create(Texture2D.whiteTexture, new Rect(0f, 0f, 1f, 1f),
                new Vector2(0.5f, 0.5f), 1f);
            Build();
        }

        public void BindResourceIcon(Image target, ResourceInfoKind kind)
        {
            Bind(target, () => Show(kind, null));
        }

        public void BindRewardIcon(Image target, ResourceInfoKind kind)
        {
            Bind(target, () => Show(kind, target != null ? target.sprite : null));
        }

        public void BindMessage(Image target, Func<string> title, Func<string> description)
        {
            Bind(target, () => ShowMessage(title?.Invoke(), description?.Invoke()));
        }

        public void Show(ResourceInfoKind kind, Sprite iconOverride)
        {
            var (title, description, iconResource) = kind switch
            {
                ResourceInfoKind.Chips => (GameText.ResourceChipsTitle(), GameText.ResourceChipsDescription(), "UI/Common/chip_2"),
                ResourceInfoKind.Coins => (GameText.ResourceCoinsTitle(), GameText.ResourceCoinsDescription(), "UI/Shop/coin"),
                ResourceInfoKind.Crystals => (GameText.ResourceCrystalsTitle(), GameText.ResourceCrystalsDescription(), "UI/Shop/vip_coin"),
                _ => (GameText.ResourceTaskRewardTitle(), GameText.ResourceTaskRewardDescription(), "UI/Common/chip_2")
            };

            ShowMessage(title, description, iconOverride != null ? iconOverride : LoadSprite(iconResource));
        }

        public void ShowMessage(string title, string description, ResourceInfoKind? kind = null)
        {
            ShowMessage(title, description, kind.HasValue ? LoadSprite(ResourceIconPath(kind.Value)) : null);
        }

        private void ShowMessage(string title, string description, Sprite icon)
        {
            _title.text = title ?? string.Empty;
            _description.text = description ?? string.Empty;
            _icon.sprite = icon ?? _whiteSprite;
            _icon.enabled = icon != null;
            _card.sizeDelta = new Vector2(CardSize.x, icon != null ? CardSize.y : CardSize.y * 0.5f);
            ApplyTextOnlyLayout(icon == null, !string.IsNullOrWhiteSpace(_title.text));
            _overlay.transform.SetAsLastSibling();
            _overlay.SetActive(true);
        }

        private void Bind(Image target, Action action)
        {
            if (target == null || action == null)
            {
                return;
            }

            target.raycastTarget = true;
            var button = target.GetComponent<Button>() ?? target.gameObject.AddComponent<Button>();
            button.targetGraphic = target;
            button.transition = Selectable.Transition.None;
            button.interactable = true;
            button.onClick.AddListener(() => action());
        }

        private void Build()
        {
            _overlay = CreateImage("Resource Info Overlay", transform, Vector2.zero, Vector2.one,
                Vector2.zero, Vector2.zero, string.Empty).gameObject;
            var overlayImage = _overlay.GetComponent<Image>();
            overlayImage.color = new Color(0f, 0f, 0f, 0.58f);
            overlayImage.raycastTarget = true;
            var overlayButton = _overlay.AddComponent<Button>();
            overlayButton.targetGraphic = overlayImage;
            overlayButton.transition = Selectable.Transition.None;
            overlayButton.onClick.AddListener(Hide);

            var card = CreateImage("Resource Info Card", _overlay.transform,
                new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), CardSize,
                Vector2.zero, "UI/Battle/card");
            _card = card.rectTransform;
            var cardImage = card.GetComponent<Image>();
            cardImage.type = Image.Type.Sliced;
            cardImage.preserveAspect = false;
            cardImage.raycastTarget = true;
            var cardButton = card.gameObject.AddComponent<Button>();
            cardButton.targetGraphic = cardImage;
            cardButton.transition = Selectable.Transition.None;
            cardButton.onClick.AddListener(() => { });

            _title = CreateText("Resource Info Title", card, new Vector2(0.12f, 0.78f),
                new Vector2(0.88f, 0.90f), 42, TextAlignmentOptions.Center,
                GameTextRole.DisplayTitle);
            _titleRect = _title.rectTransform;
            _icon = CreateImage("Resource Info Icon", card, new Vector2(0.5f, 0.5f),
                new Vector2(0.5f, 0.5f), IconSize, new Vector2(0f, 45f), string.Empty).GetComponent<Image>();
            _icon.preserveAspect = true;
            _description = CreateText("Resource Info Description", card, new Vector2(0.18f, 0.18f),
                new Vector2(0.82f, 0.45f), 28, TextAlignmentOptions.Top,
                GameTextRole.Body);
            _description.textWrappingMode = TextWrappingModes.Normal;
            _description.overflowMode = TextOverflowModes.Truncate;
            _descriptionRect = _description.rectTransform;

            var close = CreateRect("Resource Info Close", card, new Vector2(1f, 1f),
                new Vector2(1f, 1f), new Vector2(0.5f, 0.5f), new Vector2(120f, 120f), new Vector2(-170f, -130f));
            _closeRect = close;
            var closeImage = close.gameObject.AddComponent<Image>();
            closeImage.sprite = _whiteSprite;
            closeImage.color = new Color(1f, 1f, 1f, 0f);
            closeImage.raycastTarget = true;
            var closeButton = close.gameObject.AddComponent<Button>();
            closeButton.targetGraphic = closeImage;
            closeButton.transition = Selectable.Transition.None;
            closeButton.onClick.AddListener(Hide);
            var closeText = CreateText("Resource Info Close Label", close, Vector2.zero, Vector2.one,
                76, TextAlignmentOptions.Center, GameTextRole.DisplayTitle);
            closeText.text = GameText.CloseGlyph();
            closeText.color = new Color(1f, 0.18f, 0.16f, 1f);

            _overlay.SetActive(false);
        }

        private void ApplyTextOnlyLayout(bool isTextOnly, bool hasTitle)
        {
            if (_titleRect == null || _descriptionRect == null || _closeRect == null)
            {
                return;
            }

            if (isTextOnly)
            {
                _titleRect.anchorMin = new Vector2(0.12f, 1f);
                _titleRect.anchorMax = new Vector2(0.88f, 1f);
                _titleRect.pivot = new Vector2(0.5f, 1f);
                _titleRect.sizeDelta = new Vector2(0f, 56f);
                _titleRect.anchoredPosition = new Vector2(0f, -36f);

                // The description fills the card below the title and sits in its
                // middle, so a short message is centred instead of hugging the top.
                _descriptionRect.anchorMin = new Vector2(0.14f, 0f);
                _descriptionRect.anchorMax = new Vector2(0.86f, 1f);
                _descriptionRect.pivot = new Vector2(0.5f, 0.5f);
                _descriptionRect.offsetMin = new Vector2(0f, 40f);
                _descriptionRect.offsetMax = new Vector2(0f, hasTitle ? -96f : -40f);
                _description.alignment = TextAlignmentOptions.Center;
                _closeRect.anchoredPosition = new Vector2(-170f, -70f);
                return;
            }

            _description.alignment = TextAlignmentOptions.Top;

            _titleRect.anchorMin = new Vector2(0.12f, 0.78f);
            _titleRect.anchorMax = new Vector2(0.88f, 0.90f);
            _titleRect.pivot = new Vector2(0.5f, 0.5f);
            _titleRect.sizeDelta = Vector2.zero;
            _titleRect.anchoredPosition = new Vector2(0f, -40f);
            _descriptionRect.anchorMin = new Vector2(0.18f, 0.18f);
            _descriptionRect.anchorMax = new Vector2(0.82f, 0.45f);
            _descriptionRect.pivot = new Vector2(0.5f, 0.5f);
            _descriptionRect.sizeDelta = Vector2.zero;
            _descriptionRect.anchoredPosition = Vector2.zero;
            _closeRect.anchoredPosition = new Vector2(-170f, -130f);
        }

        private void Hide()
        {
            if (_overlay != null)
            {
                _overlay.SetActive(false);
            }
        }

        private static string ResourceIconPath(ResourceInfoKind kind)
        {
            return kind switch
            {
                ResourceInfoKind.Chips => "UI/Common/chip_2",
                ResourceInfoKind.Coins => "UI/Shop/coin",
                ResourceInfoKind.Crystals => "UI/Shop/vip_coin",
                _ => "UI/Common/chip_2"
            };
        }

        private Image CreateImage(string name, Component parent, Vector2 anchorMin, Vector2 anchorMax,
            Vector2 size, Vector2 position, string resourceName)
        {
            var rect = CreateRect(name, parent, anchorMin, anchorMax, new Vector2(0.5f, 0.5f), size, position);
            var image = rect.gameObject.AddComponent<Image>();
            image.sprite = string.IsNullOrWhiteSpace(resourceName) ? _whiteSprite : LoadSprite(resourceName);
            image.color = Color.white;
            image.raycastTarget = false;
            return image;
        }

        private TMP_Text CreateText(string name, Component parent, Vector2 anchorMin, Vector2 anchorMax,
            int fontSize, TextAlignmentOptions alignment, GameTextRole role)
        {
            var rect = CreateRect(name, parent, anchorMin, anchorMax, new Vector2(0.5f, 0.5f),
                Vector2.zero, Vector2.zero);
            var text = rect.gameObject.AddComponent<TextMeshProUGUI>();
            SciFiTextStyle.Apply(text, role, fontSize, alignment);
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

        private static RectTransform CreateRect(string name, Component parent, Vector2 anchorMin, Vector2 anchorMax,
            Vector2 pivot, Vector2 size, Vector2 position)
        {
            var objectRoot = new GameObject(name, typeof(RectTransform));
            var rect = (RectTransform)objectRoot.transform;
            rect.SetParent(parent.transform, false);
            rect.anchorMin = anchorMin;
            rect.anchorMax = anchorMax;
            rect.pivot = pivot;
            rect.sizeDelta = size;
            rect.anchoredPosition = position;
            return rect;
        }
    }
}
