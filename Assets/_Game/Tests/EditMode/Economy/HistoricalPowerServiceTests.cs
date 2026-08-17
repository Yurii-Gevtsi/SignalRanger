using Game.Data.Progression;
using Game.Data.Save;
using Game.Economy;
using NUnit.Framework;

namespace Game.Tests.EditMode.Economy
{
    public sealed class HistoricalPowerServiceTests
    {
        [Test]
        public void CurrentPowerIncludesLevelOwnershipRanksEvolutionAndDrone()
        {
            var data = SaveData.CreateNew();
            data.profile.commanderExperience = CommanderProgressionTable.GetExperienceRequiredForLevel(2);
            data.inventory.unlockedWeaponIds.Add("weapon.bur");
            var impulse = data.inventory.GetOrCreateWeaponProgress("weapon.impulse");
            impulse.rank = 3;
            impulse.branch = (int)WeaponUpgradeBranch.A;
            data.inventory.droneUpgradePower = 100;

            Assert.That(HistoricalPowerService.CalculateCurrentAccountPower(data), Is.EqualTo(1_350));
        }

        [Test]
        public void RefreshIsMonotonicWhenProgressIsResetOrCorrupted()
        {
            var data = SaveData.CreateNew();
            var service = new HistoricalPowerService(data);
            data.profile.commanderExperience = CommanderProgressionTable.GetExperienceRequiredForLevel(5);
            data.inventory.GetOrCreateWeaponProgress("weapon.impulse").rank = 10;
            data.inventory.droneUpgradePower = 240;
            var highWaterMark = service.Refresh();

            data.profile.commanderExperience = 0;
            data.inventory.weaponProgress.Clear();
            data.inventory.droneUpgradePower = 0;

            Assert.That(service.Refresh(), Is.EqualTo(highWaterMark));
            Assert.That(service.HistoricalPower, Is.EqualTo(highWaterMark));
        }

        [Test]
        public void LegacySaveMigratesWithSafeHistoricalPowerAndDroneDefaults()
        {
            var restored = SaveDataSerializer.FromJson(@"{
                ""schemaVersion"": 7,
                ""profile"": { ""profileId"": ""legacy"" },
                ""inventory"": { ""unlockedWeaponIds"": [""weapon.impulse""] }
            }");

            Assert.That(restored.schemaVersion, Is.EqualTo(SaveData.CurrentSchemaVersion));
            Assert.That(restored.profile.historicalPower, Is.Zero);
            Assert.That(restored.inventory.droneUpgradePower, Is.Zero);
            Assert.That(new HistoricalPowerService(restored).Refresh(), Is.GreaterThan(0));
        }
    }
}
