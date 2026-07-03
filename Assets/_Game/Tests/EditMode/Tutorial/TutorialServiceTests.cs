using Game.Data.Save;
using Game.Tutorial;
using NUnit.Framework;

namespace Game.Tests.EditMode.Tutorial
{
    public sealed class TutorialServiceTests
    {
        [Test]
        public void FindNext_ReturnsFirstAvailableStepForTrigger()
        {
            var saveData = SaveData.CreateNew();
            var service = new TutorialService(saveData, TutorialStepLibrary.CreateFirstBattleSteps());

            var step = service.FindNext(TutorialStepTrigger.OnMissionStart);

            Assert.That(step, Is.Not.Null);
            Assert.That(step.StepId, Is.EqualTo("tutorial.aim"));
        }

        [Test]
        public void FindNext_WaitsForRequiredSteps()
        {
            var saveData = SaveData.CreateNew();
            var service = new TutorialService(saveData, TutorialStepLibrary.CreateFirstBattleSteps());

            var blocked = service.FindNext(TutorialStepTrigger.OnAimStarted);
            service.Complete("tutorial.aim");
            var available = service.FindNext(TutorialStepTrigger.OnAimStarted);

            Assert.That(blocked, Is.Null);
            Assert.That(available, Is.Not.Null);
            Assert.That(available.StepId, Is.EqualTo("tutorial.fire"));
        }

        [Test]
        public void Complete_IsIdempotent()
        {
            var saveData = SaveData.CreateNew();
            var service = new TutorialService(saveData, TutorialStepLibrary.CreateFirstBattleSteps());

            Assert.That(service.Complete("tutorial.aim"), Is.True);
            Assert.That(service.Complete("tutorial.aim"), Is.False);
            Assert.That(saveData.tutorial.completedStepIds, Has.Count.EqualTo(1));
        }

        [Test]
        public void FindNext_RespectsAllowedMissionStepIds()
        {
            var saveData = SaveData.CreateNew();
            var service = new TutorialService(saveData, TutorialStepLibrary.CreateCampaignSteps());

            var step = service.FindNext(TutorialStepTrigger.OnMissionStart, new[] { "tutorial.arc", "tutorial.power" });

            Assert.That(step, Is.Not.Null);
            Assert.That(step.StepId, Is.EqualTo("tutorial.arc"));
        }
    }
}
