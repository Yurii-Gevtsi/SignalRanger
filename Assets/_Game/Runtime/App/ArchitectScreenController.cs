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
        private const string WorkshopSelectedCardResource = "UI/Common/card_lighted";
        private const string WorkshopCarouselCardResource = "UI/Shop/card";
        private const string WorkshopLeftPanelResource = "UI/Common/rect_long_card";
        private const string WorkshopRightPanelResource = "UI/Common/rect_card_long";
        private static readonly Vector2 WorkshopLeftPanelPosition = new(-720f, 40f);
        private static readonly Vector2 WorkshopRightPanelPosition = new(780f, 40f);
        private static readonly Vector2 WorkshopLeftPanelSize = new(580f, 720f);
        private static readonly Vector2 WorkshopRightPanelSize = new(500f, 620f);
        private static readonly Vector2 WorkshopCardSize = new(250f, 300f);
        private static readonly float WorkshopCardsY = -75f;
        private static readonly float[] WorkshopCardPositionsX = { -390f, -180f, 20f, 220f, 420f };
        private static readonly Vector2 WorkshopIconSize = new(120f, 110f);
        private const string WorkshopStatIconsResource = "UI/Menu/Architect/icons";
        private static readonly Vector2 WorkshopStatIconSize = new(70f, 70f);
        private static readonly Vector2 WorkshopCurrentArtCenterPosition = new(-50f, 200f);
        private static readonly Color WorkshopBarBackground = new(0.10f, 0.19f, 0.28f, 0.95f);
        private static readonly Color WorkshopBarOuter = new Color32(0x12, 0x2F, 0x49, 0xFF);
        private static readonly Color WorkshopBarTrack = new Color32(0x09, 0x34, 0x5D, 0xFF);
        private static readonly Color WorkshopCurrentBarFill = new Color32(0x29, 0xD8, 0xFF, 0xFF);
        private static readonly Color WorkshopBarFill = new(0.22f, 0.86f, 0.38f, 1f);
        private static readonly Color WorkshopDisabledCardColor = new(0.22f, 0.25f, 0.29f, 1f);
        private static readonly Color WorkshopDisabledContentColor = new(0.58f, 0.62f, 0.68f, 1f);
        private static readonly Color WorkshopDividerColor = new Color32(0x8A, 0x95, 0xA2, 0xB8);
        private static readonly Vector2 WorkshopDividerSize = new(300f, 2f);
        private static readonly Vector2 WorkshopLeftDividerSize = new(340f, 2f);
        private const string WorkshopUpgradeGreenHex = "72FF86";
        // Artwork that carries enough empty margin to read far smaller than the rest once
        // aspect-fitted, so it gets a per-weapon boost on the detail card.
        private static readonly Dictionary<string, DetailsArtBoost> DetailsArtBoosts = new(StringComparer.Ordinal)
        {
            { "weapon.impulse", new DetailsArtBoost(1.2f, true, WeaponUpgradeBranch.None) },
            { "weapon.blaster-caset", new DetailsArtBoost(1.5f, true, WeaponUpgradeBranch.None) },
            { "weapon.mortira", new DetailsArtBoost(1.5f, true, WeaponUpgradeBranch.None) },
            { "weapon.granata", new DetailsArtBoost(1.2f, true, WeaponUpgradeBranch.None) },
            { "weapon.mine", new DetailsArtBoost(1f, true, WeaponUpgradeBranch.None) },
            { "weapon.heal", new DetailsArtBoost(1.5f, true, WeaponUpgradeBranch.None) },
            { "weapon.toxic", new DetailsArtBoost(1.5f, true, WeaponUpgradeBranch.None) },
            { "weapon.missle", new DetailsArtBoost(1.5f, true, WeaponUpgradeBranch.None) },
            { "weapon.bur", new DetailsArtBoost(1.3f, true, WeaponUpgradeBranch.None) },
            { "weapon.fire-gun", new DetailsArtBoost(1.3f, true, WeaponUpgradeBranch.None) },
            { "weapon.rail-rifle", new DetailsArtBoost(new Vector2(1.4f, 1.5f), true, WeaponUpgradeBranch.None) },
            // Only the upper branch's art needs the boost at ranks 5/10.
            { "weapon.freeze", new DetailsArtBoost(1.3f, true, WeaponUpgradeBranch.A) },
            // Base configuration only - the evolution art is already large enough.
            { "weapon.c4", new DetailsArtBoost(1.3f, false, WeaponUpgradeBranch.None) }
        };
        private const float PhoneElementScale = 1.2f;
        private const int WeaponsPerPage = 7;
        private const float CarouselSpacing = 200f;
        private const float CarouselStartCenter = -180f;
        private const float CarouselEdgeAllowance = 100f;
        private const int MinimumVisibleCarouselCards = 3;

        private readonly Dictionary<string, Sprite> _sprites = new(StringComparer.Ordinal);
        private readonly List<WeaponDefinition> _items = new();
        private readonly List<SlotView> _slots = new();
        private readonly List<EvolutionIconView> _evolutionIcons = new();
        private readonly List<ChevronIconView> _chevronIcons = new();
        private readonly List<WorkshopCardView> _workshopCards = new();
        private readonly Dictionary<WeaponUpgradeBranch, List<GameObject>> _branchObjects = new();
        private Sprite _whiteSprite;
        private Sprite _roundedBarSprite;
        private Sprite _workshopSegmentSprite;
        private Sprite _workshopDisabledMarkerSprite;
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
        private float _carouselScroll;
        private float _carouselDragStartScroll;
        private int _visibleCarouselCount = WeaponsPerPage;
        private bool _carouselDragMoved;
        private bool _suppressNextCarouselClick;
        private WeaponUpgradeBranch _pendingBranch;
        private WeaponUpgradePath _pendingPath;
        private string _pendingEvolutionOptionId;
        private GameObject _evolutionChoiceRoot;
        private int _pendingUpgradeRank;
        private TMP_Text _workshopCurrentName;
        private TMP_Text _workshopCurrentDescription;
        private ScrollRect _workshopCurrentDescriptionScroll;
        private Image _workshopCurrentArt;
        private WorkshopStatRow[] _workshopCurrentStats;
        private Image _workshopNextIcon;
        private TMP_Text _workshopNextName;
        private TMP_Text _workshopNextLevel;
        private TMP_Text _workshopNextDescription;
        private ScrollRect _workshopNextDescriptionScroll;
        private Image _workshopNextDescriptionTopDivider;
        private Image _workshopNextDescriptionBottomDivider;
        private Image _workshopNextRequiredChipsTopDivider;
        private Image _workshopNextCostTopDivider;
        private Image _workshopNextStatIcon;
        private TMP_Text _workshopNextStatLabel;
        private TMP_Text _workshopNextStatValue;
        private TMP_Text _workshopNextChips;
        private TMP_Text _workshopNextRequiredChipsLabel;
        private TMP_Text _workshopNextCostLabel;
        private TMP_Text _workshopNextMaxLevelText;
        private Image _workshopNextChipIcon;
        private Image _workshopNextCoinIcon;
        private Image _workshopNextCrystalIcon;
        private TMP_Text _workshopNextCoinAmount;
        private TMP_Text _workshopNextCrystalAmount;
        private TMP_Text _workshopNextOrText;
        private Button _workshopNextButton;
        private ResourceInfoPopupController _resourceInfoPopup;
        private int _selectedWorkshopCardIndex;

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
                : this(new Vector2(scale, scale), includesEvolutions, evolutionBranch)
            {
            }

            public DetailsArtBoost(Vector2 scale, bool includesEvolutions, WeaponUpgradeBranch evolutionBranch)
            {
                Scale = scale;
                IncludesEvolutions = includesEvolutions;
                EvolutionBranch = evolutionBranch;
            }

            public Vector2 Scale { get; }
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

        private sealed class WorkshopCardView
        {
            public WeaponUpgradePath Path;
            public Image Frame;
            public Image Icon;
            public TMP_Text Title;
            public Button CardButton;
            public Outline SelectionOutline;
            public Image Lock;
            public Image DisabledMarker;
            public Image[] ProgressBars;
        }

        private sealed class WorkshopStatRow
        {
            public RectTransform Row;
            public Image Icon;
            public TMP_Text Label;
            public TMP_Text Value;
            public Image Outer;
            public Image Bar;
            public Image Fill;
        }

        private enum WorkshopParameter
        {
            None = 0,
            Core,
            Aim,
            Loadout,
            EffectScale
        }

        private readonly struct WorkshopCardDefinition
        {
            public WorkshopCardDefinition(WeaponUpgradePath path, string iconResource, string titleKey,
                string descriptionKey, WorkshopParameter parameter, int requiredRank = 1, bool isSpecial = false)
            {
                Path = path;
                IconResource = iconResource;
                TitleKey = titleKey;
                DescriptionKey = descriptionKey;
                Parameter = parameter;
                RequiredRank = requiredRank;
                IsSpecial = isSpecial;
            }

            public WeaponUpgradePath Path { get; }
            public string IconResource { get; }
            public string TitleKey { get; }
            public string DescriptionKey { get; }
            public WorkshopParameter Parameter { get; }
            public int RequiredRank { get; }
            public bool IsSpecial { get; }
        }

        private static readonly WorkshopCardDefinition[] WorkshopCardDefinitions =
        {
            new(WeaponUpgradePath.Core, "UI/Menu/Architect/core", "architect.card.core",
                "architect.detail.coreDescription", WorkshopParameter.Core),
            new(WeaponUpgradePath.Aim, "UI/Menu/Architect/aim", "architect.card.aim",
                "architect.detail.aimDescription", WorkshopParameter.Aim),
            new(WeaponUpgradePath.Loadout, "UI/Menu/Architect/loadout", "architect.card.loadout",
                "architect.detail.loadoutDescription", WorkshopParameter.Loadout),
            new(WeaponUpgradePath.Radius, "UI/Menu/Architect/radius", "architect.card.radius",
                "architect.detail.effectScaleDescription", WorkshopParameter.EffectScale),
            new(WeaponUpgradePath.None, "UI/Menu/Architect/special_5", "architect.card.special",
                string.Empty, WorkshopParameter.None, 5, true),
        };

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
            _roundedBarSprite = CreateRoundedRectSprite();
            _workshopSegmentSprite = CreateRoundedRectSprite(5f);
            _workshopDisabledMarkerSprite = CreateDisabledWorkshopMarkerSprite();
            BuildItems();
            Build();
        }

        private void BuildItems()
        {
            foreach (var weapon in PrototypeWeaponLibrary.CreateAll())
            {
                if (weapon == null
                    || !AppServices.Inventory.IsWeaponUnlocked(weapon.WeaponId)
                    || !WeaponProgressionCatalog.TryGet(weapon.WeaponId, out var progression)
                    || !WeaponCatalogAvailability.IsVisibleInWorkshop(weapon.WeaponId, progression)
                    || WeaponPresentationText.IsDropOnly(weapon))
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
            MenuScreenInputShield.Create(root, "Architect Input Shield");
            _resourceInfoPopup = ResourceInfoPopupController.Create(root);
            _blueprintBackgroundImage = CreateImage("Architect Blueprint", root, Vector2.zero, Vector2.one,
                new Vector2(0.5f, 0.5f), Vector2.zero, Vector2.zero,
                MenuResourceRoot + "arch_menu", false, Color.white).GetComponent<Image>();

            BuildTopBar(root);
            BuildWorkshopLayout(root);
            BuildWeaponCarousel(root);
        }

        private void BuildWorkshopLayout(Transform root)
        {
            BuildWorkshopSidePanels(root);

            _workshopCurrentArt = CreateImage("Architect Current Weapon Art", root,
                new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                new Vector2(360f, 220f), WorkshopCurrentArtCenterPosition, string.Empty, true, Color.white)
                .GetComponent<Image>();
            _workshopCurrentArt.rectTransform.localScale = new Vector3(1.2f, 1.2f, 1f);

            for (var i = 0; i < WorkshopCardDefinitions.Length; i++)
            {
                var definition = WorkshopCardDefinitions[i];
                var card = CreateImage($"Architect Upgrade Card {i}", root,
                    new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                    WorkshopCardSize,
                    new Vector2(WorkshopCardPositionsX[i], WorkshopCardsY),
                    WorkshopCarouselCardResource, false, Color.white);
                var frame = card.GetComponent<Image>();
                frame.raycastTarget = true;

                var cardButton = card.gameObject.AddComponent<Button>();
                cardButton.targetGraphic = frame;
                cardButton.transition = Selectable.Transition.ColorTint;
                var cardButtonColors = CreateButtonColors();
                cardButtonColors.disabledColor = Color.white;
                cardButton.colors = cardButtonColors;
                var cardIndex = i;
                cardButton.onClick.AddListener(() => ShowWorkshopCard(cardIndex));

                var selectionOutline = card.gameObject.AddComponent<Outline>();
                selectionOutline.effectColor = new Color(0.12f, 1f, 0.40f, 0.95f);
                selectionOutline.effectDistance = new Vector2(4f, 4f);
                selectionOutline.useGraphicAlpha = true;
                selectionOutline.enabled = false;

                var icon = CreateImage($"Architect Upgrade Icon {i}", card, new Vector2(0.5f, 0.5f),
                    new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), WorkshopIconSize,
                    new Vector2(0f, 12f), definition.IconResource, true, Color.white).GetComponent<Image>();
                icon.raycastTarget = false;

                var title = CreateText($"Architect Upgrade Title {i}", card, new Vector2(0.08f, 0.70f),
                    new Vector2(0.92f, 0.82f), new Vector2(0.5f, 0.5f), Vector2.zero, Vector2.zero,
                    24, TextAlignmentOptions.Center, string.Empty, GameTextRole.DisplayTitle);
                title.enableAutoSizing = true;
                title.fontSizeMin = 16f;
                title.fontSizeMax = 24f;

                var bars = new Image[4];
                for (var barIndex = 0; barIndex < bars.Length; barIndex++)
                {
                    var bar = CreatePanel($"Architect Upgrade Progress {i} {barIndex}", card,
                        new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                        new Vector2(34f, 14f), new Vector2(-60f + barIndex * 40f, -70f),
                        WorkshopBarBackground);
                    bar.sprite = _workshopSegmentSprite;
                    bar.type = Image.Type.Sliced;
                    bar.fillCenter = true;
                    bar.raycastTarget = false;
                    bars[barIndex] = bar;
                }

                var lockImage = CreateImage($"Architect Upgrade Lock {i}", card,
                    new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                    new Vector2(62f, 62f), new Vector2(0f, -70f), ShopResourceRoot + "lock", true,
                    new Color(0.72f, 0.78f, 0.84f, 1f)).GetComponent<Image>();
                lockImage.gameObject.SetActive(false);

                var disabledMarker = CreateImage($"Architect Upgrade Disabled Marker {i}", card,
                    new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                    new Vector2(58f, 58f), new Vector2(0f, -70f), string.Empty, true,
                    WorkshopDisabledContentColor).GetComponent<Image>();
                disabledMarker.sprite = _workshopDisabledMarkerSprite;
                disabledMarker.raycastTarget = false;
                disabledMarker.gameObject.SetActive(false);

                _workshopCards.Add(new WorkshopCardView
                {
                    Path = definition.Path,
                    Frame = frame,
                    Icon = icon,
                    Title = title,
                    CardButton = cardButton,
                    SelectionOutline = selectionOutline,
                    Lock = lockImage,
                    DisabledMarker = disabledMarker,
                    ProgressBars = bars
                });
            }
        }

        private void BuildWorkshopSidePanels(Transform root)
        {
            var leftPanel = CreateRect("Architect Current Weapon Panel", root,
                new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                WorkshopLeftPanelSize, WorkshopLeftPanelPosition);
            var leftFrame = CreateImage("Architect Current Weapon Frame", leftPanel, new Vector2(0.5f, 0.5f),
                new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(560f, 720f), Vector2.zero,
                WorkshopLeftPanelResource, false, Color.white);
            CreateWorkshopDivider(leftPanel, "Architect Current Divider Above Description", new Vector2(0f, 220f), WorkshopLeftDividerSize);
            CreateWorkshopDivider(leftPanel, "Architect Current Divider Below Description", new Vector2(0f, 20f), WorkshopLeftDividerSize);

            _workshopCurrentName = CreateText("Architect Current Weapon Name", leftPanel,
                new Vector2(0.20f, 0.80f), new Vector2(0.90f, 0.89f), new Vector2(0f, 0.5f),
                Vector2.zero, Vector2.zero, 30, TextAlignmentOptions.MidlineLeft, string.Empty, GameTextRole.DisplayTitle);
            _workshopCurrentName.enableAutoSizing = true;
            _workshopCurrentName.fontSizeMin = 22f;
            _workshopCurrentName.fontSizeMax = 30f;
            _workshopCurrentDescription = CreateWorkshopDescriptionScroll("Architect Current Weapon Description",
                leftPanel, new Vector2(0.20f, 0.53f), new Vector2(0.80f, 0.805f), 22,
                out _workshopCurrentDescriptionScroll);
            _workshopCurrentDescription.enableAutoSizing = false;
            _workshopCurrentDescription.fontSizeMin = 20f;
            _workshopCurrentDescription.fontSizeMax = 22f;
            _workshopCurrentDescription.textWrappingMode = TextWrappingModes.Normal;
            _workshopCurrentDescription.overflowMode = TextOverflowModes.Truncate;
            _workshopCurrentStats = new[]
            {
                CreateWorkshopStatRow(leftPanel, "Architect Current Damage", "architect.detail.statDamage", 0.45f, 0),
                CreateWorkshopStatRow(leftPanel, "Architect Current Accuracy", "architect.detail.statAccuracy", 0.36f, 1),
                CreateWorkshopStatRow(leftPanel, "Architect Current Ammo", "architect.detail.statAmmo", 0.27f, 2),
                CreateWorkshopStatRow(leftPanel, "Architect Current Radius", "architect.detail.statRadius", 0.18f, 3)
            };

            var rightPanel = CreateRect("Architect Next Upgrade Panel", root,
                new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                WorkshopRightPanelSize, WorkshopRightPanelPosition);
            var rightFrame = CreateImage("Architect Next Upgrade Frame", rightPanel,
                new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                new Vector2(620f, 500f), Vector2.zero, WorkshopRightPanelResource, false, Color.white);
            rightFrame.localEulerAngles = new Vector3(0f, 0f, 90f);
            _workshopNextDescriptionTopDivider = CreateWorkshopDivider(rightPanel,
                "Architect Divider Above Description", new Vector2(-30f, 160f));
            _workshopNextDescriptionBottomDivider = CreateWorkshopDivider(rightPanel,
                "Architect Divider Below Description", new Vector2(-30f, 55f));
            _workshopNextRequiredChipsTopDivider = CreateWorkshopDivider(rightPanel,
                "Architect Divider Above Required Chips", new Vector2(-30f, 10f));
            _workshopNextCostTopDivider = CreateWorkshopDivider(rightPanel,
                "Architect Divider Above Cost", new Vector2(-30f, -105f));
            _workshopNextIcon = CreateImage("Architect Selected Upgrade Icon", rightPanel,
                new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                new Vector2(132f, 120f), new Vector2(80f, 220f), string.Empty, true, Color.white).GetComponent<Image>();
            _workshopNextName = CreateText("Architect Next Upgrade Name", rightPanel,
                new Vector2(0.12f, 0.82f), new Vector2(0.56f, 0.91f), new Vector2(0.5f, 0.5f),
                Vector2.zero, new Vector2(20f, 10f), 32, TextAlignmentOptions.MidlineLeft, string.Empty, GameTextRole.DisplayTitle);
            _workshopNextName.enableAutoSizing = true;
            _workshopNextName.fontSizeMin = 24f;
            _workshopNextName.fontSizeMax = 34f;
            _workshopNextLevel = CreateText("Architect Next Upgrade Level", rightPanel,
                new Vector2(0.12f, 0.75f), new Vector2(0.84f, 0.81f), new Vector2(0.5f, 0.5f),
                Vector2.zero, new Vector2(20f, 10f), 18, TextAlignmentOptions.MidlineLeft, string.Empty, GameTextRole.Caption);
            _workshopNextLevel.enableAutoSizing = true;
            _workshopNextLevel.fontSizeMin = 14f;
            _workshopNextLevel.fontSizeMax = 18f;
            _workshopNextLevel.richText = true;
            _workshopNextDescription = CreateWorkshopDescriptionScroll("Architect Next Upgrade Description",
                rightPanel, new Vector2(0.18f, 0.55f), new Vector2(0.82f, 0.69f), 22,
                out _workshopNextDescriptionScroll, new Vector2(-20f, 30f));
            _workshopNextDescription.enableAutoSizing = true;
            _workshopNextDescription.fontSizeMin = 19f;
            _workshopNextDescription.fontSizeMax = 22f;
            _workshopNextDescription.textWrappingMode = TextWrappingModes.Normal;
            _workshopNextDescription.overflowMode = TextOverflowModes.Truncate;
            _workshopNextStatIcon = CreateImage("Architect Selected Upgrade Stat Icon", rightPanel,
                new Vector2(0.16f, 0.46f), new Vector2(0.16f, 0.46f), new Vector2(0.5f, 0.5f),
                new Vector2(70f, 70f), new Vector2(0f, 10f), string.Empty, true, Color.white).GetComponent<Image>();
            _workshopNextStatIcon.gameObject.SetActive(false);
            _workshopNextStatLabel = CreateText("Architect Selected Upgrade Stat Label", rightPanel,
                new Vector2(0.24f, 0.43f), new Vector2(0.46f, 0.49f), new Vector2(0.5f, 0.5f),
                Vector2.zero, new Vector2(0f, 60f), 21, TextAlignmentOptions.MidlineLeft, string.Empty, GameTextRole.Body);
            _workshopNextStatValue = CreateText("Architect Selected Upgrade Stat Value", rightPanel,
                new Vector2(0.44f, 0.43f), new Vector2(0.72f, 0.49f), new Vector2(0.5f, 0.5f),
                Vector2.zero, new Vector2(0f, 60f), 23, TextAlignmentOptions.MidlineRight, string.Empty, GameTextRole.Numeric);
            _workshopNextStatValue.richText = true;
            _workshopNextRequiredChipsLabel = CreateText("Architect Required Chips Label", rightPanel,
                new Vector2(0.10f, 0.50f), new Vector2(0.45f, 0.50f), new Vector2(0f, 0.5f),
                new Vector2(0f, 40f), new Vector2(-20f, -70f), 20, TextAlignmentOptions.MidlineLeft,
                GameText.Localized("architect.requiredChips", string.Empty), GameTextRole.Body);
            _workshopNextRequiredChipsLabel.enableAutoSizing = true;
            _workshopNextRequiredChipsLabel.fontSizeMin = 16f;
            _workshopNextRequiredChipsLabel.fontSizeMax = 20f;
            _workshopNextChipIcon = CreateImage("Architect Required Chips Icon", rightPanel,
                new Vector2(0.88f, 0.5f), new Vector2(0.88f, 0.5f), new Vector2(0.5f, 0.5f),
                new Vector2(70f, 70f), new Vector2(0f, -70f), "UI/Common/chip_2", true, Color.white).GetComponent<Image>();
            _resourceInfoPopup.BindResourceIcon(_workshopNextChipIcon, ResourceInfoKind.Chips);
            _workshopNextChips = CreateText("Architect Required Chips Value", rightPanel,
                new Vector2(0.45f, 0.50f), new Vector2(0.75f, 0.50f), new Vector2(0f, 0.5f),
                new Vector2(0f, 40f), new Vector2(-20f, -70f), 23, TextAlignmentOptions.MidlineLeft, string.Empty, GameTextRole.Numeric);
            _workshopNextCostLabel = CreateText("Architect Upgrade Cost Label", rightPanel,
                new Vector2(0.10f, 0.20f), new Vector2(0.90f, 0.25f), new Vector2(0.5f, 0.5f),
                Vector2.zero, new Vector2(-20f, 50f), 20, TextAlignmentOptions.Center,
                GameText.Localized("architect.cost", string.Empty), GameTextRole.Body);
            _workshopNextCoinIcon = CreateImage("Architect Upgrade Coin Icon", rightPanel,
                new Vector2(0.25f, 0.5f), new Vector2(0.25f, 0.5f), new Vector2(0.5f, 0.5f),
                new Vector2(70f, 70f), new Vector2(0f, -180f), ShopResourceRoot + "coin", true, Color.white).GetComponent<Image>();
            _resourceInfoPopup.BindResourceIcon(_workshopNextCoinIcon, ResourceInfoKind.Coins);
            _workshopNextCoinAmount = CreateText("Architect Upgrade Coin Amount", rightPanel,
                new Vector2(0.29f, 0.13f), new Vector2(0.43f, 0.19f), new Vector2(0.5f, 0.5f),
                Vector2.zero, new Vector2(20f, 30f), 21, TextAlignmentOptions.MidlineLeft, string.Empty, GameTextRole.CurrencyGold);
            _workshopNextOrText = CreateText("Architect Upgrade Or", rightPanel,
                new Vector2(0.44f, 0.13f), new Vector2(0.56f, 0.19f), new Vector2(0.5f, 0.5f),
                Vector2.zero, new Vector2(10f, 30f), 19, TextAlignmentOptions.Center,
                GameText.Localized("architect.or", string.Empty), GameTextRole.Caption);
            _workshopNextCrystalIcon = CreateImage("Architect Upgrade Crystal Icon", rightPanel,
                new Vector2(0.61f, 0.5f), new Vector2(0.61f, 0.5f), new Vector2(0.5f, 0.5f),
                new Vector2(70f, 70f), new Vector2(0f, -180f), ShopResourceRoot + "vip_coin", true, Color.white).GetComponent<Image>();
            _resourceInfoPopup.BindResourceIcon(_workshopNextCrystalIcon, ResourceInfoKind.Crystals);
            _workshopNextCrystalAmount = CreateText("Architect Upgrade Crystal Amount", rightPanel,
                new Vector2(0.67f, 0.13f), new Vector2(0.90f, 0.19f), new Vector2(0.5f, 0.5f),
                Vector2.zero, new Vector2(0f, 30f), 21, TextAlignmentOptions.MidlineLeft, string.Empty, GameTextRole.CurrencyCrystal);
            _workshopNextMaxLevelText = CreateText("Architect Upgrade Max Level", rightPanel,
                new Vector2(0.22f, 0.08f), new Vector2(0.78f, 0.19f), new Vector2(0.5f, 0.5f),
                Vector2.zero, new Vector2(-20f, 30f), 22, TextAlignmentOptions.Center,
                string.Empty, GameTextRole.Body);
            _workshopNextMaxLevelText.enableAutoSizing = true;
            _workshopNextMaxLevelText.fontSizeMin = 16f;
            _workshopNextMaxLevelText.fontSizeMax = 22f;
            _workshopNextMaxLevelText.textWrappingMode = TextWrappingModes.Normal;
            _workshopNextMaxLevelText.gameObject.SetActive(false);
            var nextButton = CreateImageButton("Architect Next Upgrade Action", rightPanel,
                new Vector2(0.5f, 0f), new Vector2(0.5f, 0f), new Vector2(0.5f, 0.5f),
                new Vector2(300f, 120f), new Vector2(-25f, 65f), ShopResourceRoot + "btn_green",
                ShowNextWorkshopDetails);
            nextButton.GetComponent<Image>().preserveAspect = false;
            _workshopNextButton = nextButton.GetComponent<Button>();
            var nextLabel = CreateText("Architect Next Upgrade Action Label", nextButton,
                Vector2.zero, Vector2.one, new Vector2(0.5f, 0.5f), Vector2.zero, Vector2.zero,
                25, TextAlignmentOptions.Center, GameText.Localized("architect.upgradeButton", string.Empty),
                GameTextRole.Button);
            nextLabel.raycastTarget = false;
        }

        private WorkshopStatRow CreateWorkshopStatRow(Component parent, string name, string labelKey, float y,
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
                new Vector2(0.5f, 0.5f), WorkshopStatIconSize, new Vector2(iconPositionX, 0f), string.Empty, true,
                Color.white).GetComponent<Image>();
            icon.sprite = LoadWorkshopStatIcon(iconIndex);
            icon.raycastTarget = false;
            var label = CreateText($"{name} Label", row, new Vector2(0.10f, 0f), new Vector2(0.10f, 1f),
                new Vector2(0f, 0.5f), new Vector2(220f, 0f), new Vector2(90f, 0f), 20, TextAlignmentOptions.MidlineLeft,
                GameText.Localized(labelKey, string.Empty), GameTextRole.Body);
            var outer = CreatePanel($"{name} Bar Outer", row, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                new Vector2(0.5f, 0.5f), new Vector2(160f, 17f), new Vector2(10f, 0f), WorkshopBarOuter);
            outer.sprite = _roundedBarSprite;
            outer.type = Image.Type.Sliced;
            outer.fillCenter = true;
            outer.raycastTarget = false;
            var bar = CreatePanel($"{name} Bar", row, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                new Vector2(0.5f, 0.5f), new Vector2(156f, 13f), new Vector2(10f, 0f), WorkshopBarTrack);
            bar.sprite = _roundedBarSprite;
            bar.type = Image.Type.Sliced;
            bar.fillCenter = true;
            bar.raycastTarget = false;
            var fill = CreatePanel($"{name} Bar Fill", row, new Vector2(0.5f, 0.5f),
                new Vector2(0.5f, 0.5f), new Vector2(0f, 0.5f), new Vector2(0f, 13f),
                new Vector2(-68f, 0f), WorkshopCurrentBarFill);
            fill.sprite = _roundedBarSprite;
            fill.type = Image.Type.Sliced;
            fill.fillCenter = true;
            fill.gameObject.SetActive(false);
            fill.raycastTarget = false;
            var value = CreateText($"{name} Value", row, new Vector2(0.72f, 0f), new Vector2(0.90f, 1f),
                new Vector2(1f, 0.5f), Vector2.zero, new Vector2(-40f, 0f), 25, TextAlignmentOptions.MidlineRight,
                string.Empty, GameTextRole.Numeric);
            return new WorkshopStatRow
            {
                Row = row,
                Icon = icon,
                Label = label,
                Value = value,
                Outer = outer,
                Bar = bar,
                Fill = fill
            };
        }

        private void ShowWorkshopCard(int cardIndex)
        {
            if (_selectedWeaponIndex < 0 || _selectedWeaponIndex >= _items.Count
                || cardIndex < 0 || cardIndex >= WorkshopCardDefinitions.Length)
            {
                return;
            }

            var card = WorkshopCardDefinitions[cardIndex];
            var weapon = _items[_selectedWeaponIndex];
            if (!WeaponProgressionCatalog.TryGet(weapon.WeaponId, out var progression) || progression == null)
            {
                return;
            }

            var progress = AppServices.SaveService.Current.inventory.GetOrCreateWeaponProgress(weapon.WeaponId);
            if (!card.IsSpecial && !IsWorkshopCardVisible(card, progression, progress))
            {
                _resourceInfoPopup.ShowMessage(
                    GameText.Localized(card.TitleKey, string.Empty),
                    GameText.Localized("architect.statUnavailable", string.Empty));
                return;
            }

            if (card.IsSpecial)
            {
                if (progression != null)
                {
                    var specialRank = ResolveWorkshopSpecialRank(progression, progress);
                    var options = progression.GetEvolutionOptions(specialRank);
                    if (options.Count > 1 && progress.rank >= specialRank
                        && !TryResolveWorkshopSpecialOption(progression, progress, specialRank, out _))
                    {
                        // The modal is a transient choice. Keep the previously selected
                        // card visible until the player actually chooses an evolution.
                        ShowEvolutionChoiceModal(weapon, specialRank, options);
                        return;
                    }
                }
            }

            _selectedWorkshopCardIndex = cardIndex;
            RefreshWorkshopCards();
            RefreshWorkshopSidePanels();
            RefreshWorkshopDescriptionScrolls();
        }

        private void ShowNextWorkshopDetails()
        {
            ShowUpgradeConfirmation();
        }

        private void RefreshWorkshopCards()
        {
            if (_workshopCards.Count == 0 || _selectedWeaponIndex < 0 || _selectedWeaponIndex >= _items.Count)
            {
                return;
            }

            var weapon = _items[_selectedWeaponIndex];
            if (!WeaponProgressionCatalog.TryGet(weapon.WeaponId, out var definition) || definition == null)
            {
                return;
            }

            var progress = AppServices.SaveService.Current.inventory.GetOrCreateWeaponProgress(weapon.WeaponId);
            EnsureSelectedWorkshopCardIsVisible(definition, progress);
            for (var i = 0; i < _workshopCards.Count; i++)
            {
                var view = _workshopCards[i];
                var cardDefinition = WorkshopCardDefinitions[i];
                view.Path = cardDefinition.Path;
                view.Title.text = GameText.Localized(cardDefinition.TitleKey, string.Empty);
                var isSupported = IsWorkshopCardVisible(cardDefinition, definition, progress);
                var shouldDisplay = !cardDefinition.IsSpecial || isSupported;
                view.Frame.gameObject.SetActive(shouldDisplay);
                if (!shouldDisplay)
                {
                    continue;
                }
                var currentSpecialRank = cardDefinition.IsSpecial ? ResolveWorkshopSpecialRank(definition, progress) : 0;
                var specialState = cardDefinition.IsSpecial
                    ? WorkshopSpecialCardStateResolver.Resolve(progress.rank, currentSpecialRank,
                        HasPurchasedWorkshopSpecial(definition, progress))
                    : default;
                var isAvailable = cardDefinition.IsSpecial ? specialState.IsAvailable : isSupported;
                var specialRank = cardDefinition.IsSpecial ? currentSpecialRank : 0;
                if (cardDefinition.IsSpecial && specialRank == 0)
                {
                    specialRank = definition.GetEvolutionOptions(10).Count > 0 ? 10 : 5;
                }
                var specialPurchaseLimit = cardDefinition.IsSpecial
                    ? definition.GetSpecialPurchaseLimit(specialRank)
                    : 0;
                var presentation = WorkshopCardPresentationStateResolver.Resolve(
                    cardDefinition.IsSpecial, isAvailable, specialPurchaseLimit);
                var isDisabled = !isAvailable;
                var selected = _selectedWorkshopCardIndex == i && !isDisabled;
                view.Frame.sprite = selected
                    ? LoadSprite(WorkshopSelectedCardResource)
                    : LoadSprite(WorkshopCarouselCardResource);
                view.Frame.color = isDisabled ? WorkshopDisabledCardColor : Color.white;
                view.Icon.color = isDisabled ? WorkshopDisabledContentColor : Color.white;
                view.Title.color = isDisabled ? WorkshopDisabledContentColor : SciFiTextStyle.GoldText;
                view.Lock.gameObject.SetActive(false);
                view.DisabledMarker.gameObject.SetActive(isDisabled);
                view.CardButton.interactable = presentation.IsInteractable;
                view.CardButton.transition = isDisabled
                    ? Selectable.Transition.None
                    : Selectable.Transition.ColorTint;
                view.SelectionOutline.enabled = false;
                view.CardButton.transform.localScale = selected
                    ? new Vector3(1.03f, 1.03f, 1f)
                    : Vector3.one;
                for (var barIndex = 0; barIndex < view.ProgressBars.Length; barIndex++)
                {
                    if (!presentation.ShowsProgressScale)
                    {
                        view.ProgressBars[barIndex].gameObject.SetActive(false);
                        continue;
                    }

                    if (cardDefinition.IsSpecial)
                    {
                        var purchasedSteps = definition.GetSpecialPurchaseCount(progress, specialRank);
                        var exists = barIndex < specialPurchaseLimit;
                        view.ProgressBars[barIndex].gameObject.SetActive(exists);
                        view.ProgressBars[barIndex].color = barIndex < purchasedSteps
                            ? WorkshopBarFill
                            : WorkshopBarBackground;
                    }
                    else
                    {
                        view.ProgressBars[barIndex].gameObject.SetActive(true);
                        view.ProgressBars[barIndex].color = barIndex < Mathf.Clamp(
                            WeaponUpgradeChoiceResolver.Count(progress, cardDefinition.Path), 0, 4)
                            ? WorkshopBarFill : WorkshopBarBackground;
                    }
                }
            }
        }

        private static bool IsWorkshopCardVisible(WorkshopCardDefinition card,
            WeaponProgressionDefinition definition, SaveWeaponProgressData progress)
        {
            if (card.IsSpecial)
            {
                return definition.HasEvolutionOptions;
            }

            return WorkshopBaseStatCardStateResolver.Resolve(
                WeaponUpgradeChoiceResolver.Count(progress, card.Path),
                definition.GetBaseUpgradeLimit(card.Path)).IsSupported;
        }

        private void EnsureSelectedWorkshopCardIsVisible(WeaponProgressionDefinition definition,
            SaveWeaponProgressData progress)
        {
            var selectedIndex = Mathf.Clamp(_selectedWorkshopCardIndex, 0, WorkshopCardDefinitions.Length - 1);
            if (IsWorkshopCardVisible(WorkshopCardDefinitions[selectedIndex], definition, progress))
            {
                _selectedWorkshopCardIndex = selectedIndex;
                return;
            }

            for (var i = 0; i < WorkshopCardDefinitions.Length; i++)
            {
                if (IsWorkshopCardVisible(WorkshopCardDefinitions[i], definition, progress))
                {
                    _selectedWorkshopCardIndex = i;
                    return;
                }
            }
        }

        private void RefreshWorkshopSidePanels()
        {
            if (_selectedWeaponIndex < 0 || _selectedWeaponIndex >= _items.Count)
            {
                return;
            }

            var weapon = _items[_selectedWeaponIndex];
            if (!WeaponProgressionCatalog.TryGet(weapon.WeaponId, out var definition) || definition == null)
            {
                return;
            }

            var progress = AppServices.SaveService.Current.inventory.GetOrCreateWeaponProgress(weapon.WeaponId);
            var currentBranch = (WeaponUpgradeBranch)progress.branch;
            _workshopCurrentName.text = WeaponDisplayNames.Get(weapon.WeaponId);
            _workshopCurrentDescription.text = WeaponUpgradePresentation.GetCurrentDescription(weapon, definition, progress);
            var currentArtResource = WeaponEvolutionIconResolver.ResolveCurrentIconResource(
                weapon.IconId, definition, progress);
            _workshopCurrentArt.sprite = LoadSprite(currentArtResource);
            _workshopCurrentArt.enabled = _workshopCurrentArt.sprite != null;
            ApplyWorkshopArtFit(_workshopCurrentArt);
            WeaponIconVisualTuning.ApplyWorkshopCurrentArt(
                _workshopCurrentArt.rectTransform, weapon.WeaponId, progress.rank);
            RefreshCurrentWorkshopStats(weapon, definition, progress, currentBranch);

            var selectedIndex = Mathf.Clamp(_selectedWorkshopCardIndex, 0, WorkshopCardDefinitions.Length - 1);
            var selectedCard = WorkshopCardDefinitions[selectedIndex];
            ApplyWorkshopNextPanelLayout(selectedCard.IsSpecial);
            WeaponIconVisualTuning.ApplyWorkshopSelectedUpgradeArt(
                _workshopNextIcon.rectTransform, weapon.WeaponId, selectedCard.IsSpecial,
                _workshopNextIcon.rectTransform.anchoredPosition);
            var baseCardState = !selectedCard.IsSpecial
                ? WorkshopBaseStatCardStateResolver.Resolve(
                    WeaponUpgradeChoiceResolver.Count(progress, selectedCard.Path),
                    definition.GetBaseUpgradeLimit(selectedCard.Path))
                : default;
            var isBasePathMaxed = !selectedCard.IsSpecial && baseCardState.IsAtMaximum;
            _workshopNextLevel.gameObject.SetActive(!selectedCard.IsSpecial);
            if (_workshopNextDescriptionBottomDivider != null)
            {
                _workshopNextDescriptionBottomDivider.gameObject.SetActive(!selectedCard.IsSpecial);
            }
            var nextWeaponRank = progress.rank < definition.MaxRank ? progress.rank + 1 : progress.rank;
            _workshopNextIcon.sprite = LoadSprite(selectedCard.IconResource);
            _workshopNextIcon.enabled = true;
            _workshopNextName.text = selectedCard.Parameter == WorkshopParameter.EffectScale
                ? GameText.Localized("architect.card.radius", string.Empty)
                : selectedCard.Parameter == WorkshopParameter.Loadout
                    ? GameText.Localized("architect.detail.loadoutTitle", string.Empty)
                : GameText.Localized(selectedCard.TitleKey, string.Empty);
            _workshopNextLevel.text = selectedCard.IsSpecial
                ? progress.rank < selectedCard.RequiredRank
                    ? ResolveWorkshopLevelText(progress.rank, selectedCard.RequiredRank)
                    : GameText.LevelLabel() + " " + progress.rank
                : ResolveWorkshopLevelText(baseCardState.CurrentLevel, baseCardState.NextLevel,
                    !baseCardState.IsAtMaximum);
            _workshopNextDescription.text = selectedCard.IsSpecial
                ? ResolveWorkshopSpecialDescription(definition, progress)
                : GameText.Localized(
                    WorkshopBaseStatDescriptionKeyResolver.Resolve(selectedCard.Path, weapon.WeaponId),
                    string.Empty);
            _workshopNextDescription.color = selectedCard.IsSpecial
                ? SciFiTextStyle.CyanText
                : SciFiTextStyle.MainText;

            if (selectedCard.IsSpecial)
            {
                var specialRank = ResolveWorkshopSpecialRank(definition, progress);
                if (specialRank == 0)
                {
                    _workshopNextIcon.sprite = LoadSprite(selectedCard.IconResource);
                    _workshopNextIcon.enabled = _workshopNextIcon.sprite != null;
                    _workshopNextName.text = GameText.Localized("architect.card.special", string.Empty);
                    _workshopNextDescription.text = ResolveWorkshopSpecialDescription(definition, progress);
                    _workshopNextStatLabel.text = string.Empty;
                    _workshopNextStatValue.text = string.Empty;
                    SetWorkshopUpgradeCostVisible(false);
                    _workshopNextMaxLevelText.text = GameText.Localized("architect.maxLevelReached", string.Empty);
                    _workshopNextMaxLevelText.gameObject.SetActive(true);
                    return;
                }

                SetWorkshopUpgradeCostVisible(true);
                var options = definition.GetEvolutionOptions(specialRank);
                var hasSelectedOption = TryResolveWorkshopSpecialOption(definition, progress, specialRank, out var option);
                var hasPurchasedSpecial = WeaponEvolutionSelectionResolver.TryGetChoice(
                    definition, progress, specialRank, out _);
                var hasOptions = options.Count > 0;
                _workshopNextIcon.sprite = hasSelectedOption
                    ? LoadSprite(WeaponEvolutionIconResolver.ResolveEvolutionIconResource(weapon.IconId, specialRank, option.LegacyBranch))
                    : LoadSprite(selectedCard.IconResource);
                _workshopNextName.text = GameText.Localized("architect.card.special", string.Empty);
                _workshopNextDescription.text = hasSelectedOption
                    ? GameText.WeaponEvolutionDescription(option.Id, string.Empty)
                    : ResolveWorkshopSpecialDescription(definition, progress);
                _workshopNextStatLabel.text = string.Empty;
                _workshopNextStatValue.text = string.Empty;
                var specialCost = default(WeaponUpgradeCost);
                var specialHasCost = hasOptions && WeaponUpgradeEconomy.TryGetSpecialCost(
                    progress.rank + 1, specialRank, !hasPurchasedSpecial, out specialCost);
                _workshopNextChipIcon.gameObject.SetActive(specialHasCost);
                _workshopNextCoinIcon.gameObject.SetActive(specialHasCost);
                _workshopNextCrystalIcon.gameObject.SetActive(specialHasCost);
                _workshopNextChips.text = specialHasCost ? $"{progress.blueprints}/{specialCost.Blueprints}" : string.Empty;
                _workshopNextChips.color = specialHasCost && progress.blueprints >= specialCost.Blueprints
                    ? SciFiTextStyle.GoldText
                    : SciFiTextStyle.WarningText;
                _workshopNextCostLabel.text = specialHasCost ? GameText.Localized("architect.cost", string.Empty) : string.Empty;
                _workshopNextCoinAmount.text = specialHasCost ? specialCost.Credits.ToString() : string.Empty;
                _workshopNextCrystalAmount.text = specialHasCost ? specialCost.Crystals.ToString() : string.Empty;
                _workshopNextOrText.text = specialHasCost ? GameText.Localized("architect.or", string.Empty) : string.Empty;
                _workshopNextButton.gameObject.SetActive(hasOptions);
                _workshopNextButton.interactable = hasOptions && progress.rank >= specialRank;
                return;
            }

            if (isBasePathMaxed)
            {
                var maximumValue = ResolveWorkshopParameterValue(weapon, definition, progress, selectedCard.Parameter);
                _workshopNextStatLabel.text = ResolveWorkshopParameterLabel(selectedCard.Parameter);
                _workshopNextStatValue.text = FormatWorkshopValue(maximumValue);
                SetWorkshopUpgradeCostVisible(false);
                _workshopNextRequiredChipsTopDivider.gameObject.SetActive(true);
                _workshopNextCostTopDivider.gameObject.SetActive(false);
                _workshopNextMaxLevelText.text = GameText.Localized("architect.maxLevelReached", string.Empty);
                _workshopNextMaxLevelText.gameObject.SetActive(true);
                return;
            }

            SetWorkshopUpgradeCostVisible(true);
            _workshopNextOrText.text = GameText.Localized("architect.or", string.Empty);

            if (selectedCard.Parameter != WorkshopParameter.None)
            {
                var fromValue = ResolveWorkshopParameterValue(weapon, definition, progress, selectedCard.Parameter);
                var toValue = ResolveWorkshopParameterValue(weapon, definition, progress, selectedCard.Parameter, 1);
                _workshopNextStatLabel.text = ResolveWorkshopParameterLabel(selectedCard.Parameter);
                _workshopNextStatValue.text = FormatWorkshopValue(fromValue)
                    + "  <color=#" + WorkshopUpgradeGreenHex + ">→ " + FormatWorkshopValue(toValue) + "</color>";
            }
            else
            {
                _workshopNextStatLabel.text = string.Empty;
                _workshopNextStatValue.text = string.Empty;
            }

            WeaponUpgradeCost cost = default;
            var hasCost = progress.rank < definition.MaxRank && nextWeaponRank > 1
                && WeaponUpgradeEconomy.TryGetCost(nextWeaponRank, selectedCard.Path, out cost);
            var ownedChips = progress.blueprints;
            var requiredChips = hasCost ? cost.Blueprints : 0;
            _workshopNextChips.text = GameText.WorkshopChipProgress(ownedChips, requiredChips);
            _workshopNextChips.color = ownedChips >= requiredChips ? SciFiTextStyle.GoldText : SciFiTextStyle.WarningText;
            _workshopNextCoinAmount.text = hasCost ? cost.Credits.ToString() : "-";
            _workshopNextCrystalAmount.text = hasCost ? cost.Crystals.ToString() : "-";
            _workshopNextCostLabel.text = GameText.Localized("architect.cost", string.Empty);
            _workshopNextButton.gameObject.SetActive(true);
            _workshopNextButton.interactable = true;
        }

        private void SetWorkshopUpgradeCostVisible(bool visible)
        {
            _workshopNextRequiredChipsLabel.gameObject.SetActive(visible);
            _workshopNextChipIcon.gameObject.SetActive(visible);
            _workshopNextChips.gameObject.SetActive(visible);
            _workshopNextCostLabel.gameObject.SetActive(visible);
            _workshopNextCoinIcon.gameObject.SetActive(visible);
            _workshopNextCoinAmount.gameObject.SetActive(visible);
            _workshopNextOrText.gameObject.SetActive(visible);
            _workshopNextCrystalIcon.gameObject.SetActive(visible);
            _workshopNextCrystalAmount.gameObject.SetActive(visible);
            _workshopNextButton.gameObject.SetActive(visible);
            _workshopNextMaxLevelText.gameObject.SetActive(false);
            _workshopNextRequiredChipsTopDivider.gameObject.SetActive(visible);
            _workshopNextCostTopDivider.gameObject.SetActive(visible);
        }

        private static string ResolveWorkshopSpecialDescription(WeaponProgressionDefinition definition,
            SaveWeaponProgressData progress)
        {
            if (definition == null || progress == null)
            {
                return GameText.Localized("architect.card.special", string.Empty);
            }

            var branch = (WeaponUpgradeBranch)progress.branch;
            var description = WeaponUpgradePresentation.GetActiveEvolutionText(definition, progress.rank, branch);
            return string.IsNullOrWhiteSpace(description)
                ? GameText.Localized("architect.card.special", string.Empty)
                : description;
        }

        private static int ResolveWorkshopSpecialRank(WeaponProgressionDefinition definition, SaveWeaponProgressData progress)
        {
            if (definition.GetEvolutionOptions(5).Count > 0
                && definition.GetSpecialPurchaseCount(progress, 5) < definition.GetSpecialPurchaseLimit(5)) return 5;
            if (definition.GetEvolutionOptions(10).Count > 0
                && definition.GetSpecialPurchaseCount(progress, 10) < definition.GetSpecialPurchaseLimit(10)) return 10;
            return 0;
        }

        private static bool HasPurchasedWorkshopSpecial(WeaponProgressionDefinition definition,
            SaveWeaponProgressData progress)
        {
            return definition != null && progress != null
                && (definition.GetSpecialPurchaseCount(progress, 5) > 0
                    || definition.GetSpecialPurchaseCount(progress, 10) > 0);
        }

        private bool CanUpgradeWorkshopCard(int cardIndex, WeaponProgressionDefinition definition,
            SaveWeaponProgressData progress)
        {
            if (cardIndex < 0 || cardIndex >= WorkshopCardDefinitions.Length || definition == null || progress == null)
            {
                return false;
            }

            var card = WorkshopCardDefinitions[cardIndex];
            if (card.IsSpecial)
            {
                return false;
            }
            if (definition.CountBaseUpgrades(progress, card.Path) >= definition.GetBaseUpgradeLimit(card.Path))
            {
                return false;
            }
            var targetRank = progress.rank + 1;
            var commanderLevel = CommanderProgressionTable.GetLevel(AppServices.Economy.CommanderExperience);
            if (targetRank > 10
                || targetRank > WeaponUpgradeEconomy.GetMaxRankForCommanderLevel(definition, commanderLevel))
            {
                return false;
            }

            if (!WeaponUpgradeEconomy.TryGetCost(targetRank, card.Path, out var cost))
            {
                return false;
            }

            var hasStandardResources = AppServices.Economy.CanSpend(cost.Credits)
                && (AppServices.Economy.AdminModeEnabled || progress.blueprints >= cost.Blueprints);
            var hasCrystalResources = AppServices.Economy.AdminModeEnabled
                || AppServices.Economy.CanSpendPremium(cost.Crystals);
            return CommanderProgressionTable.GetLevel(AppServices.Economy.CommanderExperience)
                    >= WeaponUpgradeEconomy.GetFreeChoiceRequiredCommanderLevel(definition, targetRank)
                && (hasStandardResources || hasCrystalResources);
        }

        private void RefreshCurrentWorkshopStats(WeaponDefinition weapon, WeaponProgressionDefinition definition,
            SaveWeaponProgressData progress, WeaponUpgradeBranch branch)
        {
            var values = new[]
            {
                ResolveWorkshopParameterValue(weapon, definition, progress, WorkshopParameter.Core),
                ResolveWorkshopParameterValue(weapon, definition, progress, WorkshopParameter.Aim),
                ResolveWorkshopParameterValue(weapon, definition, progress, WorkshopParameter.Loadout),
                ResolveWorkshopParameterValue(weapon, definition, progress, WorkshopParameter.EffectScale)
            };
            var maxValues = new[]
            {
                Mathf.Max(1f, weapon.Damage * 1.5f),
                100f,
                weapon.HasLimitedAmmo ? Mathf.Max(1f, weapon.AmmoCount + 6f) : 1f,
                Mathf.Max(0.1f, weapon.ImpactRadius * 1.5f)
            };
            Canvas.ForceUpdateCanvases();
            for (var i = 0; i < _workshopCurrentStats.Length; i++)
            {
                var stat = _workshopCurrentStats[i];
                var value = values[i];
                stat.Value.text = FormatWorkshopValue(value);
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

        private static float ResolveWorkshopParameterValue(WeaponDefinition weapon,
            WeaponProgressionDefinition definition, SaveWeaponProgressData progress,
            WorkshopParameter parameter, int extraPurchases = 0)
        {
            switch (parameter)
            {
                case WorkshopParameter.Core:
                    return WeaponUpgradeChoiceResolver.ResolveValue(weapon, definition, progress,
                        WeaponUpgradePath.Core, extraPurchases);
                case WorkshopParameter.Aim:
                    return WeaponUpgradeChoiceResolver.ResolveValue(weapon, definition, progress,
                        WeaponUpgradePath.Aim, extraPurchases);
                case WorkshopParameter.Loadout:
                    return WeaponUpgradeChoiceResolver.ResolveValue(weapon, definition, progress,
                        WeaponUpgradePath.Loadout, extraPurchases);
                case WorkshopParameter.EffectScale:
                    return WeaponUpgradeChoiceResolver.ResolveValue(weapon, definition, progress,
                        WeaponUpgradePath.Radius, extraPurchases);
                default:
                    return 0f;
            }
        }

        private string ResolveWorkshopParameterLabel(WorkshopParameter parameter)
        {
            return parameter switch
            {
                WorkshopParameter.Core => GameText.Localized("architect.detail.statDamage", string.Empty),
                WorkshopParameter.Aim => GameText.Localized("architect.detail.statAccuracy", string.Empty),
                WorkshopParameter.Loadout => GameText.Localized("architect.detail.statAmmo", string.Empty),
                WorkshopParameter.EffectScale => GameText.Localized("architect.detail.statRadius", string.Empty),
                _ => string.Empty
            };
        }

        private string ResolveWorkshopLevelText(int currentRank, int selectedRank, bool showTarget = true)
        {
            if (!showTarget || currentRank == selectedRank)
            {
                return GameText.LevelLabel() + " " + currentRank;
            }

            return GameText.LevelLabel() + " " + currentRank
                + "  <color=#" + WorkshopUpgradeGreenHex + ">→ " + selectedRank + "</color>";
        }

        private static string FormatWorkshopValue(float value)
        {
            return value < 0f ? "∞" : Mathf.Abs(value - Mathf.Round(value)) < 0.01f
                ? Mathf.RoundToInt(value).ToString()
                : value.ToString("0.0");
        }

        private static void ApplyWorkshopArtFit(Image art)
        {
            if (art == null || art.sprite == null || art.sprite.rect.height <= 0f)
            {
                return;
            }

            var aspect = art.sprite.rect.width / art.sprite.rect.height;
            const float maxWidth = 360f;
            const float maxHeight = 220f;
            art.rectTransform.sizeDelta = aspect >= maxWidth / maxHeight
                ? new Vector2(maxWidth, maxWidth / aspect)
                : new Vector2(maxHeight * aspect, maxHeight);
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

            var blueprintIcon = CreateImage("Architect Blueprint Balance Icon", group, new Vector2(0f, 0.5f), new Vector2(0f, 0.5f),
                new Vector2(0f, 0.5f), BlueprintBalanceIconSize, Vector2.zero, "UI/Common/chip_2", true, Color.white);
            _resourceInfoPopup.BindResourceIcon(blueprintIcon.GetComponent<Image>(), ResourceInfoKind.Chips);

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
            MenuPlayerLevelPanel.Create(root, "Architect Player Level Panel", new Vector2(0f, 10f), new Vector2(520f, 172f));

            var coins = CreateImage("Architect Coins", root, new Vector2(1f, 1f), new Vector2(1f, 1f),
                new Vector2(1f, 1f), new Vector2(372f, 155f), new Vector2(-486f, 0f),
                ShopResourceRoot + "Coin_panel", true, Color.white);
            _resourceInfoPopup.BindResourceIcon(coins.GetComponent<Image>(), ResourceInfoKind.Coins);
            var coinsValue = CreateText("Architect Coins Value", coins, new Vector2(0.40f, 0.24f), new Vector2(0.80f, 0.80f),
                new Vector2(0.5f, 0.5f), Vector2.zero, Vector2.zero, 35, TextAlignmentOptions.Center,
                softCurrency.ToString(), GameTextRole.CurrencyGold);
            CurrencyBalanceText.Bind(coinsValue, false);

            var premium = CreateImage("Architect Premium", root, new Vector2(1f, 1f), new Vector2(1f, 1f),
                new Vector2(1f, 1f), new Vector2(372f, 155f), new Vector2(-166f, 0f),
                ShopResourceRoot + "vip_coins_panel", true, Color.white);
            _resourceInfoPopup.BindResourceIcon(premium.GetComponent<Image>(), ResourceInfoKind.Crystals);
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

            var lockSprite = LoadSprite(ShopResourceRoot + "lock");
            var selectedCardSprite = LoadSprite(WorkshopSelectedCardResource);
            for (var i = 0; i < _items.Count; i++)
            {
                var itemIndex = i;
                var slotRoot = CreateImageButton($"Architect Weapon Slot {i}", strip,
                    new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                    WeaponSlotSize, new Vector2(0f, 90f), ShopResourceRoot + "card",
                    () => SelectCarouselSlot(itemIndex));
                AddCarouselInput(slotRoot.gameObject);

                var frame = slotRoot.GetComponent<Image>();
                frame.sprite = selectedCardSprite;
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

        private void SelectWeapon(int index, bool preserveWorkshopCardSelection = false)
        {
            if (index < 0 || index >= _items.Count)
            {
                return;
            }

            var weapon = _items[index];
            // The tree's first node stands for the weapon's rank-1 state, so it keeps the
            // original art even after an evolution replaces the icon everywhere else.
            if (_selectedWeaponNodeImage != null)
            {
                _selectedWeaponNodeImage.sprite = LoadSprite(WeaponEvolutionIconResolver.ResolveCurrentIconResource(
                    weapon.IconId, 1, WeaponUpgradeBranch.None));
                _selectedWeaponNodeImage.enabled = _selectedWeaponNodeImage.sprite != null;
            }
            _selectedWeaponIndex = index;
            if (!preserveWorkshopCardSelection)
            {
                _selectedWorkshopCardIndex = 0;
            }
            _pendingEvolutionOptionId = null;
            _pendingUpgradeRank = 0;
            if (_blueprintBackgroundImage != null)
            {
                _blueprintBackgroundImage.sprite = LoadSprite(MenuResourceRoot + "arch_menu");
            }

            RefreshCarouselSlots();
            RefreshEvolutionIcons();
            RefreshChevronIcons();
            RefreshBranchVisibility();
            RefreshResetButtonVisibility();
            RefreshWorkshopCards();
            RefreshWorkshopSidePanels();
            RefreshWorkshopDescriptionScrolls();
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

        private void SelectCarouselSlot(int itemIndex)
        {
            if (_suppressNextCarouselClick)
            {
                _suppressNextCarouselClick = false;
                return;
            }

            SelectWeapon(itemIndex);
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
            var detailsCoinIcon = CreateImage("Architect Buy Coins Icon", coinButton, new Vector2(0f, 0.5f), new Vector2(0f, 0.5f),
                new Vector2(0f, 0.5f), PurchaseIconSize, new Vector2(46f, 0f), ShopResourceRoot + "coin", true, Color.white);
            detailsCoinIcon.GetComponent<Image>().raycastTarget = false;
            _detailsCoinPriceText = CreateText("Architect Buy Coins Price", coinButton.transform,
                new Vector2(0.30f, 0.12f), new Vector2(0.90f, 0.88f), new Vector2(0.5f, 0.5f),
                Vector2.zero, Vector2.zero, 32, TextAlignmentOptions.Center, string.Empty, GameTextRole.CurrencyGold);
            _detailsCoinPriceText.fontStyle = FontStyles.Bold;

            var crystalButton = CreateImageButton("Architect Spend Blueprints", _detailsCard,
                new Vector2(0.5f, 0f), new Vector2(0.5f, 0f), new Vector2(0.5f, 0f),
                PurchaseButtonSize, new Vector2(176f, 160f), ShopResourceRoot + "btn_yellow", BuyDetailsWithBlueprints);
            crystalButton.GetComponent<Image>().preserveAspect = false;
            _detailsCrystalBuyButton = crystalButton.GetComponent<Button>();
            var detailsCrystalIcon = CreateImage("Architect Spend Blueprints Icon", crystalButton, new Vector2(0f, 0.5f), new Vector2(0f, 0.5f),
                new Vector2(0f, 0.5f), PurchaseIconSize, new Vector2(46f, 0f), ShopResourceRoot + "vip_coin", true, Color.white);
            detailsCrystalIcon.GetComponent<Image>().raycastTarget = false;
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
            _pendingPath = WeaponUpgradePath.Core;
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
            _pendingPath = ResolveLegacyUpgradePath(definition, displayRank, branch);
            _pendingUpgradeRank = isBaseConfiguration ? 0 : displayRank;
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
                WeaponIconVisualTuning.ApplyDetailArtRotation(_detailsArtImage.rectTransform, weapon.WeaponId);
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
                _detailsDescriptionText.text = isBaseConfiguration
                    ? WeaponPresentationText.Describe(weapon)
                    : WeaponUpgradePresentation.GetCurrentDescription(weapon, definition, progress);
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
            _detailsCoinBuyButton.interactable = !isOwned;
            _detailsCrystalBuyButton.interactable = !isOwned;
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

        private void ShowUpgradeConfirmation()
        {
            if (_selectedWeaponIndex < 0 || _selectedWeaponIndex >= _items.Count
                || _selectedWorkshopCardIndex < 0 || _selectedWorkshopCardIndex >= WorkshopCardDefinitions.Length)
            {
                return;
            }

            var weapon = _items[_selectedWeaponIndex];
            if (!WeaponProgressionCatalog.TryGet(weapon.WeaponId, out var definition) || definition == null)
            {
                return;
            }

            var progress = AppServices.SaveService.Current.inventory.GetOrCreateWeaponProgress(weapon.WeaponId);
            var selectedCard = WorkshopCardDefinitions[_selectedWorkshopCardIndex];
            if (selectedCard.IsSpecial)
            {
                var rank = ResolveWorkshopSpecialRank(definition, progress);
                var options = definition.GetEvolutionOptions(rank);
                if (TryResolveWorkshopSpecialOption(definition, progress, rank, out var selectedOption))
                {
                    ShowEvolutionPurchaseConfirmation(weapon, rank, selectedOption);
                    return;
                }
                if (options.Count > 1 && progress.rank >= rank)
                {
                    ShowEvolutionChoiceModal(weapon, rank, options);
                    return;
                }
                if (options.Count != 1 || progress.rank < rank
                    || !WeaponUpgradeEconomy.TryGetSpecialCost(progress.rank + 1, rank, true, out _))
                {
                    return;
                }
                ShowEvolutionPurchaseConfirmation(weapon, rank, options[0]);
                return;
            }
            if (!CanUpgradeWorkshopCard(_selectedWorkshopCardIndex, definition, progress))
            {
                ShowUpgradeRequirements(_selectedWorkshopCardIndex, definition, progress);
                return;
            }

            var card = selectedCard;
            var targetRank = progress.rank + 1;
            if (!WeaponUpgradeEconomy.TryGetCost(targetRank, card.Path, out var cost))
            {
                return;
            }

            EnsureDetailsOverlay();
            _pendingBranch = (WeaponUpgradeBranch)progress.branch;
            _pendingPath = card.Path;
            _pendingUpgradeRank = targetRank;
            _detailsOverlay.transform.SetAsLastSibling();
            ApplyDetailsLayout(false);
            _detailsCard.anchoredPosition = Vector2.zero;
            _detailsArtImage.gameObject.SetActive(false);
            _detailsNameText.text = WeaponDisplayNames.Get(weapon.WeaponId);
            ApplyDetailsNameFit();
            _detailsDescriptionText.gameObject.SetActive(true);
            _detailsDescriptionText.text = GameText.Localized("architect.confirmParameter", string.Empty,
                new LocalizationArgument("parameter", ResolveWorkshopParameterTitle(card.Parameter)));
            _detailsDamageText.transform.parent.gameObject.SetActive(false);
            _detailsUsesText.transform.parent.gameObject.SetActive(false);
            _detailsOwnedCheckmark.gameObject.SetActive(false);
            _detailsCoinPriceText.text = cost.Credits.ToString();
            _detailsCrystalPriceText.text = cost.Crystals.ToString();
            _detailsCoinPriceText.color = AppServices.Economy.CanSpend(cost.Credits)
                ? SciFiTextStyle.GoldText
                : SciFiTextStyle.WarningText;
            _detailsCrystalPriceText.color = AppServices.Economy.AdminModeEnabled
                || AppServices.Economy.CanSpendPremium(cost.Crystals)
                ? SciFiTextStyle.GoldText
                : SciFiTextStyle.WarningText;
            _detailsCoinBuyButton.gameObject.SetActive(true);
            _detailsCrystalBuyButton.gameObject.SetActive(true);
            _detailsCoinBuyButton.interactable = true;
            _detailsCrystalBuyButton.interactable = true;
            _detailsOverlay.SetActive(true);
        }

        private void ShowUpgradeRequirements(int cardIndex, WeaponProgressionDefinition definition,
            SaveWeaponProgressData progress)
        {
            if (definition == null || progress == null || cardIndex < 0 || cardIndex >= WorkshopCardDefinitions.Length)
            {
                _resourceInfoPopup.ShowMessage(GameText.ResourceRequirementsTitle(), GameText.ResourceUnavailable());
                return;
            }

            var card = WorkshopCardDefinitions[cardIndex];
            if (card.IsSpecial)
            {
                return;
            }
            var targetRank = progress.rank + 1;
            if (targetRank > definition.MaxRank || !WeaponUpgradeEconomy.TryGetCost(targetRank, card.Path, out var cost))
            {
                _resourceInfoPopup.ShowMessage(GameText.ResourceRequirementsTitle(), GameText.ResourceUnavailable());
                return;
            }

            var messages = new List<string>();
            var requiredLevel = WeaponUpgradeEconomy.GetFreeChoiceRequiredCommanderLevel(definition, targetRank);
            if (CommanderProgressionTable.GetLevel(AppServices.Economy.CommanderExperience) < requiredLevel)
            {
                messages.Add(GameText.ResourceInsufficientLevel(requiredLevel));
            }

            var canSpendCoins = AppServices.Economy.CanSpend(cost.Credits)
                && (AppServices.Economy.AdminModeEnabled || progress.blueprints >= cost.Blueprints);
            var canSpendCrystals = AppServices.Economy.AdminModeEnabled
                || AppServices.Economy.CanSpendPremium(cost.Crystals);
            if (!canSpendCoins && !canSpendCrystals)
            {
                if (!AppServices.Economy.CanSpend(cost.Credits)
                    && !AppServices.Economy.CanSpendPremium(cost.Crystals))
                {
                    messages.Add(GameText.ResourceInsufficientBalance());
                }

                if (!AppServices.Economy.AdminModeEnabled && progress.blueprints < cost.Blueprints)
                {
                    messages.Add(GameText.ResourceInsufficientChips());
                }
            }

            if (messages.Count == 0)
            {
                messages.Add(GameText.ResourceUnavailable());
            }

            _resourceInfoPopup.ShowMessage(GameText.ResourceRequirementsTitle(), string.Join("\n\n", messages),
                messages.Exists(message => message == GameText.ResourceInsufficientChips())
                    ? ResourceInfoKind.Chips
                    : ResourceInfoKind.Coins);
        }

        private void ShowEvolutionChoiceModal(WeaponDefinition weapon, int rank,
            IReadOnlyList<WeaponEvolutionOptionDefinition> options)
        {
            EnsureDetailsOverlay();
            if (_evolutionChoiceRoot != null) Destroy(_evolutionChoiceRoot);
            _detailsOverlay.transform.SetAsLastSibling();
            _detailsCard.sizeDelta = new Vector2(1320f, 760f);
            _detailsCard.GetComponent<Image>().sprite = LoadSprite(WorkshopRightPanelResource);
            _detailsCard.GetComponent<Image>().type = Image.Type.Sliced;
            _detailsNameText.gameObject.SetActive(false);
            _detailsDescriptionViewport.gameObject.SetActive(false);
            _detailsArtImage.gameObject.SetActive(false);
            _detailsDamageText.transform.parent.gameObject.SetActive(false);
            _detailsUsesText.transform.parent.gameObject.SetActive(false);
            _detailsCoinBuyButton.gameObject.SetActive(false);
            _detailsCrystalBuyButton.gameObject.SetActive(false);
            _detailsOwnedCheckmark.gameObject.SetActive(false);
            _detailsCloseRect.anchoredPosition = new Vector2(555f, 285f);

            _evolutionChoiceRoot = new GameObject("Architect Evolution Choice Cards", typeof(RectTransform));
            _evolutionChoiceRoot.transform.SetParent(_detailsCard, false);
            var root = _evolutionChoiceRoot.GetComponent<RectTransform>();
            root.anchorMin = Vector2.zero; root.anchorMax = Vector2.one; root.offsetMin = Vector2.zero; root.offsetMax = Vector2.zero;
            var modalTitle = CreateText("Architect Evolution Choice Modal Title", root,
                new Vector2(.15f, .80f), new Vector2(.85f, .91f), new Vector2(.5f, .5f), Vector2.zero,
                Vector2.zero, 32, TextAlignmentOptions.Center,
                GameText.Localized("architect.chooseOneProperty", string.Empty), GameTextRole.DisplayTitle);
            modalTitle.enableAutoSizing = true;
            modalTitle.fontSizeMin = 24f;
            modalTitle.fontSizeMax = 32f;
            var width = Mathf.Min(500f, 1080f / Mathf.Max(1, options.Count));
            for (var index = 0; index < options.Count; index++)
            {
                var option = options[index];
                var x = (index - (options.Count - 1) * 0.5f) * (width + 60f);
                var card = CreateImageButton($"Architect Evolution Choice {rank} {index}", root,
                    new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                    new Vector2(width, 500f), new Vector2(x, 5f), WorkshopLeftPanelResource,
                    () => SelectWorkshopEvolutionOption(option, rank));
                card.GetComponent<Image>().preserveAspect = false;
                var art = CreateImage($"Architect Evolution Choice Art {rank} {index}", card,
                    new Vector2(.5f,.5f), new Vector2(.5f,.5f), new Vector2(.5f,.5f), new Vector2(360f,260f), new Vector2(0f,105f),
                    WeaponEvolutionIconResolver.ResolveEvolutionIconResource(weapon.IconId, rank, option.LegacyBranch), true, Color.white);
                var description = CreateText($"Architect Evolution Choice Description {rank} {index}", card,
                    new Vector2(.18f,.31f), new Vector2(.82f,.48f), new Vector2(.5f,.5f), Vector2.zero, Vector2.zero,
                    22, TextAlignmentOptions.Center, GameText.WeaponEvolutionDescription(option.Id, string.Empty), GameTextRole.Body);
                description.textWrappingMode = TextWrappingModes.Normal;
                description.enableAutoSizing = true;
                description.fontSizeMin = 18f;
                description.fontSizeMax = 22f;
            }
            _detailsOverlay.SetActive(true);
        }

        private void ShowEvolutionPurchaseConfirmation(WeaponDefinition weapon, int rank,
            WeaponEvolutionOptionDefinition option)
        {
            if (_evolutionChoiceRoot != null) { Destroy(_evolutionChoiceRoot); _evolutionChoiceRoot = null; }
            EnsureDetailsOverlay();
            if (_detailsOverlay == null || _detailsCard == null)
            {
                return;
            }
            if (!WeaponProgressionCatalog.TryGet(weapon.WeaponId, out var definition) || definition == null)
            {
                return;
            }
            var progress = AppServices.SaveService.Current.inventory.GetOrCreateWeaponProgress(weapon.WeaponId);
            var isInitialChoice = !WeaponEvolutionSelectionResolver.TryGetChoice(definition, progress, rank, out _);
            if (!WeaponUpgradeEconomy.TryGetSpecialCost(progress.rank + 1, rank, isInitialChoice, out var cost)) return;
            _pendingEvolutionOptionId = option.Id;
            _pendingUpgradeRank = rank;
            _pendingPath = WeaponUpgradePath.None;
            _detailsCard.sizeDelta = DetailsCardSize;
            _detailsCard.GetComponent<Image>().sprite = LoadSprite("UI/Battle/card");
            _detailsNameText.gameObject.SetActive(true);
            _detailsNameText.text = WeaponDisplayNames.Get(weapon.WeaponId);
            _detailsDescriptionViewport.gameObject.SetActive(true);
            _detailsDescriptionText.gameObject.SetActive(true);
            _detailsDescriptionText.text = GameText.WeaponEvolutionDescription(option.Id, string.Empty);
            _detailsArtImage.gameObject.SetActive(false);
            _detailsDamageText.transform.parent.gameObject.SetActive(false);
            _detailsUsesText.transform.parent.gameObject.SetActive(false);
            _detailsOwnedCheckmark.gameObject.SetActive(false);
            _detailsCoinPriceText.text = cost.Credits.ToString();
            _detailsCrystalPriceText.text = cost.Crystals.ToString();
            _detailsCoinBuyButton.gameObject.SetActive(true);
            _detailsCrystalBuyButton.gameObject.SetActive(true);
            _detailsCloseRect.anchoredPosition = DetailsCloseFixedPosition;
            _detailsOverlay.SetActive(true);
        }

        private bool TryResolveWorkshopSpecialOption(WeaponProgressionDefinition definition,
            SaveWeaponProgressData progress, int rank, out WeaponEvolutionOptionDefinition option)
        {
            if (!string.IsNullOrWhiteSpace(_pendingEvolutionOptionId)
                && definition != null
                && definition.TryGetEvolutionOption(rank, _pendingEvolutionOptionId, out option))
            {
                return true;
            }

            if (WeaponEvolutionSelectionResolver.TryGetChoice(definition, progress, rank, out option))
            {
                return true;
            }

            var options = definition?.GetEvolutionOptions(rank);
            if (options != null && options.Count == 1)
            {
                option = options[0];
                return true;
            }

            option = default;
            return false;
        }

        private void SelectWorkshopEvolutionOption(WeaponEvolutionOptionDefinition option, int rank)
        {
            if (_evolutionChoiceRoot != null)
            {
                Destroy(_evolutionChoiceRoot);
                _evolutionChoiceRoot = null;
            }

            _pendingEvolutionOptionId = option.Id;
            _pendingUpgradeRank = rank;
            _selectedWorkshopCardIndex = WorkshopCardDefinitions.Length - 1;
            _detailsOverlay.SetActive(false);
            RefreshWorkshopCards();
            RefreshWorkshopSidePanels();
            RefreshWorkshopDescriptionScrolls();
        }

        private void ApplyWorkshopNextPanelLayout(bool isSpecial)
        {
            var nameRect = _workshopNextName.rectTransform;
            var iconRect = _workshopNextIcon.rectTransform;
            var descriptionRect = _workshopNextDescriptionScroll.viewport;

            if (isSpecial)
            {
                _workshopNextName.gameObject.SetActive(false);

                iconRect.anchorMin = iconRect.anchorMax = new Vector2(0.5f, 0.5f);
                iconRect.sizeDelta = new Vector2(264f, 240f);
                iconRect.anchoredPosition = new Vector2(-25f, 250f);

                descriptionRect.anchorMin = descriptionRect.anchorMax = new Vector2(0.5f, 0.5f);
                descriptionRect.sizeDelta = new Vector2(340f, 80f);
                descriptionRect.anchoredPosition = new Vector2(-25f, 105f);
                _workshopNextDescription.alignment = TextAlignmentOptions.TopLeft;
                _workshopNextDescriptionTopDivider.rectTransform.anchoredPosition = new Vector2(-30f, 115f);
                _workshopNextDescriptionBottomDivider.rectTransform.anchoredPosition = new Vector2(-30f, 15f);
                _workshopNextRequiredChipsTopDivider.rectTransform.anchoredPosition = new Vector2(-30f, 15f);
                _workshopNextCostTopDivider.rectTransform.anchoredPosition = new Vector2(-30f, -85f);
                SetWorkshopCostRowVerticalOffset(20f);
                return;
            }

            _workshopNextName.gameObject.SetActive(true);
            nameRect.anchorMin = new Vector2(0.12f, 0.82f);
            nameRect.anchorMax = new Vector2(0.56f, 0.91f);
            nameRect.sizeDelta = Vector2.zero;
            nameRect.anchoredPosition = new Vector2(20f, 10f);
            _workshopNextName.alignment = TextAlignmentOptions.MidlineLeft;

            iconRect.anchorMin = iconRect.anchorMax = new Vector2(0.5f, 0.5f);
            iconRect.sizeDelta = new Vector2(132f, 120f);
            iconRect.anchoredPosition = new Vector2(80f, 220f);

            descriptionRect.anchorMin = new Vector2(0.18f, 0.55f);
            descriptionRect.anchorMax = new Vector2(0.82f, 0.69f);
            descriptionRect.sizeDelta = Vector2.zero;
            descriptionRect.anchoredPosition = new Vector2(-20f, 30f);
            _workshopNextDescription.alignment = TextAlignmentOptions.TopLeft;
            _workshopNextDescriptionTopDivider.rectTransform.anchoredPosition = new Vector2(-30f, 160f);
            _workshopNextDescriptionBottomDivider.rectTransform.anchoredPosition = new Vector2(-30f, 55f);
            _workshopNextRequiredChipsTopDivider.rectTransform.anchoredPosition = new Vector2(-30f, 10f);
            _workshopNextCostTopDivider.rectTransform.anchoredPosition = new Vector2(-30f, -105f);
            SetWorkshopCostRowVerticalOffset(0f);
        }

        private void RefreshWorkshopDescriptionScrolls()
        {
            RefreshWorkshopDescriptionScroll(_workshopCurrentDescriptionScroll, _workshopCurrentDescription);
            RefreshWorkshopDescriptionScroll(_workshopNextDescriptionScroll, _workshopNextDescription);
        }

        private static void RefreshWorkshopDescriptionScroll(ScrollRect scroll, TMP_Text description)
        {
            if (scroll == null || description == null || scroll.content == null || scroll.viewport == null)
            {
                return;
            }

            Canvas.ForceUpdateCanvases();
            var preferredHeight = description.GetPreferredValues(description.text, scroll.viewport.rect.width, 0f).y;
            scroll.content.sizeDelta = new Vector2(0f, Mathf.Max(scroll.viewport.rect.height, preferredHeight));
            scroll.verticalNormalizedPosition = 1f;
        }

        private void SetWorkshopCostRowVerticalOffset(float offsetY)
        {
            _workshopNextCoinIcon.rectTransform.anchoredPosition = new Vector2(0f, -180f + offsetY);
            _workshopNextCoinAmount.rectTransform.anchoredPosition = new Vector2(20f, 30f + offsetY);
            _workshopNextOrText.rectTransform.anchoredPosition = new Vector2(10f, 30f + offsetY);
            _workshopNextCrystalIcon.rectTransform.anchoredPosition = new Vector2(0f, -180f + offsetY);
            _workshopNextCrystalAmount.rectTransform.anchoredPosition = new Vector2(0f, 30f + offsetY);
        }

        private static WeaponUpgradePath ResolveLegacyUpgradePath(
            WeaponProgressionDefinition definition,
            int rank,
            WeaponUpgradeBranch branch)
        {
            if (definition == null || !definition.TryGetNode(rank, branch, out var node))
            {
                return WeaponUpgradePath.Core;
            }

            return node.Stat switch
            {
                WeaponUpgradeStat.Accuracy => WeaponUpgradePath.Aim,
                WeaponUpgradeStat.Range => WeaponUpgradePath.Aim,
                WeaponUpgradeStat.Ammo => WeaponUpgradePath.Loadout,
                WeaponUpgradeStat.ProjectileCount => WeaponUpgradePath.Loadout,
                WeaponUpgradeStat.ImpactRadius => WeaponUpgradePath.Radius,
                _ => WeaponUpgradePath.Core
            };
        }

        private string ResolveWorkshopParameterTitle(WorkshopParameter parameter)
        {
            return parameter switch
            {
                WorkshopParameter.Core => GameText.Localized("architect.card.core", string.Empty),
                WorkshopParameter.Aim => GameText.Localized("architect.card.aim", string.Empty),
                WorkshopParameter.Loadout => GameText.Localized("architect.detail.loadoutTitle", string.Empty),
                WorkshopParameter.EffectScale => GameText.Localized("architect.card.radius", string.Empty),
                _ => GameText.Localized("architect.card.special", string.Empty)
            };
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
        private static Vector2 ResolveDetailsArtScale(string weaponId, int rank, WeaponUpgradeBranch branch)
        {
            return DetailsArtBoosts.TryGetValue(weaponId, out var boost) && boost.Covers(rank, branch)
                ? boost.Scale
                : Vector2.one;
        }

        private static void ApplyDetailsArtFit(Image artImage, Vector2 scale)
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
            artImage.rectTransform.sizeDelta = Vector2.Scale(fittedSize, scale);
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
            if (_evolutionChoiceRoot != null)
            {
                Destroy(_evolutionChoiceRoot);
                _evolutionChoiceRoot = null;
            }
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
            var progress = AppServices.SaveService.Current.inventory.GetOrCreateWeaponProgress(weapon.WeaponId);
            if (!string.IsNullOrWhiteSpace(_pendingEvolutionOptionId))
            {
                var evolution = AppServices.WeaponUpgrades.TryPurchaseEvolution(weapon.WeaponId,
                    _pendingUpgradeRank, _pendingEvolutionOptionId, false);
                if (!evolution.Applied) { return; }
                _pendingEvolutionOptionId = null;
                AppServices.RefreshHistoricalPower();
                AppServices.SaveService.Save(AppServices.SaveService.Current);
                SelectWeapon(_selectedWeaponIndex);
                HideWeaponDetails();
                return;
            }
            if (_pendingUpgradeRank <= 1 || _pendingUpgradeRank != progress.rank + 1)
            {
                _resourceInfoPopup.ShowMessage(GameText.ResourceRequirementsTitle(), GameText.ResourceUnavailable());
                return;
            }

            if (!WeaponUpgradeEconomy.TryGetCost(_pendingUpgradeRank, _pendingPath, out var cost)
                || !AppServices.Economy.CanSpend(cost.Credits)
                || (!AppServices.Economy.AdminModeEnabled && progress.blueprints < cost.Blueprints))
            {
                ShowPendingUpgradeRequirements(false);
                return;
            }

            var upgrade = AppServices.WeaponUpgrades.TryUpgrade(weapon.WeaponId, _pendingPath, false);
            if (!upgrade.Applied)
            {
                return;
            }

            AppServices.RefreshHistoricalPower();
            AppServices.SaveService.Save(AppServices.SaveService.Current);
            _pendingUpgradeRank = 0;
            SelectWeapon(_selectedWeaponIndex, true);
            HideWeaponDetails();
        }

        private void BuyDetailsWithBlueprints()
        {
            if (_selectedWeaponIndex < 0 || _selectedWeaponIndex >= _items.Count)
            {
                return;
            }

            var weapon = _items[_selectedWeaponIndex];
            var progress = AppServices.SaveService.Current.inventory.GetOrCreateWeaponProgress(weapon.WeaponId);
            if (!string.IsNullOrWhiteSpace(_pendingEvolutionOptionId))
            {
                var evolution = AppServices.WeaponUpgrades.TryPurchaseEvolution(weapon.WeaponId,
                    _pendingUpgradeRank, _pendingEvolutionOptionId, true);
                if (!evolution.Applied) { return; }
                _pendingEvolutionOptionId = null;
                AppServices.RefreshHistoricalPower();
                AppServices.SaveService.Save(AppServices.SaveService.Current);
                SelectWeapon(_selectedWeaponIndex);
                HideWeaponDetails();
                return;
            }
            if (_pendingUpgradeRank <= 1 || _pendingUpgradeRank != progress.rank + 1)
            {
                _resourceInfoPopup.ShowMessage(GameText.ResourceRequirementsTitle(), GameText.ResourceUnavailable());
                return;
            }

            if (!WeaponUpgradeEconomy.TryGetCost(_pendingUpgradeRank, _pendingPath, out var cost)
                || (!AppServices.Economy.AdminModeEnabled && !AppServices.Economy.CanSpendPremium(cost.Crystals)))
            {
                ShowPendingUpgradeRequirements(true);
                return;
            }

            var upgrade = AppServices.WeaponUpgrades.TryUpgrade(weapon.WeaponId, _pendingPath, true);
            if (!upgrade.Applied)
            {
                return;
            }

            AppServices.RefreshHistoricalPower();
            AppServices.SaveService.Save(AppServices.SaveService.Current);
            _pendingUpgradeRank = 0;
            SelectWeapon(_selectedWeaponIndex, true);
            HideWeaponDetails();
        }

        private void ShowPendingUpgradeRequirements(bool useCrystals)
        {
            if (!WeaponUpgradeEconomy.TryGetCost(_pendingUpgradeRank, _pendingPath, out var cost))
            {
                _resourceInfoPopup.ShowMessage(GameText.ResourceRequirementsTitle(), GameText.ResourceUnavailable());
                return;
            }

            var message = useCrystals
                ? GameText.ResourceInsufficientBalance()
                : GameText.ResourceInsufficientBalance();
            if (!useCrystals && !AppServices.Economy.AdminModeEnabled)
            {
                var progress = AppServices.SaveService.Current.inventory.GetOrCreateWeaponProgress(_items[_selectedWeaponIndex].WeaponId);
                if (progress.blueprints < cost.Blueprints && AppServices.Economy.CanSpend(cost.Credits))
                {
                    message = GameText.ResourceInsufficientChips();
                }
                else if (progress.blueprints < cost.Blueprints)
                {
                    message += "\n\n" + GameText.ResourceInsufficientChips();
                }
            }

            _resourceInfoPopup.ShowMessage(GameText.ResourceRequirementsTitle(), message,
                useCrystals ? ResourceInfoKind.Crystals : ResourceInfoKind.Coins);
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

        private Image CreateWorkshopDivider(Component parent, string name, Vector2 anchoredPosition)
        {
            return CreateWorkshopDivider(parent, name, anchoredPosition, WorkshopDividerSize);
        }

        private Image CreateWorkshopDivider(Component parent, string name, Vector2 anchoredPosition, Vector2 size)
        {
            var divider = CreatePanel(name, parent,
                new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                size, anchoredPosition, WorkshopDividerColor);
            divider.raycastTarget = false;
            return divider;
        }

        private void ChangePage(int direction)
        {
            var maxStart = GetMaxPageStart();
            if (maxStart <= 0)
            {
                return;
            }

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
                _slots[i].Root.gameObject.SetActive(visible);
                if (visible)
                {
                    _slots[i].Root.anchoredPosition = new Vector2(
                        startX + visibleIndex * CarouselSpacing, 90f);
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

        private void RefreshCarouselSlots()
        {
            for (var i = 0; i < _slots.Count; i++)
            {
                var slot = _slots[i];
                var weapon = _items[slot.SlotIndex];
                var progress = AppServices.SaveService.Current.inventory.GetOrCreateWeaponProgress(weapon.WeaponId);
                var hasProgression = WeaponProgressionCatalog.TryGet(weapon.WeaponId, out var progression);
                slot.WeaponImage.sprite = hasProgression
                    ? LoadSprite(WeaponEvolutionIconResolver.ResolveCurrentIconResource(
                        weapon.IconId, progression, progress))
                    : LoadSprite(WeaponEvolutionIconResolver.ResolveCurrentIconResource(
                        weapon.IconId, progress.rank, (WeaponUpgradeBranch)progress.branch));
                slot.WeaponImage.color = Color.white;
                ApplySlotWeaponVisual(slot.Weapon, weapon.WeaponId);
                slot.Frame.sprite = slot.SlotIndex == _selectedWeaponIndex
                    ? LoadSprite(WorkshopSelectedCardResource)
                    : LoadSprite(ShopResourceRoot + "card");
                slot.Frame.color = NormalSlotColor;
                slot.Lock.enabled = false;
                var maximumRank = hasProgression && progression != null ? progression.MaxRank : progress.rank;
                slot.Label.text = GameText.LevelLabel() + " " + progress.rank + "/" + maximumRank;
                slot.Label.color = SciFiTextStyle.SecondaryText;

                var labelRect = slot.Label.rectTransform;
                labelRect.anchorMin = new Vector2(0.197f, 0.161f);
                labelRect.anchorMax = new Vector2(0.897f, 0.321f);
                labelRect.offsetMin = Vector2.zero;
                labelRect.offsetMax = Vector2.zero;
            }
        }

        private int GetMaxPageStart()
        {
            return Mathf.Max(0, _items.Count - _visibleCarouselCount);
        }

        private int GetVisibleCarouselCount()
        {
            var canvasWidth = GetCarouselCanvasWidth();
            var arrowClearance = CarouselLeftArrowOffset.x + CarouselArrowSize.x * 0.5f;
            var availableWidth = Mathf.Max(0f, canvasWidth - arrowClearance * 2f);
            var count = Mathf.FloorToInt(
                (availableWidth + CarouselEdgeAllowance - WeaponSlotSize.x) / CarouselSpacing) + 1;
            return Mathf.Clamp(count, MinimumVisibleCarouselCards, WeaponsPerPage);
        }

        private float GetCarouselCanvasWidth()
        {
            var canvas = _slots.Count > 0 ? _slots[0].Root.GetComponentInParent<Canvas>() : null;
            var rect = canvas != null ? canvas.GetComponent<RectTransform>() : null;
            return rect != null && rect.rect.width > 0f ? rect.rect.width : 1920f;
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
                    _carouselDragStartScroll = _carouselScroll;
                    _carouselDragMoved = false;
                    _suppressNextCarouselClick = false;
                }
            });
            AddEvent(trigger, EventTriggerType.BeginDrag, data =>
            {
                if (data is PointerEventData pointer)
                {
                    _carouselDragStart = pointer.position;
                    _carouselDragStartScroll = _carouselScroll;
                }
            });
            AddEvent(trigger, EventTriggerType.Drag, data =>
            {
                if (data is not PointerEventData pointer)
                {
                    return;
                }

                var delta = pointer.position - _carouselDragStart;
                if (Mathf.Abs(delta.x) < 8f || Mathf.Abs(delta.x) < Mathf.Abs(delta.y))
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
            AddEvent(trigger, EventTriggerType.EndDrag, data =>
            {
                if (data is not PointerEventData pointer)
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
            WeaponIconVisualTuning.ApplyWorkshopCarousel(rect, weaponId, Vector2.zero);
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

        private TMP_Text CreateWorkshopDescriptionScroll(string name, Component parent, Vector2 anchorMin,
            Vector2 anchorMax, int fontSize, out ScrollRect scroll, Vector2 anchoredPosition = default)
        {
            var viewport = CreateRect(name + " Viewport", parent, anchorMin, anchorMax,
                new Vector2(0.5f, 0.5f), Vector2.zero, anchoredPosition);
            var viewportImage = viewport.gameObject.AddComponent<Image>();
            viewportImage.sprite = _whiteSprite;
            viewportImage.color = new Color(1f, 1f, 1f, 0f);
            viewportImage.raycastTarget = true;
            viewport.gameObject.AddComponent<RectMask2D>();

            var content = CreateRect(name + " Content", viewport, new Vector2(0f, 1f), new Vector2(1f, 1f),
                new Vector2(0.5f, 1f), new Vector2(0f, 1f), Vector2.zero);
            var text = CreateText(name, content, Vector2.zero, Vector2.one, new Vector2(0.5f, 1f),
                Vector2.zero, Vector2.zero, fontSize, TextAlignmentOptions.TopLeft, string.Empty, GameTextRole.Body);
            text.raycastTarget = false;

            scroll = viewport.gameObject.AddComponent<ScrollRect>();
            scroll.viewport = viewport;
            scroll.content = content;
            scroll.horizontal = false;
            scroll.vertical = true;
            scroll.movementType = ScrollRect.MovementType.Clamped;
            scroll.scrollSensitivity = 30f;
            return text;
        }

        private Sprite LoadWorkshopStatIcon(int iconIndex)
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
            var border = radius + 1f;
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
            return Sprite.Create(texture, new Rect(0f, 0f, width, height), new Vector2(0.5f, 0.5f),
                100f, 0u, SpriteMeshType.FullRect, new Vector4(border, border, border, border));
        }

        private static Sprite CreateDisabledWorkshopMarkerSprite()
        {
            const int size = 64;
            const float outerRadius = 25f;
            const float innerRadius = 20f;
            const float slashHalfWidth = 3.5f;
            var texture = new Texture2D(size, size, TextureFormat.RGBA32, false);
            var pixels = new Color32[size * size];
            var center = (size - 1) * 0.5f;

            for (var y = 0; y < size; y++)
            {
                for (var x = 0; x < size; x++)
                {
                    var horizontal = x - center;
                    var vertical = y - center;
                    var distance = Mathf.Sqrt(horizontal * horizontal + vertical * vertical);
                    var isCircle = distance >= innerRadius && distance <= outerRadius;
                    var isSlash = Mathf.Abs(horizontal - vertical) <= slashHalfWidth
                        && distance <= outerRadius;
                    pixels[y * size + x] = isCircle || isSlash
                        ? new Color32(255, 255, 255, 255)
                        : new Color32(255, 255, 255, 0);
                }
            }

            texture.SetPixels32(pixels);
            texture.Apply(false, true);
            return Sprite.Create(texture, new Rect(0f, 0f, size, size), new Vector2(0.5f, 0.5f), 100f);
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
