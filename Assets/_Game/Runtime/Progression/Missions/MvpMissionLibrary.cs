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
                    new[] { "weapon.steady-orb", "weapon.pulse-shell" },
                    new[] { "tutorial.aim", "tutorial.fire" },
                    25),
                Mission(
                    "mission.training.02",
                    "Дуга над схилом",
                    "Спробуй вищу траєкторію й використай силу пострілу.",
                    1,
                    new[] { "weapon.pulse-shell", "weapon.steady-orb" },
                    new[] { "tutorial.arc", "tutorial.power" },
                    30),
                Mission(
                    "mission.training.03",
                    "Укриття посередині",
                    "Влучай не тільки прямо, а й поруч із ціллю.",
                    2,
                    new[] { "weapon.pulse-shell", "weapon.scatter-arc" },
                    new[] { "tutorial.radius" },
                    35,
                    "weapon.scatter-arc"),
                Mission(
                    "mission.training.04",
                    "Ближня дистанція",
                    "Коротка арена для швидкого рішення й контролю ризику.",
                    0,
                    new[] { "weapon.close-pop", "weapon.steady-orb" },
                    new[] { "tutorial.close-range" },
                    40,
                    "weapon.close-pop"),
                Mission(
                    "mission.training.05",
                    "Вибір інструмента",
                    "Перемикай зброю під ситуацію замість одного звичного пострілу.",
                    1,
                    new[] { "weapon.steady-orb", "weapon.pulse-shell", "weapon.long-lance" },
                    new[] { "tutorial.weapon-switch" },
                    45,
                    "weapon.long-lance"),
                Mission(
                    "mission.training.06",
                    "Розсипний кут",
                    "Перевір зброю з ширшою зоною впливу на нерівній арені.",
                    2,
                    new[] { "weapon.scatter-arc", "weapon.pulse-shell" },
                    new[] { "tutorial.area-choice" },
                    50),
                Mission(
                    "mission.training.07",
                    "Точний спис",
                    "Нагорода за акуратний приціл і чисту траєкторію.",
                    1,
                    new[] { "weapon.long-lance", "weapon.steady-orb" },
                    new[] { "tutorial.precision" },
                    55),
                Mission(
                    "mission.training.08",
                    "Контрольний бій",
                    "Перший довший бій без нових правил: тільки рішення й виконання.",
                    2,
                    new[] { "weapon.steady-orb", "weapon.pulse-shell", "weapon.scatter-arc", "weapon.long-lance" },
                    new[] { "tutorial.review" },
                    70)
            };
        }

        private static MissionDefinition Mission(
            string missionId,
            string title,
            string description,
            int arenaIndex,
            string[] allowedWeaponIds,
            string[] tutorialStepIds,
            int softCurrency,
            string unlockWeaponId = "")
        {
            return MissionDefinition.CreateRuntime(
                missionId,
                title,
                description,
                arenaIndex,
                allowedWeaponIds,
                tutorialStepIds,
                new MissionRewardDefinition(softCurrency, unlockWeaponId));
        }
    }
}
