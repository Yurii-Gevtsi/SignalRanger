using Game.Gameplay.Battle.Domain;
using NUnit.Framework;
using UnityEngine;

namespace Game.Tests.EditMode.Gameplay
{
    public sealed class BattleResultEvaluatorTests
    {
        [Test]
        public void Evaluate_ReturnsInProgress_WhenBothTeamsHaveLivingUnits()
        {
            var state = CreateBattleState();

            var result = BattleResultEvaluator.Evaluate(state);

            Assert.AreEqual(BattleOutcome.InProgress, result.Outcome);
            Assert.IsFalse(result.IsFinished);
        }

        [Test]
        public void Evaluate_ReturnsPlayerWon_WhenOpponentIsDefeated()
        {
            var state = CreateBattleState();
            state.GetUnit("opponent-1").ApplyDamage(100, "test");

            var result = BattleResultEvaluator.Evaluate(state);

            Assert.AreEqual(BattleOutcome.PlayerWon, result.Outcome);
            Assert.AreEqual(BattleTeamId.Player, result.WinningTeam);
        }

        [Test]
        public void Evaluate_ReturnsDraw_WhenBothTeamsAreDefeated()
        {
            var state = CreateBattleState();
            state.GetUnit("player-1").ApplyDamage(100, "test");
            state.GetUnit("opponent-1").ApplyDamage(100, "test");

            var result = BattleResultEvaluator.Evaluate(state);

            Assert.AreEqual(BattleOutcome.Draw, result.Outcome);
            Assert.AreEqual(BattleTeamId.None, result.WinningTeam);
        }

        private static BattleState CreateBattleState()
        {
            var playerUnit = new BattleUnitState("player-1", BattleTeamId.Player, new Vector2(-2f, 0f), 100);
            var opponentUnit = new BattleUnitState("opponent-1", BattleTeamId.Opponent, new Vector2(2f, 0f), 100);

            var playerTeam = new BattleTeamState(BattleTeamId.Player, new[] { playerUnit });
            var opponentTeam = new BattleTeamState(BattleTeamId.Opponent, new[] { opponentUnit });
            var turn = new TurnState(BattleTeamId.Player, playerUnit.UnitId, 1, false);

            return new BattleState(new[] { playerTeam, opponentTeam }, turn);
        }
    }
}
