using System.Globalization;

namespace Game.Core.Localization
{
    /// <summary>
    /// Compatibility facade for gameplay/UI code. New code should use keys
    /// directly; this facade lets the existing game migrate incrementally.
    /// </summary>
    public static class GameText
    {
        public static string Play => T("ui.play", string.Empty);
        public static string Loadout => T("ui.loadout", string.Empty);
        public static string Settings => T("ui.settings", string.Empty);
        public static string Save => T("ui.save", string.Empty);
        public static string Back => T("ui.back", string.Empty);
        public static string Menu => T("ui.menu", string.Empty);
        public static string Restart => T("ui.restart", string.Empty);
        public static string Continue => T("ui.next", string.Empty);
        public static string Fire => T("ui.fire", string.Empty);
        public static string CancelAim => T("ui.cancelAim", string.Empty);
        public static string SkipTutorial() => T("ui.skipTutorial", string.Empty);
        public static string Weapon => T("ui.weapon", string.Empty);
        public static string PlayerTeam => T("battle.playerTeam", string.Empty);
        public static string OpponentTeam => T("battle.opponentTeam", string.Empty);
        public static string NoTeam => T("battle.noTeam", string.Empty);
        public static string UnknownWeapon => T("content.unknownWeapon", string.Empty);
        public static string NoUnlocks => T("battle.noUnlocks", string.Empty);
        public static string NextMissionUnlocked => T("battle.nextMissionUnlocked", string.Empty);
        public static string RetryOrMenu => T("battle.retryOrMenu", string.Empty);
        public static string LoadoutLockedSuffix => T("ui.loadoutLockedSuffix", string.Empty);
        public static string LoadoutSelectedSuffix => T("ui.loadoutSelectedSuffix", string.Empty);

