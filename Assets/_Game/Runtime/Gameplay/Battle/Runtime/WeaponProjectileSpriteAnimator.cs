using UnityEngine;

namespace Game.Gameplay.Battle.Runtime
{
    [RequireComponent(typeof(SpriteRenderer))]
    public sealed class WeaponProjectileSpriteAnimator : MonoBehaviour
    {
        private SpriteRenderer _renderer;
        private Sprite[] _frames;
        private float _elapsed;

        public void SetFrames(params Sprite[] frames)
        {
            _frames = frames;
            _renderer = GetComponent<SpriteRenderer>();
            if (_frames.Length > 0 && _frames[0] != null)
            {
                _renderer.sprite = _frames[0];
            }
        }

        private void Update()
        {
            if (_frames == null || _frames.Length == 0)
            {
                return;
            }

            _elapsed += Time.deltaTime;
            var frame = Mathf.FloorToInt(_elapsed * 12f) % _frames.Length;
            if (_frames[frame] != null)
            {
                _renderer.sprite = _frames[frame];
            }
        }
    }
}
