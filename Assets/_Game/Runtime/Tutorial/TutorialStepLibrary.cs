namespace Game.Tutorial
{
    public static class TutorialStepLibrary
    {
        public static TutorialStepDefinition[] CreateFirstBattleSteps()
        {
            return new[]
            {
                TutorialStepDefinition.CreateRuntime(
                    "tutorial.aim",
                    "Перетягни у зоні бою, щоб задати напрям і силу пострілу.",
                    "battle.aim-area",
                    TutorialStepTrigger.OnMissionStart,
                    TutorialCompletionType.OnTriggerObserved),
                TutorialStepDefinition.CreateRuntime(
                    "tutorial.fire",
                    "Коли приціл готовий, натисни «Вогонь».",
                    "battle.fire-button",
                    TutorialStepTrigger.OnAimStarted,
                    TutorialCompletionType.OnTriggerObserved,
                    new[] { "tutorial.aim" }),
                TutorialStepDefinition.CreateRuntime(
                    "tutorial.impact",
                    "Дивись на вибух: урон залежить від відстані до центру.",
                    "battle.impact",
                    TutorialStepTrigger.OnShotFired,
                    TutorialCompletionType.OnTriggerObserved,
                    new[] { "tutorial.fire" }),
                TutorialStepDefinition.CreateRuntime(
                    "tutorial.turn",
                    "Після пострілу хід переходить іншій стороні.",
                    "battle.turn-label",
                    TutorialStepTrigger.OnProjectileImpact,
                    TutorialCompletionType.OnTriggerObserved,
                    new[] { "tutorial.impact" }),
                TutorialStepDefinition.CreateRuntime(
                    "tutorial.win",
                    "Перемога відкриває наступну місію й видає нагороду.",
                    "battle.result",
                    TutorialStepTrigger.OnBattleWon,
                    TutorialCompletionType.OnMissionCompleted,
                    new[] { "tutorial.turn" })
            };
        }
    }
}
