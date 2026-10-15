using UnityEngine;

namespace Game.Gameplay.Battle.AI.Ballistics
{
    /// <summary>
    /// Closed-form inverse ballistics: given a launch speed, find the launch
    /// velocities that carry a projectile from origin to target under constant
    /// downward gravity. This is the same motion the simulation integrates, so a
    /// solution here is a shot the simulation can actually make.
    /// </summary>
    public static class BallisticsSolver
    {
        private const float Epsilon = 1e-4f;

        /// <summary>
        /// Solves for the launch velocities (magnitude == <paramref name="launchSpeed"/>)
        /// that reach <paramref name="target"/> from <paramref name="origin"/> under gravity
        /// of magnitude <paramref name="gravityMagnitude"/> (downward). Returns the number of
        /// real solutions: 0 (unreachable), 1 (single arc / degenerate) or 2.
        /// <paramref name="lowArc"/> is the flatter, faster arc; <paramref name="highArc"/> the lofted one.
        /// </summary>
        public static int SolveLaunchVelocities(
            Vector2 origin,
            Vector2 target,
            float launchSpeed,
            float gravityMagnitude,
            out Vector2 lowArc,
            out Vector2 highArc)
        {
            lowArc = Vector2.zero;
            highArc = Vector2.zero;

            if (launchSpeed <= Epsilon)
            {
                return 0;
            }

            var delta = target - origin;

            // Gravity-free shot travels in a straight line at launch speed.
            if (gravityMagnitude <= Epsilon)
            {
                if (delta.sqrMagnitude <= Epsilon * Epsilon)
                {
                    return 0;
                }

                lowArc = delta.normalized * launchSpeed;
                highArc = lowArc;
                return 1;
            }

            // With v = launchSpeed and u = t^2, the reach constraint becomes
            //   (g^2/4)·u^2 + (g·Δy − v^2)·u + (Δx^2 + Δy^2) = 0.
            var g = gravityMagnitude;
            var speedSquared = launchSpeed * launchSpeed;
            var a = g * g * 0.25f;
            var b = g * delta.y - speedSquared;
            var c = delta.sqrMagnitude;
            var discriminant = b * b - 4f * a * c;
            if (discriminant < 0f)
            {
                return 0;
            }

            var sqrtDiscriminant = Mathf.Sqrt(discriminant);
            // Larger u -> larger flight time -> the lofted (high) arc.
            var highTimeSquared = (-b + sqrtDiscriminant) / (2f * a);
            var lowTimeSquared = (-b - sqrtDiscriminant) / (2f * a);

            var hasHigh = TryVelocityForTimeSquared(delta, g, highTimeSquared, out var high);
            var hasLow = TryVelocityForTimeSquared(delta, g, lowTimeSquared, out var low);

            if (hasLow && hasHigh)
            {
                lowArc = low;
                highArc = high;
                return 2;
            }

            if (hasLow)
            {
                lowArc = low;
                highArc = low;
                return 1;
            }

            if (hasHigh)
            {
                lowArc = high;
                highArc = high;
                return 1;
            }

            return 0;
        }

        private static bool TryVelocityForTimeSquared(Vector2 delta, float gravityMagnitude, float timeSquared, out Vector2 velocity)
        {
            velocity = Vector2.zero;
            if (timeSquared <= Epsilon)
            {
                return false;
            }

            var t = Mathf.Sqrt(timeSquared);
            var vx = delta.x / t;
            var vy = (delta.y + 0.5f * gravityMagnitude * t * t) / t;
            velocity = new Vector2(vx, vy);
            return true;
        }
    }
}
