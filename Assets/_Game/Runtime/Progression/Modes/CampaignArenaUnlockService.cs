using System;
using System.Collections.Generic;
using Game.Progression.Missions;

namespace Game.Progression.Modes
{
    /// <summary>
    /// Which arenas the repeatable modes may use. A planet owns a chapter, and a
    /// chapter's arenas open only once the whole chapter is cleared - the raid is
    /// fiction-wise a defence of an already liberated planet, so a half-finished
    /// chapter must not hand its map over.
    /// </summary>
    public static class CampaignArenaUnlockService
    {
        /// <summary>
        /// The tutorial sandbox. It belongs to the first battle only and never joins
        /// the raid or duel pool, whatever the campaign progress says.
        /// </summary>
        public const string TrainingArenaId = "arena.training-meadow";

        /// <summary>
        /// Liberated arena ids, ordered by first appearance in the mission list so
        /// two runs - and two clients - always see the same order.
        /// </summary>
        public static string[] ResolveUnlockedArenas(
            IReadOnlyList<MissionDefinition> missions,
            IReadOnlyCollection<string> completedMissionIds)
        {
            if (missions == null || missions.Count == 0)
            {
                return Array.Empty<string>();
            }

            var completed = completedMissionIds != null
                ? new HashSet<string>(completedMissionIds, StringComparer.Ordinal)
                : new HashSet<string>(StringComparer.Ordinal);

            var clearedChapters = ResolveClearedChaptersCore(missions, completed);
            if (clearedChapters.Count == 0)
            {
                return Array.Empty<string>();
            }

            var arenas = new List<string>();
            for (var i = 0; i < missions.Count; i++)
            {
                var mission = missions[i];
                if (mission == null || string.IsNullOrWhiteSpace(mission.ArenaId))
                {
                    continue;
                }

                if (string.Equals(mission.ArenaId, TrainingArenaId, StringComparison.Ordinal))
                {
                    continue;
                }

                if (!clearedChapters.Contains(mission.ChapterId ?? string.Empty))
                {
                    continue;
                }

                if (!arenas.Contains(mission.ArenaId))
                {
                    arenas.Add(mission.ArenaId);
                }
            }

            return arenas.ToArray();
        }

        /// <summary>Chapters whose every mission is recorded as completed.</summary>
        public static HashSet<string> ResolveClearedChapters(
            IReadOnlyList<MissionDefinition> missions,
            IReadOnlyCollection<string> completedMissionIds)
        {
            var completed = completedMissionIds as HashSet<string>
                ?? (completedMissionIds != null
                    ? new HashSet<string>(completedMissionIds, StringComparer.Ordinal)
                    : new HashSet<string>(StringComparer.Ordinal));
            return ResolveClearedChaptersCore(missions, completed);
        }

        private static HashSet<string> ResolveClearedChaptersCore(
            IReadOnlyList<MissionDefinition> missions,
            HashSet<string> completed)
        {
            var pending = new HashSet<string>(StringComparer.Ordinal);
            var seen = new HashSet<string>(StringComparer.Ordinal);
            if (missions == null)
            {
                return seen;
            }

            for (var i = 0; i < missions.Count; i++)
            {
                var mission = missions[i];
                if (mission == null || string.IsNullOrWhiteSpace(mission.ChapterId))
                {
                    continue;
                }

                seen.Add(mission.ChapterId);
                if (!completed.Contains(mission.MissionId ?? string.Empty))
                {
                    pending.Add(mission.ChapterId);
                }
            }

            seen.ExceptWith(pending);
            return seen;
        }

        /// <summary>
        /// Whether the repeatable modes are open at all. False until the first planet
        /// is liberated - and the UI is expected to say why rather than hide them.
        /// </summary>
        public static bool HasAnyUnlockedArena(
            IReadOnlyList<MissionDefinition> missions,
            IReadOnlyCollection<string> completedMissionIds)
        {
            return ResolveUnlockedArenas(missions, completedMissionIds).Length > 0;
        }
    }
}
