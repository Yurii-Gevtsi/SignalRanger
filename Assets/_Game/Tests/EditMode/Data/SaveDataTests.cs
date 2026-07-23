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
            Assert.That(saveData.inventory.unlockedWeaponIds, Contains.Item("weapon.impulse"));
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
        public void ResetProgression_RestoresGameplayDefaultsAndPreservesProfileAndSettings()
        {
            var saveData = SaveData.CreateNew();
            var profileId = saveData.profile.profileId;
            saveData.settings.musicVolume = 0.3f;
            saveData.settings.sfxVolume = 0.4f;
            saveData.campaign.highestUnlockedMissionIndex = 3;
            saveData.campaign.completedMissionIds.Add("mission.training.01");
            saveData.inventory.unlockedWeaponIds.Add("weapon.test");
            saveData.inventory.selectedLoadoutWeaponIds.Clear();
            saveData.inventory.selectedLoadoutWeaponIds.Add("weapon.test");
            saveData.economy.softCurrency = 125;
            saveData.tutorial.completedStepIds.Add("tutorial.test");

            saveData.ResetProgression();

            Assert.That(saveData.profile.profileId, Is.EqualTo(profileId));
            Assert.That(saveData.settings.musicVolume, Is.EqualTo(0.3f));
            Assert.That(saveData.settings.sfxVolume, Is.EqualTo(0.4f));
            Assert.That(saveData.campaign.highestUnlockedMissionIndex, Is.Zero);
            Assert.That(saveData.campaign.completedMissionIds, Is.Empty);
            Assert.That(saveData.inventory.unlockedWeaponIds, Is.EquivalentTo(new[] { "weapon.impulse", "weapon.mortira" }));
            Assert.That(saveData.inventory.selectedLoadoutWeaponIds, Is.EquivalentTo(new[] { "weapon.impulse", "weapon.mortira" }));
            Assert.That(saveData.economy.softCurrency, Is.Zero);
            Assert.That(saveData.tutorial.completedStepIds, Is.Empty);
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
        public void Serializer_MigratesVersionThreeSaveToCurrentWithLedger()
        {
            var json = @"{
                ""schemaVersion"": 3,
                ""profile"": {
                    ""profileId"": ""v3-profile""
                },
                ""economy"": {
                    ""softCurrency"": 40
                }
            }";

            var restored = SaveDataSerializer.FromJson(json);

            Assert.That(restored.schemaVersion, Is.EqualTo(SaveData.CurrentSchemaVersion));
            Assert.That(restored.profile.profileId, Is.EqualTo("v3-profile"));
            Assert.That(restored.economy.softCurrency, Is.EqualTo(40));
            Assert.That(restored.ledger, Is.Not.Null);
            Assert.That(restored.ledger.entries, Is.Empty);
        }

        [Test]
        public void Serializer_RoundTripsLedgerEntries()
        {
            var saveData = SaveData.CreateNew();
            saveData.ledger.Append(new SaveLedgerEntry
            {
                transactionId = "tx-1",
                sourceType = "mission.first",
                sourceId = "mission.training.01",
                softCurrencyDelta = 25,
                createdUtcTicks = 123456789,
                pendingSync = true
            });

            var restored = SaveDataSerializer.FromJson(SaveDataSerializer.ToJson(saveData));

            Assert.That(restored.ledger.entries, Has.Count.EqualTo(1));
            Assert.That(restored.ledger.entries[0].transactionId, Is.EqualTo("tx-1"));
            Assert.That(restored.ledger.entries[0].softCurrencyDelta, Is.EqualTo(25));
            Assert.That(restored.ledger.entries[0].pendingSync, Is.True);
            Assert.That(restored.ledger.ContainsTransaction("tx-1"), Is.True);
        }

        [Test]
        public void Ledger_TrimsOldestSyncedEntriesBeyondWindow()
        {
            var ledger = SaveLedgerData.CreateDefault();
            for (var index = 0; index < SaveLedgerData.MaxEntries; index++)
            {
                ledger.Append(new SaveLedgerEntry
                {
                    transactionId = $"tx-{index}",
                    pendingSync = index == 0
                });
            }

            ledger.Append(new SaveLedgerEntry { transactionId = "tx-overflow", pendingSync = true });

            Assert.That(ledger.entries, Has.Count.EqualTo(SaveLedgerData.MaxEntries));
            Assert.That(ledger.ContainsTransaction("tx-0"), Is.True, "pendingSync запис не можна витісняти");
            Assert.That(ledger.ContainsTransaction("tx-1"), Is.False, "найстаріший синхронізований запис витісняється");
            Assert.That(ledger.ContainsTransaction("tx-overflow"), Is.True);
        }

        [Test]
        public void MigrationService_MigratesV3ToV4CreatingLedger()
        {
            var saveData = SaveData.CreateNew();
            saveData.schemaVersion = 3;
            saveData.ledger = null;

            new SaveMigrationService().MigrateToCurrent(saveData);

            Assert.That(saveData.schemaVersion, Is.EqualTo(SaveData.CurrentSchemaVersion));
            Assert.That(saveData.ledger, Is.Not.Null);
            Assert.That(saveData.ledger.entries, Is.Empty);
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
