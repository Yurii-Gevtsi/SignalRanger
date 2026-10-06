using System;
using System.Collections.Generic;
using Game.Core.Localization;
using Game.Data.Progression;
using Game.Data.Save;
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
        private const string WorkshopStatIconsResource = "UI/Menu/Architect/icons";
        private const string WorkshopLeftPanelResource = "UI/Common/rect_long_card";
        private const int Columns = 7;
        private const int Rows = 6;
        private const float PhoneElementScale = 1.2f;

        private static readonly Color TextShadowColor = new(0.02f, 0.03f, 0.05f, 0.9f);
        private static readonly Vector2 BackButtonSize = new(118f, 118f);
        private static readonly Vector2 BackButtonPosition = new(30f, 0f);
        private static readonly Vector2 TitlePanelSize = new(560f, 110f);
        private static readonly Vector2 TitlePanelPosition = new(128f, 0f);
        private static readonly Vector2 SettingsButtonSize = new(104f, 104f);
        private static readonly Vector2 SettingsButtonPosition = new(-26f, -10f);
        private static readonly Vector2 BureauButtonSize = new(220f, 220f);
        private static readonly Vector2 BureauButtonPosition = new(-80f, 30f);
        private static readonly Vector2 BureauButtonScale = new(1.2f, 1.5f);
        private static readonly Vector2 WeaponIconSize = new(195f, 195f);
        private static readonly Vector2 DetailsCardSize = new(580f, 720f);
        private static readonly Vector2 DetailsHitAreaSize = new(580f, 720f);
        private static readonly Vector2 DetailsFrameSize = new(560f, 720f);
        private static readonly Vector2 DetailsStatIconSize = new(70f, 70f);
        private static readonly Color DetailsBarOuter = new Color32(0x12, 0x2F, 0x49, 0xFF);
        private static readonly Color DetailsBarTrack = new Color32(0x09, 0x34, 0x5D, 0xFF);
        private static readonly Color DetailsBarFill = new Color32(0x29, 0xD8, 0xFF, 0xFF);
        private static readonly Color DetailsDividerColor = new Color32(0x8A, 0x95, 0xA2, 0xB8);
        private static readonly Vector2 DetailsDividerSize = new(340f, 2f);

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
        private Sprite _roundedBarSprite;
        private Action _onClosed;
        private GameObject _detailsOverlay;
        private RectTransform _detailsCard;
        private TMP_Text _detailsNameText;
        private TMP_Text _detailsLevelText;
        private TMP_Text _detailsDescriptionText;
        private WorkshopStatRow[] _detailsStats;
        private ResourceInfoPopupController _resourceInfoPopup;

        private sealed class WorkshopStatRow
        {
            public RectTransform Row;
            public Image Icon;
            public TMP_Text Label;
            public Image Outer;
            public Image Bar;
            public Image Fill;
            public TMP_Text Value;
        }

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
            _roundedBarSprite = CreateRoundedRectSprite();
            BuildItems();
            Build();
        }

        private void BuildItems()
        {
            var weapons = PrototypeWeaponLibrary.CreateAll();
            for (var i = 0; i < weapons.Length; i++)
            {
                var weapon = weapons[i];
                if (weapon == null
                    || !WeaponCatalogAvailability.IsVisibleInArsenal(weapon.WeaponId)
                    || WeaponPresentationText.IsDropOnly(weapon)
                    || !AppServices.Inventory.IsWeaponUnlocked(weapon.WeaponId))
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
            MenuScreenInputShield.Create(root, "Weapons Input Shield");
            _resourceInfoPopup = ResourceInfoPopupController.Create(root);
            CreateImage("Weapons Background", root, Vector2.zero, Vector2.one, new Vector2(0.5f, 0.5f),
                Vector2.zero, Vector2.zero, "weapons_menu", false, null, false);

            BuildTopBar(root);
            BuildShelfWeapons(root);
            BuildConstructorBureauButton(root);
            BuildDetails(root);
        }

        private void BuildTopBar(Transform root)
        {
            var saveData = AppServices.SaveService.Current;
            var softCurrency = Mathf.Max(0, saveData?.economy.softCurrency ?? 0);
            var premiumCurrency = Mathf.Max(0, saveData?.economy.premiumCurrency ?? 0);

            var backButton = CreateImageButton("Arsenal Back", root, new Vector2(0f, 1f), new Vector2(0f, 1f),
                new Vector2(0f, 1f), BackButtonSize, BackButtonPosition, "back_btn", Close);
            backButton.localScale = new Vector3(PhoneElementScale, PhoneElementScale, 1f);

            var titlePanel = CreateImage("Arsenal Title Panel", root, new Vector2(0f, 1f), new Vector2(0f, 1f),
                new Vector2(0f, 1f), TitlePanelSize, TitlePanelPosition, ShopResourceRoot + "info_panel", true, null, false);
            titlePanel.localScale = new Vector3(PhoneElementScale, PhoneElementScale, 1f);
            CreateText("Arsenal Title", titlePanel.transform, Vector2.zero, Vector2.one, new Vector2(0.5f, 0.5f),
                new Vector2(-60f, -12f), new Vector2(-70f, 0f), 38, TextAlignmentOptions.Center,
                GameText.ArsenalTitle(), GameTextRole.DisplayTitle);
            MenuPlayerLevelPanel.Create(root, "Arsenal Player Level Panel", new Vector2(0f, 10f), new Vector2(520f, 172f));

            var coins = CreateImage("Arsenal Coins", root, new Vector2(1f, 1f), new Vector2(1f, 1f),
                new Vector2(1f, 1f), new Vector2(372f, 155f), new Vector2(-486f, 0f), "Coin_panel", true, null, false);
            _resourceInfoPopup.BindResourceIcon(coins.GetComponent<Image>(), ResourceInfoKind.Coins);
            var coinsValue = CreateText("Arsenal Coins Value", coins.transform, new Vector2(0.40f, 0.24f), new Vector2(0.80f, 0.80f),
                new Vector2(0.5f, 0.5f), Vector2.zero, Vector2.zero, 35, TextAlignmentOptions.Center,
                softCurrency.ToString(), GameTextRole.CurrencyGold);
            CurrencyBalanceText.Bind(coinsValue, false);

            var premium = CreateImage("Arsenal Premium", root, new Vector2(1f, 1f), new Vector2(1f, 1f),
                new Vector2(1f, 1f), new Vector2(372f, 155f), new Vector2(-166f, 0f), "vip_coins_panel", true, null, false);
            _resourceInfoPopup.BindResourceIcon(premium.GetComponent<Image>(), ResourceInfoKind.Crystals);
            var premiumValue = CreateText("Arsenal Premium Value", premium.transform, new Vector2(0.40f, 0.24f), new Vector2(0.80f, 0.80f),
                new Vector2(0.5f, 0.5f), Vector2.zero, Vector2.zero, 35, TextAlignmentOptions.Center,
                premiumCurrency.ToString(), GameTextRole.CurrencyCrystal);
            CurrencyBalanceText.Bind(premiumValue, true);

            var settingsButton = CreateImageButton("Arsenal Settings", root, new Vector2(1f, 1f), new Vector2(1f, 1f),
                new Vector2(1f, 1f), SettingsButtonSize, SettingsButtonPosition, "settings_btn",
                () => SettingsScreenController.Open(null));
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
                    new Vector2(0.5f, 0.5f), WeaponIconSize, position, ResolveShelfIconId(item.Definition), () => ShowDetails(item), true);
                ApplyWeaponVisual(icon, item.Definition.WeaponId);
            }
        }

        private void BuildConstructorBureauButton(Transform root)
        {
            var button = CreateImageButton("Constructor Bureau Button", root, new Vector2(1f, 0f), new Vector2(1f, 0f),
                new Vector2(1f, 0f), BureauButtonSize, BureauButtonPosition, "UI/Common/architect_btn",
                OpenConstructorBureau);
            button.localScale = new Vector3(BureauButtonScale.x, BureauButtonScale.y, 1f);

            var label = CreateText("Constructor Bureau Label", button.transform, new Vector2(0.5f, 0f), new Vector2(0.5f, 0f),
                new Vector2(0.5f, 1f), new Vector2(430f, 72f), new Vector2(-112f, -10f),
                30, TextAlignmentOptions.Center, GameText.ConstructorBureau(), GameTextRole.Button);
            label.enableAutoSizing = true;
            label.fontSizeMin = 20f;
            label.fontSizeMax = 30f;
        }

        private void OpenConstructorBureau()
        {
            var existingArchitectScreen = FindAnyObjectByType<ArchitectScreenController>();
            Destroy(gameObject);

            if (existingArchitectScreen == null)
            {
                ArchitectScreenController.Open(null);
            }
        }

        private void BuildDetails(Transform root)
        {
            _detailsOverlay = CreatePanel("Weapon Details Overlay", root, Vector2.zero, Vector2.one,
                new Vector2(0.5f, 0.5f), Vector2.zero, Vector2.zero, new Color(0f, 0f, 0f, 0.35f)).gameObject;
            _detailsOverlay.transform.localScale = new Vector3(1.2f, 1.2f, 1f);
            var overlayButton = _detailsOverlay.AddComponent<Button>();
            overlayButton.targetGraphic = _detailsOverlay.GetComponent<Image>();
            overlayButton.transition = Selectable.Transition.None;
            overlayButton.onClick.AddListener(HideDetails);

            _detailsCard = CreateRect("Weapon Details", _detailsOverlay.transform, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                new Vector2(0.5f, 0.5f), DetailsCardSize, Vector2.zero);
            CreateImage("Weapon Details Frame", _detailsCard, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                new Vector2(0.5f, 0.5f), DetailsFrameSize, Vector2.zero, WorkshopLeftPanelResource, false, Color.white, false);

            CreateDetailsHitArea(_detailsCard);
            CreateDetailsDivider(_detailsCard, "Weapon Details Divider Above Description", new Vector2(0f, 180f));
            CreateDetailsDivider(_detailsCard, "Weapon Details Divider Below Description", new Vector2(0f, 45f));

            _detailsNameText = CreateText("Weapon Details Name", _detailsCard, new Vector2(0.08f, 0.80f), new Vector2(0.92f, 0.89f),
                new Vector2(0.5f, 0.5f), Vector2.zero, Vector2.zero, 30, TextAlignmentOptions.Center, string.Empty, GameTextRole.DisplayTitle);
            _detailsNameText.enableAutoSizing = true;
            _detailsNameText.fontSizeMin = 22f;
            _detailsNameText.fontSizeMax = 30f;

            _detailsLevelText = CreateText("Weapon Details Level", _detailsCard, new Vector2(0.12f, 0.76f), new Vector2(0.88f, 0.81f),
                new Vector2(0.5f, 0.5f), Vector2.zero, Vector2.zero, 24, TextAlignmentOptions.Center, string.Empty, GameTextRole.Caption);

            _detailsDescriptionText = CreateText("Weapon Details Description", _detailsCard, new Vector2(0.20f, 0.60f), new Vector2(0.80f, 0.76f),
                new Vector2(0.5f, 1f), Vector2.zero, new Vector2(0f, -20f), 20, TextAlignmentOptions.TopLeft, string.Empty, GameTextRole.Body);
            _detailsDescriptionText.enableAutoSizing = false;
            _detailsDescriptionText.fontSizeMin = 18f;
            _detailsDescriptionText.fontSizeMax = 20f;
            _detailsDescriptionText.textWrappingMode = TextWrappingModes.Normal;
            _detailsDescriptionText.overflowMode = TextOverflowModes.Truncate;
            _detailsStats = new[]
            {
                CreateDetailsStatRow(_detailsCard, "Weapon Details Damage", "architect.detail.statDamage", 0.45f, 0),
                CreateDetailsStatRow(_detailsCard, "Weapon Details Accuracy", "architect.detail.statAccuracy", 0.36f, 1),
                CreateDetailsStatRow(_detailsCard, "Weapon Details Ammo", "architect.detail.statAmmo", 0.27f, 2),
                CreateDetailsStatRow(_detailsCard, "Weapon Details Radius", "architect.detail.statRadius", 0.18f, 3)
            };

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

        private void CreateDetailsDivider(Component parent, string name, Vector2 anchoredPosition)
        {
            var divider = CreatePanel(name, parent, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                new Vector2(0.5f, 0.5f), DetailsDividerSize, anchoredPosition, DetailsDividerColor);
            divider.raycastTarget = false;
        }

        private WorkshopStatRow CreateDetailsStatRow(Component parent, string name, string labelKey, float y,
            int iconIndex)
        {
            var iconPositionX = iconIndex switch
            {
                1 => 105f,
                2 => 94f,
                3 => 103f,
                _ => 98f
            };
            var row = CreateRect(name, parent, new Vector2(0.08f, y), new Vector2(0.92f, y + 0.065f),
                new Vector2(0.5f, 0.5f), Vector2.zero, Vector2.zero);
            var icon = CreateImage($"{name} Icon", row, new Vector2(0f, 0.5f), new Vector2(0f, 0.5f),
                new Vector2(0.5f, 0.5f), DetailsStatIconSize, new Vector2(iconPositionX, 0f), string.Empty, true,
                Color.white, false).GetComponent<Image>();
            icon.sprite = LoadDetailsStatIcon(iconIndex);
            icon.raycastTarget = false;

            var label = CreateText($"{name} Label", row, new Vector2(0.10f, 0f), new Vector2(0.10f, 1f),
                new Vector2(0f, 0.5f), new Vector2(220f, 0f), new Vector2(90f, 0f), 20,
                TextAlignmentOptions.MidlineLeft, GameText.Localized(labelKey, string.Empty), GameTextRole.Body);
            var outer = CreatePanel($"{name} Bar Outer", row, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                new Vector2(0.5f, 0.5f), new Vector2(160f, 17f), new Vector2(10f, 0f), DetailsBarOuter);
            outer.sprite = _roundedBarSprite;
            outer.type = Image.Type.Sliced;
            outer.fillCenter = true;
            outer.raycastTarget = false;
            var bar = CreatePanel($"{name} Bar", row, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                new Vector2(0.5f, 0.5f), new Vector2(156f, 13f), new Vector2(10f, 0f), DetailsBarTrack);
            bar.sprite = _roundedBarSprite;
            bar.type = Image.Type.Sliced;
            bar.fillCenter = true;
            bar.raycastTarget = false;
            var fill = CreatePanel($"{name} Bar Fill", row, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                new Vector2(0f, 0.5f), new Vector2(0f, 13f), new Vector2(-68f, 0f), DetailsBarFill);
            fill.sprite = _roundedBarSprite;
            fill.type = Image.Type.Sliced;
            fill.fillCenter = true;
            fill.gameObject.SetActive(false);
            fill.raycastTarget = false;
            var value = CreateText($"{name} Value", row, new Vector2(0.72f, 0f), new Vector2(0.90f, 1f),
                new Vector2(1f, 0.5f), Vector2.zero, new Vector2(-40f, 0f), 25,
                TextAlignmentOptions.MidlineRight, string.Empty, GameTextRole.Numeric);
            return new WorkshopStatRow
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

        private void RefreshDetailsStats(WeaponDefinition weapon, WeaponProgressionDefinition progression,
            SaveWeaponProgressData progress)
        {
            _detailsStats[0].Label.text = GameText.Localized(
                weapon.WeaponId == "weapon.grounder"
                    ? "architect.detail.statDuration"
                    : "architect.detail.statDamage", string.Empty);
            var values = new[]
            {
                WeaponUpgradeChoiceResolver.ResolveValue(weapon, progression, progress, WeaponUpgradePath.Core),
                WeaponUpgradeChoiceResolver.ResolveValue(weapon, progression, progress, WeaponUpgradePath.Aim),
                WeaponUpgradeChoiceResolver.ResolveValue(weapon, progression, progress, WeaponUpgradePath.Loadout),
                WeaponUpgradeChoiceResolver.ResolveValue(weapon, progression, progress, WeaponUpgradePath.Radius)
            };
            var maxValues = new[]
            {
                weapon.WeaponId == "weapon.grounder" ? 6f : Mathf.Max(1f, weapon.Damage * 1.5f),
                100f,
                weapon.HasLimitedAmmo ? Mathf.Max(1f, weapon.AmmoCount + 6f) : 1f,
                weapon.WeaponId == "weapon.bur"
                    ? WeaponUpgradeChoiceResolver.ResolveDrillLaserRange(4)
                    : Mathf.Max(0.1f, weapon.ImpactRadius * 1.5f)
            };

            Canvas.ForceUpdateCanvases();
            for (var i = 0; i < _detailsStats.Length; i++)
            {
                var stat = _detailsStats[i];
                var value = values[i];
                stat.Value.text = FormatDetailsStatValue(value);
                var fillAmount = weapon.HasLimitedAmmo || i != 2
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

        private static string FormatDetailsStatValue(float value)
        {
            return value < 0f ? "∞" : Mathf.Abs(value - Mathf.Round(value)) < 0.01f
                ? Mathf.RoundToInt(value).ToString()
                : value.ToString("0.0");
        }


        private void ShowDetails(OwnedWeaponItem item)
        {
            var weapon = item.Definition;
            _detailsOverlay.transform.SetAsLastSibling();
            _detailsNameText.text = WeaponDisplayNames.Get(weapon.WeaponId);
            var progress = AppServices.SaveService.Current.inventory.GetOrCreateWeaponProgress(weapon.WeaponId);
            _detailsLevelText.text = GameText.LevelLabel() + " " + progress.rank + "/10";
            var hasProgression = WeaponProgressionCatalog.TryGet(weapon.WeaponId, out var progression)
                && progression != null;
            _detailsDescriptionText.text = hasProgression
                ? WeaponUpgradePresentation.GetCurrentDescription(weapon, progression, progress)
                : WeaponPresentationText.Describe(weapon);
            _detailsOverlay.SetActive(true);
            if (hasProgression)
            {
                RefreshDetailsStats(weapon, progression, progress);
            }
        }

        private void HideDetails()
        {
            _detailsOverlay.SetActive(false);
        }

        /// <summary>
        /// The shelf icon reflects the weapon's rank 5/10 evolution art once the
        /// player has upgraded it, same as the workshop tree and battle HUD.
        /// </summary>
        private static string ResolveShelfIconId(WeaponDefinition weapon)
        {
            var progress = AppServices.SaveService.Current.inventory.GetOrCreateWeaponProgress(weapon.WeaponId);
            var resource = WeaponProgressionCatalog.TryGet(weapon.WeaponId, out var progression)
                ? WeaponEvolutionIconResolver.ResolveCurrentIconResource(weapon.IconId, progression, progress)
                : WeaponEvolutionIconResolver.ResolveCurrentIconResource(
                    weapon.IconId, progress.rank, (WeaponUpgradeBranch)progress.branch);
            const string prefix = "UI/Weapons/";
            return resource.StartsWith(prefix, StringComparison.Ordinal)
                ? resource.Substring(prefix.Length)
                : weapon.IconId;
        }

        private static void ApplyWeaponVisual(RectTransform rect, string weaponId)
        {
            WeaponIconVisualTuning.ApplyArsenal(rect, weaponId, rect.anchoredPosition);
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

        private Sprite LoadDetailsStatIcon(int iconIndex)
        {
            iconIndex = Mathf.Clamp(iconIndex, 0, 3);
            var resourceName = WorkshopStatIconsResource + "#" + iconIndex;
            if (_sprites.TryGetValue(resourceName, out var cached))
            {
                return cached;
            }

            var texture = Resources.Load<Texture2D>(WorkshopStatIconsResource);
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
            texture.name = "Weapons Details Rounded Bar";
            return Sprite.Create(texture, new Rect(0f, 0f, width, height), new Vector2(0.5f, 0.5f), 100f,
                0, SpriteMeshType.FullRect, new Vector4(radius + 1f, radius + 1f, radius + 1f, radius + 1f));
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
