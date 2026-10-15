using UnityEngine;

namespace Game.Gameplay.Battle.AI.Ballistics
{
    /// <summary>What ended a forward-simulated projectile arc.</summary>
    public enum TrajectoryEndKind
    {
        /// <summary>Hit the terrain solidity mask.</summary>
        Terrain,
        /// <summary>Struck a unit body.</summary>
        Unit,
        /// <summary>Ran out of lifetime without hitting anything.</summary>
        Timeout,
        /// <summary>Fell below the kill height.</summary>
        OutOfWorld
    }

    /// <summary>
    /// Read-only outcome of a shot the bot considers, produced by
    /// <see cref="TrajectoryProbe"/> without mutating any game state. Mirrors the
    /// real projectile step so the bot never plans a shot the simulation would
    /// resolve differently.
    /// </summary>
    public readonly struct TrajectoryReport
    {
        public TrajectoryReport(
            TrajectoryEndKind endKind,
            Vector2 endPoint,
            float distanceToTarget,
            float clearPathRatio,
            float flightTime,
            bool endedOnTargetUnit,
            bool endedOnAllyUnit,
            int bounceCount)
        {
            EndKind = endKind;
            EndPoint = endPoint;
            DistanceToTarget = distanceToTarget;
            ClearPathRatio = Mathf.Clamp01(clearPathRatio);
            FlightTime = flightTime;
            EndedOnTargetUnit = endedOnTargetUnit;
            EndedOnAllyUnit = endedOnAllyUnit;
            BounceCount = Mathf.Max(0, bounceCount);
        }

        public TrajectoryEndKind EndKind { get; }

        /// <summary>Where the arc resolved (terrain hit / unit hit / last airborne point).</summary>
        public Vector2 EndPoint { get; }

        /// <summary>Distance from <see cref="EndPoint"/> to the intended aim target.</summary>
        public float DistanceToTarget { get; }

        /// <summary>
        /// Fraction (0..1) of the origin→target axis the arc covered before it
        /// resolved. A shot blocked by an obstacle right away scores near 0; one
        /// that reaches the target region scores near 1.
        /// </summary>
        public float ClearPathRatio { get; }

        public float FlightTime { get; }

        /// <summary>The arc ended by striking the intended target unit.</summary>
        public bool EndedOnTargetUnit { get; }

        /// <summary>The arc ended by striking a friendly unit (self-fire risk).</summary>
        public bool EndedOnAllyUnit { get; }

        /// <summary>Number of terrain bounces before the projectile resolved.</summary>
        public int BounceCount { get; }
    }
}
