using Game.Core.Modes;
using NUnit.Framework;

namespace Game.Tests.EditMode.Modes
{
    public sealed class PvpArenaSelectorTests
    {
        private static readonly string[] EarlyPool = { "arena.alpha" };
        private static readonly string[] LatePool = { "arena.alpha", "arena.beta", "arena.gamma" };

        [Test]
        public void TheLowerCommandersProgressCapsWhatBothSidesSee()
        {
            var eligible = PvpArenaSelector.ResolveEligibleArenas(
                4, 900, "low", EarlyPool,
                21, 9000, "high", LatePool);

            Assert.That(eligible, Is.EqualTo(new[] { "arena.alpha" }));
        }

        [Test]
        public void ArenaOrderFollowsTheLowerCommandersPool()
        {
            var lower = new[] { "arena.gamma", "arena.alpha" };
            var higher = new[] { "arena.alpha", "arena.beta", "arena.gamma" };

            var eligible = PvpArenaSelector.ResolveEligibleArenas(
                5, 100, "low", lower,
                9, 900, "high", higher);

            Assert.That(eligible, Is.EqualTo(new[] { "arena.gamma", "arena.alpha" }));
        }

        [Test]
        public void ArgumentOrderDoesNotChangeTheEligiblePool()
        {
            var oneWay = PvpArenaSelector.ResolveEligibleArenas(
                4, 900, "low", EarlyPool,
                21, 9000, "high", LatePool);
            var otherWay = PvpArenaSelector.ResolveEligibleArenas(
                21, 9000, "high", LatePool,
                4, 900, "low", EarlyPool);

            Assert.That(otherWay, Is.EqualTo(oneWay));
        }

        [Test]
        public void ACommanderWithNoLiberatedPlanetCannotDuel()
        {
            var eligible = PvpArenaSelector.ResolveEligibleArenas(
                1, 0, "fresh", new string[0],
                21, 9000, "high", LatePool);

            Assert.That(eligible, Is.Empty);
            Assert.That(PvpArenaSelector.SelectArena("match.1", eligible), Is.Empty);
        }

        [Test]
        public void DisjointPools_ShareNoArena()
        {
            var eligible = PvpArenaSelector.ResolveEligibleArenas(
                5, 100, "a", new[] { "arena.alpha" },
                9, 900, "b", new[] { "arena.beta" });

            Assert.That(eligible, Is.Empty);
        }

        [Test]
        public void EqualLevels_FallBackToPowerThenToPlayerId()
        {
            Assert.That(PvpArenaSelector.IsLower(7, 100, "a", 7, 200, "b"), Is.True);
            Assert.That(PvpArenaSelector.IsLower(7, 300, "a", 7, 200, "b"), Is.False);
            Assert.That(PvpArenaSelector.IsLower(7, 200, "a", 7, 200, "b"), Is.True);
            Assert.That(PvpArenaSelector.IsLower(7, 200, "b", 7, 200, "a"), Is.False);
        }

        [Test]
        public void BothClientsDeriveTheSameArenaFromTheMatchId()
        {
            var first = PvpArenaSelector.SelectArena("match.abc", LatePool);
            var second = PvpArenaSelector.SelectArena("match.abc", LatePool);

            Assert.That(second, Is.EqualTo(first));
            Assert.That(LatePool, Contains.Item(first));
        }

        [Test]
        public void DifferentMatchesDoNotAllLandOnTheSameArena()
        {
            var seen = new System.Collections.Generic.HashSet<string>();
            for (var i = 0; i < 60; i++)
            {
                seen.Add(PvpArenaSelector.SelectArena($"match.{i}", LatePool));
            }

            Assert.That(seen.Count, Is.GreaterThan(1));
        }
    }
}
