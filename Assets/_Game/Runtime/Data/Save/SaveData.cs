using System;
using System.Collections.Generic;

namespace Game.Data.Save
{
    [Serializable]
    public sealed class SaveData
    {
        public const int CurrentSchemaVersion = 1;

        public int schemaVersion;
        public SaveProfileData profile;
        public SaveCampaignData campaign;
        public SaveInventoryData inventory;
        public SaveEconomyData economy;
        public SaveSettingsData settings;
        public SaveTutorialData tutorial;

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

            profile.EnsureDefaults();
            campaign.EnsureDefaults();
            inventory.EnsureDefaults();
            economy.EnsureDefaults();
            settings.EnsureDefaults();
            tutorial.EnsureDefaults();
        }
    }

    [Serializable]
    public sealed class SaveProfileData
    {
        public string profileId;
        public string playerName;
        public long createdUtcTicks;

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

            if (unlockedWeaponIds.Count == 0)
            {
                unlockedWeaponIds.Add("weapon.steady-orb");
                unlockedWeaponIds.Add("weapon.pulse-shell");
            }

            if (selectedLoadoutWeaponIds.Count == 0)
            {
                selectedLoadoutWeaponIds.Add("weapon.steady-orb");
                selectedLoadoutWeaponIds.Add("weapon.pulse-shell");
            }
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
}
