using System;
using System.Collections.Generic;
using Game.Core.Localization;

namespace Game.Data.Save
{
    [Serializable]
    public sealed class SaveData
    {
        public const int CurrentSchemaVersion = 8;

        public int schemaVersion;
        public long lastWriteUtcTicks;
        public SaveProfileData profile;
        public SaveCampaignData campaign;
        public SaveInventoryData inventory;
        public SaveEconomyData economy;
        public SaveSettingsData settings;
        public SaveTutorialData tutorial;
        public SaveLedgerData ledger;
        public SaveActivityProgressData activity;

        public static SaveData CreateNew()
        {
            var data = new SaveData();
            data.EnsureDefaults();
            return data;
        }

        public void EnsureDefaults()
        {
            if (schemaVersion <= 0)
            {
                schemaVersion = CurrentSchemaVersion;
            }

            profile ??= SaveProfileData.CreateDefault();
            campaign ??= SaveCampaignData.CreateDefault();
            inventory ??= SaveInventoryData.CreateDefault();
            economy ??= SaveEconomyData.CreateDefault();
            settings ??= SaveSettingsData.CreateDefault();
            tutorial ??= SaveTutorialData.CreateDefault();
            ledger ??= SaveLedgerData.CreateDefault();
            activity ??= SaveActivityProgressData.CreateDefault();

            profile.EnsureDefaults();
            campaign.EnsureDefaults();
            inventory.EnsureDefaults();
            economy.EnsureDefaults();
            settings.EnsureDefaults();
            tutorial.EnsureDefaults();
            ledger.EnsureDefaults();
            activity.EnsureDefaults();
        }

        public void ResetProgression()
        {
            profile ??= SaveProfileData.CreateDefault();
            profile.commanderExperience = 0;
            campaign = SaveCampaignData.CreateDefault();
            inventory = SaveInventoryData.CreateDefault();
            economy = SaveEconomyData.CreateDefault();
            tutorial = SaveTutorialData.CreateDefault();
            ledger = SaveLedgerData.CreateDefault();
            activity = SaveActivityProgressData.CreateDefault();
            EnsureDefaults();
        }
    }

    [Serializable]
    public sealed class SaveActivityProgressData
    {
        public const int DailyLoginStreakLength = 7;

        public long dailyPeriodStartUtcTicks;
        public long weeklyPeriodStartUtcTicks;
        public long lastLoginDailyPeriodStartUtcTicks;
        public long dailyLoginRewardClaimedPeriodStartUtcTicks;
        public int dailyLoginStreakDay;
        public SaveActivityCounterData daily;
        public SaveActivityCounterData weekly;
        public List<string> dailyClaimedQuestIds;
        public List<string> weeklyClaimedQuestIds;

        public static SaveActivityProgressData CreateDefault()
        {
            var data = new SaveActivityProgressData();
            data.EnsureDefaults();
            return data;
        }

        public void EnsureDefaults()
        {
            daily ??= SaveActivityCounterData.CreateDefault();
            weekly ??= SaveActivityCounterData.CreateDefault();
            dailyClaimedQuestIds ??= new List<string>();
            weeklyClaimedQuestIds ??= new List<string>();
            daily.EnsureDefaults();
            weekly.EnsureDefaults();
            dailyClaimedQuestIds.RemoveAll(string.IsNullOrWhiteSpace);
            weeklyClaimedQuestIds.RemoveAll(string.IsNullOrWhiteSpace);
            if (dailyLoginStreakDay < 1 || dailyLoginStreakDay > DailyLoginStreakLength)
            {
                dailyLoginStreakDay = 1;
            }
        }
    }

    [Serializable]
    public sealed class SaveActivityCounterData
    {
        public int logins;
        public int battlesPlayed;
        public int battlesWon;
        public int arenaBattlesPlayed;
        public int storyBattlesPlayed;
        public int storyMissionsCompleted;
        public int damageDealt;
        public int terrainCellsDestroyed;
        public int resourcesCollected;
        public int rewardedAdsWatched;
        public int shotsFired;
        public int rocketShotsFired;
        public int grenadeShotsFired;
        public int walkDistanceCentiUnits;
        public int jumpDistanceCentiUnits;

        public static SaveActivityCounterData CreateDefault()
        {
            var data = new SaveActivityCounterData();
            data.EnsureDefaults();
            return data;
        }

