using System;
using System.Collections.Generic;
using Game.Core.Localization;
using Game.Data.Progression;
using Game.Gameplay.Battle.Weapons;
using Game.UI;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.App
{
    public sealed class ShopScreenController : MonoBehaviour
    {
        private const string ShopResourceRoot = "UI/Shop/";
        private const string BattleResourceRoot = "UI/Battle/";
        private const string WeaponResourceRoot = "UI/Weapons/";
        private const float PhoneElementScale = 1.2f;

        private static readonly Color TextShadowColor = new(0.02f, 0.03f, 0.05f, 0.9f);
        private static readonly Color NormalSlotColor = new(0.72f, 0.82f, 0.92f, 0.88f);
        private static readonly Color LockedLevelColor = SciFiTextStyle.WarningText;
        private static readonly Color HologramColor = Color.white;
        // Layout uses the Canvas reference resolution (1920 x 1080).
        private static readonly Vector2 SettingsSize = new(104f, 104f);
        private static readonly Vector2 SettingsPosition = new(-26f, -10f);
        private static readonly Vector2 PreviewPosition = new(-35f, -18f);
        private static readonly Vector2 DetailCardSize = new(1160f, 1000f);
        private static readonly Vector2 DetailCardPosition = new(120f, 90f);
        private static readonly Vector2 PurchaseButtonSize = new(324f, 149f);
        private static readonly Vector2 PurchaseIconSize = new(60f, 60f);
        private static readonly Vector2 CarouselArrowSize = new(200f, 250f);
        private static readonly Vector2 CarouselArrowOffset = new(46f, 135f);
        private static readonly Vector2 WeaponStripPosition = new(120f, -128f);
        private static readonly Vector2 WeaponSlotSize = new(266f, 266f);
        private static readonly Vector2 WeaponArtSize = new(170f, 170f);
        private const int WeaponsPerPage = 7;

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
        private TMP_Text _usesText;
        private TMP_Text _coinPriceText;
        private TMP_Text _crystalPriceText;
        private Button _coinBuyButton;
        private Button _crystalBuyButton;
        private Button _leftArrowButton;
        private Button _rightArrowButton;
        private int _selectedIndex;
        private int _pageStart;
        private Vector2 _carouselDragStart;

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
            public string WeaponId;
            public Image Frame;
            public RectTransform Weapon;
            public TMP_Text Label;
            public Image Lock;
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
            ApplyPage();
        }

        private void BuildItems()
        {
            var weapons = PrototypeWeaponLibrary.CreateAll();
            for (var catalogIndex = 0; catalogIndex < WeaponProgressionCatalog.All.Count; catalogIndex++)
            {
                var progression = WeaponProgressionCatalog.All[catalogIndex];
                for (var weaponIndex = 0; weaponIndex < weapons.Length; weaponIndex++)
                {
                    var weapon = weapons[weaponIndex];
                    if (weapon == null || weapon.WeaponId != progression.WeaponId)
                    {
                        continue;
                    }

                    var progress = AppServices.SaveService.Current.inventory.GetOrCreateWeaponProgress(weapon.WeaponId);
                    var art = WeaponEvolutionIconResolver.ResolveCurrentIconResource(
                        weapon.IconId, progress.rank, (WeaponUpgradeBranch)progress.branch);
                    _items.Add(new ShopWeaponItem(
                        weapon,
                        art,
                        WeaponDisplayNames.Get(weapon.WeaponId),
                        progression.RequiredCommanderLevel,
                        WeaponPresentationText.Describe(weapon),
                        progression.CreditPrice,
                        progression.PremiumPrice));
                    break;
                }
            }
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
            // Landscape layout: match height so the canvas is always exactly 1080
            // reference units tall. Phones wider than 16:9 (19.5:9 etc.) gain extra
            // horizontal room instead of shrinking the vertical space, which kept
            // the fixed-height details card from fitting between the bars.
            scaler.matchWidthOrHeight = 1f;
            canvasObject.AddComponent<GraphicRaycaster>();

            var root = canvasObject.transform;
            CreateImage("Shop Background", root, Vector2.zero, Vector2.one, new Vector2(0.5f, 0.5f),
                Vector2.zero, Vector2.zero, "shop_scr_light", false, null, false);

            BuildTopBar(root);
            BuildPreview(root);
            BuildDetails(root);
            BuildWeaponStrip(root);
        }

        private void BuildTopBar(Transform root)
        {
            var saveData = AppServices.SaveService.Current;
            var softCurrency = Mathf.Max(0, saveData?.economy.softCurrency ?? 0);
            var premiumCurrency = Mathf.Max(0, saveData?.economy.premiumCurrency ?? 0);

            var backButton = CreateImageButton("Back Button", root, new Vector2(0f, 1f), new Vector2(0f, 1f), new Vector2(0f, 1f),
                new Vector2(118f, 118f), new Vector2(30f, 0f), "back_btn", Close);
            EnlargeForPhone(backButton);

            var titlePanel = CreateImage("Shop Title Panel", root, new Vector2(0f, 1f), new Vector2(0f, 1f), new Vector2(0f, 1f),
                new Vector2(560f, 110f), new Vector2(128f, 0f), "info_panel", true, null, false);
            EnlargeForPhone(titlePanel);
            CreateText("Shop Title", titlePanel.transform, Vector2.zero, Vector2.one, new Vector2(0.5f, 0.5f),
                new Vector2(-60f, -12f), new Vector2(-70f, 0f), 38, TextAlignmentOptions.Center, GameText.ShopTitle(), GameTextRole.DisplayTitle);

            var coins = CreateImage("Coins Panel", root, new Vector2(1f, 1f), new Vector2(1f, 1f), new Vector2(1f, 1f),
                new Vector2(372f, 155f), new Vector2(-486f, 0f), "Coin_panel", true, null, false);
            var coinsValue = CreateText("Coins Value", coins.transform, new Vector2(0.40f, 0.24f), new Vector2(0.80f, 0.80f),
                new Vector2(0.5f, 0.5f), Vector2.zero, Vector2.zero, 35, TextAlignmentOptions.Center, softCurrency.ToString(), GameTextRole.CurrencyGold);
            CurrencyBalanceText.Bind(coinsValue, false);

            var premium = CreateImage("Premium Panel", root, new Vector2(1f, 1f), new Vector2(1f, 1f), new Vector2(1f, 1f),
                new Vector2(372f, 155f), new Vector2(-166f, 0f), "vip_coins_panel", true, null, false);
            var premiumValue = CreateText("Premium Value", premium.transform, new Vector2(0.40f, 0.24f), new Vector2(0.80f, 0.80f),
                new Vector2(0.5f, 0.5f), Vector2.zero, Vector2.zero, 35, TextAlignmentOptions.Center, premiumCurrency.ToString(), GameTextRole.CurrencyCrystal);
            CurrencyBalanceText.Bind(premiumValue, true);

            var settingsButton = CreateImageButton("Settings Button", root, new Vector2(1f, 1f), new Vector2(1f, 1f), new Vector2(1f, 1f),
                SettingsSize, SettingsPosition, "settings_btn", () => SettingsScreenController.Open(null));
            EnlargeForPhone(settingsButton);
        }

        private void BuildPreview(Transform root)
        {
            _previewRoot = CreateRect("Weapon Preview Root", root, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                new Vector2(0.5f, 0.5f), new Vector2(620f, 520f), PreviewPosition);

            _previewImage = CreateImage("Selected Weapon", _previewRoot, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                new Vector2(0.5f, 0.5f), new Vector2(470f, 470f), new Vector2(0f, 62f), "bazooka", true, HologramColor, false)
                .GetComponent<Image>();
        }

        private void BuildDetails(Transform root)
        {
            var card = CreateImage("Weapon Details", root, new Vector2(1f, 0.5f), new Vector2(1f, 0.5f), new Vector2(1f, 0.5f),
                DetailCardSize, DetailCardPosition, BattleResourceRoot + "card", false, null, false, new Vector4(250f, 250f, 250f, 250f));
            var cardImage = card.GetComponent<Image>();
            cardImage.type = Image.Type.Sliced;
            cardImage.preserveAspect = false;

            _nameText = CreateText("Weapon Name", card.transform, new Vector2(0.18f, 0.70f), new Vector2(0.86f, 0.775f),
                new Vector2(0.5f, 0.5f), Vector2.zero, Vector2.zero, 44, TextAlignmentOptions.Center, string.Empty);
            _nameText.enableAutoSizing = false;

            _levelText = CreateText("Weapon Level", card.transform, new Vector2(0.20f, 0.625f), new Vector2(0.84f, 0.68f),
                new Vector2(0.5f, 0.5f), Vector2.zero, Vector2.zero, 30, TextAlignmentOptions.Center, string.Empty);
            _levelText.enableAutoSizing = false;
            _levelText.color = SciFiTextStyle.CyanText;

            _descriptionText = CreateText("Weapon Description", card.transform, new Vector2(0.19f, 0.465f), new Vector2(0.81f, 0.61f),
                new Vector2(0.5f, 0.5f), Vector2.zero, Vector2.zero, 29, TextAlignmentOptions.TopLeft, string.Empty, GameTextRole.Body);
            SetLeftOffset(_descriptionText.rectTransform, 40f);
            _descriptionText.enableAutoSizing = false;
            _descriptionText.overflowMode = TextOverflowModes.Overflow;
            _descriptionText.textWrappingMode = TextWrappingModes.Normal;

            _damageText = CreateStatLine(card.transform, "Damage Stat", new Vector2(0.19f, 0.465f), new Vector2(0.47f, 0.525f), GameText.Damage(), 40f);
            _usesText = CreateStatLine(card.transform, "Uses Stat", new Vector2(0.52f, 0.465f), new Vector2(0.80f, 0.525f), GameText.UsesPerBattle());

            MoveStatLine(_damageText, new Vector2(0.19f, 0.385f), new Vector2(0.47f, 0.445f));
            MoveStatLine(_usesText, new Vector2(0.52f, 0.385f), new Vector2(0.80f, 0.445f));

            var coinButton = CreateImageButton("Buy Coins", card.transform, new Vector2(0.5f, 0f), new Vector2(0.5f, 0f),
                new Vector2(0.5f, 0f), PurchaseButtonSize, new Vector2(-150f, 230f), "btn_green", BuySelectedWithCoins);
            coinButton.anchoredPosition = new Vector2(-176f, 200f);
            coinButton.GetComponent<Image>().preserveAspect = false;
            _coinBuyButton = coinButton.GetComponent<Button>();
            var coinIcon = CreateImage("Buy Coins Icon", coinButton, new Vector2(0f, 0.5f), new Vector2(0f, 0.5f),
                new Vector2(0f, 0.5f), PurchaseIconSize, new Vector2(46f, 0f), "coin", true, null, false);
            _coinPriceText = CreateText("Buy Coins Label", coinButton.transform, new Vector2(0.30f, 0.12f), new Vector2(0.90f, 0.88f),
                new Vector2(0.5f, 0.5f), Vector2.zero, Vector2.zero, 32, TextAlignmentOptions.Center, string.Empty, GameTextRole.CurrencyGold);
            ConfigurePriceLabel(_coinPriceText);

            var crystalButton = CreateImageButton("Buy Crystals", card.transform, new Vector2(0.5f, 0f), new Vector2(0.5f, 0f),
                new Vector2(0.5f, 0f), PurchaseButtonSize, new Vector2(150f, 230f), "btn_yellow", BuySelectedWithCrystals);
            crystalButton.anchoredPosition = new Vector2(176f, 200f);
            crystalButton.GetComponent<Image>().preserveAspect = false;
            _crystalBuyButton = crystalButton.GetComponent<Button>();
            var crystalIcon = CreateImage("Buy Crystals Icon", crystalButton, new Vector2(0f, 0.5f), new Vector2(0f, 0.5f),
                new Vector2(0f, 0.5f), PurchaseIconSize, new Vector2(46f, 0f), "vip_coin", true, null, false);
            _crystalPriceText = CreateText("Buy Crystals Label", crystalButton.transform, new Vector2(0.30f, 0.12f), new Vector2(0.90f, 0.88f),
                new Vector2(0.5f, 0.5f), Vector2.zero, Vector2.zero, 32, TextAlignmentOptions.Center, string.Empty, GameTextRole.CurrencyCrystal);
            ConfigurePriceLabel(_crystalPriceText);
        }

        private TMP_Text CreateStatLine(Transform parent, string name, Vector2 anchorMin, Vector2 anchorMax, string label, float leftOffset = 0f)
        {
            var line = CreatePanel(name, parent, anchorMin, anchorMax,
                new Vector2(0.5f, 0.5f), Vector2.zero, Vector2.zero, new Color(0f, 0f, 0f, 0.18f));
            if (leftOffset > 0f)
            {
                SetLeftOffset(line.rectTransform, leftOffset);
            }

            line.raycastTarget = false;
            CreateText($"{name} Label", line.transform, new Vector2(0.06f, 0f), new Vector2(0.48f, 1f),
                new Vector2(0.5f, 0.5f), Vector2.zero, Vector2.zero, 29, TextAlignmentOptions.MidlineLeft, label, GameTextRole.Body);
            var value = CreateText($"{name} Value", line.transform, new Vector2(0.48f, 0f), new Vector2(0.84f, 1f),
                new Vector2(0.5f, 0.5f), Vector2.zero, Vector2.zero, 31, TextAlignmentOptions.MidlineLeft, string.Empty, GameTextRole.Numeric);
            value.enableAutoSizing = false;
            return value;
        }

        private static void ConfigurePriceLabel(TMP_Text label)
        {
            label.enableAutoSizing = false;
            label.fontStyle = FontStyles.Bold;
            label.rectTransform.localScale = Vector3.one;
        }

        private static void SetLeftOffset(RectTransform rect, float left)
        {
            rect.offsetMin = new Vector2(left, rect.offsetMin.y);
        }

        private static void MoveStatLine(TMP_Text valueText, Vector2 anchorMin, Vector2 anchorMax)
        {
            if (valueText == null)
            {
                return;
            }

            var rect = valueText.transform.parent as RectTransform;
            if (rect == null)
            {
                return;
            }

            rect.anchorMin = anchorMin;
            rect.anchorMax = anchorMax;
            rect.anchoredPosition = Vector2.zero;
            rect.sizeDelta = Vector2.zero;
        }

        private void BuildWeaponStrip(Transform root)
        {
            var strip = CreateRect("Weapon Strip", root, new Vector2(0.5f, 0f), new Vector2(0.5f, 0f),
                new Vector2(0.5f, 0f), new Vector2(1840f, 300f), WeaponStripPosition);
            EnlargeForPhone(strip);
            AddCarouselInput(strip.gameObject);

            var startX = -840f;
            const float spacing = 240f;
            for (var i = 0; i < _items.Count; i++)
            {
                var index = i;
                var isLocked = IsCatalogLocked(_items[i]);
                var slotRoot = CreateImageButton($"Weapon Slot {i}", strip, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                    new Vector2(0.5f, 0.5f), WeaponSlotSize, new Vector2(startX + i * spacing, 74f), "card",
                    () => SelectWeapon(index));
                AddCarouselInput(slotRoot.gameObject);
                var frame = slotRoot.GetComponent<Image>();
                var weapon = CreateImage($"Weapon Slot Art {i}", slotRoot.transform, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                    new Vector2(0.5f, 0.5f), WeaponArtSize, new Vector2(0f, 12f), _items[i].Art, true, null, true);
                ApplySlotWeaponVisual(weapon, _items[i].Definition.WeaponId);
                var lockIcon = CreateImage($"Weapon Slot Lock {i}", slotRoot.transform, new Vector2(0.178f, 0.214f), new Vector2(0.178f, 0.214f),
                    new Vector2(0f, 0.5f), new Vector2(34f, 34f), Vector2.zero, "lock", true, null, false).GetComponent<Image>();
                lockIcon.enabled = isLocked;
                var labelMin = isLocked ? new Vector2(0.367f, 0.143f) : new Vector2(0.197f, 0.161f);
                var labelMax = isLocked ? new Vector2(0.897f, 0.303f) : new Vector2(0.897f, 0.321f);
                var label = CreateText($"Weapon Slot Level {i}", slotRoot.transform, labelMin, labelMax,
                new Vector2(0.5f, 0.5f), Vector2.zero, Vector2.zero, 18, TextAlignmentOptions.MidlineLeft,
                    GameText.LevelLabel() + " " + _items[i].RequiredLevel);
                _slots.Add(new SlotView
                {
                    WeaponId = _items[i].Definition.WeaponId,
                    Frame = frame,
                    Weapon = weapon,
                    Label = label,
                    Lock = lockIcon
                });
            }

            var leftArrow = CreateImageButton("Weapon Carousel Left", root, new Vector2(0f, 0f), new Vector2(0f, 0f),
                new Vector2(0.5f, 0.5f), CarouselArrowSize, CarouselArrowOffset, "arrow_btn", () => ChangePage(-1));
            leftArrow.GetComponent<Image>().preserveAspect = false;
            leftArrow.localScale = new Vector3(-PhoneElementScale, PhoneElementScale, 1f);
            _leftArrowButton = leftArrow.GetComponent<Button>();

            var rightArrow = CreateImageButton("Weapon Carousel Right", root, new Vector2(1f, 0f), new Vector2(1f, 0f),
                new Vector2(0.5f, 0.5f), CarouselArrowSize, new Vector2(-CarouselArrowOffset.x, CarouselArrowOffset.y), "arrow_btn", () => ChangePage(1));
            rightArrow.GetComponent<Image>().preserveAspect = false;
            EnlargeForPhone(rightArrow);
            _rightArrowButton = rightArrow.GetComponent<Button>();
        }

        private static void EnlargeForPhone(RectTransform rect)
        {
            rect.localScale = new Vector3(PhoneElementScale, PhoneElementScale, 1f);
        }

        private void AddCarouselInput(GameObject target)
        {
            var graphic = target.GetComponent<Graphic>();
            if (graphic == null)
            {
                var hitArea = target.AddComponent<Image>();
                hitArea.sprite = _whiteSprite;
                hitArea.color = new Color(1f, 1f, 1f, 0f);
                hitArea.raycastTarget = true;
            }
            else
            {
                graphic.raycastTarget = true;
            }

            var trigger = target.GetComponent<EventTrigger>() ?? target.AddComponent<EventTrigger>();
            trigger.triggers ??= new List<EventTrigger.Entry>();
            AddEvent(trigger, EventTriggerType.PointerDown, eventData =>
            {
                if (eventData is PointerEventData pointer)
                {
                    _carouselDragStart = pointer.position;
                }
            });
            AddEvent(trigger, EventTriggerType.BeginDrag, eventData =>
            {
                if (eventData is PointerEventData pointer)
                {
                    _carouselDragStart = pointer.position;
                }
            });
            AddEvent(trigger, EventTriggerType.EndDrag, eventData =>
            {
                var pointer = eventData as PointerEventData;
                if (pointer == null)
                {
                    return;
                }

                var delta = pointer.position - _carouselDragStart;
                if (Mathf.Abs(delta.x) < 90f || Mathf.Abs(delta.x) < Mathf.Abs(delta.y) * 1.2f)
                {
                    return;
                }

                ChangePage(delta.x < 0f ? 1 : -1);
            });
            AddEvent(trigger, EventTriggerType.Scroll, eventData =>
            {
                if (eventData is PointerEventData pointer && Mathf.Abs(pointer.scrollDelta.y) > 0.01f)
                {
                    ChangePage(pointer.scrollDelta.y < 0f ? 1 : -1);
                }
            });
        }

        private static void AddEvent(EventTrigger trigger, EventTriggerType type, Action<BaseEventData> callback)
        {
            var entry = new EventTrigger.Entry { eventID = type };
            entry.callback.AddListener(data => callback?.Invoke(data));
            trigger.triggers.Add(entry);
        }

        private void ChangePage(int direction)
        {
            var maxStart = GetMaxPageStart();
            if (maxStart <= 0) return;

            _pageStart = direction > 0 ? GetNextPageStart(maxStart) : GetPreviousPageStart(maxStart);
            ApplyPage();
        }

        private void ApplyPage()
        {
            for (var i = 0; i < _slots.Count; i++)
            {
                var visibleIndex = i - _pageStart;
                var visible = visibleIndex >= 0 && visibleIndex < WeaponsPerPage;
                _slots[i].Frame.gameObject.SetActive(visible);
                if (visible)
                {
                    _slots[i].Frame.rectTransform.anchoredPosition = new Vector2(-840f + visibleIndex * 240f, 74f);
                }
            }

            var maxStart = GetMaxPageStart();
            if (_leftArrowButton != null)
            {
                _leftArrowButton.interactable = _pageStart > 0;
            }

            if (_rightArrowButton != null)
            {
                _rightArrowButton.interactable = _pageStart < maxStart;
            }
        }

        private int GetMaxPageStart()
        {
            return Mathf.Max(0, _items.Count - WeaponsPerPage);
        }

        private int GetNextPageStart(int maxStart)
        {
            var next = _pageStart + WeaponsPerPage;
            return next >= maxStart ? maxStart : next;
        }

        private int GetPreviousPageStart(int maxStart)
        {
            if (_pageStart >= maxStart && maxStart % WeaponsPerPage != 0)
            {
                return Mathf.Max(0, (maxStart / WeaponsPerPage) * WeaponsPerPage);
            }

            return Mathf.Max(0, _pageStart - WeaponsPerPage);
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
            ApplyPreviewWeaponVisual(item.Definition.WeaponId);
            _nameText.text = item.DisplayName;
            _levelText.text = GameText.RequiredLevel(item.RequiredLevel);
            _descriptionText.text = item.Description;
            _damageText.text = item.Definition.Damage.ToString();
            _usesText.text = item.Definition.HasLimitedAmmo ? item.Definition.AmmoCount.ToString() : "∞";
            _coinPriceText.text = item.CoinPrice.ToString();
            _crystalPriceText.text = item.CrystalPrice.ToString();
            var isOwned = AppServices.Inventory.IsWeaponUnlocked(item.Definition.WeaponId);
            var meetsLevel = GetCommanderLevel() >= item.RequiredLevel;
            _coinBuyButton.interactable = !isOwned && meetsLevel && AppServices.Economy.CanSpend(item.CoinPrice);
            _crystalBuyButton.interactable = !isOwned && meetsLevel
                && item.CrystalPrice > 0
                && AppServices.Economy.CanSpendPremium(item.CrystalPrice);

            for (var i = 0; i < _slots.Count; i++)
            {
                var slotLocked = IsCatalogLocked(_items[i]);
                _slots[i].Frame.color = NormalSlotColor;
                _slots[i].Weapon.GetComponent<Image>().color = Color.white;
                ApplySlotWeaponVisual(_slots[i].Weapon, _slots[i].WeaponId);
                _slots[i].Label.color = slotLocked ? LockedLevelColor : SciFiTextStyle.SecondaryText;
                _slots[i].Lock.enabled = slotLocked;
            }
        }

        private static void ApplySlotWeaponVisual(RectTransform rect, string weaponId)
        {
            var tuning = GetWeaponVisualTuning(weaponId);
            rect.localScale = new Vector3(tuning.SlotScale.x, tuning.SlotScale.y, 1f);
            rect.anchoredPosition = tuning.SlotOffset;
        }

        private void ApplyPreviewWeaponVisual(string weaponId)
        {
            var tuning = GetWeaponVisualTuning(weaponId);
            var rect = _previewImage.rectTransform;
            rect.localScale = new Vector3(tuning.PreviewScale.x, tuning.PreviewScale.y, 1f);
            rect.anchoredPosition = tuning.PreviewOffset;
        }

        private readonly struct WeaponVisualTuning
        {
            public WeaponVisualTuning(Vector2 slotScale, Vector2 slotOffset, Vector2 previewScale, Vector2 previewOffset)
            {
                SlotScale = slotScale;
                SlotOffset = slotOffset;
                PreviewScale = previewScale;
                PreviewOffset = previewOffset;
            }

            public Vector2 SlotScale { get; }
            public Vector2 SlotOffset { get; }
            public Vector2 PreviewScale { get; }
            public Vector2 PreviewOffset { get; }
        }

        private static WeaponVisualTuning GetWeaponVisualTuning(string weaponId)
        {
            var normalSlotOffset = new Vector2(0f, 12f);
            var normalPreviewOffset = new Vector2(50f, 62f);

            return weaponId switch
            {
                "weapon.impulse" => new WeaponVisualTuning(new Vector2(1.10f, 1.10f), normalSlotOffset, Vector2.one, normalPreviewOffset),
                "weapon.blaster-caset" => new WeaponVisualTuning(new Vector2(0.84f, 0.84f), normalSlotOffset, Vector2.one, normalPreviewOffset),
                "weapon.bur" => new WeaponVisualTuning(new Vector2(0.78f, 0.78f), normalSlotOffset, new Vector2(0.82f, 0.82f), normalPreviewOffset),
                "weapon.mortira" => new WeaponVisualTuning(Vector2.one, normalSlotOffset, new Vector2(1.10f, 1.10f), normalPreviewOffset),
                "weapon.granata" => new WeaponVisualTuning(new Vector2(0.68f, 0.68f), normalSlotOffset, new Vector2(0.35f, 0.35f), normalPreviewOffset),
                "weapon.teleport" => new WeaponVisualTuning(new Vector2(0.48f, 0.48f), normalSlotOffset, new Vector2(0.46f, 0.46f), normalPreviewOffset),
                "weapon.mine" => new WeaponVisualTuning(new Vector2(0.78f, 0.78f), normalSlotOffset, new Vector2(0.78f, 0.78f), normalPreviewOffset),
                "weapon.heal" => new WeaponVisualTuning(new Vector2(0.82f, 0.82f), normalSlotOffset, Vector2.one, normalPreviewOffset),
                "weapon.emp" => new WeaponVisualTuning(new Vector2(0.82f, 0.72f), normalSlotOffset, new Vector2(0.86f, 0.72f), normalPreviewOffset),
                "weapon.fire-gun" => new WeaponVisualTuning(new Vector2(0.82f, 0.82f), normalSlotOffset, Vector2.one, normalPreviewOffset),
                "weapon.fpv-drone" => new WeaponVisualTuning(new Vector2(0.72f, 0.72f), normalSlotOffset, Vector2.one, normalPreviewOffset),
                _ => new WeaponVisualTuning(Vector2.one, normalSlotOffset, Vector2.one, normalPreviewOffset)
            };
        }

        private void BuySelectedWithCoins()
        {
            if (_selectedIndex < 0 || _selectedIndex >= _items.Count) return;
            var item = _items[_selectedIndex];
            var save = AppServices.SaveService.Current;
            if (save == null
                || AppServices.Inventory.IsWeaponUnlocked(item.Definition.WeaponId)
                || GetCommanderLevel() < item.RequiredLevel)
            {
                return;
            }

            var spend = AppServices.Economy.TrySpend(item.CoinPrice, "shop.weapon", item.Definition.WeaponId,
                $"shop.weapon.{item.Definition.WeaponId}");
            if (!spend.Success) return;

            AppServices.Inventory.UnlockWeapon(item.Definition.WeaponId);
            save.inventory.GetOrCreateWeaponProgress(item.Definition.WeaponId);
            AppServices.RefreshHistoricalPower();
            AppServices.SaveService.Save(save);
            SelectWeapon(_selectedIndex);
        }

        private void BuySelectedWithCrystals()
        {
            if (_selectedIndex < 0 || _selectedIndex >= _items.Count) return;
            var item = _items[_selectedIndex];
            var save = AppServices.SaveService.Current;
            if (save == null
                || AppServices.Inventory.IsWeaponUnlocked(item.Definition.WeaponId)
                || GetCommanderLevel() < item.RequiredLevel
                || item.CrystalPrice <= 0)
            {
                return;
            }

            var spend = AppServices.Economy.TrySpendPremium(item.CrystalPrice, "shop.weapon.premium",
                item.Definition.WeaponId, $"shop.weapon.premium.{item.Definition.WeaponId}");
            if (!spend.Success) return;

            AppServices.Inventory.UnlockWeapon(item.Definition.WeaponId);
            save.inventory.GetOrCreateWeaponProgress(item.Definition.WeaponId);
            AppServices.RefreshHistoricalPower();
            AppServices.SaveService.Save(save);
            SelectWeapon(_selectedIndex);
        }

        private static int GetCommanderLevel()
        {
            return CommanderProgressionTable.GetLevel(AppServices.Economy?.CommanderExperience ?? 0);
        }

        private static bool IsCatalogLocked(ShopWeaponItem item)
        {
            return !AppServices.Inventory.IsWeaponUnlocked(item.Definition.WeaponId)
                && GetCommanderLevel() < item.RequiredLevel;
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
            Vector2 sizeDelta, Vector2 anchoredPosition, int fontSize, TextAlignmentOptions alignment, string value,
            GameTextRole role = GameTextRole.Button)
        {
            var rect = CreateRect(name, parent, anchorMin, anchorMax, pivot, sizeDelta, anchoredPosition);
            var text = rect.gameObject.AddComponent<TextMeshProUGUI>();
            SciFiTextStyle.Apply(text, role, fontSize, alignment);
            text.fontSizeMin = Mathf.Max(14, fontSize - 10);
            text.textWrappingMode = TextWrappingModes.Normal;
            text.overflowMode = TextOverflowModes.Truncate;
            text.text = value;

            var shadow = rect.gameObject.AddComponent<Shadow>();
            shadow.effectColor = TextShadowColor;
            shadow.effectDistance = new Vector2(0f, -2f);
            return text;
        }

        private Sprite LoadSprite(string resourceName, bool weaponResource, Vector4 border = default)
        {
            var resourcePath = resourceName.Contains("/")
                ? resourceName
                : (weaponResource ? WeaponResourceRoot : ShopResourceRoot) + resourceName;
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
