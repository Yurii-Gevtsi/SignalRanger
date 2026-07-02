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
                    "weapon.close-pop",
                    1),
                Mission(
                    "mission.training.05",
                    "Вибір інструмента",
                    "Перемикай зброю під ситуацію замість одного звичного пострілу.",
                    1,
                    new[] { "weapon.steady-orb", "weapon.pulse-shell", "weapon.long-lance" },
                    new[] { "tutorial.weapon-switch" },
                    45,
                    "weapon.long-lance",
                    1),
                Mission(
                    "mission.training.06",
                    "Лінія оборони",
                    "Постав укриття там, де пряма дуель невигідна, і зміни форму арени під себе.",
                    2,
                    new[] { "weapon.scatter-arc", "weapon.pulse-shell", "weapon.rampart-kit" },
                    new[] { "tutorial.area-choice", "tutorial.weapon-switch" },
                    50,
                    "weapon.rampart-kit",
                    difficulty: 1),
                Mission(
                    "mission.training.07",
                    "Під захистом",
                    "Збережи темп бою, підліковуючи себе в потрібний момент замість зайвого ризику.",
                    1,
                    new[] { "weapon.long-lance", "weapon.steady-orb", "weapon.guard-pulse" },
                    new[] { "tutorial.precision", "tutorial.review" },
                    55,
                    "weapon.guard-pulse",
                    difficulty: 2),
                Mission(
                    "mission.training.08",
                    "Контрольний маневр",
                    "Фінальний бій змушує комбінувати шкоду, позицію, укриття й швидке перепозиціювання.",
                    2,
                    new[] { "weapon.steady-orb", "weapon.pulse-shell", "weapon.scatter-arc", "weapon.long-lance", "weapon.rampart-kit", "weapon.guard-pulse", "weapon.relay-step" },
                    new[] { "tutorial.review" },
                    70,
                    "weapon.relay-step",
                    difficulty: 2)
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
            string unlockWeaponId = "",
            int difficulty = 0)
        {
            return MissionDefinition.CreateRuntime(
                missionId,
                title,
                description,
                arenaIndex,
                allowedWeaponIds,
                tutorialStepIds,
                new MissionRewardDefinition(softCurrency, unlockWeaponId),
                difficulty);
        }
    }
}
