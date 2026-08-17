using System.Collections.Generic;
using UnityEngine;

namespace Game.Gameplay.Battle.Visuals
{
    public sealed class AnimatedSkyBackground : MonoBehaviour
    {
        private const string ResourceRoot = "Visuals/Arenas/Backgrounds/Sky1/";
        private const float PixelsPerUnit = 80f;
        private const float WrapLeft = -11.5f;
        private const float WrapRight = 11.5f;

        private readonly List<CloudLayer> _clouds = new();
        private readonly List<Sprite> _runtimeSprites = new();

        private Transform _station;
        private Transform _smallPlanet;
        private Transform _backdrop;
        private Vector3 _stationOrigin;
        private Vector3 _baseLocalPosition;
        private Vector3 _baseLocalScale;
        private float _elapsed;

        private sealed class CloudLayer
        {
            public Transform Transform;
            public float Speed;
            public float HalfWidth;
        }

        private void Awake()
        {
            _baseLocalPosition = transform.localPosition;
            _baseLocalScale = transform.localScale;
            _backdrop = CreateLayer("Empty Background", "empty_back", Vector3.zero, Vector3.one, -30);

            _smallPlanet = CreateLayer(
                "Orbiting Planet",
                "small_planet",
                new Vector3(-8.55f, 4.35f, 0f),
                new Vector3(0.13f, 0.13f, 1f),
                -28);

            _stationOrigin = new Vector3(-2.35f, 3.25f, 0f);
            _station = CreateLayer(
                "Floating Station",
                "station",
                _stationOrigin,
                new Vector3(0.38f, 0.38f, 1f),
                -27);

            CreateCloud("Cloud 1 Near", "cloud1", new Vector3(-5.2f, 2.15f, 0f), new Vector2(0.34f, 0.25f), 0.20f, -25, false, 0f);
            CreateCloud("Cloud 2 Near", "cloud2", new Vector3(4.4f, 2.75f, 0f), new Vector2(0.29f, 0.22f), -0.14f, -24, true, -5f);
            CreateCloud("Cloud 1 Far", "cloud1", new Vector3(8.6f, 3.75f, 0f), new Vector2(0.20f, 0.14f), -0.08f, -26, true, 0f);
            CreateCloud("Cloud 2 Far", "cloud2", new Vector3(-9.3f, 1.35f, 0f), new Vector2(0.18f, 0.13f), 0.11f, -26, false, -5f);
        }

        private void Update()
        {
            _elapsed += Time.deltaTime;

            if (_station != null)
            {
                _station.localPosition = _stationOrigin + Vector3.up * (Mathf.Sin(_elapsed * 0.62f) * 0.22f);
            }

            if (_smallPlanet != null)
            {
                var angle = _elapsed * 0.21f;
                _smallPlanet.localPosition = new Vector3(
                    -8.55f + Mathf.Cos(angle) * 1.38f,
                    4.35f + Mathf.Sin(angle) * 0.87f,
                    0f);
            }

            for (var i = 0; i < _clouds.Count; i++)
            {
                MoveCloud(_clouds[i], Time.deltaTime);
            }
        }

        /// <summary>
        /// Places and sizes the sky once in fixed world space (rather than
        /// re-centring on the camera every frame) so zooming and panning
        /// naturally reveal more or less of it, just like any other world
        /// layer. Sized to fully cover <paramref name="cameraWorldBounds"/>
        /// expanded by the largest permitted view, so there is never a gap
        /// no matter where the camera is clamped to. Vertical placement uses
        /// the camera's position at call time (battle start), so the initial
        /// framing is unchanged from before.
        /// </summary>
        public void ConfigureCoverage(Camera camera, float maximumOrthographicSize, Rect cameraWorldBounds)
        {
            if (camera == null || _backdrop == null || maximumOrthographicSize <= 0f)
            {
                return;
            }

            var backdropRenderer = _backdrop.GetComponent<SpriteRenderer>();
            var backdropSprite = backdropRenderer != null ? backdropRenderer.sprite : null;
            if (backdropSprite == null)
            {
                return;
            }

            var backdropSize = backdropSprite.bounds.size;
            if (backdropSize.x <= 0f || backdropSize.y <= 0f)
            {
                return;
            }

            const float coveragePadding = 1.12f;
            var maxHalfHeight = maximumOrthographicSize;
            var maxHalfWidth = maximumOrthographicSize * camera.aspect;
            var targetWidth = (cameraWorldBounds.width + maxHalfWidth * 2f) * coveragePadding;
            var targetHeight = (cameraWorldBounds.height + maxHalfHeight * 2f) * coveragePadding;
            var multiplier = Mathf.Max(
                targetWidth / (backdropSize.x * _baseLocalScale.x),
                targetHeight / (backdropSize.y * _baseLocalScale.y));
            transform.localScale = _baseLocalScale * multiplier;
            transform.position = new Vector3(
                cameraWorldBounds.center.x,
                camera.transform.position.y + _baseLocalPosition.y,
                transform.position.z);
        }