        public void EnsureDefaults()
        {
            logins = Clamp(logins);
            battlesPlayed = Clamp(battlesPlayed);
            battlesWon = Clamp(battlesWon);
            arenaBattlesPlayed = Clamp(arenaBattlesPlayed);
            storyBattlesPlayed = Clamp(storyBattlesPlayed);
            storyMissionsCompleted = Clamp(storyMissionsCompleted);
            damageDealt = Clamp(damageDealt);
            terrainCellsDestroyed = Clamp(terrainCellsDestroyed);
            resourcesCollected = Clamp(resourcesCollected);
            rewardedAdsWatched = Clamp(rewardedAdsWatched);
            shotsFired = Clamp(shotsFired);
            rocketShotsFired = Clamp(rocketShotsFired);
            grenadeShotsFired = Clamp(grenadeShotsFired);
            walkDistanceCentiUnits = Clamp(walkDistanceCentiUnits);
            jumpDistanceCentiUnits = Clamp(jumpDistanceCentiUnits);
        }

        private static int Clamp(int value) => value < 0 ? 0 : value;
    }

    [Serializable]
    public sealed class SaveProfileData
    {
        public string profileId;
        public string playerName;
        public long createdUtcTicks;
        public bool introCompleted;
        public int commanderExperience;
        /// <summary>Highest account-strength score ever observed for PvP matching.</summary>
        public long historicalPower;

        public static SaveProfileData CreateDefault()
        {
            var data = new SaveProfileData();
            data.EnsureDefaults();
            return data;
        }

        public void EnsureDefaults()
        {
            if (string.IsNullOrWhiteSpace(profileId))
            {
                profileId = Guid.NewGuid().ToString("N");
            }

            if (string.IsNullOrWhiteSpace(playerName))
            {
                playerName = GameText.DefaultPlayerName();
            }

            if (createdUtcTicks <= 0)
            {
                createdUtcTicks = DateTime.UtcNow.Ticks;
            }

            if (commanderExperience < 0)
            {
                commanderExperience = 0;
            }

            if (historicalPower < 0)
            {
                historicalPower = 0;
            }
        }
    }

    [Serializable]
    public sealed class SaveCampaignData
    {
        public int highestUnlockedMissionIndex;
        public string lastPlayedPlanetChapterId;
        public List<string> completedMissionIds;

        public static SaveCampaignData CreateDefault()
        {
            var data = new SaveCampaignData();
            data.EnsureDefaults();
            return data;
        }

        public void EnsureDefaults()
        {
            if (highestUnlockedMissionIndex < 0)
            {
                highestUnlockedMissionIndex = 0;
            }

            completedMissionIds ??= new List<string>();
            lastPlayedPlanetChapterId ??= string.Empty;
        }
    }

    [Serializable]
    public sealed class SaveInventoryData
    {
        public List<string> unlockedWeaponIds;
        public List<string> selectedLoadoutWeaponIds;
        public List<SaveWeaponProgressData> weaponProgress;
        /// <summary>Non-cosmetic drone upgrades that contribute to account strength.</summary>
        public int droneUpgradePower;

        public static SaveInventoryData CreateDefault()
        {
            var data = new SaveInventoryData();
            data.EnsureDefaults();
            return data;
        }

        public void EnsureDefaults()
        {
            unlockedWeaponIds ??= new List<string>();
            selectedLoadoutWeaponIds ??= new List<string>();
            weaponProgress ??= new List<SaveWeaponProgressData>();
            if (droneUpgradePower < 0)
            {
                droneUpgradePower = 0;
            }

            ReplaceLegacyWeaponIds(unlockedWeaponIds);
            ReplaceLegacyWeaponIds(selectedLoadoutWeaponIds);
            RemoveDeprecatedWeaponIds(unlockedWeaponIds);
            RemoveDeprecatedWeaponIds(selectedLoadoutWeaponIds);

            if (unlockedWeaponIds.Count == 0)
            {
                unlockedWeaponIds.Add("weapon.rope");
                unlockedWeaponIds.Add("weapon.impulse");
                unlockedWeaponIds.Add("weapon.blaster-caset");
            }

            if (!unlockedWeaponIds.Contains("weapon.rope"))
            {
                unlockedWeaponIds.Add("weapon.rope");
            }

            if (selectedLoadoutWeaponIds.Count == 0)
            {
                selectedLoadoutWeaponIds.Add("weapon.rope");
                selectedLoadoutWeaponIds.Add("weapon.impulse");
                selectedLoadoutWeaponIds.Add("weapon.blaster-caset");
            }

            if (!selectedLoadoutWeaponIds.Contains("weapon.rope"))
            {
                selectedLoadoutWeaponIds.Insert(0, "weapon.rope");
            }

            weaponProgress.RemoveAll(item => item == null || string.IsNullOrWhiteSpace(item.weaponId));
            for (var index = 0; index < weaponProgress.Count; index++)
            {
                weaponProgress[index].EnsureDefaults();
            }

            for (var index = 0; index < unlockedWeaponIds.Count; index++)
            {
                GetOrCreateWeaponProgress(unlockedWeaponIds[index]);
            }
        }

