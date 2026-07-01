using UnityEngine;

namespace Game.Gameplay.Battle.Domain
{
    public readonly struct AimSettings
    {
        public AimSettings(float minPower, float maxPower, float powerScale, float minInputDistance, Vector2 fallbackDirection)
        {
            MinPower = Mathf.Max(0f, minPower);
            MaxPower = Mathf.Max(MinPower, maxPower);
            PowerScale = Mathf.Max(0.01f, powerScale);
            MinInputDistance = Mathf.Max(0f, minInputDistance);
            FallbackDirection = fallbackDirection.sqrMagnitude > 0f ? fallbackDirection.normalized : Vector2.right;
        }

        public float MinPower { get; }
        public float MaxPower { get; }
        public float PowerScale { get; }
        public float MinInputDistance { get; }
        public Vector2 FallbackDirection { get; }

        public static AimSettings Default => new(8f, 20f, 2.15f, 0.2f, Vector2.right);
    }
}
