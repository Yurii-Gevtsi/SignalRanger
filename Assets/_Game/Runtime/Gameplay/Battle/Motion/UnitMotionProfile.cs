using UnityEngine;

namespace Game.Gameplay.Battle.Motion
{
    /// <summary>
    /// Every tunable of unit movement physics in one place: body extents,
    /// crawl allowances, gravity and bounce damping. Values are world units.
    /// </summary>
    public sealed class UnitMotionProfile
    {
        public UnitMotionProfile(
            float halfWidth,
            float halfHeight,
            float crawlHeight,
            float walkStepHeight,
            float walkSpeed,
            float gravity = 8.6f,
            float bounceDampingX = 0.3f,
            float bounceDampingY = 0.5f,
            float restSpeed = 0.55f,
            float maxSpeed = 26f,
            float maxWalkSlopeDegrees = 60f)
        {
            HalfWidth = Mathf.Max(0.05f, halfWidth);
            HalfHeight = Mathf.Max(0.1f, halfHeight);
            CrawlHeight = Mathf.Max(0.05f, crawlHeight);
            WalkStepHeight = Mathf.Max(0.05f, walkStepHeight);
            WalkSpeed = Mathf.Max(0.1f, walkSpeed);
            Gravity = Mathf.Max(0.5f, gravity);
            BounceDampingX = Mathf.Clamp01(bounceDampingX);
            BounceDampingY = Mathf.Clamp01(bounceDampingY);
            RestSpeed = Mathf.Max(0.01f, restSpeed);
            MaxSpeed = Mathf.Max(1f, maxSpeed);
            MaxWalkSlopeDegrees = Mathf.Clamp(maxWalkSlopeDegrees, 20f, 80f);
            MaxWalkSlopeTangent = Mathf.Tan(MaxWalkSlopeDegrees * Mathf.Deg2Rad);
        }

        public float HalfWidth { get; }
        public float HalfHeight { get; }

        /// <summary>How far up a unit may be lifted to resolve a soft ground contact.</summary>
        public float CrawlHeight { get; }

        /// <summary>Highest ledge a unit can climb while walking.</summary>
        public float WalkStepHeight { get; }

        public float WalkSpeed { get; }
        public float Gravity { get; }
        public float BounceDampingX { get; }
        public float BounceDampingY { get; }

        /// <summary>Below this speed a grounded unit settles instead of bouncing.</summary>
        public float RestSpeed { get; }

        public float MaxSpeed { get; }

        /// <summary>Steepest sustained incline the unit can walk up.</summary>
        public float MaxWalkSlopeDegrees { get; }

        /// <summary>Precomputed tangent of <see cref="MaxWalkSlopeDegrees"/>.</summary>
        public float MaxWalkSlopeTangent { get; }
    }
}
