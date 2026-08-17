using System;
using System.Collections.Generic;
using Game.Gameplay.Battle.Arenas;
using Game.Gameplay.Battle.Weapons;
using Game.Progression.Missions;

namespace Game.App.Content
{
    public sealed class RuntimeGameContentCatalog
    {
        private readonly Dictionary<string, int> _arenaIndicesById = new(StringComparer.Ordinal);
        private readonly Dictionary<string, MissionDefinition> _missionsById = new(StringComparer.Ordinal);
        private readonly Dictionary<string, CampaignChapterDefinition> _chaptersById = new(StringComparer.Ordinal);
        private readonly Dictionary<string, EnemyLoadoutDefinition> _enemyLoadoutsById = new(StringComparer.Ordinal);
        private readonly Dictionary<string, DifficultyProfileDefinition> _difficultyProfilesById = new(StringComparer.Ordinal);
        private readonly Dictionary<string, AiProfileDefinition> _aiProfilesById = new(StringComparer.Ordinal);
        private readonly Dictionary<string, UnitArchetypeDefinition> _archetypesById = new(StringComparer.Ordinal);

        public RuntimeGameContentCatalog(
            WeaponDefinition[] weapons,
            ArenaDefinition[] arenas,
            MissionDefinition[] missions,
            CampaignChapterDefinition[] chapters,
            EnemyLoadoutDefinition[] enemyLoadouts,
            DifficultyProfileDefinition[] difficultyProfiles,
            AiProfileDefinition[] aiProfiles,
            UnitArchetypeDefinition[] unitArchetypes)
        {
            Weapons = weapons ?? Array.Empty<WeaponDefinition>();
            Arenas = arenas ?? Array.Empty<ArenaDefinition>();
            Missions = missions ?? Array.Empty<MissionDefinition>();
            Chapters = chapters ?? Array.Empty<CampaignChapterDefinition>();
            EnemyLoadouts = enemyLoadouts ?? Array.Empty<EnemyLoadoutDefinition>();
            DifficultyProfiles = difficultyProfiles ?? Array.Empty<DifficultyProfileDefinition>();
            AiProfiles = aiProfiles ?? Array.Empty<AiProfileDefinition>();
            UnitArchetypes = unitArchetypes ?? Array.Empty<UnitArchetypeDefinition>();

            for (var i = 0; i < Arenas.Length; i++)
            {
                var arena = Arenas[i];
                if (arena != null && !string.IsNullOrWhiteSpace(arena.ArenaId) && !_arenaIndicesById.ContainsKey(arena.ArenaId))
                {
                    _arenaIndicesById.Add(arena.ArenaId, i);
                }
            }

            IndexById(Missions, _missionsById, mission => mission.MissionId);
            IndexById(Chapters, _chaptersById, chapter => chapter.ChapterId);
            IndexById(EnemyLoadouts, _enemyLoadoutsById, loadout => loadout.LoadoutId);
            IndexById(DifficultyProfiles, _difficultyProfilesById, profile => profile.ProfileId);
            IndexById(AiProfiles, _aiProfilesById, profile => profile.ProfileId);
            IndexById(UnitArchetypes, _archetypesById, archetype => archetype.ArchetypeId);
        }

        public WeaponDefinition[] Weapons { get; }
        public ArenaDefinition[] Arenas { get; }
        public MissionDefinition[] Missions { get; }
        public CampaignChapterDefinition[] Chapters { get; }
        public EnemyLoadoutDefinition[] EnemyLoadouts { get; }
        public DifficultyProfileDefinition[] DifficultyProfiles { get; }
        public AiProfileDefinition[] AiProfiles { get; }
        public UnitArchetypeDefinition[] UnitArchetypes { get; }

        public int ResolveArenaIndex(MissionDefinition mission)
        {
            if (mission == null)
            {
                return 0;
            }

            return !string.IsNullOrWhiteSpace(mission.ArenaId) && _arenaIndicesById.TryGetValue(mission.ArenaId, out var arenaIndex)
                ? arenaIndex
                : mission.ArenaIndex;
        }

        public int ResolveArenaIndexById(string arenaId)
        {
            return !string.IsNullOrWhiteSpace(arenaId) && _arenaIndicesById.TryGetValue(arenaId, out var arenaIndex)
                ? arenaIndex
                : 0;
        }

        public bool HasArena(string arenaId)
        {
            return !string.IsNullOrWhiteSpace(arenaId) && _arenaIndicesById.ContainsKey(arenaId);
        }

        public bool HasMission(string missionId)
        {
            return !string.IsNullOrWhiteSpace(missionId) && _missionsById.ContainsKey(missionId);
        }

        public bool TryGetMission(string missionId, out MissionDefinition mission)
        {
            return _missionsById.TryGetValue(missionId ?? string.Empty, out mission);
        }

        public bool HasChapter(string chapterId)
        {
            return !string.IsNullOrWhiteSpace(chapterId) && _chaptersById.ContainsKey(chapterId);
        }

        public bool TryGetChapter(string chapterId, out CampaignChapterDefinition chapter)
        {
            return _chaptersById.TryGetValue(chapterId ?? string.Empty, out chapter);
        }

        public bool TryGetEnemyLoadout(string loadoutId, out EnemyLoadoutDefinition loadout)
        {
            return _enemyLoadoutsById.TryGetValue(loadoutId ?? string.Empty, out loadout);
        }

        public bool TryGetDifficultyProfile(string profileId, out DifficultyProfileDefinition profile)
        {
            return _difficultyProfilesById.TryGetValue(profileId ?? string.Empty, out profile);
        }

        public bool HasAiProfile(string profileId)
        {
            return !string.IsNullOrWhiteSpace(profileId) && _aiProfilesById.ContainsKey(profileId);
        }

        public bool HasUnitArchetype(string archetypeId)
        {
            return !string.IsNullOrWhiteSpace(archetypeId) && _archetypesById.ContainsKey(archetypeId);
        }

        private static void IndexById<T>(IEnumerable<T> items, IDictionary<string, T> map, Func<T, string> idSelector)
            where T : class
        {
            foreach (var item in items)
            {
                if (item == null)
                {
                    continue;
                }

                var id = idSelector(item);
                if (!string.IsNullOrWhiteSpace(id) && !map.ContainsKey(id))
                {
                    map.Add(id, item);
                }
            }
        }
    }
}