        public SaveWeaponProgressData GetOrCreateWeaponProgress(string weaponId)
        {
            if (string.IsNullOrWhiteSpace(weaponId))
            {
                return null;
            }

            weaponProgress ??= new List<SaveWeaponProgressData>();
            for (var index = 0; index < weaponProgress.Count; index++)
            {
                if (weaponProgress[index].weaponId == weaponId)
                {
                    weaponProgress[index].EnsureDefaults();
                    return weaponProgress[index];
                }
            }

            var progress = new SaveWeaponProgressData { weaponId = weaponId, rank = 1 };
            progress.EnsureDefaults();
            weaponProgress.Add(progress);
            return progress;
        }

        private static void ReplaceLegacyWeaponIds(List<string> weaponIds)
        {
            if (weaponIds == null) return;

            for (var index = weaponIds.Count - 1; index >= 0; index--)
            {
                weaponIds[index] = weaponIds[index] switch
                {
                    "weapon.steady-orb" => "weapon.impulse",
                    "weapon.pulse-shell" => "weapon.mortira",
                    "weapon.close-pop" => "weapon.blaster-caset",
                    "weapon.long-lance" => "weapon.bur",
                    "weapon.scatter-arc" => "weapon.granata",
                    "weapon.rampart-kit" => "weapon.bur",
                    "weapon.guard-pulse" => "weapon.heal",
                    "weapon.relay-step" => "weapon.teleport",
                    "weapon.fpv" => "weapon.fpv-drone",
                    _ => weaponIds[index]
                };
            }

            var unique = new HashSet<string>(weaponIds);
            weaponIds.Clear();
            weaponIds.AddRange(unique);
        }

        private static void RemoveDeprecatedWeaponIds(List<string> weaponIds)
        {
            weaponIds?.RemoveAll(id => id == "weapon.launch-platform"
                || id == "weapon.rocket-launch-platform"
                || id == "weapon.bonus-box");
        }
    }

    [Serializable]
    public sealed class SaveWeaponProgressData
    {
        public string weaponId;
        public int rank;
        public int branch;
        public int blueprints;
        /// <summary>
        /// One player-selected TTH for each purchased rank. Older saves may leave
        /// this empty; the progression resolver then derives a compatible legacy
        /// view from the old catalog nodes until the next purchase is made.
        /// </summary>
        public List<SaveWeaponUpgradeChoiceData> upgradeChoices;
        /// <summary>Paid evolution selected independently at ranks 5 and 10.</summary>
        public List<SaveWeaponEvolutionChoiceData> evolutionChoices;
        /// <summary>How many paid steps each selected evolution has received.</summary>
        public List<SaveWeaponSpecialUpgradeData> specialUpgrades;
        /// <summary>Increments on reset so a repurchased rank gets a new ledger id.</summary>
        public int resetCycle;

        public void EnsureDefaults()
        {
            weaponId ??= string.Empty;
            rank = rank < 1 ? 1 : rank > 60 ? 60 : rank;
            branch = branch < 0 ? 0 : branch > 2 ? 2 : branch;
            if (rank < 5)
            {
                branch = 0;
            }

            if (blueprints < 0)
            {
                blueprints = 0;
            }

            upgradeChoices ??= new List<SaveWeaponUpgradeChoiceData>();
            upgradeChoices.RemoveAll(choice => choice == null
                || choice.rank < 2
                || choice.rank > 60
                || choice.path < 1
                || choice.path > 4);

            evolutionChoices ??= new List<SaveWeaponEvolutionChoiceData>();
            evolutionChoices.RemoveAll(choice => choice == null
                || (choice.rank != 5 && choice.rank != 10)
                || string.IsNullOrWhiteSpace(choice.optionId));

            specialUpgrades ??= new List<SaveWeaponSpecialUpgradeData>();
            specialUpgrades.RemoveAll(upgrade => upgrade == null
                || (upgrade.rank != 5 && upgrade.rank != 10)
                || upgrade.purchaseCount < 1);
            for (var index = 0; index < specialUpgrades.Count; index++)
            {
                specialUpgrades[index].purchaseCount = Math.Min(4, specialUpgrades[index].purchaseCount);
            }

            if (resetCycle < 0)
            {
                resetCycle = 0;
            }
        }
    }

