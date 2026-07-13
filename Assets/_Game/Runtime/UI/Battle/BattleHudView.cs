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
    public sealed class BattleHudView : MonoBehaviour, IBattleHud
    {
        private const float SafeMargin = 24f;
        private const float WalkButtonSize = 168f;
        private const float JumpButtonSize = 112f;
        private const string BattleResourceRoot = "UI/Battle/";
        private const string WeaponResourceRoot = "UI/Weapons/";

        private static readonly Color InkSolid = new(0.055f, 0.085f, 0.13f, 0.96f);
        private static readonly Color Fire = new(0.14f, 0.78f, 0.36f, 1f);
        private static readonly Color Gold = new(1f, 0.72f, 0.18f, 1f);
        private static readonly Color Neutral = new(0.17f, 0.24f, 0.34f, 1f);
        private static readonly Color Disabled = new(0.20f, 0.23f, 0.28f, 0.42f);
        private static readonly Color TutorialHighlight = new(1f, 0.83f, 0.26f, 0.98f);
        private static readonly Color Control = new(0.11f, 0.36f, 0.62f, 1f);
        private static readonly Color ControlDisabled = new(0.11f, 0.18f, 0.26f, 0.62f);

        private readonly Dictionary<string, Sprite> _weaponSprites = new(StringComparer.Ordinal);
        private Sprite _whiteSprite;
        private Sprite _panelSprite;
        private Sprite _pillSprite;
        private Sprite _buttonSprite;
        private Sprite _circleSprite;
        private Sprite _tutorialCardSprite;
        private Sprite _timerPanelSprite;
        private Sprite _moveButtonSprite;
        private Sprite _jumpButtonSprite;

        private TMP_Text _statusText;
        private TMP_Text _turnText;
        private TMP_Text _powerText;
        private Image _powerFill;
        private Image _statusPanel;
        private Image _turnPanel;
        private Button _moveLeftButton;
        private Button _moveRightButton;
        private Button _jumpLeftButton;
        private Button _jumpRightButton;
        private Image _moveLeftPanel;
        private Image _moveRightPanel;
        private Image _jumpLeftPanel;
        private Image _jumpRightPanel;
        private Button _fireButton;
        private Image _firePanel;
        private Image _fireIcon;
        private string _weaponIconId = string.Empty;
        private GameObject _aimGuideRoot;
        private TMP_Text _aimGuideText;
        private GameObject _resultRoot;
        private Image _resultPanel;
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
        public event Action MoveLeftStarted;
        public event Action MoveLeftStopped;
        public event Action MoveRightStarted;
        public event Action MoveRightStopped;
        public event Action JumpLeftRequested;
        public event Action JumpRightRequested;
        public event Action ZoomInRequested;
        public event Action ZoomOutRequested;
        public event Action RestartRequested;
        public event Action ContinueRequested;
        public event Action MenuRequested;

        private void Awake()
        {
            _whiteSprite = Sprite.Create(Texture2D.whiteTexture, new Rect(0f, 0f, 1f, 1f), new Vector2(0.5f, 0.5f), 1f);
            _panelSprite = CreateRoundedSprite(128, 30, 8);
            _pillSprite = CreateRoundedSprite(128, 42, 9);
            _buttonSprite = CreateRoundedSprite(128, 28, 9);
            _circleSprite = CreateRoundedSprite(128, 62, 10);
            _tutorialCardSprite = LoadResourceSprite(BattleResourceRoot + "card", new Vector4(250f, 250f, 250f, 250f));
            _timerPanelSprite = LoadResourceSprite(BattleResourceRoot + "smal_panel", Vector4.zero);
            _moveButtonSprite = LoadResourceSprite(BattleResourceRoot + "btn_move", Vector4.zero);
            _jumpButtonSprite = LoadResourceSprite(BattleResourceRoot + "btn_jump", Vector4.zero);

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
            _turnText.text = Mathf.CeilToInt(state.TurnSecondsRemaining).ToString();
            var power01 = state.PowerFraction;
            _powerFill.fillAmount = power01;
            _powerFill.color = Color.Lerp(new Color(0.54f, 0.95f, 1f, 1f), new Color(1f, 0.38f, 0.12f, 1f), power01);
            _powerText.text = $"{Mathf.RoundToInt(power01 * 100f)}%";

            var movementEnabled = state.CanUseMovement && !state.TutorialBlocksMovement;
            SetControlState(_moveLeftButton, _moveLeftPanel, movementEnabled);
            SetControlState(_moveRightButton, _moveRightPanel, movementEnabled);
            SetControlState(_jumpLeftButton, _jumpLeftPanel, movementEnabled);
            SetControlState(_jumpRightButton, _jumpRightPanel, movementEnabled);

            _fireButton.interactable = state.CanFire;
            _firePanel.color = state.CanFire ? Fire : Disabled;
            _fireIcon.color = state.CanFire ? Color.white : new Color(1f, 1f, 1f, 0.42f);
            SetWeaponIcon(state.WeaponIconId);

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
            scaler.referenceResolution = new Vector2(1920f, 1080f);
            scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            scaler.matchWidthOrHeight = 0.5f;

            canvasObject.AddComponent<GraphicRaycaster>();

            var root = canvasObject.transform;
            BuildTopLayer(root);
            BuildMovementControls(root);
            BuildPowerIndicator(root);
            BuildFireButton(root);
            BuildAimGuide(root);
            BuildResultOverlay(root);
        }

        private void BuildTopLayer(Transform root)
        {
            _statusPanel = CreatePanel("Tutorial Card", root, new Vector2(0f, 1f), new Vector2(0f, 1f), new Vector2(0f, 1f),
                new Vector2(700f, 520f), new Vector2(SafeMargin, -SafeMargin), Color.white, _tutorialCardSprite);
            _statusPanel.type = Image.Type.Sliced;
            _statusText = CreateText("Tutorial Text", _statusPanel.transform, Vector2.zero, Vector2.one, new Vector2(0f, 1f),
                new Vector2(-120f, -170f), new Vector2(60f, -70f), 25, TextAlignmentOptions.TopLeft);
            _statusText.overflowMode = TextOverflowModes.Overflow;

            _turnPanel = CreatePanel("Turn Timer", root, new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(0.5f, 1f),
                new Vector2(163f, 72f), new Vector2(0f, -2f), Color.white, _timerPanelSprite);
            _turnPanel.type = Image.Type.Simple;
            _turnPanel.preserveAspect = true;
            _turnText = CreateText("Turn Seconds", _turnPanel.transform, Vector2.zero, Vector2.one, new Vector2(0.5f, 0.5f),
                new Vector2(-34f, -16f), new Vector2(0f, -1f), 32, TextAlignmentOptions.Center);
            _turnText.fontStyle = FontStyles.Bold;
            _turnText.color = new Color(1f, 0.88f, 0.45f, 1f);

            _ = CreateGameButton("Restart", root, new Vector2(1f, 1f), new Vector2(1f, 1f), new Vector2(122f, 62f),
                new Vector2(-SafeMargin, -SafeMargin), Neutral, "R", 32, string.Empty, 0, () => RestartRequested?.Invoke());
            _ = CreateGameButton("Menu", root, new Vector2(1f, 1f), new Vector2(1f, 1f), new Vector2(122f, 62f),
                new Vector2(-(SafeMargin + 134f), -SafeMargin), new Color(0.36f, 0.17f, 0.19f, 1f), "M", 34, string.Empty, 0, () => MenuRequested?.Invoke());
            _ = CreateGameButton("Zoom Out", root, new Vector2(1f, 1f), new Vector2(1f, 1f), new Vector2(72f, 72f),
                new Vector2(-(SafeMargin + 88f), -108f), Neutral, "−", 44, string.Empty, 0, () => ZoomOutRequested?.Invoke());
            _ = CreateGameButton("Zoom In", root, new Vector2(1f, 1f), new Vector2(1f, 1f), new Vector2(72f, 72f),
                new Vector2(-SafeMargin, -108f), Neutral, "+", 40, string.Empty, 0, () => ZoomInRequested?.Invoke());
        }

        private void BuildMovementControls(Transform root)
        {
            _moveLeftButton = CreateMovementButton("Move Left", root, new Vector2(0f, 0f), new Vector2(0f, 0f),
                WalkButtonSize, new Vector2(36f + WalkButtonSize, 34f), _moveButtonSprite, true, null);
            _moveLeftPanel = _moveLeftButton.GetComponent<Image>();
            WireHold(_moveLeftButton.gameObject, () => MoveLeftStarted?.Invoke(), () => MoveLeftStopped?.Invoke());

            _jumpLeftButton = CreateMovementButton("Jump Left", root, new Vector2(0f, 0f), new Vector2(0f, 0f),
                JumpButtonSize, new Vector2(58f + JumpButtonSize, 226f), _jumpButtonSprite, true, () => JumpLeftRequested?.Invoke());
            _jumpLeftPanel = _jumpLeftButton.GetComponent<Image>();

            _moveRightButton = CreateMovementButton("Move Right", root, new Vector2(1f, 0f), new Vector2(1f, 0f),
                WalkButtonSize, new Vector2(-36f, 34f), _moveButtonSprite, false, null);
            _moveRightPanel = _moveRightButton.GetComponent<Image>();
            WireHold(_moveRightButton.gameObject, () => MoveRightStarted?.Invoke(), () => MoveRightStopped?.Invoke());

            _jumpRightButton = CreateMovementButton("Jump Right", root, new Vector2(1f, 0f), new Vector2(1f, 0f),
                JumpButtonSize, new Vector2(-58f, 226f), _jumpButtonSprite, false, () => JumpRightRequested?.Invoke());
            _jumpRightPanel = _jumpRightButton.GetComponent<Image>();
        }

        private void BuildPowerIndicator(Transform root)
        {
            var powerPanel = CreatePanel("Power Capsule", root, new Vector2(0.5f, 0f), new Vector2(0.5f, 0f), new Vector2(0.5f, 0f),
                new Vector2(480f, 42f), new Vector2(0f, 38f), new Color(0.045f, 0.07f, 0.10f, 0.9f), _pillSprite);
            var powerBack = CreatePanel("Power Track", powerPanel.transform, new Vector2(0f, 0.5f), new Vector2(1f, 0.5f), new Vector2(0f, 0.5f),
                new Vector2(-84f, 12f), new Vector2(20f, 0f), new Color(1f, 1f, 1f, 0.16f), _pillSprite);
            powerBack.raycastTarget = false;
            _powerFill = CreatePanel("Power Fill", powerBack.transform, Vector2.zero, Vector2.one, new Vector2(0f, 0.5f),
                Vector2.zero, Vector2.zero, new Color(0.54f, 0.95f, 1f, 1f), _pillSprite);
            _powerFill.type = Image.Type.Filled;
            _powerFill.fillMethod = Image.FillMethod.Horizontal;
            _powerFill.fillOrigin = (int)Image.OriginHorizontal.Left;
            _powerFill.fillAmount = 0f;
            _powerText = CreateText("Power Text", powerPanel.transform, new Vector2(1f, 0f), new Vector2(1f, 1f), new Vector2(1f, 0.5f),
                new Vector2(78f, -4f), new Vector2(-14f, 0f), 18, TextAlignmentOptions.Center);
            _powerText.color = new Color(0.88f, 0.97f, 1f, 1f);
        }

        private void BuildFireButton(Transform root)
        {
            _firePanel = CreatePanel("Fire Weapon", root, new Vector2(0.5f, 0f), new Vector2(0.5f, 0f), new Vector2(0.5f, 0f),
                new Vector2(148f, 148f), new Vector2(350f, 30f), Fire, _circleSprite);
            AddOuterStroke(_firePanel.gameObject, new Color(0.72f, 1f, 0.45f, 0.8f), new Vector2(2f, -2f));
            _fireButton = _firePanel.gameObject.AddComponent<Button>();
            _fireButton.targetGraphic = _firePanel;
            _fireButton.colors = CreateButtonColors(Fire);
            _fireButton.onClick.AddListener(() => FireRequested?.Invoke());

            _fireIcon = CreatePanel("Selected Weapon Icon", _firePanel.transform, Vector2.zero, Vector2.one, new Vector2(0.5f, 0.5f),
                new Vector2(-22f, -22f), Vector2.zero, Color.white, _whiteSprite);
            _fireIcon.type = Image.Type.Simple;
            _fireIcon.preserveAspect = true;
            _fireIcon.raycastTarget = false;
        }

        private void BuildAimGuide(Transform root)
        {
            _aimGuideRoot = CreateGlassPanel("Aim Guide", root, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                new Vector2(760f, 74f), new Vector2(0f, -170f), new Color(0.05f, 0.08f, 0.12f, 0.78f), Gold).gameObject;
            _aimGuideText = CreateText("Aim Guide Text", _aimGuideRoot.transform, Vector2.zero, Vector2.one, new Vector2(0.5f, 0.5f),
                new Vector2(-34f, -16f), Vector2.zero, 22, TextAlignmentOptions.Center);
            _aimGuideText.color = new Color(1f, 0.92f, 0.62f, 1f);
            _aimGuideRoot.SetActive(false);
        }

        private void BuildResultOverlay(Transform root)
        {
            var blocker = CreatePanel("Result Overlay", root, Vector2.zero, Vector2.one, new Vector2(0.5f, 0.5f),
                Vector2.zero, Vector2.zero, new Color(0f, 0f, 0f, 0.5f), _whiteSprite);
            blocker.raycastTarget = true;
            _resultRoot = blocker.gameObject;

            _resultPanel = CreateGlassPanel("Result Panel", blocker.transform, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                new Vector2(780f, 486f), Vector2.zero, InkSolid, Gold);
            AddTopSheen(_resultPanel.transform, 0.28f);
            AddRibbon(_resultPanel.transform, new Color(0.78f, 0.18f, 0.18f, 0.96f));

            _resultTitleText = CreateText("Result Title", _resultPanel.transform, new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(0.5f, 1f),
                new Vector2(-86f, 78f), new Vector2(0f, -26f), 47, TextAlignmentOptions.Center);
            _resultTitleText.fontStyle = FontStyles.Bold;
            _resultTitleText.color = new Color(1f, 0.88f, 0.42f, 1f);
            _resultSubtitleText = CreateText("Result Subtitle", _resultPanel.transform, new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(0.5f, 1f),
                new Vector2(-96f, 46f), new Vector2(0f, -112f), 27, TextAlignmentOptions.Center);
            _resultOutcomeText = CreateText("Result Outcome", _resultPanel.transform, new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(0.5f, 1f),
                new Vector2(-110f, 42f), new Vector2(0f, -172f), 24, TextAlignmentOptions.Center);
            _resultRewardText = CreateText("Result Reward", _resultPanel.transform, new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(0.5f, 1f),
                new Vector2(-110f, 42f), new Vector2(0f, -220f), 24, TextAlignmentOptions.Center);
            _resultUnlockText = CreateText("Result Unlock", _resultPanel.transform, new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(0.5f, 1f),
                new Vector2(-110f, 42f), new Vector2(0f, -268f), 23, TextAlignmentOptions.Center);
            _resultUnlockText.color = new Color(0.70f, 0.91f, 1f, 1f);
            _resultContinueNoteText = CreateText("Result Continue Note", _resultPanel.transform, new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(0.5f, 1f),
                new Vector2(-120f, 34f), new Vector2(0f, -314f), 19, TextAlignmentOptions.Center);
            _resultContinueNoteText.color = new Color(0.83f, 0.91f, 1f, 0.85f);

            var retry = CreateGameButton("Result Retry", _resultPanel.transform, new Vector2(0f, 0f), new Vector2(0f, 0f), new Vector2(286f, 78f),
                new Vector2(44f, 36f), Neutral, "R", 30, GameText.Restart, 28, () => RestartRequested?.Invoke());
            _resultRetryLabel = retry.GetComponentInChildren<TMP_Text>();
            _resultContinueButton = CreateGameButton("Result Continue", _resultPanel.transform, new Vector2(1f, 0f), new Vector2(1f, 0f), new Vector2(286f, 78f),
                new Vector2(-44f, 36f), Fire, ">", 26, GameText.Continue, 28, () => ContinueRequested?.Invoke());
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
            var pulse = 0.25f + Mathf.PingPong(Time.unscaledTime * 0.95f, 0.5f);
            var movementColor = _moveLeftButton != null && _moveLeftButton.interactable ? Control : ControlDisabled;
            var fireColor = _fireButton != null && _fireButton.interactable ? Fire : Disabled;
            ApplyHighlight(_statusPanel, TargetsStatus(tutorialTargetId), Color.white, pulse);
            ApplyHighlight(_turnPanel, tutorialTargetId == "battle.turn-label", Color.white, pulse);
            ApplyHighlight(_moveLeftPanel, tutorialTargetId == "battle.move-controls", movementColor, pulse);
            ApplyHighlight(_moveRightPanel, tutorialTargetId == "battle.move-controls", movementColor, pulse);
            ApplyHighlight(_jumpLeftPanel, tutorialTargetId == "battle.move-controls", movementColor, pulse);
            ApplyHighlight(_jumpRightPanel, tutorialTargetId == "battle.move-controls", movementColor, pulse);
            ApplyHighlight(_firePanel, tutorialTargetId == "battle.fire-button" || tutorialTargetId == "battle.weapon-strip", fireColor, pulse);
            ApplyHighlight(_resultPanel, tutorialTargetId == "battle.result", InkSolid, pulse);

            var showAimGuide = tutorialTargetId == "battle.aim-area";
            _aimGuideRoot.SetActive(showAimGuide);
            if (showAimGuide)
            {
                _aimGuideText.text = tutorialMessage;
            }
        }

        private static void SetControlState(Button button, Image panel, bool enabled)
        {
            if (button != null)
            {
                button.interactable = enabled;
            }

            if (panel != null)
            {
                panel.color = enabled ? Control : ControlDisabled;
            }
        }

        private void SetWeaponIcon(string iconId)
        {
            iconId ??= string.Empty;
            if (_weaponIconId == iconId)
            {
                return;
            }

            _weaponIconId = iconId;
            if (!_weaponSprites.TryGetValue(iconId, out var sprite))
            {
                sprite = LoadResourceSprite(WeaponResourceRoot + iconId, Vector4.zero);
                _weaponSprites[iconId] = sprite;
            }

            _fireIcon.sprite = sprite;
            _fireIcon.enabled = sprite != null;
        }

        private Button CreateMovementButton(string name, Component parent, Vector2 anchor, Vector2 pivot, float size,
            Vector2 anchoredPosition, Sprite sprite, bool mirrorHorizontally, Action onClick)
        {
            var image = CreatePanel(name, parent, anchor, anchor, pivot, new Vector2(size, size), anchoredPosition, Color.white, sprite ?? _circleSprite);
            image.type = Image.Type.Simple;
            image.preserveAspect = true;
            image.rectTransform.localScale = mirrorHorizontally ? new Vector3(-1f, 1f, 1f) : Vector3.one;

            var button = image.gameObject.AddComponent<Button>();
            button.targetGraphic = image;
            button.colors = CreateMovementButtonColors();
            if (onClick != null)
            {
                button.onClick.AddListener(() => onClick());
            }

            return button;
        }

        private static void WireHold(GameObject target, Action onStart, Action onStop)
        {
            var trigger = target.GetComponent<EventTrigger>() ?? target.AddComponent<EventTrigger>();
            trigger.triggers ??= new List<EventTrigger.Entry>();
            AddTrigger(trigger, EventTriggerType.PointerDown, onStart);
            AddTrigger(trigger, EventTriggerType.PointerUp, onStop);
            AddTrigger(trigger, EventTriggerType.PointerExit, onStop);
        }

        private static void AddTrigger(EventTrigger trigger, EventTriggerType type, Action callback)
        {
            var entry = new EventTrigger.Entry { eventID = type };
            entry.callback.AddListener(_ => callback?.Invoke());
            trigger.triggers.Add(entry);
        }

        private Sprite LoadResourceSprite(string path, Vector4 border)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return null;
            }

            var texture = Resources.Load<Texture2D>(path);
            return texture == null
                ? null
                : Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100f, 0, SpriteMeshType.FullRect, border);
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

            image.color = highlighted ? Color.Lerp(baseColor, TutorialHighlight, pulse) : baseColor;
        }

        private Button CreateGameButton(string name, Component parent, Vector2 anchorMin, Vector2 anchorMax, Vector2 size,
            Vector2 anchoredPosition, Color color, string icon, int iconSize, string label, int labelSize, Action onClick)
        {
            var pivot = new Vector2(anchorMin.x, anchorMin.y);
            var image = CreatePanel(name, parent, anchorMin, anchorMax, pivot, size, anchoredPosition, color, size.x <= 132f ? _circleSprite : _buttonSprite);
            AddOuterStroke(image.gameObject, Color.Lerp(color, Color.white, 0.28f), new Vector2(1.4f, -1.4f));
            AddTopSheen(image.transform, 0.18f);

            var button = image.gameObject.AddComponent<Button>();
            button.targetGraphic = image;
            button.transition = Selectable.Transition.ColorTint;
            button.colors = CreateButtonColors(color);
            button.onClick.AddListener(() => onClick?.Invoke());

            var iconText = CreateText($"{name} Icon", image.transform, Vector2.zero, Vector2.one, new Vector2(0.5f, 0.5f),
                Vector2.zero, new Vector2(0f, labelSize > 0 ? 9f : 0f), iconSize, TextAlignmentOptions.Center);
            iconText.text = icon;
            iconText.fontStyle = FontStyles.Bold;

            if (labelSize > 0)
            {
                var labelText = CreateText($"{name} Label", image.transform, Vector2.zero, Vector2.one, new Vector2(0.5f, 0f),
                    new Vector2(-22f, 28f), new Vector2(0f, 10f), labelSize, TextAlignmentOptions.Bottom);
                labelText.text = label;
                labelText.fontStyle = FontStyles.Bold;
                labelText.characterSpacing = 3f;
            }

            return button;
        }

        private Image CreateGlassPanel(string name, Component parent, Vector2 anchorMin, Vector2 anchorMax, Vector2 pivot,
            Vector2 sizeDelta, Vector2 anchoredPosition, Color color, Color accent)
        {
            var panel = CreatePanel(name, parent, anchorMin, anchorMax, pivot, sizeDelta, anchoredPosition, color, _panelSprite);
            AddOuterStroke(panel.gameObject, accent, new Vector2(1.3f, -1.3f));
            AddTopSheen(panel.transform, 0.22f);
            return panel;
        }

        private Image CreatePanel(string name, Component parent, Vector2 anchorMin, Vector2 anchorMax, Vector2 pivot,
            Vector2 sizeDelta, Vector2 anchoredPosition, Color color, Sprite sprite)
        {
            var rect = CreateRect(name, parent, anchorMin, anchorMax, pivot, sizeDelta, anchoredPosition);
            var image = rect.gameObject.AddComponent<Image>();
            image.sprite = sprite ?? _panelSprite ?? _whiteSprite;
            image.type = Image.Type.Sliced;
            image.color = color;

            var shadow = rect.gameObject.AddComponent<Shadow>();
            shadow.effectColor = new Color(0f, 0f, 0f, 0.35f);
            shadow.effectDistance = new Vector2(0f, -4f);
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
            text.overflowMode = TextOverflowModes.Truncate;
            text.raycastTarget = false;

            var shadow = rect.gameObject.AddComponent<Shadow>();
            shadow.effectColor = new Color(0f, 0f, 0f, 0.62f);
            shadow.effectDistance = new Vector2(1.6f, -1.8f);
            return text;
        }

        private void AddTopSheen(Transform parent, float alpha)
        {
            var sheen = CreatePanel("Top Sheen", parent, new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(0.5f, 1f),
                new Vector2(-30f, 12f), new Vector2(0f, -8f), new Color(1f, 1f, 1f, alpha), _pillSprite);
            sheen.raycastTarget = false;
        }

        private void AddOuterStroke(GameObject target, Color color, Vector2 distance)
        {
            var outline = target.AddComponent<Outline>();
            outline.effectColor = color;
            outline.effectDistance = distance;
        }

        private void AddRibbon(Transform parent, Color color)
        {
            var ribbon = CreatePanel("Result Ribbon", parent, new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(0.5f, 0.5f),
                new Vector2(460f, 54f), new Vector2(0f, -18f), color, _pillSprite);
            ribbon.raycastTarget = false;
            AddOuterStroke(ribbon.gameObject, new Color(1f, 0.78f, 0.42f, 0.58f), new Vector2(1.4f, -1.4f));
        }

        private static Sprite CreateRoundedSprite(int size, int radius, int border)
        {
            var texture = new Texture2D(size, size, TextureFormat.RGBA32, false);
            texture.filterMode = FilterMode.Bilinear;
            texture.wrapMode = TextureWrapMode.Clamp;

            var pixels = new Color32[size * size];
            var edge = size - 1;
            for (var y = 0; y < size; y++)
            {
                for (var x = 0; x < size; x++)
                {
                    var dx = Mathf.Max(Mathf.Max(radius - x, x - (edge - radius)), 0);
                    var dy = Mathf.Max(Mathf.Max(radius - y, y - (edge - radius)), 0);
                    var distance = Mathf.Sqrt(dx * dx + dy * dy);
                    var alpha = Mathf.Clamp01(radius + 0.5f - distance);
                    pixels[y * size + x] = new Color(1f, 1f, 1f, alpha);
                }
            }

            texture.SetPixels32(pixels);
            texture.Apply();
            return Sprite.Create(texture, new Rect(0f, 0f, size, size), new Vector2(0.5f, 0.5f), 100f, 0, SpriteMeshType.FullRect, Vector4.one * border);
        }

        private static ColorBlock CreateButtonColors(Color baseColor)
        {
            return new ColorBlock
            {
                normalColor = baseColor,
                highlightedColor = Color.Lerp(baseColor, Color.white, 0.18f),
                pressedColor = Color.Lerp(baseColor, Color.black, 0.24f),
                selectedColor = Color.Lerp(baseColor, Color.white, 0.10f),
                disabledColor = Disabled,
                colorMultiplier = 1f,
                fadeDuration = 0.075f
            };
        }

        private static ColorBlock CreateMovementButtonColors()
        {
            return new ColorBlock
            {
                normalColor = Color.white,
                highlightedColor = new Color(1f, 1f, 1f, 1f),
                pressedColor = new Color(0.72f, 0.82f, 0.94f, 1f),
                selectedColor = Color.white,
                disabledColor = Color.white,
                colorMultiplier = 1f,
                fadeDuration = 0.075f
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
