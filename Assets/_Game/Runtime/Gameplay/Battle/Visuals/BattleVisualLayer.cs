using UnityEngine;

namespace Game.Gameplay.Battle.Visuals
{
    public readonly struct BattleVisualLayer
    {
        public BattleVisualLayer(string resourcePath, Vector2 position, Vector2 scale, int sortingOrder, bool solid = false, bool coverCamera = false)
        {
            ResourcePath = resourcePath ?? string.Empty;
            Position = position;
            Scale = scale;
            SortingOrder = sortingOrder;
            Solid = solid;
            CoverCamera = coverCamera;
        }

        public string ResourcePath { get; }
        public Vector2 Position { get; }
        public Vector2 Scale { get; }
        public int SortingOrder { get; }
        public bool Solid { get; }
        public bool CoverCamera { get; }

        public bool HasSprite => !string.IsNullOrWhiteSpace(ResourcePath);
    }
}
