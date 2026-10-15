using System;
using Game.Core;
using Game.Core.Localization;
using System.Collections.Generic;
using Game.UI;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
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
        private const string FlagResourceRoot = "UI/Flags/CircleFlags/";

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
        private Action _onLanguageSelected;
        private GameObject _resetConfirmationRoot;
        private GameObject _languageOptionsRoot;
        private GameObject _canvasObject;
        private ResourceInfoPopupController _resourceInfoPopup;
        private bool _isLanguagePickerOnly;

        private float _music;
        private float _sfx;
        private bool _adminMode;
        private TMP_Text _musicValue;
        private TMP_Text _sfxValue;
        private TMP_Text _adminModeValue;
        private Image _musicToggleImage;
        private Image _sfxToggleImage;
        private Image _adminModeToggleImage;

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

        public static void OpenLanguagePicker(Action onLanguageSelected)
        {
            if (FindAnyObjectByType<SettingsScreenController>() != null)
            {
                return;
            }

            var root = new GameObject("Initial Language Picker");
            root.SetActive(false);
            var controller = root.AddComponent<SettingsScreenController>();
            controller._isLanguagePickerOnly = true;
            controller._onLanguageSelected = onLanguageSelected;
            root.SetActive(true);
        }

        private void Awake()
        {
            _whiteSprite = Sprite.Create(Texture2D.whiteTexture, new Rect(0f, 0f, 1f, 1f), new Vector2(0.5f, 0.5f), 1f);
            _music = Mathf.Clamp01(AppServices.Audio.MusicVolume);
            _sfx = Mathf.Clamp01(AppServices.Audio.SfxVolume);
            _adminMode = AppServices.SaveService?.Current?.settings?.adminModeEnabled ?? false;
            if (_isLanguagePickerOnly)
            {
                BuildLanguagePickerCanvas();
            }
            else
            {
                Build();
            }
        }

        private void BuildLanguagePickerCanvas()
        {
            EnsureEventSystem();
            var canvasObject = new GameObject("Initial Language Picker Canvas");
            _canvasObject = canvasObject;
            canvasObject.transform.SetParent(transform, false);
            var canvas = canvasObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 1100;

            var scaler = canvasObject.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920f, 1080f);
            scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            scaler.matchWidthOrHeight = 0.5f;
            canvasObject.AddComponent<GraphicRaycaster>();

            ShowLanguageOptions();
        }

        private static void EnsureEventSystem()
        {
            if (FindAnyObjectByType<EventSystem>() != null)
            {
                return;
            }

            var eventSystem = new GameObject("Language EventSystem");
            eventSystem.AddComponent<EventSystem>();
            eventSystem.AddComponent<InputSystemUIInputModule>();
        }

        private void Build()
        {
            var canvasObject = new GameObject("Settings Canvas");
            _canvasObject = canvasObject;
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
            _resourceInfoPopup = ResourceInfoPopupController.Create(root);
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
            if (!_isLanguagePickerOnly)
            {
                var panelBlocker = panel.gameObject.AddComponent<Button>();
                panelBlocker.targetGraphic = panel;
                panelBlocker.transition = Selectable.Transition.None;
            }

            CreateText("Title", panel.transform, new Vector2(0.18f, 0.70f), new Vector2(0.82f, 0.78f), new Vector2(0.5f, 0.5f),
                Vector2.zero, Vector2.zero, 48, TextAlignmentOptions.Center, T("ui.settings"), GameTextRole.DisplayTitle);
            CreateCloseButton(panel.transform);

            CreateToggleRow(panel.transform, 0.64f, T("ui.music"), out _musicValue, out _musicToggleImage, ToggleMusic);
            CreateToggleRow(panel.transform, 0.53f, T("ui.sounds"), out _sfxValue, out _sfxToggleImage, ToggleSfx);

            CreateLanguageRow(panel.transform);
            CreateToggleRow(panel.transform, 0.32f, T("ui.adminMode"), out _adminModeValue, out _adminModeToggleImage, ToggleAdminMode);

            if (_onRestart != null || _onMenu != null)
            {
                CreateButton("Restart Battle", panel.transform, new Vector2(0.5f, 0f), new Vector2(0.5f, 0f),
                    new Vector2(252f, 125f), new Vector2(-138f, 275f), ResetColor, T("ui.restart"), 30,
                    RestartBattle, ButtonYellowResource);
                CreateButton("Exit To Menu", panel.transform, new Vector2(0.5f, 0f), new Vector2(0.5f, 0f),
                    new Vector2(252f, 125f), new Vector2(138f, 275f), BackColor, T("ui.menu"), 30,
                    ExitBattleToMenu, ButtonBlueResource);
            }
            else
            {
                CreateButton("Reset Progression", panel.transform, new Vector2(0.5f, 0f), new Vector2(0.5f, 0f),
                    new Vector2(480f, 125f), new Vector2(0f, 260f), ResetColor, T("ui.resetProgression"), 28,
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

        private void ToggleAdminMode()
        {
            _adminMode = !_adminMode;
            RefreshValues();
            AppServices.SetAdminMode(_adminMode);
        }

        private void RefreshValues()
        {
            if (_musicValue != null)
            {
                var enabled = _music > 0f;
                _musicValue.text = enabled ? T("ui.enabled") : T("ui.disabled");
                if (_musicToggleImage != null)
                {
                    _musicToggleImage.sprite = LoadResourceSprite(enabled ? ButtonGreenResource : ButtonRedResource, Vector4.zero);
                }
            }

            if (_sfxValue != null)
            {
                var enabled = _sfx > 0f;
                _sfxValue.text = enabled ? T("ui.enabled") : T("ui.disabled");
                if (_sfxToggleImage != null)
                {
                    _sfxToggleImage.sprite = LoadResourceSprite(enabled ? ButtonGreenResource : ButtonRedResource, Vector4.zero);
                }
            }

            if (_adminModeValue != null)
            {
                _adminModeValue.text = _adminMode ? T("ui.enabled") : T("ui.disabled");
                if (_adminModeToggleImage != null)
                {
                    _adminModeToggleImage.sprite = LoadResourceSprite(_adminMode ? ButtonGreenResource : ButtonRedResource, Vector4.zero);
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
                new Vector2(-80f, 64f), new Vector2(0f, -52f), 34, TextAlignmentOptions.Center, T("ui.resetProgressionTitle"), GameTextRole.DisplayTitle);
            CreateText("Confirmation Warning", dialog.transform, new Vector2(0f, 0.5f), new Vector2(1f, 0.5f), new Vector2(0.5f, 0.5f),
                new Vector2(-100f, 150f), new Vector2(0f, 16f), 24, TextAlignmentOptions.Center, T("ui.resetProgressionWarning"));

            CreateButton("Confirm Reset", dialog.transform, new Vector2(0.5f, 0f), new Vector2(0.5f, 0f),
                new Vector2(336f, 101f), new Vector2(-182f, 62f), ResetColor, T("ui.confirmReset"), 26,
                ConfirmResetProgression, ButtonRedResource);
            CreateButton("Cancel Reset", dialog.transform, new Vector2(0.5f, 0f), new Vector2(0.5f, 0f),
                new Vector2(336f, 101f), new Vector2(182f, 62f), StepColor, T("ui.cancel"), 26,
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

        private void CreateLanguageRow(Transform parent)
        {
            CreateText("Language Label", parent, new Vector2(0.24f, 0.385f), new Vector2(0.49f, 0.475f),
                new Vector2(0.5f, 0.5f), Vector2.zero, Vector2.zero, 34, TextAlignmentOptions.MidlineLeft,
                T("ui.languageName"));

            var languageButton = CreatePanel("Language Value", parent, new Vector2(0.50f, 0.365f), new Vector2(0.74f, 0.495f),
                new Vector2(0.5f, 0.5f), Vector2.zero, Vector2.zero, Color.white,
                LoadResourceSprite(ButtonBlueResource, Vector4.zero));
            languageButton.preserveAspect = false;
            var button = languageButton.gameObject.AddComponent<Button>();
            button.targetGraphic = languageButton;
            button.onClick.AddListener(ToggleLanguageOptions);
            CreateText("Language Value Label", languageButton.transform, Vector2.zero, Vector2.one,
                new Vector2(0.5f, 0.5f), Vector2.zero, Vector2.zero, 27, TextAlignmentOptions.Center,
                AppServices.Localization.GetLanguageDisplayName());
        }

        private void ToggleLanguageOptions()
        {
            if (_languageOptionsRoot != null)
            {
                HideLanguageOptions();
                return;
            }

            if (_canvasObject == null || AppServices.Localization == null)
            {
                return;
            }

            ShowLanguageOptions();
        }

        private void ShowLanguageOptions()
        {
            if (_languageOptionsRoot != null || _canvasObject == null || AppServices.Localization == null)
            {
                return;
            }

            var overlay = CreatePanel("Language Picker Overlay", _canvasObject.transform, Vector2.zero, Vector2.one,
                new Vector2(0.5f, 0.5f), Vector2.zero, Vector2.zero, Color.clear);
            _languageOptionsRoot = overlay.gameObject;
            if (!_isLanguagePickerOnly)
            {
                var overlayButton = overlay.gameObject.AddComponent<Button>();
                overlayButton.targetGraphic = overlay;
                overlayButton.transition = Selectable.Transition.None;
                overlayButton.onClick.AddListener(HideLanguageOptions);
            }

            var panel = CreatePanel("Language Picker Panel", overlay.transform, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                new Vector2(0.5f, 0.5f), new Vector2(1470f, 1080f), new Vector2(0f, 80f), Color.white,
                LoadResourceSprite(BattleResourceRoot + "card", new Vector4(250f, 250f, 250f, 250f)));
            panel.type = Image.Type.Sliced;
            panel.preserveAspect = false;
            var panelBlocker = panel.gameObject.AddComponent<Button>();
            panelBlocker.targetGraphic = panel;
            panelBlocker.transition = Selectable.Transition.None;
            var languagePickerTitle = CreateText("Language Picker Title", panel.transform,
                new Vector2(0.18f, 0.66f), new Vector2(0.82f, 0.80f),
                new Vector2(0.5f, 0.5f), Vector2.zero, Vector2.zero, 48, TextAlignmentOptions.Center,
                T("ui.selectLanguage"), GameTextRole.DisplayTitle);
            languagePickerTitle.rectTransform.offsetMax = new Vector2(
                languagePickerTitle.rectTransform.offsetMax.x,
                70f);
            languagePickerTitle.textWrappingMode = TextWrappingModes.Normal;
            languagePickerTitle.maxVisibleLines = 2;
            if (!_isLanguagePickerOnly)
            {
                CreateLanguagePickerCloseButton(panel.transform);
            }

            var languages = AppServices.Localization.AvailableLanguageCodes;
            CreateLanguageScrollList(panel.transform, languages);
        }

        private void CreateLanguageScrollList(Transform parent, IReadOnlyList<string> languages)
        {
            var viewport = CreatePanel("Language Scroll Viewport", parent, new Vector2(0.13f, 0.16f), new Vector2(0.87f, 0.65f),
                new Vector2(0.5f, 0.5f), Vector2.zero, Vector2.zero, new Color(0f, 0f, 0f, 0.14f));
            viewport.rectTransform.offsetMin = new Vector2(100f, 60f);
            viewport.rectTransform.offsetMax = new Vector2(-100f, 60f);
            viewport.raycastTarget = true;
            viewport.gameObject.AddComponent<RectMask2D>();

            var scrollRect = viewport.gameObject.AddComponent<ScrollRect>();
            scrollRect.horizontal = false;
            scrollRect.vertical = true;
            scrollRect.movementType = ScrollRect.MovementType.Elastic;
            scrollRect.scrollSensitivity = 34f;
            scrollRect.viewport = viewport.rectTransform;

            const int columns = 2;
            var rowCount = Mathf.CeilToInt(languages.Count / (float)columns);
            var content = CreateRect("Language Scroll Content", viewport.transform, new Vector2(0f, 1f), new Vector2(1f, 1f),
                new Vector2(0.5f, 1f), new Vector2(0f, Mathf.Max(viewport.rectTransform.rect.height, rowCount * 90f + 24f)), Vector2.zero);
            scrollRect.content = content;

            for (var index = 0; index < languages.Count; index++)
            {
                var row = index / columns;
                var column = index % columns;
                var y = -54f - row * 90f;
                CreateLanguageLink(content, languages[index], column, y);
            }
        }

        private void CreateLanguageLink(Transform parent, string languageCode, int column, float y)
        {
            var columnMin = column == 0 ? 0.03f : 0.51f;
            var columnMax = column == 0 ? 0.49f : 0.97f;
            var link = CreateRect($"Language Link {languageCode}", parent, new Vector2(columnMin, 1f), new Vector2(columnMax, 1f),
                new Vector2(0.5f, 0.5f), new Vector2(0f, 68f), new Vector2(0f, y));
            var hitArea = link.gameObject.AddComponent<Image>();
            hitArea.sprite = _whiteSprite;
            hitArea.color = new Color(0.1f, 0.55f, 0.85f, 0.01f);
            var button = link.gameObject.AddComponent<Button>();
            button.targetGraphic = hitArea;
            button.transition = Selectable.Transition.ColorTint;
            button.colors = new ColorBlock
            {
                normalColor = new Color(0.1f, 0.55f, 0.85f, 0.01f),
                highlightedColor = new Color(0.16f, 0.68f, 1f, 0.2f),
                pressedColor = new Color(0.1f, 0.42f, 0.7f, 0.32f),
                selectedColor = new Color(0.16f, 0.68f, 1f, 0.2f),
                disabledColor = Color.clear,
                colorMultiplier = 1f,
                fadeDuration = 0.06f
            };
            button.onClick.AddListener(() => SelectLanguage(languageCode));

            CreateFlag(link, languageCode);
            var label = CreateText($"Language Link Label {languageCode}", link, new Vector2(0.25f, 0.5f), new Vector2(0.96f, 0.5f),
                new Vector2(0f, 0.5f), new Vector2(0f, 54f), Vector2.zero, 24, TextAlignmentOptions.MidlineLeft,
                $"<u>{AppServices.Localization.GetLanguageDisplayName(languageCode)}</u>", GameTextRole.Button);
            label.raycastTarget = false;
            label.overflowMode = TextOverflowModes.Ellipsis;
            label.color = string.Equals(languageCode, AppServices.Localization.LanguageCode, StringComparison.OrdinalIgnoreCase)
                ? new Color(1f, 0.78f, 0.24f, 1f)
                : new Color(0.40f, 0.85f, 1f, 1f);
        }

        private void CreateFlag(Transform parent, string languageCode)
        {
            var flag = CreateRect($"Flag {languageCode}", parent, new Vector2(0.06f, 0.5f), new Vector2(0.06f, 0.5f),
                new Vector2(0f, 0.5f), new Vector2(34f, 34f), Vector2.zero);
            var flagResourceName = GetFlagResourceName(languageCode);
            var flagSprite = string.IsNullOrWhiteSpace(flagResourceName)
                ? null
                : LoadResourceSprite(FlagResourceRoot + flagResourceName, Vector4.zero);
            if (flagSprite != null)
            {
                var image = flag.gameObject.AddComponent<Image>();
                image.sprite = flagSprite;
                image.preserveAspect = true;
                image.raycastTarget = false;
                return;
            }

            // A simple fallback keeps language selection usable if a future locale has no flag asset yet.
            var definition = GetFlagDefinition(languageCode);
            var border = flag.gameObject.AddComponent<Image>();
            border.sprite = _whiteSprite;
            border.color = new Color(0.92f, 0.96f, 1f, 0.88f);

            for (var index = 0; index < definition.Colors.Length; index++)
            {
                var band = CreatePanel($"Band {index}", flag, Vector2.zero, Vector2.one, new Vector2(0.5f, 0.5f),
                    Vector2.zero, Vector2.zero, definition.Colors[index]);
                var min = index / (float)definition.Colors.Length;
                var max = (index + 1) / (float)definition.Colors.Length;
                if (definition.Vertical)
                {
                    band.rectTransform.anchorMin = new Vector2(0.08f + 0.84f * min, 0.08f);
                    band.rectTransform.anchorMax = new Vector2(0.08f + 0.84f * max, 0.92f);
                }
                else
                {
                    band.rectTransform.anchorMin = new Vector2(0.08f, 0.08f + 0.84f * min);
                    band.rectTransform.anchorMax = new Vector2(0.92f, 0.08f + 0.84f * max);
                }
                band.rectTransform.offsetMin = Vector2.zero;
                band.rectTransform.offsetMax = Vector2.zero;
                band.raycastTarget = false;
            }
        }

        private static string GetFlagResourceName(string languageCode)
        {
            return languageCode.ToLowerInvariant() switch
            {
                "uk" => "ua",
                "en" => "gb",
                "pl" => "pl",
                "de" => "de",
                "es" => "es",
                "fr" => "fr",
                "it" => "it",
                "pt-br" => "br",
                "ja" => "jp",
                "ko" => "kr",
                "tr" => "tr",
                "ar" => "sa",
                "zh-hans" => "cn",
                _ => null
            };
        }

        private static FlagDefinition GetFlagDefinition(string languageCode)
        {
            return languageCode.ToLowerInvariant() switch
            {
                "uk" => new FlagDefinition(false, new Color(0f, 0.34f, 0.71f), new Color(1f, 0.84f, 0f)),
                "pl" => new FlagDefinition(false, Color.white, new Color(0.86f, 0.10f, 0.16f)),
                "de" => new FlagDefinition(false, Color.black, new Color(0.82f, 0.05f, 0.10f), new Color(1f, 0.80f, 0f)),
                "es" => new FlagDefinition(false, new Color(0.76f, 0.04f, 0.12f), new Color(1f, 0.78f, 0f), new Color(0.76f, 0.04f, 0.12f)),
                "fr" => new FlagDefinition(true, new Color(0f, 0.33f, 0.64f), Color.white, new Color(0.94f, 0.16f, 0.23f)),
                "it" => new FlagDefinition(true, new Color(0f, 0.57f, 0.30f), Color.white, new Color(0.81f, 0.10f, 0.16f)),
                "pt-br" => new FlagDefinition(false, new Color(0f, 0.55f, 0.28f), new Color(1f, 0.85f, 0f), new Color(0f, 0.20f, 0.55f)),
                "ja" => new FlagDefinition(false, Color.white, new Color(0.86f, 0.12f, 0.20f)),
                "ko" => new FlagDefinition(false, Color.white, new Color(0f, 0.30f, 0.72f), new Color(0.82f, 0.12f, 0.18f)),
                "tr" => new FlagDefinition(false, new Color(0.80f, 0.05f, 0.10f), Color.white),
                "ar" => new FlagDefinition(false, new Color(0.05f, 0.05f, 0.05f), Color.white, new Color(0f, 0.48f, 0.25f)),
                "zh-hans" => new FlagDefinition(false, new Color(0.85f, 0.06f, 0.10f), new Color(1f, 0.84f, 0f)),
                _ => new FlagDefinition(false, new Color(0.12f, 0.35f, 0.75f), Color.white, new Color(0.85f, 0.12f, 0.18f))
            };
        }

        private void SelectLanguage(string languageCode)
        {
            HideLanguageOptions();
            AppServices.SetLanguage(languageCode);

            if (_isLanguagePickerOnly)
            {
                var callback = _onLanguageSelected;
                _onLanguageSelected = null;
                Destroy(gameObject);
                callback?.Invoke();
                return;
            }

            ReturnToMainMenuAfterLanguageChange();
        }

        private void ReturnToMainMenuAfterLanguageChange()
        {
            HideResetConfirmation();
            _onClosed = null;
            _onRestart = null;
            _onMenu = null;
            Destroy(gameObject);
            AppServices.SceneLoader.Load(GameSceneNames.Menu);
        }

        private void HideLanguageOptions()
        {
            if (_languageOptionsRoot == null)
            {
                return;
            }

            Destroy(_languageOptionsRoot);
            _languageOptionsRoot = null;
        }

        private readonly struct FlagDefinition
        {
            public FlagDefinition(bool vertical, params Color[] colors)
            {
                Vertical = vertical;
                Colors = colors;
            }

            public bool Vertical { get; }
            public Color[] Colors { get; }
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

        private void CreateLanguagePickerCloseButton(Transform parent)
        {
            var rect = CreateRect("Close Language Picker", parent, Vector2.zero, Vector2.one,
                new Vector2(0.5f, 0.5f), Vector2.zero, Vector2.zero);
            rect.offsetMin = new Vector2(900f, 600f);
            rect.offsetMax = new Vector2(0f, 0f);
            var hitArea = rect.gameObject.AddComponent<Image>();
            hitArea.sprite = _whiteSprite;
            hitArea.color = new Color(1f, 1f, 1f, 0f);
            hitArea.raycastTarget = true;
            var button = rect.gameObject.AddComponent<Button>();
            button.targetGraphic = hitArea;
            button.transition = Selectable.Transition.None;
            button.onClick.AddListener(HideLanguageOptions);

            var label = CreateText("Close Language Picker Label", rect, Vector2.zero, Vector2.one, new Vector2(0.5f, 0.5f),
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

        private static string T(string key, params LocalizationArgument[] arguments) =>
            AppServices.Localization?.Translate(key, arguments) ?? $"[{key}]";
    }
}
