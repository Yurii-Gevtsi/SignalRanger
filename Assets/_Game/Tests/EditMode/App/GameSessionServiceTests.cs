using Game.App;
using Game.App.Content;
using Game.Core;
using Game.Data.Save;
using Game.Progression.Missions;
using NUnit.Framework;

namespace Game.Tests.EditMode.App
{
    public sealed class GameSessionServiceTests
    {
        [Test]
        public void SelectTrainingSkirmish_UsesDedicatedTrainingArena()
        {
            var content = DefaultGameContentCatalog.Create();
            var save = SaveData.CreateNew();
            save.EnsureDefaults();
            var campaign = new CampaignProgressionService(save, content.Missions);
            var session = new GameSessionService(save, campaign, content);

            session.SelectTrainingSkirmish();

            Assert.That(GameSessionContext.IsTrainingSkirmish, Is.True);
            Assert.That(GameSessionContext.SelectedArenaId, Is.EqualTo("arena.training-meadow"));
            Assert.That(GameSessionContext.SelectedArenaIndex, Is.EqualTo(0));
        }

        [Test]
        public void NewCampaign_SelectsKerionsBattleArenaForTheFirstMission()
        {
            var content = DefaultGameContentCatalog.Create();
            var save = SaveData.CreateNew();
            var campaign = new CampaignProgressionService(save, content.Missions);
            var session = new GameSessionService(save, campaign, content);

            session.SelectNextMission();

            Assert.That(GameSessionContext.SelectedMissionId, Is.EqualTo("mission.training.01"));
            Assert.That(GameSessionContext.SelectedArenaId, Is.EqualTo("arena.desert-sci-fi"));
            Assert.That(GameSessionContext.SelectedArenaIndex, Is.EqualTo(1));
        }

        [TestCase(8, "mission.training.09", "arena.city", 3)]
        [TestCase(10, "mission.training.11", "arena.grib", 4)]
        [TestCase(12, "mission.training.13", "arena.laguna", 5)]
        public void SelectMission_NewPlanetMissionPublishesItsDedicatedArena(
            int missionIndex,
            string missionId,
            string arenaId,
            int arenaIndex)
        {
            var content = DefaultGameContentCatalog.Create();
            var save = SaveData.CreateNew();
            var campaign = new CampaignProgressionService(save, content.Missions);
            var session = new GameSessionService(save, campaign, content);

            Assert.That(campaign.MissionCount, Is.EqualTo(18));
            session.SelectMission(missionIndex);

            Assert.That(GameSessionContext.SelectedMissionId, Is.EqualTo(missionId));
            Assert.That(GameSessionContext.SelectedArenaId, Is.EqualTo(arenaId));
            Assert.That(GameSessionContext.SelectedArenaIndex, Is.EqualTo(arenaIndex));
        }
    }
}
