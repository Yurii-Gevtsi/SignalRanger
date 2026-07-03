namespace Game.Tutorial
{
    public static class TutorialStepLibrary
    {
        public static TutorialStepDefinition[] CreateFirstBattleSteps()
        {
            return CreateCampaignSteps();
        }

        public static TutorialStepDefinition[] CreateCampaignSteps()
        {
            return new[]
            {
                TutorialStepDefinition.CreateRuntime(
                    "tutorial.aim",
                    "Затисни й потягни в зоні бою. Дуга покаже, куди саме піде постріл.",
                    "battle.aim-area",
                    TutorialStepTrigger.OnMissionStart,
                    TutorialCompletionType.OnTriggerObserved),
                TutorialStepDefinition.CreateRuntime(
                    "tutorial.fire",
                    "Коли траєкторія влаштовує, підтверди постріл кнопкою «Вогонь».",
                    "battle.fire-button",
                    TutorialStepTrigger.OnAimStarted,
                    TutorialCompletionType.OnTriggerObserved,
                    new[] { "tutorial.aim" }),
                TutorialStepDefinition.CreateRuntime(
                    "tutorial.impact",
                    "Стартова сфера карає за точність: чим ближче до центру удару, тим більше шкоди.",
                    "battle.impact",
                    TutorialStepTrigger.OnShotFired,
                    TutorialCompletionType.OnTriggerObserved,
                    new[] { "tutorial.fire" }),
                TutorialStepDefinition.CreateRuntime(
                    "tutorial.turn",
                    "Після пострілу хід переходить супернику. Плануй не лише удар, а й відповідь у відповідь.",
                    "battle.turn-label",
                    TutorialStepTrigger.OnProjectileImpact,
                    TutorialCompletionType.OnTriggerObserved,
                    new[] { "tutorial.impact" }),
                TutorialStepDefinition.CreateRuntime(
                    "tutorial.win",
                    "Перемога дає монети, відкриває наступну місію й підштовхує до нових інструментів.",
                    "battle.result",
                    TutorialStepTrigger.OnBattleWon,
                    TutorialCompletionType.OnMissionCompleted,
                    new[] { "tutorial.turn" }),
                TutorialStepDefinition.CreateRuntime(
                    "tutorial.arc",
                    "У цій місії веди приціл вище цілі: навісний постріл має перелетіти перепад висоти.",
                    "battle.aim-area",
                    TutorialStepTrigger.OnMissionStart,
                    TutorialCompletionType.OnTriggerObserved),
                TutorialStepDefinition.CreateRuntime(
                    "tutorial.power",
                    "Якщо дуга вже висока, дотягни силу ще трохи, щоб снаряд не впав перед суперником.",
                    "battle.power-meter",
                    TutorialStepTrigger.OnAimStarted,
                    TutorialCompletionType.OnTriggerObserved,
                    new[] { "tutorial.arc" }),
                TutorialStepDefinition.CreateRuntime(
                    "tutorial.arc-review",
                    "Після першої спроби коригуй одне з двох: або вище дуга, або трохи більша сила.",
                    "battle.status",
                    TutorialStepTrigger.OnShotFired,
                    TutorialCompletionType.OnTriggerObserved,
                    new[] { "tutorial.power" }),
                TutorialStepDefinition.CreateRuntime(
                    "tutorial.cover-target",
                    "Не шукай прямого влучання: цілься в землю або край укриття поруч із суперником.",
                    "battle.aim-area",
                    TutorialStepTrigger.OnMissionStart,
                    TutorialCompletionType.OnTriggerObserved),
                TutorialStepDefinition.CreateRuntime(
                    "tutorial.radius",
                    "Вибух поруч теж рахується. У цій місії важливіший радіус, ніж ідеальний центр попадання.",
                    "battle.impact",
                    TutorialStepTrigger.OnShotFired,
                    TutorialCompletionType.OnTriggerObserved,
                    new[] { "tutorial.cover-target" }),
                TutorialStepDefinition.CreateRuntime(
                    "tutorial.close-range",
                    "Коротка арена не про довгу дугу. Тут краще швидко оцінити ризик і вдарити без зайвого розгону.",
                    "battle.aim-area",
                    TutorialStepTrigger.OnMissionStart,
                    TutorialCompletionType.OnTriggerObserved),
                TutorialStepDefinition.CreateRuntime(
                    "tutorial.weapon-switch",
                    "Перед пострілом перевір кнопку зброї зліва: правильний інструмент часто важливіший за ідеальний кут.",
                    "battle.weapon-strip",
                    TutorialStepTrigger.OnMissionStart,
                    TutorialCompletionType.OnTriggerObserved),
                TutorialStepDefinition.CreateRuntime(
                    "tutorial.area-choice",
                    "Інколи вигідніше змінити саму арену під себе: укриття й зональний тиск створюють безпечніші ходи.",
                    "battle.weapon-strip",
                    TutorialStepTrigger.OnMissionStart,
                    TutorialCompletionType.OnTriggerObserved),
                TutorialStepDefinition.CreateRuntime(
                    "tutorial.precision",
                    "Точний постріл винагороджує терпіння: спочатку вирівняй кут, а вже потім додавай силу.",
                    "battle.aim-area",
                    TutorialStepTrigger.OnAimStarted,
                    TutorialCompletionType.OnTriggerObserved),
                TutorialStepDefinition.CreateRuntime(
                    "tutorial.review",
                    "Перед «Вогонь» перевір три речі: позицію, обрану зброю й ризик відповіді суперника.",
                    "battle.status",
                    TutorialStepTrigger.OnMissionStart,
                    TutorialCompletionType.OnTriggerObserved)
            };
        }
    }
}
