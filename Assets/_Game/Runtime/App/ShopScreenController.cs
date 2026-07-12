using System;
using System.Collections.Generic;
using Game.Gameplay.Battle.Weapons;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.App
{
    public sealed class ShopScreenController : MonoBehaviour
    {
        private const string ShopResourceRoot = "UI/Shop/";
        private const string WeaponResourceRoot = "UI/Weapons/";

        private static readonly Color TextShadowColor = new(0.02f, 0.03f, 0.05f, 0.9f);
        private static readonly Color SelectedSlotColor = new(1f, 1f, 1f, 1f);
        private static readonly Color NormalSlotColor = new(0.72f, 0.82f, 0.92f, 0.88f);
        private static readonly Color HologramColor = new(0.70f, 1f, 0.96f, 0.95f);

        private readonly Dictionary<string, Sprite> _sprites = new();
        private readonly List<SlotView> _slots = new();
        private readonly List<ShopWeaponItem> _items = new();

        private Sprite _whiteSprite;
        private Action _onClosed;
        private RectTransform _previewRoot;
        private Image _previewImage;
        private TMP_Text _nameText;
        private TMP_Text _levelText;
        private TMP_Text _descriptionText;
        private TMP_Text _damageText;
        private TMP_Text _blastText;
        private TMP_Text _usesText;
        private TMP_Text _coinPriceText;
        private TMP_Text _crystalPriceText;
        private int _selectedIndex;

        private readonly struct ShopWeaponItem
        {
            public ShopWeaponItem(
                WeaponDefinition definition,
                string art,
                string displayName,
                int requiredLevel,
                string description,
                int coinPrice,
                int crystalPrice)
            {
                Definition = definition;
                Art = art;
                DisplayName = displayName;
                RequiredLevel = requiredLevel;
                Description = description;
                CoinPrice = coinPrice;
                CrystalPrice = crystalPrice;
            }

            public WeaponDefinition Definition { get; }
            public string Art { get; }
            public string DisplayName { get; }
            public int RequiredLevel { get; }
            public string Description { get; }
            public int CoinPrice { get; }
            public int CrystalPrice { get; }
        }

        private struct SlotView
        {
            public Image Frame;
            public RectTransform Weapon;
            public TMP_Text Label;
        }

        public static void Open(Action onClosed)
        {
            if (FindAnyObjectByType<ShopScreenController>() != null)
            {
                return;
            }

            var root = new GameObject("Shop Screen");
            root.AddComponent<ShopScreenController>()._onClosed = onClosed;
        }

        private void Awake()
        {
            _whiteSprite = Sprite.Create(Texture2D.whiteTexture, new Rect(0f, 0f, 1f, 1f), new Vector2(0.5f, 0.5f), 1f);
            BuildItems();
            Build();
            SelectWeapon(0);
        }

        private void BuildItems()
        {
            var weapons = PrototypeWeaponLibrary.CreateAll();
            Add(weapons, "weapon.steady-orb", "bazooka_simple", "Базова труба", 1,
                "Надійний стартовий постріл з передбачуваною траєкторією.", 240, 8);
            Add(weapons, "weapon.pulse-shell", "bazooka", "Імпульсний заряд", 2,
                "Дуга з ширшим вибухом для ворогів за укриттям.", 420, 14);
            Add(weapons, "weapon.close-pop", "drobovik", "Ближній розрив", 3,
                "Коротка дистанція, швидкий контакт і стабільний тиск.", 520, 18);
            Add(weapons, "weapon.long-lance", "rifle", "Довгий спис", 5,
                "Точна зброя для дальнього пострілу по одиночній цілі.", 780, 26);
            Add(weapons, "weapon.scatter-arc", "bomber", "Розсипна дуга", 6,
                "Велика зона вибуху, щоб вибивати суперника з позиції.", 960, 32);
            Add(weapons, "weapon.rampart-kit", "bur", "Комплект валу", 7,
                "Тактичний інструмент для створення захисної лінії.", 1080, 36);
            Add(weapons, "weapon.guard-pulse", "glove", "Захисний імпульс", 8,
                "Утилітарний заряд для оборони й контролю темпу.", 1220, 42);
            Add(weapons, "weapon.relay-step", "fpv", "Релейний крок", 9,
                "Маневровий пристрій для швидкої зміни позиції.", 1440, 48);
        }

        private void Add(
            IReadOnlyList<WeaponDefinition> weapons,
            string weaponId,
            string art,
            string displayName,
            int requiredLevel,
            string description,
            int coinPrice,
            int crystalPrice)
        {
            for (var i = 0; i < weapons.Count; i++)
            {
                if (weapons[i] == null || weapons[i].WeaponId != weaponId)
                {
                    continue;
                }

                _items.Add(new ShopWeaponItem(weapons[i], art, displayName, requiredLevel, description, coinPrice, crystalPrice));
                return;
            }
        }

        private void Build()
        {
            var canvasObject = new GameObject("Shop Canvas");
            canvasObject.transform.SetParent(transform, false);
            var canvas = canvasObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 130;

            var scaler = canvasObject.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920f, 1080f);
            scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            scaler.matchWidthOrHeight = 0.5f;
            canvasObject.AddComponent<GraphicRaycaster>();

            var root = canvasObject.transform;
            CreateImage("Shop Background", root, Vector2.zero, Vector2.one, new Vector2(0.5f, 0.5f),
                Vector2.zero, Vector2.zero, "shop_scr", false, null, false);

            BuildTopBar(root);
            BuildPreview(root);
            BuildDetails(root);
            BuildWeaponStrip(root);
        }

        private void BuildTopBar(Transform root)
        {
            var saveData = AppServices.SaveService.Current;
            var softCurrency = Mathf.Max(0, saveData?.economy.softCurrency ?? 0);

            CreateImageButton("Back Button", root, new Vector2(0f, 1f), new Vector2(0f, 1f), new Vector2(0f, 1f),
                new Vector2(118f, 118f), new Vector2(30f, -28f), "back_btn", Close);

            var titlePanel = CreatePanel("Shop Title Panel", root, new Vector2(0f, 1f), new Vector2(0f, 1f), new Vector2(0f, 1f),
                new Vector2(430f, 86f), new Vector2(152f, -42f), new Color(0.03f, 0.07f, 0.10f, 0.72f));
            CreateText("Shop Title", titlePanel.transform, Vector2.zero, Vector2.one, new Vector2(0.5f, 0.5f),
                new Vector2(-44f, -8f), Vector2.zero, 38, TextAlignmentOptions.Center, "Магазин зброї");

            var coins = CreateImage("Coins Panel", root, new Vector2(1f, 1f), new Vector2(1f, 1f), new Vector2(1f, 1f),
                new Vector2(310f, 155f), new Vector2(-486f, -20f), "Coin_panel", true, null, false);
            CreateText("Coins Value", coins.transform, new Vector2(0.34f, 0.22f), new Vector2(0.76f, 0.78f),
                new Vector2(0.5f, 0.5f), Vector2.zero, Vector2.zero, 35, TextAlignmentOptions.Center, softCurrency.ToString());

            var premium = CreateImage("Premium Panel", root, new Vector2(1f, 1f), new Vector2(1f, 1f), new Vector2(1f, 1f),
                new Vector2(310f, 155f), new Vector2(-166f, -20f), "vip_coins_panel", true, null, false);
            CreateText("Premium Value", premium.transform, new Vector2(0.36f, 0.22f), new Vector2(0.76f, 0.78f),
                new Vector2(0.5f, 0.5f), Vector2.zero, Vector2.zero, 35, TextAlignmentOptions.Center, "0");

            CreateImageButton("Settings Button", root, new Vector2(1f, 1f), new Vector2(1f, 1f), new Vector2(1f, 1f),
                new Vector2(124f, 124f), new Vector2(-28f, -25f), "settings_btn", () => SettingsScreenController.Open(null));
        }

        private void BuildPreview(Transform root)
        {
            _previewRoot = CreateRect("Weapon Preview Root", root, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                new Vector2(0.5f, 0.5f), new Vector2(620f, 520f), new Vector2(-105f, -18f));

            var glow = CreatePanel("Weapon Preview Glow", _previewRoot, new Vector2(0.5f, 0f), new Vector2(0.5f, 0f),
                new Vector2(0.5f, 0f), new Vector2(500f, 84f), new Vector2(0f, 4f), new Color(0.28f, 0.95f, 0.95f, 0.18f));
            glow.raycastTarget = false;

            _previewImage = CreateImage("Selected Weapon", _previewRoot, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                new Vector2(0.5f, 0.5f), new Vector2(470f, 470f), new Vector2(0f, 62f), "bazooka", true, HologramColor, false)
                .GetComponent<Image>();
        }

        private void BuildDetails(Transform root)
        {
            var card = CreateImage("Weapon Details", root, new Vector2(1f, 0.5f), new Vector2(1f, 0.5f), new Vector2(1f, 0.5f),
                new Vector2(470f, 588f), new Vector2(-54f, -16f), "weapon_details", true, null, false);

            _nameText = CreateText("Weapon Name", card.transform, new Vector2(0.12f, 0.80f), new Vector2(0.88f, 0.91f),
                new Vector2(0.5f, 0.5f), Vector2.zero, Vector2.zero, 38, TextAlignmentOptions.Center, string.Empty);
            _levelText = CreateText("Weapon Level", card.transform, new Vector2(0.15f, 0.70f), new Vector2(0.85f, 0.78f),
                new Vector2(0.5f, 0.5f), Vector2.zero, Vector2.zero, 28, TextAlignmentOptions.Center, string.Empty);
            _descriptionText = CreateText("Weapon Description", card.transform, new Vector2(0.14f, 0.55f), new Vector2(0.86f, 0.69f),
                new Vector2(0.5f, 0.5f), Vector2.zero, Vector2.zero, 23, TextAlignmentOptions.Center, string.Empty);

            CreateText("Stats Header", card.transform, new Vector2(0.16f, 0.49f), new Vector2(0.84f, 0.54f),
                new Vector2(0.5f, 0.5f), Vector2.zero, Vector2.zero, 25, TextAlignmentOptions.Center, "ТТХ");
            _damageText = CreateStatLine(card.transform, "Damage Stat", 0.41f, "Шкода");
            _blastText = CreateStatLine(card.transform, "Blast Stat", 0.34f, "Сила вибуху");
            _usesText = CreateStatLine(card.transform, "Uses Stat", 0.27f, "За бій");

            var coinButton = CreateImageButton("Buy Coins", card.transform, new Vector2(0.5f, 0f), new Vector2(0.5f, 0f),
                new Vector2(0.5f, 0f), new Vector2(310f, 104f), new Vector2(0f, 104f), "Green_btn", BuySelectedWithCoins);
            _coinPriceText = CreateText("Buy Coins Label", coinButton.transform, new Vector2(0.12f, 0.18f), new Vector2(0.88f, 0.82f),
                new Vector2(0.5f, 0.5f), Vector2.zero, Vector2.zero, 28, TextAlignmentOptions.Center, string.Empty);

            var crystalButton = CreateImageButton("Buy Crystals", card.transform, new Vector2(0.5f, 0f), new Vector2(0.5f, 0f),
                new Vector2(0.5f, 0f), new Vector2(310f, 104f), new Vector2(0f, 22f), "yellow_btn", BuySelectedWithCrystals);
            _crystalPriceText = CreateText("Buy Crystals Label", crystalButton.transform, new Vector2(0.12f, 0.18f), new Vector2(0.88f, 0.82f),
                new Vector2(0.5f, 0.5f), Vector2.zero, Vector2.zero, 28, TextAlignmentOptions.Center, string.Empty);
        }

        private TMP_Text CreateStatLine(Transform parent, string name, float centerY, string label)
        {
            var line = CreatePanel(name, parent, new Vector2(0.16f, centerY - 0.025f), new Vector2(0.84f, centerY + 0.025f),
                new Vector2(0.5f, 0.5f), Vector2.zero, Vector2.zero, new Color(0f, 0f, 0f, 0.18f));
            line.raycastTarget = false;
            CreateText($"{name} Label", line.transform, new Vector2(0.04f, 0f), new Vector2(0.58f, 1f),
                new Vector2(0.5f, 0.5f), Vector2.zero, Vector2.zero, 22, TextAlignmentOptions.MidlineLeft, label);
            return CreateText($"{name} Value", line.transform, new Vector2(0.58f, 0f), new Vector2(0.96f, 1f),
                new Vector2(0.5f, 0.5f), Vector2.zero, Vector2.zero, 22, TextAlignmentOptions.MidlineRight, string.Empty);
        }

        private void BuildWeaponStrip(Transform root)
        {
            var strip = CreateRect("Weapon Strip", root, new Vector2(0.5f, 0f), new Vector2(0.5f, 0f),
                new Vector2(0.5f, 0f), new Vector2(1270f, 205f), new Vector2(-28f, 18f));

            var startX = -552f;
            const float spacing = 158f;
            for (var i = 0; i < _items.Count; i++)
            {
                var index = i;
                var slotRoot = CreateImageButton($"Weapon Slot {i}", strip, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                    new Vector2(0.5f, 0.5f), new Vector2(148f, 148f), new Vector2(startX + i * spacing, 42f), "card",
                    () => SelectWeapon(index));
                var frame = slotRoot.GetComponent<Image>();
                var weapon = CreateImage($"Weapon Slot Art {i}", slotRoot.transform, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                    new Vector2(0.5f, 0.5f), new Vector2(112f, 112f), new Vector2(0f, 8f), _items[i].Art, true, null, true);
                var label = CreateText($"Weapon Slot Level {i}", slotRoot.transform, new Vector2(0.12f, -0.08f), new Vector2(0.88f, 0.16f),
                    new Vector2(0.5f, 0.5f), Vector2.zero, Vector2.zero, 18, TextAlignmentOptions.Center, $"LVL {_items[i].RequiredLevel}");
                _slots.Add(new SlotView
                {
                    Frame = frame,
                    Weapon = weapon,
                    Label = label
                });
            }
        }

        private void SelectWeapon(int index)
        {
            if (index < 0 || index >= _items.Count)
            {
                return;
            }

            _selectedIndex = index;
            var item = _items[index];
            _previewImage.sprite = LoadSprite(item.Art, true);
            _nameText.text = item.DisplayName;
            _levelText.text = $"Потрібен рівень {item.RequiredLevel}";
            _descriptionText.text = item.Description;
            _damageText.text = item.Definition.Damage.ToString();
            _blastText.text = item.Definition.ImpactRadius.ToString("0.0");
            _usesText.text = item.Definition.HasLimitedAmmo ? item.Definition.AmmoCount.ToString() : "∞";
            _coinPriceText.text = $"Купити  {item.CoinPrice}";
            _crystalPriceText.text = $"Купити  {item.CrystalPrice}";

            for (var i = 0; i < _slots.Count; i++)
            {
                var selected = i == index;
                _slots[i].Frame.color = selected ? SelectedSlotColor : NormalSlotColor;
                _slots[i].Weapon.localScale = selected ? new Vector3(1.12f, 1.12f, 1f) : Vector3.one;
                _slots[i].Label.color = selected ? Color.white : new Color(0.82f, 0.92f, 1f, 0.95f);
            }
        }

        private void BuySelectedWithCoins()
        {
        }

        private void BuySelectedWithCrystals()
        {
        }

        private void Close()
        {
            var callback = _onClosed;
            _onClosed = null;
            Destroy(gameObject);
            callback?.Invoke();
        }

        private RectTransform CreateImage(string name, Component parent, Vector2 anchorMin, Vector2 anchorMax,
            Vector2 pivot, Vector2 sizeDelta, Vector2 anchoredPosition, string resourceName, bool preserveAspect,
            Color? tint, bool weaponResource)
        {
            var rect = CreateRect(name, parent, anchorMin, anchorMax, pivot, sizeDelta, anchoredPosition);
            var image = rect.gameObject.AddComponent<Image>();
            image.sprite = LoadSprite(resourceName, weaponResource);
            image.color = tint ?? Color.white;
            image.preserveAspect = preserveAspect;
            image.raycastTarget = false;
            return rect;
        }

        private RectTransform CreateImageButton(string name, Component parent, Vector2 anchorMin, Vector2 anchorMax,
            Vector2 pivot, Vector2 sizeDelta, Vector2 anchoredPosition, string resourceName, Action onClick)
        {
            var rect = CreateImage(name, parent, anchorMin, anchorMax, pivot, sizeDelta, anchoredPosition, resourceName, true, null, false);
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
            return image;
        }

        private TMP_Text CreateText(string name, Component parent, Vector2 anchorMin, Vector2 anchorMax, Vector2 pivot,
            Vector2 sizeDelta, Vector2 anchoredPosition, int fontSize, TextAlignmentOptions alignment, string value)
        {
            var rect = CreateRect(name, parent, anchorMin, anchorMax, pivot, sizeDelta, anchoredPosition);
            var text = rect.gameObject.AddComponent<TextMeshProUGUI>();
            text.fontSize = fontSize;
            text.enableAutoSizing = true;
            text.fontSizeMin = Mathf.Max(14, fontSize - 10);
            text.fontSizeMax = fontSize;
            text.fontStyle = FontStyles.Bold;
            text.color = Color.white;
            text.alignment = alignment;
            text.textWrappingMode = TextWrappingModes.Normal;
            text.overflowMode = TextOverflowModes.Ellipsis;
            text.text = value;
            text.raycastTarget = false;

            var shadow = rect.gameObject.AddComponent<Shadow>();
            shadow.effectColor = TextShadowColor;
            shadow.effectDistance = new Vector2(0f, -2f);
            return text;
        }

        private Sprite LoadSprite(string resourceName, bool weaponResource)
        {
            var key = $"{(weaponResource ? "weapon" : "shop")}:{resourceName}";
            if (_sprites.TryGetValue(key, out var cached))
            {
                return cached;
            }

            var resourcePath = resourceName.Contains("/")
                ? resourceName
                : (weaponResource ? WeaponResourceRoot : ShopResourceRoot) + resourceName;
            var texture = Resources.Load<Texture2D>(resourcePath);
            if (texture == null)
            {
                return _whiteSprite;
            }

            var sprite = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height),
                new Vector2(0.5f, 0.5f), 100f);
            _sprites[key] = sprite;
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
