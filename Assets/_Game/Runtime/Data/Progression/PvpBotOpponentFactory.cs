using Game.Core.Modes;
using UnityEngine;

namespace Game.Data.Progression
{
    /// <summary>
    /// Builds the opponent that fills a duel when the queue has nobody to offer.
    /// It is generated to sit inside the same historical-power band a live match
    /// would have used, so a backfilled duel is as fair as a real one.
    /// </summary>
    public static class PvpBotOpponentFactory
    {
        /// <summary>Commander levels a backfill opponent may differ by, in either direction.</summary>
        public const int LevelSpread = 1;

        /// <summary>
        /// A profile inside <paramref name="bandFraction"/> of the requester's power.
        /// <paramref name="seed"/> makes it reproducible, which is what lets the
        /// queue, the tests and any replay agree on the same opponent.
        /// </summary>
        public static PvpOpponentProfile Create(MatchmakingRequest request, string seed, float bandFraction)
        {
            if (request == null)
            {
                return null;
            }

            var hash = DeterministicSeed.Hash(seed);
            var levelOffset = DeterministicSeed.Index(hash, LevelSpread * 2 + 1) - LevelSpread;
            var level = Mathf.Max(1, request.CommanderLevel + levelOffset);

            // -10..+10 tenths of the band, so the spread fills it evenly instead of
            // clustering on the edges.
            var bandSteps = DeterministicSeed.Index(hash >> 8, 21) - 10;
            var power = (long)Mathf.Round(request.HistoricalPower * (1f + bandFraction * bandSteps / 10f));

            var loadout = OpponentLoadoutBuilder.Build(level, 0, AiWeaponRankResolver.MaxRank, seed);

            return new PvpOpponentProfile(
                "bot." + DeterministicSeed.Hash(seed, "id").ToString("x8"),
                BotCallSignGenerator.Generate(seed),
                level,
                power < 0 ? 0 : power,
                // A backfill opponent cannot have liberated more than the player has
                // seen; sharing the pool keeps the anti-spoiler arena rule intact.
                request.UnlockedArenaIds,
                loadout.WeaponIds,
                loadout.WeaponRanks,
                loadout.WeaponBranches,
                ResolveArchetype(hash),
                isBotBackfill: true);
        }

        private static string ResolveArchetype(uint seed)
        {
            return DeterministicSeed.Index(seed >> 16, 3) switch
            {
                0 => "archetype.skirmisher",
                1 => "archetype.bulwark",
                _ => "archetype.striker"
            };
        }
    }
}
