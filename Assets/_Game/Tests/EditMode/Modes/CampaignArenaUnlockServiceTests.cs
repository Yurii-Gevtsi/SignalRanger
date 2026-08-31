using System.Collections.Generic;
using Game.App.Content;
using Game.Progression.Missions;
using Game.Progression.Modes;
using NUnit.Framework;

namespace Game.Tests.EditMode.Modes
{
    public sealed class CampaignArenaUnlockServiceTests
    {
        [Test]
        public void NoProgress_LeavesTheRepeatableModesClosed()
        {
            var missions = DefaultGameContentCatalog.Create().Missions;

            var unlocked = CampaignArenaUnlockService.ResolveUnlockedArenas(missions, new List<string>());

            Assert.That(unlocked, Is.Empty);
            Assert.That(CampaignArenaUnlockService.HasAnyUnlockedArena(missions, new List<string>()), Is.False);
        }

        [Test]
        public void HalfClearedChapter_DoesNotHandOverItsArena()
        {
            var missions = new[]
            {
                Mission("mission.a1", "chapter.a", "arena.alpha"),
                Mission("mission.a2", "chapter.a", "arena.alpha")
            };

            var unlocked = CampaignArenaUnlockService.ResolveUnlockedArenas(missions, new[] { "mission.a1" });

            Assert.That(unlocked, Is.Empty);
        }

        [Test]
        public void ClearedChapter_UnlocksEveryArenaItUses()
        {
            var missions = new[]
            {
                Mission("mission.a1", "chapter.a", "arena.alpha"),
                Mission("mission.a2", "chapter.a", "arena.beta"),
                Mission("mission.b1", "chapter.b", "arena.gamma")
            };

            var unlocked = CampaignArenaUnlockService.ResolveUnlockedArenas(
                missions,
                new[] { "mission.a1", "mission.a2" });

            Assert.That(unlocked, Is.EqualTo(new[] { "arena.alpha", "arena.beta" }));
        }

        [Test]
        public void TrainingArena_NeverJoinsThePool()
        {
            var missions = new[]
            {
                Mission("mission.t1", "chapter.t", CampaignArenaUnlockService.TrainingArenaId),
                Mission("mission.t2", "chapter.t", "arena.alpha")
            };

            var unlocked = CampaignArenaUnlockService.ResolveUnlockedArenas(
                missions,
                new[] { "mission.t1", "mission.t2" });

            Assert.That(unlocked, Is.EqualTo(new[] { "arena.alpha" }));
        }

        [Test]
        public void PoolOrder_FollowsTheMissionListSoItIsStable()
        {
            var missions = new[]
            {
                Mission("mission.a1", "chapter.a", "arena.beta"),
                Mission("mission.a2", "chapter.a", "arena.alpha"),
                Mission("mission.a3", "chapter.a", "arena.beta")
            };

            var unlocked = CampaignArenaUnlockService.ResolveUnlockedArenas(
                missions,
                new[] { "mission.a1", "mission.a2", "mission.a3" });

            Assert.That(unlocked, Is.EqualTo(new[] { "arena.beta", "arena.alpha" }));
        }

        [Test]
        public void ClearingTheFirstShippedChapter_OpensTheDesertArena()
        {
            var content = DefaultGameContentCatalog.Create();
            var completed = new List<string>();
            foreach (var mission in content.Missions)
            {
                if (mission != null && mission.ChapterId == "chapter.basics")
                {
                    completed.Add(mission.MissionId);
                }
            }

            var unlocked = CampaignArenaUnlockService.ResolveUnlockedArenas(content.Missions, completed);

            Assert.That(unlocked, Is.EqualTo(new[] { "arena.desert-sci-fi" }));
        }

        private static MissionDefinition Mission(string missionId, string chapterId, string arenaId)
        {
            return MissionDefinition.CreateRuntime(
                missionId,
                missionId,
                missionId,
                0,
                null,
                null,
                new MissionRewardDefinition(0),
                chapterId: chapterId,
                arenaId: arenaId,
                enemyLoadoutId: "enemy.training.direct",
                difficultyProfileId: "difficulty.normal");
        }
    }
}
