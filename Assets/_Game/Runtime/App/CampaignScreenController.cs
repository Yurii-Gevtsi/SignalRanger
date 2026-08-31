using System;
using System.Collections.Generic;
using Game.Core;
using Game.Core.Localization;
using Game.Progression.Missions;
using Game.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.App
{
    /// <summary>
    /// Visual campaign galaxy map. Clusters unlock one after another; the open cluster
    /// holds a small constellation of individually playable planets, each owning one
    /// chapter of the linear mission list. Planet and cluster state are both derived
    /// from campaign progression, so there is no separate map progress to keep in sync.
    /// </summary>
    public sealed class CampaignScreenController : MonoBehaviour
    {
        private const string MenuResourceRoot = "UI/Menu/";
        private const string CampaignResourceRoot = "UI/Menu/Campaign/";
        private const string ShopResourceRoot = "UI/Shop/";
        private const string CommonResourceRoot = "UI/Common/";
        private const string CheckmarkResource = "UI/Menu/Rewards/Icons/check";

        private static readonly Color TextShadowColor = new(0.02f, 0.03f, 0.05f, 0.9f);
        private static readonly Vector2 BackButtonSize = new(118f, 118f);
        private static readonly Vector2 BackButtonPosition = new(30f, 0f);
        private static readonly Vector2 TitlePanelSize = new(560f, 110f);
        private static readonly Vector2 TitlePanelPosition = new(128f, 0f);
        private static readonly Vector2 SettingsButtonSize = new(104f, 104f);
        private static readonly Vector2 SettingsButtonPosition = new(-26f, -10f);
        private static readonly Vector2 LockSize = new(118f, 118f);
        private static readonly Vector2 FirstClusterPosition = new(-750f, -250f);
        private static readonly Vector2 FirstClusterLabelPosition = new(100f, 0f);
        private static readonly Vector2 ClusterLabelPosition = new(0f, 28f);
        // Sits high in the node so the padlock reads as belonging to the number above it.
        private static readonly Vector2 LockPosition = new(0f, 44f);

        private static readonly Vector2 StarSize = new(150f, 150f);
        private static readonly Vector2 StarCheckmarkSize = new(80f, 80f);
        // The sprite's bar occupies only ~64 of its 1254px height, so the rect has to be
        // far taller than the line the player actually sees.
        private const float ConnectorThickness = 200f;

        private static readonly Vector2 CardSize = new(880f, 880f);
        private static readonly Vector2 BattleButtonSize = new(340f, 128f);
        private static readonly Vector2 MissionCheckSize = new(52f, 52f);
        private const int MaxMissionRows = 8;
        private const float MissionRowHeight = 78f;
        // Step is larger than the row itself, so the rows read as separate entries
        // instead of one solid block of text.
        private const float MissionRowSpacing = 104f;
        private const float MissionRowTop = 180f;
        private const float MissionRowOffsetX = 50f;
        private const float MissionCheckOffsetX = -100f;
        private static readonly Vector2 CardTitlePosition = new(0f, -90f);

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


        private struct PlanetView
        {
            public Button Button;
            public Image Star;
            public Image Checkmark;
            public List<int> MissionIndices;
        }

        private struct MissionRowView
        {
            public RectTransform Root;
            public TMP_Text Title;
            public Image Check;
        }


        private readonly List<PlanetView> _planets = new();
        private readonly List<MissionRowView> _missionRows = new();
        private readonly Dictionary<int, Image> _clusterLocks = new();
        private GameObject _cardOverlay;
        private TMP_Text _cardTitle;
        private TMP_Text _cardEmptyLabel;
        private Button _battleButton;
        private TMP_Text _battleLockedLabel;
        private int _openPlanetIndex = -1;
        private string _focusMissionId = string.Empty;

        /// <summary>
        /// Opens the map. When <paramref name="focusMissionId"/> is given, the planet
        /// owning that mission has its card opened straight away - used when returning
        /// from a battle so the player sees what they cleared and what is next.
        /// </summary>
        public static void Open(string focusMissionId = null)
        {
            if (FindAnyObjectByType<CampaignScreenController>() != null)
            {
                return;
            }

            var root = new GameObject("Campaign Screen");
            root.AddComponent<CampaignScreenController>()._focusMissionId = focusMissionId ?? string.Empty;
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

            BuildMissionCard(root);
            RefreshMap();
            OpenFocusedPlanet();
        }

        private void OpenFocusedPlanet()
        {
            if (string.IsNullOrWhiteSpace(_focusMissionId) || AppServices.Campaign == null)
            {
                return;
            }

            for (var planet = 0; planet < _planets.Count; planet++)
            {
                var missions = _planets[planet].MissionIndices;
                for (var i = 0; i < missions.Count; i++)
                {
                    var mission = AppServices.Campaign.GetMission(missions[i]);
                    if (mission != null && string.Equals(mission.MissionId, _focusMissionId, StringComparison.Ordinal))
                    {
                        OpenMissionCard(planet);
                        return;
                    }
                }
            }
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
                if (cluster.Number == 1)
                {
                    // The open cluster carries the playable constellation; every other
                    // cluster is still just a locked marker.
                    BuildConstellation(node);
                }
                else
                {
                    var lockIcon = CreateImage($"Campaign Lock {cluster.Number}", node, new Vector2(0.5f, 0.5f),
                        new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), LockSize,
                        LockPosition, ShopResourceRoot + "lock", true, Color.white).GetComponent<Image>();
                    _clusterLocks[cluster.Number] = lockIcon;
                }

                var label = CreateText($"Campaign Cluster Number {cluster.Number}", node,
                    new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(0.5f, 0.5f),
                    new Vector2(90f, 60f),
                    cluster.Number == 1 ? FirstClusterLabelPosition : ClusterLabelPosition, 44,
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

        /// <summary>
        /// The four planets of the open cluster plus the straight links between them.
        /// The connector sprite is a horizontal bar, so each link is stretched to the
        /// gap length and rotated onto the line joining its two stars.
        /// </summary>
        private void BuildConstellation(Transform clusterNode)
        {
            for (var i = 0; i < CampaignPlanetCatalog.Planets.Length - 1; i++)
            {
                var from = CampaignPlanetCatalog.Planets[i].Position;
                var delta = CampaignPlanetCatalog.Planets[i + 1].Position - from;
                var link = CreateImage($"Campaign Route {i + 1}", clusterNode, new Vector2(0.5f, 0.5f),
                    new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                    new Vector2(delta.magnitude, ConnectorThickness), from + delta * 0.5f,
                    CampaignResourceRoot + "connector", false, Color.white);
                link.localRotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(delta.y, delta.x) * Mathf.Rad2Deg);
            }

            for (var i = 0; i < CampaignPlanetCatalog.Planets.Length; i++)
            {
                var layout = CampaignPlanetCatalog.Planets[i];
                var planetIndex = i;
                var star = CreateImageButton($"Campaign Planet {i + 1}", clusterNode, new Vector2(0.5f, 0.5f),
                    new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), StarSize, layout.Position,
                    "UI/Generated/Campaign/" + layout.MapResource, () => OpenMissionCard(planetIndex));

                var checkmark = CreateImage($"Campaign Planet {i + 1} Checkmark", star, new Vector2(0.5f, 0.5f),
                    new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), StarCheckmarkSize, Vector2.zero,
                    CheckmarkResource, true, Color.white).GetComponent<Image>();
                checkmark.enabled = false;

                _planets.Add(new PlanetView
                {
                    Button = star.GetComponent<Button>(),
                    Star = star.GetComponent<Image>(),
                    Checkmark = checkmark,
                    MissionIndices = ResolveMissionIndices(layout.ChapterId)
                });
            }
        }

        /// <summary>
        /// Indices of every mission in a chapter, in campaign order. Empty for planets
        /// with no chapter yet, and when the campaign service is unavailable (e.g. the
        /// screen opened outside a booted app).
        /// </summary>
        private static List<int> ResolveMissionIndices(string chapterId)
        {
            var indices = new List<int>();
            var campaign = AppServices.Campaign;
            if (campaign == null || string.IsNullOrWhiteSpace(chapterId))
            {
                return indices;
            }

            for (var i = 0; i < campaign.MissionCount; i++)
            {
                var mission = campaign.GetMission(i);
                if (mission != null && string.Equals(mission.ChapterId, chapterId, StringComparison.Ordinal))
                {
                    indices.Add(i);
                }
            }

            return indices;
        }

        private void RefreshMap()
        {
            for (var i = 0; i < _planets.Count; i++)
            {
                var view = _planets[i];
                var cleared = IsPlanetCleared(view.MissionIndices);

                // Every planet is drawn at full brightness and stays openable so the
                // player can read ahead; the card is what explains that a later planet
                // cannot be played yet. A cleared planet is the one exception - it stops
                // responding and wears a checkmark.
                view.Button.interactable = !cleared;
                view.Checkmark.enabled = cleared;
            }

            // The next cluster opens once every planet of the open one is cleared.
            var openClusterCleared = IsOpenClusterCleared();
            foreach (var pair in _clusterLocks)
            {
                pair.Value.enabled = !(pair.Key == 2 && openClusterCleared);
            }
        }

        private bool IsOpenClusterCleared()
        {
            var hasContent = false;
            for (var i = 0; i < _planets.Count; i++)
            {
                var missions = _planets[i].MissionIndices;
                if (missions.Count == 0)
                {
                    // Planets held for future content must not block the cluster.
                    continue;
                }

                hasContent = true;
                if (!IsPlanetCleared(missions))
                {
                    return false;
                }
            }

            return hasContent;
        }

        private static bool IsPlanetCleared(List<int> missionIndices)
        {
            if (missionIndices == null || missionIndices.Count == 0)
            {
                return false;
            }

            for (var i = 0; i < missionIndices.Count; i++)
            {
                if (!IsMissionCompleted(missionIndices[i]))
                {
                    return false;
                }
            }

            return true;
        }

        private static bool IsMissionCompleted(int missionIndex)
        {
            var mission = AppServices.Campaign?.GetMission(missionIndex);
            var completed = AppServices.SaveService?.Current?.campaign?.completedMissionIds;
            return mission != null && completed != null && completed.Contains(mission.MissionId);
        }

        /// <summary>The planet's first unfinished mission, or -1 when it is fully cleared.</summary>
        private static int FindNextMission(List<int> missionIndices)
        {
            for (var i = 0; i < missionIndices.Count; i++)
            {
                if (!IsMissionCompleted(missionIndices[i]))
                {
                    return missionIndices[i];
                }
            }

            return -1;
        }

        private void BuildMissionCard(Transform root)
        {
            _cardOverlay = new GameObject("Campaign Mission Overlay", typeof(RectTransform));
            var overlayRect = (RectTransform)_cardOverlay.transform;
            overlayRect.SetParent(root, false);
            overlayRect.anchorMin = Vector2.zero;
            overlayRect.anchorMax = Vector2.one;
            overlayRect.sizeDelta = Vector2.zero;
            overlayRect.anchoredPosition = Vector2.zero;

            // Full-screen blocker: dims the map and swallows clicks meant for the stars.
            var blocker = _cardOverlay.AddComponent<Image>();
            blocker.sprite = _whiteSprite;
            blocker.color = new Color(0f, 0.02f, 0.06f, 0.72f);
            blocker.raycastTarget = true;
            var blockerButton = _cardOverlay.AddComponent<Button>();
            blockerButton.targetGraphic = blocker;
            blockerButton.transition = Selectable.Transition.None;
            blockerButton.onClick.AddListener(CloseMissionCard);

            var card = CreateImage("Campaign Mission Card", overlayRect, new Vector2(0.5f, 0.5f),
                new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), CardSize, Vector2.zero,
                CommonResourceRoot + "rect_card", true, Color.white);
            card.GetComponent<Image>().raycastTarget = true;

            _cardTitle = CreateText("Campaign Mission Card Title", card, new Vector2(0.1f, 1f),
                new Vector2(0.9f, 1f), new Vector2(0.5f, 1f), new Vector2(0f, 90f), CardTitlePosition,
                48, TextAlignmentOptions.Center, string.Empty);
            _cardTitle.color = SciFiTextStyle.GoldText;

            for (var i = 0; i < MaxMissionRows; i++)
            {
                var row = CreateRect($"Campaign Mission Row {i + 1}", card, new Vector2(0.5f, 1f),
                    new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(CardSize.x - 220f, MissionRowHeight),
                    new Vector2(MissionRowOffsetX, -(MissionRowTop + i * MissionRowSpacing)));

                var title = CreateText($"Campaign Mission Row {i + 1} Title", row, new Vector2(0f, 0f),
                    new Vector2(0.82f, 1f), new Vector2(0.5f, 0.5f), Vector2.zero, Vector2.zero,
                    30, TextAlignmentOptions.MidlineLeft, string.Empty);
                title.textWrappingMode = TextWrappingModes.Normal;
                title.enableAutoSizing = true;
                title.fontSizeMin = 22f;
                title.fontSizeMax = 30f;

                var check = CreateImage($"Campaign Mission Row {i + 1} Check", row, new Vector2(1f, 0.5f),
                    new Vector2(1f, 0.5f), new Vector2(1f, 0.5f), MissionCheckSize,
                    new Vector2(MissionCheckOffsetX, 0f),
                    CheckmarkResource, true, Color.white).GetComponent<Image>();
                check.enabled = false;

                row.gameObject.SetActive(false);
                _missionRows.Add(new MissionRowView { Root = row, Title = title, Check = check });
            }

            _cardEmptyLabel = CreateText("Campaign Mission Card Empty", card, new Vector2(0.12f, 0.42f),
                new Vector2(0.88f, 0.58f), new Vector2(0.5f, 0.5f), Vector2.zero, Vector2.zero,
                30, TextAlignmentOptions.Center, GameText.CampaignPlanetLocked());
            _cardEmptyLabel.textWrappingMode = TextWrappingModes.Normal;
            _cardEmptyLabel.color = SciFiTextStyle.SecondaryText;
            _cardEmptyLabel.gameObject.SetActive(false);

            var battleButton = CreateImageButton("Campaign Battle Button", card, new Vector2(0.5f, 0f),
                new Vector2(0.5f, 0f), new Vector2(0.5f, 0f), BattleButtonSize, new Vector2(0f, 90f),
                ShopResourceRoot + "btn_green", StartSelectedPlanetBattle);
            battleButton.GetComponent<Image>().preserveAspect = false;
            _battleButton = battleButton.GetComponent<Button>();
            CreateText("Campaign Battle Button Label", battleButton, Vector2.zero,
                Vector2.one, new Vector2(0.5f, 0.5f), Vector2.zero, Vector2.zero, 36,
                TextAlignmentOptions.Center, GameText.EnterBattle());

            _battleLockedLabel = CreateText("Campaign Battle Locked Label", card, new Vector2(0.5f, 0f),
                new Vector2(0.5f, 0f), new Vector2(0.5f, 0.5f), new Vector2(CardSize.x - 160f, BattleButtonSize.y),
                new Vector2(0f, 136f), 32, TextAlignmentOptions.Center, GameText.CampaignPreviousPlanetLocked());
            _battleLockedLabel.textWrappingMode = TextWrappingModes.Normal;
            _battleLockedLabel.color = SciFiTextStyle.WarningText;
            _battleLockedLabel.gameObject.SetActive(false);

            _cardOverlay.SetActive(false);
        }

        private void OpenMissionCard(int planetIndex)
        {
            if (planetIndex < 0 || planetIndex >= _planets.Count)
            {
                return;
            }

            _openPlanetIndex = planetIndex;
            var missions = _planets[planetIndex].MissionIndices;
            _cardTitle.text = GameText.Localized(
                CampaignPlanetCatalog.Planets[planetIndex].LocalizationKey,
                string.Empty);
            _cardEmptyLabel.gameObject.SetActive(missions.Count == 0);
            var currentMission = FindNextMission(missions);

            for (var i = 0; i < _missionRows.Count; i++)
            {
                var row = _missionRows[i];
                var hasMission = i < missions.Count;
                row.Root.gameObject.SetActive(hasMission);
                if (!hasMission)
                {
                    continue;
                }

                var missionIndex = missions[i];
                var completed = IsMissionCompleted(missionIndex);
                row.Title.text = $"{i + 1}. {ResolveMissionText(AppServices.Campaign?.GetMission(missionIndex))}";
                // Cleared missions read green, the one the player is on reads gold, and
                // anything beyond it stays neutral.
                row.Title.color = completed
                    ? SciFiTextStyle.SuccessText
                    : missionIndex == currentMission
                        ? SciFiTextStyle.GoldText
                        : SciFiTextStyle.MainText;
                row.Check.enabled = completed;
            }

            var nextMission = FindNextMission(missions);
            var canFight = nextMission >= 0
                && AppServices.Campaign != null
                && AppServices.Campaign.IsUnlocked(nextMission);
            // A planet the player has not reached yet swaps the button for the reason
            // it cannot be started, rather than showing a dead control.
            _battleButton.gameObject.SetActive(canFight);
            _battleLockedLabel.gameObject.SetActive(!canFight && missions.Count > 0);

            _cardOverlay.SetActive(true);
            _cardOverlay.transform.SetAsLastSibling();
        }

        /// <summary>
        /// What the mission actually asks of the player. The short title reads as a bare
        /// codename in a list, so the briefing text is shown instead, with the title kept
        /// only as a fallback for missions that carry no description.
        /// </summary>
        private static string ResolveMissionText(MissionDefinition mission)
        {
            if (mission == null)
            {
                return GameText.DefaultMissionDetails();
            }

            if (!string.IsNullOrWhiteSpace(mission.Description))
            {
                return GameText.MissionDescription(mission.MissionId, mission.Description);
            }

            return string.IsNullOrWhiteSpace(mission.Title)
                ? GameText.DefaultMissionDetails()
                : GameText.MissionTitle(mission.MissionId, mission.Title);
        }

        private void CloseMissionCard()
        {
            _openPlanetIndex = -1;
            _cardOverlay.SetActive(false);
        }

        private void StartSelectedPlanetBattle()
        {
            if (_openPlanetIndex < 0 || _openPlanetIndex >= _planets.Count)
            {
                return;
            }

            var nextMission = FindNextMission(_planets[_openPlanetIndex].MissionIndices);
            if (nextMission < 0 || AppServices.Campaign == null || !AppServices.Campaign.IsUnlocked(nextMission))
            {
                return;
            }

            AppServices.Session.SelectMission(nextMission);
            AppServices.SceneLoader.Load(GameSceneNames.Battle);
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
