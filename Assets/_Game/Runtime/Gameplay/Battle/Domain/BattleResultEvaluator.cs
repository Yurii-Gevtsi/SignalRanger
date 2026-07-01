namespace Game.Gameplay.Battle.Domain
{
    public static class BattleResultEvaluator
    {
        public static BattleResult Evaluate(BattleState state)
        {
            var playerAlive = state.GetTeam(BattleTeamId.Player).HasLivingUnits;
            var opponentAlive = state.GetTeam(BattleTeamId.Opponent).HasLivingUnits;

            if (playerAlive && opponentAlive)
            {
                return BattleResult.InProgress;
            }

            if (!playerAlive && !opponentAlive)
            {
                return BattleResult.Draw;
            }

            return playerAlive
                ? BattleResult.Win(BattleTeamId.Player)
                : BattleResult.Win(BattleTeamId.Opponent);
        }
    }
}
