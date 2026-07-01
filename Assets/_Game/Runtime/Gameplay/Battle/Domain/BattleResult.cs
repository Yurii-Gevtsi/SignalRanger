namespace Game.Gameplay.Battle.Domain
{
    public readonly struct BattleResult
    {
        public BattleResult(BattleOutcome outcome, BattleTeamId winningTeam)
        {
            Outcome = outcome;
            WinningTeam = winningTeam;
        }

        public BattleOutcome Outcome { get; }
        public BattleTeamId WinningTeam { get; }
        public bool IsFinished => Outcome != BattleOutcome.InProgress;

        public static BattleResult InProgress => new(BattleOutcome.InProgress, BattleTeamId.None);
        public static BattleResult Draw => new(BattleOutcome.Draw, BattleTeamId.None);

        public static BattleResult Win(BattleTeamId team)
        {
            return team == BattleTeamId.Player
                ? new BattleResult(BattleOutcome.PlayerWon, BattleTeamId.Player)
                : new BattleResult(BattleOutcome.OpponentWon, BattleTeamId.Opponent);
        }
    }
}
