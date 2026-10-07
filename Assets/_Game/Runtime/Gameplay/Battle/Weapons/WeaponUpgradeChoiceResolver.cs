using System;
using Game.Data.Progression;
using Game.Data.Save;
using UnityEngine;

namespace Game.Gameplay.Battle.Weapons
{
    /// <summary>
    /// Shared interpretation of the four free-choice weapon upgrades. UI and battle
    /// code use this resolver so a value shown in the workshop is the value used by
    /// the projectile simulation as well.
    /// </summary>
    public static class WeaponUpgradeChoiceResolver
    {
        public const float DrillLaserBaseRange = 3.25f;
        public const float RopeBaseLength = 8f;
        private const float DrillLaserRangePerUpgrade = 0.10f;
        private const int BaseAccuracy = 70;
        private const int AccuracyPerAimUpgrade = 4;
        private const int DamagePercentPerCoreUpgrade = 3;
        private const int AmmoPerLoadoutUpgrade = 1;
        private const float RadiusPercentPerUpgrade = 0.08f;
        private const float StandardRadiusPercentPerUpgrade = 0.10f;

        public static int Count(SaveWeaponProgressData progress, WeaponUpgradePath path, int throughRank = 60)
        {
            if (progress == null || !WeaponUpgradeEconomy.IsValidPath(path))
            {
                return 0;
            }

            return WeaponProgressionCatalog.TryGet(progress.weaponId, out var definition)
                ? definition.CountBaseUpgrades(progress, path, throughRank)
                : 0;
        }

        public static float ResolveValue(
            WeaponDefinition weapon,
            WeaponProgressionDefinition definition,
            SaveWeaponProgressData progress,
            WeaponUpgradePath path,
            int extraPurchases = 0)
        {
            if (weapon == null)
            {
                return 0f;
            }

            var count = Count(progress, path) + Math.Max(0, extraPurchases);
            return path switch
            {
                WeaponUpgradePath.Core => weapon.WeaponId is "weapon.grounder" or "weapon.force-field"
                    ? 2f + count
                    : weapon.WeaponId == "weapon.heal" ? ResolveRepairHealPercent(count)
                    : Mathf.RoundToInt(weapon.Damage *
                        (1f + ResolveCoreDamagePercent(weapon, count) / 100f)),
                WeaponUpgradePath.Aim when weapon.WeaponId == "weapon.rope" =>
                    ResolveRopeAimSectorDegrees(count),
                WeaponUpgradePath.Aim => BaseAccuracy + count * AccuracyPerAimUpgrade,
                WeaponUpgradePath.Loadout when weapon.WeaponId == "weapon.mine" => 4f,
                WeaponUpgradePath.Loadout => weapon.HasLimitedAmmo
                    ? weapon.AmmoCount + count * AmmoPerLoadoutUpgrade
                    : -1f,
                // For the hopping grenade the Radius path buys extra jumps, not a bigger blast.
                WeaponUpgradePath.Radius when weapon.WeaponId == "weapon.hopping-grenade" => 1f + Math.Clamp(count, 0, 4),
                WeaponUpgradePath.Radius when weapon.WeaponId == "weapon.bur" =>
                    ResolveDrillLaserRange(count),
                WeaponUpgradePath.Radius when weapon.WeaponId == "weapon.bur-rocket" =>
                    ResolveDrillRocketTerrainSeconds(count),
                WeaponUpgradePath.Radius when weapon.WeaponId == TitaniumBarrierRules.WeaponId =>
                    weapon.ImpactRadius * TitaniumBarrierRules.MaximumScale(
                        definition != null && progress?.rank >= 5
                        && WeaponEvolutionSelectionResolver.TryGetChoice(definition, progress, 5, out var option)
                        && option.Id == TitaniumBarrierRules.SizeFeatureId, count),
                WeaponUpgradePath.Radius when weapon.WeaponId == "weapon.rope" =>
                    ResolveRopeLength(count),
                WeaponUpgradePath.Radius when weapon.WeaponId == "weapon.boost" =>
                    ResolveBoostSeconds(count),
                WeaponUpgradePath.Radius when weapon.WeaponId is "weapon.bomber" or "weapon.ufo-carrier" =>
                    ResolveBomberSeconds(count),
                WeaponUpgradePath.Radius when weapon.WeaponId == "weapon.ion-shotgun" =>
                    weapon.ProjectileRadius * (1f + count * RadiusPercentPerUpgrade),
                // The repair jammer's two Radius purchases add a turn each: 2 -> 3 -> 4.
                WeaponUpgradePath.Radius when weapon.WeaponId == "weapon.repair-jammer" => 2f + Math.Clamp(count, 0, 2),
                WeaponUpgradePath.Radius when IsElementCannon(weapon.WeaponId) =>
                    ResolveElementCannonStatusTurns(count),
                // The laser fence's Radius lengthens the link between its hulls by 10% of 5.4 per purchase.
                WeaponUpgradePath.Radius when weapon.WeaponId == "weapon.laser-fence" =>
                    5.4f * (1f + Math.Clamp(count, 0, 4) * StandardRadiusPercentPerUpgrade),
                // The standard Radius step is +10% of the base (the shotgun pellet above has its own 8%).
                WeaponUpgradePath.Radius => weapon.ImpactRadius * (1f + count * StandardRadiusPercentPerUpgrade),
                _ => 0f
            };
        }

