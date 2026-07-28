using System;
using System.Collections.Generic;

namespace Game.Data.Save
{
    [Serializable]
    public sealed class SaveData
    {
        public const int CurrentSchemaVersion = 4;

        public int schemaVersion;
        public long lastWriteUtcTicks;
        public SaveProfileData profile;
        public SaveCampaignData campaign;
        public SaveInventoryData inventory;
        public SaveEconomyData economy;
        public SaveSettingsData settings;
        public SaveTutorialData tutorial;
        public SaveLedgerData ledger;

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

            profile.EnsureDefaults();
            campaign.EnsureDefaults();
            inventory.EnsureDefaults();
            economy.EnsureDefaults();
            settings.EnsureDefaults();
            tutorial.EnsureDefaults();
            ledger.EnsureDefaults();
        }

        public void ResetProgression()
        {
            campaign = SaveCampaignData.CreateDefault();
            inventory = SaveInventoryData.CreateDefault();
            economy = SaveEconomyData.CreateDefault();
            tutorial = SaveTutorialData.CreateDefault();
            ledger = SaveLedgerData.CreateDefault();
            EnsureDefaults();
        }
    }

    [Serializable]
    public sealed class SaveProfileData
    {
        public string profileId;
        public string playerName;
        public long createdUtcTicks;
        public bool introCompleted;

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
                playerName = "Гравець";
            }

            if (createdUtcTicks <= 0)
            {
                createdUtcTicks = DateTime.UtcNow.Ticks;
            }
        }
    }

    [Serializable]
    public sealed class SaveCampaignData
    {
        public int highestUnlockedMissionIndex;
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
        }
    }

    [Serializable]
    public sealed class SaveInventoryData
    {
        public List<string> unlockedWeaponIds;
        public List<string> selectedLoadoutWeaponIds;

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

            ReplaceLegacyWeaponIds(unlockedWeaponIds);
            ReplaceLegacyWeaponIds(selectedLoadoutWeaponIds);
            RemoveDeprecatedWeaponIds(unlockedWeaponIds);
            RemoveDeprecatedWeaponIds(selectedLoadoutWeaponIds);

            if (unlockedWeaponIds.Count == 0)
            {
                unlockedWeaponIds.Add("weapon.impulse");
                unlockedWeaponIds.Add("weapon.mortira");
            }

            if (selectedLoadoutWeaponIds.Count == 0)
            {
                selectedLoadoutWeaponIds.Add("weapon.impulse");
                selectedLoadoutWeaponIds.Add("weapon.mortira");
            }
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
    public sealed class SaveEconomyData
    {
        public int softCurrency;

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
