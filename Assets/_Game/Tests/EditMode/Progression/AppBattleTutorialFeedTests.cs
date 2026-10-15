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

#pragma warning disable 67
            public event System.Action<SaveData> Saved;
#pragma warning restore 67

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
            var tutorial = new TutorialService(data, TutorialStepLibrary.CreateCampaignSteps());
            var save = new FakeSaveService(data);
            return (new AppBattleTutorialFeed(tutorial, save), tutorial, save);
        }

        [Test]
        public void ReportTrigger_ReturnsMessageAndCompletesStep_OnFirstObservation()
        {
            var (feed, tutorial, save) = CreateFeed();
            GameSessionContext.SelectMission(0, 0, "mission.training.01", "arena.training-meadow", null,
                new[] { "tutorial.aim", "tutorial.fire", "tutorial.impact", "tutorial.turn", "tutorial.win" },
                string.Empty, null, string.Empty, string.Empty, 0, string.Empty, 0);

            var cue = feed.ReportTrigger(BattleTutorialBridge.MissionStart);

            Assert.That(cue.Message, Is.Not.Empty);
            Assert.That(cue.UiTargetId, Is.EqualTo("battle.aim-area"));
            Assert.That(tutorial.IsCompleted("tutorial.aim"), Is.True);
            Assert.That(save.SaveCount, Is.EqualTo(1));
        }

        [Test]
        public void ReportTrigger_ReturnsEmptyWhenAlreadyCompleted()
        {
            var (feed, _, _) = CreateFeed();
            GameSessionContext.SelectMission(0, 0, "mission.training.01", "arena.training-meadow", null,
                new[] { "tutorial.aim", "tutorial.fire", "tutorial.impact", "tutorial.turn", "tutorial.win" },
                string.Empty, null, string.Empty, string.Empty, 0, string.Empty, 0);
            feed.ReportTrigger(BattleTutorialBridge.MissionStart);

            var second = feed.ReportTrigger(BattleTutorialBridge.MissionStart);

            Assert.That(second.IsEmpty, Is.True);
        }

        [Test]
        public void ReportTrigger_ReturnsEmptyForUnknownKey()
        {
            var (feed, _, save) = CreateFeed();
            GameSessionContext.SelectMission(0, 0, "mission.training.01", "arena.training-meadow", null,
                new[] { "tutorial.aim", "tutorial.fire", "tutorial.impact", "tutorial.turn", "tutorial.win" },
                string.Empty, null, string.Empty, string.Empty, 0, string.Empty, 0);

            var cue = feed.ReportTrigger("not-a-trigger");

            Assert.That(cue.IsEmpty, Is.True);
            Assert.That(save.SaveCount, Is.EqualTo(0));
        }

        [Test]
        public void ReportTrigger_UsesMissionSpecificTutorialSteps()
        {
            var (feed, tutorial, save) = CreateFeed();
            GameSessionContext.SelectMission(1, 0, "mission.training.02", "arena.training-meadow", null,
                new[] { "tutorial.arc", "tutorial.power" },
                string.Empty, null, string.Empty, string.Empty, 0, string.Empty, 0);

            var cue = feed.ReportTrigger(BattleTutorialBridge.MissionStart);

            Assert.That(cue.Message, Is.Not.Empty);
            Assert.That(cue.UiTargetId, Is.EqualTo("battle.aim-area"));
            Assert.That(tutorial.IsCompleted("tutorial.arc"), Is.True);
            Assert.That(tutorial.IsCompleted("tutorial.aim"), Is.False);
            Assert.That(save.SaveCount, Is.EqualTo(1));
        }

        [Test]
        public void SkipActiveMissionTutorial_CompletesAndSavesEveryMissionStep()
        {
            var (feed, tutorial, save) = CreateFeed();
            GameSessionContext.SelectMission(0, 0, "mission.training.01", "arena.training-meadow", null,
                new[] { "tutorial.aim", "tutorial.fire", "tutorial.impact", "tutorial.turn", "tutorial.win" },
                string.Empty, null, string.Empty, string.Empty, 0, string.Empty, 0);

            feed.SkipActiveMissionTutorial();

            Assert.That(tutorial.IsCompleted("tutorial.aim"), Is.True);
            Assert.That(tutorial.IsCompleted("tutorial.fire"), Is.True);
            Assert.That(tutorial.IsCompleted("tutorial.impact"), Is.True);
            Assert.That(tutorial.IsCompleted("tutorial.turn"), Is.True);
            Assert.That(tutorial.IsCompleted("tutorial.win"), Is.True);
            Assert.That(save.SaveCount, Is.EqualTo(1));
            Assert.That(feed.IsActiveMissionTutorialCompleted(), Is.True);
        }
    }
}
