using System;
using Game.Core.Battle;
using Game.Core.Localization;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

namespace Game.UI.Battle
{
    /// <summary>
    /// Runtime-built uGUI battle HUD: top status/turn bar, per-team health bars,
    /// a power meter, and large touch-friendly action buttons. Attached to a parent
    /// transform by <see cref="BattleHudViewFactory"/> in the app composition root.
    /// </summary>
    public sealed class BattleHudView : MonoBehaviour, IBattleHud
    {
        private const float Margin = 28f;
        private const float TopBarHeight = 132f;
        private const float ButtonHeight = 168f;

        private static readonly Color PanelColor = new(0.08f, 0.11f, 0.16f, 0.72f);
        private static readonly Color BarBackColor = new(0f, 0f, 0f, 0.45f);
        private static readonly Color PlayerColor = new(0.24f, 0.62f, 0.98f);
        private static readonly Color OpponentColor = new(0.96f, 0.34f, 0.28f);
        private static readonly Color PowerColor = new(0.98f, 0.78f, 0.24f);
        private static readonly Color FireColor = new(0.22f, 0.66f, 0.34f);
        private static readonly Color NeutralButtonColor = new(0.2f, 0.26f, 0.34f);

        private Sprite _whiteSprite;

        private TMP_Text _statusText;
        private TMP_Text _turnText;
        private TMP_Text _playerHealthText;
        private TMP_Text _opponentHealthText;
        private TMP_Text _weaponText;
        private TMP_Text _powerText;
        private Image _playerHealthFill;
        private Image _opponentHealthFill;
        private Image _powerFill;
        private Button _fireButton;
        private TMP_Text _fireButtonLabel;
        private GameObject _resultRoot;
        private TMP_Text _resultTitleText;
        private TMP_Text _resultSubtitleText;
        private TMP_Text _resultOutcomeText;
        private TMP_Text _resultRewardText;
        private TMP_Text _resultUnlockText;
        private TMP_Text _resultContinueNoteText;
        private Button _resultContinueButton;
        private TMP_Text _resultRetryLabel;
        private TMP_Text _resultContinueLabel;

        public event Action FireRequested;
        public event Action CancelRequested;
        public event Action NextWeaponRequested;
        public event Action RestartRequested;
        public event Action ContinueRequested;
        public event Action MenuRequested;

        private void Awake()
        {
            _whiteSprite = Sprite.Create(Texture2D.whiteTexture, new Rect(0f, 0f, 1f, 1f), new Vector2(0.5f, 0.5f), 1f);
            EnsureEventSystem();
            BuildCanvas();
        }

        public void Render(in BattleHudState state)
        {
            if (_statusText == null)
            {
                return;
            }

            _statusText.text = state.Status;
            _turnText.text = GameText.BattleTurn(state.TurnNumber, state.ActiveTeam);

            _playerHealthText.text = GameText.TeamHealth(GameText.PlayerTeam, state.PlayerHealth, state.PlayerMaxHealth);
            _opponentHealthText.text = GameText.TeamHealth(GameText.OpponentTeam, state.OpponentHealth, state.OpponentMaxHealth);
            _playerHealthFill.fillAmount = state.PlayerHealthFraction;
            _opponentHealthFill.fillAmount = state.OpponentHealthFraction;

            _weaponText.text = $"{state.WeaponLabel}\n{state.AmmoLabel}";
            _powerText.text = GameText.Power(state.Power);
            _powerFill.fillAmount = state.PowerFraction;

            _fireButton.interactable = state.CanFire;
            _fireButtonLabel.color = state.CanFire ? Color.white : new Color(1f, 1f, 1f, 0.4f);

            RenderResult(state);
        }

        private static void EnsureEventSystem()
        {
            if (FindAnyObjectByType<EventSystem>() != null)
            {
                return;
            }

            var eventSystem = new GameObject("Battle EventSystem");
            eventSystem.AddComponent<EventSystem>();
            eventSystem.AddComponent<InputSystemUIInputModule>();
        }

