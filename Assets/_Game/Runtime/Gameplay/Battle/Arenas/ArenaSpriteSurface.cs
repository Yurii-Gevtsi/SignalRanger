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

        public ArenaSpriteSurface(
            string instanceId,
            string visualResourcePath,
            string maskResourcePath,
            Vector2 position,
            float scale,
            int sortingOrder)
        {
            _instanceId = instanceId;
            _visualResourcePath = visualResourcePath;
            _maskResourcePath = maskResourcePath;
            _position = position;
            _scale = scale;
            _sortingOrder = sortingOrder;
        }

        public string InstanceId => _instanceId;
        public string VisualResourcePath => _visualResourcePath;
        public string MaskResourcePath => _maskResourcePath;
        public Vector2 Position => _position;
        public float Scale => _scale;
        public int SortingOrder => _sortingOrder;
    }
}
