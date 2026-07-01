using System;
using Game.Core;
using Game.Data.Save;
using Game.Progression.Missions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Game.App
{
    public sealed class MenuSceneController : MonoBehaviour
    {
        private static readonly Color BackgroundColor = new(0.12f, 0.17f, 0.20f);
        private static readonly Color PanelColor = new(0.08f, 0.11f, 0.13f, 0.86f);
        private static readonly Color PrimaryColor = new(0.22f, 0.66f, 0.38f);
        private static readonly Color SecondaryColor = new(0.18f, 0.25f, 0.31f);
        private static readonly Color DisabledColor = new(0.16f, 0.18f, 0.20f);

        private Sprite _whiteSprite;
        private Font _font;
        private Text _statusText;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void CreateForMenuScene()
        {
            if (SceneManager.GetActiveScene().name != GameSceneNames.Menu)
            {
                return;
            }

            EnsureCreated();
        }

        public static void EnsureCreated()
        {
            if (FindAnyObjectByType<MenuSceneController>() != null)
            {
                return;
            }

            var root = new GameObject("Menu Scene Controller");
            root.AddComponent<MenuSceneController>();
        }

        private void Awake()
        {
            AppServices.Initialize();
            _whiteSprite = Sprite.Create(Texture2D.whiteTexture, new Rect(0f, 0f, 1f, 1f), new Vector2(0.5f, 0.5f), 1f);
            _font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            if (_font == null)
            {
                _font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            }

            BuildCamera();
            EnsureEventSystem();
            BuildMenu();
        }

        private static void EnsureEventSystem()
        {
            if (FindAnyObjectByType<EventSystem>() != null)
            {
                return;
            }

            var eventSystem = new GameObject("Menu EventSystem");
            eventSystem.AddComponent<EventSystem>();
            eventSystem.AddComponent<InputSystemUIInputModule>();
        }

        private void BuildCamera()
        {
            var cameraObject = new GameObject("Menu Camera");
            var camera = cameraObject.AddComponent<Camera>();
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = BackgroundColor;
            camera.orthographic = true;
            camera.orthographicSize = 5f;
            cameraObject.tag = "MainCamera";
        }

        private void BuildMenu()
        {
            var saveData = AppServices.SaveService.Current;
            var nextMission = AppServices.Campaign.GetNextMission();
            var canvasObject = new GameObject("Menu Canvas");
            var canvas = canvasObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 50;

            var scaler = canvasObject.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1080f, 1920f);
            scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            scaler.matchWidthOrHeight = 0.5f;
            canvasObject.AddComponent<GraphicRaycaster>();

            var root = canvasObject.transform;
            CreatePanel("Top Band", root, new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(0.5f, 1f),
                new Vector2(0f, 260f), Vector2.zero, new Color(0.05f, 0.08f, 0.10f, 0.92f));

            CreateText("Title", root, new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(0.5f, 1f),
                new Vector2(-96f, 88f), new Vector2(0f, -68f), 52, TextAnchor.MiddleLeft, "Тактичний бій");
            CreateText("Subtitle", root, new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(0.5f, 1f),
                new Vector2(-96f, 72f), new Vector2(0f, -148f), 28, TextAnchor.MiddleLeft, "Короткі артилерійські дуелі. MVP build.");

            var panel = CreatePanel("Mission Panel", root, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                new Vector2(0.5f, 0.5f), new Vector2(760f, 760f), new Vector2(0f, -120f), PanelColor);

            CreateText("Mission Label", panel.transform, new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(0.5f, 1f),
                new Vector2(-88f, 68f), new Vector2(0f, -54f), 34, TextAnchor.MiddleLeft, "Наступна місія");
            CreateText("Mission Name", panel.transform, new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(0.5f, 1f),
                new Vector2(-88f, 84f), new Vector2(0f, -132f), 46, TextAnchor.MiddleLeft, DescribeMissionTitle(nextMission));
            CreateText("Mission Details", panel.transform, new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(0.5f, 1f),
                new Vector2(-88f, 112f), new Vector2(0f, -232f), 26, TextAnchor.UpperLeft,
                DescribeMissionDetails(nextMission));
            CreateText("Profile Summary", panel.transform, new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(0.5f, 1f),
                new Vector2(-88f, 82f), new Vector2(0f, -352f), 24, TextAnchor.UpperLeft, DescribeProfile(saveData, nextMission));

            CreateButton("Play Button", panel.transform, new Vector2(0.5f, 0f), new Vector2(0.5f, 0f),
                new Vector2(620f, 116f), new Vector2(0f, 236f), PrimaryColor, "Грати", 44, StartBattle);
            CreateButton("Loadout Button", panel.transform, new Vector2(0.5f, 0f), new Vector2(0.5f, 0f),
                new Vector2(620f, 86f), new Vector2(0f, 132f), SecondaryColor, "Спорядження", 30, OpenLoadout);
            CreateButton("Settings Button", panel.transform, new Vector2(0.5f, 0f), new Vector2(0.5f, 0f),
                new Vector2(620f, 86f), new Vector2(0f, 34f), SecondaryColor, "Налаштування", 30, OpenSettings);
            CreateButton("Debug Battle Button", panel.transform, new Vector2(0.5f, 0f), new Vector2(0.5f, 0f),
                new Vector2(620f, 74f), new Vector2(0f, -60f), DisabledColor, "Debug: відкрити бій", 26, StartBattle);

            _statusText = CreateText("Status", root, new Vector2(0f, 0f), new Vector2(1f, 0f), new Vector2(0.5f, 0f),
                new Vector2(-96f, 74f), new Vector2(0f, 48f), 26, TextAnchor.MiddleLeft,
                DescribeMenuStatus(AppServices.LastSaveLoadResult, AppServices.LastMissionCompletionResult));
        }

        private void StartBattle()
        {
            AppServices.Session.SelectNextMission();
            AppServices.SceneLoader.Load(GameSceneNames.Battle);
        }

        private void OpenLoadout()
        {
            LoadoutScreenController.Open(() => SetStatus("Спорядження оновлено."));
        }

        private void OpenSettings()
        {
            SettingsScreenController.Open(() => SetStatus("Налаштування збережено."));
        }

        private void SetStatus(string message)
        {
            if (_statusText != null)
            {
                _statusText.text = message;
            }
        }

        private static string DescribeProfile(SaveData saveData, MissionDefinition mission)
        {
            if (saveData == null)
            {
                return "Профіль: локальні дані ще не готові.";
            }

            var reward = mission != null ? mission.Reward.softCurrency : 0;
            return $"Профіль: {saveData.profile.playerName}\nМісія: {saveData.campaign.highestUnlockedMissionIndex + 1}\nМонети: {saveData.economy.softCurrency}\nНагорода: {reward} монет";
        }

        private static string DescribeMissionTitle(MissionDefinition mission)
        {
            return mission != null ? mission.Title : "Тренувальна місія";
        }

        private static string DescribeMissionDetails(MissionDefinition mission)
        {
            return mission != null
                ? mission.Description
                : "Наведи постріл перетягуванням, зафіксуй приціл і підтвердь постріл кнопкою.";
        }

        private static string DescribeLoadStatus(SaveLoadResult result)
        {
            return result.Status switch
            {
                SaveLoadStatus.CreatedNew => "Створено локальний профіль. Натисни «Грати», щоб перейти до першого бою.",
                SaveLoadStatus.RecoveredFromCorrupt => "Save було відновлено з резервною копією. Можна грати далі.",
                _ => "Локальний профіль завантажено. Натисни «Грати», щоб перейти до бою."
            };
        }

        private static string DescribeMenuStatus(SaveLoadResult loadResult, MissionCompletionResult completionResult)
        {
            if (completionResult.Applied)
            {
                var unlock = completionResult.HasUnlock ? " Нове спорядження відкрито." : string.Empty;
                return $"Місію завершено. +{completionResult.GrantedSoftCurrency} монет.{unlock}";
            }

            return DescribeLoadStatus(loadResult);
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
            var image = CreatePanel(name, parent, anchorMin, anchorMax, new Vector2(anchorMin.x, anchorMin.y), size, anchoredPosition, color);
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
