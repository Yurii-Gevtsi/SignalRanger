using UnityEngine;

namespace Game.Gameplay.Battle.Projectiles
{
    /// <summary>
    /// What a projectile needs to know about the battle: the terrain mask,
    /// hittable units and where the world ends. Implemented by the battle
    /// scene so projectiles fly through the same collision source as units.
    /// </summary>
    public interface IProjectileWorld
    {
        /// <summary>Below this height a projectile resolves without effect.</summary>
        float KillY { get; }

        bool IsSolid(Vector2 point);

        bool TryHitUnit(Vector2 point, float radius, string ignoredUnitId, out Vector2 hitPoint);
    }
}
