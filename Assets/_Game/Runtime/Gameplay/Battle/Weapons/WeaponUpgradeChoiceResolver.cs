using System;
using Game.Data.Progression;
using Game.Data.Save;

namespace Game.Gameplay.Battle.Weapons
{
    /// <summary>
    /// Shared interpretation of the four free-choice weapon upgrades. UI and battle
    /// code use this resolver so a value shown in the workshop is the value used by
    /// the projectile simulation as well.
    /// </summary>
    public static class WeaponUpgradeChoiceResolver
    {
        private const int BaseAccuracy = 70;
        private const int AccuracyPerAimUpgrade = 4;
        private const int DamagePercentPerCoreUpgrade = 3;
        private const int AmmoPerLoadoutUpgrade = 1;
        private const float RadiusPercentPerUpgrade = 0.08f;

        public static int Count(SaveWeaponProgressData progress, WeaponUpgradePath path, int throughRank = 60)
        {
            if (progress == null || !WeaponUpgradeEconomy.IsValidPath(path))
            {
                return 0;
            }

            var count = 0;
            for (var rank = 2; rank <= Math.Min(progress.rank, throughRank); rank++)
            {
                if (TryGetChoice(progress, rank, out var selectedPath))
                {
                    count += selectedPath == path ? 1 : 0;
                    continue;
                }

                // Compatibility for saves created before free TTH choices existed.
                // A legacy node is only used when no explicit choice was saved for
                // that rank, preventing double counting after the first new purchase.
                if (LegacyPath(progress, rank) == path)
                {
                    count++;
                }
            }

            return count;
        }

        public static float ResolveValue(
            WeaponDefinition weapon,
            WeaponProgressionDefinition definition,
            SaveWeaponProgressData progress,
            WeaponUpgradePath path,
            int extraPurchases = 0)
        {
            if (weapon == null || progress == null)
            {
                return 0f;
            }

            var count = Count(progress, path) + Math.Max(0, extraPurchases);
            return path switch
            {
                WeaponUpgradePath.Core => weapon.Damage * (1f + count * DamagePercentPerCoreUpgrade / 100f),
                WeaponUpgradePath.Aim => BaseAccuracy + count * AccuracyPerAimUpgrade,
                WeaponUpgradePath.Loadout => weapon.HasLimitedAmmo
                    ? weapon.AmmoCount + count * AmmoPerLoadoutUpgrade
                    : -1f,
                WeaponUpgradePath.Radius => weapon.ImpactRadius * (1f + count * RadiusPercentPerUpgrade),
                _ => 0f
            };
        }

        public static int ResolveDamagePercent(
            WeaponDefinition weapon,
            SaveWeaponProgressData progress,
            WeaponProgressionDefinition definition = null)
        {
            return Count(progress, WeaponUpgradePath.Core) * DamagePercentPerCoreUpgrade;
        }

        public static int ResolveImpactRadiusPercent(
            WeaponDefinition weapon,
            SaveWeaponProgressData progress,
            WeaponProgressionDefinition definition = null)
        {
            return (int)Math.Round(Count(progress, WeaponUpgradePath.Radius) * RadiusPercentPerUpgrade * 100f);
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

        private static WeaponUpgradePath LegacyPath(SaveWeaponProgressData progress, int rank)
        {
            if (progress == null || string.IsNullOrWhiteSpace(progress.weaponId)
                || !WeaponProgressionCatalog.TryGet(progress.weaponId, out var definition)
                || definition == null
                || !definition.TryGetNode(rank, (WeaponUpgradeBranch)progress.branch, out var node)
                || node.IsEvolution)
            {
                return WeaponUpgradePath.None;
            }

            return node.Stat switch
            {
                WeaponUpgradeStat.Damage => WeaponUpgradePath.Core,
                WeaponUpgradeStat.Accuracy => WeaponUpgradePath.Aim,
                WeaponUpgradeStat.Range => WeaponUpgradePath.Aim,
                WeaponUpgradeStat.Ammo => WeaponUpgradePath.Loadout,
                WeaponUpgradeStat.ProjectileCount => WeaponUpgradePath.Loadout,
                WeaponUpgradeStat.ImpactRadius => WeaponUpgradePath.Radius,
                _ => WeaponUpgradePath.None
            };
        }
    }
}
