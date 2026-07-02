using System;
using Game.Core;
using Game.Core.Localization;
using Game.Data.Save;
using Game.Progression.Missions;
using TMPro;
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
        private static readonly Color PrimaryColor = new(0.22f, 0.66f, 0.38f);
        private static readonly Color SecondaryColor = new(0.18f, 0.25f, 0.31f);

        private Sprite _whiteSprite;
        private TMP_Text _statusText;

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

            // Top band with title/subtitle (anchored to the top edge).
            CreatePanel("Top Band", root, new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(0.5f, 1f),
                new Vector2(0f, 220f), Vector2.zero, new Color(0.05f, 0.08f, 0.10f, 0.92f));
            CreateText("Title", root, new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(0.5f, 1f),
                new Vector2(-96f, 78f), new Vector2(0f, -58f), 48, TextAlignmentOptions.MidlineLeft, GameText.MenuTitle());
            CreateText("Subtitle", root, new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(0.5f, 1f),
                new Vector2(-96f, 64f), new Vector2(0f, -132f), 26, TextAlignmentOptions.MidlineLeft, GameText.MenuSubtitle());

            // Mission info block, anchored under the top band.
            CreateText("Mission Label", root, new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(0.5f, 1f),
                new Vector2(-120f, 42f), new Vector2(0f, -280f), 30, TextAlignmentOptions.MidlineLeft, GameText.NextMissionLabel());
            CreateText("Mission Name", root, new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(0.5f, 1f),
                new Vector2(-120f, 60f), new Vector2(0f, -336f), 42, TextAlignmentOptions.MidlineLeft, DescribeMissionTitle(nextMission));
            CreateText("Mission Details", root, new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(0.5f, 1f),
                new Vector2(-120f, 120f), new Vector2(0f, -430f), 25, TextAlignmentOptions.TopLeft, DescribeMissionDetails(nextMission));
            CreateText("Profile Summary", root, new Vector2(0f, 1f), new Vector2(1f, 1f), new Vector2(0.5f, 1f),
                new Vector2(-120f, 120f), new Vector2(0f, -560f), 23, TextAlignmentOptions.TopLeft, DescribeProfile(saveData, nextMission));

            // Actions, anchored to the bottom edge so they never overlap the info above.
            CreateButton("Play Button", root, new Vector2(0.5f, 0f), new Vector2(0.5f, 0f),
                new Vector2(680f, 124f), new Vector2(0f, 250f), PrimaryColor, GameText.Play, 44, StartBattle);
            CreateButton("Loadout Button", root, new Vector2(0.5f, 0f), new Vector2(0.5f, 0f),
                new Vector2(332f, 100f), new Vector2(-176f, 132f), SecondaryColor, GameText.Loadout, 28, OpenLoadout);
            CreateButton("Settings Button", root, new Vector2(0.5f, 0f), new Vector2(0.5f, 0f),
                new Vector2(332f, 100f), new Vector2(176f, 132f), SecondaryColor, GameText.Settings, 28, OpenSettings);

            _statusText = CreateText("Status", root, new Vector2(0f, 0f), new Vector2(1f, 0f), new Vector2(0.5f, 0f),
                new Vector2(-96f, 44f), new Vector2(0f, 48f), 24, TextAlignmentOptions.Center,
                DescribeMenuStatus(AppServices.LastSaveLoadResult, AppServices.LastMissionCompletionResult));
        }

        private void StartBattle()
        {
            AppServices.Session.SelectNextMission();
            AppServices.SceneLoader.Load(GameSceneNames.Battle);
        }

        private void OpenLoadout()
        {
            LoadoutScreenController.Open(() => SetStatus(GameText.LoadoutUpdated()));
        }

        private void OpenSettings()
        {
            SettingsScreenController.Open(() => SetStatus(GameText.SettingsSaved()));
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
                return GameText.ProfileUnavailable();
            }

            var reward = mission != null ? mission.Reward.softCurrency : 0;
            return GameText.ProfileSummary(
                saveData.profile.playerName,
                saveData.campaign.highestUnlockedMissionIndex + 1,
                saveData.economy.softCurrency,
                reward);
        }

        private static string DescribeMissionTitle(MissionDefinition mission)
        {
            return mission != null ? mission.Title : GameText.DefaultMissionTitle();
        }

        private static string DescribeMissionDetails(MissionDefinition mission)
        {
            return mission != null
                ? mission.Description
                : GameText.DefaultMissionDetails();
        }

        private static string DescribeMenuStatus(SaveLoadResult loadResult, MissionCompletionResult completionResult)
        {
            if (completionResult.Applied)
            {
                return GameText.MissionCompleted(completionResult.GrantedSoftCurrency, completionResult.HasUnlock);
            }

            return loadResult.Status switch
            {
                SaveLoadStatus.CreatedNew => GameText.LoadStatusCreatedNew(),
                SaveLoadStatus.RecoveredFromCorrupt => GameText.LoadStatusRecovered(),
                _ => GameText.LoadStatusReady()
            };
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
            var image = CreatePanel(name, parent, anchorMin, anchorMax, new Vector2(anchorMin.x, anchorMin.y), size, anchoredPosition, color);
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
