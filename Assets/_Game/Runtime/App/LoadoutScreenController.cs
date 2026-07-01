using System;
using System.Collections.Generic;
using System.Linq;
using Game.Gameplay.Battle.Weapons;
using UnityEngine;
using UnityEngine.UI;

namespace Game.App
{
    /// <summary>
    /// Modal loadout screen built on demand from the menu. Lists every weapon,
    /// lets the player toggle unlocked ones into their loadout, and persists the
    /// choice through <see cref="Game.Inventory.InventoryService"/> + save.
    /// </summary>
    public sealed class LoadoutScreenController : MonoBehaviour
    {
        private static readonly Color OverlayColor = new(0.04f, 0.06f, 0.08f, 0.92f);
        private static readonly Color PanelColor = new(0.10f, 0.13f, 0.16f, 1f);
        private static readonly Color SelectedColor = new(0.22f, 0.62f, 0.40f);
        private static readonly Color UnselectedColor = new(0.20f, 0.26f, 0.32f);
        private static readonly Color LockedColor = new(0.14f, 0.15f, 0.17f);
        private static readonly Color SaveColor = new(0.22f, 0.60f, 0.86f);
        private static readonly Color BackColor = new(0.30f, 0.20f, 0.22f);

        private readonly HashSet<string> _working = new();
        private readonly List<Row> _rows = new();

        private struct Row
        {
            public string WeaponId;
            public bool Unlocked;
            public string BaseLabel;
            public Image Image;
            public Text Label;
        }

        private Sprite _whiteSprite;
        private Font _font;
        private Text _statusText;
        private Action _onClosed;

        public static void Open(Action onClosed)
        {
            var existing = FindAnyObjectByType<LoadoutScreenController>();
            if (existing != null)
            {
                return;
            }

            var root = new GameObject("Loadout Screen");
            var controller = root.AddComponent<LoadoutScreenController>();
            controller._onClosed = onClosed;
        }

        private void Awake()
        {
            _whiteSprite = Sprite.Create(Texture2D.whiteTexture, new Rect(0f, 0f, 1f, 1f), new Vector2(0.5f, 0.5f), 1f);
            _font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            if (_font == null)
            {
                _font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            }

            foreach (var id in AppServices.Inventory.SelectedLoadoutWeaponIds)
            {
                _working.Add(id);
            }

            Build();
        }

        private void Build()
        {
            var canvasObject = new GameObject("Loadout Canvas");
            canvasObject.transform.SetParent(transform, false);
            var canvas = canvasObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 120;

            var scaler = canvasObject.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1080f, 1920f);
            scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            scaler.matchWidthOrHeight = 0.5f;
            canvasObject.AddComponent<GraphicRaycaster>();

            var root = canvasObject.transform;
            CreatePanel("Overlay", root, Vector2.zero, Vector2.one, new Vector2(0.5f, 0.5f), Vector2.zero, Vector2.zero, OverlayColor);

            var panel = CreatePanel("Loadout Panel", root, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                new Vector2(0.5f, 0.5f), new Vector2(840f, 1180f), Vector2.zero, PanelColor);

            CreateText("Title", panel.transform, new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(0.5f, 1f),
                new Vector2(-80f, 80f), new Vector2(0f, -64f), 44, TextAnchor.MiddleLeft, "Спорядження");
            CreateText("Hint", panel.transform, new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(0.5f, 1f),
                new Vector2(-80f, 60f), new Vector2(0f, -132f), 24, TextAnchor.MiddleLeft,
                "Обери зброю, яку береш у бій. Заблоковану треба спершу відкрити.");

            var weapons = PrototypeWeaponLibrary.CreateAll();
            var rowHeight = 118f;
            var startY = -240f;
            for (var i = 0; i < weapons.Length; i++)
            {
                CreateWeaponRow(panel.transform, weapons[i], startY - i * (rowHeight + 16f), rowHeight);
            }

            _statusText = CreateText("Status", panel.transform, new Vector2(0f, 0f), new Vector2(1f, 0f), new Vector2(0.5f, 0f),
                new Vector2(-80f, 60f), new Vector2(0f, 210f), 24, TextAnchor.MiddleCenter, DescribeCount());