    [Serializable]
    public sealed class SaveWeaponUpgradeChoiceData
    {
        public int rank;
        public int path;
    }

    [Serializable]
    public sealed class SaveWeaponEvolutionChoiceData
    {
        public int rank;
        public string optionId;
    }

    [Serializable]
    public sealed class SaveWeaponSpecialUpgradeData
    {
        public int rank;
        public int purchaseCount;
    }

    [Serializable]
    public sealed class SaveEconomyData
    {
        public int softCurrency;
        public int premiumCurrency;

        public static SaveEconomyData CreateDefault()
        {
            var data = new SaveEconomyData();
            data.EnsureDefaults();
            return data;
        }

        public void EnsureDefaults()
        {
            if (softCurrency < 0)
            {
                softCurrency = 0;
            }

            if (premiumCurrency < 0)
            {
                premiumCurrency = 0;
            }
        }
    }

    [Serializable]
    public sealed class SaveSettingsData
    {
        public string languageCode;
        public bool hasSelectedLanguage;
        public bool hasVolumeSettings;
        public float musicVolume;
        public float sfxVolume;
        /// <summary>
        /// QA/debug toggle: max commander level and unlimited currency/blueprints,
        /// without mutating the player's real saved progress (see
        /// CommanderProgressionTable.ForceMaxLevel and EconomyService.AdminModeEnabled).
        /// </summary>
        public bool adminModeEnabled;

        public static SaveSettingsData CreateDefault()
        {
            var data = new SaveSettingsData
            {
                hasVolumeSettings = true,
                musicVolume = 0.8f,
                sfxVolume = 1f
            };
            data.EnsureDefaults();
            return data;
        }

        public void EnsureDefaults()
        {
            if (string.IsNullOrWhiteSpace(languageCode))
            {
                languageCode = "uk";
            }

            if (!hasVolumeSettings)
            {
                musicVolume = 0.8f;
                sfxVolume = 1f;
                hasVolumeSettings = true;
                return;
            }

            musicVolume = Clamp01(musicVolume);
            sfxVolume = Clamp01(sfxVolume);
        }

        private static float Clamp01(float value)
        {
            if (float.IsNaN(value) || value < 0f)
            {
                return 0f;
            }

            return value > 1f ? 1f : value;
        }
    }

    [Serializable]
    public sealed class SaveTutorialData
    {
        public List<string> completedStepIds;

        public static SaveTutorialData CreateDefault()
        {
            var data = new SaveTutorialData();
            data.EnsureDefaults();
            return data;
        }

        public void EnsureDefaults()
        {
            completedStepIds ??= new List<string>();
        }
    }

    [Serializable]
    public sealed class SaveLedgerEntry
    {
        public string transactionId;
        public string sourceType;
        public string sourceId;
        public int softCurrencyDelta;
        public int premiumCurrencyDelta;
        public int commanderExperienceDelta;
        public string blueprintWeaponId;
        public int blueprintDelta;
        public long createdUtcTicks;
        public bool pendingSync;
    }

    [Serializable]
    public sealed class SaveLedgerData
    {
        public const int MaxEntries = 200;

        public List<SaveLedgerEntry> entries;

        public static SaveLedgerData CreateDefault()
        {
            var data = new SaveLedgerData();
            data.EnsureDefaults();
            return data;
        }

        public void EnsureDefaults()
        {
            entries ??= new List<SaveLedgerEntry>();
            entries.RemoveAll(entry => entry == null || string.IsNullOrWhiteSpace(entry.transactionId));
        }

        public bool ContainsTransaction(string transactionId)
        {
            if (string.IsNullOrWhiteSpace(transactionId))
            {
                return false;
            }

            for (var index = entries.Count - 1; index >= 0; index--)
            {
                if (entries[index].transactionId == transactionId)
                {
                    return true;
                }
            }

            return false;
        }

        public void Append(SaveLedgerEntry entry)
        {
            if (entry == null || string.IsNullOrWhiteSpace(entry.transactionId))
            {
                return;
            }

            entries.Add(entry);

            // Ковзне вікно ідемпотентності: сховище сейву не повинно рости
            // безмежно; синхронізовані записи найстаріші й видаляються першими.
            var overflow = entries.Count - MaxEntries;
            for (var index = 0; overflow > 0 && index < entries.Count; )
            {
                if (!entries[index].pendingSync)
                {
                    entries.RemoveAt(index);
                    overflow--;
                }
                else
                {
                    index++;
                }
            }
        }
    }
}
