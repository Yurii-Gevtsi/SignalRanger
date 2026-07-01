namespace Game.Gameplay.Battle.Domain
{
    public interface ITurnService
    {
        void StartFirstTurn(BattleState state, BattleTeamId startingTeam);
        void SpendAction(BattleState state);
        void AdvanceTurn(BattleState state);
    }
}
