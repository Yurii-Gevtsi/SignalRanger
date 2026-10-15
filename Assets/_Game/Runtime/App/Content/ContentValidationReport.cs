using System;
using System.Collections.Generic;
using System.Linq;
using Game.Progression.Missions;

namespace Game.App.Content
{
    public sealed class ContentValidationReport
    {
        private readonly List<string> _errors = new();

        public IReadOnlyList<string> Errors => _errors;
        public bool IsValid => _errors.Count == 0;

        public void Add(string error)
        {
            if (!string.IsNullOrWhiteSpace(error))
            {
                _errors.Add(error);
            }
        }

        public static ContentValidationReport Validate(RuntimeGameContentCatalog content)
        {
            var report = new ContentValidationReport();
            if (content == null)
            {
                report.Add("Content catalog is missing.");
                return report;
            }

            ValidateDistinctIds(content.Missions, report, mission => mission.MissionId, "mission");
            ValidateDistinctIds(content.Weapons, report, weapon => weapon.WeaponId, "weapon");
            ValidateDistinctIds(content.Chapters, report, chapter => chapter.ChapterId, "campaign chapter");
            ValidateDistinctIds(content.EnemyLoadouts, report, loadout => loadout.LoadoutId, "enemy loadout");
            ValidateDistinctIds(content.DifficultyProfiles, report, profile => profile.ProfileId, "difficulty profile");
            ValidateDistinctIds(content.AiProfiles, report, profile => profile.ProfileId, "AI profile");
            ValidateDistinctIds(content.UnitArchetypes, report, archetype => archetype.ArchetypeId, "unit archetype");

            var weaponIds = new HashSet<string>(content.Weapons.Select(weapon => weapon.WeaponId), StringComparer.Ordinal);
            var missionIndicesById = content.Missions
                .Where(mission => mission != null && !string.IsNullOrWhiteSpace(mission.MissionId))
                .Select((mission, index) => new { mission.MissionId, Index = index })
                .ToDictionary(item => item.MissionId, item => item.Index, StringComparer.Ordinal);
            var chapterAssignments = new Dictionary<string, string>(StringComparer.Ordinal);

            foreach (var mission in content.Missions)
            {
                if (mission == null)
                {
                    report.Add("Mission entry is null.");
                    continue;
                }

                if (!mission.IsValid())
                {
                    report.Add($"Mission '{mission.MissionId}' is invalid.");
                }

                if (!content.HasArena(mission.ArenaId))
                {
                    report.Add($"Mission '{mission.MissionId}' references missing arena '{mission.ArenaId}'.");
                }

                if (!content.TryGetEnemyLoadout(mission.EnemyLoadoutId, out _))
                {
                    report.Add($"Mission '{mission.MissionId}' references missing enemy loadout '{mission.EnemyLoadoutId}'.");
                }

                if (!content.TryGetDifficultyProfile(mission.DifficultyProfileId, out _))
                {
                    report.Add($"Mission '{mission.MissionId}' references missing difficulty profile '{mission.DifficultyProfileId}'.");
                }

                if (!content.HasChapter(mission.ChapterId))
                {
                    report.Add($"Mission '{mission.MissionId}' references missing chapter '{mission.ChapterId}'.");
                }
            }

            foreach (var chapter in content.Chapters)
            {
                if (chapter == null)
                {
                    report.Add("Campaign chapter entry is null.");
                    continue;
                }

                if (!chapter.IsValid())
                {
                    report.Add($"Campaign chapter '{chapter.ChapterId}' is invalid.");
                }

                var previousMissionIndex = -1;
                foreach (var missionId in chapter.MissionIds)
                {
                    if (!content.HasMission(missionId))
                    {
                        report.Add($"Campaign chapter '{chapter.ChapterId}' references missing mission '{missionId}'.");
                        continue;
                    }

                    if (chapterAssignments.TryGetValue(missionId, out var existingChapterId))
                    {
                        report.Add($"Mission '{missionId}' is assigned to both chapter '{existingChapterId}' and '{chapter.ChapterId}'.");
                    }
                    else
                    {
                        chapterAssignments.Add(missionId, chapter.ChapterId);
                    }

                    var missionIndex = missionIndicesById[missionId];
                    if (previousMissionIndex >= 0 && missionIndex != previousMissionIndex + 1)
                    {
                        report.Add($"Campaign chapter '{chapter.ChapterId}' has a non-contiguous mission order around '{missionId}'.");
                    }

                    previousMissionIndex = missionIndex;
                }

                foreach (var weaponId in chapter.FeaturedWeaponIds)
                {
                    if (!weaponIds.Contains(weaponId))
                    {
                        report.Add($"Campaign chapter '{chapter.ChapterId}' references missing featured weapon '{weaponId}'.");
                    }
                }
            }

            foreach (var mission in content.Missions)
            {
                if (mission == null || string.IsNullOrWhiteSpace(mission.MissionId))
                {
                    continue;
                }

                if (!chapterAssignments.TryGetValue(mission.MissionId, out var assignedChapterId))
                {
                    report.Add($"Mission '{mission.MissionId}' is not listed in any campaign chapter.");
                    continue;
                }

                if (!string.Equals(assignedChapterId, mission.ChapterId, StringComparison.Ordinal))
                {
                    report.Add($"Mission '{mission.MissionId}' declares chapter '{mission.ChapterId}' but is listed under '{assignedChapterId}'.");
                }
            }

            foreach (var weapon in content.Weapons)
            {
                if (weapon != null && !weapon.IsValid)
                {
                    report.Add($"Weapon '{weapon.WeaponId}' is invalid.");
                }
            }

            foreach (var loadout in content.EnemyLoadouts)
            {
                if (loadout == null)
                {
                    report.Add("Enemy loadout entry is null.");
                    continue;
                }

                foreach (var weaponId in loadout.WeaponIds)
                {
                    if (!weaponIds.Contains(weaponId))
                    {
                        report.Add($"Enemy loadout '{loadout.LoadoutId}' references missing weapon '{weaponId}'.");
                    }
                }
            }

            foreach (var profile in content.DifficultyProfiles)
            {
                if (profile == null)
                {
                    report.Add("Difficulty profile entry is null.");
                    continue;
                }

                if (!content.HasAiProfile(profile.AiProfileId))
                {
                    report.Add($"Difficulty profile '{profile.ProfileId}' references missing AI profile '{profile.AiProfileId}'.");
                }

                if (!content.HasUnitArchetype(profile.OpponentArchetypeId))
                {
                    report.Add($"Difficulty profile '{profile.ProfileId}' references missing archetype '{profile.OpponentArchetypeId}'.");
                }
            }

            return report;
        }

        private static void ValidateDistinctIds<T>(IEnumerable<T> items, ContentValidationReport report, Func<T, string> idSelector, string label)
            where T : class
        {
            var seen = new HashSet<string>(StringComparer.Ordinal);
            foreach (var item in items)
            {
                if (item == null)
                {
                    report.Add($"{label} entry is null.");
                    continue;
                }

                var id = idSelector(item);
                if (string.IsNullOrWhiteSpace(id))
                {
                    report.Add($"{label} entry has an empty id.");
                    continue;
                }

                if (!seen.Add(id))
                {
                    report.Add($"Duplicate {label} id '{id}'.");
                }
            }
        }
    }
}
