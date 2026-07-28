using System.Collections;
using System.Collections.Generic;
using Game.Core;
using Game.Core.Localization;
using Game.UI;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Game.App
{
    public sealed class LoadingScreenController : MonoBehaviour
    {
        private const string LoaderResourceRoot = "UI/Loaders/";
        private const int BackgroundCount = 5;
        private const float LoaderRotationDegreesPerSecond = -220f;
        private const float ProgressFillSpeed = 1.35f;
        private const float MinimumDisplaySeconds = 0.85f;

        private static readonly Color TextShadowColor = new(0.02f, 0.03f, 0.05f, 0.92f);
        private static readonly Vector2 MissionTitlePosition = new(-270f, 270f);
        private static readonly Vector2 MissionTitleSize = new(520f, 76f);
        private static readonly Vector2 MissionDescriptionPosition = new(-260f, 176f);
        private static readonly Vector2 MissionDescriptionSize = new(520f, 150f);
        private static readonly Vector2 TipPosition = new(230f, -438f);
        private static readonly Vector2 TipSize = new(1040f, 82f);
        private static readonly Vector2 LoaderPosition = new(0f, -230f);
        private static readonly Vector2 LoaderSize = new(224f, 224f);
        private static readonly Vector2 PercentSize = new(210f, 100f);

        private static readonly string[] Tips =
        {
            "tip.arc",
            "tip.retreat",
            "tip.ground",
            "tip.straight",
            "tip.repair"
        };

        private readonly Dictionary<string, Sprite> _sprites = new();

        private static LoadingScreenController _active;

        private Sprite _whiteSprite;
        private RectTransform _loaderRing;
        private TMP_Text _percentText;
        private float _targetProgress;
        private float _displayProgress;

        public static void LoadScene(string sceneName)
        {
            var controller = Create(sceneName);
            controller.StartCoroutine(controller.LoadRoutine(sceneName));
        }

        public static IEnumerator LoadSceneAsync(string sceneName)
        {
            var controller = Create(sceneName);
            yield return controller.StartCoroutine(controller.LoadRoutine(sceneName));
        }

        public void InitializeForPreview(string sceneName)
        {
            Build(sceneName);
            SetProgress(0f);
        }

        private static LoadingScreenController Create(string sceneName)
        {
            if (_active != null)
            {
                Destroy(_active.gameObject);
                _active = null;
            }

            var root = new GameObject("Loading Screen");
            DontDestroyOnLoad(root);
            _active = root.AddComponent<LoadingScreenController>();
            _active.Build(sceneName);
            return _active;
        }

        private void Awake()
        {
            _whiteSprite = Sprite.Create(Texture2D.whiteTexture, new Rect(0f, 0f, 1f, 1f), new Vector2(0.5f, 0.5f), 1f);
        }

        private void Update()
        {
            if (_loaderRing != null)
            {
                _loaderRing.Rotate(0f, 0f, LoaderRotationDegreesPerSecond * Time.unscaledDeltaTime);
            }

            _displayProgress = Mathf.MoveTowards(_displayProgress, _targetProgress, ProgressFillSpeed * Time.unscaledDeltaTime);
            UpdatePercentText();
        }

        private void Build(string sceneName)
        {
            if (_whiteSprite == null)
            {
                _whiteSprite = Sprite.Create(Texture2D.whiteTexture, new Rect(0f, 0f, 1f, 1f), new Vector2(0.5f, 0.5f), 1f);
            }

            var canvasObject = new GameObject("Loading Canvas");
            canvasObject.transform.SetParent(transform, false);
            var canvas = canvasObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 5000;

            var scaler = canvasObject.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920f, 1080f);
            scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            scaler.matchWidthOrHeight = 1f;

            var root = canvasObject.transform;
            var backgroundIndex = UnityEngine.Random.Range(1, BackgroundCount + 1);
            CreateImage("Loading Background", root, Vector2.zero, Vector2.one, new Vector2(0.5f, 0.5f),
                Vector2.zero, Vector2.zero, backgroundIndex.ToString(), false);

            var mission = ResolveMission(sceneName);
            CreateText("Mission Title", root, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                new Vector2(0.5f, 0.5f), MissionTitleSize, MissionTitlePosition, 38,
                TextAlignmentOptions.TopLeft, mission.title, GameTextRole.DisplayTitle);
            var description = CreateText("Mission Description", root, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                new Vector2(0.5f, 0.5f), MissionDescriptionSize, MissionDescriptionPosition, 27,
                TextAlignmentOptions.TopLeft, mission.description, GameTextRole.Body);
            description.enableAutoSizing = true;
            description.fontSizeMin = 20f;
            description.fontSizeMax = 27f;
            description.textWrappingMode = TextWrappingModes.Normal;
            description.overflowMode = TextOverflowModes.Truncate;

            var tip = CreateText("Loading Tip", root, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                new Vector2(0.5f, 0.5f), TipSize, TipPosition, 28,
                TextAlignmentOptions.MidlineLeft, ResolveTip(Tips[UnityEngine.Random.Range(0, Tips.Length)]), GameTextRole.Body);
            tip.enableAutoSizing = true;
            tip.fontSizeMin = 20f;
            tip.fontSizeMax = 28f;

            _loaderRing = CreateImage("Loading Ring", root, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                new Vector2(0.5f, 0.5f), LoaderSize, LoaderPosition, "loader", true);
            _percentText = CreateText("Loading Percent", root, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f),
                new Vector2(0.5f, 0.5f), PercentSize, LoaderPosition, 40,
                TextAlignmentOptions.Center, "0%", GameTextRole.DisplayTitle);
        }

        private IEnumerator LoadRoutine(string sceneName)
        {
            SetProgress(0f);
            var startTime = Time.unscaledTime;
            var operation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
            if (operation == null)
            {
                SceneManager.LoadScene(sceneName);
                DestroySelf();
                yield break;
            }

            operation.allowSceneActivation = false;
            while (operation.progress < 0.9f)
            {
                SetProgress(Mathf.Clamp01(operation.progress / 0.9f));
                yield return null;
            }

            SetProgress(1f);
            while (_displayProgress < 0.995f || Time.unscaledTime - startTime < MinimumDisplaySeconds)
            {
                yield return null;
            }

            UpdatePercentText(1f);
            operation.allowSceneActivation = true;
            while (!operation.isDone)
            {
                yield return null;
            }

            yield return null;
            DestroySelf();
        }

        private void SetProgress(float progress)
        {
            _targetProgress = Mathf.Clamp01(progress);
        }

        private void UpdatePercentText(float? forcedProgress = null)
        {
            if (_percentText == null)
            {
                return;
            }

            var progress = forcedProgress ?? _displayProgress;
            _percentText.text = $"{Mathf.RoundToInt(Mathf.Clamp01(progress) * 100f)}%";
        }

        private static (string title, string description) ResolveMission(string sceneName)
        {
            if (sceneName == GameSceneNames.Battle && AppServices.IsInitialized)
            {
                var mission = AppServices.Session?.SelectedMission;
                if (mission != null)
                {
                    return (
                        string.IsNullOrWhiteSpace(mission.Title) ? GameText.Tip("battleTitle", "Бойова місія") : mission.Title,
                        GameText.Tip("trainingMission", "Навчальна місія"));
                }
            }

            if (sceneName == GameSceneNames.Battle)
            {
                return (GameText.Tip("battleTitle", "Бойова місія"), GameText.Tip("trainingMission", "Навчальна місія"));
            }

            if (sceneName == GameSceneNames.Menu)
            {
                return (GameText.Tip("returnBase", "Повернення на базу"), GameText.Tip("returnBaseDescription", "Оновлюємо прогрес, нагороди та доступний арсенал."));
            }

            return (GameText.Tip("loading", "Завантаження"), GameText.Tip("loadingDescription", "Готуємо сцену, ресурси та бойові системи."));
        }

        private static string ResolveTip(string key)
        {
            return key switch
            {
                "tip.arc" => GameText.Tip(key, "Порада: висока дуга допомагає перекидати укриття."),
                "tip.retreat" => GameText.Tip(key, "Порада: перед пострілом перевір, чи вистачить часу на відступ."),
                "tip.ground" => GameText.Tip(key, "Порада: наземна зброя сильна біля проходів і країв ям."),
                "tip.straight" => GameText.Tip(key, "Порада: пряма зброя найкраща, коли між дроїдами чистий простір."),
                _ => GameText.Tip(key, "Порада: ремонт краще зберігати до моменту, коли хід ще можна використати.")
            };
        }

        private RectTransform CreateImage(string name, Component parent, Vector2 anchorMin, Vector2 anchorMax,
            Vector2 pivot, Vector2 sizeDelta, Vector2 anchoredPosition, string resourceName, bool preserveAspect)
        {
            var rect = CreateRect(name, parent, anchorMin, anchorMax, pivot, sizeDelta, anchoredPosition);
            var image = rect.gameObject.AddComponent<Image>();
            image.sprite = LoadSprite(resourceName);
            image.color = Color.white;
            image.preserveAspect = preserveAspect;
            image.raycastTarget = false;
            return rect;
        }

        private TMP_Text CreateText(string name, Component parent, Vector2 anchorMin, Vector2 anchorMax, Vector2 pivot,
            Vector2 sizeDelta, Vector2 anchoredPosition, int fontSize, TextAlignmentOptions alignment, string value,
            GameTextRole role)
        {
            var rect = CreateRect(name, parent, anchorMin, anchorMax, pivot, sizeDelta, anchoredPosition);
            var text = rect.gameObject.AddComponent<TextMeshProUGUI>();
            SciFiTextStyle.Apply(text, role, fontSize, alignment);
            text.fontSizeMin = Mathf.Max(14, fontSize - 10);
            text.textWrappingMode = TextWrappingModes.Normal;
            text.overflowMode = TextOverflowModes.Truncate;
            text.text = value;
            text.raycastTarget = false;

            var shadow = rect.gameObject.AddComponent<Shadow>();
            shadow.effectColor = TextShadowColor;
            shadow.effectDistance = new Vector2(0f, -2f);
            return text;
        }

        private Sprite LoadSprite(string resourceName)
        {
            var resourcePath = resourceName.Contains("/")
                ? resourceName
                : LoaderResourceRoot + resourceName;
            if (_sprites.TryGetValue(resourcePath, out var cached))
            {
                return cached;
            }

            var texture = Resources.Load<Texture2D>(resourcePath);
            if (texture == null)
            {
                return _whiteSprite;
            }

            var sprite = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height),
                new Vector2(0.5f, 0.5f), 100f);
            _sprites[resourcePath] = sprite;
            return sprite;
        }

        private void DestroySelf()
        {
            if (_active == this)
            {
                _active = null;
            }

            Destroy(gameObject);
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
