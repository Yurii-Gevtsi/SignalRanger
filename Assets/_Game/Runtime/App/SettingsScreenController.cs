using System;
using Game.Core;
using Game.Core.Localization;
using Game.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.App
{
    /// <summary>
    /// Modal settings screen built on demand from the menu. Adjusts music/SFX
    /// volume through <see cref="Game.Audio.IAudioService"/> and persists to save.
    /// Uses stepped +/- controls (touch-friendly, no slider wiring needed).
    /// </summary>
    public sealed class SettingsScreenController : MonoBehaviour
    {
        private const string BattleResourceRoot = "UI/Battle/";
        private const string ButtonBlueResource = "UI/Shop/btn_blue";
        private const string ButtonGreenResource = "UI/Shop/btn_green";
        private const string ButtonRedResource = "UI/Shop/btn_red";
        private const string ButtonYellowResource = "UI/Shop/btn_yellow";

        private static readonly Color OverlayColor = new(0.04f, 0.06f, 0.08f, 0.92f);
        private static readonly Color PanelColor = new(0.10f, 0.13f, 0.16f, 1f);
        private static readonly Color StepColor = new(0.20f, 0.26f, 0.32f);
        private static readonly Color BackColor = new(0.30f, 0.20f, 0.22f);
        private static readonly Color ResetColor = new(0.58f, 0.20f, 0.18f);
        private static readonly Color ConfirmationOverlayColor = new(0.02f, 0.03f, 0.04f, 0.88f);

        private Sprite _whiteSprite;
        private Action _onClosed;
        private Action _onRestart;
        private Action _onMenu;
        private GameObject _resetConfirmationRoot;

        private float _music;
        private float _sfx;
        private TMP_Text _musicValue;
        private TMP_Text _sfxValue;
        private Image _musicToggleImage;
        private Image _sfxToggleImage;

        public static void Open(Action onClosed, Action onRestart = null, Action onMenu = null)
        {
            if (FindAnyObjectByType<SettingsScreenController>() != null)
            {
                return;
            }

            var root = new GameObject("Settings Screen");
            root.SetActive(false);
            var controller = root.AddComponent<SettingsScreenController>();
            controller._onClosed = onClosed;
            controller._onRestart = onRestart;
            controller._onMenu = onMenu;
            root.SetActive(true);
        }

        private void Awake()
        {
            _whiteSprite = Sprite.Create(Texture2D.whiteTexture, new Rect(0f, 0f, 1f, 1f), new Vector2(0.5f, 0.5f), 1f);
            _music = Mathf.Clamp01(AppServices.Audio.MusicVolume);
            _sfx = Mathf.Clamp01(AppServices.Audio.SfxVolume);
            Build();
        }

        private void Build()
        {
            var canvasObject = new GameObject("Settings Canvas");
            canvasObject.transform.SetParent(transform, false);
            var canvas = canvasObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 150;

            var scaler = canvasObject.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920f, 1080f);
            scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            scaler.matchWidthOrHeight = 0.5f;
            canvasObject.AddComponent<GraphicRaycaster>();

            var root = canvasObject.transform;
            var overlay = CreatePanel("Overlay", root, Vector2.zero, Vector2.one, new Vector2(0.5f, 0.5f),
                Vector2.zero, Vector2.zero, OverlayColor);
            var overlayButton = overlay.gameObject.AddComponent<Button>();
            overlayButton.targetGraphic = overlay;
            overlayButton.transition = Selectable.Transition.None;
            overlayButton.onClick.AddListener(Close);

            var panel = CreatePanel("Settings Panel", overlay.transform, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                new Vector2(0.5f, 0.5f), new Vector2(980f, 1080f), new Vector2(0f, -20f), Color.white,
                LoadResourceSprite(BattleResourceRoot + "card", new Vector4(250f, 250f, 250f, 250f)));
            panel.type = Image.Type.Sliced;
            panel.preserveAspect = false;
            var panelBlocker = panel.gameObject.AddComponent<Button>();
            panelBlocker.targetGraphic = panel;
            panelBlocker.transition = Selectable.Transition.None;

            CreateText("Title", panel.transform, new Vector2(0.18f, 0.70f), new Vector2(0.82f, 0.78f), new Vector2(0.5f, 0.5f),
                Vector2.zero, Vector2.zero, 48, TextAlignmentOptions.Center, GameText.SettingsTitle(), GameTextRole.DisplayTitle);
            CreateCloseButton(panel.transform);

            CreateToggleRow(panel.transform, 0.58f, GameText.Music(), out _musicValue, out _musicToggleImage, ToggleMusic);
            CreateToggleRow(panel.transform, 0.48f, GameText.Sounds(), out _sfxValue, out _sfxToggleImage, ToggleSfx);

            CreateText("Language", panel.transform, new Vector2(0.18f, 0.39f), new Vector2(0.82f, 0.45f), new Vector2(0.5f, 0.5f),
                Vector2.zero, Vector2.zero, 26, TextAlignmentOptions.Center, GameText.LanguageLabel());

            if (_onRestart != null || _onMenu != null)
            {
                CreateButton("Restart Battle", panel.transform, new Vector2(0.5f, 0f), new Vector2(0.5f, 0f),
                    new Vector2(210f, 104f), new Vector2(-120f, 275f), ResetColor, GameText.Restart, 30,
                    RestartBattle, ButtonYellowResource);
                CreateButton("Exit To Menu", panel.transform, new Vector2(0.5f, 0f), new Vector2(0.5f, 0f),
                    new Vector2(210f, 104f), new Vector2(120f, 275f), BackColor, GameText.Menu, 30,
                    ExitBattleToMenu, ButtonBlueResource);
            }
            else
            {
                CreateButton("Reset Progression", panel.transform, new Vector2(0.5f, 0f), new Vector2(0.5f, 0f),
                    new Vector2(400f, 104f), new Vector2(0f, 260f), ResetColor, GameText.ResetProgressionLabel(), 28,
                    () => ShowResetConfirmation(panel.transform), ButtonRedResource);
            }

            RefreshValues();
        }

        private void CreateToggleRow(Transform parent, float centerY, string label, out TMP_Text valueText, out Image buttonImage, Action onToggle)
        {
            CreateText($"{label} Label", parent, new Vector2(0.24f, centerY - 0.045f), new Vector2(0.49f, centerY + 0.045f),
                new Vector2(0.5f, 0.5f), Vector2.zero, Vector2.zero, 34, TextAlignmentOptions.MidlineLeft, label);

            var buttonPanel = CreatePanel($"{label} Toggle", parent, new Vector2(0.50f, centerY - 0.065f), new Vector2(0.74f, centerY + 0.065f),
                new Vector2(0.5f, 0.5f), Vector2.zero, Vector2.zero, Color.white, LoadResourceSprite(ButtonGreenResource, Vector4.zero));
            buttonPanel.preserveAspect = false;
            buttonImage = buttonPanel;
            var button = buttonPanel.gameObject.AddComponent<Button>();
            button.targetGraphic = buttonPanel;
            button.onClick.AddListener(() => onToggle?.Invoke());
            valueText = CreateText($"{label} Value", buttonPanel.transform, Vector2.zero, Vector2.one, new Vector2(0.5f, 0.5f),
                Vector2.zero, Vector2.zero, 30, TextAlignmentOptions.Center, string.Empty);
        }

        private void ToggleMusic()
        {
            _music = _music > 0f ? 0f : 1f;
            RefreshValues();
            SaveSettings();
        }

        private void ToggleSfx()
        {
            _sfx = _sfx > 0f ? 0f : 1f;
            RefreshValues();
            SaveSettings();
        }

        private void RefreshValues()
        {
            if (_musicValue != null)
            {
                var enabled = _music > 0f;
                _musicValue.text = enabled ? "Вкл" : "Викл";
                if (_musicToggleImage != null)
                {
                    _musicToggleImage.sprite = LoadResourceSprite(enabled ? ButtonGreenResource : ButtonRedResource, Vector4.zero);
                }
            }

            if (_sfxValue != null)
            {
                var enabled = _sfx > 0f;
                _sfxValue.text = enabled ? "Вкл" : "Викл";
                if (_sfxToggleImage != null)
                {
                    _sfxToggleImage.sprite = LoadResourceSprite(enabled ? ButtonGreenResource : ButtonRedResource, Vector4.zero);
                }
            }
        }

        private void ShowResetConfirmation(Transform parent)
        {
            if (_resetConfirmationRoot != null)
            {
                return;
            }

            var overlay = CreatePanel("Reset Confirmation", parent, Vector2.zero, Vector2.one, new Vector2(0.5f, 0.5f),
                Vector2.zero, Vector2.zero, ConfirmationOverlayColor);
            _resetConfirmationRoot = overlay.gameObject;

            var dialog = CreatePanel("Confirmation Dialog", overlay.transform, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                new Vector2(0.5f, 0.5f), new Vector2(700f, 430f), Vector2.zero, PanelColor);
            CreateText("Confirmation Title", dialog.transform, new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(0.5f, 1f),
                new Vector2(-80f, 64f), new Vector2(0f, -52f), 34, TextAlignmentOptions.Center, GameText.ResetProgressionTitle(), GameTextRole.DisplayTitle);
            CreateText("Confirmation Warning", dialog.transform, new Vector2(0f, 0.5f), new Vector2(1f, 0.5f), new Vector2(0.5f, 0.5f),
                new Vector2(-100f, 150f), new Vector2(0f, 16f), 24, TextAlignmentOptions.Center, GameText.ResetProgressionWarning());

            CreateButton("Confirm Reset", dialog.transform, new Vector2(0.5f, 0f), new Vector2(0.5f, 0f),
                new Vector2(280f, 84f), new Vector2(-154f, 62f), ResetColor, GameText.ConfirmResetProgression(), 26,
                ConfirmResetProgression, ButtonRedResource);
            CreateButton("Cancel Reset", dialog.transform, new Vector2(0.5f, 0f), new Vector2(0.5f, 0f),
                new Vector2(280f, 84f), new Vector2(154f, 62f), StepColor, GameText.Cancel(), 26,
                HideResetConfirmation, ButtonBlueResource);
        }

        private void HideResetConfirmation()
        {
            if (_resetConfirmationRoot == null)
            {
                return;
            }

            Destroy(_resetConfirmationRoot);
            _resetConfirmationRoot = null;
        }

        private void ConfirmResetProgression()
        {
            AppServices.Audio.SetVolumes(_music, _sfx);
            AppServices.ResetProgression();
            _onClosed = null;
            Destroy(gameObject);
            AppServices.SceneLoader.Load(GameSceneNames.Menu);
        }

        private void RestartBattle()
        {
            CloseForBattleAction(_onRestart);
        }

        private void ExitBattleToMenu()
        {
            CloseForBattleAction(_onMenu);
        }

        private void CloseForBattleAction(Action action)
        {
            SaveSettings();
            _onClosed = null;
            _onRestart = null;
            _onMenu = null;
            Destroy(gameObject);
            action?.Invoke();
        }

        private void SaveSettings()
        {
            AppServices.Audio.SetVolumes(_music, _sfx);
            AppServices.SaveService.Save(AppServices.SaveService.Current);
        }

        private void Close()
        {
            SaveSettings();
            var callback = _onClosed;
            _onClosed = null;
            _onRestart = null;
            _onMenu = null;
            Destroy(gameObject);
            callback?.Invoke();
        }

        private Image CreatePanel(string name, Component parent, Vector2 anchorMin, Vector2 anchorMax, Vector2 pivot,
            Vector2 sizeDelta, Vector2 anchoredPosition, Color color, Sprite sprite = null)
        {
            var rect = CreateRect(name, parent, anchorMin, anchorMax, pivot, sizeDelta, anchoredPosition);
            var image = rect.gameObject.AddComponent<Image>();
            image.sprite = sprite ?? _whiteSprite;
            image.color = color;
            return image;
        }

        private TMP_Text CreateText(string name, Component parent, Vector2 anchorMin, Vector2 anchorMax, Vector2 pivot,
            Vector2 sizeDelta, Vector2 anchoredPosition, int fontSize, TextAlignmentOptions alignment, string value,
            GameTextRole? roleOverride = null)
        {
            var rect = CreateRect(name, parent, anchorMin, anchorMax, pivot, sizeDelta, anchoredPosition);
            var text = rect.gameObject.AddComponent<TextMeshProUGUI>();
            var role = roleOverride ?? (fontSize >= 40 ? GameTextRole.ScreenTitle : GameTextRole.Body);
            SciFiTextStyle.Apply(text, role, fontSize, alignment);
            text.textWrappingMode = TextWrappingModes.Normal;
            text.overflowMode = TextOverflowModes.Overflow;
            text.text = value;
            return text;
        }

        private Button CreateButton(string name, Component parent, Vector2 anchorMin, Vector2 anchorMax,
            Vector2 size, Vector2 anchoredPosition, Color color, string label, int fontSize, Action onClick,
            string resourcePath = ButtonBlueResource)
        {
            var useStandardButton = name != "Close Settings";
            var image = CreatePanel(name, parent, anchorMin, anchorMax, new Vector2(0.5f, 0.5f), size, anchoredPosition,
                useStandardButton ? Color.white : color,
                useStandardButton ? LoadResourceSprite(resourcePath, Vector4.zero) : null);
            image.preserveAspect = false;
            var button = image.gameObject.AddComponent<Button>();
            button.targetGraphic = image;
            button.onClick.AddListener(() => onClick?.Invoke());
            CreateText($"{name} Label", image.transform, Vector2.zero, Vector2.one, new Vector2(0.5f, 0.5f),
                Vector2.zero, Vector2.zero, fontSize, TextAlignmentOptions.Center, label);
            return button;
        }

        private void CreateCloseButton(Transform parent)
        {
            var rect = CreateRect("Close Settings", parent, Vector2.zero, Vector2.one,
                new Vector2(0.5f, 0.5f), Vector2.zero, Vector2.zero);
            rect.offsetMin = new Vector2(450f, 620f);
            rect.offsetMax = new Vector2(0f, 0f);
            var hitArea = rect.gameObject.AddComponent<Image>();
            hitArea.sprite = _whiteSprite;
            hitArea.color = new Color(1f, 1f, 1f, 0f);
            hitArea.raycastTarget = true;
            var button = rect.gameObject.AddComponent<Button>();
            button.targetGraphic = hitArea;
            button.transition = Selectable.Transition.None;
            button.onClick.AddListener(Close);

            var label = CreateText("Close Settings Label", rect, Vector2.zero, Vector2.one, new Vector2(0.5f, 0.5f),
                Vector2.zero, Vector2.zero, 72, TextAlignmentOptions.Center, "×");
            label.rectTransform.localScale = new Vector3(1.5f, 1.5f, 1f);
            label.color = new Color(1f, 0.18f, 0.16f, 1f);
            label.raycastTarget = false;
        }

        private static Sprite LoadResourceSprite(string path, Vector4 border)
        {
            var texture = Resources.Load<Texture2D>(path);
            return texture == null
                ? null
                : Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100f, 0, SpriteMeshType.FullRect, border);
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
