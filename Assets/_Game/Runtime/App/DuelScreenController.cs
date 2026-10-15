using System;
using System.Text;
using Game.Core;
using Game.Core.Localization;
using Game.Core.Modes;
using Game.Gameplay.Battle.Weapons;
using Game.Progression.Modes;
using Game.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.App
{
    /// <summary>
    /// Duel queue and match card. The player picks nothing here: the opponent comes
    /// from the historical-power band and the arena from the anti-spoiler rule. The
    /// screen owns the search clock, ticking <see cref="IMatchmakingService"/> until
    /// it answers.
    /// </summary>
    public sealed class DuelScreenController : MonoBehaviour
    {
        private static readonly Color OverlayColor = new(0.04f, 0.06f, 0.08f, 0.92f);
        private static readonly Vector2 PanelSize = new(880f, 1220f);
        private static readonly Vector4 CardSliceBorder = new(220f, 220f, 220f, 220f);

        private Sprite _whiteSprite;
        private Action _onClosed;
        private MatchmakingRequest _request;
        private PvpBattleSetup _setup;
        private bool _searching;
        private TMP_Text _statusText;
        private TMP_Text _opponentText;
        private Button _startButton;
        private ResourceInfoPopupController _resourceInfoPopup;

        public static void Open(Action onClosed = null)
        {
            if (FindAnyObjectByType<DuelScreenController>() != null)
            {
                return;
            }

            var root = new GameObject("Duel Screen");
            root.AddComponent<DuelScreenController>()._onClosed = onClosed;
        }

        private void Awake()
        {
            _whiteSprite = Sprite.Create(Texture2D.whiteTexture, new Rect(0f, 0f, 1f, 1f), new Vector2(0.5f, 0.5f), 1f);
            Build();
        }

        private void OnDestroy()
        {
            if (_searching)
            {
                AppServices.Matchmaking?.CancelSearch();
            }
        }

        private void Update()
        {
            if (!_searching)
            {
                return;
            }

            var matchmaking = AppServices.Matchmaking;
            if (matchmaking == null)
            {
                return;
            }

            matchmaking.Tick(Time.unscaledDeltaTime);
            if (_searching && _statusText != null)
            {
                _statusText.text = GameText.DuelSearching(Mathf.FloorToInt(matchmaking.SearchElapsedSeconds));
            }
        }

        private void Build()
        {
            var canvasObject = new GameObject("Duel Canvas");
            canvasObject.transform.SetParent(transform, false);
            var canvas = canvasObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 120;

            var scaler = canvasObject.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1080f, 1920f);
            scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            scaler.matchWidthOrHeight = 0.5f;
            canvasObject.AddComponent<GraphicRaycaster>();

            var root = canvasObject.transform;
            _resourceInfoPopup = ResourceInfoPopupController.Create(root);
            var overlay = CreatePanel("Overlay", root, Vector2.zero, Vector2.one, new Vector2(0.5f, 0.5f),
                Vector2.zero, Vector2.zero, OverlayColor);
            overlay.raycastTarget = true;

            var panel = CreateSlicedPanel("Duel Panel", root, PanelSize, Vector2.zero);

            CreateText("Title", panel.transform, new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(0.5f, 1f),
                new Vector2(700f, 90f), new Vector2(0f, -120f), 52, TextAlignmentOptions.Center,
                GameText.DuelTitle(), GameTextRole.DisplayTitle);
            _statusText = CreateText("Status", panel.transform, new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(0.5f, 1f),
                new Vector2(700f, 120f), new Vector2(0f, -240f), 28, TextAlignmentOptions.Center, string.Empty);

            _opponentText = CreateText("Opponent", panel.transform, new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(0.5f, 1f),
                new Vector2(700f, 380f), new Vector2(0f, -380f), 26, TextAlignmentOptions.Top, string.Empty);

            _startButton = CreateImageButton("Start", panel.transform, "UI/Shop/btn_green", new Vector2(0f, -520f),
                GameText.EnterBattle(), StartDuel);
            _startButton.gameObject.SetActive(false);

            CreateImageButton("Back", panel.transform, "UI/Shop/btn_blue", new Vector2(0f, -660f),
                GameText.Back, Close);

            if (AppServices.Session == null || !AppServices.Session.AreRepeatableModesUnlocked)
            {
                _statusText.text = GameText.DuelLocked();
                return;
            }

            StartSearch();
        }

        private void StartSearch()
        {
            var matchmaking = AppServices.Matchmaking;
            if (matchmaking == null)
            {
                _statusText.text = GameText.DuelUnavailable();
                return;
            }

            _request = AppServices.Session.CreateMatchmakingRequest();
            _searching = true;
            _statusText.text = GameText.DuelSearching(0);
            matchmaking.FindOpponent(_request, OnMatchmakingCompleted);
        }

        private void OnMatchmakingCompleted(MatchmakingResult result)
        {
            _searching = false;
            if (!result.Matched)
            {
                _statusText.text = DescribeDenial(result.Denial);
                return;
            }

            _setup = AppServices.Session.PrepareDuel(_request, result);
            if (!_setup.IsAvailable)
            {
                // The pair shares no arena either side may legally see; refusing is
                // better than showing the lower commander a map they have not reached.
                _statusText.text = DescribeDenial(MatchmakingDenial.NoUnlockedArena);
                return;
            }

            _statusText.text = GameText.DuelMatchFound(
                GameText.ArenaName(_setup.ArenaId, GameText.ArenaFallback()));
            _opponentText.text = DescribeOpponent(_setup.Opponent);
            _startButton.gameObject.SetActive(true);
        }

        /// <summary>
        /// The opponent card. A backfilled opponent is described exactly like a live
        /// one - <see cref="PvpOpponentProfile.IsBotBackfill"/> is for telemetry and
        /// rating, never for this screen.
        /// </summary>
        private static string DescribeOpponent(PvpOpponentProfile opponent)
        {
            var builder = new StringBuilder();
            builder.AppendLine(opponent.DisplayName);
            builder.AppendLine(GameText.DuelOpponentLevel(opponent.CommanderLevel, opponent.HistoricalPower));
            builder.AppendLine();
            for (var i = 0; i < opponent.LoadoutWeaponIds.Length; i++)
            {
                var weaponId = opponent.LoadoutWeaponIds[i];
                var rank = i < opponent.LoadoutWeaponRanks.Length ? opponent.LoadoutWeaponRanks[i] : 1;
                var name = WeaponDisplayNames.Get(weaponId);
                builder.AppendLine(rank > 1 ? GameText.WeaponWithRank(name, rank) : name);
            }

            return builder.ToString().TrimEnd();
        }

        private static string DescribeDenial(MatchmakingDenial denial)
        {
            return denial switch
            {
                MatchmakingDenial.NoUnlockedArena => GameText.DuelLocked(),
                MatchmakingDenial.NoOpponentFound => GameText.DuelNoOpponent(),
                _ => GameText.DuelUnavailable()
            };
        }

        private void StartDuel()
        {
            if (!_setup.IsAvailable || !AppServices.Session.SelectDuel(_setup))
            {
                return;
            }

            _onClosed = null;
            AppServices.SceneLoader.Load(GameSceneNames.Battle);
        }

        private void Close()
        {
            if (_searching)
            {
                AppServices.Matchmaking?.CancelSearch();
                _searching = false;
            }

            var callback = _onClosed;
            _onClosed = null;
            Destroy(gameObject);
            callback?.Invoke();
        }

        private Image CreateSlicedPanel(string name, Transform parent, Vector2 size, Vector2 position)
        {
            var rect = CreateRect(name, parent, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                new Vector2(0.5f, 0.5f), size, position);
            var image = rect.gameObject.AddComponent<Image>();
            var texture = Resources.Load<Texture2D>("UI/Common/rect_card");
            image.sprite = texture != null
                ? Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height),
                    new Vector2(0.5f, 0.5f), 100f, 0u, SpriteMeshType.FullRect, CardSliceBorder)
                : _whiteSprite;
            image.type = Image.Type.Sliced;
            image.raycastTarget = true;
            return image;
        }

        private Image CreatePanel(string name, Transform parent, Vector2 anchorMin, Vector2 anchorMax, Vector2 pivot,
            Vector2 sizeDelta, Vector2 anchoredPosition, Color color)
        {
            var rect = CreateRect(name, parent, anchorMin, anchorMax, pivot, sizeDelta, anchoredPosition);
            var image = rect.gameObject.AddComponent<Image>();
            image.sprite = _whiteSprite;
            image.color = color;
            return image;
        }

        private Button CreateImageButton(string name, Transform parent, string resourcePath, Vector2 position,
            string label, Action onClick)
        {
            var rect = CreateRect(name, parent, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                new Vector2(0.5f, 0.5f), new Vector2(360f, 120f), position);
            var image = rect.gameObject.AddComponent<Image>();
            var texture = Resources.Load<Texture2D>(resourcePath);
            image.sprite = texture != null
                ? Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100f)
                : _whiteSprite;
            image.type = Image.Type.Simple;
            image.preserveAspect = true;
            var button = image.gameObject.AddComponent<Button>();
            button.targetGraphic = image;
            button.onClick.AddListener(() => onClick?.Invoke());
            CreateText($"{name} Label", image.transform, Vector2.zero, Vector2.one, new Vector2(0.5f, 0.5f),
                Vector2.zero, Vector2.zero, 30, TextAlignmentOptions.Center, label);
            return button;
        }

        private static TMP_Text CreateText(string name, Transform parent, Vector2 anchorMin, Vector2 anchorMax,
            Vector2 pivot, Vector2 sizeDelta, Vector2 anchoredPosition, int fontSize, TextAlignmentOptions alignment,
            string value, GameTextRole? roleOverride = null)
        {
            var rect = CreateRect(name, parent, anchorMin, anchorMax, pivot, sizeDelta, anchoredPosition);
            var text = rect.gameObject.AddComponent<TextMeshProUGUI>();
            var role = roleOverride ?? (fontSize >= 40 ? GameTextRole.ScreenTitle : GameTextRole.Body);
            SciFiTextStyle.Apply(text, role, fontSize, alignment);
            text.textWrappingMode = TextWrappingModes.Normal;
            text.overflowMode = TextOverflowModes.Overflow;
            text.raycastTarget = false;
            text.text = value;
            return text;
        }

        private static RectTransform CreateRect(string name, Transform parent, Vector2 anchorMin, Vector2 anchorMax,
            Vector2 pivot, Vector2 sizeDelta, Vector2 anchoredPosition)
        {
            var rectObject = new GameObject(name, typeof(RectTransform));
            var rect = (RectTransform)rectObject.transform;
            rect.SetParent(parent, false);
            rect.anchorMin = anchorMin;
            rect.anchorMax = anchorMax;
            rect.pivot = pivot;
            rect.sizeDelta = sizeDelta;
            rect.anchoredPosition = anchoredPosition;
            return rect;
        }
    }
}