        private void BuildCanvas()
        {
            var canvasObject = new GameObject("Battle HUD Canvas");
            canvasObject.transform.SetParent(transform, false);

            var canvas = canvasObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 100;

            var scaler = canvasObject.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1080f, 1920f);
            scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            scaler.matchWidthOrHeight = 0.5f;

            canvasObject.AddComponent<GraphicRaycaster>();

            var root = canvasObject.transform;

            BuildTopBar(root);
            BuildHealthBars(root);
            BuildPowerMeter(root);
            BuildActionButtons(root);
            BuildResultOverlay(root);
        }

        private void BuildTopBar(Transform root)
        {
            var bar = CreatePanel("Top Bar", root, new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(0.5f, 1f),
                new Vector2(-2f * Margin, TopBarHeight), new Vector2(0f, -Margin), PanelColor);

            _statusText = CreateText("Status", bar, new Vector2(0f, 0f), new Vector2(1f, 1f), new Vector2(0f, 0f),
                new Vector2(-40f, -12f), new Vector2(20f, 6f), 34, TextAlignmentOptions.TopLeft);
            _turnText = CreateText("Turn", bar, new Vector2(0f, 0f), new Vector2(1f, 1f), new Vector2(0f, 0f),
                new Vector2(-40f, -12f), new Vector2(20f, 6f), 30, TextAlignmentOptions.BottomLeft);

            var restart = CreateButton("Restart", bar, new Vector2(1f, 1f), new Vector2(1f, 1f),
                new Vector2(176f, 72f), new Vector2(-16f, -16f), NeutralButtonColor, GameText.Restart, 24,
                () => RestartRequested?.Invoke());
            _ = restart;

            var menu = CreateButton("Menu", bar, new Vector2(1f, 1f), new Vector2(1f, 1f),
                new Vector2(140f, 72f), new Vector2(-(16f + 176f + 12f), -16f), new Color(0.30f, 0.20f, 0.22f), GameText.Menu, 24,
                () => MenuRequested?.Invoke());
            _ = menu;
        }

        private void BuildHealthBars(Transform root)
        {
            var topOffset = -(TopBarHeight + 2f * Margin);

            var playerBack = CreateAnchoredPanel("Player HP", root, new Vector2(0f, 1f),
                new Vector2(Margin, topOffset), new Vector2(440f, 74f), BarBackColor);
            _playerHealthFill = CreateBarFill("Player HP Fill", playerBack, PlayerColor);
            _playerHealthText = CreateText("Player HP Text", playerBack, Vector2.zero, Vector2.one, new Vector2(0.5f, 0.5f),
                new Vector2(-24f, 0f), Vector2.zero, 28, TextAlignmentOptions.MidlineLeft);

            var opponentBack = CreateAnchoredPanel("Enemy HP", root, new Vector2(1f, 1f),
                new Vector2(-Margin, topOffset), new Vector2(440f, 74f), BarBackColor);
            _opponentHealthFill = CreateBarFill("Enemy HP Fill", opponentBack, OpponentColor);
            _opponentHealthText = CreateText("Enemy HP Text", opponentBack, Vector2.zero, Vector2.one, new Vector2(0.5f, 0.5f),
                new Vector2(-24f, 0f), Vector2.zero, 28, TextAlignmentOptions.MidlineRight);
        }

        private void BuildPowerMeter(Transform root)
        {
            var back = CreateAnchoredPanel("Power Meter", root, new Vector2(0.5f, 0f),
                new Vector2(0f, Margin + ButtonHeight + 40f), new Vector2(680f, 56f), BarBackColor);
            _powerFill = CreateBarFill("Power Fill", back, PowerColor);
            _powerText = CreateText("Power Text", back, Vector2.zero, Vector2.one, new Vector2(0.5f, 0.5f),
                Vector2.zero, Vector2.zero, 28, TextAlignmentOptions.Center);
        }

