using System;
using System.Collections.Generic;
using Game.Data.Progression;

namespace Game.Data.Save
{
    /// <summary>
    /// The 2026-10-07 catalog shortened some scales (single-step sticky toxin and
    /// spider element, two duration purchases for the element cannons). Paid ranks
    /// beyond the new limits are removed and the remaining ranks are renumbered, so
    /// a trimmed purchase is never re-read as an unpaid base upgrade.
    /// </summary>
    public sealed class SaveMigrationV8ToV9 : ISaveMigration
    {
        public int FromVersion => 8;
        public int ToVersion => 9;

        public void Migrate(SaveData data)
        {
            data.profile ??= SaveProfileData.CreateDefault();
            data.inventory ??= SaveInventoryData.CreateDefault();
            var progressList = data.inventory.weaponProgress;
            if (progressList != null)
            {
                for (var index = 0; index < progressList.Count; index++)
                {
                    var progress = progressList[index];
                    if (progress != null && WeaponProgressionCatalog.TryGet(progress.weaponId, out var definition))
                    {
                        TrimToCatalogLimits(progress, definition);
                    }
                }
            }

            data.schemaVersion = ToVersion;
            data.EnsureDefaults();
        }

        public static void TrimToCatalogLimits(SaveWeaponProgressData progress, WeaponProgressionDefinition definition)
        {
            if (progress == null || definition == null)
            {
                return;
            }

            var allowedSpecialSlots = 0;
            if (progress.specialUpgrades != null)
            {
                for (var index = 0; index < progress.specialUpgrades.Count; index++)
                {
                    var upgrade = progress.specialUpgrades[index];
                    if (upgrade == null)
                    {
                        continue;
                    }

                    var limit = definition.GetSpecialPurchaseLimit(upgrade.rank);
                    upgrade.purchaseCount = Math.Clamp(upgrade.purchaseCount, 0, limit);
                    allowedSpecialSlots += upgrade.purchaseCount;
                }
            }

            // Saves without recorded TTH choices are resolved by the catalog itself.
            if (progress.upgradeChoices == null || progress.upgradeChoices.Count == 0)
            {
                progress.rank = Math.Min(progress.rank, definition.MaxRank);
                return;
            }

            var recorded = new Dictionary<int, int>();
            for (var index = 0; index < progress.upgradeChoices.Count; index++)
            {
                var choice = progress.upgradeChoices[index];
                if (choice != null && choice.path != (int)WeaponUpgradePath.None && !recorded.ContainsKey(choice.rank))
                {
                    recorded[choice.rank] = choice.path;
                }
            }

            var kept = new List<int>();
            var pathCounts = new int[5];
            var specialSlots = 0;
            for (var rank = 2; rank <= progress.rank; rank++)
            {
                if (recorded.TryGetValue(rank, out var path))
                {
                    if (path < 1 || path > 4 || pathCounts[path] >= definition.GetBaseUpgradeLimit((WeaponUpgradePath)path))
                    {
                        continue;
                    }

                    pathCounts[path]++;
                    kept.Add(path);
                    continue;
                }

                if (specialSlots < allowedSpecialSlots)
                {
                    specialSlots++;
                    kept.Add((int)WeaponUpgradePath.None);
                }
            }

            progress.upgradeChoices.Clear();
            for (var index = 0; index < kept.Count; index++)
            {
                if (kept[index] != (int)WeaponUpgradePath.None)
                {
                    progress.upgradeChoices.Add(new SaveWeaponUpgradeChoiceData { rank = index + 2, path = kept[index] });
                }
            }

            progress.rank = 1 + kept.Count;
        }
    }
}
