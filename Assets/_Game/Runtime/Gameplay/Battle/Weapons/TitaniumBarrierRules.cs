using UnityEngine;

namespace Game.Gameplay.Battle.Weapons
{
    /// <summary>Physical placement scale, independent of camera zoom and launch power.</summary>
    public static class TitaniumBarrierRules
    {
        public const string WeaponId = "weapon.titanium-barrier";
        public const string SizeFeatureId = "size-unlock";
        public const float MinimumScale = 0.25f;
        // 120 HUD units at 0.8 icon scale, in the authored 1080p camera frame.
        public const float ReferenceIconWidth = 96f;
        public const float ReferenceViewportHeight = 1080f;
        // Owner decision 2026-10-08: the placed beam is twice the size of the button icon.
        public const float BaseSizeMultiplier = 2f;
        public const float BaseWidth = BaseSizeMultiplier * 2f * 6.2f * ReferenceIconWidth / ReferenceViewportHeight;

        public static float MaximumScale(bool sizeUnlocked, int radiusPurchases)
        {
            return sizeUnlocked ? 1.1f + Mathf.Clamp(radiusPurchases, 0, 4) * 0.1f : 1f;
        }

        public static float ClampScale(float requested, bool sizeUnlocked, int radiusPurchases)
        {
            return float.IsNaN(requested) || float.IsInfinity(requested)
                ? 1f : Mathf.Clamp(requested, MinimumScale, MaximumScale(sizeUnlocked, radiusPurchases));
        }

        public static Vector2 BaseSize(float referenceCameraSize, float textureAspect)
        {
            var width = BaseSizeMultiplier * 2f * referenceCameraSize * ReferenceIconWidth / ReferenceViewportHeight;
            return new Vector2(width, width / Mathf.Max(0.001f, textureAspect));
        }
    }
}
