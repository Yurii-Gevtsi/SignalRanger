using System;
using Game.Core;
using Game.Core.Localization;
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
        private const float Step = 0.1f;

        private static readonly Color OverlayColor = new(0.04f, 0.06f, 0.08f, 0.92f);
        private static readonly Color PanelColor = new(0.10f, 0.13f, 0.16f, 1f);
        private static readonly Color BarBackColor = new(0f, 0f, 0f, 0.45f);
        private static readonly Color MusicColor = new(0.36f, 0.56f, 0.92f);
        private static readonly Color SfxColor = new(0.30f, 0.72f, 0.48f);
        private static readonly Color StepColor = new(0.20f, 0.26f, 0.32f);
        private static readonly Color SaveColor = new(0.22f, 0.60f, 0.86f);
        private static readonly Color BackColor = new(0.30f, 0.20f, 0.22f);
        private static readonly Color ResetColor = new(0.58f, 0.20f, 0.18f);
        private static readonly Color ConfirmationOverlayColor = new(0.02f, 0.03f, 0.04f, 0.88f);

        private Sprite _whiteSprite;
        private Action _onClosed;
        private GameObject _resetConfirmationRoot;

        private float _music;
        private float _sfx;
        private Image _musicFill;
        private TMP_Text _musicValue;
        private Image _sfxFill;
        private TMP_Text _sfxValue;

        public static void Open(Action onClosed)
        {
            if (FindAnyObjectByType<SettingsScreenController>() != null)
            {
                return;
            }

            var root = new GameObject("Settings Screen");
            root.AddComponent<SettingsScreenController>()._onClosed = onClosed;
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
            scaler.referenceResolution = new Vector2(1080f, 1920f);
            scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            scaler.matchWidthOrHeight = 0.5f;
            canvasObject.AddComponent<GraphicRaycaster>();

            var root = canvasObject.transform;
            CreatePanel("Overlay", root, Vector2.zero, Vector2.one, new Vector2(0.5f, 0.5f), Vector2.zero, Vector2.zero, OverlayColor);

            var panel = CreatePanel("Settings Panel", root, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                new Vector2(0.5f, 0.5f), new Vector2(840f, 900f), Vector2.zero, PanelColor);

            CreateText("Title", panel.transform, new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(0.5f, 1f),
                new Vector2(-80f, 80f), new Vector2(0f, -64f), 44, TextAlignmentOptions.MidlineLeft, GameText.SettingsTitle());

            _musicFill = CreateVolumeRow(panel.transform, -220f, GameText.Music(), MusicColor, out _musicValue,
                () => Adjust(ref _music, -Step), () => Adjust(ref _music, Step));
            _sfxFill = CreateVolumeRow(panel.transform, -400f, GameText.Sounds(), SfxColor, out _sfxValue,
                () => Adjust(ref _sfx, -Step), () => Adjust(ref _sfx, Step));

            CreateText("Language", panel.transform, new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(0.5f, 1f),
                new Vector2(-80f, 48f), new Vector2(0f, -600f), 26, TextAlignmentOptions.MidlineLeft, GameText.LanguageLabel());

            CreateButton("Reset Progression", panel.transform, new Vector2(0.5f, 1f), new Vector2(0.5f, 1f),
                new Vector2(752f, 84f), new Vector2(0f, -700f), ResetColor, GameText.ResetProgressionLabel(), 28,
                () => ShowResetConfirmation(panel.transform));

            CreateButton("Save", panel.transform, new Vector2(0.5f, 0f), new Vector2(0.5f, 0f),
                new Vector2(360f, 104f), new Vector2(-196f, 96f), SaveColor, GameText.Save, 34, Save);
            CreateButton("Back", panel.transform, new Vector2(0.5f, 0f), new Vector2(0.5f, 0f),
                new Vector2(360f, 104f), new Vector2(196f, 96f), BackColor, GameText.Back, 34, Close);

            RefreshValues();
        }

        private Image CreateVolumeRow(Transform parent, float y, string label, Color fillColor, out TMP_Text valueText,
            Action onMinus, Action onPlus)
        {
            CreateText($"{label} Label", parent, new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(0.5f, 1f),
                new Vector2(-80f, 44f), new Vector2(0f, y), 30, TextAlignmentOptions.MidlineLeft, label);

            var back = CreatePanel($"{label} Bar", parent, new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(0.5f, 1f),
                new Vector2(520f, 44f), new Vector2(-40f, y - 64f), BarBackColor);
            var fill = CreateFill($"{label} Fill", back.transform, fillColor);

            valueText = CreateText($"{label} Value", parent, new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(0.5f, 1f),
                new Vector2(120f, 44f), new Vector2(300f, y - 64f), 28, TextAlignmentOptions.MidlineRight, "0%");

            CreateButton($"{label} Minus", parent, new Vector2(0.5f, 1f), new Vector2(0.5f, 1f),
                new Vector2(64f, 64f), new Vector2(-330f, y - 60f), StepColor, "-", 28, () => onMinus());
            CreateButton($"{label} Plus", parent, new Vector2(0.5f, 1f), new Vector2(0.5f, 1f),
                new Vector2(64f, 64f), new Vector2(360f, y - 60f), StepColor, "+", 28, () => onPlus());
            return fill;
        }

        private void Adjust(ref float value, float delta)
        {
            value = Mathf.Clamp01(Mathf.Round((value + delta) * 10f) / 10f);
            RefreshValues();
        }

        private void RefreshValues()
        {
            if (_musicFill != null)
            {
                _musicFill.fillAmount = _music;
                _musicValue.text = $"{Mathf.RoundToInt(_music * 100f)}%";
            }

            if (_sfxFill != null)
            {
                _sfxFill.fillAmount = _sfx;
                _sfxValue.text = $"{Mathf.RoundToInt(_sfx * 100f)}%";
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
                new Vector2(-80f, 64f), new Vector2(0f, -52f), 34, TextAlignmentOptions.Center, GameText.ResetProgressionTitle());
            CreateText("Confirmation Warning", dialog.transform, new Vector2(0f, 0.5f), new Vector2(1f, 0.5f), new Vector2(0.5f, 0.5f),
                new Vector2(-100f, 150f), new Vector2(0f, 16f), 24, TextAlignmentOptions.Center, GameText.ResetProgressionWarning());

            CreateButton("Confirm Reset", dialog.transform, new Vector2(0.5f, 0f), new Vector2(0.5f, 0f),
                new Vector2(280f, 84f), new Vector2(-154f, 62f), ResetColor, GameText.ConfirmResetProgression(), 26,
                ConfirmResetProgression);
            CreateButton("Cancel Reset", dialog.transform, new Vector2(0.5f, 0f), new Vector2(0.5f, 0f),
                new Vector2(280f, 84f), new Vector2(154f, 62f), StepColor, GameText.Cancel(), 26,
                HideResetConfirmation);
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

        private void Save()
        {
            AppServices.Audio.SetVolumes(_music, _sfx);
            AppServices.SaveService.Save(AppServices.SaveService.Current);
            Close();
        }

        private void Close()
        {
            var callback = _onClosed;
            _onClosed = null;
            Destroy(gameObject);
            callback?.Invoke();
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

        private Image CreateFill(string name, Component parent, Color color)
        {
            var rect = CreateRect(name, parent, Vector2.zero, Vector2.one, new Vector2(0.5f, 0.5f), Vector2.zero, Vector2.zero);
            var image = rect.gameObject.AddComponent<Image>();
            image.sprite = _whiteSprite;
            image.color = color;
            image.type = Image.Type.Filled;
            image.fillMethod = Image.FillMethod.Horizontal;
            image.fillOrigin = (int)Image.OriginHorizontal.Left;
            image.fillAmount = 1f;
            image.raycastTarget = false;
            return image;
        }

        private TMP_Text CreateText(string name, Component parent, Vector2 anchorMin, Vector2 anchorMax, Vector2 pivot,
            Vector2 sizeDelta, Vector2 anchoredPosition, int fontSize, TextAlignmentOptions alignment, string value)
        {
            var rect = CreateRect(name, parent, anchorMin, anchorMax, pivot, sizeDelta, anchoredPosition);
            var text = rect.gameObject.AddComponent<TextMeshProUGUI>();
            text.fontSize = fontSize;
            text.color = Color.white;
            text.alignment = alignment;
            text.textWrappingMode = TextWrappingModes.Normal;
            text.overflowMode = TextOverflowModes.Overflow;
            text.text = value;
            text.raycastTarget = false;
            return text;
        }

        private Button CreateButton(string name, Component parent, Vector2 anchorMin, Vector2 anchorMax,
            Vector2 size, Vector2 anchoredPosition, Color color, string label, int fontSize, Action onClick)
        {
            var image = CreatePanel(name, parent, anchorMin, anchorMax, new Vector2(0.5f, 0.5f), size, anchoredPosition, color);
            var button = image.gameObject.AddComponent<Button>();
            button.targetGraphic = image;
            button.onClick.AddListener(() => onClick?.Invoke());
            CreateText($"{name} Label", image.transform, Vector2.zero, Vector2.one, new Vector2(0.5f, 0.5f),
                Vector2.zero, Vector2.zero, fontSize, TextAlignmentOptions.Center, label);
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
