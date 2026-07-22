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
        private const float TutorialCardPositionY = 24f;
        private const float TimerPositionY = 0f;
        private const float SettingsPositionY = -20f;
        private const float ArsenalPositionY = -30f;
        private const float WalkButtonSize = 202f;
        private const float JumpButtonSize = 134f;
        private const float WeaponButtonSize = 180f;
        private const float FireWeaponIconSize = 120f;
        private const float ArsenalButtonSize = 264f;
        private const float MovementBottomMargin = 34f;
        private const float MovementButtonGap = 24f;
        private const string BattleResourceRoot = "UI/Battle/";
        private const string WeaponResourceRoot = "UI/Weapons/";

        private static readonly Color InkSolid = new(0.055f, 0.085f, 0.13f, 0.96f);
        private static readonly Color Fire = new(0.14f, 0.78f, 0.36f, 1f);
        private static readonly Color Gold = new(1f, 0.72f, 0.18f, 1f);
        private static readonly Color Neutral = new(0.17f, 0.24f, 0.34f, 1f);
        private static readonly Color Disabled = new(0.20f, 0.23f, 0.28f, 0.42f);
        private static readonly Color TutorialHighlight = new(1f, 0.83f, 0.26f, 0.98f);
        private static readonly Color ControlVisual = new(1f, 1f, 1f, 0.4f);
        private static readonly Color FireControlVisual = new(1f, 1f, 1f, 0.68f);

        private readonly Dictionary<string, Sprite> _weaponSprites = new(StringComparer.Ordinal);
        private Sprite _whiteSprite;
        private Sprite _panelSprite;
        private Sprite _pillSprite;
        private Sprite _buttonSprite;
        private Sprite _circleSprite;
        private Sprite _fireRingSprite;
        private Sprite _tutorialCardSprite;
        private Sprite _timerPanelSprite;
        private Sprite _moveButtonSprite;
        private Sprite _jumpButtonSprite;
        private Sprite _arsenalButtonSprite;
        private Sprite _resetButtonSprite;
        private Sprite _settingsButtonSprite;
        private Sprite _weaponsCardSprite;
        private Sprite _weaponMiniCardSprite;

        private TMP_Text _statusText;
        private TMP_Text _turnText;
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
        private Button _arsenalButton;
        private Image _arsenalPanel;
        private GameObject _arsenalRoot;
        private Image _arsenalWindow;
        private Transform _arsenalItemsRoot;
        private IReadOnlyList<BattleWeaponHudItem> _renderedArsenalWeapons;
        private int _renderedSelectedWeaponIndex = -1;
        private Button _fireButton;
        private Image _firePanel;
        private Image _fireIcon;
        private Button _weaponResetButton;
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
        public event Action<int> WeaponSelected;
        public event Action<bool> ArsenalVisibilityChanged;
        public event Action WeaponResetRequested;
        public event Action SettingsRequested;
        public event Action MoveLeftStarted;
        public event Action MoveLeftStopped;
        public event Action MoveRightStarted;
        public event Action MoveRightStopped;
        public event Action JumpLeftRequested;
        public event Action JumpRightRequested;
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
            _fireRingSprite = CreateCircleFrameSprite(256, 7f);
            _tutorialCardSprite = LoadResourceSprite(BattleResourceRoot + "card", new Vector4(250f, 250f, 250f, 250f));
            _timerPanelSprite = LoadResourceSprite(BattleResourceRoot + "smal_panel", Vector4.zero);
            _moveButtonSprite = LoadResourceSprite(BattleResourceRoot + "btn_move", Vector4.zero);
            _jumpButtonSprite = LoadResourceSprite(BattleResourceRoot + "btn_jump", Vector4.zero);
            _arsenalButtonSprite = LoadResourceSprite(BattleResourceRoot + "arsenal", Vector4.zero);
            _resetButtonSprite = LoadResourceSprite(BattleResourceRoot + "reset_btn", Vector4.zero);
            _settingsButtonSprite = LoadResourceSprite("UI/Menu/settings_btn", Vector4.zero);
            _weaponsCardSprite = LoadResourceSprite(BattleResourceRoot + "weapons_card", Vector4.zero);
            _weaponMiniCardSprite = LoadResourceSprite(BattleResourceRoot + "weapon_mini_card", Vector4.zero);

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
            var movementEnabled = state.CanUseMovement && !state.TutorialBlocksMovement;
            SetControlState(_moveLeftButton, _moveLeftPanel, movementEnabled);
            SetControlState(_moveRightButton, _moveRightPanel, movementEnabled);
            SetControlState(_jumpLeftButton, _jumpLeftPanel, movementEnabled);
            SetControlState(_jumpRightButton, _jumpRightPanel, movementEnabled);

            _arsenalButton.interactable = state.CanOpenArsenal;
            _arsenalPanel.color = Color.white;
            RenderArsenal(state);

            var fireIcon = LoadWeaponSprite(state.WeaponIconId);
            _fireIcon.sprite = fireIcon;
            _firePanel.gameObject.SetActive(state.ShowFireButton && fireIcon != null);
            _fireButton.interactable = state.CanFire;
            _firePanel.color = FireControlVisual;
            _fireIcon.color = Color.white;

            _weaponResetButton.gameObject.SetActive(state.HasSelectedWeapon);
            _weaponResetButton.interactable = state.HasSelectedWeapon && state.CanOpenArsenal;

            RenderTutorialHighlight(state.TutorialTargetId);
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
            BuildWeaponControls(root);
            BuildResultOverlay(root);
        }

        private void BuildTopLayer(Transform root)
        {
            _statusPanel = CreatePanel("Tutorial Card", root, new Vector2(0f, 1f), new Vector2(0f, 1f), new Vector2(0f, 1f),
                new Vector2(500f, 500f), new Vector2(SafeMargin, TutorialCardPositionY), Color.white, _tutorialCardSprite);
            _statusPanel.type = Image.Type.Simple;
            _statusPanel.preserveAspect = true;
            _statusText = CreateText("Tutorial Text", _statusPanel.transform, new Vector2(0.22f, 0.20f), new Vector2(0.86f, 0.79f),
                new Vector2(0.5f, 0.5f), Vector2.zero, Vector2.zero, 25, TextAlignmentOptions.TopLeft);
            _statusText.overflowMode = TextOverflowModes.Truncate;

            _turnPanel = CreatePanel("Turn Timer", root, new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(0.5f, 1f),
                new Vector2(246f, 180f), new Vector2(0f, TimerPositionY), Color.white, _timerPanelSprite);
            _turnPanel.type = Image.Type.Simple;
            _turnPanel.preserveAspect = false;
            _turnText = CreateText("Turn Seconds", _turnPanel.transform, new Vector2(0.24f, 0.28f), new Vector2(0.76f, 0.72f),
                new Vector2(0.5f, 0.5f), Vector2.zero, Vector2.zero, 36, TextAlignmentOptions.Center);
            SciFiTextStyle.Apply(_turnText, GameTextRole.Numeric, 36, TextAlignmentOptions.Center);

            var settingsRect = CreateRect("Settings", root, new Vector2(0.5f, 1f), new Vector2(0.5f, 1f),
                new Vector2(0.5f, 1f), new Vector2(140f, 140f), new Vector2(220f, SettingsPositionY));
            var settingsImage = settingsRect.gameObject.AddComponent<Image>();
            settingsImage.sprite = _settingsButtonSprite;
            settingsImage.type = Image.Type.Simple;
            settingsImage.preserveAspect = true;
            settingsImage.color = Color.white;
            var settingsButton = settingsRect.gameObject.AddComponent<Button>();
            settingsButton.targetGraphic = settingsImage;
            settingsButton.colors = CreateButtonColors(Color.white);
            settingsButton.onClick.AddListener(() => SettingsRequested?.Invoke());
        }

        private void BuildMovementControls(Transform root)
        {
            var jumpBottom = MovementBottomMargin + WalkButtonSize + MovementButtonGap;
            _moveLeftButton = CreateMovementButton("Move Left", root, new Vector2(0f, 0f), new Vector2(0f, 0f),
                WalkButtonSize, new Vector2(36f + WalkButtonSize, MovementBottomMargin), _moveButtonSprite, true, null);
            _moveLeftPanel = _moveLeftButton.GetComponent<Image>();
            WireHold(_moveLeftButton.gameObject, () => MoveLeftStarted?.Invoke(), () => MoveLeftStopped?.Invoke());

            _jumpLeftButton = CreateMovementButton("Jump Left", root, new Vector2(0f, 0f), new Vector2(0f, 0f),
                JumpButtonSize, new Vector2(58f + JumpButtonSize, jumpBottom), _jumpButtonSprite, true, () => JumpLeftRequested?.Invoke());
            _jumpLeftPanel = _jumpLeftButton.GetComponent<Image>();

            _moveRightButton = CreateMovementButton("Move Right", root, new Vector2(1f, 0f), new Vector2(1f, 0f),
                WalkButtonSize, new Vector2(-36f, MovementBottomMargin), _moveButtonSprite, false, null);
            _moveRightPanel = _moveRightButton.GetComponent<Image>();
            WireHold(_moveRightButton.gameObject, () => MoveRightStarted?.Invoke(), () => MoveRightStopped?.Invoke());

            _jumpRightButton = CreateMovementButton("Jump Right", root, new Vector2(1f, 0f), new Vector2(1f, 0f),
                JumpButtonSize, new Vector2(-58f, jumpBottom), _jumpButtonSprite, false, () => JumpRightRequested?.Invoke());
            _jumpRightPanel = _jumpRightButton.GetComponent<Image>();
        }

        private void BuildWeaponControls(Transform root)
        {
            _arsenalPanel = CreatePanel("Arsenal", root, new Vector2(1f, 1f), new Vector2(1f, 1f), new Vector2(1f, 1f),
                new Vector2(ArsenalButtonSize, ArsenalButtonSize), new Vector2(-24f, ArsenalPositionY), Color.white, _arsenalButtonSprite);
            _arsenalPanel.type = Image.Type.Simple;
            _arsenalPanel.preserveAspect = true;
            DisableGraphicEffects(_arsenalPanel);
            _arsenalButton = _arsenalPanel.gameObject.AddComponent<Button>();
            _arsenalButton.targetGraphic = _arsenalPanel;
            _arsenalButton.transition = Selectable.Transition.None;
            _arsenalButton.onClick.AddListener(ToggleArsenal);

            var weaponButtonCenterY = MovementBottomMargin + WalkButtonSize * 0.5f;
            var fireButtonCenterY = MovementBottomMargin + WalkButtonSize + MovementButtonGap + JumpButtonSize + 198f;
            var fireRect = CreateRect("Fire Weapon", root, new Vector2(1f, 0f), new Vector2(1f, 0f),
                new Vector2(0.5f, 0.5f), new Vector2(WeaponButtonSize, WeaponButtonSize), new Vector2(-148f, fireButtonCenterY));
            _firePanel = fireRect.gameObject.AddComponent<Image>();
            _firePanel.sprite = _fireRingSprite;
            _firePanel.type = Image.Type.Simple;
            _firePanel.preserveAspect = true;
            _firePanel.color = FireControlVisual;
            _fireButton = _firePanel.gameObject.AddComponent<Button>();
            _fireButton.targetGraphic = _firePanel;
            _fireButton.transition = Selectable.Transition.None;
            _fireButton.onClick.AddListener(() => FireRequested?.Invoke());

            var fireIconRect = CreateRect("Selected Weapon Icon", _firePanel, Vector2.zero, Vector2.one,
                new Vector2(0.5f, 0.5f), new Vector2(FireWeaponIconSize, FireWeaponIconSize), new Vector2(0f, -22f));
            fireIconRect.localScale = new Vector3(0.8f, 0.8f, 1f);
            _fireIcon = fireIconRect.gameObject.AddComponent<Image>();
            _fireIcon.type = Image.Type.Simple;
            _fireIcon.preserveAspect = true;
            _fireIcon.color = Color.white;
            _fireIcon.raycastTarget = false;
            _firePanel.gameObject.SetActive(false);

            var resetRect = CreateRect("Reset Weapon", root, new Vector2(0.25f, 0f), new Vector2(0.25f, 0f),
                new Vector2(0.5f, 0.5f), new Vector2(WeaponButtonSize, WeaponButtonSize), new Vector2(0f, weaponButtonCenterY));
            var resetImage = resetRect.gameObject.AddComponent<Image>();
            resetImage.sprite = _resetButtonSprite;
            resetImage.type = Image.Type.Simple;
            resetImage.preserveAspect = true;
            resetImage.color = ControlVisual;
            _weaponResetButton = resetRect.gameObject.AddComponent<Button>();
            _weaponResetButton.targetGraphic = resetImage;
            _weaponResetButton.transition = Selectable.Transition.None;
            _weaponResetButton.onClick.AddListener(() => WeaponResetRequested?.Invoke());
            _weaponResetButton.gameObject.SetActive(false);

            var overlayRect = CreateRect("Arsenal Overlay", root, Vector2.zero, Vector2.one, new Vector2(0.5f, 0.5f),
                Vector2.zero, Vector2.zero);
            var overlay = overlayRect.gameObject.AddComponent<Image>();
            overlay.sprite = _whiteSprite;
            overlay.color = new Color(0f, 0f, 0f, 0.38f);
            overlay.raycastTarget = true;
            _arsenalRoot = overlay.gameObject;
            var overlayButton = overlay.gameObject.AddComponent<Button>();
            overlayButton.targetGraphic = overlay;
            overlayButton.transition = Selectable.Transition.None;
            overlayButton.onClick.AddListener(() => SetArsenalVisible(false));

            _arsenalWindow = CreatePanel("Arsenal Window", overlay.transform, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                new Vector2(0.5f, 0.5f), new Vector2(1640f, 1120f), new Vector2(0f, -22f), new Color32(255, 255, 255, 183), _weaponsCardSprite);
            _arsenalWindow.type = Image.Type.Simple;
            _arsenalWindow.preserveAspect = false;
            DisableGraphicEffects(_arsenalWindow);

            _arsenalItemsRoot = CreateRect("Arsenal Items", _arsenalWindow, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                new Vector2(0.5f, 0.5f), new Vector2(1320f, 820f), new Vector2(0f, 12f));
            CreateArsenalCloseButton(_arsenalWindow.transform);
            _arsenalRoot.SetActive(false);
        }

        private void ToggleArsenal()
        {
            if (_arsenalRoot != null && _arsenalButton != null && _arsenalButton.interactable)
            {
                SetArsenalVisible(!_arsenalRoot.activeSelf);
            }
        }

        private void SetArsenalVisible(bool visible)
        {
            if (_arsenalRoot == null || _arsenalRoot.activeSelf == visible)
            {
                return;
            }

            _arsenalRoot.SetActive(visible);
            ArsenalVisibilityChanged?.Invoke(visible);
        }

        private void CreateArsenalCloseButton(Transform parent)
        {
            var rect = CreateRect("Close Arsenal", parent, new Vector2(1f, 1f), new Vector2(1f, 1f),
                new Vector2(0.5f, 0.5f), new Vector2(132f, 132f), new Vector2(-168f, -132f));
            var hitArea = rect.gameObject.AddComponent<Image>();
            hitArea.sprite = _whiteSprite;
            hitArea.color = new Color(1f, 1f, 1f, 0f);
            hitArea.raycastTarget = true;
            var button = rect.gameObject.AddComponent<Button>();
            button.targetGraphic = hitArea;
            button.transition = Selectable.Transition.None;
            button.onClick.AddListener(() => SetArsenalVisible(false));

            var label = CreateText("Close Arsenal Label", rect, Vector2.zero, Vector2.one, new Vector2(0.5f, 0.5f),
                Vector2.zero, Vector2.zero, 87, TextAlignmentOptions.Center);
            label.text = "\u00D7";
            label.rectTransform.localScale = new Vector3(1.1f, 1.1f, 1f);
            label.color = new Color(1f, 0.18f, 0.16f, 1f);
            label.raycastTarget = false;
        }

        private void RenderArsenal(in BattleHudState state)
        {
            if (_arsenalRoot == null)
            {
                return;
            }

            if (!state.CanOpenArsenal)
            {
                SetArsenalVisible(false);
            }

            if (ReferenceEquals(_renderedArsenalWeapons, state.ArsenalWeapons)
                && _renderedSelectedWeaponIndex == state.SelectedWeaponIndex)
            {
                return;
            }

            _renderedArsenalWeapons = state.ArsenalWeapons;
            _renderedSelectedWeaponIndex = state.SelectedWeaponIndex;
            for (var index = _arsenalItemsRoot.childCount - 1; index >= 0; index--)
            {
                Destroy(_arsenalItemsRoot.GetChild(index).gameObject);
            }

            var realCount = state.ArsenalWeapons.Count;
            if (realCount == 0)
            {
                return;
            }

            var count = realCount;
            const int columns = 7;
            const float cardSize = 176f;
            const float cardScaleX = 1.2f;
            const float cardScaleY = 0.9f;
            const float visualCardWidth = cardSize * cardScaleX;
            const float visualCardHeight = cardSize * cardScaleY;
            const float columnGap = -18f;
            const float rowGap = 18f;
            var totalWidth = columns * visualCardWidth + (columns - 1) * columnGap;
            const float totalHeight = 4f * visualCardHeight + 3f * rowGap;
            var startX = -totalWidth * 0.5f + visualCardWidth * 0.5f;
            var startY = totalHeight * 0.5f - visualCardHeight * 0.5f;
            for (var index = 0; index < count; index++)
            {
                var item = state.ArsenalWeapons[index];
                var row = index / columns;
                var column = index % columns;
                var position = new Vector2(startX + column * (visualCardWidth + columnGap), startY - row * (visualCardHeight + rowGap));
                var slot = CreatePanel($"Weapon {item.Index + 1}", _arsenalItemsRoot, new Vector2(0.5f, 0.5f),
                    new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(cardSize, cardSize),
                    position, item.IsUsable ? Color.white : new Color(1f, 1f, 1f, 0.48f), _weaponMiniCardSprite);
                slot.type = Image.Type.Simple;
                slot.preserveAspect = true;
                slot.rectTransform.localScale = new Vector3(cardScaleX, cardScaleY, 1f);
                DisableGraphicEffects(slot);

                var button = slot.gameObject.AddComponent<Button>();
                button.targetGraphic = slot;
                button.interactable = item.IsUsable;
                button.colors = CreateButtonColors(Color.white);
                var weaponIndex = item.Index;
                button.onClick.AddListener(() =>
                {
                    SetArsenalVisible(false);
                    WeaponSelected?.Invoke(weaponIndex);
                });

                var icon = CreatePanel("Weapon Icon", slot.transform, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                    new Vector2(0.5f, 0.5f), new Vector2(156f, 156f), new Vector2(0f, 0f), item.IsUsable ? Color.white : new Color(1f, 1f, 1f, 0.42f),
                    LoadWeaponSprite(item.IconId));
                icon.type = Image.Type.Simple;
                icon.preserveAspect = true;
                icon.raycastTarget = false;
                DisableGraphicEffects(icon);

                var badgeText = CreateText("Ammo Count", slot.transform, new Vector2(1f, 1f), new Vector2(1f, 1f),
                    new Vector2(0.5f, 0.5f), new Vector2(74f, 48f), new Vector2(-45f, -24f), 26, TextAlignmentOptions.Center);
                badgeText.text = item.HasLimitedAmmo ? Mathf.Max(0, item.AmmoRemaining).ToString() : "\u221E";
                badgeText.fontStyle = FontStyles.Bold;
                badgeText.color = item.IsUsable ? Color.white : new Color(1f, 1f, 1f, 0.56f);
                badgeText.raycastTarget = false;
            }
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
            SciFiTextStyle.Apply(_resultTitleText, GameTextRole.DisplayTitle, 47, TextAlignmentOptions.Center);
            _resultSubtitleText = CreateText("Result Subtitle", _resultPanel.transform, new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(0.5f, 1f),
                new Vector2(-96f, 46f), new Vector2(0f, -112f), 27, TextAlignmentOptions.Center);
            _resultOutcomeText = CreateText("Result Outcome", _resultPanel.transform, new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(0.5f, 1f),
                new Vector2(-110f, 42f), new Vector2(0f, -172f), 24, TextAlignmentOptions.Center);
            _resultRewardText = CreateText("Result Reward", _resultPanel.transform, new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(0.5f, 1f),
                new Vector2(-110f, 42f), new Vector2(0f, -220f), 24, TextAlignmentOptions.Center);
            _resultUnlockText = CreateText("Result Unlock", _resultPanel.transform, new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(0.5f, 1f),
                new Vector2(-110f, 42f), new Vector2(0f, -268f), 23, TextAlignmentOptions.Center);
            _resultUnlockText.color = SciFiTextStyle.CyanText;
            _resultContinueNoteText = CreateText("Result Continue Note", _resultPanel.transform, new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(0.5f, 1f),
                new Vector2(-120f, 34f), new Vector2(0f, -314f), 19, TextAlignmentOptions.Center);
            _resultContinueNoteText.color = SciFiTextStyle.SecondaryText;

            var retry = CreateGameButton("Result Retry", _resultPanel.transform, new Vector2(0f, 0f), new Vector2(0f, 0f), new Vector2(286f, 78f),
                new Vector2(44f, 36f), Gold, "R", 30, GameText.Restart, 28, () => RestartRequested?.Invoke());
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
            _resultContinueLabel.color = SciFiTextStyle.MainText;
        }

        private void RenderTutorialHighlight(string tutorialTargetId)
        {
            var pulse = 0.25f + Mathf.PingPong(Time.unscaledTime * 0.95f, 0.5f);
            ApplyHighlight(_statusPanel, TargetsStatus(tutorialTargetId), Color.white, pulse);
            ApplyHighlight(_turnPanel, tutorialTargetId == "battle.turn-label", Color.white, pulse);
            ApplyHighlight(_resultPanel, tutorialTargetId == "battle.result", InkSolid, pulse);

        }

        private static void SetControlState(Button button, Image panel, bool enabled)
        {
            if (button != null)
            {
                button.interactable = enabled;
            }

            if (panel != null)
            {
                panel.color = ControlVisual;
            }
        }

        private Sprite LoadWeaponSprite(string iconId)
        {
            iconId ??= string.Empty;
            if (!_weaponSprites.TryGetValue(iconId, out var sprite))
            {
                sprite = LoadResourceSprite(WeaponResourceRoot + iconId, Vector4.zero);
                _weaponSprites[iconId] = sprite;
            }

            return sprite;
        }

        private Button CreateMovementButton(string name, Component parent, Vector2 anchor, Vector2 pivot, float size,
            Vector2 anchoredPosition, Sprite sprite, bool mirrorHorizontally, Action onClick)
        {
            var image = CreatePanel(name, parent, anchor, anchor, pivot, new Vector2(size, size), anchoredPosition, ControlVisual, sprite ?? _circleSprite);
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
            Vector2 anchoredPosition, Color color, string icon, int iconSize, string label, int labelSize, Action onClick,
            Vector2? pivotOverride = null)
        {
            var pivot = pivotOverride ?? new Vector2(anchorMin.x, anchorMin.y);
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

        private static void DisableGraphicEffects(Graphic graphic)
        {
            foreach (var effect in graphic.GetComponents<Shadow>())
            {
                effect.enabled = false;
            }
        }

        private TMP_Text CreateText(string name, Component parent, Vector2 anchorMin, Vector2 anchorMax, Vector2 pivot,
            Vector2 sizeDelta, Vector2 anchoredPosition, int fontSize, TextAlignmentOptions alignment)
        {
            var rect = CreateRect(name, parent, anchorMin, anchorMax, pivot, sizeDelta, anchoredPosition);
            var text = rect.gameObject.AddComponent<TextMeshProUGUI>();
            var role = fontSize >= 42 ? GameTextRole.ScreenTitle : GameTextRole.Body;
            SciFiTextStyle.Apply(text, role, fontSize, alignment);
            text.textWrappingMode = TextWrappingModes.Normal;
            text.overflowMode = TextOverflowModes.Truncate;

            var shadow = rect.gameObject.AddComponent<Shadow>();
            shadow.effectColor = SciFiTextStyle.ShadowText;
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

        private static Sprite CreateCircleFrameSprite(int size, float thickness)
        {
            var texture = new Texture2D(size, size, TextureFormat.RGBA32, false)
            {
                name = "Battle Fire Ring",
                filterMode = FilterMode.Bilinear,
                wrapMode = TextureWrapMode.Clamp
            };

            var pixels = new Color32[size * size];
            var center = (size - 1) * 0.5f;
            var outerRadius = center - 1f;
            var innerRadius = outerRadius - Mathf.Max(1f, thickness);
            for (var y = 0; y < size; y++)
            {
                for (var x = 0; x < size; x++)
                {
                    var distance = Vector2.Distance(new Vector2(x, y), new Vector2(center, center));
                    var outerAlpha = Mathf.Clamp01(outerRadius + 0.5f - distance);
                    var innerAlpha = Mathf.Clamp01(distance - innerRadius + 0.5f);
                    pixels[y * size + x] = new Color(1f, 1f, 1f, outerAlpha * innerAlpha);
                }
            }

            texture.SetPixels32(pixels);
            texture.Apply(false, false);
            var sprite = Sprite.Create(
                texture,
                new Rect(0f, 0f, size, size),
                new Vector2(0.5f, 0.5f),
                100f,
                0,
                SpriteMeshType.FullRect);
            sprite.name = "Battle Fire Ring";
            return sprite;
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
                highlightedColor = Color.white,
                pressedColor = Color.white,
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
