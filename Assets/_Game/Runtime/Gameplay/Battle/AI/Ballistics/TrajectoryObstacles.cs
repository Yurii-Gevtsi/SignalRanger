using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Gameplay.Battle.AI.Ballistics
{
    /// <summary>
    /// A unit the probe can collide against, as an axis-aligned body. The
    /// half-extents mirror how <c>BattleSimulation.TryHitUnit</c> tests hits:
    /// <see cref="HalfWidth"/> is the collision half-width and
    /// <see cref="HalfHeight"/> is half the archetype body height.
    /// </summary>
    public readonly struct TrajectoryUnitBody
    {
        public TrajectoryUnitBody(Vector2 position, float halfWidth, float halfHeight, bool isAlly, bool isTarget)
        {
            Position = position;
            HalfWidth = Mathf.Max(0f, halfWidth);
            HalfHeight = Mathf.Max(0f, halfHeight);
            IsAlly = isAlly;
            IsTarget = isTarget;
        }

        public Vector2 Position { get; }
        public float HalfWidth { get; }
        public float HalfHeight { get; }

        /// <summary>Friendly to the firing unit — a hit here is self-inflicted risk.</summary>
        public bool IsAlly { get; }

        /// <summary>The unit the shot is aimed at.</summary>
        public bool IsTarget { get; }
    }

    /// <summary>
    /// Everything the probe needs about units for one shot: the collidable
    /// bodies (the firing unit is excluded by the caller, matching the
    /// projectile's owner-skip) and the world-space aim target.
    /// </summary>
    public readonly struct TrajectoryObstacles
    {
        private static readonly TrajectoryUnitBody[] NoBodies = Array.Empty<TrajectoryUnitBody>();

        public TrajectoryObstacles(IReadOnlyList<TrajectoryUnitBody> bodies, Vector2 targetPoint)
        {
            Bodies = bodies ?? NoBodies;
            TargetPoint = targetPoint;
        }

        public IReadOnlyList<TrajectoryUnitBody> Bodies { get; }
        public Vector2 TargetPoint { get; }

        /// <summary>Obstacles where only the terrain matters (no unit bodies).</summary>
        public static TrajectoryObstacles TerrainOnly(Vector2 targetPoint)
        {
            return new TrajectoryObstacles(NoBodies, targetPoint);
        }
    }
}