        public static float ResolveDrillLaserRange(int radiusUpgrades)
        {
            return DrillLaserBaseRange *
                (1f + Math.Min(Math.Max(radiusUpgrades, 0), 4) * DrillLaserRangePerUpgrade);
        }

        public static float ResolveDrillRocketTerrainSeconds(int radiusUpgrades)
        {
            return 1f + Math.Clamp(radiusUpgrades, 0, 4) * 0.10f;
        }

        public static float ResolveRepairHealPercent(int coreUpgrades)
        {
            return 25f * (1f + Math.Clamp(coreUpgrades, 0, 4) * 0.10f);
        }

        /// <summary>Rocket fuel burns for five seconds; every Radius purchase adds one.</summary>
        public static float ResolveBoostSeconds(int radiusUpgrades)
        {
            return 5f + Math.Clamp(radiusUpgrades, 0, 4);
        }

        /// <summary>The copter's ten-second window grows by one second per Radius purchase.</summary>
        public static float ResolveBomberSeconds(int radiusUpgrades)
        {
            return 10f + Math.Clamp(radiusUpgrades, 0, 4);
        }

        public static float ResolveRopeLength(int radiusUpgrades)
        {
            return RopeBaseLength * (1f + Math.Clamp(radiusUpgrades, 0, 4) * 0.10f);
        }

        public static float ResolveRopeAimSectorDegrees(int aimUpgrades)
        {
            return 45f + Math.Clamp(aimUpgrades, 0, 4) * 4f;
        }

        public static float ResolveAimPreviewMultiplier(int aimUpgrades)
        {
            return (BaseAccuracy + Math.Clamp(aimUpgrades, 0, 4) * AccuracyPerAimUpgrade) /
                (float)BaseAccuracy;
        }

        public static int ResolveDamagePercent(
            WeaponDefinition weapon,
            SaveWeaponProgressData progress,
            WeaponProgressionDefinition definition = null)
        {
            return ResolveCoreDamagePercent(weapon, Count(progress, WeaponUpgradePath.Core));
        }

        private static int ResolveCoreDamagePercent(WeaponDefinition weapon, int count)
        {
            if (weapon != null && (weapon.WeaponId == "weapon.impulse"
                || weapon.WeaponId == "weapon.granata"
                || weapon.WeaponId == "weapon.plasma-sword"
                || weapon.WeaponId == "weapon.ion-shotgun"
                || weapon.WeaponId == "weapon.c4" || weapon.WeaponId == "weapon.mine"
                || weapon.WeaponId == "weapon.titanium-disc"))
            {
                return Math.Clamp(count, 0, 4) switch
                {
                    1 => 50,
                    2 => 200,
                    3 => 400,
                    4 => 900,
                    _ => 0
                };
            }

            return count * DamagePercentPerCoreUpgrade;
        }

        public static int ResolveImpactRadiusPercent(
            WeaponDefinition weapon,
            SaveWeaponProgressData progress,
            WeaponProgressionDefinition definition = null)
        {
            if (weapon?.WeaponId is "weapon.bur" or "weapon.bur-rocket" or "weapon.titanium-barrier"
                or "weapon.hopping-grenade" or "weapon.boost" or "weapon.bomber" or "weapon.ufo-carrier"
                or "weapon.laser-fence" || IsElementCannon(weapon?.WeaponId))
            {
                return 0;
            }

            var percentPerUpgrade = weapon?.WeaponId == "weapon.ion-shotgun" ? 8 : 10;
            return Count(progress, WeaponUpgradePath.Radius) * percentPerUpgrade;
        }

        /// <summary>The four element cannons: their Radius purchases lengthen the status instead of the blast.</summary>
        public static bool IsElementCannon(string weaponId)
        {
            return weaponId is "weapon.emp" or "weapon.freeze" or "weapon.fire-gun" or "weapon.toxic";
        }

        /// <summary>Status turns of an element cannon: 2, +1 per Radius purchase (two purchases, so 4 at most).</summary>
        public static int ResolveElementCannonStatusTurns(int radiusPurchases)
        {
            return 2 + Math.Clamp(radiusPurchases, 0, 2);
        }

        public static bool TryGetChoice(SaveWeaponProgressData progress, int rank, out WeaponUpgradePath path)
        {
            if (progress?.upgradeChoices != null)
            {
                for (var index = 0; index < progress.upgradeChoices.Count; index++)
                {
                    var choice = progress.upgradeChoices[index];
                    if (choice != null && choice.rank == rank
                        && Enum.IsDefined(typeof(WeaponUpgradePath), choice.path)
                        && choice.path != (int)WeaponUpgradePath.None)
                    {
                        path = (WeaponUpgradePath)choice.path;
                        return true;
                    }
                }
            }

            path = WeaponUpgradePath.None;
            return false;
        }

    }
}
