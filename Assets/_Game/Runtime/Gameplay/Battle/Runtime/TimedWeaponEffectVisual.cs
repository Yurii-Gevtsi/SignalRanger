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
        private float _thicknessMultiplier = 1f;

        public void SetThickness(float multiplier)
        {
            _thicknessMultiplier = Mathf.Max(0.01f, multiplier);
        }

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
            var heightScale = 1.2f * _thicknessMultiplier / Mathf.Max(0.01f, spriteSize.y);
            transform.localScale = new Vector3(widthScale, heightScale, 1f);
        }
    }

    public sealed class FireBlobVisual : MonoBehaviour
    {
        // How fast the flame visual glides toward the simulation's burn point
        // (which creeps down as the fire eats the ground, or drops to a fresh
        // surface after burning through).
        private const float BurnPointFollowSpeed = 9f;

        private Vector2 _origin;
        private Vector2 _initialVelocity;
        private Vector2 _gravity;
        private Vector2 _burnPoint;
        private float _travelSeconds = 0.2f;
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
            _burnPoint = impactPoint;
            _travelSeconds = Mathf.Max(0.01f, travelSeconds);
            transform.position = origin;
        }

        /// <summary>
        /// Simulation-driven burn position: the flame follows the puddle it
        /// represents instead of animating an independent downward drift.
        /// </summary>
        public void SetBurnPoint(Vector2 point)
        {
            _burnPoint = point;
        }

        private void Update()
        {
            _elapsed += Time.deltaTime;
            if (_elapsed >= _travelSeconds)
            {
                var follow = 1f - Mathf.Exp(-BurnPointFollowSpeed * Time.deltaTime);
                transform.position = Vector2.Lerp(transform.position, _burnPoint, follow);
                return;
            }

            var t = Mathf.Min(_elapsed, _travelSeconds);
            transform.position = _origin + _initialVelocity * t + 0.5f * _gravity * (t * t);
        }
    }

    /// <summary>A released cargo item falls straight down under gravity instead of jumping to the ground.</summary>
    public sealed class CargoFallVisual : MonoBehaviour
    {
        private const float FallGravity = 14f;
        private Vector2 _destination;
        private float _speed;

        public void Play(Vector2 origin, Vector2 destination)
        {
            _destination = destination;
            _speed = 0f;
            transform.position = origin;
        }

        private void Update()
        {
            _speed += FallGravity * Time.deltaTime;
            var position = (Vector2)transform.position;
            var next = Vector2.MoveTowards(position, _destination, _speed * Time.deltaTime);
            transform.position = next;
            if ((next - _destination).sqrMagnitude < 0.0001f)
            {
                Destroy(this);
            }
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
