using UnityEngine;

namespace Game.Gameplay.Battle.Domain
{
    public readonly struct AimSettings
    {
        public AimSettings(float minPower, float maxPower, float maxInputDistance, float minInputDistance, Vector2 fallbackDirection)
        {
            MinPower = Mathf.Max(0f, minPower);
            MaxPower = Mathf.Max(MinPower, maxPower);
            MinInputDistance = Mathf.Max(0f, minInputDistance);
            MaxInputDistance = Mathf.Max(MinInputDistance + 0.01f, maxInputDistance);
            FallbackDirection = fallbackDirection.sqrMagnitude > 0f ? fallbackDirection.normalized : Vector2.right;
        }

        public float MinPower { get; }
        public float MaxPower { get; }

        /// <summary>
        /// Pull length (from the press anchor) at which power reaches <see cref="MaxPower"/>.
        /// Power is interpolated across [MinInputDistance, MaxInputDistance], so power is
        /// controlled by how far you drag the slingshot back, in world units, and is
        /// independent of where on screen the pull begins.
        /// </summary>
        public float MaxInputDistance { get; }

        /// <summary>Dead zone: pulls shorter than this register no shot.</summary>
        public float MinInputDistance { get; }
        public Vector2 FallbackDirection { get; }

        public static AimSettings Default => new(4f, 13.5f, 6.8f, 0.5f, Vector2.right);
    }
}