            CreateButton("Save", panel.transform, new Vector2(0.5f, 0f), new Vector2(0.5f, 0f),
                new Vector2(360f, 104f), new Vector2(-196f, 96f), SaveColor, "Зберегти", 34, Save);
            CreateButton("Back", panel.transform, new Vector2(0.5f, 0f), new Vector2(0.5f, 0f),
                new Vector2(360f, 104f), new Vector2(196f, 96f), BackColor, "Назад", 34, Close);
        }

        private void CreateWeaponRow(Transform parent, WeaponDefinition weapon, float y, float height)
        {
            var weaponId = weapon.WeaponId;
            var unlocked = AppServices.Inventory.IsWeaponUnlocked(weaponId);

            var image = CreatePanel($"Row {weaponId}", parent, new Vector2(0.5f, 1f), new Vector2(0.5f, 1f),
                new Vector2(0.5f, 1f), new Vector2(760f, height), new Vector2(0f, y), UnselectedColor);

            var label = CreateText($"Row Label {weaponId}", image.transform, Vector2.zero, Vector2.one, new Vector2(0.5f, 0.5f),
                new Vector2(-48f, 0f), Vector2.zero, 30, TextAnchor.MiddleLeft, string.Empty);

            _rows.Add(new Row
            {
                WeaponId = weaponId,
                Unlocked = unlocked,
                BaseLabel = $"{DisplayName(weaponId)}  ·  {weapon.Role}",
                Image = image,
                Label = label
            });

            if (unlocked)
            {
                var button = image.gameObject.AddComponent<Button>();
                button.targetGraphic = image;
                button.onClick.AddListener(() => Toggle(weaponId));
            }

            RefreshRow(_rows.Count - 1);
        }

        private void Toggle(string weaponId)
        {
            if (!_working.Remove(weaponId))
            {
                _working.Add(weaponId);
            }

            for (var i = 0; i < _rows.Count; i++)
            {
                if (_rows[i].WeaponId == weaponId)
                {
                    RefreshRow(i);
                    break;
                }
            }

            SetStatus(DescribeCount());
        }

        private void RefreshRow(int index)
        {
            var row = _rows[index];
            var selected = _working.Contains(row.WeaponId);
            row.Image.color = !row.Unlocked ? LockedColor : selected ? SelectedColor : UnselectedColor;

            var suffix = !row.Unlocked ? "  [заблоковано]" : selected ? "  ✓" : string.Empty;
            row.Label.text = $"{row.BaseLabel}{suffix}";
        }

        private void Save()
        {
            if (_working.Count == 0)
            {
                SetStatus("Обери хоча б одну зброю.");
                return;
            }

            if (!AppServices.Inventory.TrySetLoadout(_working))
            {
                SetStatus("Не вдалося зберегти: перевір, що вся зброя відкрита.");
                return;
            }

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

        private string DescribeCount()
        {
            return $"У спорядженні: {_working.Count} зброї.";
        }

        private void SetStatus(string message)
        {
            if (_statusText != null)
            {
                _statusText.text = message;
            }
        }

        private static string DisplayName(string weaponId)
        {
            var name = weaponId.StartsWith("weapon.", StringComparison.Ordinal) ? weaponId.Substring(7) : weaponId;
            name = name.Replace('-', ' ').Replace('.', ' ');
            return string.Join(" ", name.Split(' ')
                .Where(part => part.Length > 0)
                .Select(part => char.ToUpperInvariant(part[0]) + part.Substring(1)));
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

        private Text CreateText(string name, Component parent, Vector2 anchorMin, Vector2 anchorMax, Vector2 pivot,
            Vector2 sizeDelta, Vector2 anchoredPosition, int fontSize, TextAnchor alignment, string value)
        {
            var rect = CreateRect(name, parent, anchorMin, anchorMax, pivot, sizeDelta, anchoredPosition);
            var text = rect.gameObject.AddComponent<Text>();
            text.font = _font;
            text.fontSize = fontSize;
            text.color = Color.white;
            text.alignment = alignment;
            text.horizontalOverflow = HorizontalWrapMode.Wrap;
            text.verticalOverflow = VerticalWrapMode.Overflow;
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
                Vector2.zero, Vector2.zero, fontSize, TextAnchor.MiddleCenter, label);
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