        /// <summary>
        /// Top world-space extent of the decorative layers (station/clouds),
        /// used by the battle camera to make sure its pan bounds actually
        /// reach up far enough to bring this content into view.
        /// </summary>
        public float GetHighestDecorativeWorldY()
        {
            var topY = float.NegativeInfinity;
            topY = IncludeTop(topY, _station);
            topY = IncludeTop(topY, _smallPlanet);
            for (var i = 0; i < _clouds.Count; i++)
            {
                topY = IncludeTop(topY, _clouds[i].Transform);
            }

            return topY;
        }

        private static float IncludeTop(float topY, Transform layer)
        {
            var renderer = layer != null ? layer.GetComponent<SpriteRenderer>() : null;
            if (renderer == null || renderer.sprite == null)
            {
                return topY;
            }

            return Mathf.Max(topY, renderer.bounds.max.y);
        }

        private void OnDestroy()
        {
            for (var i = 0; i < _runtimeSprites.Count; i++)
            {
                if (_runtimeSprites[i] != null)
                {
                    if (Application.isPlaying)
                    {
                        Destroy(_runtimeSprites[i]);
                    }
                    else
                    {
                        DestroyImmediate(_runtimeSprites[i]);
                    }
                }
            }
        }

        private void CreateCloud(
            string name,
            string resourceName,
            Vector3 position,
            Vector2 scale,
            float speed,
            int sortingOrder,
            bool flipX,
            float rotationZ)
        {
            var cloud = CreateLayer(
                name,
                resourceName,
                position,
                new Vector3(scale.x, scale.y, 1f),
                sortingOrder,
                flipX);
            if (cloud == null)
            {
                return;
            }

            cloud.localRotation = Quaternion.Euler(0f, 0f, rotationZ);

            var renderer = cloud.GetComponent<SpriteRenderer>();
            _clouds.Add(new CloudLayer
            {
                Transform = cloud,
                Speed = speed,
                HalfWidth = renderer.sprite.bounds.extents.x * Mathf.Abs(scale.x)
            });
        }

        private Transform CreateLayer(
            string name,
            string resourceName,
            Vector3 position,
            Vector3 scale,
            int sortingOrder,
            bool flipX = false)
        {
            var texture = Resources.Load<Texture2D>(ResourceRoot + resourceName);
            if (texture == null)
            {
                Debug.LogWarning($"Animated sky layer '{ResourceRoot + resourceName}' was not found.");
                return null;
            }

            var sprite = Sprite.Create(
                texture,
                new Rect(0f, 0f, texture.width, texture.height),
                new Vector2(0.5f, 0.5f),
                PixelsPerUnit,
                0,
                SpriteMeshType.FullRect);
            sprite.name = resourceName;
            _runtimeSprites.Add(sprite);

            var layer = new GameObject(name);
            layer.transform.SetParent(transform, false);
            layer.transform.localPosition = position;
            layer.transform.localScale = scale;

            var renderer = layer.AddComponent<SpriteRenderer>();
            renderer.sprite = sprite;
            renderer.sortingOrder = sortingOrder;
            renderer.flipX = flipX;
            return layer.transform;
        }

        private static void MoveCloud(CloudLayer cloud, float deltaTime)
        {
            if (cloud?.Transform == null)
            {
                return;
            }

            var position = cloud.Transform.localPosition;
            position.x += cloud.Speed * deltaTime;
            if (cloud.Speed > 0f && position.x - cloud.HalfWidth > WrapRight)
            {
                position.x = WrapLeft - cloud.HalfWidth;
            }
            else if (cloud.Speed < 0f && position.x + cloud.HalfWidth < WrapLeft)
            {
                position.x = WrapRight + cloud.HalfWidth;
            }

            cloud.Transform.localPosition = position;
        }
    }
}
