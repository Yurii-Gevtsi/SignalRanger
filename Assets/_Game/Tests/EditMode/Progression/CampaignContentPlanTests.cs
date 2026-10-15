using Game.App.Content;
using NUnit.Framework;

namespace Game.Tests.EditMode.Progression
{
    public sealed class CampaignContentPlanTests
    {
        [Test]
        public void ReservedPlanetPlanContainsTwoOrderedFutureChapters()
        {
            Assert.That(CampaignContentPlan.AllReservedPlanets, Has.Count.EqualTo(2));
            Assert.That(CampaignContentPlan.AllReservedPlanets[0].ChapterId, Is.EqualTo("chapter.gale-foundry"));
            Assert.That(CampaignContentPlan.AllReservedPlanets[1].ChapterId, Is.EqualTo("chapter.cryo-relay"));
            Assert.That(CampaignContentPlan.AllReservedPlanets[0].MissionCount, Is.EqualTo(6));
            Assert.That(CampaignContentPlan.AllReservedPlanets[1].MissionCount, Is.EqualTo(7));
        }

        [Test]
        public void PacingTargetsMatchApprovedBaseline()
        {
            Assert.That(CampaignPacingTargets.FirstNewWeaponMinutesMin, Is.EqualTo(30));
            Assert.That(CampaignPacingTargets.FirstNewWeaponMinutesMax, Is.EqualTo(60));
            Assert.That(CampaignPacingTargets.Rank5HoursMin, Is.EqualTo(2));
            Assert.That(CampaignPacingTargets.Rank5HoursMax, Is.EqualTo(4));
            Assert.That(CampaignPacingTargets.Rank10HoursMin, Is.EqualTo(8));
            Assert.That(CampaignPacingTargets.Rank10HoursMax, Is.EqualTo(12));
        }
    }
}
