using System;
using System.Collections.Generic;
using Game.Core.Localization;
using Game.Gameplay.Battle.Weapons;
using Game.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.App
{
    public sealed class WeaponsScreenController : MonoBehaviour
    {
        private const string MenuResourceRoot = "UI/Menu/";
        private const string ShopResourceRoot = "UI/Shop/";
        private const string WeaponResourceRoot = "UI/Weapons/";
        private const string BattleResourceRoot = "UI/Battle/";
        private const int Columns = 7;
        private const int Rows = 6;
        private const float PhoneElementScale = 1.2f;

        private static readonly Color TextShadowColor = new(0.02f, 0.03f, 0.05f, 0.9f);
        private static readonly Vector2 BackButtonSize = new(118f, 118f);
        private static readonly Vector2 BackButtonPosition = new(30f, 0f);
        private static readonly Vector2 SettingsButtonSize = new(140f, 140f);
        private static readonly Vector2 SettingsButtonPosition = new(-26f, -31f);
        private static readonly Vector2 BureauButtonSize = new(220f, 220f);
        private static readonly Vector2 BureauButtonPosition = new(-80f, 30f);
        private static readonly Vector2 BureauButtonScale = new(1.2f, 1.5f);
        private static readonly Vector2 WeaponIconSize = new(195f, 195f);
        private static readonly Vector2 DetailsCardSize = new(1400f, 940f);
        private static readonly Vector2 DetailsHitAreaSize = new(1240f, 760f);
        private static readonly Vector2 DetailsLeftPosition = new(-550f, 0f);
        private static readonly Vector2 DetailsRightPosition = new(400f, 0f);

        private static readonly float[] SlotColumns =
        {
            -730f, -500f, -250f, -20f, 220f, 455f, 720f
        };

        private static readonly float[] SlotRows =
        {
            330f, 190f, 50f, -90f, -230f, -370f
        };

        private readonly Dictionary<string, Sprite> _sprites = new();
        private readonly List<OwnedWeaponItem> _items = new();

        private Sprite _whiteSprite;
        private Action _onClosed;
        private GameObject _detailsOverlay;
        private RectTransform _detailsCard;
        private TMP_Text _detailsNameText;
        private TMP_Text _detailsDescriptionText;
        private TMP_Text _detailsDamageText;
        private TMP_Text _detailsUsesText;

        private readonly struct OwnedWeaponItem
        {
            public OwnedWeaponItem(WeaponDefinition definition)
            {
                Definition = definition;
            }

            public WeaponDefinition Definition { get; }
        }

        public static void Open(Action onClosed)
        {
            if (FindAnyObjectByType<WeaponsScreenController>() != null)
            {
                return;
            }

            var root = new GameObject("Weapons Screen");
            root.AddComponent<WeaponsScreenController>()._onClosed = onClosed;
        }

        private void Awake()
        {
            _whiteSprite = Sprite.Create(Texture2D.whiteTexture, new Rect(0f, 0f, 1f, 1f), new Vector2(0.5f, 0.5f), 1f);
            BuildItems();
            Build();
        }

        private void BuildItems()
        {
            var weapons = PrototypeWeaponLibrary.CreateAll();
            for (var i = 0; i < weapons.Length; i++)
            {
                var weapon = weapons[i];
                if (weapon == null || WeaponPresentationText.IsDropOnly(weapon))
                {
                    continue;
                }

                _items.Add(new OwnedWeaponItem(weapon));
            }
        }

        private void Build()
        {
            var canvasObject = new GameObject("Weapons Canvas");
            canvasObject.transform.SetParent(transform, false);
            var canvas = canvasObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 125;

            var scaler = canvasObject.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920f, 1080f);
            scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            scaler.matchWidthOrHeight = 1f;
            canvasObject.AddComponent<GraphicRaycaster>();

            var root = canvasObject.transform;
            CreateImage("Weapons Background", root, Vector2.zero, Vector2.one, new Vector2(0.5f, 0.5f),
                Vector2.zero, Vector2.zero, "weapons_menu", false, null, false);

            BuildShelfWeapons(root);
            BuildConstructorBureauButton(root);
            BuildDetails(root);

            var backButton = CreateImageButton("Back Button", root, new Vector2(0f, 1f), new Vector2(0f, 1f),
                new Vector2(0f, 1f), BackButtonSize, BackButtonPosition, "back_btn", Close);
            backButton.localScale = new Vector3(PhoneElementScale, PhoneElementScale, 1f);

            var settingsButton = CreateImageButton("Settings Button", root, new Vector2(1f, 1f), new Vector2(1f, 1f),
                new Vector2(1f, 1f), SettingsButtonSize, SettingsButtonPosition, "settings_btn", () => SettingsScreenController.Open(null));
            settingsButton.localScale = new Vector3(PhoneElementScale, PhoneElementScale, 1f);
        }

        private void BuildShelfWeapons(Transform root)
        {
            for (var i = 0; i < _items.Count && i < Columns * Rows; i++)
            {
                var row = i / Columns;
                var column = i % Columns;
                var position = new Vector2(SlotColumns[column], SlotRows[row]);
                var item = _items[i];
                var icon = CreateImageButton($"Shelf Weapon {i}", root, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                    new Vector2(0.5f, 0.5f), WeaponIconSize, position, item.Definition.IconId, () => ShowDetails(item, column <= 4), true);
                ApplyWeaponVisual(icon, item.Definition.WeaponId);
            }
        }

        private void BuildConstructorBureauButton(Transform root)
        {
            var button = CreateImageButton("Constructor Bureau Button", root, new Vector2(1f, 0f), new Vector2(1f, 0f),
                new Vector2(1f, 0f), BureauButtonSize, BureauButtonPosition, "UI/Common/architect_btn", () => { });
            button.localScale = new Vector3(BureauButtonScale.x, BureauButtonScale.y, 1f);

            var label = CreateText("Constructor Bureau Label", button.transform, new Vector2(0.5f, 0f), new Vector2(0.5f, 0f),
                new Vector2(0.5f, 1f), new Vector2(430f, 72f), new Vector2(-112f, -10f),
                30, TextAlignmentOptions.Center, GameText.ConstructorBureau(), GameTextRole.Button);
            label.enableAutoSizing = true;
            label.fontSizeMin = 20f;
            label.fontSizeMax = 30f;
        }

        private void BuildDetails(Transform root)
        {
            _detailsOverlay = CreatePanel("Weapon Details Overlay", root, Vector2.zero, Vector2.one,
                new Vector2(0.5f, 0.5f), Vector2.zero, Vector2.zero, new Color(0f, 0f, 0f, 0.35f)).gameObject;
            var overlayButton = _detailsOverlay.AddComponent<Button>();
            overlayButton.targetGraphic = _detailsOverlay.GetComponent<Image>();
            overlayButton.transition = Selectable.Transition.None;
            overlayButton.onClick.AddListener(HideDetails);

            _detailsCard = CreateImage("Weapon Details", _detailsOverlay.transform, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                new Vector2(0.5f, 0.5f), DetailsCardSize, Vector2.zero, BattleResourceRoot + "card", false, null, false,
                new Vector4(250f, 250f, 250f, 250f));
            var cardImage = _detailsCard.GetComponent<Image>();
            cardImage.type = Image.Type.Sliced;
            cardImage.preserveAspect = false;
            cardImage.raycastTarget = false;

            CreateDetailsHitArea(_detailsCard.transform);
            CreateDetailsCloseButton(_detailsCard.transform);

            _detailsNameText = CreateText("Weapon Details Name", _detailsCard.transform, new Vector2(0.20f, 1f), new Vector2(0.82f, 1f),
                new Vector2(0.5f, 0.5f), new Vector2(0f, 86f), new Vector2(0f, -250f), 50, TextAlignmentOptions.Center, string.Empty, GameTextRole.DisplayTitle);
            _detailsNameText.enableAutoSizing = true;
            _detailsNameText.fontSizeMin = 32f;
            _detailsNameText.fontSizeMax = 50f;

            _detailsDescriptionText = CreateText("Weapon Details Description", _detailsCard.transform, new Vector2(0.20f, 0.43f), new Vector2(0.80f, 0.68f),
                new Vector2(0.5f, 1f), Vector2.zero, new Vector2(0f, -20f), 34, TextAlignmentOptions.TopLeft, string.Empty, GameTextRole.Body);
            _detailsDescriptionText.enableAutoSizing = true;
            _detailsDescriptionText.fontSizeMin = 24f;
            _detailsDescriptionText.fontSizeMax = 34f;
            _detailsDescriptionText.overflowMode = TextOverflowModes.Overflow;
            _detailsDescriptionText.textWrappingMode = TextWrappingModes.Normal;

            _detailsDamageText = CreateStatLine(_detailsCard.transform, "Weapon Details Damage", new Vector2(0.16f, 0.27f), new Vector2(0.45f, 0.36f), GameText.Damage());
            _detailsUsesText = CreateStatLine(_detailsCard.transform, "Weapon Details Uses", new Vector2(0.49f, 0.27f), new Vector2(0.78f, 0.36f), GameText.UsesPerBattle());
            _detailsOverlay.SetActive(false);
        }

        private void CreateDetailsHitArea(Transform parent)
        {
            var rect = CreateRect("Weapon Details Hit Area", parent, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                new Vector2(0.5f, 0.5f), DetailsHitAreaSize, Vector2.zero);
            var image = rect.gameObject.AddComponent<Image>();
            image.sprite = _whiteSprite;
            image.color = new Color(1f, 1f, 1f, 0f);
            image.raycastTarget = true;
        }

        private void CreateDetailsCloseButton(Transform parent)
        {
            var rect = CreateRect("Close Weapon Details", parent, new Vector2(1f, 1f), new Vector2(1f, 1f),
                new Vector2(0.5f, 0.5f), new Vector2(132f, 132f), new Vector2(-260f, -250f));
            var hitArea = rect.gameObject.AddComponent<Image>();
            hitArea.sprite = _whiteSprite;
            hitArea.color = new Color(1f, 1f, 1f, 0f);
            hitArea.raycastTarget = true;
            var button = rect.gameObject.AddComponent<Button>();
            button.targetGraphic = hitArea;
            button.transition = Selectable.Transition.None;
            button.onClick.AddListener(HideDetails);

            var label = CreateText("Close Weapon Details Label", rect, Vector2.zero, Vector2.one,
                new Vector2(0.5f, 0.5f), Vector2.zero, Vector2.zero, 72, TextAlignmentOptions.Center, "×", GameTextRole.DisplayTitle);
            label.rectTransform.localScale = new Vector3(1.5f, 1.5f, 1f);
            label.color = new Color(1f, 0.18f, 0.16f, 1f);
        }

        private TMP_Text CreateStatLine(Transform parent, string name, Vector2 anchorMin, Vector2 anchorMax, string label)
        {
            var line = CreatePanel(name, parent, anchorMin, anchorMax,
                new Vector2(0.5f, 0.5f), Vector2.zero, Vector2.zero, new Color(0f, 0f, 0f, 0.18f));
            line.raycastTarget = false;
            CreateText($"{name} Label", line.transform, new Vector2(0.06f, 0f), new Vector2(0.48f, 1f),
                new Vector2(0.5f, 0.5f), Vector2.zero, Vector2.zero, 28, TextAlignmentOptions.MidlineLeft, label, GameTextRole.Body);
            var value = CreateText($"{name} Value", line.transform, new Vector2(0.48f, 0f), new Vector2(0.90f, 1f),
                new Vector2(0.5f, 0.5f), Vector2.zero, Vector2.zero, 31, TextAlignmentOptions.MidlineLeft, string.Empty, GameTextRole.Numeric);
            value.enableAutoSizing = false;
            return value;
        }

        private void ShowDetails(OwnedWeaponItem item, bool showOnRight)
        {
            var weapon = item.Definition;
            _detailsOverlay.transform.SetAsLastSibling();
            _detailsCard.anchoredPosition = showOnRight ? DetailsRightPosition : DetailsLeftPosition;
            _detailsNameText.text = WeaponDisplayNames.Get(weapon.WeaponId);
            _detailsDescriptionText.text = WeaponPresentationText.Describe(weapon);
            _detailsDamageText.text = weapon.Damage.ToString();
            _detailsUsesText.text = weapon.HasLimitedAmmo ? weapon.AmmoCount.ToString() : "\u221E";
            _detailsOverlay.SetActive(true);
        }

        private void HideDetails()
        {
            _detailsOverlay.SetActive(false);
        }

        private static void ApplyWeaponVisual(RectTransform rect, string weaponId)
        {
            var tuning = GetWeaponVisualTuning(weaponId);
            rect.localScale = new Vector3(tuning.Scale.x, tuning.Scale.y, 1f);
            rect.anchoredPosition += tuning.Offset;
        }

        private readonly struct WeaponVisualTuning
        {
            public WeaponVisualTuning(Vector2 scale, Vector2 offset)
            {
                Scale = scale;
                Offset = offset;
            }

            public Vector2 Scale { get; }
            public Vector2 Offset { get; }
        }

        private static WeaponVisualTuning GetWeaponVisualTuning(string weaponId)
        {
            var defaultOffset = new Vector2(0f, 8f);

            return weaponId switch
            {
                "weapon.impulse" => new WeaponVisualTuning(new Vector2(1.26f, 1.26f), defaultOffset),
                "weapon.blaster-caset" => new WeaponVisualTuning(new Vector2(1.06f, 1.06f), defaultOffset),
                "weapon.bur" => new WeaponVisualTuning(new Vector2(0.82f, 0.82f), defaultOffset),
                "weapon.mortira" => new WeaponVisualTuning(Vector2.one, defaultOffset),
                "weapon.granata" => new WeaponVisualTuning(new Vector2(0.70f, 0.70f), new Vector2(0f, 12f)),
                "weapon.teleport" => new WeaponVisualTuning(new Vector2(0.53f, 0.53f), new Vector2(0f, 14f)),
                "weapon.c4" => new WeaponVisualTuning(new Vector2(0.96f, 0.96f), new Vector2(0f, 4f)),
                "weapon.mine" => new WeaponVisualTuning(new Vector2(0.79f, 0.79f), new Vector2(0f, 4f)),
                "weapon.bur-rocket" => new WeaponVisualTuning(new Vector2(1.03f, 1.03f), defaultOffset),
                "weapon.heal" => new WeaponVisualTuning(new Vector2(0.94f, 0.94f), new Vector2(0f, 4f)),
                "weapon.emp" => new WeaponVisualTuning(new Vector2(0.94f, 0.82f), defaultOffset),
                "weapon.freeze" => new WeaponVisualTuning(Vector2.one, defaultOffset),
                "weapon.fire-gun" => new WeaponVisualTuning(new Vector2(0.98f, 0.98f), defaultOffset),
                "weapon.fpv-drone" => new WeaponVisualTuning(new Vector2(0.82f, 0.82f), new Vector2(0f, 6f)),
                _ => new WeaponVisualTuning(new Vector2(1.20f, 1.20f), defaultOffset)
            };
        }

        private RectTransform CreateImage(string name, Component parent, Vector2 anchorMin, Vector2 anchorMax,
            Vector2 pivot, Vector2 sizeDelta, Vector2 anchoredPosition, string resourceName, bool preserveAspect,
            Color? tint, bool weaponResource, Vector4? border = null)
        {
            var rect = CreateRect(name, parent, anchorMin, anchorMax, pivot, sizeDelta, anchoredPosition);
            var image = rect.gameObject.AddComponent<Image>();
            image.sprite = LoadSprite(resourceName, weaponResource, border ?? Vector4.zero);
            image.color = tint ?? Color.white;
            image.preserveAspect = preserveAspect;
            image.raycastTarget = false;
            return rect;
        }

        private RectTransform CreateImageButton(string name, Component parent, Vector2 anchorMin, Vector2 anchorMax,
            Vector2 pivot, Vector2 sizeDelta, Vector2 anchoredPosition, string resourceName, Action onClick, bool weaponResource = false)
        {
            var rect = CreateImage(name, parent, anchorMin, anchorMax, pivot, sizeDelta, anchoredPosition, resourceName, true, null, weaponResource);
            var image = rect.GetComponent<Image>();
            image.raycastTarget = true;

            var button = rect.gameObject.AddComponent<Button>();
            button.targetGraphic = image;
            button.transition = Selectable.Transition.ColorTint;
            button.colors = CreateButtonColors();
            button.onClick.AddListener(() => onClick?.Invoke());
            return rect;
        }

        private Image CreatePanel(string name, Component parent, Vector2 anchorMin, Vector2 anchorMax, Vector2 pivot,
            Vector2 sizeDelta, Vector2 anchoredPosition, Color color)
        {
            var rect = CreateRect(name, parent, anchorMin, anchorMax, pivot, sizeDelta, anchoredPosition);
            var image = rect.gameObject.AddComponent<Image>();
            image.sprite = _whiteSprite;
            image.color = color;
            image.raycastTarget = true;
            return image;
        }

        private TMP_Text CreateText(string name, Component parent, Vector2 anchorMin, Vector2 anchorMax, Vector2 pivot,
            Vector2 sizeDelta, Vector2 anchoredPosition, int fontSize, TextAlignmentOptions alignment, string value,
            GameTextRole role)
        {
            var rect = CreateRect(name, parent, anchorMin, anchorMax, pivot, sizeDelta, anchoredPosition);
            var text = rect.gameObject.AddComponent<TextMeshProUGUI>();
            SciFiTextStyle.Apply(text, role, fontSize, alignment);
            text.fontSizeMin = Mathf.Max(14, fontSize - 10);
            text.textWrappingMode = TextWrappingModes.Normal;
            text.overflowMode = TextOverflowModes.Truncate;
            text.text = value;
            text.raycastTarget = false;

            var shadow = rect.gameObject.AddComponent<Shadow>();
            shadow.effectColor = TextShadowColor;
            shadow.effectDistance = new Vector2(0f, -2f);
            return text;
        }

        private Sprite LoadSprite(string resourceName, bool weaponResource, Vector4 border = default)
        {
            var resourcePath = resourceName.Contains("/")
                ? resourceName
                : (weaponResource ? WeaponResourceRoot : resourceName == "back_btn" ? ShopResourceRoot : MenuResourceRoot) + resourceName;
            var key = $"{resourcePath}:{border.x:0.###},{border.y:0.###},{border.z:0.###},{border.w:0.###}";
            if (_sprites.TryGetValue(key, out var cached))
            {
                return cached;
            }

            var texture = Resources.Load<Texture2D>(resourcePath);
            if (texture == null)
            {
                return _whiteSprite;
            }

            var sprite = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height),
                new Vector2(0.5f, 0.5f), 100f, 0, SpriteMeshType.FullRect, border);
            _sprites[key] = sprite;
            return sprite;
        }

        private void Close()
        {
            var callback = _onClosed;
            _onClosed = null;
            Destroy(gameObject);
            callback?.Invoke();
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
