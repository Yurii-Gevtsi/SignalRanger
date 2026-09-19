using Game.Core.Localization;
using Game.Data.Progression;
using Game.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.App
{
    /// <summary>
    /// Shared top-bar level panel used by the menu screens that also show balances.
    /// </summary>
    public sealed class MenuPlayerLevelPanel : MonoBehaviour
    {
        private TMP_Text _label;
        private TMP_Text _value;
        private bool _initialized;
        private bool _subscribed;

        public static RectTransform Create(Transform parent, string name, Vector2 position, Vector2 size)
        {
            var panelObject = new GameObject(name, typeof(RectTransform));
            var panel = (RectTransform)panelObject.transform;
            panel.SetParent(parent, false);
            panel.anchorMin = new Vector2(0.5f, 1f);
            panel.anchorMax = new Vector2(0.5f, 1f);
            panel.pivot = new Vector2(0.5f, 1f);
            panel.sizeDelta = size;
            panel.anchoredPosition = position;

            var image = panelObject.AddComponent<Image>();
            image.sprite = CreatePanelSprite();
            image.preserveAspect = true;
            image.raycastTarget = false;

            var label = CreateText(
                "Label",
                panel,
                new Vector2(0.08f, 0.12f),
                new Vector2(0.67f, 0.88f),
                TextAlignmentOptions.Right,
                GameTextRole.Caption);
            var value = CreateText(
                "Value",
                panel,
                new Vector2(0.69f, 0.12f),
                new Vector2(0.92f, 0.88f),
                TextAlignmentOptions.Left,
                GameTextRole.Numeric);
            label.rectTransform.anchoredPosition = new Vector2(-70f, 0f);
            value.rectTransform.anchoredPosition = new Vector2(-70f, 0f);

            var binding = panelObject.AddComponent<MenuPlayerLevelPanel>();
            binding.Initialize(label, value);
            return panel;
        }

        private void Initialize(TMP_Text label, TMP_Text value)
        {
            _label = label;
            _value = value;
            _initialized = true;
            Subscribe();
            Refresh();
        }

        private void OnEnable()
        {
            if (_initialized)
            {
                Subscribe();
                Refresh();
            }
        }

        private void OnDisable()
        {
            Unsubscribe();
        }

        private void OnDestroy()
        {
            Unsubscribe();
        }

        private void Subscribe()
        {
            if (_subscribed)
            {
                return;
            }

            if (AppServices.Economy != null)
            {
                AppServices.Economy.ProgressionChanged += Refresh;
            }

            if (AppServices.Localization != null)
            {
                AppServices.Localization.LanguageChanged += Refresh;
            }

            _subscribed = true;
        }

        private void Unsubscribe()
        {
            if (!_subscribed)
            {
                return;
            }

            if (AppServices.Economy != null)
            {
                AppServices.Economy.ProgressionChanged -= Refresh;
            }

            if (AppServices.Localization != null)
            {
                AppServices.Localization.LanguageChanged -= Refresh;
            }

            _subscribed = false;
        }

        private void Refresh()
        {
            if (_label == null || _value == null)
            {
                return;
            }

            var experience = AppServices.Economy?.CommanderExperience
                ?? AppServices.SaveService?.Current?.profile?.commanderExperience
                ?? 0;
            _label.text = GameText.LevelLabel();
            _value.text = CommanderProgressionTable.GetLevel(experience).ToString();
        }

        private static TMP_Text CreateText(
            string name,
            RectTransform parent,
            Vector2 anchorMin,
            Vector2 anchorMax,
            TextAlignmentOptions alignment,
            GameTextRole role)
        {
            var textObject = new GameObject(name, typeof(RectTransform));
            var rect = (RectTransform)textObject.transform;
            rect.SetParent(parent, false);
            rect.anchorMin = anchorMin;
            rect.anchorMax = anchorMax;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;

            var text = textObject.AddComponent<TextMeshProUGUI>();
            SciFiTextStyle.Apply(text, role, role == GameTextRole.Numeric ? 32 : 24, alignment);
            text.enableAutoSizing = true;
            text.fontSizeMin = 14f;
            text.fontSizeMax = role == GameTextRole.Numeric ? 32f : 24f;
            text.textWrappingMode = TextWrappingModes.NoWrap;
            text.overflowMode = TextOverflowModes.Truncate;
            text.raycastTarget = false;
            return text;
        }

        private static Sprite CreatePanelSprite()
        {
            var texture = Resources.Load<Texture2D>("UI/Battle/smal_panel");
            if (texture == null)
            {
                Debug.LogWarning("[MenuPlayerLevelPanel] Missing UI/Battle/smal_panel resource.");
                return null;
            }

            return Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height),
                new Vector2(0.5f, 0.5f), 100f);
        }
    }
}
