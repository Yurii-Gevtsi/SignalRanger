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

            // A mirror bounce off the local surface: the angle of reflection equals the angle of incidence. The
            // per-axis flips below only remain for spots where no surface direction can be read.
            if (TryEstimateSurfaceNormal(terrain, contactPoint, sample * 1.5f, out var normal))
            {
                var into = Vector2.Dot(velocity, normal);
                if (into < 0f)
                {
                    return velocity - 2f * into * normal;
                }
            }
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

        private const int NormalSampleCount = 16;

        /// <summary>
        /// Reads the outward surface direction at a contact point from which neighbouring samples are solid: the
        /// normal points away from the solid side. False inside solid ground or in open air.
        /// </summary>
        private static bool TryEstimateSurfaceNormal(TerrainGrid terrain, Vector2 point, float radius, out Vector2 normal)
        {
            var solidSum = Vector2.zero;
            var solidCount = 0;
            for (var index = 0; index < NormalSampleCount; index++)
            {
                var angle = index * (2f * Mathf.PI / NormalSampleCount);
                var direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
                if (terrain.IsSolidWorld(point + direction * radius))
                {
                    solidSum += direction;
                    solidCount++;
                }
            }

            normal = Vector2.zero;
            if (solidCount == 0 || solidCount == NormalSampleCount || solidSum.sqrMagnitude < 0.0001f)
            {
                return false;
            }

            normal = -solidSum.normalized;
            return true;
        }
    }
}
