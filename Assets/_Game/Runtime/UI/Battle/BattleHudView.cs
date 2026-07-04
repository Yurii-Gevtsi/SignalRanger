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
        private const float DockHeight = 176f;

        private static readonly Color Ink = new(0.04f, 0.065f, 0.095f, 0.88f);
        private static readonly Color InkSolid = new(0.055f, 0.085f, 0.13f, 0.96f);
        private static readonly Color Fire = new(0.14f, 0.78f, 0.36f, 1f);
        private static readonly Color FireDark = new(0.07f, 0.43f, 0.22f, 1f);
        private static readonly Color Blue = new(0.15f, 0.47f, 0.88f, 1f);
        private static readonly Color Gold = new(1f, 0.72f, 0.18f, 1f);
        private static readonly Color Orange = new(0.86f, 0.43f, 0.13f, 1f);
        private static readonly Color Neutral = new(0.17f, 0.24f, 0.34f, 1f);
        private static readonly Color Disabled = new(0.20f, 0.23f, 0.28f, 0.42f);
        private static readonly Color TutorialHighlight = new(1f, 0.83f, 0.26f, 0.98f);

        private Sprite _whiteSprite;
        private Sprite _panelSprite;
        private Sprite _pillSprite;
        private Sprite _buttonSprite;
        private Sprite _circleSprite;

        private TMP_Text _statusText;
        private TMP_Text _turnText;
        private TMP_Text _weaponTitleText;
        private TMP_Text _weaponMetaText;
        private TMP_Text _weaponHintText;
        private TMP_Text _mobilityText;
        private TMP_Text _powerText;
        private Image _powerFill;
        private Image _statusPanel;
        private Image _turnPanel;
        private Image _weaponPanel;
        private Image _weaponAccent;
        private Button _weaponButton;
        private Button _moveLeftButton;
        private Image _moveLeftPanel;
        private Button _moveRightButton;
        private Image _moveRightPanel;
        private Button _jumpBackButton;
        private Image _jumpBackPanel;
        private Button _jumpForwardButton;
        private Image _jumpForwardPanel;
        private Button _cancelButton;
        private Image _cancelPanel;
        private Button _fireButton;
        private Image _firePanel;
        private TMP_Text _fireLabel;
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
            _panelSprite = CreateRoundedSprite(128, 30, 8);
            _pillSprite = CreateRoundedSprite(128, 42, 9);
            _buttonSprite = CreateRoundedSprite(128, 28, 9);
            _circleSprite = CreateRoundedSprite(128, 62, 10);

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
            _weaponTitleText.text = state.WeaponLabel;
            _weaponMetaText.text = $"{GameText.WeaponType(state.WeaponRoleLabel)}  /  {state.AmmoLabel}";
            _weaponHintText.text = state.WeaponHint;
            _mobilityText.text = state.MobilityLabel;

            var power01 = state.PowerFraction;
            _powerFill.fillAmount = power01;
            _powerFill.color = Color.Lerp(new Color(0.54f, 0.95f, 1f, 1f), new Color(1f, 0.38f, 0.12f, 1f), power01);
            _powerText.text = $"{Mathf.RoundToInt(power01 * 100f)}%";

            _weaponButton.interactable = !state.TutorialBlocksWeaponSwitch;
            _cancelButton.interactable = !state.TutorialBlocksCancel;
            _moveLeftButton.interactable = state.CanMoveLeft && !state.TutorialBlocksMovement;
            _moveRightButton.interactable = state.CanMoveRight && !state.TutorialBlocksMovement;
            _jumpBackButton.interactable = state.CanJumpBackward && !state.TutorialBlocksMovement;
            _jumpForwardButton.interactable = state.CanJumpForward && !state.TutorialBlocksMovement;

            _fireButton.interactable = state.CanFire;
            _firePanel.color = state.CanFire ? Fire : Disabled;
            _fireLabel.color = state.CanFire ? Color.white : new Color(1f, 1f, 1f, 0.42f);

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
            BuildBottomDock(root);
            BuildAimGuide(root);
            BuildResultOverlay(root);
        }

        private void BuildTopLayer(Transform root)
        {
            _statusPanel = CreateGlassPanel("Status Chip", root, new Vector2(0f, 1f), new Vector2(0f, 1f), new Vector2(0f, 1f),
                new Vector2(620f, 86f), new Vector2(SafeMargin, -SafeMargin), Ink, new Color(0.21f, 0.73f, 1f, 0.62f));
            _ = AddAccentStrip(_statusPanel.transform, Gold);

            _statusText = CreateText("Status", _statusPanel.transform, Vector2.zero, Vector2.one, new Vector2(0f, 1f),
                new Vector2(-46f, -10f), new Vector2(28f, -10f), 24, TextAlignmentOptions.TopLeft);
            _turnText = CreateText("Turn", _statusPanel.transform, Vector2.zero, Vector2.one, new Vector2(0f, 0f),
                new Vector2(-46f, -12f), new Vector2(28f, 10f), 22, TextAlignmentOptions.BottomLeft);
            _turnText.color = new Color(0.76f, 0.91f, 1f, 1f);

            _turnPanel = CreateGlassPanel("Turn Badge", root, new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(0.5f, 1f),
                new Vector2(260f, 58f), new Vector2(0f, -SafeMargin), new Color(0.06f, 0.08f, 0.11f, 0.7f), Gold);
            var turnStatic = CreateText("Turn Badge Text", _turnPanel.transform, Vector2.zero, Vector2.one, new Vector2(0.5f, 0.5f),
                new Vector2(-22f, -10f), Vector2.zero, 20, TextAlignmentOptions.Center);
            turnStatic.text = "30 SEC TURN";
            turnStatic.color = new Color(1f, 0.88f, 0.45f, 1f);

            _ = CreateGameButton("Restart", root, new Vector2(1f, 1f), new Vector2(1f, 1f), new Vector2(122f, 62f),
                new Vector2(-SafeMargin, -SafeMargin), Neutral, "R", 32, string.Empty, 0, () => RestartRequested?.Invoke());
            _ = CreateGameButton("Menu", root, new Vector2(1f, 1f), new Vector2(1f, 1f), new Vector2(122f, 62f),
                new Vector2(-(SafeMargin + 134f), -SafeMargin), new Color(0.36f, 0.17f, 0.19f, 1f), "M", 34, string.Empty, 0, () => MenuRequested?.Invoke());
        }

        private void BuildBottomDock(Transform root)
        {
            var dock = CreateGlassPanel("Battle Control Dock", root, new Vector2(0f, 0f), new Vector2(1f, 0f), new Vector2(0.5f, 0f),
                new Vector2(-2f * SafeMargin, DockHeight), new Vector2(0f, SafeMargin), new Color(0.035f, 0.055f, 0.08f, 0.82f), new Color(0.26f, 0.57f, 0.86f, 0.28f));
            AddTopSheen(dock.transform, 0.26f);
            BuildWeaponCard(dock.transform);
            BuildMovementCluster(dock.transform);
            BuildActionCluster(dock.transform);
        }

        private void BuildWeaponCard(Transform dock)
        {
            _weaponPanel = CreateGlassPanel("Weapon Card", dock, new Vector2(0f, 0.5f), new Vector2(0f, 0.5f), new Vector2(0f, 0.5f),
                new Vector2(500f, 128f), new Vector2(26f, 0f), new Color(0.08f, 0.13f, 0.19f, 0.94f), Gold);
            _weaponAccent = AddAccentStrip(_weaponPanel.transform, Gold);

            _weaponButton = _weaponPanel.gameObject.AddComponent<Button>();
            _weaponButton.targetGraphic = _weaponPanel;
            _weaponButton.transition = Selectable.Transition.ColorTint;
            _weaponButton.colors = CreateButtonColors(_weaponPanel.color);
            _weaponButton.onClick.AddListener(() => NextWeaponRequested?.Invoke());

            var iconPlate = CreatePanel("Weapon Icon Plate", _weaponPanel.transform, new Vector2(0f, 0.5f), new Vector2(0f, 0.5f),
                new Vector2(0.5f, 0.5f), new Vector2(84f, 84f), new Vector2(62f, 0f), new Color(0.93f, 0.64f, 0.18f, 0.98f), _circleSprite);
            AddOuterStroke(iconPlate.gameObject, new Color(1f, 0.93f, 0.62f, 0.78f), new Vector2(2f, -2f));

            var iconText = CreateText("Weapon Icon", iconPlate.transform, Vector2.zero, Vector2.one, new Vector2(0.5f, 0.5f),
                Vector2.zero, Vector2.zero, 39, TextAlignmentOptions.Center);
            iconText.text = "*";
            iconText.color = new Color(0.18f, 0.12f, 0.06f, 1f);

            _weaponTitleText = CreateText("Weapon Title", _weaponPanel.transform, Vector2.zero, Vector2.one, new Vector2(0f, 1f),
                new Vector2(-144f, 38f), new Vector2(128f, -14f), 25, TextAlignmentOptions.TopLeft);
            _weaponTitleText.fontStyle = FontStyles.Bold;
            _weaponMetaText = CreateText("Weapon Meta", _weaponPanel.transform, Vector2.zero, Vector2.one, new Vector2(0f, 0.5f),
                new Vector2(-144f, 26f), new Vector2(128f, -15f), 18, TextAlignmentOptions.Left);
            _weaponMetaText.color = new Color(1f, 0.83f, 0.44f, 1f);
            _weaponHintText = CreateText("Weapon Hint", _weaponPanel.transform, Vector2.zero, Vector2.one, new Vector2(0f, 0f),
                new Vector2(-144f, 38f), new Vector2(128f, 13f), 16, TextAlignmentOptions.BottomLeft);
            _weaponHintText.color = new Color(0.82f, 0.91f, 1f, 0.9f);
            _weaponHintText.maxVisibleLines = 2;
        }

        private void BuildMovementCluster(Transform dock)
        {
            var cluster = CreatePanel("Movement Cluster", dock, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                new Vector2(0.5f, 0.5f), new Vector2(650f, 134f), Vector2.zero, new Color(0.05f, 0.075f, 0.11f, 0.68f), _pillSprite);
            AddTopSheen(cluster.transform, 0.18f);

            _mobilityText = CreateText("Mobility Text", cluster.transform, new Vector2(0f, 1f), new Vector2(1f, 1f),
                new Vector2(0.5f, 1f), new Vector2(-34f, 30f), new Vector2(0f, -9f), 18, TextAlignmentOptions.Center);
            _mobilityText.color = new Color(0.76f, 0.91f, 1f, 0.95f);

            _jumpBackButton = CreateRoundAction("Jump Back", cluster.transform, new Vector2(-241f, -17f), Orange, "JB", 31, () => JumpBackwardRequested?.Invoke());
            _jumpBackPanel = _jumpBackButton.GetComponent<Image>();
            _moveLeftButton = CreateRoundAction("Move Left", cluster.transform, new Vector2(-91f, -17f), Blue, "<", 52, () => { });
            _moveLeftPanel = _moveLeftButton.GetComponent<Image>();
            WireHold(_moveLeftButton, () => MoveLeftStarted?.Invoke(), () => MoveLeftStopped?.Invoke());
            _moveRightButton = CreateRoundAction("Move Right", cluster.transform, new Vector2(91f, -17f), Blue, ">", 52, () => { });
            _moveRightPanel = _moveRightButton.GetComponent<Image>();
            WireHold(_moveRightButton, () => MoveRightStarted?.Invoke(), () => MoveRightStopped?.Invoke());
            _jumpForwardButton = CreateRoundAction("Jump Forward", cluster.transform, new Vector2(241f, -17f), new Color(0.32f, 0.68f, 0.22f, 1f), "JF", 31, () => JumpForwardRequested?.Invoke());
            _jumpForwardPanel = _jumpForwardButton.GetComponent<Image>();
        }

        private void BuildActionCluster(Transform dock)
        {
            var actionRoot = CreateRect("Action Cluster", dock, new Vector2(1f, 0.5f), new Vector2(1f, 0.5f), new Vector2(1f, 0.5f),
                new Vector2(540f, 138f), new Vector2(-26f, 0f));

            var powerPanel = CreatePanel("Power Capsule", actionRoot, new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(0.5f, 1f),
                new Vector2(0f, 36f), Vector2.zero, new Color(0.045f, 0.07f, 0.10f, 0.92f), _pillSprite);
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

            _cancelButton = CreateGameButton("Cancel", actionRoot, new Vector2(0f, 0f), new Vector2(0f, 0f), new Vector2(190f, 88f),
                Vector2.zero, Neutral, "X", 37, GameText.CancelAim, 18, () => CancelRequested?.Invoke());
            _cancelPanel = _cancelButton.GetComponent<Image>();
            _fireButton = CreateGameButton("Fire", actionRoot, new Vector2(1f, 0f), new Vector2(1f, 0f), new Vector2(326f, 88f),
                Vector2.zero, Fire, "!", 32, GameText.Fire, 32, () => FireRequested?.Invoke());
            _firePanel = _fireButton.GetComponent<Image>();
            _fireLabel = _fireButton.GetComponentInChildren<TMP_Text>();
            _ = AddAccentStrip(_fireButton.transform, new Color(0.72f, 1f, 0.45f, 0.8f));
            AddBottomDepth(_fireButton.transform, FireDark);
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
            ApplyHighlight(_statusPanel, TargetsStatus(tutorialTargetId), Ink, pulse);
            ApplyHighlight(_turnPanel, tutorialTargetId == "battle.turn-label", new Color(0.06f, 0.08f, 0.11f, 0.7f), pulse);
            ApplyHighlight(_weaponPanel, tutorialTargetId == "battle.weapon-strip", new Color(0.08f, 0.13f, 0.19f, 0.94f), pulse);
            ApplyHighlight(_moveLeftPanel, tutorialTargetId == "battle.move-controls", Blue, pulse);
            ApplyHighlight(_moveRightPanel, tutorialTargetId == "battle.move-controls", Blue, pulse);
            ApplyHighlight(_jumpBackPanel, tutorialTargetId == "battle.move-controls", Orange, pulse);
            ApplyHighlight(_jumpForwardPanel, tutorialTargetId == "battle.move-controls", new Color(0.32f, 0.68f, 0.22f, 1f), pulse);
            ApplyHighlight(_firePanel, tutorialTargetId == "battle.fire-button", Fire, pulse);
            ApplyHighlight(_cancelPanel, tutorialTargetId == "battle.fire-button", Neutral, pulse * 0.45f);
            ApplyHighlight(_resultPanel, tutorialTargetId == "battle.result", InkSolid, pulse);

            if (_weaponAccent != null)
            {
                _weaponAccent.color = tutorialTargetId == "battle.weapon-strip" ? TutorialHighlight : Gold;
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

            image.color = highlighted ? Color.Lerp(baseColor, TutorialHighlight, pulse) : baseColor;
        }

        private Button CreateRoundAction(string name, Component parent, Vector2 position, Color color, string icon, int iconSize, Action onClick)
        {
            var button = CreateGameButton(name, parent, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(118f, 92f),
                position, color, icon, iconSize, string.Empty, 0, onClick);
            AddBottomDepth(button.transform, Color.Lerp(color, Color.black, 0.34f));
            return button;
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

        private Image AddAccentStrip(Transform parent, Color color)
        {
            var strip = CreatePanel("Accent Strip", parent, Vector2.zero, new Vector2(0f, 1f), new Vector2(0f, 0.5f),
                new Vector2(9f, -22f), Vector2.zero, color, _pillSprite);
            strip.raycastTarget = false;
            return strip;
        }

        private void AddTopSheen(Transform parent, float alpha)
        {
            var sheen = CreatePanel("Top Sheen", parent, new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(0.5f, 1f),
                new Vector2(-30f, 12f), new Vector2(0f, -8f), new Color(1f, 1f, 1f, alpha), _pillSprite);
            sheen.raycastTarget = false;
        }

        private void AddBottomDepth(Transform parent, Color color)
        {
            var depth = CreatePanel("Bottom Depth", parent, new Vector2(0f, 0f), new Vector2(1f, 0f), new Vector2(0.5f, 0f),
                new Vector2(-28f, 9f), new Vector2(0f, 5f), color, _pillSprite);
            depth.raycastTarget = false;
            depth.transform.SetAsFirstSibling();
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
