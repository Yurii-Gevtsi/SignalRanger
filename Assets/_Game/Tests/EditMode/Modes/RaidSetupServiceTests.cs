using System.Collections.Generic;
using Game.App.Content;
using Game.Data.Progression;
using Game.Progression.Missions;
using Game.Progression.Modes;
using NUnit.Framework;

namespace Game.Tests.EditMode.Modes
{
    public sealed class RaidSetupServiceTests
    {
        [Test]
        public void WithNoLiberatedPlanet_TheRaidIsUnavailable()
        {
            var service = new RaidSetupService(DefaultGameContentCatalog.Create().Missions);

            var setup = service.CreateSetup(new List<string>(), 5, RaidDifficulty.Normal, "seed");

            Assert.That(setup.IsAvailable, Is.False);
            Assert.That(setup.ArenaId, Is.Empty);
            Assert.That(setup.OpponentWeaponIds, Is.Empty);
        }

        [Test]
        public void AfterTheFirstChapter_TheRaidRunsOnItsArena()
        {
            var content = DefaultGameContentCatalog.Create();
            var service = new RaidSetupService(content.Missions);

            var setup = service.CreateSetup(ClearedBasics(content), 5, RaidDifficulty.Normal, "seed");

            Assert.That(setup.IsAvailable, Is.True);
            Assert.That(setup.ArenaId, Is.EqualTo("arena.desert-sci-fi"));
            Assert.That(setup.OpponentWeaponIds, Is.Not.Empty);
        }

        [Test]
        public void TheArenaIsAlwaysDrawnFromTheLiberatedPool()
        {
            var content = DefaultGameContentCatalog.Create();
            var service = new RaidSetupService(content.Missions);
            var completed = ClearedBasics(content);
            var unlocked = service.GetUnlockedArenas(completed);

            for (var attempt = 0; attempt < 25; attempt++)
            {
                var setup = service.CreateSetup(completed, 5, RaidDifficulty.Normal, $"seed.{attempt}");

                Assert.That(unlocked, Contains.Item(setup.ArenaId));
            }
        }

        [Test]
        public void EveryRaidIsFoughtAgainstSigma()
        {
            var content = DefaultGameContentCatalog.Create();
            var service = new RaidSetupService(content.Missions);

            var setup = service.CreateSetup(ClearedBasics(content), 5, RaidDifficulty.Hard, "seed");

            Assert.That(setup.OpponentNameKey, Is.EqualTo("content.opponent.sigma"));
        }

        [Test]
        public void DifficultyDrivesTheProfileTheRewardTierAndTheRankCap()
        {
            var content = DefaultGameContentCatalog.Create();
            var service = new RaidSetupService(content.Missions);
            var completed = ClearedBasics(content);

            var easy = service.CreateSetup(completed, 30, RaidDifficulty.Easy, "seed");
            var normal = service.CreateSetup(completed, 30, RaidDifficulty.Normal, "seed");
            var hard = service.CreateSetup(completed, 30, RaidDifficulty.Hard, "seed");

            Assert.That(easy.Rules.DifficultyProfileId, Is.EqualTo("difficulty.easy"));
            Assert.That(normal.Rules.DifficultyProfileId, Is.EqualTo("difficulty.normal"));
            Assert.That(hard.Rules.DifficultyProfileId, Is.EqualTo("difficulty.hard"));

            Assert.That(easy.Rules.RewardDifficulty, Is.EqualTo(BattleRewardDifficulty.Easy));
            Assert.That(normal.Rules.RewardDifficulty, Is.EqualTo(BattleRewardDifficulty.Medium));
            Assert.That(hard.Rules.RewardDifficulty, Is.EqualTo(BattleRewardDifficulty.Hard));

            Assert.That(easy.Rules.AiDifficultyTier, Is.EqualTo(0));
            Assert.That(normal.Rules.AiDifficultyTier, Is.EqualTo(1));
            Assert.That(hard.Rules.AiDifficultyTier, Is.EqualTo(2));

            // A veteran commander tops out at the difficulty's cap, never above it.
            Assert.That(MaxRank(easy), Is.EqualTo(3));
            Assert.That(MaxRank(normal), Is.EqualTo(5));
            Assert.That(MaxRank(hard), Is.EqualTo(7));
        }

        [Test]
        public void NoRaidOpponentEverReachesTheBossRanks()
        {
            var content = DefaultGameContentCatalog.Create();
            var service = new RaidSetupService(content.Missions);
            var completed = ClearedBasics(content);

            for (var level = 1; level <= 40; level++)
            {
                var setup = service.CreateSetup(completed, level, RaidDifficulty.Hard, $"seed.{level}");

                Assert.That(MaxRank(setup), Is.LessThanOrEqualTo(7), $"level {level}");
            }
        }

        [Test]
        public void TheSameRequestAlwaysBuildsTheSameRaid()
        {
            var content = DefaultGameContentCatalog.Create();
            var service = new RaidSetupService(content.Missions);
            var completed = ClearedBasics(content);

            var first = service.CreateSetup(completed, 9, RaidDifficulty.Hard, "seed.7");
            var second = service.CreateSetup(completed, 9, RaidDifficulty.Hard, "seed.7");

            Assert.That(second.ArenaId, Is.EqualTo(first.ArenaId));
            Assert.That(second.OpponentWeaponIds, Is.EqualTo(first.OpponentWeaponIds));
            Assert.That(second.OpponentWeaponRanks, Is.EqualTo(first.OpponentWeaponRanks));
        }

        private static int MaxRank(RaidBattleSetup setup)
        {
            var max = 0;
            foreach (var rank in setup.OpponentWeaponRanks)
            {
                if (rank > max)
                {
                    max = rank;
                }
            }

            return max;
        }

        private static List<string> ClearedBasics(RuntimeGameContentCatalog content)
        {
            var completed = new List<string>();
            foreach (var mission in content.Missions)
            {
                if (mission != null && mission.ChapterId == "chapter.basics")
                {
                    completed.Add(mission.MissionId);
                }
            }

            return completed;
        }
    }
}