        private void BuildActionButtons(Transform root)
        {
            var weapon = CreateButton("Weapon", root, new Vector2(0f, 0f), new Vector2(0f, 0f),
                new Vector2(320f, ButtonHeight), new Vector2(Margin, Margin), NeutralButtonColor, GameText.Weapon, 30,
                () => NextWeaponRequested?.Invoke());
            _weaponText = weapon.GetComponentInChildren<TMP_Text>();

            var cancel = CreateButton("Cancel", root, new Vector2(1f, 0f), new Vector2(1f, 0f),
                new Vector2(240f, ButtonHeight), new Vector2(-(Margin + 320f + 20f), Margin), NeutralButtonColor, GameText.CancelAim, 28,
                () => CancelRequested?.Invoke());
            _ = cancel;

            _fireButton = CreateButton("Fire", root, new Vector2(1f, 0f), new Vector2(1f, 0f),
                new Vector2(320f, ButtonHeight), new Vector2(-Margin, Margin), FireColor, GameText.Fire, 40,
                () => FireRequested?.Invoke());
            _fireButtonLabel = _fireButton.GetComponentInChildren<TMP_Text>();
        }

        private void BuildResultOverlay(Transform root)
        {
            var blocker = CreatePanel("Result Overlay", root, Vector2.zero, Vector2.one, new Vector2(0.5f, 0.5f),
                Vector2.zero, Vector2.zero, new Color(0f, 0f, 0f, 0.42f));
            _resultRoot = blocker.gameObject;

            var panel = CreateAnchoredPanel("Result Panel", blocker.transform, new Vector2(0.5f, 0.5f),
                Vector2.zero, new Vector2(680f, 440f), new Color(0.08f, 0.11f, 0.16f, 0.92f));

            _resultTitleText = CreateText("Result Title", panel.transform, new Vector2(0f, 1f), new Vector2(1f, 1f),
                new Vector2(0.5f, 1f), new Vector2(-56f, 64f), new Vector2(0f, -30f), 46, TextAlignmentOptions.Center);
            _resultSubtitleText = CreateText("Result Subtitle", panel.transform, new Vector2(0f, 1f), new Vector2(1f, 1f),
                new Vector2(0.5f, 1f), new Vector2(-56f, 48f), new Vector2(0f, -102f), 28, TextAlignmentOptions.Center);
            _resultOutcomeText = CreateText("Result Outcome", panel.transform, new Vector2(0f, 1f), new Vector2(1f, 1f),
                new Vector2(0.5f, 1f), new Vector2(-56f, 42f), new Vector2(0f, -158f), 26, TextAlignmentOptions.Center);
            _resultRewardText = CreateText("Result Reward", panel.transform, new Vector2(0f, 1f), new Vector2(1f, 1f),
                new Vector2(0.5f, 1f), new Vector2(-56f, 42f), new Vector2(0f, -204f), 26, TextAlignmentOptions.Center);
            _resultUnlockText = CreateText("Result Unlock", panel.transform, new Vector2(0f, 1f), new Vector2(1f, 1f),
                new Vector2(0.5f, 1f), new Vector2(-56f, 42f), new Vector2(0f, -250f), 24, TextAlignmentOptions.Center);
            _resultContinueNoteText = CreateText("Result Continue Note", panel.transform, new Vector2(0f, 1f), new Vector2(1f, 1f),
                new Vector2(0.5f, 1f), new Vector2(-56f, 36f), new Vector2(0f, -292f), 20, TextAlignmentOptions.Center);

            var retry = CreateButton("Result Retry", panel.transform, new Vector2(0f, 0f), new Vector2(0f, 0f),
                new Vector2(280f, 74f), new Vector2(36f, 34f), NeutralButtonColor, GameText.Restart, 30,
                () => RestartRequested?.Invoke());
            _resultRetryLabel = retry.GetComponentInChildren<TMP_Text>();

            _resultContinueButton = CreateButton("Result Continue", panel.transform, new Vector2(1f, 0f), new Vector2(1f, 0f),
                new Vector2(280f, 74f), new Vector2(-36f, 34f), FireColor, GameText.Continue, 32,
                () => ContinueRequested?.Invoke());
            _resultContinueLabel = _resultContinueButton.GetComponentInChildren<TMP_Text>();

            _resultRoot.SetActive(false);
        }

