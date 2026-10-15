using Game.Data.Save;
using Game.Progression.Missions;
using NUnit.Framework;

namespace Game.Tests.EditMode.Progression
{
    public sealed class CampaignProgressionServiceTests
    {
        [Test]
        public void CompleteMission_AppliesRewardAndUnlocksNextMission()
        {
            var saveData = SaveData.CreateNew();
            var service = new CampaignProgressionService(saveData, MvpMissionLibrary.CreateAll());

            var result = service.CompleteMission(0);

            Assert.That(result.Applied, Is.True);
            Assert.That(saveData.campaign.completedMissionIds, Contains.Item("mission.training.01"));
            Assert.That(saveData.campaign.highestUnlockedMissionIndex, Is.EqualTo(3));
            Assert.That(saveData.economy.softCurrency, Is.EqualTo(250));
            Assert.That(saveData.profile.commanderExperience, Is.EqualTo(80));
            Assert.That(saveData.inventory.GetOrCreateWeaponProgress("weapon.impulse").blueprints, Is.EqualTo(25));
        }

        [Test]
        public void CompleteMission_DoesNotApplyRewardTwice()
        {
            var saveData = SaveData.CreateNew();
            var service = new CampaignProgressionService(saveData, MvpMissionLibrary.CreateAll());

            service.CompleteMission(0);
            var secondResult = service.CompleteMission(0);

            Assert.That(secondResult.Applied, Is.False);
            Assert.That(saveData.economy.softCurrency, Is.EqualTo(250));
            Assert.That(saveData.campaign.completedMissionIds, Has.Count.EqualTo(1));
        }

        [Test]
        public void CompleteMission_AppliesWeaponUnlockOnce()
        {
            var saveData = SaveData.CreateNew();
            var service = new CampaignProgressionService(saveData, MvpMissionLibrary.CreateAll());

            var result = service.CompleteMission(2);
            var secondResult = service.CompleteMission(2);

            Assert.That(result.HasUnlock, Is.True);
            Assert.That(result.UnlockedWeaponId, Is.EqualTo("weapon.granata"));
            Assert.That(secondResult.HasUnlock, Is.False);
            Assert.That(saveData.inventory.unlockedWeaponIds, Contains.Item("weapon.granata"));
        }

        [Test]
        public void CompleteMission_RecordsLedgerTransaction()
        {
            var saveData = SaveData.CreateNew();
            var service = new CampaignProgressionService(saveData, MvpMissionLibrary.CreateAll());

            service.CompleteMission(0);

            Assert.That(saveData.ledger.entries, Has.Count.EqualTo(1));
            Assert.That(saveData.ledger.entries[0].transactionId, Is.EqualTo("mission.first.mission.training.01"));
            Assert.That(saveData.ledger.entries[0].sourceType, Is.EqualTo("mission.first"));
            Assert.That(saveData.ledger.entries[0].softCurrencyDelta, Is.EqualTo(250));
            Assert.That(saveData.ledger.entries[0].commanderExperienceDelta, Is.EqualTo(80));
            Assert.That(saveData.ledger.entries[0].blueprintDelta, Is.EqualTo(25));
        }

        [Test]
        public void GetNextMission_UsesHighestUnlockedMissionIndex()
        {
            var saveData = SaveData.CreateNew();
            saveData.campaign.highestUnlockedMissionIndex = 3;
            var service = new CampaignProgressionService(saveData, MvpMissionLibrary.CreateAll());

            var mission = service.GetNextMission();

            Assert.That(mission.MissionId, Is.EqualTo("mission.training.06"));
        }

        [Test]
        public void CompletingEasyMission_OpensNextPlanetsEasyMissionAndKeepsOptionalMissionsOpen()
        {
            var saveData = SaveData.CreateNew();
            var missions = MvpMissionLibrary.CreateAll();
            var service = new CampaignProgressionService(saveData, missions);

            service.CompleteMission(0);

            Assert.That(service.IsUnlocked(1), Is.True, "Current planet medium mission should remain available.");
            Assert.That(service.IsUnlocked(2), Is.True, "Current planet hard mission should remain available.");
            Assert.That(service.IsUnlocked(3), Is.True, "Next planet easy mission should become available.");
            Assert.That(service.IsUnlocked(4), Is.False, "Next planet medium mission should wait for its easy mission.");
        }

        [Test]
        public void IsUnlocked_AdminModeAllowsEveryValidMissionWithoutChangingProgress()
        {
            var saveData = SaveData.CreateNew();
            saveData.settings.adminModeEnabled = true;
            var missions = MvpMissionLibrary.CreateAll();
            var service = new CampaignProgressionService(saveData, missions);

            Assert.That(service.IsUnlocked(missions.Length - 1), Is.True);
            Assert.That(service.IsUnlocked(-1), Is.False);
            Assert.That(saveData.campaign.highestUnlockedMissionIndex, Is.EqualTo(0));
        }
    }
}
