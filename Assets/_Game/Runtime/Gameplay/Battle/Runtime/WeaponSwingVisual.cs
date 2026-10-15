using UnityEngine;

namespace Game.Gameplay.Battle.Runtime
{
    public sealed class WeaponSwingVisual : MonoBehaviour
    {
        private float _elapsed;
        private float _duration;
        private float _degreesPerSecond;
        private Transform _blade;

        public void Play(float durationSeconds, float degreesPerSecond, Transform blade = null)
        {
            _elapsed = 0f;
            _duration = Mathf.Max(0.01f, durationSeconds);
            _degreesPerSecond = degreesPerSecond;
            _blade = blade != null ? blade : transform;
            _blade.localRotation = Quaternion.identity;
        }

        private void Update()
        {
            _elapsed += Time.deltaTime;
            if (_blade != null)
            {
                _blade.Rotate(0f, 0f, _degreesPerSecond * Time.deltaTime, Space.Self);
            }
            if (_elapsed >= _duration)
            {
                Destroy(gameObject);
            }
        }
    }

    /// <summary>Rotates an in-flight sprite without affecting its ballistic heading.</summary>
    public sealed class WeaponProjectileSpinVisual : MonoBehaviour
    {
        private float _degreesPerSecond = 720f;

        public void SetDegreesPerSecond(float degreesPerSecond)
        {
            _degreesPerSecond = degreesPerSecond;
        }

        private void Update()
        {
            transform.Rotate(0f, 0f, _degreesPerSecond * Time.deltaTime, Space.Self);
        }
    }
}