        private void RenderResult(in BattleHudState state)
        {
            if (_resultRoot == null)
            {
                return;
            }

            _resultRoot.SetActive(state.ShowResult);
            if (!state.ShowResult)
            {
                return;
            }

            _resultTitleText.text = state.ResultTitle;
            _resultSubtitleText.text = state.ResultSubtitle;
            _resultOutcomeText.text = state.ResultOutcome;
            _resultRewardText.text = state.ResultReward;
            _resultUnlockText.text = state.ResultUnlock;
            _resultContinueNoteText.text = state.CanContinue ? GameText.NextMissionUnlocked : GameText.RetryOrMenu;
            _resultRetryLabel.text = state.RetryLabel;
            _resultContinueLabel.text = state.ContinueLabel;
            // Right button always leaves the battle: continue (win) or back to menu (loss).
            _resultContinueButton.interactable = true;
            _resultContinueLabel.color = Color.white;
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

        private Image CreateAnchoredPanel(string name, Component parent, Vector2 anchor,
            Vector2 anchoredPosition, Vector2 size, Color color)
        {
            return CreatePanel(name, parent, anchor, anchor, anchor, size, anchoredPosition, color);
        }

        private Image CreateBarFill(string name, Image background, Color color)
        {
            var rect = CreateRect(name, background.transform, Vector2.zero, Vector2.one, new Vector2(0.5f, 0.5f),
                Vector2.zero, Vector2.zero);
            var image = rect.gameObject.AddComponent<Image>();
            image.sprite = _whiteSprite;
            image.color = color;
            image.type = Image.Type.Filled;
            image.fillMethod = Image.FillMethod.Horizontal;
            image.fillOrigin = (int)Image.OriginHorizontal.Left;
            image.fillAmount = 1f;
            return image;
        }

        private TMP_Text CreateText(string name, Component parent, Vector2 anchorMin, Vector2 anchorMax, Vector2 pivot,
            Vector2 sizeDelta, Vector2 anchoredPosition, int fontSize, TextAlignmentOptions alignment)
        {
            var rect = CreateRect(name, parent, anchorMin, anchorMax, pivot, sizeDelta, anchoredPosition);
            var text = rect.gameObject.AddComponent<TextMeshProUGUI>();
            text.fontSize = fontSize;
            text.color = Color.white;
            text.alignment = alignment;
            text.textWrappingMode = TextWrappingModes.Normal;
            text.overflowMode = TextOverflowModes.Overflow;
            text.raycastTarget = false;
            return text;
        }

        private Button CreateButton(string name, Component parent, Vector2 anchorMin, Vector2 anchorMax,
            Vector2 size, Vector2 anchoredPosition, Color color, string label, int fontSize, Action onClick)
        {
            var pivot = new Vector2(anchorMin.x, anchorMin.y);
            var image = CreatePanel(name, parent, anchorMin, anchorMax, pivot, size, anchoredPosition, color);
            var button = image.gameObject.AddComponent<Button>();
            button.targetGraphic = image;
            button.onClick.AddListener(() => onClick?.Invoke());

            var text = CreateText($"{name} Label", image.transform, Vector2.zero, Vector2.one, new Vector2(0.5f, 0.5f),
                Vector2.zero, Vector2.zero, fontSize, TextAlignmentOptions.Center);
            text.text = label;
            return button;
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

    /// <summary>Default factory implementation that attaches a <see cref="BattleHudView"/> under the given parent.</summary>
    public sealed class BattleHudViewFactory : IBattleHudFactory
    {
        public IBattleHud Create(Transform parent)
        {
            var hudObject = new GameObject("Battle HUD");
            hudObject.transform.SetParent(parent, false);
            return hudObject.AddComponent<BattleHudView>();
        }
    }
}
