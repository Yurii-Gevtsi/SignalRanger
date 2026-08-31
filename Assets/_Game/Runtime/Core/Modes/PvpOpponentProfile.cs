using System;

namespace Game.Core.Modes
{
    /// <summary>
    /// Everything the duel needs to know about the other commander. Kept as plain
    /// values so the same shape works for a real matched player, for a cached
    /// profile and for the bot that backfills an empty queue.
    /// </summary>
    public sealed class PvpOpponentProfile
    {
        public PvpOpponentProfile(
            string playerId,
            string displayName,
            int commanderLevel,
            long historicalPower,
            string[] unlockedArenaIds,
            string[] loadoutWeaponIds,
            int[] loadoutWeaponRanks,
            int[] loadoutWeaponBranches,
            string archetypeId,
            bool isBotBackfill)
        {
            PlayerId = playerId ?? string.Empty;
            DisplayName = displayName ?? string.Empty;
            CommanderLevel = commanderLevel < 1 ? 1 : commanderLevel;
            HistoricalPower = historicalPower < 0 ? 0 : historicalPower;
            UnlockedArenaIds = unlockedArenaIds ?? Array.Empty<string>();
            LoadoutWeaponIds = loadoutWeaponIds ?? Array.Empty<string>();
            LoadoutWeaponRanks = loadoutWeaponRanks ?? Array.Empty<int>();
            LoadoutWeaponBranches = loadoutWeaponBranches ?? Array.Empty<int>();
            ArchetypeId = archetypeId ?? string.Empty;
            IsBotBackfill = isBotBackfill;
        }

        public string PlayerId { get; }

        /// <summary>
        /// The commander's handle. A backfill bot carries a generated call sign here
        /// like any other opponent, so the duel screen has one code path.
        /// </summary>
        public string DisplayName { get; }
        public int CommanderLevel { get; }
        public long HistoricalPower { get; }

        /// <summary>Arenas this commander has liberated; the arena rule intersects both pools.</summary>
        public string[] UnlockedArenaIds { get; }
        public string[] LoadoutWeaponIds { get; }

        /// <summary>Rank per loadout entry, aligned by index with <see cref="LoadoutWeaponIds"/>.</summary>
        public int[] LoadoutWeaponRanks { get; }

        /// <summary>Evolution branch per loadout entry, aligned the same way; 0 below rank 5.</summary>
        public int[] LoadoutWeaponBranches { get; }
        public string ArchetypeId { get; }

        /// <summary>
        /// True when the queue found nobody in time and filled the match with a bot.
        /// <para>
        /// This flag is for the systems behind the battle - telemetry, and any future
        /// rating, which must not move on a backfilled result - and deliberately not
        /// for the duel UI, which presents every opponent the same way.
        /// </para>
        /// </summary>
        public bool IsBotBackfill { get; }
    }
}
