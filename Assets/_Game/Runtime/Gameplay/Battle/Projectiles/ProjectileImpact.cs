using UnityEngine;

namespace Game.Gameplay.Battle.Projectiles
{
    public readonly struct ProjectileImpact
    {
        public ProjectileImpact(
            string projectileId,
            string ownerUnitId,
            Vector2 point,
            Vector2 velocity,
            int damage,
            float impactRadius,
            float knockbackMultiplier = 1f,
            Vector2? sourceOrigin = null)
        {
            ProjectileId = projectileId;
            OwnerUnitId = ownerUnitId;
            Point = point;
            Velocity = velocity;
            Damage = damage;
            ImpactRadius = impactRadius;
            KnockbackMultiplier = Mathf.Max(0f, knockbackMultiplier);
            SourceOrigin = sourceOrigin;
        }

        public string ProjectileId { get; }
        public string OwnerUnitId { get; }
        public Vector2 Point { get; }
        public Vector2 Velocity { get; }
        public int Damage { get; }
        public float ImpactRadius { get; }
        public float KnockbackMultiplier { get; }
        public Vector2? SourceOrigin { get; }
    }
}
