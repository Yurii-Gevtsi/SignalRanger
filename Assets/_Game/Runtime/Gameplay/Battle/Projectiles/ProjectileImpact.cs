using UnityEngine;

namespace Game.Gameplay.Battle.Projectiles
{
    public readonly struct ProjectileImpact
    {
        public ProjectileImpact(string projectileId, string ownerUnitId, Vector2 point, Vector2 velocity)
        {
            ProjectileId = projectileId;
            OwnerUnitId = ownerUnitId;
            Point = point;
            Velocity = velocity;
        }

        public string ProjectileId { get; }
        public string OwnerUnitId { get; }
        public Vector2 Point { get; }
        public Vector2 Velocity { get; }
    }
}
