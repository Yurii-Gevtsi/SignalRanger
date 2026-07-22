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
            var heightScale = 0.18f / Mathf.Max(0.01f, spriteSize.y);
            transform.localScale = new Vector3(widthScale, heightScale, 1f);
        }
    }

    public sealed class FireBlobVisual : MonoBehaviour
    {
        private Vector2 _origin;
        private Vector2 _target;
        private float _travelSeconds = 0.18f;
        private float _elapsed;

        public void Play(Vector2 origin, Vector2 target, float travelSeconds)
        {
            _origin = origin;
            _target = target;
            _travelSeconds = Mathf.Max(0.01f, travelSeconds);
            transform.position = origin;
        }

        private void Update()
        {
            if (_elapsed >= _travelSeconds)
            {
                transform.position = _target;
                return;
            }

            _elapsed += Time.deltaTime;
            var t = Mathf.Clamp01(_elapsed / _travelSeconds);
            transform.position = Vector2.Lerp(_origin, _target, t);
        }
    }
}
