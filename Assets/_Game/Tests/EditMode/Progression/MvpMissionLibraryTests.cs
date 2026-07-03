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
                Assert.That(mission.ArenaId, Is.Not.Empty);
                Assert.That(mission.EnemyLoadoutId, Is.Not.Empty);
                Assert.That(mission.DifficultyProfileId, Is.Not.Empty);
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

        [Test]
        public void CreateAll_FirstChapterFormsAPolishedTeachingSlice()
        {
            var missions = MvpMissionLibrary.CreateAll();

            Assert.That(missions[0].ChapterId, Is.EqualTo("chapter.basics"));
            Assert.That(missions[1].ChapterId, Is.EqualTo("chapter.basics"));
            Assert.That(missions[2].ChapterId, Is.EqualTo("chapter.basics"));
            Assert.That(missions[3].ChapterId, Is.EqualTo("chapter.basics"));
            Assert.That(missions[4].ChapterId, Is.EqualTo("chapter.basics"));

            Assert.That(missions[0].TutorialStepIds, Is.EquivalentTo(new[] { "tutorial.aim", "tutorial.fire", "tutorial.impact", "tutorial.turn", "tutorial.win" }));
            Assert.That(missions[1].TutorialStepIds, Contains.Item("tutorial.arc"));
            Assert.That(missions[1].TutorialStepIds, Contains.Item("tutorial.power"));
            Assert.That(missions[1].TutorialStepIds, Contains.Item("tutorial.arc-review"));
            Assert.That(missions[2].TutorialStepIds, Contains.Item("tutorial.cover-target"));
            Assert.That(missions[2].TutorialStepIds, Contains.Item("tutorial.radius"));
            Assert.That(missions[4].TutorialStepIds, Contains.Item("tutorial.weapon-switch"));
        }
    }
}
