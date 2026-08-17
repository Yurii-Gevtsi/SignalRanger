namespace Game.Progression.Missions
{
    /// <summary>
    /// Extra condition a mission asks for on top of winning the battle. Missions with
    /// <see cref="None"/> only need a win, which is how every mission behaved before
    /// objectives existed.
    /// </summary>
    public enum MissionObjectiveKind
    {
        None = 0,

        /// <summary>Finish the battle still holding at least a share of your health.</summary>
        KeepHealthPercent,

        /// <summary>Put the opponent out of bounds - over the edge or into the kill zone.</summary>
        RingOut
    }

    /// <summary>
    /// What a battle actually ended up looking like, gathered when the simulation
    /// finishes so the objective can be judged after the fact. Only facts the battle
    /// layer can state plainly are here; anything needing damage attribution is not.
    /// </summary>
    public readonly struct BattleObjectiveFacts
    {
        public BattleObjectiveFacts(bool playerWon, int playerHealthPercent, bool opponentRingOut)
        {
            PlayerWon = playerWon;
            PlayerHealthPercent = playerHealthPercent < 0 ? 0 : playerHealthPercent > 100 ? 100 : playerHealthPercent;
            OpponentRingOut = opponentRingOut;
        }

        public bool PlayerWon { get; }
        public int PlayerHealthPercent { get; }
        public bool OpponentRingOut { get; }

        public static BattleObjectiveFacts None => new(false, 0, false);
    }

    public readonly struct MissionObjective
    {
        public MissionObjective(MissionObjectiveKind kind, int threshold = 0)
        {
            Kind = kind;
            Threshold = threshold;
        }

        public MissionObjectiveKind Kind { get; }

        /// <summary>Percentage for <see cref="MissionObjectiveKind.KeepHealthPercent"/>.</summary>
        public int Threshold { get; }

        public static MissionObjective None => new(MissionObjectiveKind.None);

        /// <summary>
        /// A mission is only cleared when the battle was won. A lost battle never counts,
        /// even if the objective itself happened to be satisfied along the way.
        /// </summary>
        public bool IsSatisfiedBy(in BattleObjectiveFacts facts)
        {
            if (!facts.PlayerWon)
            {
                return false;
            }

            return Kind switch
            {
                MissionObjectiveKind.KeepHealthPercent => facts.PlayerHealthPercent >= Threshold,
                MissionObjectiveKind.RingOut => facts.OpponentRingOut,
                _ => true
            };
        }
    }
}
