namespace Game.Gameplay.Battle.Domain
{
    public interface ITurnService
    {
        void StartFirstTurn(BattleState state, BattleTeamId startingTeam);
        void SpendAction(BattleState state);
        void AdvanceTurn(BattleState state);

        /// <summary>
        /// Hands the rest of the current turn to another living droid of the active team: same turn number, the
        /// action not spent, and the team's rotation continues from that droid.
        /// </summary>
        bool TransferTurn(BattleState state, string unitId);
    }
}
