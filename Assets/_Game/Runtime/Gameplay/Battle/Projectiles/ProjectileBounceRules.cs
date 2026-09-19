using Game.Gameplay.Battle.Terrain;
using UnityEngine;

namespace Game.Gameplay.Battle.Projectiles
{
    /// <summary>
    /// Shared deterministic terrain-bounce math used by the live projectile
    /// simulation and the bot's read-only trajectory probe.
    /// </summary>
    public static class ProjectileBounceRules
    {
        public const float SpeedRetain = 0.46f;
        public const int MaxBounces = 5;
        public const float RestingFuseMinSpeed = 0.35f;

        public static Vector2 ReflectOffTerrain(
            TerrainGrid terrain,
            Vector2 contactPoint,
            Vector2 velocity,
            float projectileRadius,
            float subStepLength)
        {
            var reflected = velocity;
            if (terrain == null)
            {
                return new Vector2(velocity.x, -velocity.y);
            }

            var sample = Mathf.Max(projectileRadius, 0.06f) + Mathf.Max(0.005f, subStepLength);
            var hitLeft = terrain.IsSolidWorld(contactPoint + Vector2.left * sample);
            var hitRight = terrain.IsSolidWorld(contactPoint + Vector2.right * sample);
            var hitTop = terrain.IsSolidWorld(contactPoint + Vector2.up * sample);
            var hitBottom = terrain.IsSolidWorld(contactPoint + Vector2.down * sample);

            if (hitLeft && !hitRight)
            {
                reflected.x = Mathf.Abs(reflected.x);
            }
            else if (hitRight && !hitLeft)
            {
                reflected.x = -Mathf.Abs(reflected.x);
            }

            if (hitBottom && !hitTop)
            {
                reflected.y = Mathf.Abs(reflected.y);
            }
            else if (hitTop && !hitBottom)
            {
                reflected.y = -Mathf.Abs(reflected.y);
            }

            if (reflected == velocity)
            {
                // Corner or thin sliver the axis probes missed: fall back to a
                // velocity-based reflection so the projectile never tunnels.
                reflected = Mathf.Abs(velocity.y) >= Mathf.Abs(velocity.x)
                    ? new Vector2(velocity.x, -velocity.y)
                    : new Vector2(-velocity.x, velocity.y);
            }

            return reflected;
        }
    }
}
