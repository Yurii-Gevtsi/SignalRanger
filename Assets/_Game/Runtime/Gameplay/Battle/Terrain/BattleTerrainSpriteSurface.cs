using UnityEngine;

namespace Game.Gameplay.Battle.Terrain
{
    public readonly struct BattleTerrainSpriteSurface
    {
        public BattleTerrainSpriteSurface(SpriteRenderer renderer, Texture2D collisionMask)
        {
            Renderer = renderer;
            CollisionMask = collisionMask;
        }

        public SpriteRenderer Renderer { get; }
        public Texture2D CollisionMask { get; }
        public bool HasCollisionMask => CollisionMask != null;
    }
}