        public static string BattleTurn(int turnNumber, string activeTeam) => T("battle.turn", string.Empty,
            Arg("turn", turnNumber), Arg("team", activeTeam));
        public static string TeamHealth(string teamName, int health, int maxHealth) => T("battle.teamHealth", string.Empty,
            Arg("team", teamName), Arg("health", health), Arg("maxHealth", maxHealth));
        public static string Power(float power) => T("battle.power", string.Empty, Arg("power", power.ToString("0.0", CultureInfo.InvariantCulture)));
        public static string Reward(int amount) => T("battle.reward", string.Empty, Arg("amount", amount));
        public static string AmmoLimited(int amount) => T("battle.ammo", string.Empty, Arg("count", amount));
        public static string AmmoUnlimited() => T("battle.ammoUnlimited", string.Empty);
        public static string WeaponSlot(string weaponName, int slotIndex, int slotCount) => T("battle.weaponSlot", string.Empty,
            Arg("weapon", weaponName), Arg("index", slotIndex), Arg("count", slotCount));
        public static string BattleIntro(string arenaName) => T("battle.intro", string.Empty, Arg("arena", arenaName));
        public static string WeaponSelected(string weaponName) => T("battle.weaponSelected", string.Empty, Arg("weapon", weaponName));
        public static string WeaponSelectedGeneric() => T("battle.weaponSelectedGeneric", string.Empty);
        public static string AimRequired() => T("battle.aimRequired", string.Empty);
        public static string GuideChooseFirstWeapon() => T("tutorial.chooseWeapon", string.Empty);
        public static string GuideAimFirstShot() => T("tutorial.aimFirstShot", string.Empty);
        public static string GuideConfirmFirstShot() => T("tutorial.confirmShot", string.Empty);
        public static string GuideWatchImpact() => T("tutorial.watchImpact", string.Empty);
        public static string GuideWatchOpponentTurn() => T("tutorial.opponentTurn", string.Empty);
        public static string AmmoDepleted(string weaponName) => T("battle.ammoDepleted", string.Empty, Arg("weapon", weaponName));
        public static string NoWeaponSelected() => T("battle.noWeaponSelected", string.Empty);
        public static string ShotStatus(string teamName, string weaponName) => T("battle.shotStatus", string.Empty, Arg("team", teamName), Arg("weapon", weaponName));
        public static string TurnStatus(string teamName) => T("battle.turnStatus", string.Empty, Arg("team", teamName));
        public static string ResultPrompt(string title) => T("battle.resultPrompt", string.Empty, Arg("title", title));
        public static string UnlockAfterBattle() => T("battle.unlockAfterBattle", string.Empty);
        public static string UnlockAfterBattle(string weaponName) => T("battle.unlockWeapon", string.Empty, Arg("weapon", weaponName));
        public static string AimReset() => T("battle.aimReset", string.Empty);
        public static string WeaponReset() => T("battle.weaponReset", string.Empty);
        public static string OpponentAiming() => T("battle.opponentAiming", string.Empty);
        public static string OpponentFiring() => T("battle.opponentFiring", string.Empty);
        public static string CoverDeployed(string weaponName) => T("battle.coverDeployed", string.Empty, Arg("weapon", weaponName));
        public static string ShieldPulse(string teamName, int restoredHealth) => restoredHealth > 0
            ? T("battle.shieldRestored", string.Empty, Arg("team", teamName), Arg("health", restoredHealth))
            : T("battle.shieldActivated", string.Empty, Arg("team", teamName));
        public static string Repositioned(string teamName) => T("battle.repositioned", string.Empty, Arg("team", teamName));
        public static string RepositionFailed() => T("battle.repositionFailed", string.Empty);
        public static string PlayerWon() => T("battle.playerWon", string.Empty);
        public static string OpponentWon() => T("battle.opponentWon", string.Empty);
        public static string Draw() => T("battle.draw", string.Empty);
        public static string ArenaFallback() => T("content.arenaFallback", string.Empty);
        public static string LoadoutUpdated() => T("loadout.updated", string.Empty);
        public static string SettingsSaved() => T("settings.saved", string.Empty);
        public static string MenuTitle() => T("menu.title", string.Empty);
        public static string MenuSubtitle() => T("menu.subtitle", string.Empty);
        public static string CampaignMapTitle() => T("campaign.mapTitle", string.Empty);
        public static string ModeTraining() => T("mode.training", string.Empty);
        public static string ModeMap() => T("mode.map", string.Empty);
        public static string ModeArena() => T("mode.arena", string.Empty);
        public static string ModeBosses() => T("mode.bosses", string.Empty);
        public static string ModeMapHint() => T("mode.mapHint", string.Empty);
        public static string ModeTrainingHint() => T("mode.trainingHint", string.Empty);
        public static string ModeRaidHint() => T("mode.raidHint", string.Empty);
        public static string ModeDifficultyHint() => T("mode.difficultyHint", string.Empty);
        public static string ModeArenaHint() => T("mode.arenaHint", string.Empty);
        public static string ModeBossesHint() => T("mode.bossesHint", string.Empty);
        public static string ModeMissions() => T("mode.missions", string.Empty);
        public static string ModeSelectMap() => T("mode.selectMap", string.Empty);
        public static string ModePreview() => T("mode.preview", string.Empty);
        public static string ModeOnline() => T("mode.online", string.Empty);
        public static string ModeCluster(int number) => T("mode.cluster", string.Empty, Arg("number", number));
        public static string ModeClusterLocked() => T("mode.clusterLocked", string.Empty);
        public static string CampaignPreviousPlanetLockedFromSigma() => T("campaign.previousPlanetLockedFromSigma", string.Empty);
        public static string CampaignExperienceAbbreviation() => T("campaign.experienceAbbreviation", string.Empty);
        public static string CampaignSectorName(int number) => T("campaign.sector." + number, string.Empty);
        public static string CampaignSectorMissionProgress(int completed, int total) =>
            T("campaign.sectorMissionProgress", string.Empty, Arg("completed", completed), Arg("total", total));
        public static string CampaignSectorProgressHint(string sector, int total) =>
            T("campaign.sectorProgressHint", string.Empty, Arg("sector", sector), Arg("total", total));
        public static string CampaignMissionsForPlanet(string planet) =>
            T("campaign.missionsForPlanet", string.Empty, Arg("planet", planet));
        public static string ChapterLabel() => T("campaign.chapter", string.Empty);
        public static string NextMissionLabel() => T("campaign.nextMission", string.Empty);
        public static string MissionProgress(int missionNumber, int missionCount) => missionCount > 0
            ? T("campaign.missionProgress", string.Empty, Arg("mission", missionNumber), Arg("count", missionCount))
            : NextMissionLabel();
        public static string ContinueToLoadout() => T("loadout.continue", string.Empty);
        public static string EnterBattle() => T("campaign.enterBattle", string.Empty);
        public static string CampaignPlanetLocked() => T("campaign.planetLocked", string.Empty);
        public static string CampaignPreviousPlanetLocked() => T("campaign.previousPlanetLocked", string.Empty);
        public static string ChapterFallbackTitle() => T("campaign.defaultTitle", string.Empty);
        public static string ChapterFallbackSummary() => T("campaign.defaultSummary", string.Empty);
        public static string ProfileUnavailable() => T("profile.unavailable", string.Empty);
        public static string ProfileSummary(string playerName, int missionIndex, int softCurrency, int reward) => T("profile.summary",
            string.Empty,
            Arg("player", playerName), Arg("mission", missionIndex), Arg("coins", softCurrency), Arg("reward", reward));
        public static string DefaultMissionTitle() => T("mission.defaultTitle", string.Empty);
        public static string DefaultMissionDetails() => T("mission.defaultDetails", string.Empty);
        public static string LoadStatusCreatedNew() => T("save.created", string.Empty);
        public static string LoadStatusRecovered() => T("save.recovered", string.Empty);
        public static string LoadStatusReady() => T("save.ready", string.Empty);
        public static string MissionCompleted(int softCurrency, bool hasUnlock) => T("mission.completed",
            string.Empty,
            Arg("coins", softCurrency), Arg("unlock", hasUnlock ? T("ui.yes", string.Empty) : string.Empty));
        public static string MissionCompleted(int softCurrency, string unlockedWeaponName) => T("mission.completedWeapon",
            string.Empty,
            Arg("coins", softCurrency), Arg("weapon", unlockedWeaponName ?? string.Empty));
        public static string LoadoutTitle() => T("ui.loadout", string.Empty);
        public static string LoadoutHint() => T("loadout.hint", string.Empty);
        public static string LoadoutChooseOne() => T("loadout.chooseOne", string.Empty);
        public static string LoadoutSaveFailed() => T("loadout.saveFailed", string.Empty);
        public static string LoadoutCount(int count) => T("loadout.count", string.Empty, Arg("count", count));
        public static string LoadoutReviewUnlocked(string weaponName) => T("loadout.reviewUnlocked", string.Empty, Arg("weapon", weaponName));
        public static string LoadoutUnlockAllForTest() => T("loadout.unlockAllTest", string.Empty);
        public static string SettingsTitle() => T("ui.settings", string.Empty);
        public static string RewardsTitle() => T("ui.rewards", string.Empty);
        public static string DailyRewards() => T("ui.dailyRewards", string.Empty);
        public static string WeeklyRewards() => T("ui.weeklyRewards", string.Empty);
        public static string ClaimReward() => T("rewardsScreen.claim", string.Empty);
        public static string RewardClaimed() => T("rewardsScreen.claimed", string.Empty);
        public static string GoToQuest() => T("rewardsScreen.go", string.Empty);
        public static string ClaimDailyLoginReward() => T("rewardsScreen.claimDaily", string.Empty);
        public static string DailyLoginTitle() => T("rewardsScreen.dailyLoginTitle", string.Empty);
        public static string DailyLoginDescription() => T("rewardsScreen.dailyLoginDescription", string.Empty);
        public static string RewardsHint() => T("rewardsScreen.hint", string.Empty);
        public static string RewardsRefresh(string time) => T("rewardsScreen.refresh", string.Empty, Arg("time", time));
        public static string RewardDay(int day) => T("rewardsScreen.day", string.Empty, Arg("day", day));
        public static string RewardDayLabel() => T("rewardsScreen.dayLabel", string.Empty);
        public static string RewardQuest(string questId, string fallback) => T("rewardsScreen.quest." + questId, fallback);
        public static string Music() => T("ui.music", string.Empty);
        public static string Sounds() => T("ui.sounds", string.Empty);
        public static string LanguageLabel() => T("ui.language", string.Empty, Arg("language", T("meta.uk", string.Empty)));
        public static string ResetProgressionLabel() => T("ui.resetProgression", string.Empty);
        public static string ResetProgressionTitle() => T("ui.resetProgressionTitle", string.Empty);
        public static string ResetProgressionWarning() => T("ui.resetProgressionWarning", string.Empty);
        public static string ConfirmResetProgression() => T("ui.confirmReset", string.Empty);
        public static string Cancel() => T("ui.cancel", string.Empty);
        public static string RoleDirect() => T("content.role.direct", string.Empty);
        public static string RoleArc() => T("content.role.arc", string.Empty);
        public static string RoleBurst() => T("content.role.burst", string.Empty);
        public static string RoleCover() => T("content.role.cover", string.Empty);
        public static string RoleDefense() => T("content.role.defense", string.Empty);
        public static string RoleMovement() => T("content.role.movement", string.Empty);
        public static string RoleFallback() => T("content.role.weapon", string.Empty);
        /// <summary>The campaign antagonist; also the opponent behind every raid.</summary>
        public static string SigmaName() => T("content.opponent.sigma", string.Empty);

