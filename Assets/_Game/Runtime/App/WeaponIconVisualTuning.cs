using UnityEngine;

namespace Game.App
{
    /// <summary>
    /// Keeps weapon artwork at a consistent perceived size across the compact UI
    /// surfaces. Legacy art retains its established framing; newer art uses the
    /// smaller default so different transparent margins do not overfill a slot.
    /// </summary>
    public static class WeaponIconVisualTuning
    {
        private const float NewArtScale = 0.84f;

        public static void ApplyArsenal(RectTransform rect, string weaponId, Vector2 basePosition)
        {
            Apply(rect, ResolveArsenal(weaponId), basePosition);
        }

        public static void ApplyShopSlot(RectTransform rect, string weaponId, Vector2 basePosition)
        {
            Apply(rect, ResolveShopSlot(weaponId), basePosition);
        }

        public static void ApplyShopPreview(RectTransform rect, string weaponId, Vector2 basePosition)
        {
            Apply(rect, ResolveShopPreview(weaponId), basePosition);
        }

        public static void ApplyWorkshopCarousel(RectTransform rect, string weaponId, Vector2 basePosition)
        {
            Apply(rect, ResolveWorkshopCarousel(weaponId), basePosition);
        }

        public static void ApplyWorkshopCurrentArt(RectTransform rect, string weaponId, int rank)
        {
            var tuning = ResolveWorkshopCurrentArt(weaponId, rank);
            rect.localScale = new Vector3(tuning.Scale.x, tuning.Scale.y, 1f);
            rect.anchoredPosition = tuning.Offset;
            rect.localRotation = Quaternion.Euler(0f, 0f, tuning.RotationZ);
        }

        public static void ApplyWorkshopSelectedUpgradeArt(RectTransform rect, string weaponId, bool isSpecial, Vector2 basePosition)
        {
            Apply(rect, isSpecial ? ResolveWorkshopSelectedUpgradeArt(weaponId) : VisualTuning.Default, basePosition);
        }

        public static void ApplyDetailArtRotation(RectTransform rect, string weaponId)
        {
            rect.localRotation = Quaternion.Euler(0f, 0f, weaponId == "weapon.sticky-grenade" ? 15f : 0f);
        }

        private static void Apply(RectTransform rect, VisualTuning tuning, Vector2 basePosition)
        {
            rect.localScale = new Vector3(tuning.Scale.x, tuning.Scale.y, 1f);
            rect.anchoredPosition = basePosition + tuning.Offset;
            rect.localRotation = Quaternion.Euler(0f, 0f, tuning.RotationZ);
        }

        private static VisualTuning ResolveArsenal(string weaponId)
        {
            var defaultOffset = new Vector2(0f, 8f);
            return weaponId switch
            {
                "weapon.impulse" => new VisualTuning(1.26f, defaultOffset),
                "weapon.blaster-caset" => new VisualTuning(1.06f, defaultOffset),
                "weapon.bur" => new VisualTuning(0.82f, defaultOffset),
                "weapon.mortira" => new VisualTuning(1f, defaultOffset),
                "weapon.granata" => new VisualTuning(0.70f, new Vector2(0f, 12f)),
                "weapon.teleport" => new VisualTuning(0.53f, new Vector2(0f, 14f)),
                "weapon.c4" => new VisualTuning(0.96f, new Vector2(0f, 4f)),
                "weapon.mine" => new VisualTuning(0.79f, new Vector2(0f, 4f)),
                "weapon.bur-rocket" => new VisualTuning(1.03f, defaultOffset),
                "weapon.heal" => new VisualTuning(0.94f, new Vector2(0f, 4f)),
                "weapon.emp" => new VisualTuning(new Vector2(0.94f, 0.82f), defaultOffset),
                "weapon.freeze" => new VisualTuning(1f, defaultOffset),
                "weapon.fire-gun" => new VisualTuning(0.98f, defaultOffset),
                "weapon.fpv-drone" => new VisualTuning(0.82f, new Vector2(0f, 6f)),
                "weapon.sticky-grenade" => new VisualTuning(NewArtScale, defaultOffset, 15f),
                _ => new VisualTuning(NewArtScale, defaultOffset)
            };
        }

        private static VisualTuning ResolveShopSlot(string weaponId)
        {
            var normalOffset = new Vector2(0f, 12f);
            return weaponId switch
            {
                "weapon.impulse" => new VisualTuning(1.10f, normalOffset),
                "weapon.blaster-caset" => new VisualTuning(0.84f, normalOffset),
                "weapon.bur" => new VisualTuning(0.78f, normalOffset),
                "weapon.mortira" => new VisualTuning(1f, normalOffset),
                "weapon.granata" => new VisualTuning(0.68f, normalOffset),
                "weapon.teleport" => new VisualTuning(0.48f, normalOffset),
                "weapon.mine" => new VisualTuning(0.78f, normalOffset),
                "weapon.heal" => new VisualTuning(0.82f, normalOffset),
                "weapon.emp" => new VisualTuning(new Vector2(0.82f, 0.72f), normalOffset),
                "weapon.fire-gun" => new VisualTuning(0.82f, normalOffset),
                "weapon.fpv-drone" => new VisualTuning(0.72f, normalOffset),
                "weapon.sticky-grenade" => new VisualTuning(NewArtScale, normalOffset, 15f),
                _ => new VisualTuning(NewArtScale, normalOffset)
            };
        }

