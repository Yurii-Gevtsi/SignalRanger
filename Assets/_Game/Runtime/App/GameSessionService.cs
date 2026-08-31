using System.Linq;
using Game.App.Content;
using Game.Core;
using Game.Core.Localization;
using Game.Core.Modes;
using Game.Data.Progression;
using Game.Data.Save;
using Game.Progression.Missions;
using Game.Progression.Modes;

namespace Game.App
{
    public sealed class GameSessionService
    {
        private const string TrainingArenaId = "arena.training-meadow";
        private readonly SaveData _saveData;
        private readonly CampaignProgressionService _campaign;
        private readonly RuntimeGameContentCatalog _content;
        private readonly RaidSetupService _raids;
        private readonly PvpSetupService _duels;

        public GameSessionService(SaveData saveData, CampaignProgressionService campaign, RuntimeGameContentCatalog content)
        {
            _saveData = saveData ?? SaveData.CreateNew();
            _campaign = campaign;
            _content = content;
            _raids = new RaidSetupService(content?.Missions);
            _duels = new PvpSetupService(content?.Missions);
            SelectedMissionIndex = _saveData.campaign.highestUnlockedMissionIndex;
            PublishSelectedMission();
        }

        public int SelectedMissionIndex { get; private set; }
        public MissionDefinition SelectedMission => _campaign?.GetMission(SelectedMissionIndex);

        /// <summary>Arenas the repeatable modes may use; empty until a planet is liberated.</summary>
        public string[] UnlockedArenaIds => _raids.GetUnlockedArenas(_saveData?.campaign?.completedMissionIds);

        public bool AreRepeatableModesUnlocked => UnlockedArenaIds.Length > 0;

        /// <summary>Commander level the opponent scaling is built from.</summary>
        public int CommanderLevel => CommanderProgressionTable.GetLevel(_saveData?.profile?.commanderExperience ?? 0);

        public void SelectNextMission()
        {
            SelectedMissionIndex = _saveData.campaign.highestUnlockedMissionIndex;
            PublishSelectedMission();
        }

        public void SelectMission(int missionIndex)
        {
            SelectedMissionIndex = missionIndex < 0 ? 0 : missionIndex;
            PublishSelectedMission();
        }

        public void SelectTrainingSkirmish()
        {
            SelectMission(0);
            GameSessionContext.ConfigureTrainingSkirmish();
            GameSessionContext.SelectArena(
                _content?.ResolveArenaIndexById(TrainingArenaId) ?? 0,
                TrainingArenaId);
        }

        /// <summary>
        /// Builds a raid without starting it, so a screen can show the arena, the
        /// opponent and the stakes before the player commits.
        /// </summary>
        public RaidBattleSetup PrepareRaid(RaidDifficulty difficulty, string seed)
        {
            return _raids.CreateSetup(
                _saveData?.campaign?.completedMissionIds,
                CommanderLevel,
                difficulty,
                seed);
        }

        /// <summary>
        /// Commits a prepared raid to the session context. Returns false when the
        /// setup is unavailable, so the caller shows the "liberate a planet first"
        /// path instead of loading into nothing.
        /// </summary>
        public bool SelectRaid(RaidBattleSetup setup)
        {
            if (!setup.IsAvailable)
            {
                return false;
            }

            var archetypeId = ResolveArchetypeId(setup.Rules.DifficultyProfileId);
            GameSessionContext.ConfigureRaid(
                _content?.ResolveArenaIndexById(setup.ArenaId) ?? 0,
                setup.ArenaId,
                setup.Rules.DifficultyProfileId,
                archetypeId,
                setup.OpponentWeaponIds,
                setup.OpponentWeaponRanks,
                setup.OpponentWeaponBranches,
                setup.Rules.AiDifficultyTier);
            GameSessionContext.SelectLoadout(_saveData?.inventory?.selectedLoadoutWeaponIds?.ToArray());
            return true;
        }

        /// <summary>What the local commander brings to the duel queue.</summary>
        public MatchmakingRequest CreateMatchmakingRequest()
        {
            return new MatchmakingRequest(
                _saveData?.profile?.profileId,
                _saveData?.profile?.playerName,
                CommanderLevel,
                _saveData?.profile?.historicalPower ?? 0,
                UnlockedArenaIds,
                _saveData?.inventory?.selectedLoadoutWeaponIds?.ToArray());
        }

        public PvpBattleSetup PrepareDuel(MatchmakingRequest request, MatchmakingResult match)
        {
            return _duels.CreateSetup(request, match);
        }

        public bool SelectDuel(PvpBattleSetup setup)
        {
            if (!setup.IsAvailable || setup.Opponent == null)
            {
                return false;
            }

            GameSessionContext.ConfigurePvpMatch(
                _content?.ResolveArenaIndexById(setup.ArenaId) ?? 0,
                setup.ArenaId,
                setup.MatchId,
                _saveData?.profile?.profileId,
                setup.Opponent,
                setup.Opponent.ArchetypeId,
                setup.OpponentWeaponIds,
                setup.OpponentWeaponRanks,
                setup.OpponentWeaponBranches,
                setup.AiDifficultyTier);
            GameSessionContext.SelectLoadout(_saveData?.inventory?.selectedLoadoutWeaponIds?.ToArray());
            return true;
        }

        private string ResolveArchetypeId(string difficultyProfileId)
        {
            return _content != null && _content.TryGetDifficultyProfile(difficultyProfileId, out var profile)
                ? profile.OpponentArchetypeId
                : string.Empty;
        }

        private void PublishSelectedMission()
        {
            var mission = SelectedMission;
            var arenaIndex = _content?.ResolveArenaIndex(mission) ?? (mission != null ? mission.ArenaIndex : SelectedMissionIndex);
            var enemyLoadout = mission != null && _content != null && _content.TryGetEnemyLoadout(mission.EnemyLoadoutId, out var resolvedLoadout)
                ? resolvedLoadout
                : null;
            var difficultyProfile = mission != null && _content != null && _content.TryGetDifficultyProfile(mission.DifficultyProfileId, out var resolvedProfile)
                ? resolvedProfile
                : null;
            GameSessionContext.SelectMission(
                SelectedMissionIndex,
                arenaIndex,
                mission != null ? mission.MissionId : string.Empty,
                mission != null ? mission.ArenaId : string.Empty,
                mission != null ? mission.AllowedWeaponIds : null,
                mission != null ? mission.TutorialStepIds : null,
                mission != null ? mission.EnemyLoadoutId : string.Empty,
                enemyLoadout != null ? enemyLoadout.WeaponIds : null,
                mission != null ? mission.DifficultyProfileId : string.Empty,
                difficultyProfile != null ? difficultyProfile.OpponentArchetypeId : string.Empty,
                mission != null ? mission.Reward.softCurrency : 0,
                mission != null ? mission.Reward.unlockWeaponId : string.Empty,
                difficultyProfile != null ? difficultyProfile.AiDifficultyTier : mission != null ? mission.Difficulty : 0,
                mission == null || mission.SupportDropsEnabled,
                mission != null
                    ? (string.IsNullOrWhiteSpace(mission.Description)
                        ? GameText.MissionTitle(mission.MissionId, mission.Title)
                        : GameText.MissionDescription(mission.MissionId, mission.Description))
                    : string.Empty);
            GameSessionContext.SelectLoadout(_saveData?.inventory?.selectedLoadoutWeaponIds?.ToArray());
        }
    }
}