        /// <summary>Opponent readout for the repeatable modes, without a weapon rank.</summary>
        public static string OpponentBar(string name, string weapon) =>
            T("battle.opponentBar", string.Empty, Arg("name", name), Arg("weapon", weapon));

        /// <summary>
        /// Opponent readout including how developed the weapon is, so a shot that
        /// hits harder than expected has a visible reason.
        /// </summary>
        public static string OpponentBarRanked(string name, string weapon, int rank) =>
            T("battle.opponentBarRanked", string.Empty,
                Arg("name", name), Arg("weapon", weapon), Arg("rank", rank));

        /// <summary>Reward line on the result card of a raid or duel.</summary>
        public static string RewardSummary(int credits, int experience, int blueprints) =>
            T("battle.rewardSummary", string.Empty,
                Arg("credits", credits), Arg("experience", experience), Arg("blueprints", blueprints));

        /// <summary>Note shown when the daily full-reward budget is already spent.</summary>
        public static string RewardDailyLimit() =>
            T("battle.rewardDailyLimit", string.Empty);

        public static string WeaponWithRank(string weapon, int rank) =>
            T("battle.weaponWithRank", string.Empty, Arg("weapon", weapon), Arg("rank", rank));

        public static string RaidTitle() => T("raid.title", string.Empty);
        public static string RaidDifficultyLabel() => T("raid.difficulty", string.Empty);
        public static string RaidDifficultyEasy() => T("raid.difficultyEasy", string.Empty);
        public static string RaidDifficultyNormal() => T("raid.difficultyNormal", string.Empty);
        public static string RaidDifficultyHard() => T("raid.difficultyHard", string.Empty);
        public static string RaidOpponentLabel() => T("raid.opponent", string.Empty);
        public static string RaidLocked() =>
            T("raid.locked", string.Empty);
        public static string RaidOpenCampaign() => T("raid.openCampaign", string.Empty);

