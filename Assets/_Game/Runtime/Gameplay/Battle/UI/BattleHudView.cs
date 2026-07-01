using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

namespace Game.Gameplay.Battle.UI
{
    /// <summary>
    /// Runtime-built uGUI battle HUD: top status/turn bar, per-team health bars,
    /// a power meter, and large touch-friendly action buttons.
    /// Anchored to screen corners so panels never overlap across mobile aspect ratios.
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
        private Font _font;

        private Text _statusText;
        private Text _turnText;
        private Text _playerHealthText;
        private Text _opponentHealthText;
        private Text _weaponText;
        private Text _powerText;
        private Image _playerHealthFill;
        private Image _opponentHealthFill;
        private Image _powerFill;
        private Button _fireButton;
        private Text _fireButtonLabel;
        private GameObject _resultRoot;
        private Text _resultTitleText;
        private Text _resultSubtitleText;
        private Text _resultOutcomeText;
        private Text _resultRewardText;
        private Text _resultUnlockText;
        private Text _resultContinueNoteText;
        private Button _resultContinueButton;
        private Text _resultRetryLabel;
        private Text _resultContinueLabel;

        public event Action FireRequested;
        public event Action CancelRequested;
        public event Action NextWeaponRequested;
        public event Action RestartRequested;
        public event Action ContinueRequested;

        private void Awake()
        {
            _whiteSprite = Sprite.Create(Texture2D.whiteTexture, new Rect(0f, 0f, 1f, 1f), new Vector2(0.5f, 0.5f), 1f);
            _font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            if (_font == null)
            {
                _font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            }

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
            _turnText.text = $"Хід {state.TurnNumber} — {state.ActiveTeam}";

            _playerHealthText.text = $"Гравець {state.PlayerHealth}/{state.PlayerMaxHealth}";
            _opponentHealthText.text = $"Суперник {state.OpponentHealth}/{state.OpponentMaxHealth}";
            _playerHealthFill.fillAmount = state.PlayerHealthFraction;
            _opponentHealthFill.fillAmount = state.OpponentHealthFraction;

            _weaponText.text = $"{state.WeaponLabel}\n{state.AmmoLabel}";
            _powerText.text = $"Сила {state.Power:0.0}";
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
                new Vector2(-40f, -12f), new Vector2(20f, 6f), 34, TextAnchor.UpperLeft);
            _turnText = CreateText("Turn", bar, new Vector2(0f, 0f), new Vector2(1f, 1f), new Vector2(0f, 0f),
                new Vector2(-40f, -12f), new Vector2(20f, 6f), 30, TextAnchor.LowerLeft);

