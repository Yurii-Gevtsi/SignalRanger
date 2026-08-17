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
    }
}
