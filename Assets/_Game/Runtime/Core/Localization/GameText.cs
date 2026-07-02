namespace Game.Core.Localization
{
    public static class GameText
    {
        public const string Play = "Грати";
        public const string Loadout = "Спорядження";
        public const string Settings = "Налаштування";
        public const string Save = "Зберегти";
        public const string Back = "Назад";
        public const string Menu = "Меню";
        public const string Restart = "Повторити";
        public const string Continue = "Далі";
        public const string Fire = "ВОГОНЬ";
        public const string CancelAim = "Скинути";
        public const string Weapon = "Зброя";
        public const string PlayerTeam = "Гравець";
        public const string OpponentTeam = "Суперник";
        public const string NoTeam = "Немає";
        public const string UnknownWeapon = "Невідома зброя";
        public const string NoUnlocks = "Нових відкриттів немає";
        public const string NextMissionUnlocked = "Наступну місію відкрито.";
        public const string RetryOrMenu = "Спробуй ще раз або повернись у меню.";
        public const string LoadoutLockedSuffix = "  [заблоковано]";
        public const string LoadoutSelectedSuffix = "  [вибрано]";

        public static string BattleTurn(int turnNumber, string activeTeam) => $"Хід {turnNumber} — {activeTeam}";
        public static string TeamHealth(string teamName, int health, int maxHealth) => $"{teamName} {health}/{maxHealth}";
        public static string Power(float power) => $"Сила {power:0.0}";
        public static string Reward(int amount) => $"Нагорода: {amount} монет";
        public static string AmmoLimited(int amount) => $"Боєприпаси: {amount}";
        public static string AmmoUnlimited() => "Боєприпаси: безліміт";
        public static string WeaponSlot(string weaponName, int slotIndex, int slotCount) => $"{weaponName} ({slotIndex}/{slotCount})";
        public static string BattleIntro(string arenaName) => $"{arenaName}. Наведи постріл перетягуванням, потім натисни Вогонь.";
        public static string WeaponSelected(string weaponName) => $"Обрано зброю: {weaponName}.";
        public static string WeaponSelectedGeneric() => "Зброю обрано.";
        public static string AimRequired() => "Спочатку наведи постріл перетягуванням у зоні бою.";
        public static string AmmoDepleted(string weaponName) => $"Боєприпаси для {weaponName} закінчилися. Перемкни зброю.";
        public static string NoWeaponSelected() => "Зброю не обрано.";
        public static string ShotStatus(string teamName, string weaponName) => $"{teamName} стріляє: {weaponName}.";
        public static string TurnStatus(string teamName) => $"Хід: {teamName}.";
        public static string ResultPrompt(string title) => $"{title}. Обери Повторити або Далі.";
        public static string UnlockAfterBattle() => "Нове спорядження відкрито після бою";
        public static string AimReset() => "Приціл скинуто.";
        public static string OpponentAiming() => "Суперник прицілюється.";
        public static string OpponentFiring() => "Суперник стріляє.";
        public static string CoverDeployed(string weaponName) => $"{weaponName} розгортає укриття.";
        public static string ShieldPulse(string teamName, int restoredHealth) => restoredHealth > 0
            ? $"{teamName} відновлює {restoredHealth} здоров'я."
            : $"{teamName} активує захист без додаткового лікування.";
        public static string Repositioned(string teamName) => $"{teamName} змінює позицію.";
        public static string RepositionFailed() => "Маневр не дав виграшної позиції.";
        public static string PlayerWon() => "Ти переміг у бою.";
        public static string OpponentWon() => "Команда суперника перемогла.";
        public static string Draw() => "Обидві команди вибули.";
        public static string ArenaFallback() => "Арена";
        public static string LoadoutUpdated() => "Спорядження оновлено.";
        public static string SettingsSaved() => "Налаштування збережено.";
        public static string MenuTitle() => "Тактичний бій";
        public static string MenuSubtitle() => "Короткі артилерійські дуелі. MVP build.";
        public static string NextMissionLabel() => "Наступна місія";
        public static string ProfileUnavailable() => "Профіль: локальні дані ще не готові.";
        public static string ProfileSummary(string playerName, int missionIndex, int softCurrency, int reward) =>
            $"Профіль: {playerName}\nМісія: {missionIndex}\nМонети: {softCurrency}\nНагорода: {reward} монет";
        public static string DefaultMissionTitle() => "Тренувальна місія";
        public static string DefaultMissionDetails() => "Наведи постріл перетягуванням, зафіксуй приціл і підтвердь постріл кнопкою.";
        public static string LoadStatusCreatedNew() => "Створено локальний профіль. Натисни «Грати», щоб перейти до першого бою.";
        public static string LoadStatusRecovered() => "Save було відновлено з резервною копією. Можна грати далі.";
        public static string LoadStatusReady() => "Локальний профіль завантажено. Натисни «Грати», щоб перейти до бою.";

        public static string MissionCompleted(int softCurrency, bool hasUnlock)
        {
            var unlock = hasUnlock ? " Нове спорядження відкрито." : string.Empty;
            return $"Місію завершено. +{softCurrency} монет.{unlock}";
        }

        public static string LoadoutTitle() => "Спорядження";
        public static string LoadoutHint() => "Обери зброю, яку береш у бій. Заблоковану треба спершу відкрити.";
        public static string LoadoutChooseOne() => "Обери хоча б одну зброю.";
        public static string LoadoutSaveFailed() => "Не вдалося зберегти: перевір, що вся зброя відкрита.";
        public static string LoadoutCount(int count) => $"У спорядженні: {count} зброї.";
        public static string LoadoutUnlockAllForTest() => "Тест: відкрити все";
        public static string SettingsTitle() => "Налаштування";
        public static string Music() => "Музика";
        public static string Sounds() => "Звуки";
        public static string LanguageLabel() => "Мова: Українська";
        public static string RoleDirect() => "прямий";
        public static string RoleArc() => "навісний";
        public static string RoleBurst() => "ближній";
        public static string RoleCover() => "укриття";
        public static string RoleDefense() => "захист";
        public static string RoleMovement() => "маневр";
        public static string RoleFallback() => "зброя";
        public static string VictoryTitle() => "Перемога";
        public static string DefeatTitle() => "Поразка";
        public static string DrawTitle() => "Нічия";
        public static string OngoingBattleTitle() => "Бій триває";
        public static string VictorySubtitle(int turnNumber) => $"Бій завершено за {turnNumber} ходів.";
        public static string DefeatSubtitle(int turnNumber) => $"Суперник переміг після {turnNumber} ходів.";
        public static string DrawSubtitle(int turnNumber) => $"Обидві команди вибули після {turnNumber} ходів.";
        public static string NextArenaPreview() => "Відкрито попередній доступ до наступної арени";
    }
}
