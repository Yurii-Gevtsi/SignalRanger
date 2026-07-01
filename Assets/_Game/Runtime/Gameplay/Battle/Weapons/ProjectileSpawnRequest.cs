using Game.Gameplay.Battle.Projectiles;
using UnityEngine;

namespace Game.Gameplay.Battle.Weapons
{
    /// <summary>
    /// Data describing a single projectile a weapon effect wants to spawn.
    /// Free of scene concerns so weapon effects stay testable with a fake spawner.
    /// </summary>
    public readonly struct ProjectileSpawnRequest
    {
        public ProjectileSpawnRequest(
            ProjectileDefinition definition,
            string ownerUnitId,
            Vector2 position,
            Vector2 velocity,
            float visualDiameter,
            Color color,
            Collider2D ignoredOwnerCollider)
        {
            Definition = definition;
            OwnerUnitId = ownerUnitId;
            Position = position;
            Velocity = velocity;
            VisualDiameter = visualDiameter;
            Color = color;
            IgnoredOwnerCollider = ignoredOwnerCollider;
        }

        public ProjectileDefinition Definition { get; }
        public string OwnerUnitId { get; }
        public Vector2 Position { get; }
        public Vector2 Velocity { get; }
        public float VisualDiameter { get; }
        public Color Color { get; }
        public Collider2D IgnoredOwnerCollider { get; }
    }
}
