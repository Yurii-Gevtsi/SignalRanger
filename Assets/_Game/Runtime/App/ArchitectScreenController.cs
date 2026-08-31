using System;
using System.Collections.Generic;
using Game.Core.Localization;
using Game.Data.Progression;
using Game.Data.Save;
using Game.Gameplay.Battle.Weapons;
using Game.Progression;
using Game.UI;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.App
{
    /// <summary>
    /// Blueprint-style architecture screen opened from the Architect Bureau button.
    /// The empty nodes are part of the supplied artwork and are intentionally left
    /// ready for future construction and progression content.
    /// </summary>
    public sealed class ArchitectScreenController : MonoBehaviour
    {
        private const string MenuResourceRoot = "UI/Menu/";
        private const string ShopResourceRoot = "UI/Shop/";
        private const string CheckmarkResource = "UI/Menu/Rewards/Icons/check";
        private const string ChevronResource = "UI/Common/chevron";
        private static readonly Color TextShadowColor = new(0.02f, 0.03f, 0.05f, 0.9f);
        private static readonly Vector2 BackButtonSize = new(118f, 118f);
        private static readonly Vector2 BackButtonPosition = new(30f, 0f);
        private static readonly Vector2 SettingsButtonSize = new(104f, 104f);
        private static readonly Vector2 SettingsButtonPosition = new(-26f, -10f);
        private static readonly Vector2 TitlePanelSize = new(850f, 110f);
        private static readonly Vector2 TitlePanelPosition = new(190f, 0f);
        private static readonly Color NormalSlotColor = new(0.72f, 0.82f, 0.92f, 0.88f);
        private static readonly Color LockedLevelColor = SciFiTextStyle.WarningText;
        private static readonly Vector2 CarouselArrowSize = new(195f, 260f);
        // Keep the carousel controls inside the safe visual area; the right control sits
        // slightly lower to align with the weapon row.
        private static readonly Vector2 CarouselLeftArrowOffset = new(130f, 135f);
        private static readonly Vector2 CarouselRightArrowOffset = new(-130f, 120f);
        private static readonly Vector2 WeaponStripPosition = new(220f, -160f);
        private static readonly Vector2 WeaponSlotSize = new(220f, 220f);
        private static readonly Vector2 WeaponArtSize = new(145f, 145f);
        private static readonly Vector2 SelectedWeaponNodePosition = new(-820f, 20f);
        // Sized to fill the enlarged rank-1/5/10 cells painted on the background.
        private static readonly Vector2 SelectedWeaponNodeSize = new(182f, 182f);
        private static readonly Vector2 EvolutionIconSize = new(185f, 157f);
        private static readonly Vector2 BlueprintChevronSize = new(83f, 83f);
        // Tall enough to stack the evolution art and its own (smaller) checkmark without
        // the two touching; plain ranks show only the full-size chevron/checkmark.
        private static readonly Vector2 EvolutionNodeHitAreaSize = new(136f, 200f);
        private static readonly Vector2 EvolutionCheckmarkSize = new(44f, 44f);
        // Per-rank because the rank-5 and rank-10 cells differ in width.
        private static readonly Vector2 EvolutionCheckmarkPositionRank5 = new(-70f, 50f);
        private static readonly Vector2 EvolutionCheckmarkPositionRank10 = new(-55f, 60f);
        private static readonly Vector2 DetailsCardSize = new(1200f, 800f);
        private static readonly Vector2 DetailsCardSizeEvolution = new(1200f, 1080f);
        private static readonly Vector2 DetailsArtMaxSize = new(430f, 280f);
        private static readonly Vector2 DetailsArtCenterOffset = new(0f, 130f);
        private static readonly Vector2 DetailsCardVisualInset = new(160f, 110f);
        private static readonly Vector2 DetailsLeftPosition = new(-240f, 0f);
        private static readonly Vector2 DetailsRightPosition = new(240f, 0f);
        private static readonly Vector2 DetailsCloseFixedPosition = new(340f, 300f);
        private static readonly Vector2 PurchaseButtonSize = new(324f, 149f);
        private static readonly Vector2 PurchaseIconSize = new(60f, 60f);
        private static readonly Vector2 ResetButtonSize = new(300f, 130f);
        private static readonly Vector2 ResetButtonPosition = new(750f, 300f);
        private static readonly Vector2 ResetButtonIconSize = new(50f, 50f);
        private static readonly Vector2 BlueprintBalanceSize = new(210f, 90f);
        private static readonly Vector2 BlueprintBalanceIconSize = new(96f, 96f);
        private const float BlueprintBalanceGap = 24f;
        private static readonly Color BlueprintBalanceColor = new Color32(0x00, 0xFF, 0x00, 0xFF);
        // Artwork that carries enough empty margin to read far smaller than the rest once
        // aspect-fitted, so it gets a per-weapon boost on the detail card.
        private static readonly Dictionary<string, DetailsArtBoost> DetailsArtBoosts = new(StringComparer.Ordinal)
        {
            { "weapon.impulse", new DetailsArtBoost(1.5f, true, WeaponUpgradeBranch.None) },
            { "weapon.blaster-caset", new DetailsArtBoost(1.5f, true, WeaponUpgradeBranch.None) },
            { "weapon.mortira", new DetailsArtBoost(1.5f, true, WeaponUpgradeBranch.None) },
            { "weapon.mine", new DetailsArtBoost(1.5f, true, WeaponUpgradeBranch.None) },
            { "weapon.heal", new DetailsArtBoost(1.5f, true, WeaponUpgradeBranch.None) },
            { "weapon.toxic", new DetailsArtBoost(1.5f, true, WeaponUpgradeBranch.None) },
            { "weapon.missle", new DetailsArtBoost(1.5f, true, WeaponUpgradeBranch.None) },
            { "weapon.bur", new DetailsArtBoost(1.3f, true, WeaponUpgradeBranch.None) },
            { "weapon.fire-gun", new DetailsArtBoost(1.3f, true, WeaponUpgradeBranch.None) },
            // Only the upper branch's art needs the boost at ranks 5/10.
            { "weapon.freeze", new DetailsArtBoost(1.3f, true, WeaponUpgradeBranch.A) },
            // Base configuration only - the evolution art is already large enough.
            { "weapon.c4", new DetailsArtBoost(1.3f, false, WeaponUpgradeBranch.None) }
        };
        private const float PhoneElementScale = 1.2f;
        private const int WeaponsPerPage = 7;

        private readonly Dictionary<string, Sprite> _sprites = new(StringComparer.Ordinal);
        private readonly List<WeaponDefinition> _items = new();
        private readonly List<SlotView> _slots = new();
        private readonly List<EvolutionIconView> _evolutionIcons = new();
        private readonly List<ChevronIconView> _chevronIcons = new();
        private readonly Dictionary<WeaponUpgradeBranch, List<GameObject>> _branchObjects = new();
        private Sprite _whiteSprite;
        private Transform _screenRoot;
        private Action _onClosed;
        private Button _leftArrowButton;
        private Button _rightArrowButton;
        private Image _blueprintBackgroundImage;
        private Image _selectedWeaponNodeImage;
        private GameObject _detailsOverlay;
        private RectTransform _detailsCard;
        private RectTransform _detailsCloseRect;
        private TMP_Text _detailsNameText;
        private RectTransform _detailsDescriptionViewport;
        private TMP_Text _detailsDescriptionText;
        private Image _detailsArtImage;
        private Image _detailsOwnedCheckmark;
        private TMP_Text _detailsPrimaryStatLabel;
        private Image _detailsPrimaryStatIcon;
        private TMP_Text _detailsDamageText;
        private TMP_Text _detailsSecondaryStatLabel;
        private TMP_Text _detailsUsesText;
        private TMP_Text _detailsCoinPriceText;
        private TMP_Text _detailsCrystalPriceText;
        private Button _detailsCoinBuyButton;
        private Button _detailsCrystalBuyButton;
        private Button _detailsResetButton;
        private TMP_Text _blueprintBalanceText;
        private int _selectedWeaponIndex;
        private int _pageStart;
        private Vector2 _carouselDragStart;
        private WeaponUpgradeBranch _pendingBranch;

        private struct SlotView
        {
            public int SlotIndex;
            public RectTransform Root;
            public Image Frame;
            public RectTransform Weapon;
            public Image WeaponImage;
            public TMP_Text Label;
            public Image Lock;
        }

        /// <summary>
        /// How much a weapon's detail-card art is enlarged, and where that applies:
        /// rank 1 always, the rank 5/10 evolutions only when <see cref="IncludesEvolutions"/>
        /// is set, and then only on <see cref="EvolutionBranch"/> unless that is
        /// <see cref="WeaponUpgradeBranch.None"/>, which means both branches.
        /// </summary>
        private readonly struct DetailsArtBoost
        {
            public DetailsArtBoost(float scale, bool includesEvolutions, WeaponUpgradeBranch evolutionBranch)
            {
                Scale = scale;
                IncludesEvolutions = includesEvolutions;
                EvolutionBranch = evolutionBranch;
            }

            public float Scale { get; }
            public bool IncludesEvolutions { get; }
            public WeaponUpgradeBranch EvolutionBranch { get; }

            public bool Covers(int rank, WeaponUpgradeBranch branch)
            {
                if (rank == 1)
                {
                    return true;
                }

                return IncludesEvolutions
                    && (EvolutionBranch == WeaponUpgradeBranch.None || EvolutionBranch == branch);
            }
        }

        private struct EvolutionIconView
        {
            public int Rank;
            public WeaponUpgradeBranch Branch;
            public Image Icon;
            public Image Checkmark;
        }

        private struct ChevronIconView
        {
            public int Rank;
            public WeaponUpgradeBranch Branch;
            public Image Icon;
        }

        private readonly struct BlueprintNodeLayout
        {
            public BlueprintNodeLayout(int number, Vector2 chevronPosition, Vector2 numberPosition)
            {
                Number = number;
                ChevronPosition = chevronPosition;
                NumberPosition = numberPosition;
            }

            public int Number { get; }
            public Vector2 ChevronPosition { get; }
            public Vector2 NumberPosition { get; }
        }

        public static void Open(Action onClosed)
        {
            if (FindAnyObjectByType<ArchitectScreenController>() != null)
            {
                return;
            }

            var root = new GameObject("Architect Screen");
            root.AddComponent<ArchitectScreenController>()._onClosed = onClosed;
        }

        private void Awake()
        {
            _whiteSprite = Sprite.Create(Texture2D.whiteTexture, new Rect(0f, 0f, 1f, 1f),
                new Vector2(0.5f, 0.5f), 1f);
            BuildItems();
            Build();
        }

        private void BuildItems()
        {
            foreach (var weapon in PrototypeWeaponLibrary.CreateAll())
            {
                if (weapon == null
                || WeaponPresentationText.IsDropOnly(weapon)
                || !AppServices.Inventory.IsWeaponUnlocked(weapon.WeaponId)
                    || !WeaponProgressionCatalog.TryGet(weapon.WeaponId, out var progression)
                    || progression == null
                    || !progression.HasDevelopment)
                {
                    continue;
                }

                _items.Add(weapon);
            }
        }

        private void Build()
        {
            var canvasObject = new GameObject("Architect Canvas");
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
            _screenRoot = root;
            _blueprintBackgroundImage = CreateImage("Architect Blueprint", root, Vector2.zero, Vector2.one,
                new Vector2(0.5f, 0.5f), Vector2.zero, Vector2.zero,
                MenuResourceRoot + "arch_menu", false, Color.white).GetComponent<Image>();

            BuildTopBar(root);
            BuildResetButton(root);
            BuildSelectedWeaponNode(root);
            BuildWeaponCarousel(root);
            // Keep progression nodes above the carousel hit area so evolution slots remain clickable.
            BuildBlueprintNodeLabels(root);
        }

        /// <summary>
        /// Sits on the blueprint background above the rank-1 weapon node, between the
        /// tree and the title panel.
        /// </summary>
        private void BuildResetButton(Transform root)
        {
            var button = CreateImageButton("Architect Reset Upgrade", root,
                new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                ResetButtonSize, ResetButtonPosition, ShopResourceRoot + "btn_red", ResetSelectedWeaponUpgrade);
            button.GetComponent<Image>().preserveAspect = false;
            _detailsResetButton = button.GetComponent<Button>();
            CreateImage("Architect Reset Upgrade Icon", button, new Vector2(0f, 0.5f), new Vector2(0f, 0.5f),
                new Vector2(0f, 0.5f), ResetButtonIconSize, new Vector2(48f, 0f), "UI/Common/reset_btn2", true, Color.white);
            var resetLabel = CreateText("Architect Reset Upgrade Label", button, new Vector2(0.34f, 0f),
                new Vector2(0.95f, 1f), new Vector2(0.5f, 0.5f), Vector2.zero, Vector2.zero, 30,
                TextAlignmentOptions.Center, GameText.ResetUpgrade(), GameTextRole.Button);
            resetLabel.rectTransform.offsetMin = new Vector2(resetLabel.rectTransform.offsetMin.x, 10f);
            resetLabel.rectTransform.offsetMax = new Vector2(-30f, resetLabel.rectTransform.offsetMax.y);
            _detailsResetButton.gameObject.SetActive(false);

            BuildBlueprintBalance(root);
        }

        /// <summary>
        /// Chip balance of the selected weapon, sitting immediately left of the reset
        /// button. Unlike the button it stays visible at rank 1, since the balance is
        /// what the player checks before deciding whether an upgrade is affordable.
        /// </summary>
        private void BuildBlueprintBalance(Transform root)
        {
            var group = CreateRect("Architect Blueprint Balance", root,
                new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(1f, 0.5f),
                BlueprintBalanceSize,
                new Vector2(ResetButtonPosition.x - ResetButtonSize.x * 0.5f - BlueprintBalanceGap,
                    ResetButtonPosition.y));

            CreateImage("Architect Blueprint Balance Icon", group, new Vector2(0f, 0.5f), new Vector2(0f, 0.5f),
                new Vector2(0f, 0.5f), BlueprintBalanceIconSize, Vector2.zero, "UI/Common/chip_2", true, Color.white);

            _blueprintBalanceText = CreateText("Architect Blueprint Balance Value", group,
                new Vector2(0.36f, 0f), new Vector2(1f, 1f), new Vector2(0.5f, 0.5f),
                Vector2.zero, Vector2.zero, 44, TextAlignmentOptions.MidlineLeft,
                string.Empty, GameTextRole.Numeric);
            // Clears the (now larger) chip icon so the number never sits on top of it.
            _blueprintBalanceText.rectTransform.offsetMin =
                new Vector2(20f, _blueprintBalanceText.rectTransform.offsetMin.y);
            _blueprintBalanceText.color = BlueprintBalanceColor;
        }

        private void RefreshResetButtonVisibility()
        {
            if (_selectedWeaponIndex < 0 || _selectedWeaponIndex >= _items.Count)
            {
                return;
            }

            var progress = AppServices.SaveService.Current.inventory.GetOrCreateWeaponProgress(_items[_selectedWeaponIndex].WeaponId);
            if (_detailsResetButton != null)
            {
                _detailsResetButton.gameObject.SetActive(progress.rank > 1);
            }

            if (_blueprintBalanceText != null)
            {
                _blueprintBalanceText.text = progress.blueprints.ToString();
            }
        }

        private void BuildTopBar(Transform root)
        {
            var saveData = AppServices.SaveService.Current;
            var softCurrency = Mathf.Max(0, saveData?.economy.softCurrency ?? 0);
            var premiumCurrency = Mathf.Max(0, saveData?.economy.premiumCurrency ?? 0);

            var backButton = CreateImageButton("Architect Back", root, new Vector2(0f, 1f), new Vector2(0f, 1f),
                new Vector2(0f, 1f), BackButtonSize, BackButtonPosition, ShopResourceRoot + "back_btn", Close);
            backButton.localScale = new Vector3(PhoneElementScale, PhoneElementScale, 1f);

            var titlePanel = CreateImage("Architect Title Panel", root, new Vector2(0f, 1f), new Vector2(0f, 1f),
                new Vector2(0f, 1f), TitlePanelSize, TitlePanelPosition, ShopResourceRoot + "info_panel", true, Color.white);
            titlePanel.localScale = new Vector3(PhoneElementScale, PhoneElementScale, 1f);
            CreateText("Architect Title", titlePanel.transform, Vector2.zero, Vector2.one, new Vector2(0.5f, 0.5f),
                new Vector2(-60f, -12f), new Vector2(-260f, 0f), 38, TextAlignmentOptions.Center,
                GameText.ConstructorBureau(), GameTextRole.DisplayTitle);

            var coins = CreateImage("Architect Coins", root, new Vector2(1f, 1f), new Vector2(1f, 1f),
                new Vector2(1f, 1f), new Vector2(372f, 155f), new Vector2(-486f, 0f),
                ShopResourceRoot + "Coin_panel", true, Color.white);
            var coinsValue = CreateText("Architect Coins Value", coins, new Vector2(0.40f, 0.24f), new Vector2(0.80f, 0.80f),
                new Vector2(0.5f, 0.5f), Vector2.zero, Vector2.zero, 35, TextAlignmentOptions.Center,
                softCurrency.ToString(), GameTextRole.CurrencyGold);
            CurrencyBalanceText.Bind(coinsValue, false);

            var premium = CreateImage("Architect Premium", root, new Vector2(1f, 1f), new Vector2(1f, 1f),
                new Vector2(1f, 1f), new Vector2(372f, 155f), new Vector2(-166f, 0f),
                ShopResourceRoot + "vip_coins_panel", true, Color.white);
            var premiumValue = CreateText("Architect Premium Value", premium, new Vector2(0.40f, 0.24f), new Vector2(0.80f, 0.80f),
                new Vector2(0.5f, 0.5f), Vector2.zero, Vector2.zero, 35, TextAlignmentOptions.Center,
                premiumCurrency.ToString(), GameTextRole.CurrencyCrystal);
            CurrencyBalanceText.Bind(premiumValue, true);

            var settingsButton = CreateImageButton("Architect Settings", root, new Vector2(1f, 1f), new Vector2(1f, 1f),
                new Vector2(1f, 1f), SettingsButtonSize, SettingsButtonPosition, ShopResourceRoot + "settings_btn",
                () => SettingsScreenController.Open(null));
            settingsButton.localScale = new Vector3(PhoneElementScale, PhoneElementScale, 1f);
        }

        private void BuildBlueprintNodeLabels(Transform root)
        {
            var nodes = new[]
            {
                // X values line each chevron up with the cell painted on the workshop
                // background; the spacing is deliberately uneven because the artwork's is.
                new BlueprintNodeLayout(2, new Vector2(-615f, 5f), new Vector2(-615f, 61f)),
                new BlueprintNodeLayout(3, new Vector2(-455f, 5f), new Vector2(-455f, 61f)),
                new BlueprintNodeLayout(4, new Vector2(-307f, 5f), new Vector2(-307f, 61f)),
                new BlueprintNodeLayout(5, new Vector2(-45f, 145f), new Vector2(-45f, 172f)),
                new BlueprintNodeLayout(6, new Vector2(158f, 130f), new Vector2(158f, 180f)),
                new BlueprintNodeLayout(7, new Vector2(325f, 130f), new Vector2(325f, 180f)),
                new BlueprintNodeLayout(8, new Vector2(495f, 130f), new Vector2(495f, 180f)),
                new BlueprintNodeLayout(9, new Vector2(663f, 130f), new Vector2(663f, 180f)),
                new BlueprintNodeLayout(10, new Vector2(835f, 145f), new Vector2(835f, 172f)),
                new BlueprintNodeLayout(5, new Vector2(-45f, -100f), new Vector2(-45f, -69f)),
                new BlueprintNodeLayout(6, new Vector2(158f, -115f), new Vector2(158f, -69f)),
                new BlueprintNodeLayout(7, new Vector2(325f, -115f), new Vector2(325f, -69f)),
                new BlueprintNodeLayout(8, new Vector2(495f, -115f), new Vector2(495f, -69f)),
                new BlueprintNodeLayout(9, new Vector2(663f, -115f), new Vector2(663f, -69f)),
                new BlueprintNodeLayout(10, new Vector2(835f, -115f), new Vector2(835f, -69f))
            };
            for (var i = 0; i < nodes.Length; i++)
            {
                var node = nodes[i];
                Action clickAction = () => ShowWeaponDetails(
                    _selectedWeaponIndex,
                    node.ChevronPosition.x < 0f ? DetailsRightPosition : DetailsLeftPosition,
                    node.Number >= 5 && node.ChevronPosition.y > 0f
                        ? WeaponUpgradeBranch.A
                        : node.Number >= 5 && node.ChevronPosition.y < 0f
                            ? WeaponUpgradeBranch.B
                            : WeaponUpgradeBranch.None,
                    node.Number);

                var isEvolutionNode = node.Number == 5 || node.Number == 10;
                var branch = node.Number >= 5
                    ? node.ChevronPosition.y > 0f ? WeaponUpgradeBranch.A : WeaponUpgradeBranch.B
                    : WeaponUpgradeBranch.None;
                if (isEvolutionNode)
                {
                    var hitArea = CreateTransparentButton($"Architect Evolution Hit Area {node.Number} {node.ChevronPosition.y}", root,
                        new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                        EvolutionNodeHitAreaSize, node.ChevronPosition, clickAction);
                    // The rank-10 lower-branch cell sits slightly lower than its sibling,
                    // so its art needs nudging back up to stay centred in the frame.
                    var iconOffset = node.Number == 10 && node.ChevronPosition.y < 0f
                        ? new Vector2(0f, 10f)
                        : Vector2.zero;
                    var icon = CreateImage($"Architect Evolution Icon {node.Number} {node.ChevronPosition.y}", hitArea,
                        new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                        EvolutionIconSize, iconOffset, string.Empty, true, Color.white)
                        .GetComponent<Image>();
                    // Evolution nodes show the weapon art instead of a chevron, so an
                    // owned rank needs its own checkmark stacked under that art - kept
                    // smaller than the plain-rank one so both fit the node.
                    var checkmark = CreateImage($"Architect Evolution Checkmark {node.Number} {node.ChevronPosition.y}", hitArea,
                        new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                        EvolutionCheckmarkSize,
                        node.Number == 5 ? EvolutionCheckmarkPositionRank5 : EvolutionCheckmarkPositionRank10,
                        CheckmarkResource, true, Color.white)
                        .GetComponent<Image>();
                    checkmark.enabled = false;
                    _evolutionIcons.Add(new EvolutionIconView
                    {
                        Rank = node.Number,
                        Branch = branch,
                        Icon = icon,
                        Checkmark = checkmark
                    });
                    RegisterBranchObject(branch, hitArea.gameObject);
                }
                else
                {
                    var chevronBranch = node.Number >= 5 && node.ChevronPosition.y > 0f
                        ? WeaponUpgradeBranch.A
                        : node.Number >= 5 && node.ChevronPosition.y < 0f
                            ? WeaponUpgradeBranch.B
                            : WeaponUpgradeBranch.None;
                    var chevronIcon = CreateImageButton($"Architect Node Chevron {node.Number}", root,
                        new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                        BlueprintChevronSize, node.ChevronPosition, "UI/Common/chevron", clickAction)
                        .GetComponent<Image>();
                    _chevronIcons.Add(new ChevronIconView { Rank = node.Number, Branch = chevronBranch, Icon = chevronIcon });
                    RegisterBranchObject(chevronBranch, chevronIcon.gameObject);
                }

                if (isEvolutionNode)
                {
                    continue;
                }

                var label = CreateText($"Architect Node Number {node.Number}", root,
                    new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                    new Vector2(150f, 48f), node.NumberPosition, 36,
                    TextAlignmentOptions.Center, node.Number.ToString(), GameTextRole.Caption);
                label.color = SciFiTextStyle.CyanText;
                RegisterBranchObject(branch, label.gameObject);
            }

            RefreshEvolutionIcons();
            RefreshChevronIcons();
            RefreshBranchVisibility();
        }

        private void BuildSelectedWeaponNode(Transform root)
        {
            var node = CreateRect("Architect Selected Weapon Node", root, new Vector2(0.5f, 0.5f),
                new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), SelectedWeaponNodeSize,
                SelectedWeaponNodePosition);
            _selectedWeaponNodeImage = node.gameObject.AddComponent<Image>();
            _selectedWeaponNodeImage.preserveAspect = true;
            _selectedWeaponNodeImage.raycastTarget = true;
            _selectedWeaponNodeImage.enabled = false;

            var button = node.gameObject.AddComponent<Button>();
            button.targetGraphic = _selectedWeaponNodeImage;
            button.transition = Selectable.Transition.None;
            button.onClick.AddListener(() => ShowWeaponDetails(_selectedWeaponIndex,
                SelectedWeaponNodePosition.x < 0f));
        }

        private void BuildWeaponCarousel(Transform root)
        {
            var strip = CreateRect("Architect Weapon Strip", root, new Vector2(0.5f, 0f), new Vector2(0.5f, 0f),
                new Vector2(0.5f, 0f), new Vector2(1840f, 300f), WeaponStripPosition);
            strip.localScale = new Vector3(PhoneElementScale, PhoneElementScale, 1f);
            AddCarouselInput(strip.gameObject);

            const float startX = -780f;
            const float spacing = 200f;
            var lockSprite = LoadSprite(ShopResourceRoot + "lock");
            for (var i = 0; i < WeaponsPerPage; i++)
            {
                var slotIndex = i;
                var slotPosition = new Vector2(startX + i * spacing, 90f);
                var slotRoot = CreateImageButton($"Architect Weapon Slot {i}", strip,
                    new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                    WeaponSlotSize, slotPosition, ShopResourceRoot + "card",
                    () => SelectCarouselSlot(slotIndex));
                AddCarouselInput(slotRoot.gameObject);

                var frame = slotRoot.GetComponent<Image>();
                frame.color = NormalSlotColor;
                var weaponArt = CreateRect($"Architect Weapon Art {i}", slotRoot.transform,
                    new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                    WeaponArtSize, new Vector2(0f, 12f));
                var weaponImage = weaponArt.gameObject.AddComponent<Image>();
                weaponImage.preserveAspect = true;
                weaponImage.color = Color.white;
                weaponImage.raycastTarget = false;

                var lockRect = CreateRect($"Architect Weapon Lock {i}", slotRoot.transform,
                    new Vector2(0.178f, 0.214f), new Vector2(0.178f, 0.214f), new Vector2(0f, 0.5f),
                    new Vector2(34f, 34f), Vector2.zero);
                var lockIcon = lockRect.gameObject.AddComponent<Image>();
                lockIcon.sprite = lockSprite;
                lockIcon.preserveAspect = true;
                lockIcon.raycastTarget = false;

                var label = CreateText($"Architect Weapon Level {i}", slotRoot.transform,
                    new Vector2(0.197f, 0.161f), new Vector2(0.897f, 0.321f),
                    new Vector2(0.5f, 0.5f), Vector2.zero, Vector2.zero, 18,
                    TextAlignmentOptions.MidlineLeft, string.Empty, GameTextRole.Caption);

                _slots.Add(new SlotView
                {
                    SlotIndex = i,
                    Root = slotRoot,
                    Frame = frame,
                    Weapon = weaponArt,
                    WeaponImage = weaponImage,
                    Label = label,
                    Lock = lockIcon
                });
            }

            var leftArrow = CreateImageButton("Architect Weapon Carousel Left", root,
                new Vector2(0f, 0f), new Vector2(0f, 0f), new Vector2(0.5f, 0.5f),
                CarouselArrowSize, CarouselLeftArrowOffset, ShopResourceRoot + "arrow_btn", () => ChangePage(-1));
            leftArrow.GetComponent<Image>().preserveAspect = false;
            leftArrow.localScale = new Vector3(-1f, 1f, 1f);
            _leftArrowButton = leftArrow.GetComponent<Button>();

            var rightArrow = CreateImageButton("Architect Weapon Carousel Right", root,
                new Vector2(1f, 0f), new Vector2(1f, 0f), new Vector2(0.5f, 0.5f),
                CarouselArrowSize, CarouselRightArrowOffset,
                ShopResourceRoot + "arrow_btn", () => ChangePage(1));
            rightArrow.GetComponent<Image>().preserveAspect = false;
            rightArrow.localScale = Vector3.one;
            _rightArrowButton = rightArrow.GetComponent<Button>();
            ApplyPage();
            SelectWeapon(0);
        }

        private void SelectWeapon(int index)
        {
            if (index < 0 || index >= _items.Count)
            {
                return;
            }

            var weapon = _items[index];
            // The tree's first node stands for the weapon's rank-1 state, so it keeps the
            // original art even after an evolution replaces the icon everywhere else.
            _selectedWeaponNodeImage.sprite = LoadSprite(WeaponEvolutionIconResolver.ResolveCurrentIconResource(
                weapon.IconId, 1, WeaponUpgradeBranch.None));
            _selectedWeaponNodeImage.enabled = _selectedWeaponNodeImage.sprite != null;
            _selectedWeaponIndex = index;

            var hasSingleBranch = WeaponProgressionCatalog.TryGet(weapon.WeaponId, out var progression)
                && progression != null
                && progression.HasDevelopment
                && (progression.HasBranch(WeaponUpgradeBranch.A) ^ progression.HasBranch(WeaponUpgradeBranch.B));
            _blueprintBackgroundImage.sprite = LoadSprite(MenuResourceRoot + (hasSingleBranch ? "arch_menu_2" : "arch_menu"));

            RefreshCarouselSlots();
            RefreshEvolutionIcons();
            RefreshChevronIcons();
            RefreshBranchVisibility();
            RefreshResetButtonVisibility();
        }

        private void RegisterBranchObject(WeaponUpgradeBranch branch, GameObject target)
        {
            if (branch == WeaponUpgradeBranch.None || target == null)
            {
                return;
            }

            if (!_branchObjects.TryGetValue(branch, out var objects))
            {
                objects = new List<GameObject>();
                _branchObjects.Add(branch, objects);
            }

            objects.Add(target);
        }

        private void RefreshBranchVisibility()
        {
            var hasA = false;
            var hasB = false;
            if (_selectedWeaponIndex >= 0 && _selectedWeaponIndex < _items.Count
                && WeaponProgressionCatalog.TryGet(_items[_selectedWeaponIndex].WeaponId, out var progression)
                && progression != null)
            {
                hasA = progression.HasBranch(WeaponUpgradeBranch.A);
                hasB = progression.HasBranch(WeaponUpgradeBranch.B);
            }

            SetBranchObjectsActive(WeaponUpgradeBranch.A, hasA);
            SetBranchObjectsActive(WeaponUpgradeBranch.B, hasB);
        }

        private void SetBranchObjectsActive(WeaponUpgradeBranch branch, bool active)
        {
            if (!_branchObjects.TryGetValue(branch, out var objects))
            {
                return;
            }

            for (var i = 0; i < objects.Count; i++)
            {
                if (objects[i] != null)
                {
                    objects[i].SetActive(active);
                }
            }
        }

        private void RefreshEvolutionIcons()
        {
            var iconId = _selectedWeaponIndex >= 0 && _selectedWeaponIndex < _items.Count
                ? _items[_selectedWeaponIndex].IconId
                : string.Empty;
            SaveWeaponProgressData progress = null;
            if (_selectedWeaponIndex >= 0 && _selectedWeaponIndex < _items.Count)
            {
                progress = AppServices.SaveService.Current.inventory.GetOrCreateWeaponProgress(_items[_selectedWeaponIndex].WeaponId);
            }

            for (var i = 0; i < _evolutionIcons.Count; i++)
            {
                var view = _evolutionIcons[i];
                var resourceName = WeaponEvolutionIconResolver.ResolveEvolutionIconResource(iconId, view.Rank, view.Branch);
                view.Icon.sprite = string.IsNullOrWhiteSpace(resourceName) ? null : LoadSprite(resourceName);
                view.Icon.enabled = !string.IsNullOrWhiteSpace(resourceName);
                if (view.Checkmark != null)
                {
                    view.Checkmark.enabled = progress != null
                        && progress.rank >= view.Rank
                        && (WeaponUpgradeBranch)progress.branch == view.Branch;
                }
            }
        }

        /// <summary>
        /// Ranks already purchased on the selected weapon show a green checkmark
        /// instead of the plain chevron, so the tree reads as progress instead of
        /// a flat row of identical arrows.
        /// </summary>
        private void RefreshChevronIcons()
        {
            if (_chevronIcons.Count == 0)
            {
                return;
            }

            SaveWeaponProgressData progress = null;
            if (_selectedWeaponIndex >= 0 && _selectedWeaponIndex < _items.Count)
            {
                progress = AppServices.SaveService.Current.inventory.GetOrCreateWeaponProgress(_items[_selectedWeaponIndex].WeaponId);
            }

            for (var i = 0; i < _chevronIcons.Count; i++)
            {
                var view = _chevronIcons[i];
                var owned = progress != null
                    && progress.rank >= view.Rank
                    && (view.Branch == WeaponUpgradeBranch.None || (WeaponUpgradeBranch)progress.branch == view.Branch);
                view.Icon.sprite = LoadSprite(owned ? CheckmarkResource : ChevronResource);
            }
        }

        private void SelectCarouselSlot(int slotIndex)
        {
            var itemIndex = _pageStart + slotIndex;
            if (itemIndex >= 0 && itemIndex < _items.Count)
            {
                SelectWeapon(itemIndex);
            }
        }

        private void BuildDetailsOverlay(Transform root)
        {
            var overlay = CreatePanel("Architect Weapon Details Overlay", root, Vector2.zero, Vector2.one,
                new Vector2(0.5f, 0.5f), Vector2.zero, Vector2.zero, new Color(0f, 0f, 0f, 0.35f));
            _detailsOverlay = overlay.gameObject;
            var overlayClickHandler = _detailsOverlay.AddComponent<DetailsOverlayClickHandler>();
            overlayClickHandler.Initialize(this);

            _detailsCard = CreateImage("Architect Weapon Details Card", _detailsOverlay.transform,
                new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                DetailsCardSize, Vector2.zero, "UI/Battle/card", false, Color.white).GetComponent<RectTransform>();
            var cardImage = _detailsCard.GetComponent<Image>();
            cardImage.type = Image.Type.Sliced;
            cardImage.preserveAspect = false;
            // The source card texture contains transparent outer padding. Let the overlay
            // receive clicks in that padding and decide whether they are outside the visible card.
            cardImage.raycastTarget = false;

            _detailsNameText = CreateText("Architect Weapon Details Name", _detailsCard,
                new Vector2(0.16f, 0.72f), new Vector2(0.84f, 0.82f), new Vector2(0.5f, 0.5f),
                Vector2.zero, Vector2.zero, 50, TextAlignmentOptions.Center, string.Empty, GameTextRole.DisplayTitle);
            _detailsNameText.enableAutoSizing = true;
            _detailsNameText.fontSizeMin = 28f;
            _detailsNameText.fontSizeMax = 50f;
            _detailsNameText.textWrappingMode = TextWrappingModes.Normal;
            _detailsNameText.overflowMode = TextOverflowModes.Truncate;

            _detailsDescriptionViewport = CreateRect("Architect Weapon Details Description Viewport", _detailsCard,
                new Vector2(0.18f, 0.48f), new Vector2(0.82f, 0.61f), new Vector2(0.5f, 0.5f),
                Vector2.zero, Vector2.zero);
            _detailsDescriptionViewport.offsetMin = new Vector2(20f, _detailsDescriptionViewport.offsetMin.y);
            // Nudge the top edge down so the description never rides up under the artwork.
            _detailsDescriptionViewport.offsetMax = new Vector2(_detailsDescriptionViewport.offsetMax.x, -20f);
            _detailsDescriptionViewport.gameObject.AddComponent<RectMask2D>();
            _detailsDescriptionText = CreateText("Architect Weapon Details Description", _detailsDescriptionViewport,
                Vector2.zero, Vector2.one, new Vector2(0.5f, 1f),
                Vector2.zero, Vector2.zero, 30, TextAlignmentOptions.Top, string.Empty, GameTextRole.Body);
            _detailsDescriptionText.enableAutoSizing = true;
            _detailsDescriptionText.fontSizeMin = 22f;
            _detailsDescriptionText.fontSizeMax = 30f;
            _detailsDescriptionText.textWrappingMode = TextWrappingModes.Normal;
            _detailsDescriptionText.overflowMode = TextOverflowModes.Truncate;

            _detailsArtImage = CreateImage("Architect Weapon Details Art", _detailsCard,
                new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                DetailsArtMaxSize, DetailsArtCenterOffset, string.Empty, true, Color.white).GetComponent<Image>();
            _detailsArtImage.gameObject.SetActive(false);

            _detailsDamageText = CreateDetailsStatLine(_detailsCard, "Architect Details Damage", GameText.Damage(),
                new Vector2(0.26f, 0.39f), new Vector2(0.50f, 0.46f), out _detailsPrimaryStatLabel);
            _detailsPrimaryStatIcon = CreateImage("Architect Details Chip Icon", _detailsPrimaryStatLabel.transform.parent,
                new Vector2(0.25f, 0.5f), new Vector2(0.25f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(96f, 96f), Vector2.zero,
                "UI/Common/chip_2", true, Color.white).GetComponent<Image>();
            _detailsPrimaryStatIcon.gameObject.SetActive(false);
            _detailsUsesText = CreateDetailsStatLine(_detailsCard, "Architect Details Uses", GameText.UsesPerBattle(),
                new Vector2(0.53f, 0.39f), new Vector2(0.80f, 0.46f), out _detailsSecondaryStatLabel);

            var coinButton = CreateImageButton("Architect Buy Coins", _detailsCard,
                new Vector2(0.5f, 0f), new Vector2(0.5f, 0f), new Vector2(0.5f, 0f),
                PurchaseButtonSize, new Vector2(-176f, 160f), ShopResourceRoot + "btn_green", BuyDetailsWithCoins);
            coinButton.GetComponent<Image>().preserveAspect = false;
            _detailsCoinBuyButton = coinButton.GetComponent<Button>();
            CreateImage("Architect Buy Coins Icon", coinButton, new Vector2(0f, 0.5f), new Vector2(0f, 0.5f),
                new Vector2(0f, 0.5f), PurchaseIconSize, new Vector2(46f, 0f), ShopResourceRoot + "coin", true, Color.white);
            _detailsCoinPriceText = CreateText("Architect Buy Coins Price", coinButton.transform,
                new Vector2(0.30f, 0.12f), new Vector2(0.90f, 0.88f), new Vector2(0.5f, 0.5f),
                Vector2.zero, Vector2.zero, 32, TextAlignmentOptions.Center, string.Empty, GameTextRole.CurrencyGold);
            _detailsCoinPriceText.fontStyle = FontStyles.Bold;

            var crystalButton = CreateImageButton("Architect Spend Blueprints", _detailsCard,
                new Vector2(0.5f, 0f), new Vector2(0.5f, 0f), new Vector2(0.5f, 0f),
                PurchaseButtonSize, new Vector2(176f, 160f), ShopResourceRoot + "btn_yellow", BuyDetailsWithBlueprints);
            crystalButton.GetComponent<Image>().preserveAspect = false;
            _detailsCrystalBuyButton = crystalButton.GetComponent<Button>();
            CreateImage("Architect Spend Blueprints Icon", crystalButton, new Vector2(0f, 0.5f), new Vector2(0f, 0.5f),
                new Vector2(0f, 0.5f), PurchaseIconSize, new Vector2(46f, 0f), ShopResourceRoot + "vip_coin", true, Color.white);
            _detailsCrystalPriceText = CreateText("Architect Buy Crystals Price", crystalButton.transform,
                new Vector2(0.30f, 0.12f), new Vector2(0.90f, 0.88f), new Vector2(0.5f, 0.5f),
                Vector2.zero, Vector2.zero, 32, TextAlignmentOptions.Center, string.Empty, GameTextRole.CurrencyCrystal);
            _detailsCrystalPriceText.fontStyle = FontStyles.Bold;

            _detailsOwnedCheckmark = CreateImage("Architect Details Owned Checkmark", _detailsCard,
                new Vector2(0.5f, 0f), new Vector2(0.5f, 0f), new Vector2(0.5f, 0f),
                new Vector2(90f, 90f), new Vector2(0f, 200f), CheckmarkResource, true, Color.white)
                .GetComponent<Image>();
            _detailsOwnedCheckmark.gameObject.SetActive(false);

            _detailsCloseRect = CreateRect("Architect Weapon Details Close", _detailsCard,
                new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                new Vector2(180f, 180f), DetailsCloseFixedPosition);
            var closeRect = _detailsCloseRect;
            var closeImage = closeRect.gameObject.AddComponent<Image>();
            closeImage.sprite = _whiteSprite;
            closeImage.color = new Color(1f, 1f, 1f, 0f);
            closeImage.raycastTarget = true;
            var closeButton = closeRect.gameObject.AddComponent<Button>();
            closeButton.targetGraphic = closeImage;
            closeButton.transition = Selectable.Transition.None;
            closeButton.onClick.AddListener(HideWeaponDetails);
            var closeText = CreateText("Architect Weapon Details Close Label", closeRect, Vector2.zero, Vector2.one,
                new Vector2(0.5f, 0.5f), Vector2.zero, Vector2.zero, 100, TextAlignmentOptions.Center,
                "\u00D7", GameTextRole.DisplayTitle);
            closeText.color = new Color(1f, 0.18f, 0.16f, 1f);
            closeRect.SetAsLastSibling();

            _detailsOverlay.SetActive(false);
        }

        private TMP_Text CreateDetailsStatLine(Component parent, string name, string label,
            Vector2 anchorMin, Vector2 anchorMax, out TMP_Text labelText)
        {
            var line = CreatePanel(name, parent, anchorMin, anchorMax, new Vector2(0.5f, 0.5f),
                Vector2.zero, Vector2.zero, new Color(0f, 0f, 0f, 0.18f));
            line.raycastTarget = false;
            labelText = CreateText($"{name} Label", line.transform, new Vector2(0.06f, 0f), new Vector2(0.48f, 1f),
                new Vector2(0.5f, 0.5f), Vector2.zero, Vector2.zero, 28, TextAlignmentOptions.MidlineLeft,
                label, GameTextRole.Body);
            return CreateText($"{name} Value", line.transform, new Vector2(0.48f, 0f), new Vector2(0.90f, 1f),
                new Vector2(0.5f, 0.5f), Vector2.zero, Vector2.zero, 31, TextAlignmentOptions.MidlineLeft,
                string.Empty, GameTextRole.Numeric);
        }

        private void ShowWeaponDetails(int index, bool showOnRight)
        {
            ShowWeaponDetails(index, showOnRight ? DetailsRightPosition : DetailsLeftPosition,
                WeaponUpgradeBranch.None, 1);
        }

        private void ShowWeaponDetails(int index, Vector2 cardPosition)
        {
            var weaponId = index >= 0 && index < _items.Count ? _items[index].WeaponId : string.Empty;
            var progress = AppServices.SaveService.Current.inventory.GetOrCreateWeaponProgress(weaponId);
            ShowWeaponDetails(index, cardPosition, _pendingBranch, Mathf.Min(10, progress.rank + 1));
        }

        private void ShowWeaponDetails(int index, Vector2 cardPosition, WeaponUpgradeBranch branch, int displayRank)
        {
            EnsureDetailsOverlay();
            if (_detailsOverlay == null || index < 0 || index >= _items.Count)
            {
                return;
            }

            var weapon = _items[index];
            if (!WeaponProgressionCatalog.TryGet(weapon.WeaponId, out var definition))
            {
                return;
            }

            var progress = AppServices.SaveService.Current.inventory.GetOrCreateWeaponProgress(weapon.WeaponId);
            displayRank = Mathf.Clamp(displayRank, 1, 10);
            if (displayRank < 5)
            {
                branch = WeaponUpgradeBranch.None;
            }

            var isEvolution = displayRank == 5 || displayRank == 10;
            var isBaseConfiguration = displayRank == 1;
            _pendingBranch = branch;
            _selectedWeaponIndex = index;
            _detailsOverlay.transform.SetAsLastSibling();
            ApplyDetailsLayout(isEvolution || isBaseConfiguration);
            _detailsCard.anchoredPosition = cardPosition;
            // Rank 1 documents the weapon as it shipped, so it shows the base art the
            // same way ranks 5/10 show their evolution art.
            var artResource = isEvolution
                ? WeaponEvolutionIconResolver.ResolveEvolutionIconResource(weapon.IconId, displayRank, branch)
                : isBaseConfiguration
                    ? WeaponEvolutionIconResolver.ResolveCurrentIconResource(weapon.IconId, 1, WeaponUpgradeBranch.None)
                    : string.Empty;
            _detailsArtImage.gameObject.SetActive(!string.IsNullOrWhiteSpace(artResource));
            if (!string.IsNullOrWhiteSpace(artResource))
            {
                _detailsArtImage.sprite = LoadSprite(artResource);
                ApplyDetailsArtFit(_detailsArtImage,
                    ResolveDetailsArtScale(weapon.WeaponId, displayRank, branch));
            }
            _detailsNameText.text = WeaponUpgradePresentation.GetWeaponName(
                definition,
                displayRank,
                branch,
                WeaponDisplayNames.Get(weapon.WeaponId));
            ApplyDetailsNameFit();

            var isOwned = isBaseConfiguration
                || (displayRank <= progress.rank
                    && (displayRank < 5 || (WeaponUpgradeBranch)progress.branch == branch));
            var isCurrentRank = isBaseConfiguration || (isOwned && displayRank == progress.rank);
            var isPastRank = isOwned && !isCurrentRank;
            // Every rank (not just the 5/10 evolutions) gets a description of what
            // improves, so incremental nodes read as more than a bare percentage.
            _detailsDescriptionText.gameObject.SetActive(true);
            WeaponUpgradeCost cost = default;
            var hasCost = !isBaseConfiguration && WeaponUpgradeEconomy.TryGetCost(displayRank, out cost);
            var requiredCommanderLevel = isBaseConfiguration
                ? definition.RequiredCommanderLevel
                : WeaponUpgradeEconomy.GetRequiredCommanderLevel(definition, displayRank);
            var primaryRowRoot = _detailsDamageText.transform.parent.gameObject;
            var secondaryRowRoot = _detailsUsesText.transform.parent.gameObject;
            if (isCurrentRank)
            {
                // Rank 1 is a historical record of the weapon's original state, so its
                // stats stay at the base values no matter how far the player has upgraded;
                // every other "current rank" card reflects what is actually fielded now.
                var statRank = isBaseConfiguration ? 1 : progress.rank;
                var currentBranch = isBaseConfiguration
                    ? WeaponUpgradeBranch.None
                    : (WeaponUpgradeBranch)progress.branch;
                var evolutionText = WeaponUpgradePresentation.GetActiveEvolutionText(definition, statRank, currentBranch);
                _detailsDescriptionText.text = string.IsNullOrWhiteSpace(evolutionText)
                    ? WeaponPresentationText.Describe(weapon)
                    : $"{WeaponPresentationText.Describe(weapon)} {evolutionText}";
                _detailsPrimaryStatIcon.gameObject.SetActive(false);
                _detailsPrimaryStatLabel.gameObject.SetActive(true);
                primaryRowRoot.SetActive(true);
                _detailsPrimaryStatLabel.text = GameText.Damage();
                _detailsDamageText.text = WeaponUpgradePresentation
                    .ResolveCurrentDamage(weapon, definition, statRank, currentBranch)
                    .ToString();
                _detailsDamageText.color = SciFiTextStyle.GoldText;
                secondaryRowRoot.SetActive(true);
                _detailsSecondaryStatLabel.text = GameText.UsesPerBattle();
                _detailsUsesText.text = weapon.HasLimitedAmmo ? weapon.AmmoCount.ToString() : "\u221E";
                _detailsUsesText.color = SciFiTextStyle.GoldText;
            }
            else if (isPastRank)
            {
                // Already surpassed rank: only the historical modifier text and the
                // checkmark matter here, so hide both stat rows entirely.
                _detailsDescriptionText.text = WeaponUpgradePresentation.GetDescription(definition, displayRank, branch);
                primaryRowRoot.SetActive(false);
                secondaryRowRoot.SetActive(false);
            }
            else
            {
                _detailsDescriptionText.text = WeaponUpgradePresentation.GetDescription(definition, displayRank, branch);
                _detailsPrimaryStatLabel.gameObject.SetActive(false);
                _detailsPrimaryStatIcon.gameObject.SetActive(true);
                primaryRowRoot.SetActive(true);
                var ownedBlueprints = progress.blueprints;
                var requiredBlueprints = hasCost ? cost.Blueprints : 0;
                _detailsDamageText.text = $"{ownedBlueprints}/{requiredBlueprints}";
                _detailsDamageText.color = ownedBlueprints >= requiredBlueprints
                    ? SciFiTextStyle.GoldText
                    : SciFiTextStyle.WarningText;
                secondaryRowRoot.SetActive(true);
                _detailsSecondaryStatLabel.text = GameText.LevelLabel();
                _detailsUsesText.text = requiredCommanderLevel.ToString();
                _detailsUsesText.color = CommanderProgressionTable.GetLevel(AppServices.Economy.CommanderExperience) >= requiredCommanderLevel
                    ? SciFiTextStyle.GoldText
                    : SciFiTextStyle.WarningText;
            }

            var targetRank = progress.rank + 1;
            var hasBranch = displayRank < 5 || branch != WeaponUpgradeBranch.None;
            var isNextUpgrade = targetRank == displayRank;
            _detailsCoinPriceText.text = hasCost ? cost.Credits.ToString() : "-";
            _detailsCrystalPriceText.text = hasCost ? cost.Blueprints.ToString() : "-";
            var branchLocked = displayRank >= 5
                && progress.branch != 0
                && (WeaponUpgradeBranch)progress.branch != branch;
            var hasRequiredLevel = CommanderProgressionTable.GetLevel(AppServices.Economy.CommanderExperience) >= requiredCommanderLevel;
            var hasRequiredCredits = AppServices.Economy.CanSpend(cost.Credits);
            var hasRequiredBlueprints = AppServices.Economy.AdminModeEnabled || progress.blueprints >= cost.Blueprints;
            _detailsCoinPriceText.color = hasRequiredCredits ? SciFiTextStyle.GoldText : SciFiTextStyle.WarningText;
            _detailsCrystalPriceText.color = hasRequiredBlueprints ? SciFiTextStyle.GoldText : SciFiTextStyle.WarningText;
            var canUpgrade = hasCost
                && isNextUpgrade
                && hasBranch
                && !branchLocked
                && hasRequiredLevel
                && hasRequiredCredits
                && hasRequiredBlueprints;

            _detailsCoinBuyButton.gameObject.SetActive(!isOwned);
            _detailsCrystalBuyButton.gameObject.SetActive(!isOwned);
            _detailsOwnedCheckmark.gameObject.SetActive(isOwned);
            _detailsCoinBuyButton.interactable = canUpgrade;
            _detailsCrystalBuyButton.interactable = canUpgrade;
            _detailsOverlay.SetActive(true);
        }

        private void ResetSelectedWeaponUpgrade()
        {
            if (_selectedWeaponIndex < 0 || _selectedWeaponIndex >= _items.Count)
            {
                return;
            }

            var weapon = _items[_selectedWeaponIndex];
            if (!AppServices.WeaponUpgrades.TryReset(weapon.WeaponId))
            {
                return;
            }

            AppServices.SaveService.Save(AppServices.SaveService.Current);
            SelectWeapon(_selectedWeaponIndex);
            HideWeaponDetails();
        }

        /// <summary>
        /// Ranks that carry artwork - the 5/10 evolutions and the rank-1 base
        /// configuration - get a taller card with a big weapon image slotted between the
        /// name and the description; plain upgrade ranks keep the compact layout.
        /// </summary>
        private void ApplyDetailsLayout(bool showsArt)
        {
            _detailsCard.sizeDelta = showsArt ? DetailsCardSizeEvolution : DetailsCardSize;

            var nameRect = _detailsNameText.rectTransform;
            var descriptionRect = _detailsDescriptionViewport;
            var damageRowRect = (RectTransform)_detailsDamageText.transform.parent;
            var usesRowRect = (RectTransform)_detailsUsesText.transform.parent;

            // Widest symmetric column that still clears the fixed-position close button
            // (its left edge sits at card-local x 250), so names get as much single-line
            // room as possible before <see cref="ApplyDetailsNameFit"/> allows a wrap.
            nameRect.anchorMin = new Vector2(0.30f, nameRect.anchorMin.y);
            nameRect.anchorMax = new Vector2(0.70f, nameRect.anchorMax.y);

            if (showsArt)
            {
                nameRect.anchorMin = new Vector2(nameRect.anchorMin.x, 0.72f);
                nameRect.anchorMax = new Vector2(nameRect.anchorMax.x, 0.83f);
                descriptionRect.anchorMin = new Vector2(descriptionRect.anchorMin.x, 0.37f);
                descriptionRect.anchorMax = new Vector2(descriptionRect.anchorMax.x, 0.475f);
                damageRowRect.anchorMin = new Vector2(damageRowRect.anchorMin.x, 0.30f);
                damageRowRect.anchorMax = new Vector2(damageRowRect.anchorMax.x, 0.355f);
                usesRowRect.anchorMin = new Vector2(usesRowRect.anchorMin.x, 0.30f);
                usesRowRect.anchorMax = new Vector2(usesRowRect.anchorMax.x, 0.355f);
            }
            else
            {
                nameRect.anchorMin = new Vector2(nameRect.anchorMin.x, 0.72f);
                nameRect.anchorMax = new Vector2(nameRect.anchorMax.x, 0.82f);
                descriptionRect.anchorMin = new Vector2(descriptionRect.anchorMin.x, 0.48f);
                descriptionRect.anchorMax = new Vector2(descriptionRect.anchorMax.x, 0.61f);
                damageRowRect.anchorMin = new Vector2(damageRowRect.anchorMin.x, 0.39f);
                damageRowRect.anchorMax = new Vector2(damageRowRect.anchorMax.x, 0.46f);
                usesRowRect.anchorMin = new Vector2(usesRowRect.anchorMin.x, 0.39f);
                usesRowRect.anchorMax = new Vector2(usesRowRect.anchorMax.x, 0.46f);
            }

            if (_detailsCloseRect != null)
            {
                _detailsCloseRect.anchoredPosition = new Vector2(
                    DetailsCloseFixedPosition.x, showsArt ? 340f : 250f);
            }
        }

        /// <summary>
        /// Keeps a weapon name on a single line whenever it can fit the name column at
        /// the smallest allowed font size, and only permits a wrap when it genuinely
        /// cannot - two-word names used to break onto a second line while there was
        /// still horizontal room. Auto-sizing then picks the largest size that fits.
        /// </summary>
        private void ApplyDetailsNameFit()
        {
            var text = _detailsNameText;
            var available = text.rectTransform.rect.width;
            if (available <= 0f)
            {
                text.textWrappingMode = TextWrappingModes.Normal;
                return;
            }

            text.enableAutoSizing = false;
            text.fontSize = text.fontSizeMin;
            var narrowestWidth = text.GetPreferredValues(text.text, 0f, 0f).x;
            text.textWrappingMode = narrowestWidth <= available
                ? TextWrappingModes.NoWrap
                : TextWrappingModes.Normal;
            text.enableAutoSizing = true;
        }

        /// <summary>
        /// Sizes the evolution art image to fit inside <see cref="DetailsArtMaxSize"/>
        /// while preserving the source sprite's aspect ratio - computed explicitly here
        /// (rather than relied upon via a stretch anchor + Image.preserveAspect) so wide
        /// or tall source art can never push the image past the card's edges.
        /// </summary>
        private static float ResolveDetailsArtScale(string weaponId, int rank, WeaponUpgradeBranch branch)
        {
            return DetailsArtBoosts.TryGetValue(weaponId, out var boost) && boost.Covers(rank, branch)
                ? boost.Scale
                : 1f;
        }

        private static void ApplyDetailsArtFit(Image artImage, float scale)
        {
            var sprite = artImage.sprite;
            if (sprite == null || sprite.rect.height <= 0f)
            {
                return;
            }

            var aspect = sprite.rect.width / sprite.rect.height;
            var boxAspect = DetailsArtMaxSize.x / DetailsArtMaxSize.y;
            var fittedSize = aspect >= boxAspect
                ? new Vector2(DetailsArtMaxSize.x, DetailsArtMaxSize.x / aspect)
                : new Vector2(DetailsArtMaxSize.y * aspect, DetailsArtMaxSize.y);
            artImage.rectTransform.sizeDelta = fittedSize * scale;
        }

        private void EnsureDetailsOverlay()
        {
            if (_detailsOverlay == null && _screenRoot != null)
            {
                BuildDetailsOverlay(_screenRoot);
            }
        }

        private void HideWeaponDetails()
        {
            if (_detailsOverlay != null)
            {
                _detailsOverlay.SetActive(false);
            }
        }

        private void BuyDetailsWithCoins()
        {
            if (_selectedWeaponIndex < 0 || _selectedWeaponIndex >= _items.Count)
            {
                return;
            }

            var weapon = _items[_selectedWeaponIndex];
            var upgrade = AppServices.WeaponUpgrades.TryUpgrade(weapon.WeaponId, _pendingBranch);
            if (!upgrade.Applied)
            {
                return;
            }

            AppServices.RefreshHistoricalPower();
            AppServices.SaveService.Save(AppServices.SaveService.Current);
            SelectWeapon(_selectedWeaponIndex);
            HideWeaponDetails();
        }

        private void BuyDetailsWithBlueprints()
        {
            BuyDetailsWithCoins();
        }

        private Image CreatePanel(string name, Component parent, Vector2 anchorMin, Vector2 anchorMax,
            Vector2 pivot, Vector2 sizeDelta, Vector2 anchoredPosition, Color color)
        {
            var rect = CreateRect(name, parent, anchorMin, anchorMax, pivot, sizeDelta, anchoredPosition);
            var image = rect.gameObject.AddComponent<Image>();
            image.sprite = _whiteSprite;
            image.color = color;
            image.raycastTarget = true;
            return image;
        }

        private void ChangePage(int direction)
        {
            var maxStart = Mathf.Max(0, _items.Count - WeaponsPerPage);
            if (maxStart <= 0)
            {
                return;
            }

            var next = direction > 0 ? _pageStart + WeaponsPerPage : _pageStart - WeaponsPerPage;
            _pageStart = Mathf.Clamp(next, 0, maxStart);
            ApplyPage();
        }

        private void ApplyPage()
        {
            RefreshCarouselSlots();

            var maxStart = Mathf.Max(0, _items.Count - WeaponsPerPage);
            if (_leftArrowButton != null)
            {
                _leftArrowButton.interactable = _pageStart > 0;
            }

            if (_rightArrowButton != null)
            {
                _rightArrowButton.interactable = _pageStart < maxStart;
            }
        }

        private void RefreshCarouselSlots()
        {
            for (var i = 0; i < _slots.Count; i++)
            {
                var slot = _slots[i];
                var itemIndex = _pageStart + slot.SlotIndex;
                var visible = itemIndex >= 0 && itemIndex < _items.Count;
                slot.Root.gameObject.SetActive(visible);
                if (!visible)
                {
                    continue;
                }

                var weapon = _items[itemIndex];
                var progress = AppServices.SaveService.Current.inventory.GetOrCreateWeaponProgress(weapon.WeaponId);
                slot.WeaponImage.sprite = LoadSprite(WeaponEvolutionIconResolver.ResolveCurrentIconResource(
                    weapon.IconId, progress.rank, (WeaponUpgradeBranch)progress.branch));
                slot.WeaponImage.color = Color.white;
                ApplySlotWeaponVisual(slot.Weapon, weapon.WeaponId);
                slot.Frame.color = NormalSlotColor;
                slot.Lock.enabled = false;
                slot.Label.text = $"LVL {progress.rank}";
                slot.Label.color = SciFiTextStyle.SecondaryText;

                var labelRect = slot.Label.rectTransform;
                labelRect.anchorMin = new Vector2(0.197f, 0.161f);
                labelRect.anchorMax = new Vector2(0.897f, 0.321f);
                labelRect.offsetMin = Vector2.zero;
                labelRect.offsetMax = Vector2.zero;
            }
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
            AddEvent(trigger, EventTriggerType.PointerDown, data =>
            {
                if (data is PointerEventData pointer)
                {
                    _carouselDragStart = pointer.position;
                }
            });
            AddEvent(trigger, EventTriggerType.BeginDrag, data =>
            {
                if (data is PointerEventData pointer)
                {
                    _carouselDragStart = pointer.position;
                }
            });
            AddEvent(trigger, EventTriggerType.EndDrag, data =>
            {
                if (data is not PointerEventData pointer)
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
            AddEvent(trigger, EventTriggerType.Scroll, data =>
            {
                if (data is PointerEventData pointer && Mathf.Abs(pointer.scrollDelta.y) > 0.01f)
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

        private static void ApplySlotWeaponVisual(RectTransform rect, string weaponId)
        {
            var scale = weaponId switch
            {
                "weapon.impulse" => new Vector2(1.10f, 1.10f),
                "weapon.blaster-caset" => new Vector2(0.84f, 0.84f),
                "weapon.bur" => new Vector2(0.78f, 0.78f),
                "weapon.mortira" => Vector2.one,
                "weapon.granata" => new Vector2(0.68f, 0.68f),
                "weapon.teleport" => new Vector2(0.48f, 0.48f),
                "weapon.mine" => new Vector2(0.78f, 0.78f),
                "weapon.heal" => new Vector2(0.82f, 0.82f),
                "weapon.emp" => new Vector2(0.82f, 0.72f),
                "weapon.fire-gun" => new Vector2(0.82f, 0.82f),
                "weapon.fpv-drone" => new Vector2(0.72f, 0.72f),
                _ => Vector2.one
            };
            rect.localScale = new Vector3(scale.x, scale.y, 1f);
            rect.anchoredPosition = new Vector2(0f, 12f);
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

        private RectTransform CreateTransparentButton(string name, Component parent, Vector2 anchorMin, Vector2 anchorMax,
            Vector2 pivot, Vector2 sizeDelta, Vector2 anchoredPosition, Action onClick)
        {
            var rect = CreateRect(name, parent, anchorMin, anchorMax, pivot, sizeDelta, anchoredPosition);
            var image = rect.gameObject.AddComponent<Image>();
            image.sprite = _whiteSprite;
            image.color = Color.clear;
            image.raycastTarget = true;

            var button = rect.gameObject.AddComponent<Button>();
            button.targetGraphic = image;
            button.transition = Selectable.Transition.None;
            button.onClick.AddListener(() => onClick?.Invoke());
            return rect;
        }

        private TMP_Text CreateText(string name, Component parent, Vector2 anchorMin, Vector2 anchorMax,
            Vector2 pivot, Vector2 sizeDelta, Vector2 anchoredPosition, int fontSize,
            TextAlignmentOptions alignment, string value, GameTextRole role)
        {
            var rect = CreateRect(name, parent, anchorMin, anchorMax, pivot, sizeDelta, anchoredPosition);
            var text = rect.gameObject.AddComponent<TextMeshProUGUI>();
            SciFiTextStyle.Apply(text, role, fontSize, alignment);
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

        private sealed class DetailsOverlayClickHandler : MonoBehaviour, IPointerClickHandler
        {
            private ArchitectScreenController _owner;

            public void Initialize(ArchitectScreenController owner)
            {
                _owner = owner;
            }

            public void OnPointerClick(PointerEventData eventData)
            {
                if (_owner == null || _owner._detailsCard == null)
                {
                    return;
                }

                if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(
                        _owner._detailsCard, eventData.position, eventData.pressEventCamera, out var localPoint))
                {
                    _owner.HideWeaponDetails();
                    return;
                }

                var cardRect = _owner._detailsCard.rect;
                var visibleHalfWidth = cardRect.width * 0.5f - DetailsCardVisualInset.x;
                var visibleHalfHeight = cardRect.height * 0.5f - DetailsCardVisualInset.y;
                if (Mathf.Abs(localPoint.x) > visibleHalfWidth || Mathf.Abs(localPoint.y) > visibleHalfHeight)
                {
                    _owner.HideWeaponDetails();
                }
            }
        }
    }
}
