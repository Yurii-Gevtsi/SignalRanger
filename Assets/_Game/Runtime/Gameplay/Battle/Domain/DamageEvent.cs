namespace Game.Gameplay.Battle.Domain
{
    public readonly struct DamageEvent
    {
        public DamageEvent(
            string targetUnitId,
            BattleTeamId targetTeam,
            string sourceId,
            int damage,
            int healthBefore,
            int healthAfter)
        {
            TargetUnitId = targetUnitId;
            TargetTeam = targetTeam;
            SourceId = sourceId;
            Damage = damage;
            HealthBefore = healthBefore;
            HealthAfter = healthAfter;
        }

        public string TargetUnitId { get; }
        public BattleTeamId TargetTeam { get; }
        public string SourceId { get; }
        public int Damage { get; }
        public int HealthBefore { get; }
        public int HealthAfter { get; }
        public bool DefeatedTarget => HealthBefore > 0 && HealthAfter <= 0;
    }
}
