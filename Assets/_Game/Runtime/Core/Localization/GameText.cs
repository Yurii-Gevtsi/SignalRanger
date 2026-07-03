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

        public static string BattleTurn(int turnNumber, string activeTeam) => $"Хід {turnNumber} - {activeTeam}";
        public static string TeamHealth(string teamName, int health, int maxHealth) => $"{teamName} {health}/{maxHealth}";
        public static string Power(float power) => $"Сила {power:0.0}";
        public static string Reward(int amount) => $"Нагорода: {amount} монет";
        public static string AmmoLimited(int amount) => $"Боєприпаси: {amount}";
        public static string AmmoUnlimited() => "Боєприпаси: безліміт";
        public static string WeaponSlot(string weaponName, int slotIndex, int slotCount) => $"{weaponName} ({slotIndex}/{slotCount})";
        public static string WeaponType(string roleName) => $"Тип: {roleName}";
        public static string BattleIntro(string arenaName) => $"{arenaName}. Наведи постріл перетягуванням, потім натисни Вогонь.";
        public static string WeaponSelected(string weaponName) => $"Обрано зброю: {weaponName}.";
        public static string WeaponSelectedGeneric() => "Зброю обрано.";
        public static string AimRequired() => "Спочатку наведи постріл перетягуванням у зоні бою.";
        public static string GuideAimFirstShot() => "Перший крок: затисни й потягни в зоні бою, щоб з'явилась траєкторія.";
        public static string GuideConfirmFirstShot() => "Тепер підтвердь підготовлений постріл кнопкою «Вогонь».";
        public static string GuideWatchImpact() => "Подивись, куди впаде снаряд і скільки шкоди дасть удар.";
        public static string GuideWatchOpponentTurn() => "Тепер черга суперника. Зверни увагу, як хід переходить після твого пострілу.";
        public static string AmmoDepleted(string weaponName) => $"Боєприпаси для {weaponName} закінчилися. Перемкни зброю.";
        public static string NoWeaponSelected() => "Зброю не обрано.";
        public static string ShotStatus(string teamName, string weaponName) => $"{teamName} стріляє: {weaponName}.";
        public static string TurnStatus(string teamName) => $"Хід: {teamName}.";
        public static string ResultPrompt(string title) => $"{title}. Обери Повторити або Далі.";
        public static string UnlockAfterBattle() => "Нове спорядження відкрито після бою";
        public static string UnlockAfterBattle(string weaponName) => $"Нове спорядження відкрито: {weaponName}.";
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
        public static string ChapterLabel() => "Розділ кампанії";
        public static string NextMissionLabel() => "Наступна місія";
        public static string MissionProgress(int missionNumber, int missionCount) => missionCount > 0
            ? $"Наступна місія {missionNumber}/{missionCount}"
            : NextMissionLabel();
        public static string ContinueToLoadout() => "До спорядження";
        public static string ChapterFallbackTitle() => "Початковий розділ";
        public static string ChapterFallbackSummary() => "Коротка серія боїв, що поступово вводить у тактичний ритм гри.";
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

        public static string MissionCompleted(int softCurrency, string unlockedWeaponName)
        {
            var unlock = string.IsNullOrWhiteSpace(unlockedWeaponName)
                ? string.Empty
                : $" Відкрито: {unlockedWeaponName}.";
            return $"Місію завершено. +{softCurrency} монет.{unlock}";
        }

        public static string LoadoutTitle() => "Спорядження";
        public static string LoadoutHint() => "Обери зброю, яку береш у бій. Заблоковану треба спершу відкрити.";
        public static string LoadoutChooseOne() => "Обери хоча б одну зброю.";
        public static string LoadoutSaveFailed() => "Не вдалося зберегти: перевір, що вся зброя відкрита.";
        public static string LoadoutCount(int count) => $"У спорядженні: {count} зброї.";
        public static string LoadoutReviewUnlocked(string weaponName) => $"Перевір нове спорядження: {weaponName}. За потреби додай його у свій набір.";
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
        public static string WeaponHintSteadyOrb() => "Рівний стартовий постріл для чистого прямого влучання.";
        public static string WeaponHintPulseShell() => "Працює краще, коли треба перекинути снаряд через схил або укриття.";
        public static string WeaponHintClosePop() => "Найкращий вибір, коли суперник уже поруч і треба швидко добити.";
        public static string WeaponHintLongLance() => "Точний далекий постріл із пласкою траєкторією.";
        public static string WeaponHintScatterArc() => "Тисне по площі й карає суперника, що сховався поруч з укриттям.";
        public static string WeaponHintRampartKit() => "Став укриття, коли не хочеш мінятися прямими пострілами.";
        public static string WeaponHintGuardPulse() => "Повертає здоров'я й допомагає пережити відповідь суперника.";
        public static string WeaponHintRelayStep() => "Маневр для зміни позиції, коли кут важливіший за ще один постріл.";
        public static string WeaponHintFallback() => "Змінюй інструмент під позицію, а не лише під шкоду.";
        public static string VictoryTitle() => "Перемога";
        public static string DefeatTitle() => "Поразка";
        public static string DrawTitle() => "Нічия";
        public static string OngoingBattleTitle() => "Бій триває";
        public static string VictorySubtitle(int turnNumber) => $"Бій завершено за {turnNumber} ходів.";
        public static string DefeatSubtitle(int turnNumber) => $"Суперник переміг після {turnNumber} ходів.";
        public static string DrawSubtitle(int turnNumber) => $"Обидві команди вибули після {turnNumber} ходів.";
        public static string NextArenaPreview() => "Відкрито попередній доступ до наступної арени";
        public const string WalkLeft = "<";
        public const string WalkRight = ">";
        public static string JumpBack() => "\u041d\u0430\u0437\u0430\u0434";
        public static string JumpForward() => "\u0412\u043f\u0435\u0440\u0435\u0434";
        public static string MobilityLabel(float remainingSeconds) =>
            $"\u0425\u0456\u0434: {remainingSeconds:0}\u0441 \u2022 \u0440\u0443\u0445 \u0456 \u0441\u0442\u0440\u0438\u0431\u043a\u0438 \u0432\u0456\u043b\u044c\u043d\u0456";
        public static string Moved(string teamName) => $"{teamName} \u0437\u043c\u0456\u043d\u044e\u0454 \u043f\u043e\u0437\u0438\u0446\u0456\u044e.";
        public static string FrontJumped(string teamName) => $"{teamName} \u0440\u043e\u0431\u0438\u0442\u044c \u043f\u0435\u0440\u0435\u0434\u043d\u0456\u0439 \u0441\u0442\u0440\u0438\u0431\u043e\u043a.";
        public static string BackJumped(string teamName) => $"{teamName} \u0440\u043e\u0431\u0438\u0442\u044c \u0437\u0432\u043e\u0440\u043e\u0442\u043d\u0456\u0439 \u0441\u0442\u0440\u0438\u0431\u043e\u043a.";
        public static string MovementBlocked() => "\u0422\u0443\u0442 \u043d\u0435\u043c\u0430 \u0441\u0442\u0456\u0439\u043a\u043e\u0457 \u043f\u043e\u0432\u0435\u0440\u0445\u043d\u0456 \u0434\u043b\u044f \u043a\u0440\u043e\u043a\u0443.";
        public static string JumpBlocked() => "\u0421\u0442\u0440\u0438\u0431\u043e\u043a \u043d\u0435 \u0434\u043e\u0442\u044f\u0433\u0443\u0454\u0442\u044c\u0441\u044f \u0434\u043e \u0431\u0435\u0437\u043f\u0435\u0447\u043d\u043e\u0457 \u0442\u043e\u0447\u043a\u0438.";
        public static string JumpSpent() => "\u0421\u0442\u0440\u0438\u0431\u043e\u043a \u0443 \u0446\u044c\u043e\u043c\u0443 \u0445\u043e\u0434\u0456 \u0432\u0436\u0435 \u0432\u0438\u0442\u0440\u0430\u0447\u0435\u043d\u043e.";
        public static string MovementGuideBlocked() => "\u0421\u043f\u043e\u0447\u0430\u0442\u043a\u0443 \u0437\u0440\u043e\u0431\u0438 \u0441\u0442\u0430\u0440\u0442\u043e\u0432\u0438\u0439 \u043f\u043e\u0441\u0442\u0440\u0456\u043b, \u0430 \u043f\u043e\u0442\u0456\u043c \u0434\u043e\u0434\u0430\u043c\u043e \u043c\u0430\u043d\u0435\u0432\u0440.";
    }
}
