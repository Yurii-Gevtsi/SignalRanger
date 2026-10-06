using System;
using UnityEngine;

namespace Game.Gameplay.Battle.Arenas
{
    [Serializable]
    public struct ArenaSpriteSurface
    {
        [SerializeField] private string _instanceId;
        [SerializeField] private string _visualResourcePath;
        [SerializeField] private string _maskResourcePath;
        [SerializeField] private Vector2 _position;
        [SerializeField] private float _scale;
        [SerializeField] private int _sortingOrder;
        [SerializeField] private float _horizontalMotionRange;
        [SerializeField] private float _horizontalMotionSpeed;

        public ArenaSpriteSurface(
            string instanceId,
            string visualResourcePath,
            string maskResourcePath,
            Vector2 position,
            float scale,
            int sortingOrder,
            float horizontalMotionRange = 0f,
            float horizontalMotionSpeed = 0f)
        {
            _instanceId = instanceId;
            _visualResourcePath = visualResourcePath;
            _maskResourcePath = maskResourcePath;
            _position = position;
            _scale = scale;
            _sortingOrder = sortingOrder;
            _horizontalMotionRange = Mathf.Max(0f, horizontalMotionRange);
            _horizontalMotionSpeed = Mathf.Max(0f, horizontalMotionSpeed);
        }

        public string InstanceId => _instanceId;
        public string VisualResourcePath => _visualResourcePath;
        public string MaskResourcePath => _maskResourcePath;
        public Vector2 Position => _position;
        public float Scale => _scale;
        public int SortingOrder => _sortingOrder;
        public bool HasHorizontalMotion => _horizontalMotionRange > 0f && _horizontalMotionSpeed > 0f;
        public float HorizontalMotionRange => _horizontalMotionRange;
        public float HorizontalMotionSpeed => _horizontalMotionSpeed;
    }
}
