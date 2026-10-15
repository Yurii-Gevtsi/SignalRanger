using System.Collections.Generic;
using Game.App.Content;
using Game.Core.Modes;
using Game.Data.Progression;
using Game.Infrastructure;
using Game.Progression.Modes;
using NUnit.Framework;

namespace Game.Tests.EditMode.Modes
{
    public sealed class PvpMatchmakingTests
    {
        private static readonly string[] Pool = { "arena.desert-sci-fi" };
        private const long PlayerPower = 5000;
        private const int PlayerLevel = 9;

        [Test]
        public void ACommanderWithNoLiberatedPlanetIsTurnedAwayWithAReason()
        {
            var service = new LocalPvpMatchmakingService(0f, () => 1);
            MatchmakingResult result = default;

            service.FindOpponent(Request(new string[0]), value => result = value);

            Assert.That(result.Matched, Is.False);
            Assert.That(result.Denial, Is.EqualTo(MatchmakingDenial.NoUnlockedArena));
            Assert.That(service.IsSearching, Is.False);
        }

        [Test]
        public void TheQueueWaitsTheFullFortyFiveSecondsBeforeFillingTheMatch()
        {
            var service = new LocalPvpMatchmakingService();
            MatchmakingResult result = default;

            service.FindOpponent(Request(Pool), value => result = value);
            Assert.That(service.IsSearching, Is.True);

            // One tick short of the authored wait: still looking for a live commander.
            service.Tick(PvpMatchmakingPolicy.BackfillAfterSeconds - 1f);
            Assert.That(result.Matched, Is.False);
            Assert.That(service.IsSearching, Is.True);

            service.Tick(1f);
            Assert.That(result.Matched, Is.True);
            Assert.That(service.IsSearching, Is.False);
        }

        [Test]
        public void TheSearchClockIsReadableWhileTheQueueRuns()
        {
            var service = new LocalPvpMatchmakingService();
            service.FindOpponent(Request(Pool), _ => { });

            service.Tick(5f);
            service.Tick(3f);

            Assert.That(service.SearchElapsedSeconds, Is.EqualTo(8f).Within(0.001f));
        }

        [Test]
        public void ABackfilledOpponentIsIndistinguishableFromACommander()
        {
            var service = new LocalPvpMatchmakingService(0f, () => 1);
            MatchmakingResult result = default;

            service.FindOpponent(Request(Pool), value => result = value);

            Assert.That(result.Matched, Is.True);
            Assert.That(result.Opponent.DisplayName, Is.Not.Empty);
            Assert.That(result.Opponent.LoadoutWeaponIds, Is.Not.Empty);
            // The flag exists for telemetry and rating, never for the duel screen.
            Assert.That(result.Opponent.IsBotBackfill, Is.True);
        }

        [Test]
        public void ABackfilledOpponentStaysInsideTheAuthoredPowerBand()
        {
            for (var search = 0; search < 40; search++)
            {
                var seed = search;
                var service = new LocalPvpMatchmakingService(0f, () => seed);
                MatchmakingResult result = default;
                service.FindOpponent(Request(Pool), value => result = value);

                var opponent = result.Opponent;
                Assert.That(
                    PvpMatchmakingPolicy.IsInBand(PlayerPower, opponent.HistoricalPower, 0f),
                    Is.True,
                    $"power {opponent.HistoricalPower} outside the opening band");
                Assert.That(opponent.CommanderLevel, Is.InRange(
                    PlayerLevel - PvpBotOpponentFactory.LevelSpread,
                    PlayerLevel + PvpBotOpponentFactory.LevelSpread));
            }
        }

        [Test]
        public void ACancelledSearchNeverCallsBack()
        {
            var service = new LocalPvpMatchmakingService();
            var called = false;
            service.FindOpponent(Request(Pool), _ => called = true);

            service.CancelSearch();
            service.Tick(PvpMatchmakingPolicy.BackfillAfterSeconds * 2f);

            Assert.That(called, Is.False);
            Assert.That(service.IsSearching, Is.False);
        }

        [Test]
        public void AMatchTurnsIntoADuelOnASharedArena()
        {
            var setupService = new PvpSetupService(DefaultGameContentCatalog.Create().Missions);
            var service = new LocalPvpMatchmakingService(0f, () => 3);
            var request = Request(Pool);
            MatchmakingResult result = default;
            service.FindOpponent(request, value => result = value);

            var setup = setupService.CreateSetup(request, result);

            Assert.That(setup.IsAvailable, Is.True);
            Assert.That(setup.ArenaId, Is.EqualTo("arena.desert-sci-fi"));
            Assert.That(setup.MatchId, Is.EqualTo(result.MatchId));
            Assert.That(setup.OpponentWeaponIds, Is.Not.Empty);
            Assert.That(setup.OpponentWeaponRanks.Length, Is.EqualTo(setup.OpponentWeaponIds.Length));
        }

        [Test]
        public void ADeniedMatchNeverProducesABattle()
        {
            var setupService = new PvpSetupService(DefaultGameContentCatalog.Create().Missions);

            var setup = setupService.CreateSetup(
                Request(Pool),
                MatchmakingResult.Denied(MatchmakingDenial.NoOpponentFound));

            Assert.That(setup.IsAvailable, Is.False);
            Assert.That(setup.Opponent, Is.Null);
        }

        [Test]
        public void ADuelIsRefusedWhenTheTwoSidesShareNoArena()
        {
            var setupService = new PvpSetupService(DefaultGameContentCatalog.Create().Missions);
            var opponent = Opponent(new[] { "arena.late-game" }, new[] { "weapon.impulse" }, new[] { 1 });

            var setup = setupService.CreateSetup(Request(Pool), MatchmakingResult.Success("match.1", opponent));

            Assert.That(setup.IsAvailable, Is.False);
        }

        [Test]
        public void AnOpponentsOwnRanksAreKeptRatherThanRecomputed()
        {
            var setupService = new PvpSetupService(DefaultGameContentCatalog.Create().Missions);
            var opponent = Opponent(Pool, new[] { "weapon.granata", "weapon.impulse" }, new[] { 4, 2 });

            var setup = setupService.CreateSetup(Request(Pool), MatchmakingResult.Success("match.1", opponent));

            Assert.That(setup.OpponentWeaponRanks, Is.EqualTo(new[] { 4, 2 }));
        }

        [Test]
        public void MissingRanksAreResolvedFromTheOpponentsLevel()
        {
            var setupService = new PvpSetupService(DefaultGameContentCatalog.Create().Missions);
            var opponent = Opponent(Pool, new[] { "weapon.granata" }, new int[0]);

            var setup = setupService.CreateSetup(Request(Pool), MatchmakingResult.Success("match.1", opponent));

            Assert.That(setup.OpponentWeaponRanks.Length, Is.EqualTo(1));
            Assert.That(setup.OpponentWeaponRanks[0], Is.GreaterThan(1));
        }

        private static PvpOpponentProfile Opponent(string[] arenas, string[] weaponIds, int[] ranks)
        {
            return new PvpOpponentProfile(
                "other", "RivalCommander", 20, 20000, arenas, weaponIds, ranks, null, "archetype.striker", false);
        }

        private static MatchmakingRequest Request(IReadOnlyList<string> arenas)
        {
            var pool = new string[arenas.Count];
            for (var i = 0; i < arenas.Count; i++)
            {
                pool[i] = arenas[i];
            }

            return new MatchmakingRequest(
                "local",
                "Commander",
                PlayerLevel,
                PlayerPower,
                pool,
                new[] { "weapon.impulse", "weapon.granata" });
        }
    }
}