        public static string DuelTitle() => T("duel.title", string.Empty);
        public static string OpponentLeftDuel() =>
            T("duel.opponentLeft", string.Empty);
        public static string DuelSearching(int seconds) =>
            T("duel.searching", string.Empty, Arg("seconds", seconds));
        public static string DuelMatchFound(string arena) =>
            T("duel.matchFound", string.Empty, Arg("arena", arena));
        public static string DuelOpponentLevel(int level, long power) =>
            T("duel.opponentLevel", string.Empty, Arg("level", level), Arg("power", power));
        public static string DuelLocked() =>
            T("duel.locked", string.Empty);
        public static string DuelNoOpponent() =>
            T("duel.noOpponent", string.Empty);
        public static string DuelUnavailable() =>
            T("duel.unavailable", string.Empty);
        public static string WeaponName(string weaponId, string fallback) => T("content.weapon." + weaponId, fallback);
        public static string WeaponDescription(string weaponId, string fallback) => T("content.weaponDescription." + weaponId, fallback);
        public static string WeaponCategory(string category, string fallback) => T("content.category." + category, fallback);
        public static string WeaponEvolutionName(string mechanicId, string fallback) => T("content.weaponEvolutionName." + mechanicId, fallback);
        public static string WeaponEvolutionDescription(string mechanicId, string fallback) => T("content.weaponEvolutionDescription." + mechanicId, fallback);
        public static string UpgradeStatName(string statKey, string fallback) => T("content.upgradeStat." + statKey, fallback);
        public static string UpgradeBaseConfigTitle() => T("content.upgradeTitle.baseConfig", string.Empty);
        public static string UpgradeGenericTitle() => T("content.upgradeTitle.generic", string.Empty);
        public static string UpgradeEvolutionChoiceTitle() => T("content.upgradeTitle.evolutionChoice", string.Empty);
        public static string UpgradeFinalEvolutionTitle() => T("content.upgradeTitle.finalEvolution", string.Empty);
        public static string UpgradeStatBonusTitle(string statName, int amount) => T("content.upgradeTitle.statBonus", string.Empty,
            Arg("stat", statName), Arg("amount", amount));
        public static string UpgradeStatImprovement(string statName, int amount) => T("content.upgradeDescription.statImprovement", string.Empty,
            Arg("stat", statName), Arg("amount", amount));
        public static string ArenaName(string arenaId, string fallback) => T("content.arena." + arenaId, fallback);
        public static string ShopTitle() => T("shop.title", string.Empty);
        public static string ArsenalTitle() => T("ui.arsenal", string.Empty);
        public static string Damage() => T("shop.damage", string.Empty);
        public static string UsesPerBattle() => T("shop.uses", string.Empty);
        public static string RequiredLevel(int level) => T("shop.requiredLevel", string.Empty, Arg("level", level));
        public static string ConstructorBureau() => T("loadout.constructor", string.Empty);
        public static string ResetUpgrade() => T("architect.resetUpgrade", string.Empty);
        public static string ChipRewardTitle() => T("chipReward.title", string.Empty);
        public static string ChipRewardReadyHint() => T("chipReward.ready", string.Empty);
        public static string SupplyBoxCoins(int amount) => T("battle.supplyBoxCoins", string.Empty, Arg("amount", amount));
        public static string SupplyBoxWeapon(string weaponName) => T("battle.supplyBoxWeapon", string.Empty, Arg("weapon", weaponName));
        public static string Tip(string key, string fallback) => T("loading." + key, fallback);

