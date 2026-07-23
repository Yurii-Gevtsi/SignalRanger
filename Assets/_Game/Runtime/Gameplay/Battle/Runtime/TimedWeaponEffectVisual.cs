using UnityEngine;

namespace Game.Gameplay.Battle.Runtime
{
    [RequireComponent(typeof(SpriteRenderer))]
    public sealed class LinearWeaponEffectVisual : MonoBehaviour
    {
        private SpriteRenderer _renderer;
        private Vector2 _origin;
        private Vector2 _direction = Vector2.right;
        private float _range;
        private float _duration = 1f;
        private float _elapsed;

        public void Play(Vector2 origin, Vector2 direction, float range, float durationSeconds)
        {
            _renderer = GetComponent<SpriteRenderer>();
            _origin = origin;
            _direction = direction.sqrMagnitude > 0.01f ? direction.normalized : Vector2.right;
            _range = Mathf.Max(0.01f, range);
            _duration = Mathf.Max(0.01f, durationSeconds);
            Apply(0.01f);
        }

        private void Update()
        {
            _elapsed += Time.deltaTime;
            Apply(Mathf.Clamp01(_elapsed / _duration));
        }

        private void Apply(float normalizedTime)
        {
            var length = Mathf.Lerp(0.25f, _range, normalizedTime);
            transform.position = _origin + _direction * (length * 0.5f);
            transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg);

            var spriteSize = _renderer != null && _renderer.sprite != null
                ? _renderer.sprite.bounds.size
                : Vector3.one;
            var widthScale = length / Mathf.Max(0.01f, spriteSize.x);
            var heightScale = 0.24f / Mathf.Max(0.01f, spriteSize.y);
            transform.localScale = new Vector3(widthScale, heightScale, 1f);
        }
    }

    public sealed class FireBlobVisual : MonoBehaviour
    {
        private Vector2 _origin;
        private Vector2 _initialVelocity;
        private Vector2 _gravity;
        private Vector2 _impactPoint;
        private float _travelSeconds = 0.2f;
        private float _burnLifetimeSeconds = 3f;
        private float _elapsed;

        public void Play(
            Vector2 origin,
            Vector2 initialVelocity,
            Vector2 gravity,
            Vector2 impactPoint,
            float travelSeconds,
            float burnLifetimeSeconds)
        {
            _origin = origin;
            _initialVelocity = initialVelocity;
            _gravity = gravity;
            _impactPoint = impactPoint;
            _travelSeconds = Mathf.Max(0.01f, travelSeconds);
            _burnLifetimeSeconds = Mathf.Max(0.01f, burnLifetimeSeconds);
            transform.position = origin;
        }

        private void Update()
        {
            if (_elapsed >= _travelSeconds)
            {
                var burnElapsed = Mathf.Min(_elapsed - _travelSeconds, _burnLifetimeSeconds);
                transform.position = _impactPoint + Vector2.down * (burnElapsed * 0.62f);
                _elapsed += Time.deltaTime;
                return;
            }

            _elapsed += Time.deltaTime;
            var t = Mathf.Min(_elapsed, _travelSeconds);
            transform.position = _origin + _initialVelocity * t + 0.5f * _gravity * (t * t);
        }
    }

    public sealed class GroundDropVisual : MonoBehaviour
    {
        private Vector2 _origin;
        private Vector2 _destination;
        private float _duration = 0.45f;
        private float _elapsed;

        public void Play(Vector2 origin, Vector2 destination, float durationSeconds)
        {
            _origin = origin;
            _destination = destination;
            _duration = Mathf.Max(0.01f, durationSeconds);
            transform.position = origin;
        }

        private void Update()
        {
            _elapsed = Mathf.Min(_duration, _elapsed + Time.deltaTime);
            var t = _elapsed / _duration;
            var arc = Mathf.Sin(t * Mathf.PI) * 0.18f;
            transform.position = Vector2.Lerp(_origin, _destination, t) + Vector2.up * arc;
        }
    }
}
