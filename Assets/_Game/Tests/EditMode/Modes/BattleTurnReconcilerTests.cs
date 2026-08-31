using System.Collections.Generic;
using Game.Gameplay.Battle.Domain;
using Game.Gameplay.Battle.Net;
using NUnit.Framework;
using UnityEngine;

namespace Game.Tests.EditMode.Modes
{
    public sealed class BattleTurnReconcilerTests
    {
        [Test]
        public void ASnapshotDescribesEveryUnitInAStableOrder()
        {
            var state = CreateState();

            var snapshot = BattleTurnSnapshot.Capture(state, new List<BattleCraterSnapshot>());

            Assert.That(snapshot.TurnNumber, Is.EqualTo(4));
            Assert.That(snapshot.ActiveUnitId, Is.EqualTo("unit.player"));
            Assert.That(snapshot.Units.Count, Is.EqualTo(2));
            // Ordered by id so two clients hash the same turn identically.
            Assert.That(snapshot.Units[0].UnitId, Is.EqualTo("unit.opponent"));
            Assert.That(snapshot.Units[1].UnitId, Is.EqualTo("unit.player"));
        }

        [Test]
        public void AClientThatAlreadyAgreesIsLeftAlone()
        {
            var state = CreateState();
            var authoritative = BattleTurnSnapshot.Capture(state, new List<BattleCraterSnapshot>());

            var result = BattleTurnReconciler.Apply(state, null, authoritative);

            Assert.That(result.Diverged, Is.False);
            Assert.That(result.CorrectedUnits, Is.EqualTo(0));
        }

        [Test]
        public void SubTolerancePositionDriftIsNotWorthCorrecting()
        {
            var state = CreateState();
            var authoritative = SnapshotWith("unit.player", new Vector2(-4.005f, 1f), 100);

            var result = BattleTurnReconciler.Apply(state, null, authoritative);

            Assert.That(result.CorrectedUnits, Is.EqualTo(0));
            Assert.That(state.GetUnit("unit.player").Position.x, Is.EqualTo(-4f).Within(0.0001f));
        }

        [Test]
        public void ARealPositionDisagreementIsPulledToTheHost()
        {
            var state = CreateState();
            var authoritative = SnapshotWith("unit.player", new Vector2(-2.5f, 1f), 100);

            var result = BattleTurnReconciler.Apply(state, null, authoritative);

            Assert.That(result.CorrectedUnits, Is.EqualTo(1));
            Assert.That(result.Diverged, Is.True);
            Assert.That(state.GetUnit("unit.player").Position.x, Is.EqualTo(-2.5f).Within(0.0001f));
        }

        [Test]
        public void AClientThatMissedDamageTakesIt()
        {
            var state = CreateState();
            var authoritative = SnapshotWith("unit.player", new Vector2(-4f, 1f), 60);

            BattleTurnReconciler.Apply(state, null, authoritative);

            Assert.That(state.GetUnit("unit.player").Health, Is.EqualTo(60));
        }

        [Test]
        public void AClientThatOverAppliedDamageIsHealedBackToTheHostsNumber()
        {
            var state = CreateState();
            state.GetUnit("unit.player").ApplyDamage(40, "local");
            var authoritative = SnapshotWith("unit.player", new Vector2(-4f, 1f), 100);

            BattleTurnReconciler.Apply(state, null, authoritative);

            Assert.That(state.GetUnit("unit.player").Health, Is.EqualTo(100));
        }

        [Test]
        public void FacingFollowsTheHostToo()
        {
            var state = CreateState();
            var authoritative = new BattleTurnSnapshot(
                4,
                "unit.player",
                new List<BattleUnitSnapshot> { new("unit.player", new Vector2(-4f, 1f), 100, -1) },
                new List<BattleCraterSnapshot>());

            var result = BattleTurnReconciler.Apply(state, null, authoritative);

            Assert.That(state.GetUnit("unit.player").FacingDirectionX, Is.EqualTo(-1));
            Assert.That(result.CorrectedUnits, Is.EqualTo(1));
        }

        [Test]
        public void AUnitTheClientDoesNotHaveIsIgnoredRatherThanFatal()
        {
            var state = CreateState();
            var authoritative = SnapshotWith("unit.ghost", Vector2.zero, 50);

            Assert.That(() => BattleTurnReconciler.Apply(state, null, authoritative), Throws.Nothing);
        }

        [Test]
        public void ANullSnapshotChangesNothing()
        {
            var state = CreateState();

            var result = BattleTurnReconciler.Apply(state, null, null);

            Assert.That(result.Diverged, Is.False);
            Assert.That(state.GetUnit("unit.player").Health, Is.EqualTo(100));
        }

        private static BattleTurnSnapshot SnapshotWith(string unitId, Vector2 position, int health)
        {
            return new BattleTurnSnapshot(
                4,
                "unit.player",
                new List<BattleUnitSnapshot> { new(unitId, position, health, 1) },
                new List<BattleCraterSnapshot>());
        }

        private static BattleState CreateState()
        {
            var player = new BattleUnitState("unit.player", BattleTeamId.Player, new Vector2(-4f, 1f), 100);
            var opponent = new BattleUnitState("unit.opponent", BattleTeamId.Opponent, new Vector2(4f, 1f), 100);
            var teams = new[]
            {
                new BattleTeamState(BattleTeamId.Player, new[] { player, opponent }),
                new BattleTeamState(BattleTeamId.Opponent, new[] { player, opponent })
            };
            return new BattleState(teams, new TurnState(BattleTeamId.Player, "unit.player", 4, false));
        }
    }
}
