namespace Game.Gameplay.Battle.Domain
{
    public readonly struct TurnState
    {
        public TurnState(BattleTeamId activeTeam, string activeUnitId, int turnNumber, bool actionSpent)
        {
            ActiveTeam = activeTeam;
            ActiveUnitId = activeUnitId;
            TurnNumber = turnNumber;
            ActionSpent = actionSpent;
        }

        public BattleTeamId ActiveTeam { get; }
        public string ActiveUnitId { get; }
        public int TurnNumber { get; }
        public bool ActionSpent { get; }

        public TurnState SpendAction()
        {
            return new TurnState(ActiveTeam, ActiveUnitId, TurnNumber, true);
        }
    }
}
