using System.Linq;

namespace Game.Gameplay.Battle.Domain
{
    public sealed class TurnService : ITurnService
    {
        public void StartFirstTurn(BattleState state, BattleTeamId startingTeam)
        {
            var activeTeam = HasLivingUnits(state, startingTeam)
                ? startingTeam
                : GetOpposingTeam(startingTeam);

            state.SetTurn(CreateTurn(state, activeTeam, 1));
        }

        public void SpendAction(BattleState state)
        {
            if (!state.Turn.ActionSpent)
            {
                state.SetTurn(state.Turn.SpendAction());
            }
        }

        public void AdvanceTurn(BattleState state)
        {
            var nextTeam = GetOpposingTeam(state.Turn.ActiveTeam);
            if (!HasLivingUnits(state, nextTeam))
            {
                nextTeam = state.Turn.ActiveTeam;
            }

            state.SetTurn(CreateTurn(state, nextTeam, state.Turn.TurnNumber + 1));
        }

        private static TurnState CreateTurn(BattleState state, BattleTeamId activeTeam, int turnNumber)
        {
            var activeUnit = state
                .GetLivingUnits(activeTeam)
                .FirstOrDefault();

            return new TurnState(
                activeTeam,
                activeUnit?.UnitId ?? string.Empty,
                turnNumber,
                false);
        }

        private static bool HasLivingUnits(BattleState state, BattleTeamId team)
        {
            return state.GetTeam(team).HasLivingUnits;
        }

        private static BattleTeamId GetOpposingTeam(BattleTeamId team)
        {
            return team == BattleTeamId.Player
                ? BattleTeamId.Opponent
                : BattleTeamId.Player;
        }
    }
}
