using UnityEngine;

namespace Game.Gameplay.Battle.Terrain
{
    public readonly struct BattleTerrainSpriteSurface
    {
        public BattleTerrainSpriteSurface(SpriteRenderer renderer, Texture2D collisionMask, bool isMoving = false)
        {
            Renderer = renderer;
            CollisionMask = collisionMask;
            IsMoving = isMoving;
        }

        public SpriteRenderer Renderer { get; }
        public Texture2D CollisionMask { get; }
        public bool IsMoving { get; }
        public bool HasCollisionMask => CollisionMask != null;
    }
}
