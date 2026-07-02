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
        /// Drag length (from the fire point) at which power reaches <see cref="MaxPower"/>.
        /// Power is interpolated across [MinInputDistance, MaxInputDistance], so power is
        /// controlled by how far you drag, not by how far away the target happens to be.
        /// </summary>
        public float MaxInputDistance { get; }
        public float MinInputDistance { get; }
        public Vector2 FallbackDirection { get; }

        public static AimSettings Default => new(8f, 20f, 5f, 0.35f, Vector2.right);
    }
}
