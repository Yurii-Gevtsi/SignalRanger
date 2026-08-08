using System.Globalization;

namespace Game.Core.Localization
{
    /// <summary>
    /// Compatibility facade for gameplay/UI code. New code should use keys
    /// directly; this facade lets the existing game migrate incrementally.
    /// </summary>
    public static class GameText
    {
        public static string Play => T("ui.play", "Грати");
        public static string Loadout => T("ui.loadout", "Спорядження");
        public static string Settings => T("ui.settings", "Налаштування");
        public static string Save => T("ui.save", "Зберегти");
        public static string Back => T("ui.back", "Назад");
        public static string Menu => T("ui.menu", "Меню");
        public static string Restart => T("ui.restart", "Повторити");
        public static string Continue => T("ui.next", "Далі");
        public static string Fire => T("ui.fire", "ВОГОНЬ");
        public static string CancelAim => T("ui.cancelAim", "Скинути");
        public static string Weapon => T("ui.weapon", "Зброя");
        public static string PlayerTeam => T("battle.playerTeam", "Гравець");
        public static string OpponentTeam => T("battle.opponentTeam", "Суперник");
        public static string NoTeam => T("battle.noTeam", "Немає");
        public static string UnknownWeapon => T("content.unknownWeapon", "Невідома зброя");
        public static string NoUnlocks => T("battle.noUnlocks", "Нових відкриттів немає");
        public static string NextMissionUnlocked => T("battle.nextMissionUnlocked", "Наступну місію відкрито.");
        public static string RetryOrMenu => T("battle.retryOrMenu", "Спробуй ще раз або повернись у меню.");
        public static string LoadoutLockedSuffix => T("ui.loadoutLockedSuffix", "  [заблоковано]");
        public static string LoadoutSelectedSuffix => T("ui.loadoutSelectedSuffix", "  [вибрано]");

