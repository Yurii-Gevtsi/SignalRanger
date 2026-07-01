using System.IO;
using Game.Data.Save;
using NUnit.Framework;

namespace Game.Tests.EditMode.Data
{
    public sealed class SaveDataTests
    {
        [Test]
        public void CreateNew_FillsRequiredDefaults()
        {
            var saveData = SaveData.CreateNew();

            Assert.That(saveData.schemaVersion, Is.EqualTo(SaveData.CurrentSchemaVersion));
            Assert.That(saveData.profile.playerName, Is.EqualTo("Гравець"));
            Assert.That(saveData.campaign.highestUnlockedMissionIndex, Is.EqualTo(0));
            Assert.That(saveData.inventory.unlockedWeaponIds, Contains.Item("weapon.steady-orb"));
            Assert.That(saveData.economy.softCurrency, Is.EqualTo(0));
            Assert.That(saveData.settings.languageCode, Is.EqualTo("uk"));
            Assert.That(saveData.tutorial.completedStepIds, Is.Not.Null);
        }

        [Test]
        public void Serializer_RoundTripsProgress()
        {
            var saveData = SaveData.CreateNew();
            saveData.profile.playerName = "Тестер";
            saveData.campaign.highestUnlockedMissionIndex = 2;
            saveData.campaign.completedMissionIds.Add("mission.training.01");
            saveData.economy.softCurrency = 75;
            saveData.settings.musicVolume = 0f;

            var json = SaveDataSerializer.ToJson(saveData);
            var restored = SaveDataSerializer.FromJson(json);

            Assert.That(restored.profile.playerName, Is.EqualTo("Тестер"));
            Assert.That(restored.campaign.highestUnlockedMissionIndex, Is.EqualTo(2));
            Assert.That(restored.campaign.completedMissionIds, Contains.Item("mission.training.01"));
            Assert.That(restored.economy.softCurrency, Is.EqualTo(75));
            Assert.That(restored.settings.musicVolume, Is.EqualTo(0f));
        }

        [Test]
        public void Serializer_MissingFieldsReceiveDefaults()
        {
            var restored = SaveDataSerializer.FromJson("{}");

            Assert.That(restored.schemaVersion, Is.EqualTo(SaveData.CurrentSchemaVersion));
            Assert.That(restored.profile.profileId, Is.Not.Empty);
            Assert.That(restored.inventory.selectedLoadoutWeaponIds, Is.Not.Empty);
            Assert.That(restored.settings.languageCode, Is.EqualTo("uk"));
        }

        [Test]
        public void Serializer_MigratesVersionOneSaveToCurrent()
        {
            var json = @"{
                ""schemaVersion"": 1,
                ""profile"": {
                    ""profileId"": ""legacy-profile"",
                    ""playerName"": ""Старий профіль"",
                    ""createdUtcTicks"": 42
                },
                ""campaign"": {
                    ""highestUnlockedMissionIndex"": 3
                },
                ""economy"": {
                    ""softCurrency"": 25
                },
                ""settings"": {
                    ""languageCode"": ""uk""
                }
            }";

            var restored = SaveDataSerializer.FromJson(json);

            Assert.That(restored.schemaVersion, Is.EqualTo(SaveData.CurrentSchemaVersion));
            Assert.That(restored.profile.profileId, Is.EqualTo("legacy-profile"));
            Assert.That(restored.profile.playerName, Is.EqualTo("Старий профіль"));
            Assert.That(restored.campaign.highestUnlockedMissionIndex, Is.EqualTo(3));
            Assert.That(restored.economy.softCurrency, Is.EqualTo(25));
            Assert.That(restored.settings.hasVolumeSettings, Is.True);
            Assert.That(restored.settings.musicVolume, Is.EqualTo(0.8f));
            Assert.That(restored.settings.sfxVolume, Is.EqualTo(1f));
        }

        [Test]
        public void MigrationService_RejectsNewerSchema()
        {
            var saveData = SaveData.CreateNew();
            saveData.schemaVersion = SaveData.CurrentSchemaVersion + 1;
            var migrationService = new SaveMigrationService();

            Assert.That(
                () => migrationService.MigrateToCurrent(saveData),
                Throws.InvalidOperationException);
        }
    }

    public sealed class JsonFileSaveServiceTests
    {
        private string _tempDirectory;

        [SetUp]
        public void SetUp()
        {
            _tempDirectory = Path.Combine(Path.GetTempPath(), "GameSaveTests", TestContext.CurrentContext.Test.ID);
            Directory.CreateDirectory(_tempDirectory);
        }

        [TearDown]
        public void TearDown()
        {
            if (Directory.Exists(_tempDirectory))
            {
                Directory.Delete(_tempDirectory, true);
            }
        }

        [Test]
        public void LoadOrCreate_CreatesFileWhenMissing()
        {
            var savePath = Path.Combine(_tempDirectory, "save.json");
            var service = new JsonFileSaveService(savePath);

            var result = service.LoadOrCreate();

            Assert.That(result.Status, Is.EqualTo(SaveLoadStatus.CreatedNew));
            Assert.That(File.Exists(savePath), Is.True);
            Assert.That(service.Current.profile.playerName, Is.EqualTo("Гравець"));
        }

        [Test]
        public void Save_ThenLoad_PreservesData()
        {
            var savePath = Path.Combine(_tempDirectory, "save.json");
            var service = new JsonFileSaveService(savePath);
            var saveData = SaveData.CreateNew();
            saveData.profile.playerName = "Локальний";
            saveData.economy.softCurrency = 120;

            service.Save(saveData);
            var secondService = new JsonFileSaveService(savePath);
            var result = secondService.LoadOrCreate();

            Assert.That(result.Status, Is.EqualTo(SaveLoadStatus.Loaded));
            Assert.That(secondService.Current.profile.playerName, Is.EqualTo("Локальний"));
            Assert.That(secondService.Current.economy.softCurrency, Is.EqualTo(120));
        }

        [Test]
        public void LoadOrCreate_BacksUpCorruptSaveAndRecovers()
        {
            var savePath = Path.Combine(_tempDirectory, "save.json");
            File.WriteAllText(savePath, "{ this is not valid json");
            var service = new JsonFileSaveService(savePath);

            var result = service.LoadOrCreate();

            Assert.That(result.Status, Is.EqualTo(SaveLoadStatus.RecoveredFromCorrupt));
            Assert.That(service.Current.profile.playerName, Is.EqualTo("Гравець"));
            Assert.That(Directory.GetFiles(_tempDirectory, "save.corrupt-*.json"), Is.Not.Empty);
            Assert.That(File.Exists(savePath), Is.True);
        }
    }
}
