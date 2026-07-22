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
        private Vector3 _stationOrigin;
        private float _elapsed;

        private sealed class CloudLayer
        {
            public Transform Transform;
            public float Speed;
            public float HalfWidth;
        }

        private void Awake()
        {
            CreateLayer("Empty Background", "empty_back", Vector3.zero, Vector3.one, -30);

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
