using Game.Progression.Missions;
using NUnit.Framework;

namespace Game.Tests.EditMode.Progression
{
    public sealed class MvpMissionLibraryTests
    {
        [Test]
        public void CreateAll_ProducesValidMissionsWithDistinctIds()
        {
            var missions = MvpMissionLibrary.CreateAll();

            Assert.That(missions.Length, Is.InRange(8, 12));
            foreach (var mission in missions)
            {
                Assert.That(mission.IsValid(), Is.True);
                Assert.That(mission.Difficulty, Is.InRange(0, 2));
            }
        }

        [Test]
        public void CreateAll_DifficultyEscalatesAcrossCampaign()
        {
            var missions = MvpMissionLibrary.CreateAll();

            Assert.That(missions[0].Difficulty, Is.EqualTo(0), "First mission should be easy.");
            Assert.That(missions[^1].Difficulty, Is.EqualTo(2), "Final mission should be hard.");

            for (var i = 1; i < missions.Length; i++)
            {
                Assert.That(missions[i].Difficulty, Is.GreaterThanOrEqualTo(missions[i - 1].Difficulty),
                    "Difficulty must not drop as the campaign advances.");
            }
        }
    }
}
