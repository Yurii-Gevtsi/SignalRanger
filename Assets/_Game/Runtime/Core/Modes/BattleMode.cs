namespace Game.Core.Modes
{
    /// <summary>
    /// Which of the game's battle modes the session is about to play. Replaces the
    /// pair of implicit signals the battle scene used to read (a training flag plus
    /// "is the mission id empty"), which stopped scaling once raid and duel joined
    /// story and training.
    /// </summary>
    public enum BattleMode
    {
        /// <summary>Campaign mission: advances progression, grants first-completion rewards.</summary>
        Story = 0,

        /// <summary>Tutorial sandbox on the training arena; no progression, no economy.</summary>
        Training = 1,

        /// <summary>Offline PvE on a liberated planet: player picks the difficulty.</summary>
        Raid = 2,

        /// <summary>Online 1v1 against another commander; difficulty comes from the match.</summary>
        Pvp = 3
    }

    public static class BattleModeRules
    {
        /// <summary>Only story battles feed campaign progression and mission objectives.</summary>
        public static bool IsStory(this BattleMode mode) => mode == BattleMode.Story;

        /// <summary>Repeatable non-story combat, counted as "arena" by quests and activities.</summary>
        public static bool IsArena(this BattleMode mode) => mode == BattleMode.Raid || mode == BattleMode.Pvp;

        /// <summary>
        /// Support drops hand out random weapons and currency mid-battle, which is fine
        /// against the AI and unacceptable against a real opponent.
        /// </summary>
        public static bool AllowsSupportDrops(this BattleMode mode) => mode != BattleMode.Pvp;
    }
}
