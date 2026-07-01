using Game.App;
using Game.Core;
using Game.Data.Save;
using Game.Tutorial;
using NUnit.Framework;

namespace Game.Tests.EditMode.Progression
{
    public sealed class AppBattleTutorialFeedTests
    {
        private sealed class FakeSaveService : ISaveService
        {
            public FakeSaveService(SaveData data)
            {
                Current = data;
            }

            public int SaveCount { get; private set; }
            public SaveData Current { get; }

            public SaveLoadResult LoadOrCreate() => default;

            public void Save(SaveData data) => SaveCount++;

            public void Delete()
            {
            }
        }

        private static (AppBattleTutorialFeed feed, TutorialService tutorial, FakeSaveService save) CreateFeed()
        {
            var data = SaveData.CreateNew();
            data.EnsureDefaults();
            var tutorial = new TutorialService(data, TutorialStepLibrary.CreateFirstBattleSteps());
            var save = new FakeSaveService(data);
            return (new AppBattleTutorialFeed(tutorial, save), tutorial, save);
        }

        [Test]
        public void ReportTrigger_ReturnsMessageAndCompletesStep_OnFirstObservation()
        {
            var (feed, tutorial, save) = CreateFeed();

            var message = feed.ReportTrigger(BattleTutorialBridge.MissionStart);

            Assert.That(message, Is.Not.Empty);
            Assert.That(tutorial.IsCompleted("tutorial.aim"), Is.True);
            Assert.That(save.SaveCount, Is.EqualTo(1));
        }

        [Test]
        public void ReportTrigger_ReturnsEmptyWhenAlreadyCompleted()
        {
            var (feed, _, _) = CreateFeed();
            feed.ReportTrigger(BattleTutorialBridge.MissionStart);

            var second = feed.ReportTrigger(BattleTutorialBridge.MissionStart);

            Assert.That(second, Is.Empty);
        }

        [Test]
        public void ReportTrigger_ReturnsEmptyForUnknownKey()
        {
            var (feed, _, save) = CreateFeed();

            var message = feed.ReportTrigger("not-a-trigger");

            Assert.That(message, Is.Empty);
            Assert.That(save.SaveCount, Is.EqualTo(0));
        }
    }
}
