using System;
using System.Collections.Generic;
using Game.Data.Progression;
using Game.Data.Save;

namespace Game.Economy
{
    /// <summary>
    /// Owns the monotonic account-strength value used by future PvP matching.
    /// </summary>
    public sealed class HistoricalPowerService
    {
        private readonly SaveData _saveData;

        public HistoricalPowerService(SaveData saveData)
        {
            _saveData = saveData ?? throw new ArgumentNullException(nameof(saveData));
            _saveData.EnsureDefaults();
        }

        public long HistoricalPower => _saveData.profile.historicalPower;

        /// <summary>Reads the current score and records it if it exceeds the historical high-water mark.</summary>
        public long Refresh()
        {
            var current = CalculateCurrentAccountPower(_saveData);
            if (current > _saveData.profile.historicalPower)
            {
                _saveData.profile.historicalPower = current;
            }

            return _saveData.profile.historicalPower;
        }

        public static long CalculateCurrentAccountPower(SaveData data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            data.EnsureDefaults();
            // Use real saved experience, never the QA ForceMaxLevel override.
            var level = 1;
            var safeExperience = Math.Max(0, data.profile.commanderExperience);
            var thresholds = CommanderProgressionTable.Thresholds;
            for (var thresholdIndex = 0; thresholdIndex < thresholds.Count; thresholdIndex++)
            {
                if (safeExperience < thresholds[thresholdIndex])
                {
                    break;
                }

                level = thresholdIndex + 1;
            }
            var ownedIds = new HashSet<string>(StringComparer.Ordinal);
            for (var index = 0; index < data.inventory.unlockedWeaponIds.Count; index++)
            {
                var id = data.inventory.unlockedWeaponIds[index];
                if (!string.IsNullOrWhiteSpace(id))
                {
                    ownedIds.Add(id);
                }
            }

            var rankSum = 0L;
            var evolutionChoices = 0L;
            for (var index = 0; index < data.inventory.weaponProgress.Count; index++)
            {
                var progress = data.inventory.weaponProgress[index];
                if (progress == null)
                {
                    continue;
                }

                rankSum += Math.Max(0, progress.rank - 1);
                if (progress.branch != 0)
                {
                    evolutionChoices++;
                }
            }

            return 100L * level
                + 250L * ownedIds.Count
                + 75L * rankSum
                + 150L * evolutionChoices
                + data.inventory.droneUpgradePower;
        }
    }
}
