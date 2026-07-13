using Game.Gameplay.Battle.Terrain;
using UnityEngine;

namespace Game.Gameplay.Battle.AI.Ballistics
{
    /// <summary>
    /// Forward-simulates a projectile arc against terrain and unit bodies without
    /// touching game state. The integration mirrors
    /// <c>BattleSimulation.StepProjectiles</c> exactly (same fixed tick, sub-step
    /// length, gravity, kill height and collision tests) so a probed arc lands
    /// where the real shot would. Constants are injected, never re-hardcoded, so
    /// the probe and the simulation cannot drift apart.
    /// </summary>
    public sealed class TrajectoryProbe
    {
        private const float Epsilon = 1e-4f;

        private readonly TerrainGrid _terrain;
        private readonly Vector2 _gravityBase;
        private readonly float _tickSeconds;
        private readonly float _subStepLength;
        private readonly float _fallDeathY;

        /// <param name="terrain">Solidity mask the arc collides against.</param>
        /// <param name="gravityBase">Per-scale gravity (the simulation's projectile gravity, e.g. Physics2D.gravity).</param>
        /// <param name="tickSeconds">Fixed simulation tick (BattleSimulationSettings.SimulationTickSeconds).</param>
        /// <param name="subStepLength">Max distance per collision sub-step (BattleSimulation.ProjectileSubStepLength = 0.04).</param>
        /// <param name="fallDeathY">Kill height (BattleSimulationSettings.FallDeathY).</param>
        public TrajectoryProbe(TerrainGrid terrain, Vector2 gravityBase, float tickSeconds, float subStepLength, float fallDeathY)
        {
            _terrain = terrain;
            _gravityBase = gravityBase;
            _tickSeconds = Mathf.Max(1f / 480f, tickSeconds);
            _subStepLength = Mathf.Max(0.005f, subStepLength);
            _fallDeathY = fallDeathY;
        }

        /// <summary>
        /// Simulates the arc launched from <paramref name="origin"/> (already
        /// muzzle-offset by the caller) with <paramref name="launchVelocity"/>.
        /// </summary>
        public TrajectoryReport Simulate(
            Vector2 origin,
            Vector2 launchVelocity,
            float gravityScale,
            float projectileRadius,
            float timeoutSeconds,
            in TrajectoryObstacles obstacles)
        {
            var gravity = _gravityBase * gravityScale;
            var position = origin;
            var velocity = launchVelocity;
            var remainingLifetime = Mathf.Max(0f, timeoutSeconds);
            var elapsed = 0f;

            var toTarget = obstacles.TargetPoint - origin;
            var baseDistance = toTarget.magnitude;
            var axis = baseDistance > Epsilon ? toTarget / baseDistance : Vector2.zero;
            var maxProgress = 0f;

            // Bound the loop the same way lifetime bounds the real projectile.
            var maxTicks = Mathf.CeilToInt(remainingLifetime / _tickSeconds) + 2;
            for (var tick = 0; tick < maxTicks; tick++)
            {
                remainingLifetime -= _tickSeconds;
                if (remainingLifetime <= 0f)
                {
                    return Report(TrajectoryEndKind.Timeout, position, obstacles, baseDistance, maxProgress, elapsed, false, false);
                }

                velocity += gravity * _tickSeconds;
                var displacement = velocity * _tickSeconds;
                var stepCount = Mathf.Max(1, Mathf.CeilToInt(displacement.magnitude / _subStepLength));
                var step = displacement / stepCount;

                for (var stepIndex = 0; stepIndex < stepCount; stepIndex++)
                {
                    position += step;
                    elapsed += _tickSeconds / stepCount;

                    if (baseDistance > Epsilon)
                    {
                        var progress = Vector2.Dot(position - origin, axis);
                        if (progress > maxProgress)
                        {
                            maxProgress = progress;
                        }
                    }

                    if (position.y < _fallDeathY - 2f)
                    {
                        return Report(TrajectoryEndKind.OutOfWorld, position, obstacles, baseDistance, maxProgress, elapsed, false, false);
                    }

                    if (_terrain != null && _terrain.IsSolidWorld(position))
                    {
                        return Report(TrajectoryEndKind.Terrain, position, obstacles, baseDistance, maxProgress, elapsed, false, false);
                    }

                    if (TryHitBody(position, projectileRadius, obstacles, out var hitPoint, out var hitTarget, out var hitAlly))
                    {
                        return Report(TrajectoryEndKind.Unit, hitPoint, obstacles, baseDistance, maxProgress, elapsed, hitTarget, hitAlly);
                    }
                }
            }

            return Report(TrajectoryEndKind.Timeout, position, obstacles, baseDistance, maxProgress, elapsed, false, false);
        }

        private static bool TryHitBody(
            Vector2 position,
            float radius,
            in TrajectoryObstacles obstacles,
            out Vector2 hitPoint,
            out bool hitTarget,
            out bool hitAlly)
        {
            hitPoint = position;
            hitTarget = false;
            hitAlly = false;

            var bodies = obstacles.Bodies;
            for (var i = 0; i < bodies.Count; i++)
            {
                var body = bodies[i];
                if (Mathf.Abs(position.x - body.Position.x) <= body.HalfWidth + radius
                    && Mathf.Abs(position.y - body.Position.y) <= body.HalfHeight + radius)
                {
                    // Mirror BattleSimulation.TryHitUnit's reported impact point.
                    hitPoint = body.Position + (position - body.Position) * 0.5f;
                    hitTarget = body.IsTarget;
                    hitAlly = body.IsAlly;
                    return true;
                }
            }

            return false;
        }

        private static TrajectoryReport Report(
            TrajectoryEndKind endKind,
            Vector2 endPoint,
            in TrajectoryObstacles obstacles,
            float baseDistance,
            float maxProgress,
            float flightTime,
            bool endedOnTarget,
            bool endedOnAlly)
        {
            var distanceToTarget = (endPoint - obstacles.TargetPoint).magnitude;
            var clearPathRatio = baseDistance > Epsilon
                ? maxProgress / baseDistance
                : (endedOnTarget ? 1f : 0f);
            return new TrajectoryReport(endKind, endPoint, distanceToTarget, clearPathRatio, flightTime, endedOnTarget, endedOnAlly);
        }
    }
}
