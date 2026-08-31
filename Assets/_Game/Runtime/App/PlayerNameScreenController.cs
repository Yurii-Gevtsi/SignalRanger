using System;
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
    /// Wide profile-name modal opened from the main menu profile panel.
    /// </summary>
    public sealed class PlayerNameScreenController : MonoBehaviour
    {
        private const string BattleResourceRoot = "UI/Battle/";
        private const string ButtonBlueResource = "UI/Shop/btn_blue";
        private const string ButtonGreenResource = "UI/Shop/btn_green";
        private const string ButtonRedResource = "UI/Shop/btn_red";
        private const string ButtonYellowResource = "UI/Shop/btn_yellow";
        private const int MinimumNameLength = 3;
        private const int MaximumNameLength = 12;

        private static readonly Color OverlayColor = new(0.04f, 0.06f, 0.08f, 0.92f);
        private static readonly Color InputColor = new(0.025f, 0.07f, 0.12f, 0.98f);
        private static readonly Color InputBorderColor = new(0.16f, 0.78f, 1f, 0.95f);
        private static readonly Color ErrorColor = new(1f, 0.30f, 0.34f, 1f);
        private static readonly Color AvailableColor = new(0.26f, 1f, 0.55f, 1f);
        private static readonly HashSet<string> ReservedNames = new(StringComparer.OrdinalIgnoreCase)
        {
            "player",
            "гравець",
            "admin",
            "commander",
            "rocketmaster17"
        };

        private Sprite _whiteSprite;
        private Action _onClosed;
        private GameObject _canvasObject;
        private TMP_InputField _nameInput;
        private TMP_Text _counter;
        private TMP_Text _state;
        private Button _saveButton;
        private string _originalName;

        public static void Open(Action onClosed)
        {
            if (FindAnyObjectByType<PlayerNameScreenController>() != null
                || FindAnyObjectByType<SettingsScreenController>() != null)
            {
                return;
            }

            var root = new GameObject("Player Name Screen");
            root.SetActive(false);
            var controller = root.AddComponent<PlayerNameScreenController>();
            controller._onClosed = onClosed;
            root.SetActive(true);
        }

        private void Awake()
        {
            _whiteSprite = Sprite.Create(Texture2D.whiteTexture, new Rect(0f, 0f, 1f, 1f),
                new Vector2(0.5f, 0.5f), 1f);
            _originalName = AppServices.SaveService?.Current?.profile?.playerName ?? string.Empty;
            Build();
        }

        private void Build()
        {
            EnsureEventSystem();

            _canvasObject = new GameObject("Player Name Canvas");
            _canvasObject.transform.SetParent(transform, false);
            var canvas = _canvasObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 160;

            var scaler = _canvasObject.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920f, 1080f);
            scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            scaler.matchWidthOrHeight = 0.5f;
            _canvasObject.AddComponent<GraphicRaycaster>();

            var overlay = CreatePanel("Overlay", _canvasObject.transform, Vector2.zero, Vector2.one,
                new Vector2(0.5f, 0.5f), Vector2.zero, Vector2.zero, OverlayColor);
            var overlayButton = overlay.gameObject.AddComponent<Button>();
            overlayButton.targetGraphic = overlay;
            overlayButton.transition = Selectable.Transition.None;
            overlayButton.onClick.AddListener(Close);

            var panel = CreatePanel("Change Name Panel", overlay.transform, new Vector2(0.5f, 0.5f),
                new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(1420f, 1140f),
                new Vector2(0f, -10f), Color.white,
                LoadResourceSprite(BattleResourceRoot + "card", new Vector4(250f, 250f, 250f, 250f)));
            panel.type = Image.Type.Sliced;
            panel.preserveAspect = false;
            var panelBlocker = panel.gameObject.AddComponent<Button>();
            panelBlocker.targetGraphic = panel;
            panelBlocker.transition = Selectable.Transition.None;

            CreateText("Title", panel.transform, new Vector2(0.16f, 0.69f), new Vector2(0.84f, 0.81f),
                new Vector2(0.5f, 0.5f), Vector2.zero, Vector2.zero, 52, TextAlignmentOptions.Center,
                T("profile.changeName", string.Empty), GameTextRole.DisplayTitle);
            CreateCloseButton(panel.transform);

            CreateText("New Name Label", panel.transform, new Vector2(0.20f, 0.60f), new Vector2(0.46f, 0.68f),
                new Vector2(0f, 0.5f), Vector2.zero, Vector2.zero, 30, TextAlignmentOptions.MidlineLeft,
                T("profile.newName", string.Empty), GameTextRole.Button);
            _counter = CreateText("Name Counter", panel.transform, new Vector2(0.54f, 0.60f), new Vector2(0.80f, 0.68f),
                new Vector2(1f, 0.5f), Vector2.zero, Vector2.zero, 28, TextAlignmentOptions.MidlineRight,
                string.Empty, GameTextRole.Caption);

            CreateNameInput(panel.transform);
            _state = CreateText("Name State", panel.transform, new Vector2(0.20f, 0.40f), new Vector2(0.80f, 0.49f),
                new Vector2(0.5f, 0.5f), Vector2.zero, Vector2.zero, 26, TextAlignmentOptions.MidlineLeft,
                string.Empty, GameTextRole.Caption);

            CreateText("Name Rules", panel.transform, new Vector2(0.20f, 0.28f), new Vector2(0.80f, 0.38f),
                new Vector2(0.5f, 0.5f), Vector2.zero, Vector2.zero, 23, TextAlignmentOptions.MidlineLeft,
                T("profile.nameHint", string.Empty), GameTextRole.Caption);

            CreateButton("Cancel", panel.transform, new Vector2(0.5f, 0f), new Vector2(0.5f, 0f),
                new Vector2(430f, 118f), new Vector2(-235f, 250f), T("ui.cancel", string.Empty),
                Cancel, ButtonBlueResource);
            _saveButton = CreateButton("Save Name", panel.transform, new Vector2(0.5f, 0f), new Vector2(0.5f, 0f),
                new Vector2(430f, 118f), new Vector2(235f, 250f), T("ui.save", string.Empty),
                SaveName, ButtonGreenResource);

            _nameInput.text = ResolveDisplayName(_originalName);
            _nameInput.Select();
            _nameInput.ActivateInputField();
            RefreshState(_nameInput.text);
        }

        private void CreateNameInput(Transform parent)
        {
            var rect = CreateRect("Name Input", parent, new Vector2(0.20f, 0.5175f), new Vector2(0.80f, 0.5725f),
                new Vector2(0.5f, 0.5f), Vector2.zero, Vector2.zero);
            var background = rect.gameObject.AddComponent<Image>();
            background.sprite = _whiteSprite;
            background.color = InputColor;
            background.raycastTarget = true;
            var outline = rect.gameObject.AddComponent<Outline>();
            outline.effectColor = InputBorderColor;
            outline.effectDistance = new Vector2(3f, 3f);

            var textRect = CreateRect("Name Text", rect, Vector2.zero, Vector2.one, new Vector2(0.5f, 0.5f),
                new Vector2(-54f, -12f), Vector2.zero);
            textRect.offsetMin = new Vector2(28f, 6f);
            textRect.offsetMax = new Vector2(-28f, -6f);
            var text = textRect.gameObject.AddComponent<TextMeshProUGUI>();
            SciFiTextStyle.Apply(text, GameTextRole.Button, 35, TextAlignmentOptions.MidlineLeft);
            text.raycastTarget = false;

            _nameInput = rect.gameObject.AddComponent<TMP_InputField>();
            _nameInput.targetGraphic = background;
            _nameInput.textComponent = text;
            _nameInput.characterLimit = MaximumNameLength;
            _nameInput.lineType = TMP_InputField.LineType.SingleLine;
            _nameInput.onValueChanged.AddListener(RefreshState);
        }

        private void RefreshState(string value)
        {
            var length = value?.Length ?? 0;
            if (length == 0)
            {
                SetState(T("profile.namePrompt", string.Empty), ErrorColor, false);
                return;
            }

            if (!IsValidCharacters(value) || length < MinimumNameLength || length > MaximumNameLength)
            {
                SetState(T("profile.nameInvalid", string.Empty),
                    ErrorColor, false);
                return;
            }

            if (IsReservedName(value) && !string.Equals(value, _originalName, StringComparison.OrdinalIgnoreCase))
            {
                SetState(T("profile.nameTaken", string.Empty), ErrorColor, false);
                return;
            }

            SetState(T("profile.nameAvailable", string.Empty), AvailableColor, true);
        }

        private void SetState(string message, Color color, bool canSave)
        {
            if (_state != null)
            {
                _state.text = message;
                _state.color = color;
            }

            if (_counter != null)
            {
                _counter.text = GameText.NameCharacterCount(_nameInput?.text?.Length ?? 0, MaximumNameLength);
            }

            if (_saveButton != null)
            {
                _saveButton.interactable = canSave;
            }
        }

        private void SaveName()
        {
            var value = _nameInput?.text?.Trim();
            if (!CanSave(value))
            {
                RefreshState(value);
                return;
            }

            var saveData = AppServices.SaveService?.Current;
            if (saveData == null)
            {
                return;
            }

            saveData.profile.playerName = value;
            AppServices.SaveService.Save(saveData);
            Close();
        }

        private bool CanSave(string value) => IsValidCharacters(value)
            && value.Length >= MinimumNameLength
            && value.Length <= MaximumNameLength
            && (!IsReservedName(value) || string.Equals(value, _originalName, StringComparison.OrdinalIgnoreCase));

        private static bool IsValidCharacters(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return false;
            }

            foreach (var character in value)
            {
                if (!char.IsLetterOrDigit(character) && character != '_')
                {
                    return false;
                }
            }

            return true;
        }

        private static bool IsReservedName(string value) => ReservedNames.Contains(value.Trim());

        private void Cancel() => Close();

        private void Close()
        {
            var callback = _onClosed;
            _onClosed = null;
            Destroy(gameObject);
            callback?.Invoke();
        }

        private void CreateCloseButton(Transform parent)
        {
            var rect = CreateRect("Close Name Dialog", parent, new Vector2(1f, 1f), new Vector2(1f, 1f),
                new Vector2(1f, 1f), new Vector2(100f, 100f), new Vector2(-220f, -200f));
            var hitArea = rect.gameObject.AddComponent<Image>();
            hitArea.sprite = _whiteSprite;
            hitArea.color = Color.clear;
            var button = rect.gameObject.AddComponent<Button>();
            button.targetGraphic = hitArea;
            button.transition = Selectable.Transition.None;
            button.onClick.AddListener(Close);
            var label = CreateText("Close Name Dialog Label", rect, Vector2.zero, Vector2.one,
                new Vector2(0.5f, 0.5f), Vector2.zero, Vector2.zero, 72, TextAlignmentOptions.Center,
                "X", GameTextRole.Button);
            label.color = ErrorColor;
            label.fontSize = 64f;
        }

        private Button CreateButton(string name, Component parent, Vector2 anchorMin, Vector2 anchorMax,
            Vector2 size, Vector2 anchoredPosition, string label, Action onClick, string resourcePath)
        {
            var rect = CreateRect(name, parent, anchorMin, anchorMax, new Vector2(0.5f, 0.5f), size, anchoredPosition);
            var image = rect.gameObject.AddComponent<Image>();
            image.sprite = LoadResourceSprite(resourcePath, Vector4.zero);
            image.color = Color.white;
            image.preserveAspect = false;
            var button = rect.gameObject.AddComponent<Button>();
            button.targetGraphic = image;
            button.onClick.AddListener(() => onClick?.Invoke());
            CreateText($"{name} Label", rect, Vector2.zero, Vector2.one, new Vector2(0.5f, 0.5f),
                Vector2.zero, Vector2.zero, 32, TextAlignmentOptions.Center, label, GameTextRole.Button);
            return button;
        }

        private Image CreatePanel(string name, Component parent, Vector2 anchorMin, Vector2 anchorMax,
            Vector2 pivot, Vector2 sizeDelta, Vector2 anchoredPosition, Color color, Sprite sprite = null)
        {
            var rect = CreateRect(name, parent, anchorMin, anchorMax, pivot, sizeDelta, anchoredPosition);
            var image = rect.gameObject.AddComponent<Image>();
            image.sprite = sprite ?? _whiteSprite;
            image.color = color;
            return image;
        }

        private TMP_Text CreateText(string name, Component parent, Vector2 anchorMin, Vector2 anchorMax,
            Vector2 pivot, Vector2 sizeDelta, Vector2 anchoredPosition, int fontSize,
            TextAlignmentOptions alignment, string value, GameTextRole role)
        {
            var rect = CreateRect(name, parent, anchorMin, anchorMax, pivot, sizeDelta, anchoredPosition);
            var text = rect.gameObject.AddComponent<TextMeshProUGUI>();
            SciFiTextStyle.Apply(text, role, fontSize, alignment);
            text.textWrappingMode = TextWrappingModes.Normal;
            text.overflowMode = TextOverflowModes.Ellipsis;
            text.text = value;
            return text;
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

        private static void EnsureEventSystem()
        {
            if (FindAnyObjectByType<EventSystem>() != null)
            {
                return;
            }

            var eventSystem = new GameObject("Name EventSystem");
            eventSystem.AddComponent<EventSystem>();
            eventSystem.AddComponent<InputSystemUIInputModule>();
        }

        private static string ResolveDisplayName(string value) => string.IsNullOrWhiteSpace(value)
            || string.Equals(value, "Гравець", StringComparison.OrdinalIgnoreCase)
            ? string.Empty
            : value;

        private static Sprite LoadResourceSprite(string path, Vector4 border)
        {
            var texture = Resources.Load<Texture2D>(path);
            return texture == null
                ? null
                : Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height),
                    new Vector2(0.5f, 0.5f), 100f, 0, SpriteMeshType.FullRect, border);
        }

        private static string T(string key, string fallback) => AppServices.Localization?.Translate(key) ?? fallback;
    }
}