            var restart = CreateButton("Restart", bar, new Vector2(1f, 1f), new Vector2(1f, 1f),
                new Vector2(180f, 72f), new Vector2(-16f, -16f), NeutralButtonColor, "Рестарт", 24,
                () => RestartRequested?.Invoke());
            _ = restart;
        }

        private void BuildHealthBars(Transform root)
        {
            var topOffset = -(TopBarHeight + 2f * Margin);

            var playerBack = CreateAnchoredPanel("Player HP", root, new Vector2(0f, 1f),
                new Vector2(Margin, topOffset), new Vector2(440f, 74f), BarBackColor);
            _playerHealthFill = CreateBarFill("Player HP Fill", playerBack, PlayerColor);
            _playerHealthText = CreateText("Player HP Text", playerBack, Vector2.zero, Vector2.one, new Vector2(0.5f, 0.5f),
                new Vector2(-24f, 0f), Vector2.zero, 28, TextAnchor.MiddleLeft);

            var opponentBack = CreateAnchoredPanel("Enemy HP", root, new Vector2(1f, 1f),
                new Vector2(-Margin, topOffset), new Vector2(440f, 74f), BarBackColor);
            _opponentHealthFill = CreateBarFill("Enemy HP Fill", opponentBack, OpponentColor);
            _opponentHealthText = CreateText("Enemy HP Text", opponentBack, Vector2.zero, Vector2.one, new Vector2(0.5f, 0.5f),
                new Vector2(-24f, 0f), Vector2.zero, 28, TextAnchor.MiddleRight);
        }

        private void BuildPowerMeter(Transform root)
        {
            var back = CreateAnchoredPanel("Power Meter", root, new Vector2(0.5f, 0f),
                new Vector2(0f, Margin + ButtonHeight + 40f), new Vector2(680f, 56f), BarBackColor);
            _powerFill = CreateBarFill("Power Fill", back, PowerColor);
            _powerText = CreateText("Power Text", back, Vector2.zero, Vector2.one, new Vector2(0.5f, 0.5f),
                Vector2.zero, Vector2.zero, 28, TextAnchor.MiddleCenter);
        }

        private void BuildActionButtons(Transform root)
        {
            var weapon = CreateButton("Weapon", root, new Vector2(0f, 0f), new Vector2(0f, 0f),
                new Vector2(320f, ButtonHeight), new Vector2(Margin, Margin), NeutralButtonColor, "Зброя", 30,
                () => NextWeaponRequested?.Invoke());
            _weaponText = weapon.GetComponentInChildren<Text>();

            var cancel = CreateButton("Cancel", root, new Vector2(1f, 0f), new Vector2(1f, 0f),
                new Vector2(240f, ButtonHeight), new Vector2(-(Margin + 320f + 20f), Margin), NeutralButtonColor, "Скинути", 28,
                () => CancelRequested?.Invoke());
            _ = cancel;

            _fireButton = CreateButton("Fire", root, new Vector2(1f, 0f), new Vector2(1f, 0f),
                new Vector2(320f, ButtonHeight), new Vector2(-Margin, Margin), FireColor, "ВОГОНЬ", 40,
                () => FireRequested?.Invoke());
            _fireButtonLabel = _fireButton.GetComponentInChildren<Text>();
        }

        private void BuildResultOverlay(Transform root)
        {
            var blocker = CreatePanel("Result Overlay", root, Vector2.zero, Vector2.one, new Vector2(0.5f, 0.5f),
                Vector2.zero, Vector2.zero, new Color(0f, 0f, 0f, 0.42f));
            _resultRoot = blocker.gameObject;

            var panel = CreateAnchoredPanel("Result Panel", blocker.transform, new Vector2(0.5f, 0.5f),
                Vector2.zero, new Vector2(680f, 440f), new Color(0.08f, 0.11f, 0.16f, 0.92f));

            _resultTitleText = CreateText("Result Title", panel.transform, new Vector2(0f, 1f), new Vector2(1f, 1f),
                new Vector2(0.5f, 1f), new Vector2(-56f, 64f), new Vector2(0f, -30f), 46, TextAnchor.MiddleCenter);
            _resultSubtitleText = CreateText("Result Subtitle", panel.transform, new Vector2(0f, 1f), new Vector2(1f, 1f),
                new Vector2(0.5f, 1f), new Vector2(-56f, 48f), new Vector2(0f, -102f), 28, TextAnchor.MiddleCenter);
            _resultOutcomeText = CreateText("Result Outcome", panel.transform, new Vector2(0f, 1f), new Vector2(1f, 1f),
                new Vector2(0.5f, 1f), new Vector2(-56f, 42f), new Vector2(0f, -158f), 26, TextAnchor.MiddleCenter);
            _resultRewardText = CreateText("Result Reward", panel.transform, new Vector2(0f, 1f), new Vector2(1f, 1f),
                new Vector2(0.5f, 1f), new Vector2(-56f, 42f), new Vector2(0f, -204f), 26, TextAnchor.MiddleCenter);
            _resultUnlockText = CreateText("Result Unlock", panel.transform, new Vector2(0f, 1f), new Vector2(1f, 1f),
                new Vector2(0.5f, 1f), new Vector2(-56f, 42f), new Vector2(0f, -250f), 24, TextAnchor.MiddleCenter);
            _resultContinueNoteText = CreateText("Result Continue Note", panel.transform, new Vector2(0f, 1f), new Vector2(1f, 1f),
                new Vector2(0.5f, 1f), new Vector2(-56f, 36f), new Vector2(0f, -292f), 20, TextAnchor.MiddleCenter);

            var retry = CreateButton("Result Retry", panel.transform, new Vector2(0f, 0f), new Vector2(0f, 0f),
                new Vector2(280f, 74f), new Vector2(36f, 34f), NeutralButtonColor, "Повторити", 30,
                () => RestartRequested?.Invoke());
            _resultRetryLabel = retry.GetComponentInChildren<Text>();

            _resultContinueButton = CreateButton("Result Continue", panel.transform, new Vector2(1f, 0f), new Vector2(1f, 0f),
                new Vector2(280f, 74f), new Vector2(-36f, 34f), FireColor, "Далі", 32,
                () => ContinueRequested?.Invoke());
            _resultContinueLabel = _resultContinueButton.GetComponentInChildren<Text>();

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
            _resultContinueNoteText.text = state.CanContinue ? "Наступна арена готова." : "Спробуй ще раз і покращ результат.";
            _resultRetryLabel.text = state.RetryLabel;
            _resultContinueLabel.text = state.ContinueLabel;
            _resultContinueButton.interactable = state.CanContinue;
            _resultContinueLabel.color = state.CanContinue ? Color.white : new Color(1f, 1f, 1f, 0.4f);
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

        private Text CreateText(string name, Component parent, Vector2 anchorMin, Vector2 anchorMax, Vector2 pivot,
            Vector2 sizeDelta, Vector2 anchoredPosition, int fontSize, TextAnchor alignment)
        {
            var rect = CreateRect(name, parent, anchorMin, anchorMax, pivot, sizeDelta, anchoredPosition);
            var text = rect.gameObject.AddComponent<Text>();
            text.font = _font;
            text.fontSize = fontSize;
            text.color = Color.white;
            text.alignment = alignment;
            text.horizontalOverflow = HorizontalWrapMode.Overflow;
            text.verticalOverflow = VerticalWrapMode.Overflow;
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
                Vector2.zero, Vector2.zero, fontSize, TextAnchor.MiddleCenter);
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
}
