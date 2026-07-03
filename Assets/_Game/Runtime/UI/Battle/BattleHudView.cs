using System;
using System.Collections.Generic;
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
    /// Runtime-built uGUI battle HUD: top status/turn bar, bottom movement controls,
    /// weapon info, and large touch-friendly action buttons.
    /// Attached to a parent transform by <see cref="BattleHudViewFactory"/> in the
    /// app composition root.
    /// </summary>
    public sealed class BattleHudView : MonoBehaviour, IBattleHud
    {
        private const float Margin = 28f;
        private const float TopBarHeight = 132f;
        private const float ButtonHeight = 168f;

        private static readonly Color PanelColor = new(0.08f, 0.11f, 0.16f, 0.72f);
        private static readonly Color FireColor = new(0.22f, 0.66f, 0.34f);
        private static readonly Color NeutralButtonColor = new(0.2f, 0.26f, 0.34f);
        private static readonly Color MoveButtonColor = new(0.18f, 0.35f, 0.56f);
        private static readonly Color JumpBackColor = new(0.43f, 0.31f, 0.18f);
        private static readonly Color JumpForwardColor = new(0.30f, 0.49f, 0.20f);
        private static readonly Color TutorialHighlightColor = new(1f, 0.84f, 0.28f, 0.98f);

        private Sprite _whiteSprite;

        private TMP_Text _statusText;
        private TMP_Text _turnText;
        private TMP_Text _weaponText;
        private TMP_Text _mobilityText;
        private Image _topBarPanel;
        private Button _weaponButton;
        private Image _weaponButtonPanel;
        private Button _moveLeftButton;
        private Image _moveLeftButtonPanel;
        private Button _moveRightButton;
        private Image _moveRightButtonPanel;
        private Button _jumpBackButton;
        private Image _jumpBackButtonPanel;
        private Button _jumpForwardButton;
        private Image _jumpForwardButtonPanel;
        private Button _cancelButton;
        private Button _fireButton;
        private Image _fireButtonPanel;
        private TMP_Text _fireButtonLabel;
        private GameObject _resultRoot;
        private Image _resultPanel;
        private TMP_Text _resultTitleText;
        private TMP_Text _resultSubtitleText;
        private TMP_Text _resultOutcomeText;
        private TMP_Text _resultRewardText;
        private TMP_Text _resultUnlockText;
        private TMP_Text _resultContinueNoteText;
        private GameObject _aimGuideRoot;
        private TMP_Text _aimGuideText;
        private Button _resultContinueButton;
        private TMP_Text _resultRetryLabel;
        private TMP_Text _resultContinueLabel;

        public event Action FireRequested;
        public event Action CancelRequested;
        public event Action NextWeaponRequested;
        public event Action MoveLeftStarted;
        public event Action MoveLeftStopped;
        public event Action MoveRightStarted;
        public event Action MoveRightStopped;
        public event Action JumpForwardRequested;
        public event Action JumpBackwardRequested;
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

            _weaponText.text = $"{state.WeaponLabel}\n{GameText.WeaponType(state.WeaponRoleLabel)} | {state.AmmoLabel}\n{state.WeaponHint}";
            _mobilityText.text = state.MobilityLabel;

            if (_weaponButton != null)
            {
                _weaponButton.interactable = !state.TutorialBlocksWeaponSwitch;
            }

            if (_cancelButton != null)
            {
                _cancelButton.interactable = !state.TutorialBlocksCancel;
            }

            _moveLeftButton.interactable = state.CanMoveLeft && !state.TutorialBlocksMovement;
            _moveRightButton.interactable = state.CanMoveRight && !state.TutorialBlocksMovement;
            _jumpBackButton.interactable = state.CanJumpBackward && !state.TutorialBlocksMovement;
            _jumpForwardButton.interactable = state.CanJumpForward && !state.TutorialBlocksMovement;

            _fireButton.interactable = state.CanFire;
            _fireButtonLabel.color = state.CanFire ? Color.white : new Color(1f, 1f, 1f, 0.4f);
            RenderTutorialHighlight(state.TutorialTargetId, state.Status);

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
            BuildMobilityPanel(root);
            BuildActionButtons(root);
            BuildResultOverlay(root);
        }

        private void BuildTopBar(Transform root)
        {
            var bar = CreatePanel("Top Bar", root, new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(0.5f, 1f),
                new Vector2(-2f * Margin, TopBarHeight), new Vector2(0f, -Margin), PanelColor);
            _topBarPanel = bar;

            _statusText = CreateText("Status", bar, new Vector2(0f, 0f), new Vector2(1f, 1f), new Vector2(0f, 0f),
                new Vector2(-40f, -12f), new Vector2(20f, 6f), 34, TextAlignmentOptions.TopLeft);
            _turnText = CreateText("Turn", bar, new Vector2(0f, 0f), new Vector2(1f, 1f), new Vector2(0f, 0f),
                new Vector2(-40f, -12f), new Vector2(20f, 6f), 30, TextAlignmentOptions.BottomLeft);

            _ = CreateButton("Restart", bar, new Vector2(1f, 1f), new Vector2(1f, 1f),
                new Vector2(176f, 72f), new Vector2(-16f, -16f), NeutralButtonColor, GameText.Restart, 24,
                () => RestartRequested?.Invoke());

            _ = CreateButton("Menu", bar, new Vector2(1f, 1f), new Vector2(1f, 1f),
                new Vector2(140f, 72f), new Vector2(-(16f + 176f + 12f), -16f), new Color(0.30f, 0.20f, 0.22f), GameText.Menu, 24,
                () => MenuRequested?.Invoke());
        }

        private void BuildMobilityPanel(Transform root)
        {
            var panel = CreateAnchoredPanel("Mobility Panel", root, new Vector2(0.5f, 0f),
                new Vector2(0f, Margin), new Vector2(560f, ButtonHeight), new Color(0.08f, 0.11f, 0.16f, 0.28f));

            _mobilityText = CreateText("Mobility Text", panel.transform, new Vector2(0f, 1f), new Vector2(1f, 1f),
                new Vector2(0.5f, 1f), new Vector2(-24f, 34f), new Vector2(0f, -8f), 20, TextAlignmentOptions.Center);

            _jumpBackButton = CreateButton("Jump Back", panel.transform, new Vector2(0f, 0f), new Vector2(0f, 0f),
                new Vector2(144f, 72f), new Vector2(16f, 26f), JumpBackColor, GameText.JumpBack(), 20,
                () => JumpBackwardRequested?.Invoke());
            _jumpBackButtonPanel = _jumpBackButton.GetComponent<Image>();

            _moveLeftButton = CreateButton("Move Left", panel.transform, new Vector2(0f, 0f), new Vector2(0f, 0f),
                new Vector2(86f, 72f), new Vector2(174f, 26f), MoveButtonColor, GameText.WalkLeft, 28, () => { });
            _moveLeftButtonPanel = _moveLeftButton.GetComponent<Image>();
            WireHold(_moveLeftButton, () => MoveLeftStarted?.Invoke(), () => MoveLeftStopped?.Invoke());

            _moveRightButton = CreateButton("Move Right", panel.transform, new Vector2(0f, 0f), new Vector2(0f, 0f),
                new Vector2(86f, 72f), new Vector2(272f, 26f), MoveButtonColor, GameText.WalkRight, 28, () => { });
            _moveRightButtonPanel = _moveRightButton.GetComponent<Image>();
            WireHold(_moveRightButton, () => MoveRightStarted?.Invoke(), () => MoveRightStopped?.Invoke());

            _jumpForwardButton = CreateButton("Jump Forward", panel.transform, new Vector2(0f, 0f), new Vector2(0f, 0f),
                new Vector2(144f, 72f), new Vector2(372f, 26f), JumpForwardColor, GameText.JumpForward(), 20,
                () => JumpForwardRequested?.Invoke());
            _jumpForwardButtonPanel = _jumpForwardButton.GetComponent<Image>();
        }

        private void BuildActionButtons(Transform root)
        {
            var weapon = CreateButton("Weapon", root, new Vector2(0f, 0f), new Vector2(0f, 0f),
                new Vector2(300f, ButtonHeight), new Vector2(Margin, Margin), NeutralButtonColor, GameText.Weapon, 30,
                () => NextWeaponRequested?.Invoke());
            _weaponButton = weapon;
            _weaponButtonPanel = weapon.GetComponent<Image>();
            _weaponText = weapon.GetComponentInChildren<TMP_Text>();
            _weaponText.fontSize = 21;

            var cancel = CreateButton("Cancel", root, new Vector2(1f, 0f), new Vector2(1f, 0f),
                new Vector2(220f, ButtonHeight), new Vector2(-(Margin + 300f + 20f), Margin), NeutralButtonColor, GameText.CancelAim, 28,
                () => CancelRequested?.Invoke());
            _cancelButton = cancel;

            _fireButton = CreateButton("Fire", root, new Vector2(1f, 0f), new Vector2(1f, 0f),
                new Vector2(300f, ButtonHeight), new Vector2(-Margin, Margin), FireColor, GameText.Fire, 40,
                () => FireRequested?.Invoke());
            _fireButtonPanel = _fireButton.GetComponent<Image>();
            _fireButtonLabel = _fireButton.GetComponentInChildren<TMP_Text>();

            _aimGuideRoot = CreateAnchoredPanel("Aim Guide", root, new Vector2(0.5f, 0.5f),
                new Vector2(0f, -180f), new Vector2(720f, 90f), new Color(0.10f, 0.14f, 0.20f, 0.74f)).gameObject;
            _aimGuideText = CreateText("Aim Guide Text", _aimGuideRoot.transform, Vector2.zero, Vector2.one, new Vector2(0.5f, 0.5f),
                new Vector2(-32f, -16f), Vector2.zero, 24, TextAlignmentOptions.Center);
            _aimGuideRoot.SetActive(false);
        }

        private void BuildResultOverlay(Transform root)
        {
            var blocker = CreatePanel("Result Overlay", root, Vector2.zero, Vector2.one, new Vector2(0.5f, 0.5f),
                Vector2.zero, Vector2.zero, new Color(0f, 0f, 0f, 0.42f));
            _resultRoot = blocker.gameObject;

            var panel = CreateAnchoredPanel("Result Panel", blocker.transform, new Vector2(0.5f, 0.5f),
                Vector2.zero, new Vector2(680f, 440f), new Color(0.08f, 0.11f, 0.16f, 0.92f));
            _resultPanel = panel;

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
            _resultContinueButton.interactable = true;
            _resultContinueLabel.color = Color.white;
        }

        private void RenderTutorialHighlight(string tutorialTargetId, string tutorialMessage)
        {
            var pulse = 0.3f + Mathf.PingPong(Time.unscaledTime * 0.9f, 0.45f);
            ApplyHighlight(_topBarPanel, TargetsStatus(tutorialTargetId), PanelColor, pulse);
            ApplyHighlight(_weaponButtonPanel, tutorialTargetId == "battle.weapon-strip", NeutralButtonColor, pulse);
            ApplyHighlight(_moveLeftButtonPanel, tutorialTargetId == "battle.move-controls", MoveButtonColor, pulse);
            ApplyHighlight(_moveRightButtonPanel, tutorialTargetId == "battle.move-controls", MoveButtonColor, pulse);
            ApplyHighlight(_jumpBackButtonPanel, tutorialTargetId == "battle.move-controls", JumpBackColor, pulse);
            ApplyHighlight(_jumpForwardButtonPanel, tutorialTargetId == "battle.move-controls", JumpForwardColor, pulse);
            ApplyHighlight(_fireButtonPanel, tutorialTargetId == "battle.fire-button", FireColor, pulse);
            ApplyHighlight(_resultPanel, tutorialTargetId == "battle.result", new Color(0.08f, 0.11f, 0.16f, 0.92f), pulse);

            if (_aimGuideRoot == null || _aimGuideText == null)
            {
                return;
            }

            var showAimGuide = tutorialTargetId == "battle.aim-area";
            _aimGuideRoot.SetActive(showAimGuide);
            if (showAimGuide)
            {
                _aimGuideText.text = tutorialMessage;
            }
        }

        private static bool TargetsStatus(string tutorialTargetId)
        {
            return tutorialTargetId == "battle.status"
                || tutorialTargetId == "battle.turn-label"
                || tutorialTargetId == "battle.impact";
        }

        private static void ApplyHighlight(Image image, bool highlighted, Color baseColor, float pulse)
        {
            if (image == null)
            {
                return;
            }

            image.color = highlighted
                ? Color.Lerp(baseColor, TutorialHighlightColor, pulse)
                : baseColor;
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

        private static void WireHold(Button button, Action onDown, Action onUp)
        {
            if (button == null)
            {
                return;
            }

            var trigger = button.gameObject.GetComponent<EventTrigger>() ?? button.gameObject.AddComponent<EventTrigger>();
            AddTrigger(trigger, EventTriggerType.PointerDown, _ => onDown?.Invoke());
            AddTrigger(trigger, EventTriggerType.PointerUp, _ => onUp?.Invoke());
            AddTrigger(trigger, EventTriggerType.PointerExit, _ => onUp?.Invoke());
        }

        private static void AddTrigger(EventTrigger trigger, EventTriggerType type, Action<BaseEventData> action)
        {
            trigger.triggers ??= new List<EventTrigger.Entry>();
            var entry = new EventTrigger.Entry { eventID = type };
            entry.callback.AddListener(data => action?.Invoke(data));
            trigger.triggers.Add(entry);
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
