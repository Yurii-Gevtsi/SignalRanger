using Game.Gameplay.Battle.Domain;
using NUnit.Framework;
using UnityEngine;

namespace Game.Tests.EditMode.Gameplay
{
    public sealed class TurnServiceTests
    {
        [Test]
        public void StartFirstTurn_ActivatesRequestedLivingTeam()
        {
            var state = CreateBattleState();
            var turns = new TurnService();

            turns.StartFirstTurn(state, BattleTeamId.Player);

            Assert.AreEqual(BattleTeamId.Player, state.Turn.ActiveTeam);
            Assert.AreEqual("player-1", state.Turn.ActiveUnitId);
            Assert.AreEqual(1, state.Turn.TurnNumber);
            Assert.IsFalse(state.Turn.ActionSpent);
        }

        [Test]
        public void SpendAction_MarksCurrentTurnActionSpent()
        {
            var state = CreateBattleState();
            var turns = new TurnService();
            turns.StartFirstTurn(state, BattleTeamId.Player);

            turns.SpendAction(state);

            Assert.IsTrue(state.Turn.ActionSpent);
        }

        [Test]
        public void AdvanceTurn_AlternatesToOpponent()
        {
            var state = CreateBattleState();
            var turns = new TurnService();
            turns.StartFirstTurn(state, BattleTeamId.Player);

            turns.AdvanceTurn(state);

            Assert.AreEqual(BattleTeamId.Opponent, state.Turn.ActiveTeam);
            Assert.AreEqual("opponent-1", state.Turn.ActiveUnitId);
            Assert.AreEqual(2, state.Turn.TurnNumber);
            Assert.IsFalse(state.Turn.ActionSpent);
        }

        [Test]
        public void AdvanceTurn_SkipsDefeatedUnitOnActiveTeam()
        {
            var state = CreateBattleStateWithTwoOpponentUnits();
            var turns = new TurnService();
            state.GetUnit("opponent-1").ApplyDamage(100, "test");
            turns.StartFirstTurn(state, BattleTeamId.Player);

            turns.AdvanceTurn(state);

            Assert.AreEqual(BattleTeamId.Opponent, state.Turn.ActiveTeam);
            Assert.AreEqual("opponent-2", state.Turn.ActiveUnitId);
        }

        private static BattleState CreateBattleState()
        {
            var playerUnit = new BattleUnitState("player-1", BattleTeamId.Player, new Vector2(-2f, 0f), 100);
            var opponentUnit = new BattleUnitState("opponent-1", BattleTeamId.Opponent, new Vector2(2f, 0f), 100);

            var playerTeam = new BattleTeamState(BattleTeamId.Player, new[] { playerUnit });
            var opponentTeam = new BattleTeamState(BattleTeamId.Opponent, new[] { opponentUnit });
            var turn = new TurnState(BattleTeamId.None, string.Empty, 0, false);

            return new BattleState(new[] { playerTeam, opponentTeam }, turn);
        }

        private static BattleState CreateBattleStateWithTwoOpponentUnits()
        {
            var playerUnit = new BattleUnitState("player-1", BattleTeamId.Player, new Vector2(-2f, 0f), 100);
            var opponentUnitA = new BattleUnitState("opponent-1", BattleTeamId.Opponent, new Vector2(2f, 0f), 100);
            var opponentUnitB = new BattleUnitState("opponent-2", BattleTeamId.Opponent, new Vector2(3f, 0f), 100);

            var playerTeam = new BattleTeamState(BattleTeamId.Player, new[] { playerUnit });
            var opponentTeam = new BattleTeamState(BattleTeamId.Opponent, new[] { opponentUnitA, opponentUnitB });
            var turn = new TurnState(BattleTeamId.None, string.Empty, 0, false);

            return new BattleState(new[] { playerTeam, opponentTeam }, turn);
        }
    }
}
