using System;
using System.Collections.Generic;
using Game.Core.Localization;
using Game.Data.Progression;
using Game.Data.Save;
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
        private const string WeaponResourceRoot = "UI/Weapons/";
        private const string SelectedCarouselCardResource = "UI/Common/card_lighted";
        private const string ShopStatIconsResource = "UI/Menu/Architect/icons";
        private const string CurrentWeaponCardResource = "UI/Common/rect_card";
        private const float PhoneElementScale = 1.2f;

        private static readonly Color TextShadowColor = new(0.02f, 0.03f, 0.05f, 0.9f);
        private static readonly Color NormalSlotColor = new(0.72f, 0.82f, 0.92f, 0.88f);
        private static readonly Color LockedLevelColor = SciFiTextStyle.WarningText;
        private static readonly Color HologramColor = Color.white;
        // Layout uses the Canvas reference resolution (1920 x 1080).
        private static readonly Vector2 SettingsSize = new(104f, 104f);
        private static readonly Vector2 SettingsPosition = new(-26f, -10f);
        private static readonly Vector2 PreviewPosition = new(-35f, -18f);
        private static readonly Vector2 DetailCardSize = new(754f, 750f);
        private static readonly Vector2 DetailCardPosition = new(713f, 80f);
        private static readonly Vector2 PurchaseButtonSize = new(300f, 78f);
        private static readonly Vector2 PurchaseConfirmationButtonSize = new(324f, 132f);
        private static readonly Vector2 PurchaseIconSize = new(60f, 60f);
        private static readonly Vector2 CarouselArrowSize = new(200f, 250f);
        private static readonly Vector2 CarouselArrowOffset = new(46f, 135f);
        private static readonly Vector2 WeaponStripPosition = new(120f, -128f);
        private static readonly Vector2 WeaponSlotSize = new(266f, 266f);
        private static readonly Vector2 WeaponArtSize = new(170f, 170f);
        private static readonly Vector2 ShopStatIconSize = new(50f, 50f);
        private static readonly Vector2 PurchaseResourceIconSize = new(70f, 70f);
        private static readonly Color ShopBarOuter = new Color32(0x12, 0x2F, 0x49, 0xFF);
        private static readonly Color ShopBarTrack = new Color32(0x09, 0x34, 0x5D, 0xFF);
        private static readonly Color ShopBarFill = new Color32(0x29, 0xD8, 0xFF, 0xFF);
        private const float CarouselSpacing = 240f;
        private const float CarouselStartCenter = -120f;
        private const float CarouselEdgeAllowance = 100f;
        private const int MinimumVisibleCarouselCards = 3;
        private const int WeaponsPerPage = 7;

        private readonly Dictionary<string, Sprite> _sprites = new();
        private readonly List<SlotView> _slots = new();
        private readonly List<ShopWeaponItem> _items = new();

        private Sprite _whiteSprite;
        private Sprite _roundedBarSprite;
        private Action _onClosed;
        private RectTransform _previewRoot;
        private Image _previewImage;
        private TMP_Text _nameText;
        private TMP_Text _descriptionText;
        private TMP_Text _damageText;
        private TMP_Text _usesText;
        private ShopStatRow[] _ownedStats;
        private GameObject _purchaseSectionRoot;
        private Button _purchaseButton;
        private TMP_Text _coinPriceText;
        private TMP_Text _crystalPriceText;
        private GameObject _purchaseConfirmationRoot;
        private TMP_Text _purchaseConfirmationTitle;
        private TMP_Text _purchaseConfirmationMessage;
        private TMP_Text _confirmationCoinPriceText;
        private TMP_Text _confirmationCrystalPriceText;
        private Button _confirmationCoinButton;
        private Button _confirmationCrystalButton;
        private ResourceInfoPopupController _resourceInfoPopup;
        private Button _leftArrowButton;
        private Button _rightArrowButton;
        private RectTransform _weaponStrip;
        private int _selectedIndex;
        private int _pageStart;
        private Vector2 _carouselDragStart;
        private float _carouselScroll;
        private float _carouselDragStartScroll;
        private int _visibleCarouselCount = WeaponsPerPage;
        private bool _carouselDragMoved;
        private bool _suppressNextCarouselClick;

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
            public Image Purchased;
        }

        private sealed class ShopStatRow
        {
            public RectTransform Row;
            public Image Icon;
            public TMP_Text Label;
            public Image Outer;
            public Image Bar;
            public Image Fill;
            public TMP_Text Value;
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
            _roundedBarSprite = CreateRoundedRectSprite();
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
                if (!AppServices.IsAdminModeEnabled
                    && !WeaponCatalogAvailability.IsVisibleInBaseShop(progression))
                {
                    continue;
                }

                for (var weaponIndex = 0; weaponIndex < weapons.Length; weaponIndex++)
                {
                    var weapon = weapons[weaponIndex];
                    if (weapon == null || weapon.WeaponId != progression.WeaponId)
                    {
                        continue;
                    }

                    var progress = AppServices.SaveService.Current.inventory.GetOrCreateWeaponProgress(weapon.WeaponId);
                    var art = WeaponEvolutionIconResolver.ResolveCurrentIconResource(
                        weapon.IconId, progression, progress);
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
            MenuScreenInputShield.Create(root, "Shop Input Shield");
            _resourceInfoPopup = ResourceInfoPopupController.Create(root);
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
            MenuPlayerLevelPanel.Create(root, "Shop Player Level Panel", new Vector2(0f, 10f), new Vector2(520f, 172f));

            var coins = CreateImage("Coins Panel", root, new Vector2(1f, 1f), new Vector2(1f, 1f), new Vector2(1f, 1f),
                new Vector2(372f, 155f), new Vector2(-486f, 0f), "Coin_panel", true, null, false);
            _resourceInfoPopup.BindResourceIcon(coins.GetComponent<Image>(), ResourceInfoKind.Coins);
            var coinsValue = CreateText("Coins Value", coins.transform, new Vector2(0.40f, 0.24f), new Vector2(0.80f, 0.80f),
                new Vector2(0.5f, 0.5f), Vector2.zero, Vector2.zero, 35, TextAlignmentOptions.Center, softCurrency.ToString(), GameTextRole.CurrencyGold);
            CurrencyBalanceText.Bind(coinsValue, false);

            var premium = CreateImage("Premium Panel", root, new Vector2(1f, 1f), new Vector2(1f, 1f), new Vector2(1f, 1f),
                new Vector2(372f, 155f), new Vector2(-166f, 0f), "vip_coins_panel", true, null, false);
            _resourceInfoPopup.BindResourceIcon(premium.GetComponent<Image>(), ResourceInfoKind.Crystals);
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
            var card = CreateImage("Weapon Details", root, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                DetailCardSize, DetailCardPosition, CurrentWeaponCardResource, false, Color.white, false);

            _nameText = CreateText("Weapon Name", card.transform,
                new Vector2(0.08f, 0.80f), new Vector2(0.92f, 0.89f), new Vector2(0.5f, 0.5f),
                Vector2.zero, Vector2.zero, 32, TextAlignmentOptions.Center, string.Empty, GameTextRole.DisplayTitle);
            _nameText.enableAutoSizing = true;
            _nameText.fontSizeMin = 24f;
            _nameText.fontSizeMax = 32f;

            _descriptionText = CreateText("Weapon Description", card.transform,
                new Vector2(0.20f, 0.60f), new Vector2(0.80f, 0.76f), new Vector2(0.5f, 1f),
                Vector2.zero, new Vector2(0f, 40f), 22, TextAlignmentOptions.TopLeft, string.Empty, GameTextRole.Body);
            _descriptionText.enableAutoSizing = false;
            _descriptionText.fontSizeMin = 20f;
            _descriptionText.fontSizeMax = 22f;
            _descriptionText.textWrappingMode = TextWrappingModes.Normal;
            _descriptionText.overflowMode = TextOverflowModes.Truncate;

            _damageText = CreateStatLine(card.transform, "Damage Stat", Vector2.zero, Vector2.zero, GameText.Damage());
            _usesText = CreateStatLine(card.transform, "Uses Stat", Vector2.zero, Vector2.zero, GameText.UsesPerBattle());
            _damageText.transform.parent.gameObject.SetActive(false);
            _usesText.transform.parent.gameObject.SetActive(false);

            _ownedStats = new[]
            {
                CreateShopStatRow(card.transform, "Owned Damage Stat", "architect.detail.statDamage", 0.5967f, 0),
                CreateShopStatRow(card.transform, "Owned Accuracy Stat", "architect.detail.statAccuracy", 0.5192f, 1),
                CreateShopStatRow(card.transform, "Owned Ammo Stat", "architect.detail.statAmmo", 0.4417f, 2),
                CreateShopStatRow(card.transform, "Owned Radius Stat", "architect.detail.statRadius", 0.3642f, 3)
            };
            for (var i = 0; i < _ownedStats.Length; i++)
            {
                _ownedStats[i].Row.gameObject.SetActive(false);
            }

            var purchaseSection = CreateRect("Weapon Purchase Section", card.transform, Vector2.zero, Vector2.one,
                new Vector2(0.5f, 0.5f), Vector2.zero, Vector2.zero);
            _purchaseSectionRoot = purchaseSection.gameObject;

            var costLabel = CreateText("Weapon Purchase Cost", purchaseSection,
                new Vector2(0.10f, 0.20f), new Vector2(0.90f, 0.25f), new Vector2(0.5f, 0.5f),
                Vector2.zero, new Vector2(10f, 65f), 20, TextAlignmentOptions.Center,
                GameText.Localized("architect.cost", string.Empty), GameTextRole.Body);
            costLabel.enableAutoSizing = true;
            costLabel.fontSizeMin = 16f;
            costLabel.fontSizeMax = 20f;

            var purchaseCoinIcon = CreateImage("Weapon Purchase Coin Icon", purchaseSection, new Vector2(0.25f, 0.5f),
                new Vector2(0.25f, 0.5f), new Vector2(0.5f, 0.5f), PurchaseResourceIconSize,
                new Vector2(20f, -195f), "coin", true, Color.white, false);
            _resourceInfoPopup.BindResourceIcon(purchaseCoinIcon.GetComponent<Image>(), ResourceInfoKind.Coins);
            _coinPriceText = CreateText("Weapon Purchase Coin Amount", purchaseSection,
                new Vector2(0.29f, 0.13f), new Vector2(0.43f, 0.19f), new Vector2(0.5f, 0.5f),
                Vector2.zero, new Vector2(40f, 65f), 25, TextAlignmentOptions.MidlineLeft,
                string.Empty, GameTextRole.CurrencyGold);

            CreateText("Weapon Purchase Or", purchaseSection,
                new Vector2(0.44f, 0.13f), new Vector2(0.56f, 0.19f), new Vector2(0.5f, 0.5f),
                Vector2.zero, new Vector2(10f, 65f), 23, TextAlignmentOptions.Center,
                GameText.Localized("architect.or", string.Empty), GameTextRole.Caption);

            var purchaseCrystalIcon = CreateImage("Weapon Purchase Crystal Icon", purchaseSection, new Vector2(0.61f, 0.5f),
                new Vector2(0.61f, 0.5f), new Vector2(0.5f, 0.5f), PurchaseResourceIconSize,
                new Vector2(20f, -190f), "vip_coin", true, Color.white, false);
            _resourceInfoPopup.BindResourceIcon(purchaseCrystalIcon.GetComponent<Image>(), ResourceInfoKind.Crystals);
            _crystalPriceText = CreateText("Weapon Purchase Crystal Amount", purchaseSection,
                new Vector2(0.67f, 0.13f), new Vector2(0.90f, 0.19f), new Vector2(0.5f, 0.5f),
                Vector2.zero, new Vector2(20f, 65f), 25, TextAlignmentOptions.MidlineLeft,
                string.Empty, GameTextRole.CurrencyCrystal);

            var purchaseButton = CreateImageButton("Weapon Purchase Action", purchaseSection,
                new Vector2(0.5f, 0f), new Vector2(0.5f, 0f), new Vector2(0.5f, 0.5f),
                new Vector2(PurchaseButtonSize.x, 100f), new Vector2(10f, 120f), "btn_green", ShowPurchaseConfirmation);
            purchaseButton.GetComponent<Image>().preserveAspect = false;
            _purchaseButton = purchaseButton.GetComponent<Button>();
            var purchaseLabel = CreateText("Weapon Purchase Action Label", purchaseButton,
                Vector2.zero, Vector2.one, new Vector2(0.5f, 0.5f), Vector2.zero, Vector2.zero,
                25, TextAlignmentOptions.Center, GameText.Localized("shop.buy", string.Empty), GameTextRole.Button);
            purchaseLabel.raycastTarget = false;

            BuildPurchaseConfirmation(root);
        }

        private void BuildPurchaseConfirmation(Transform root)
        {
            var overlay = CreatePanel("Shop Purchase Confirmation Overlay", root, Vector2.zero, Vector2.one,
                new Vector2(0.5f, 0.5f), Vector2.zero, Vector2.zero, new Color(0f, 0f, 0f, 0.55f));
            _purchaseConfirmationRoot = overlay.gameObject;
            var overlayClickHandler = _purchaseConfirmationRoot.AddComponent<ShopPurchaseOverlayClickHandler>();
            overlayClickHandler.Initialize(this);

            var card = CreateImage("Shop Purchase Confirmation Card", overlay.transform,
                new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                new Vector2(1200f, 760f), new Vector2(0f, 120f), "UI/Battle/card", false, Color.white, false,
                new Vector4(250f, 250f, 250f, 250f));
            var cardImage = card.GetComponent<Image>();
            cardImage.type = Image.Type.Sliced;
            cardImage.preserveAspect = false;

            _purchaseConfirmationTitle = CreateText("Shop Purchase Confirmation Title", card,
                new Vector2(0.14f, 0.76f), new Vector2(0.86f, 0.88f), new Vector2(0.5f, 0.5f),
                Vector2.zero, new Vector2(0f, -100f), 44, TextAlignmentOptions.Center, string.Empty,
                GameTextRole.DisplayTitle);
            _purchaseConfirmationTitle.enableAutoSizing = true;
            _purchaseConfirmationTitle.fontSizeMin = 28f;
            _purchaseConfirmationTitle.fontSizeMax = 44f;

            _purchaseConfirmationMessage = CreateText("Shop Purchase Confirmation Message", card,
                new Vector2(0.18f, 0.51f), new Vector2(0.82f, 0.68f), new Vector2(0.5f, 0.5f),
                Vector2.zero, Vector2.zero, 30, TextAlignmentOptions.Center,
                GameText.Localized("shop.confirmPurchase", string.Empty), GameTextRole.Body);
            _purchaseConfirmationMessage.enableAutoSizing = true;
            _purchaseConfirmationMessage.fontSizeMin = 22f;
            _purchaseConfirmationMessage.fontSizeMax = 30f;
            _purchaseConfirmationMessage.textWrappingMode = TextWrappingModes.Normal;

            var coinButton = CreateImageButton("Shop Confirm Coins", card, new Vector2(0.5f, 0f),
                new Vector2(0.5f, 0f), new Vector2(0.5f, 0f), PurchaseConfirmationButtonSize,
                new Vector2(-176f, 200f), "btn_green", ConfirmPurchaseWithCoins);
            coinButton.GetComponent<Image>().preserveAspect = false;
            _confirmationCoinButton = coinButton.GetComponent<Button>();
            var confirmationCoinIcon = CreateImage("Shop Confirm Coins Icon", coinButton, new Vector2(0f, 0.5f), new Vector2(0f, 0.5f),
                new Vector2(0f, 0.5f), PurchaseIconSize, new Vector2(46f, 0f), "coin", true, Color.white, false);
            _resourceInfoPopup.BindResourceIcon(confirmationCoinIcon.GetComponent<Image>(), ResourceInfoKind.Coins);
            _confirmationCoinPriceText = CreateText("Shop Confirm Coins Amount", coinButton,
                new Vector2(0.30f, 0.12f), new Vector2(0.90f, 0.88f), new Vector2(0.5f, 0.5f),
                Vector2.zero, Vector2.zero, 32, TextAlignmentOptions.Center, string.Empty,
                GameTextRole.CurrencyGold);
            ConfigurePriceLabel(_confirmationCoinPriceText);

            var crystalButton = CreateImageButton("Shop Confirm Crystals", card, new Vector2(0.5f, 0f),
                new Vector2(0.5f, 0f), new Vector2(0.5f, 0f), PurchaseConfirmationButtonSize,
                new Vector2(176f, 200f), "btn_yellow", ConfirmPurchaseWithCrystals);
            crystalButton.GetComponent<Image>().preserveAspect = false;
            _confirmationCrystalButton = crystalButton.GetComponent<Button>();
            var confirmationCrystalIcon = CreateImage("Shop Confirm Crystals Icon", crystalButton, new Vector2(0f, 0.5f), new Vector2(0f, 0.5f),
                new Vector2(0f, 0.5f), PurchaseIconSize, new Vector2(46f, 0f), "vip_coin", true, Color.white, false);
            _resourceInfoPopup.BindResourceIcon(confirmationCrystalIcon.GetComponent<Image>(), ResourceInfoKind.Crystals);
            _confirmationCrystalPriceText = CreateText("Shop Confirm Crystals Amount", crystalButton,
                new Vector2(0.30f, 0.12f), new Vector2(0.90f, 0.88f), new Vector2(0.5f, 0.5f),
                Vector2.zero, Vector2.zero, 32, TextAlignmentOptions.Center, string.Empty,
                GameTextRole.CurrencyCrystal);
            ConfigurePriceLabel(_confirmationCrystalPriceText);

            _purchaseConfirmationRoot.SetActive(false);
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

        private ShopStatRow CreateShopStatRow(Transform parent, string name, string labelKey, float y, int iconIndex)
        {
            var iconPositionX = iconIndex switch
            {
                1 => 105f,
                2 => 94f,
                3 => 103f,
                _ => 98f
            };
            var row = CreateRect(name, parent, new Vector2(0.08f, y), new Vector2(0.92f, y + 0.065f),
                new Vector2(0.5f, 0.5f), Vector2.zero, new Vector2(0f, -20f));
            var icon = CreateImage($"{name} Icon", row, new Vector2(0f, 0.5f), new Vector2(0f, 0.5f),
                new Vector2(0.5f, 0.5f), ShopStatIconSize, new Vector2(iconPositionX, 0f), string.Empty, true, Color.white, false)
                .GetComponent<Image>();
            icon.sprite = LoadShopStatIcon(iconIndex);
            icon.raycastTarget = false;

            var label = CreateText($"{name} Label", row, new Vector2(0.10f, 0f), new Vector2(0.10f, 1f),
                new Vector2(0f, 0.5f), new Vector2(220f, 0f), new Vector2(90f, 0f), 20,
                TextAlignmentOptions.MidlineLeft, GameText.Localized(labelKey, string.Empty), GameTextRole.Body);
            var outer = CreatePanel($"{name} Bar Outer", row, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                new Vector2(0.5f, 0.5f), new Vector2(160f, 17f), new Vector2(10f, 0f), ShopBarOuter);
            outer.sprite = _roundedBarSprite;
            outer.type = Image.Type.Sliced;
            outer.fillCenter = true;
            outer.raycastTarget = false;
            var bar = CreatePanel($"{name} Bar", row, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                new Vector2(0.5f, 0.5f), new Vector2(156f, 13f), new Vector2(10f, 0f), ShopBarTrack);
            bar.sprite = _roundedBarSprite;
            bar.type = Image.Type.Sliced;
            bar.fillCenter = true;
            bar.raycastTarget = false;
            var fill = CreatePanel($"{name} Bar Fill", row, new Vector2(0.5f, 0.5f),
                new Vector2(0.5f, 0.5f), new Vector2(0f, 0.5f), new Vector2(0f, 13f),
                new Vector2(-68f, 0f), ShopBarFill);
            fill.sprite = _roundedBarSprite;
            fill.type = Image.Type.Sliced;
            fill.fillCenter = true;
            fill.gameObject.SetActive(false);
            fill.raycastTarget = false;
            var value = CreateText($"{name} Value", row, new Vector2(0.72f, 0f), new Vector2(0.90f, 1f),
                new Vector2(1f, 0.5f), Vector2.zero, new Vector2(-40f, 0f), 25,
                TextAlignmentOptions.MidlineRight, string.Empty, GameTextRole.Numeric);
            return new ShopStatRow
            {
                Row = row,
                Icon = icon,
                Label = label,
                Outer = outer,
                Bar = bar,
                Fill = fill,
                Value = value
            };
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
            _weaponStrip = strip;
            EnlargeForPhone(strip);
            AddCarouselInput(strip.gameObject);

            for (var i = 0; i < _items.Count; i++)
            {
                var index = i;
                var isLocked = IsCatalogLocked(_items[i]);
                var slotRoot = CreateImageButton($"Weapon Slot {i}", strip, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                    new Vector2(0.5f, 0.5f), WeaponSlotSize, new Vector2(0f, 74f), "card",
                    () => SelectWeaponFromCarousel(index));
                AddCarouselInput(slotRoot.gameObject);
                var frame = slotRoot.GetComponent<Image>();
                var weapon = CreateImage($"Weapon Slot Art {i}", slotRoot.transform, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                    new Vector2(0.5f, 0.5f), WeaponArtSize, new Vector2(0f, 12f), _items[i].Art, true, null, true);
                ApplySlotWeaponVisual(weapon, _items[i].Definition.WeaponId);
                var lockIcon = CreateImage($"Weapon Slot Lock {i}", slotRoot.transform, new Vector2(0.178f, 0.214f), new Vector2(0.178f, 0.214f),
                    new Vector2(0f, 0.5f), new Vector2(34f, 34f), Vector2.zero, "lock", true, null, false).GetComponent<Image>();
                lockIcon.enabled = isLocked;
                var purchasedIcon = CreateImage($"Weapon Slot Purchased {i}", slotRoot.transform,
                    new Vector2(1f, 1f), new Vector2(1f, 1f), new Vector2(1f, 1f),
                    new Vector2(52f, 52f), new Vector2(-34f, -34f), "UI/Menu/Rewards/Icons/check", true,
                    Color.white, false).GetComponent<Image>();
                purchasedIcon.enabled = AppServices.Inventory.IsWeaponUnlocked(_items[i].Definition.WeaponId);
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
                    Lock = lockIcon,
                    Purchased = purchasedIcon
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
                    _carouselDragStartScroll = _carouselScroll;
                    _carouselDragMoved = false;
                    _suppressNextCarouselClick = false;
                }
            });
            AddEvent(trigger, EventTriggerType.BeginDrag, eventData =>
            {
                if (eventData is PointerEventData pointer)
                {
                    _carouselDragStart = pointer.position;
                    _carouselDragStartScroll = _carouselScroll;
                }
            });
            AddEvent(trigger, EventTriggerType.Drag, eventData =>
            {
                if (eventData is not PointerEventData pointer)
                {
                    return;
                }

                var delta = pointer.position - _carouselDragStart;
                if (Mathf.Abs(delta.x) < 8f || Mathf.Abs(delta.x) < Mathf.Abs(delta.y) * 1.2f)
                {
                    return;
                }

                _carouselDragMoved = true;
                var canvasUnitsPerPixel = GetCarouselCanvasWidth() / Mathf.Max(1f, Screen.width);
                _carouselScroll = Mathf.Clamp(
                    _carouselDragStartScroll - delta.x * canvasUnitsPerPixel / CarouselSpacing,
                    0f,
                    GetMaxPageStart());
                ApplyPage();
            });
            AddEvent(trigger, EventTriggerType.EndDrag, eventData =>
            {
                if (eventData is not PointerEventData)
                {
                    return;
                }

                if (!_carouselDragMoved)
                {
                    return;
                }

                _suppressNextCarouselClick = true;
                _carouselScroll = Mathf.Clamp(Mathf.Round(_carouselScroll), 0f, GetMaxPageStart());
                _pageStart = Mathf.RoundToInt(_carouselScroll);
                _carouselDragMoved = false;
                ApplyPage();
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

            _carouselScroll = Mathf.Clamp(
                Mathf.Round(_carouselScroll) + direction * _visibleCarouselCount,
                0f,
                maxStart);
            _pageStart = Mathf.RoundToInt(_carouselScroll);
            ApplyPage();
        }

        private void ApplyPage()
        {
            _visibleCarouselCount = GetVisibleCarouselCount();
            var maxStart = GetMaxPageStart();
            _carouselScroll = Mathf.Clamp(_carouselScroll, 0f, maxStart);
            _pageStart = Mathf.RoundToInt(_carouselScroll);
            var startX = CarouselStartCenter - (_visibleCarouselCount - 1) * CarouselSpacing * 0.5f;

            for (var i = 0; i < _slots.Count; i++)
            {
                var visibleIndex = i - _carouselScroll;
                var visible = visibleIndex >= -0.75f && visibleIndex < _visibleCarouselCount - 0.25f;
                _slots[i].Frame.gameObject.SetActive(visible);
                if (visible)
                {
                    _slots[i].Frame.rectTransform.anchoredPosition = new Vector2(
                        startX + visibleIndex * CarouselSpacing, 74f);
                }
            }

            if (_leftArrowButton != null)
            {
                _leftArrowButton.interactable = _carouselScroll > 0.01f;
            }

            if (_rightArrowButton != null)
            {
                _rightArrowButton.interactable = _carouselScroll < maxStart - 0.01f;
            }
        }

        private int GetMaxPageStart()
        {
            return Mathf.Max(0, _items.Count - _visibleCarouselCount);
        }

        private int GetVisibleCarouselCount()
        {
            var canvasWidth = GetCarouselCanvasWidth();
            var arrowClearance = CarouselArrowOffset.x + CarouselArrowSize.x * 0.5f;
            var availableWidth = Mathf.Max(0f, canvasWidth - arrowClearance * 2f);
            var count = Mathf.FloorToInt(
                (availableWidth + CarouselEdgeAllowance - WeaponSlotSize.x) / CarouselSpacing) + 1;
            return Mathf.Clamp(count, MinimumVisibleCarouselCards, WeaponsPerPage);
        }

        private float GetCarouselCanvasWidth()
        {
            var canvas = _weaponStrip != null ? _weaponStrip.GetComponentInParent<Canvas>() : null;
            var rect = canvas != null ? canvas.GetComponent<RectTransform>() : null;
            return rect != null && rect.rect.width > 0f ? rect.rect.width : 1920f;
        }

        private void SelectWeaponFromCarousel(int index)
        {
            if (_suppressNextCarouselClick)
            {
                _suppressNextCarouselClick = false;
                return;
            }

            SelectWeapon(index);
        }

        private void SelectWeapon(int index)
        {
            if (index < 0 || index >= _items.Count)
            {
                return;
            }

            HidePurchaseConfirmation();
            _selectedIndex = index;
            var item = _items[index];
            _previewImage.sprite = LoadSprite(item.Art, true);
            ApplyPreviewWeaponVisual(item.Definition.WeaponId);
            _nameText.text = item.DisplayName;
            var isOwned = AppServices.Inventory.IsWeaponUnlocked(item.Definition.WeaponId);
            _descriptionText.text = item.Description;
            if (WeaponProgressionCatalog.TryGet(item.Definition.WeaponId, out var progression)
                && progression != null)
            {
                var progress = AppServices.SaveService.Current.inventory.GetOrCreateWeaponProgress(item.Definition.WeaponId);
                if (isOwned)
                {
                    _descriptionText.text = BuildOwnedDescription(item, progression, progress);
                }
                RefreshOwnedStats(item, progression, progress);
            }
            else
            {
                HideOwnedStats();
            }

            _damageText.transform.parent.gameObject.SetActive(false);
            _usesText.transform.parent.gameObject.SetActive(false);
            _coinPriceText.text = item.CoinPrice.ToString();
            _crystalPriceText.text = item.CrystalPrice.ToString();
            var meetsLevel = GetCommanderLevel() >= item.RequiredLevel;
            var canSpendCoins = AppServices.Economy.CanSpend(item.CoinPrice);
            var canSpendCrystals = item.CrystalPrice > 0
                && AppServices.Economy.CanSpendPremium(item.CrystalPrice);
            _coinPriceText.color = canSpendCoins ? SciFiTextStyle.GoldText : SciFiTextStyle.WarningText;
            _crystalPriceText.color = canSpendCrystals ? SciFiTextStyle.GoldText : SciFiTextStyle.WarningText;
            _purchaseSectionRoot.SetActive(!isOwned);
            _purchaseButton.gameObject.SetActive(!isOwned);
            _purchaseButton.interactable = !isOwned;

            for (var i = 0; i < _slots.Count; i++)
            {
                var slotLocked = IsCatalogLocked(_items[i]);
                _slots[i].Frame.sprite = i == _selectedIndex
                    ? LoadSprite(SelectedCarouselCardResource, false)
                    : LoadSprite("card", false);
                _slots[i].Frame.color = NormalSlotColor;
                _slots[i].Weapon.GetComponent<Image>().color = Color.white;
                ApplySlotWeaponVisual(_slots[i].Weapon, _slots[i].WeaponId);
                _slots[i].Label.color = slotLocked ? LockedLevelColor : SciFiTextStyle.SecondaryText;
                _slots[i].Lock.enabled = slotLocked;
                _slots[i].Purchased.enabled = AppServices.Inventory.IsWeaponUnlocked(_items[i].Definition.WeaponId);
            }
        }

        private static string BuildOwnedDescription(ShopWeaponItem item, WeaponProgressionDefinition progression,
            SaveWeaponProgressData progress)
        {
            return WeaponUpgradePresentation.GetCurrentDescription(item.Definition, progression, progress);
        }

        private void HideOwnedStats()
        {
            if (_ownedStats == null)
            {
                return;
            }

            for (var i = 0; i < _ownedStats.Length; i++)
            {
                _ownedStats[i].Row.gameObject.SetActive(false);
            }
        }

        private void RefreshOwnedStats(ShopWeaponItem item, WeaponProgressionDefinition progression,
            SaveWeaponProgressData progress)
        {
            if (_ownedStats == null || _ownedStats.Length != 4)
            {
                return;
            }

            var values = new[]
            {
                WeaponUpgradeChoiceResolver.ResolveValue(item.Definition, progression, progress, WeaponUpgradePath.Core),
                WeaponUpgradeChoiceResolver.ResolveValue(item.Definition, progression, progress, WeaponUpgradePath.Aim),
                WeaponUpgradeChoiceResolver.ResolveValue(item.Definition, progression, progress, WeaponUpgradePath.Loadout),
                WeaponUpgradeChoiceResolver.ResolveValue(item.Definition, progression, progress, WeaponUpgradePath.Radius)
            };
            var maxValues = new[]
            {
                Mathf.Max(1f, item.Definition.Damage * 1.5f),
                100f,
                item.Definition.HasLimitedAmmo ? Mathf.Max(1f, item.Definition.AmmoCount + 6f) : 1f,
                Mathf.Max(0.1f, item.Definition.ImpactRadius * 1.5f)
            };

            Canvas.ForceUpdateCanvases();
            for (var i = 0; i < _ownedStats.Length; i++)
            {
                var stat = _ownedStats[i];
                var value = values[i];
                stat.Row.gameObject.SetActive(true);
                stat.Label.text = GameText.Localized(GetShopStatLabelKey(i), string.Empty);
                stat.Value.text = FormatShopStatValue(value);
                var fillAmount = item.Definition.HasLimitedAmmo || i != 2
                    ? Mathf.Clamp01(value / maxValues[i])
                    : 1f;
                var rowWidth = Mathf.Max(1f, stat.Row.rect.width);
                var barRight = rowWidth * 0.19f + 10f;
                var labelLeft = rowWidth * (stat.Label.rectTransform.anchorMin.x - 0.5f)
                    + stat.Label.rectTransform.anchoredPosition.x;
                var barLeft = labelLeft + stat.Label.preferredWidth + 8f;
                barLeft = Mathf.Min(barLeft, barRight - 40f);
                var barWidth = Mathf.Max(40f, barRight - barLeft);
                var barCenter = (barLeft + barRight) * 0.5f;
                stat.Outer.rectTransform.anchoredPosition = new Vector2(barCenter, 0f);
                stat.Outer.rectTransform.sizeDelta = new Vector2(barWidth + 4f, 17f);
                stat.Bar.rectTransform.anchoredPosition = new Vector2(barCenter, 0f);
                stat.Bar.rectTransform.sizeDelta = new Vector2(barWidth, 13f);
                stat.Fill.rectTransform.anchoredPosition = new Vector2(barLeft, 0f);
                stat.Fill.rectTransform.sizeDelta = new Vector2(barWidth * fillAmount, 13f);
                stat.Fill.gameObject.SetActive(fillAmount > 0f);
            }
        }

        private static string GetShopStatLabelKey(int index)
        {
            return index switch
            {
                0 => "architect.detail.statDamage",
                1 => "architect.detail.statAccuracy",
                2 => "architect.detail.statAmmo",
                _ => "architect.detail.statRadius"
            };
        }

        private static string FormatShopStatValue(float value)
        {
            return value < 0f ? "∞" : Mathf.Abs(value - Mathf.Round(value)) < 0.01f
                ? Mathf.RoundToInt(value).ToString()
                : value.ToString("0.0");
        }

        private static void ApplySlotWeaponVisual(RectTransform rect, string weaponId)
        {
            WeaponIconVisualTuning.ApplyShopSlot(rect, weaponId, Vector2.zero);
        }

        private void ApplyPreviewWeaponVisual(string weaponId)
        {
            WeaponIconVisualTuning.ApplyShopPreview(_previewImage.rectTransform, weaponId, Vector2.zero);
        }

        private void ShowPurchaseConfirmation()
        {
            if (_purchaseConfirmationRoot == null || _selectedIndex < 0 || _selectedIndex >= _items.Count)
            {
                return;
            }

            var item = _items[_selectedIndex];
            if (AppServices.Inventory.IsWeaponUnlocked(item.Definition.WeaponId))
            {
                return;
            }

            var canSpendCoins = AppServices.Economy.CanSpend(item.CoinPrice);
            var canSpendCrystals = item.CrystalPrice > 0
                && AppServices.Economy.CanSpendPremium(item.CrystalPrice);
            if (GetCommanderLevel() < item.RequiredLevel || (!canSpendCoins && !canSpendCrystals))
            {
                ShowPurchaseRequirements(item, canSpendCoins, canSpendCrystals);
                return;
            }

            _purchaseConfirmationTitle.text = item.DisplayName;
            _purchaseConfirmationMessage.text = GameText.Localized("shop.confirmPurchase", string.Empty);
            _confirmationCoinPriceText.text = item.CoinPrice.ToString();
            _confirmationCrystalPriceText.text = item.CrystalPrice.ToString();

            _confirmationCoinPriceText.color = canSpendCoins ? SciFiTextStyle.GoldText : SciFiTextStyle.WarningText;
            _confirmationCrystalPriceText.color = canSpendCrystals ? SciFiTextStyle.GoldText : SciFiTextStyle.WarningText;
            _confirmationCoinButton.interactable = true;
            _confirmationCrystalButton.interactable = true;
            _purchaseConfirmationRoot.transform.SetAsLastSibling();
            _purchaseConfirmationRoot.SetActive(true);
        }

        private void HidePurchaseConfirmation()
        {
            if (_purchaseConfirmationRoot != null)
            {
                _purchaseConfirmationRoot.SetActive(false);
            }
        }

        private void ConfirmPurchaseWithCoins()
        {
            if (_selectedIndex < 0 || _selectedIndex >= _items.Count
                || !AppServices.Economy.CanSpend(_items[_selectedIndex].CoinPrice))
            {
                HidePurchaseConfirmation();
                _resourceInfoPopup.ShowMessage(GameText.ResourceRequirementsTitle(),
                    GameText.ResourceInsufficientBalance(), ResourceInfoKind.Coins);
                return;
            }

            HidePurchaseConfirmation();
            BuySelectedWithCoins();
        }

        private void ConfirmPurchaseWithCrystals()
        {
            if (_selectedIndex < 0 || _selectedIndex >= _items.Count
                || !AppServices.Economy.CanSpendPremium(_items[_selectedIndex].CrystalPrice))
            {
                HidePurchaseConfirmation();
                _resourceInfoPopup.ShowMessage(GameText.ResourceRequirementsTitle(),
                    GameText.ResourceInsufficientBalance(), ResourceInfoKind.Crystals);
                return;
            }

            HidePurchaseConfirmation();
            BuySelectedWithCrystals();
        }

        private void ShowPurchaseRequirements(ShopWeaponItem item, bool canSpendCoins, bool canSpendCrystals)
        {
            var messages = new List<string>();
            if (GetCommanderLevel() < item.RequiredLevel)
            {
                messages.Add(GameText.ResourceInsufficientLevel(item.RequiredLevel));
            }

            if (!canSpendCoins && !canSpendCrystals)
            {
                messages.Add(GameText.ResourceInsufficientBalance());
            }

            _resourceInfoPopup.ShowMessage(GameText.ResourceRequirementsTitle(),
                string.Join("\n\n", messages),
                GetCommanderLevel() < item.RequiredLevel ? ResourceInfoKind.Coins : null);
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
            return !AppServices.IsAdminModeEnabled
                && !AppServices.Inventory.IsWeaponUnlocked(item.Definition.WeaponId)
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

        private Sprite LoadShopStatIcon(int iconIndex)
        {
            iconIndex = Mathf.Clamp(iconIndex, 0, 3);
            var resourceName = ShopStatIconsResource + "#" + iconIndex;
            if (_sprites.TryGetValue(resourceName, out var cached))
            {
                return cached;
            }

            var texture = Resources.Load<Texture2D>(ShopStatIconsResource);
            if (texture == null || texture.width < 2 || texture.height < 2)
            {
                return _whiteSprite;
            }

            var cellSize = Mathf.Min(texture.width, texture.height) / 2f;
            var column = iconIndex % 2;
            var rowFromTop = iconIndex / 2;
            var rect = new Rect(column * cellSize, texture.height - (rowFromTop + 1) * cellSize,
                cellSize, cellSize);
            var sprite = Sprite.Create(texture, rect, new Vector2(0.5f, 0.5f), 100f);
            _sprites[resourceName] = sprite;
            return sprite;
        }

        private static Sprite CreateRoundedRectSprite(float radius = 8f)
        {
            const int width = 64;
            const int height = 32;
            var texture = new Texture2D(width, height, TextureFormat.RGBA32, false);
            var pixels = new Color32[width * height];
            var center = new Vector2((width - 1) * 0.5f, (height - 1) * 0.5f);
            var halfSize = new Vector2(width * 0.5f - 1f, height * 0.5f - 1f);

            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    var local = new Vector2(Mathf.Abs(x - center.x), Mathf.Abs(y - center.y));
                    var corner = local - (halfSize - Vector2.one * radius);
                    var outside = new Vector2(Mathf.Max(corner.x, 0f), Mathf.Max(corner.y, 0f)).magnitude;
                    var inside = Mathf.Min(Mathf.Max(corner.x, corner.y), 0f);
                    var signedDistance = outside + inside - radius;
                    var alpha = (byte)Mathf.RoundToInt(Mathf.Clamp01(0.5f - signedDistance) * 255f);
                    pixels[y * width + x] = new Color32(255, 255, 255, alpha);
                }
            }

            texture.SetPixels32(pixels);
            texture.Apply(false, true);
            texture.name = "Shop Rounded Bar";
            return Sprite.Create(texture, new Rect(0f, 0f, width, height), new Vector2(0.5f, 0.5f), 100f,
                0, SpriteMeshType.FullRect, new Vector4(radius + 1f, radius + 1f, radius + 1f, radius + 1f));
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

        private sealed class ShopPurchaseOverlayClickHandler : MonoBehaviour, IPointerClickHandler
        {
            private ShopScreenController _owner;

            public void Initialize(ShopScreenController owner)
            {
                _owner = owner;
            }

            public void OnPointerClick(PointerEventData eventData)
            {
                _owner?.HidePurchaseConfirmation();
            }
        }
    }
}
