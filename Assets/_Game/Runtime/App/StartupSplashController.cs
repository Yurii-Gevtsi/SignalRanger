using System.Collections;
using Game.UI;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Game.App
{
    /// <summary>
    /// Owns the branded sequence shown once after the native application splash and
    /// before the boot scene opens the menu.
    /// </summary>
    public sealed class StartupSplashController : MonoBehaviour
    {
        private const string ResourceRoot = "UI/Startup/";
        private const float MinimumDisplayDurationSeconds = 3f;

        private static readonly string[] ImageNames =
        {
            "startup_02"
        };

        private Image _image;

        public static IEnumerator ShowWhileLoadingScene(string sceneName)
        {
            var displayedAt = Time.unscaledTime;
            CreateOverlay(out var root);

            var operation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
            if (operation == null)
            {
                SceneManager.LoadScene(sceneName);
                yield return null;
            }
            else
            {
                operation.allowSceneActivation = false;
                while (operation.progress < 0.9f)
                {
                    yield return null;
                }

                operation.allowSceneActivation = true;
                while (!operation.isDone)
                {
                    yield return null;
                }
            }

            yield return WaitForMinimumDisplay(displayedAt);

            if (root != null)
            {
                Destroy(root);
            }
        }

        public static IEnumerator ShowOverCurrentScene()
        {
            var displayedAt = Time.unscaledTime;
            CreateOverlay(out var root);
            yield return WaitForMinimumDisplay(displayedAt);

            if (root != null)
            {
                Destroy(root);
            }
        }

        private static StartupSplashController CreateOverlay(out GameObject root)
        {
            root = new GameObject("Startup Splash");
            DontDestroyOnLoad(root);

            var controller = root.AddComponent<StartupSplashController>();
            controller.Build();
            controller._image.sprite = LoadSprite(ImageNames[0]);
            return controller;
        }

        private static IEnumerator WaitForMinimumDisplay(float displayedAt)
        {
            // Scene activation and the first render happen on different frames. On a
            // fast device the menu can otherwise replace the branded screen before it
            // has ever been visible to the player.
            yield return null;
            while (Time.unscaledTime - displayedAt < MinimumDisplayDurationSeconds)
            {
                yield return null;
            }

            yield return new WaitForEndOfFrame();
        }

        private void Build()
        {
            var canvasObject = new GameObject("Startup Splash Canvas");
            canvasObject.transform.SetParent(transform, false);

            var canvas = canvasObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 4900;

            var scaler = canvasObject.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920f, 1080f);
            scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            scaler.matchWidthOrHeight = 1f;

            var imageObject = new GameObject("Startup Splash Image");
            imageObject.transform.SetParent(canvasObject.transform, false);
            var rect = imageObject.AddComponent<RectTransform>();
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;

            _image = imageObject.AddComponent<Image>();
            _image.preserveAspect = false;
            _image.raycastTarget = false;

            var versionObject = new GameObject("Startup Splash Version");
            versionObject.transform.SetParent(canvasObject.transform, false);
            var versionRect = versionObject.AddComponent<RectTransform>();
            versionRect.anchorMin = new Vector2(1f, 1f);
            versionRect.anchorMax = new Vector2(1f, 1f);
            versionRect.pivot = new Vector2(1f, 1f);
            versionRect.sizeDelta = new Vector2(180f, 42f);
            versionRect.anchoredPosition = new Vector2(-28f, -24f);
            var version = versionObject.AddComponent<TextMeshProUGUI>();
            SciFiTextStyle.Apply(version, GameTextRole.Caption, 18, TextAlignmentOptions.TopRight);
            version.text = Application.version;
            version.raycastTarget = false;
        }

        private static Sprite LoadSprite(string imageName)
        {
            var sprite = Resources.Load<Sprite>(ResourceRoot + imageName);
            if (sprite != null)
            {
                return sprite;
            }

            var texture = Resources.Load<Texture2D>(ResourceRoot + imageName);
            if (texture == null)
            {
                Debug.LogWarning($"[Startup] Missing splash image: {ResourceRoot}{imageName}");
                return null;
            }

            return Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height),
                new Vector2(0.5f, 0.5f), 100f);
        }
    }
}