        private static VisualTuning ResolveShopPreview(string weaponId)
        {
            var normalOffset = new Vector2(50f, 62f);
            return weaponId switch
            {
                "weapon.impulse" => new VisualTuning(1f, normalOffset),
                "weapon.blaster-caset" => new VisualTuning(1f, normalOffset),
                "weapon.bur" => new VisualTuning(0.82f, normalOffset),
                "weapon.mortira" => new VisualTuning(1.10f, normalOffset),
                "weapon.granata" => new VisualTuning(0.35f, normalOffset),
                "weapon.teleport" => new VisualTuning(0.46f, normalOffset),
                "weapon.mine" => new VisualTuning(0.78f, normalOffset),
                "weapon.heal" => new VisualTuning(1f, normalOffset),
                "weapon.emp" => new VisualTuning(new Vector2(0.86f, 0.72f), normalOffset),
                "weapon.fire-gun" => new VisualTuning(1f, normalOffset),
                "weapon.fpv-drone" => new VisualTuning(1f, normalOffset),
                "weapon.sticky-grenade" => new VisualTuning(NewArtScale, normalOffset, 15f),
                _ => new VisualTuning(NewArtScale, normalOffset)
            };
        }

        private static VisualTuning ResolveWorkshopCarousel(string weaponId)
        {
            return ResolveShopSlot(weaponId);
        }

        private static VisualTuning ResolveWorkshopCurrentArt(string weaponId, int rank)
        {
            var defaultTuning = new VisualTuning(1.2f, new Vector2(-50f, 200f));
            return weaponId switch
            {
                "weapon.mine" => new VisualTuning(1f, new Vector2(-50f, 200f)),
                "weapon.sticky-grenade" => new VisualTuning(1.2f, new Vector2(-50f, 200f), 15f),
                "weapon.rail-rifle" => new VisualTuning(new Vector2(1.5f, 1.8f), new Vector2(-50f, 200f)),
                "weapon.cryo-grenade" or "weapon.hopping-grenade" or "weapon.incendiary-grenade"
                    or "weapon.repair-jammer" or "weapon.guardian-drone" => new VisualTuning(1.3f, new Vector2(-50f, 230f)),
                "weapon.electric-harpoon" => new VisualTuning(1.5f, new Vector2(50f, 200f), -15f),
                "weapon.swap-jump" or "weapon.grounder" or "weapon.boost" => new VisualTuning(1.2f, new Vector2(-50f, 230f)),
                "weapon.force-field" => new VisualTuning(1.2f, new Vector2(-50f, 240f)),
                "weapon.sky-hook" => new VisualTuning(1.2f, new Vector2(-50f, 200f), -90f),
                "weapon.spider-mine" => new VisualTuning(1.2f, new Vector2(-50f, 200f)),
                "weapon.virus" => new VisualTuning(1.2f, new Vector2(-50f, 230f)),
                "weapon.ufo-carrier" => new VisualTuning(1.2f, new Vector2(-50f, 230f)),
                "weapon.self-destruct" or "weapon.nuclear-charge" or "weapon.laser-fence" => new VisualTuning(1.2f, new Vector2(-50f, 200f)),
                "weapon.freeze" when rank >= 5 => new VisualTuning(1.2f, new Vector2(-50f, 200f)),
                "weapon.fire-gun" => new VisualTuning(1.2f, new Vector2(-50f, 220f)),
                _ => defaultTuning
            };
        }

        private static VisualTuning ResolveWorkshopSelectedUpgradeArt(string weaponId)
        {
            return weaponId switch
            {
                "weapon.rail-rifle" => new VisualTuning(new Vector2(1.4f, 1.5f), Vector2.zero),
                "weapon.spider-mine" or "weapon.freeze" => new VisualTuning(1.2f, Vector2.zero),
                "weapon.toxic" => new VisualTuning(1.2f, new Vector2(0f, -20f)),
                "weapon.sticky-grenade" => new VisualTuning(1f, Vector2.zero, 15f),
                _ => VisualTuning.Default
            };
        }

        private readonly struct VisualTuning
        {
            public static readonly VisualTuning Default = new(Vector2.one, Vector2.zero);

            public VisualTuning(float scale, Vector2 offset, float rotationZ = 0f)
                : this(new Vector2(scale, scale), offset, rotationZ)
            {
            }

            public VisualTuning(Vector2 scale, Vector2 offset, float rotationZ = 0f)
            {
                Scale = scale;
                Offset = offset;
                RotationZ = rotationZ;
            }

            public Vector2 Scale { get; }
            public Vector2 Offset { get; }
            public float RotationZ { get; }
        }
    }
}
