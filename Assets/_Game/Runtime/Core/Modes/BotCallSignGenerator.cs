namespace Game.Core.Modes
{
    /// <summary>
    /// Builds a commander call sign for a backfilled duel opponent.
    /// <para>
    /// These are player-style handles, not interface copy: a nickname is a proper
    /// name and is never translated, which is why the word lists live here rather
    /// than in the localization dictionaries. Nothing here is ever shown as a
    /// sentence to the player.
    /// </para>
    /// </summary>
    public static class BotCallSignGenerator
    {
        private static readonly string[] Prefixes =
        {
            "Genuine", "Silent", "Iron", "Rapid", "Quiet", "Solar", "Hollow", "Bright",
            "Rogue", "Steady", "Distant", "Sharp", "Cobalt", "Wired", "Amber", "Frozen",
            "Lucky", "Grim", "Swift", "Crimson", "Pale", "Restless", "Vivid", "Stray"
        };

        private static readonly string[] Suffixes =
        {
            "Ranger", "Vector", "Drifter", "Warden", "Signal", "Pilot", "Nomad", "Sentry",
            "Runner", "Beacon", "Vandal", "Scout", "Anchor", "Falcon", "Marshal", "Comet",
            "Hunter", "Pioneer", "Raven", "Tracer", "Voyager", "Gunner", "Lancer", "Echo"
        };

        /// <summary>
        /// A stable call sign for <paramref name="seed"/>. Roughly half carry a
        /// trailing number, because real handles do - a queue where every name is a
        /// clean two-word pair reads as generated.
        /// </summary>
        public static string Generate(string seed)
        {
            var hash = DeterministicSeed.Hash(seed);
            var prefix = Prefixes[DeterministicSeed.Index(hash, Prefixes.Length)];
            var suffix = Suffixes[DeterministicSeed.Index(hash >> 8, Suffixes.Length)];
            var name = prefix + suffix;

            var numberRoll = DeterministicSeed.Index(hash >> 16, 100);
            if (numberRoll >= 50)
            {
                var number = DeterministicSeed.Index(hash >> 20, 89) + 11; // 11..99
                return name + number;
            }

            return name;
        }
    }
}
