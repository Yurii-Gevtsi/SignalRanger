namespace Game.Progression.Missions
{
    public static class MvpMissionLibrary
    {
        public static MissionDefinition[] CreateAll()
        {
            return new[]
            {
                Mission(
                    "mission.training.01",
                    "Перший постріл",
                    "Наведи дугу, зафіксуй приціл і заверши коротку дуель.",
                    0,
                    "arena.training-meadow",
                    new[] { "weapon.impulse", "weapon.mortira" },
                    new[] { "tutorial.aim", "tutorial.fire", "tutorial.impact", "tutorial.turn", "tutorial.win" },
                    25,
                    chapterId: "chapter.basics",
                    enemyLoadoutId: "enemy.training.direct",
                    difficultyProfileId: "difficulty.easy"),
                Mission(
                    "mission.training.02",
                    "Дуга над схилом",
                    "Спробуй вищу траєкторію й використай силу пострілу.",
                    1,
                    "arena.split-ridge",
                    new[] { "weapon.mortira", "weapon.impulse" },
                    new[] { "tutorial.arc", "tutorial.power", "tutorial.arc-review" },
                    30,
                    chapterId: "chapter.basics",
                    enemyLoadoutId: "enemy.training.arc",
                    difficultyProfileId: "difficulty.easy"),
                Mission(
                    "mission.training.03",
                    "Укриття посередині",
                    "Влучай не тільки прямо, а й поруч із ціллю.",
                    2,
                    "arena.central-cover",
                    new[] { "weapon.mortira", "weapon.granata" },
                    new[] { "tutorial.cover-target", "tutorial.radius" },
                    35,
                    "weapon.granata",
                    chapterId: "chapter.basics",
                    enemyLoadoutId: "enemy.training.arc",
                    difficultyProfileId: "difficulty.easy"),
                Mission(
                    "mission.training.04",
                    "Ближня дистанція",
                    "Коротка арена для швидкого рішення й контролю ризику.",
                    0,
                    "arena.training-meadow",
                    new[] { "weapon.blaster-caset", "weapon.impulse" },
                    new[] { "tutorial.close-range" },
                    40,
                    "weapon.blaster-caset",
                    1,
                    "chapter.basics",
                    "enemy.training.direct",
                    "difficulty.normal"),
                Mission(
                    "mission.training.05",
                    "Вибір інструмента",
                    "Перемикай зброю під ситуацію замість одного звичного пострілу.",
                    1,
                    "arena.split-ridge",
                    new[] { "weapon.impulse", "weapon.mortira", "weapon.bur" },
                    new[] { "tutorial.weapon-switch" },
                    45,
                    "weapon.bur",
                    1,
                    "chapter.basics",
                    "enemy.training.precision",
                    "difficulty.normal"),
                Mission(
                    "mission.training.06",
                    "Контроль зони",
                    "Використай гранату й мортиру, щоб тиснути по площі та вибивати суперника з позиції.",
                    2,
                    "arena.central-cover",
                    new[] { "weapon.granata", "weapon.mortira", "weapon.heal" },
                    new[] { "tutorial.area-choice", "tutorial.weapon-switch" },
                    50,
                    "weapon.heal",
                    1,
                    "chapter.tactical-tools",
                    "enemy.training.utility",
                    "difficulty.normal"),
                Mission(
                    "mission.training.07",
                    "Під захистом",
                    "Збережи темп бою, підліковуючи себе в потрібний момент замість зайвого ризику.",
                    1,
                    "arena.split-ridge",
                    new[] { "weapon.bur", "weapon.impulse", "weapon.heal" },
                    new[] { "tutorial.precision", "tutorial.review" },
                    55,
                    "weapon.heal",
                    2,
                    "chapter.tactical-tools",
                    "enemy.training.utility",
                    "difficulty.hard"),
                Mission(
                    "mission.training.08",
                    "Контрольний маневр",
                    "Фінальний бій змушує комбінувати шкоду, позицію, укриття й швидке перепозиціювання.",
                    2,
                    "arena.central-cover",
                    new[] { "weapon.impulse", "weapon.mortira", "weapon.granata", "weapon.bur", "weapon.heal", "weapon.teleport" },
                    new[] { "tutorial.review" },
                    70,
                    "weapon.teleport",
                    2,
                    "chapter.tactical-tools",
                    "enemy.training.utility",
                    "difficulty.hard")
            };
        }

        private static MissionDefinition Mission(
            string missionId,
            string title,
            string description,
            int arenaIndex,
            string arenaId,
            string[] allowedWeaponIds,
            string[] tutorialStepIds,
            int softCurrency,
            string unlockWeaponId = "",
            int difficulty = 0,
            string chapterId = "",
            string enemyLoadoutId = "",
            string difficultyProfileId = "")
        {
            title = Game.Core.Localization.GameText.Localized("missionText." + missionId + ".title", title);
            description = Game.Core.Localization.GameText.Localized("missionText." + missionId + ".description", description);
            return MissionDefinition.CreateRuntime(
                missionId,
                title,
                description,
                arenaIndex,
                allowedWeaponIds,
                tutorialStepIds,
                new MissionRewardDefinition(softCurrency, unlockWeaponId),
                difficulty,
                chapterId,
                arenaId: arenaId,
                enemyLoadoutId: enemyLoadoutId,
                difficultyProfileId: difficultyProfileId);
        }
    }
}