        /// <summary>
        /// Mission name from the dictionaries. The catalog's own <c>Title</c> is the
        /// authored Ukrainian source and is only ever the fallback - passing it
        /// straight to the screen is what left every language showing Ukrainian.
        /// </summary>
        public static string MissionTitle(string missionId, string fallback) =>
            T("missionText." + missionId + ".title", fallback);

        public static string MissionDescription(string missionId, string fallback) =>
            T("missionText." + missionId + ".description", fallback);
        public static string Localized(string key, string fallback, params LocalizationArgument[] arguments) => T(key, fallback, arguments);
        public static string MobilityLabel(float remainingSeconds) => T("battle.mobility", string.Empty, Arg("seconds", remainingSeconds.ToString("0", CultureInfo.InvariantCulture)));
        public static string Moved(string teamName) => T("battle.moved", string.Empty, Arg("team", teamName));
        public static string FrontJumped(string teamName) => T("battle.frontJumped", string.Empty, Arg("team", teamName));
        public static string BackJumped(string teamName) => T("battle.backJumped", string.Empty, Arg("team", teamName));
        public static string Jumped(string teamName) => T("battle.jumped", string.Empty, Arg("team", teamName));
        public static string MovementBlocked() => T("battle.movementBlocked", string.Empty);
        public static string JumpBlocked() => T("battle.jumpBlocked", string.Empty);
        public static string JumpSpent() => T("battle.jumpSpent", string.Empty);
        public static string MovementGuideBlocked() => T("tutorial.movementBlocked", string.Empty);
        public static string VictoryTitle() => T("result.victory", string.Empty);
        public static string DefeatTitle() => T("result.defeat", string.Empty);
        public static string DrawTitle() => T("result.draw", string.Empty);
        public static string OngoingBattleTitle() => T("result.ongoing", string.Empty);
        public static string VictorySubtitle(int turnNumber) => T("result.victorySubtitle", string.Empty, Arg("turn", turnNumber));
        public static string DefeatSubtitle(int turnNumber) => T("result.defeatSubtitle", string.Empty, Arg("turn", turnNumber));
        public static string DrawSubtitle(int turnNumber) => T("result.drawSubtitle", string.Empty, Arg("turn", turnNumber));
        public static string NextArenaPreview() => T("result.nextArena", string.Empty);

        public static string PlayerLevel(int level) => T("ui.playerLevel", string.Empty, Arg("level", level));
        public static string DefaultPlayerName() => T("ui.defaultPlayerName", string.Empty);
        public static string LevelLabel() => T("ui.levelLabel", string.Empty);
        public static string NameCharacterCount(int used, int max) =>
            T("profile.nameCounter", string.Empty, Arg("used", used), Arg("max", max));

        private static string T(string key, string fallback, params LocalizationArgument[] arguments) => LocalizationRuntime.Get(key, fallback, arguments);
        private static LocalizationArgument Arg(string name, object value) => new(name, value);
    }
}