        public static string BattleTurn(int turnNumber, string activeTeam) => T("battle.turn", $"Хід {turnNumber} - {activeTeam}",
            Arg("turn", turnNumber), Arg("team", activeTeam));
        public static string TeamHealth(string teamName, int health, int maxHealth) => T("battle.teamHealth", $"{teamName} {health}/{maxHealth}",
            Arg("team", teamName), Arg("health", health), Arg("maxHealth", maxHealth));
        public static string Power(float power) => T("battle.power", $"Сила {power:0.0}", Arg("power", power.ToString("0.0", CultureInfo.InvariantCulture)));
        public static string Reward(int amount) => T("battle.reward", $"Нагорода: {amount} монет", Arg("amount", amount));
        public static string AmmoLimited(int amount) => T("battle.ammo", $"Боєприпаси: {amount}", Arg("count", amount));
        public static string AmmoUnlimited() => T("battle.ammoUnlimited", "Боєприпаси: безліміт");
        public static string WeaponSlot(string weaponName, int slotIndex, int slotCount) => T("battle.weaponSlot", $"{weaponName} ({slotIndex}/{slotCount})",
            Arg("weapon", weaponName), Arg("index", slotIndex), Arg("count", slotCount));
        public static string BattleIntro(string arenaName) => T("battle.intro", $"{arenaName}. Наведи постріл перетягуванням, потім натисни Вогонь.", Arg("arena", arenaName));
        public static string WeaponSelected(string weaponName) => T("battle.weaponSelected", $"Обрано зброю: {weaponName}.", Arg("weapon", weaponName));
        public static string WeaponSelectedGeneric() => T("battle.weaponSelectedGeneric", "Зброю обрано.");
        public static string AimRequired() => T("battle.aimRequired", "Спочатку наведи постріл перетягуванням у зоні бою.");
        public static string GuideChooseFirstWeapon() => T("tutorial.chooseWeapon", "Відкрий арсенал і обери зброю для пострілу.");
        public static string GuideAimFirstShot() => T("tutorial.aimFirstShot", "Перший крок: затисни й потягни в зоні бою, щоб з'явилась траєкторія.");
        public static string GuideConfirmFirstShot() => T("tutorial.confirmShot", "Тепер підтвердь підготовлений постріл кнопкою «Вогонь».");
        public static string GuideWatchImpact() => T("tutorial.watchImpact", "Подивись, куди впаде снаряд і скільки шкоди дасть удар.");
        public static string GuideWatchOpponentTurn() => T("tutorial.opponentTurn", "Тепер черга суперника. Зверни увагу, як хід переходить після твого пострілу.");
        public static string AmmoDepleted(string weaponName) => T("battle.ammoDepleted", $"Боєприпаси для {weaponName} закінчилися. Перемкни зброю.", Arg("weapon", weaponName));
        public static string NoWeaponSelected() => T("battle.noWeaponSelected", "Зброю не обрано.");
        public static string ShotStatus(string teamName, string weaponName) => T("battle.shotStatus", $"{teamName} стріляє: {weaponName}.", Arg("team", teamName), Arg("weapon", weaponName));
        public static string TurnStatus(string teamName) => T("battle.turnStatus", $"Хід: {teamName}.", Arg("team", teamName));
        public static string ResultPrompt(string title) => T("battle.resultPrompt", $"{title}. Обери Повторити або Далі.", Arg("title", title));
        public static string UnlockAfterBattle() => T("battle.unlockAfterBattle", "Нове спорядження відкрито після бою");
        public static string UnlockAfterBattle(string weaponName) => T("battle.unlockWeapon", $"Нове спорядження відкрито: {weaponName}.", Arg("weapon", weaponName));
        public static string AimReset() => T("battle.aimReset", "Приціл скинуто.");
        public static string WeaponReset() => T("battle.weaponReset", "Вибір зброї скинуто.");
        public static string OpponentAiming() => T("battle.opponentAiming", "Суперник прицілюється.");
        public static string OpponentFiring() => T("battle.opponentFiring", "Суперник стріляє.");
        public static string CoverDeployed(string weaponName) => T("battle.coverDeployed", $"{weaponName} розгортає укриття.", Arg("weapon", weaponName));
        public static string ShieldPulse(string teamName, int restoredHealth) => restoredHealth > 0
            ? T("battle.shieldRestored", $"{teamName} відновлює {restoredHealth} здоров'я.", Arg("team", teamName), Arg("health", restoredHealth))
            : T("battle.shieldActivated", $"{teamName} активує захист без додаткового лікування.", Arg("team", teamName));
        public static string Repositioned(string teamName) => T("battle.repositioned", $"{teamName} змінює позицію.", Arg("team", teamName));
        public static string RepositionFailed() => T("battle.repositionFailed", "Маневр не дав виграшної позиції.");
        public static string PlayerWon() => T("battle.playerWon", "Ти переміг у бою.");
        public static string OpponentWon() => T("battle.opponentWon", "Команда суперника перемогла.");
        public static string Draw() => T("battle.draw", "Обидві команди вибули.");
        public static string ArenaFallback() => T("content.arenaFallback", "Арена");
        public static string LoadoutUpdated() => T("loadout.updated", "Спорядження оновлено.");
        public static string SettingsSaved() => T("settings.saved", "Налаштування збережено.");
        public static string MenuTitle() => T("menu.title", "Тактичний бій");
        public static string MenuSubtitle() => T("menu.subtitle", "Короткі артилерійські дуелі. MVP build.");
        public static string CampaignMapTitle() => T("campaign.mapTitle", "Campaign");
        public static string ChapterLabel() => T("campaign.chapter", "Розділ кампанії");
        public static string NextMissionLabel() => T("campaign.nextMission", "Наступна місія");
        public static string MissionProgress(int missionNumber, int missionCount) => missionCount > 0
            ? T("campaign.missionProgress", $"Наступна місія {missionNumber}/{missionCount}", Arg("mission", missionNumber), Arg("count", missionCount))
            : NextMissionLabel();
        public static string ContinueToLoadout() => T("loadout.continue", "До спорядження");
        public static string EnterBattle() => T("campaign.enterBattle", "Почати");
        public static string CampaignPlanetLocked() => T("campaign.planetLocked", "Місії цієї планети будуть доступні згодом.");
        public static string CampaignPreviousPlanetLocked() => T("campaign.previousPlanetLocked", "Звільніть попередню планету");
        public static string ChapterFallbackTitle() => T("campaign.defaultTitle", "Початковий розділ");
        public static string ChapterFallbackSummary() => T("campaign.defaultSummary", "Коротка серія боїв, що поступово вводить у тактичний ритм гри.");
        public static string ProfileUnavailable() => T("profile.unavailable", "Профіль: локальні дані ще не готові.");
        public static string ProfileSummary(string playerName, int missionIndex, int softCurrency, int reward) => T("profile.summary",
            $"Профіль: {playerName}\nМісія: {missionIndex}\nМонети: {softCurrency}\nНагорода: {reward} монет",
            Arg("player", playerName), Arg("mission", missionIndex), Arg("coins", softCurrency), Arg("reward", reward));
        public static string DefaultMissionTitle() => T("mission.defaultTitle", "Тренувальна місія");
        public static string DefaultMissionDetails() => T("mission.defaultDetails", "Наведи постріл перетягуванням, зафіксуй приціл і підтвердь постріл кнопкою.");
        public static string LoadStatusCreatedNew() => T("save.created", "Створено локальний профіль. Натисни «Грати», щоб перейти до першого бою.");
        public static string LoadStatusRecovered() => T("save.recovered", "Save було відновлено з резервної копії. Можна грати далі.");
        public static string LoadStatusReady() => T("save.ready", "Локальний профіль завантажено. Натисни «Грати», щоб перейти до бою.");
        public static string MissionCompleted(int softCurrency, bool hasUnlock) => T("mission.completed",
            hasUnlock ? $"Місію завершено. +{softCurrency} монет. Нове спорядження відкрито." : $"Місію завершено. +{softCurrency} монет.",
            Arg("coins", softCurrency), Arg("unlock", hasUnlock ? T("ui.yes", "Так") : string.Empty));
        public static string MissionCompleted(int softCurrency, string unlockedWeaponName) => T("mission.completedWeapon",
            string.IsNullOrWhiteSpace(unlockedWeaponName) ? $"Місію завершено. +{softCurrency} монет." : $"Місію завершено. +{softCurrency} монет. Відкрито: {unlockedWeaponName}.",
            Arg("coins", softCurrency), Arg("weapon", unlockedWeaponName ?? string.Empty));
        public static string LoadoutTitle() => T("ui.loadout", "Спорядження");
        public static string LoadoutHint() => T("loadout.hint", "Обери зброю, яку береш у бій. Заблоковану треба спершу відкрити.");
        public static string LoadoutChooseOne() => T("loadout.chooseOne", "Обери хоча б одну зброю.");
        public static string LoadoutSaveFailed() => T("loadout.saveFailed", "Не вдалося зберегти: перевір, що вся зброя відкрита.");
        public static string LoadoutCount(int count) => T("loadout.count", $"У спорядженні: {count} зброї.", Arg("count", count));
        public static string LoadoutReviewUnlocked(string weaponName) => T("loadout.reviewUnlocked", $"Перевір нове спорядження: {weaponName}. За потреби додай його у свій набір.", Arg("weapon", weaponName));
        public static string LoadoutUnlockAllForTest() => T("loadout.unlockAllTest", "Тест: відкрити все");
        public static string SettingsTitle() => T("ui.settings", "Налаштування");
        public static string RewardsTitle() => T("ui.rewards", "Завдання");
        public static string DailyRewards() => T("ui.dailyRewards", "Щоденні");
        public static string WeeklyRewards() => T("ui.weeklyRewards", "Щотижневі");
        public static string ClaimReward() => T("rewardsScreen.claim", "Отримати");
        public static string RewardClaimed() => T("rewardsScreen.claimed", "Отримано");
        public static string GoToQuest() => T("rewardsScreen.go", "Перейти");
        public static string ClaimDailyLoginReward() => T("rewardsScreen.claimDaily", "Отримати нагороду!");
        public static string DailyLoginTitle() => T("rewardsScreen.dailyLoginTitle", "Щоденна нагорода");
        public static string DailyLoginDescription() => T("rewardsScreen.dailyLoginDescription", "Заходь у гру 7 днів поспіль та отримай особливу нагороду!");
        public static string RewardsHint() => T("rewardsScreen.hint", "Виконуй завдання щодня та отримуй нагороди!");
        public static string RewardsRefresh(string time) => T("rewardsScreen.refresh", $"Оновлення через: {time}", Arg("time", time));
        public static string RewardDay(int day) => T("rewardsScreen.day", $"День {day}", Arg("day", day));
        public static string RewardDayLabel() => T("rewardsScreen.dayLabel", "День");
        public static string RewardQuest(string questId, string fallback) => T("rewardsScreen.quest." + questId, fallback);
        public static string Music() => T("ui.music", "Музика");
        public static string Sounds() => T("ui.sounds", "Звуки");
        public static string LanguageLabel() => T("ui.language", "Мова: Українська", Arg("language", T("meta.uk", "Українська")));
        public static string ResetProgressionLabel() => T("ui.resetProgression", "Скинути прогрес");
        public static string ResetProgressionTitle() => T("ui.resetProgressionTitle", "Скидання прогресу");
        public static string ResetProgressionWarning() => T("ui.resetProgressionWarning", "Усі місії, монети, відкрита зброя та навчання будуть скинуті. Налаштування звуку збережуться.");
        public static string ConfirmResetProgression() => T("ui.confirmReset", "Скинути");
        public static string Cancel() => T("ui.cancel", "Скасувати");
        public static string RoleDirect() => T("content.role.direct", "прямий");
        public static string RoleArc() => T("content.role.arc", "навісний");
        public static string RoleBurst() => T("content.role.burst", "ближній");
        public static string RoleCover() => T("content.role.cover", "укриття");
        public static string RoleDefense() => T("content.role.defense", "захист");
        public static string RoleMovement() => T("content.role.movement", "маневр");
        public static string RoleFallback() => T("content.role.weapon", "зброя");
        public static string WeaponName(string weaponId, string fallback) => T("content.weapon." + weaponId, fallback);
        public static string WeaponDescription(string weaponId, string fallback) => T("content.weaponDescription." + weaponId, fallback);
        public static string WeaponCategory(string category, string fallback) => T("content.category." + category, fallback);
        public static string WeaponEvolutionName(string mechanicId, string fallback) => T("content.weaponEvolutionName." + mechanicId, fallback);
        public static string ArenaName(string arenaId, string fallback) => T("content.arena." + arenaId, fallback);
        public static string ShopTitle() => T("shop.title", "Магазин зброї");
        public static string ArsenalTitle() => T("ui.arsenal", "Арсенал");
        public static string Damage() => T("shop.damage", "Шкода");
        public static string UsesPerBattle() => T("shop.uses", "За бій");
        public static string RequiredLevel(int level) => T("shop.requiredLevel", $"Потрібен рівень {level}", Arg("level", level));
        public static string ConstructorBureau() => T("loadout.constructor", "Майстерня");
        public static string ResetUpgrade() => T("architect.resetUpgrade", "Скинути");
        public static string ChipRewardTitle() => T("chipReward.title", "Отримано креслення!");
        public static string ChipRewardReadyHint() => T("chipReward.ready", "Достатньо для модернізації!");
        public static string SupplyBoxCoins(int amount) => T("battle.supplyBoxCoins", $"Знайдено {amount} кредитів!", Arg("amount", amount));
        public static string SupplyBoxWeapon(string weaponName) => T("battle.supplyBoxWeapon", $"Тимчасова зброя: {weaponName}!", Arg("weapon", weaponName));
        public static string Tip(string key, string fallback) => T("loading." + key, fallback);
        public static string Localized(string key, string fallback, params LocalizationArgument[] arguments) => T(key, fallback, arguments);
        public static string MobilityLabel(float remainingSeconds) => T("battle.mobility", $"Хід: {remainingSeconds:0}с • рух і стрибки вільні", Arg("seconds", remainingSeconds.ToString("0", CultureInfo.InvariantCulture)));
        public static string Moved(string teamName) => T("battle.moved", $"{teamName} змінює позицію.", Arg("team", teamName));
        public static string FrontJumped(string teamName) => T("battle.frontJumped", $"{teamName} робить передній стрибок.", Arg("team", teamName));
        public static string BackJumped(string teamName) => T("battle.backJumped", $"{teamName} робить зворотний стрибок.", Arg("team", teamName));
        public static string Jumped(string teamName) => T("battle.jumped", $"{teamName} робить стрибок.", Arg("team", teamName));
        public static string MovementBlocked() => T("battle.movementBlocked", "Тут нема стійкої поверхні для кроку.");
        public static string JumpBlocked() => T("battle.jumpBlocked", "Стрибок не дотягується до безпечної точки.");
        public static string JumpSpent() => T("battle.jumpSpent", "Стрибок у цьому ході вже витрачено.");
        public static string MovementGuideBlocked() => T("tutorial.movementBlocked", "Спочатку зроби стартовий постріл, а потім додамо маневр.");
        public static string VictoryTitle() => T("result.victory", "Перемога");
        public static string DefeatTitle() => T("result.defeat", "Поразка");
        public static string DrawTitle() => T("result.draw", "Нічия");
        public static string OngoingBattleTitle() => T("result.ongoing", "Бій триває");
        public static string VictorySubtitle(int turnNumber) => T("result.victorySubtitle", $"Бій завершено за {turnNumber} ходів.", Arg("turn", turnNumber));
        public static string DefeatSubtitle(int turnNumber) => T("result.defeatSubtitle", $"Суперник переміг після {turnNumber} ходів.", Arg("turn", turnNumber));
        public static string DrawSubtitle(int turnNumber) => T("result.drawSubtitle", $"Обидві команди вибули після {turnNumber} ходів.", Arg("turn", turnNumber));
        public static string NextArenaPreview() => T("result.nextArena", "Відкрито попередній доступ до наступної арени");

        private static string T(string key, string fallback, params LocalizationArgument[] arguments) => LocalizationRuntime.Get(key, fallback, arguments);
        private static LocalizationArgument Arg(string name, object value) => new(name, value);
    }
}
