using System;
using Game.Gameplay.Battle.Terrain;
using UnityEngine;

namespace Game.Gameplay.Battle.Arenas
{
    [Serializable]
    public struct ArenaTerrainBlock
    {
        [SerializeField] private string _blockId;
        [SerializeField] private Vector2 _position;
        [SerializeField] private Vector2 _size;
        [SerializeField] private Color _color;
        [SerializeField] private bool _solid;
        [SerializeField] private int _sortingOrder;
        [SerializeField] private TerrainMaterial _material;

        public ArenaTerrainBlock(
            string blockId,
            Vector2 position,
            Vector2 size,
            Color color,
            bool solid,
            int sortingOrder,
            TerrainMaterial material = TerrainMaterial.Soil)
        {
            _blockId = blockId;
            _position = position;
            _size = size;
            _color = color;
            _solid = solid;
            _sortingOrder = sortingOrder;
            _material = material;
        }

        public string BlockId => _blockId;
        public Vector2 Position => _position;
        public Vector2 Size => _size;
        public Color Color => _color;
        public bool Solid => _solid;
        public int SortingOrder => _sortingOrder;
        public TerrainMaterial Material => _material;
    }
}
