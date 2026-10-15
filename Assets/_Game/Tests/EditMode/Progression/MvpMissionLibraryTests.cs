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

            Assert.That(missions.Length, Is.EqualTo(27));
            foreach (var mission in missions)
            {
                Assert.That(mission.IsValid(), Is.True);
                Assert.That(mission.Difficulty, Is.InRange(0, 2));
                Assert.That(mission.ArenaId, Is.Not.Empty);
                Assert.That(mission.EnemyLoadoutId, Is.Not.Empty);
                Assert.That(mission.DifficultyProfileId, Is.Not.Empty);
                Assert.That(mission.SupportDropsEnabled, Is.True);
                Assert.That(mission.Objective.Kind, Is.EqualTo(MissionObjectiveKind.None));
            }
        }

        [Test]
        public void CreateAll_EachPlanetHasFixedEasyNormalHardSequence()
        {
            var missions = MvpMissionLibrary.CreateAll();

            for (var i = 0; i < missions.Length; i += 3)
            {
                Assert.That(missions[i].Difficulty, Is.EqualTo(0));
                Assert.That(missions[i + 1].Difficulty, Is.EqualTo(1));
                Assert.That(missions[i + 2].Difficulty, Is.EqualTo(2));
                Assert.That(missions[i].ChapterId, Is.EqualTo(missions[i + 1].ChapterId));
                Assert.That(missions[i].ChapterId, Is.EqualTo(missions[i + 2].ChapterId));
            }
        }

        [Test]
        public void CreateAll_UsesOnlyLocalizedDifficultyMissionText()
        {
            var missions = MvpMissionLibrary.CreateAll();
            var expectedKeys = new[]
            {
                "campaign.mission.easy",
                "campaign.mission.medium",
                "campaign.mission.hard"
            };

            for (var i = 0; i < missions.Length; i++)
            {
                var expectedKey = expectedKeys[i % 3];
                Assert.That(missions[i].DisplayNameKey, Is.EqualTo(expectedKey));
                Assert.That(missions[i].DescriptionKey, Is.EqualTo(expectedKey));
                Assert.That(missions[i].Description, Is.Not.Empty);
            }
        }

        [Test]
        public void CreateAll_FirstChapterFormsAPolishedTeachingSlice()
        {
            var missions = MvpMissionLibrary.CreateAll();

            Assert.That(missions[0].ChapterId, Is.EqualTo("chapter.basics"));
            Assert.That(missions[1].ChapterId, Is.EqualTo("chapter.basics"));
            Assert.That(missions[2].ChapterId, Is.EqualTo("chapter.basics"));

            Assert.That(missions[0].TutorialStepIds, Is.EquivalentTo(new[] { "tutorial.aim", "tutorial.fire", "tutorial.impact", "tutorial.turn", "tutorial.win" }));
            Assert.That(missions[1].TutorialStepIds, Contains.Item("tutorial.arc"));
            Assert.That(missions[1].TutorialStepIds, Contains.Item("tutorial.power"));
            Assert.That(missions[1].TutorialStepIds, Contains.Item("tutorial.arc-review"));
            Assert.That(missions[2].TutorialStepIds, Contains.Item("tutorial.cover-target"));
            Assert.That(missions[2].TutorialStepIds, Contains.Item("tutorial.radius"));
        }

        [TestCase("mission.placeholder.monument", "chapter.monument", "arena.monument")]
        [TestCase("mission.placeholder.kraken", "chapter.kraken", "arena.kraken")]
        [TestCase("mission.placeholder.vines", "chapter.vines", "arena.vines")]
        [TestCase("mission.placeholder.robots", "chapter.robots", "arena.robots")]
        public void PlaceholderMission_ProvidesAPlayableArenaForEveryImplementedPlanet(string missionId, string chapterId, string arenaId)
        {
            var mission = System.Array.Find(MvpMissionLibrary.CreateAll(), candidate => candidate.MissionId == missionId);

            Assert.That(mission, Is.Not.Null);
            Assert.That(mission.ChapterId, Is.EqualTo(chapterId));
            Assert.That(mission.ArenaId, Is.EqualTo(arenaId));
            Assert.That(mission.DisplayNameKey, Is.Not.Empty);
            Assert.That(mission.DescriptionKey, Is.Not.Empty);
        }
    }
}
