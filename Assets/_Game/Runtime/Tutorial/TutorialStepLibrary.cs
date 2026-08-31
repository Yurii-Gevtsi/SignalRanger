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
                    Text("tutorial.aim"),
                    "battle.aim-area",
                    TutorialStepTrigger.OnMissionStart,
                    TutorialCompletionType.OnTriggerObserved),
                TutorialStepDefinition.CreateRuntime(
                    "tutorial.fire",
                    Text("tutorial.fire"),
                    "battle.fire-button",
                    TutorialStepTrigger.OnAimStarted,
                    TutorialCompletionType.OnTriggerObserved,
                    new[] { "tutorial.aim" }),
                TutorialStepDefinition.CreateRuntime(
                    "tutorial.impact",
                    Text("tutorial.impact"),
                    "battle.impact",
                    TutorialStepTrigger.OnShotFired,
                    TutorialCompletionType.OnTriggerObserved,
                    new[] { "tutorial.fire" }),
                TutorialStepDefinition.CreateRuntime(
                    "tutorial.turn",
                    Text("tutorial.turn"),
                    "battle.turn-label",
                    TutorialStepTrigger.OnProjectileImpact,
                    TutorialCompletionType.OnTriggerObserved,
                    new[] { "tutorial.impact" }),
                TutorialStepDefinition.CreateRuntime(
                    "tutorial.win",
                    Text("tutorial.win"),
                    "battle.result",
                    TutorialStepTrigger.OnBattleWon,
                    TutorialCompletionType.OnMissionCompleted,
                    new[] { "tutorial.turn" }),
                TutorialStepDefinition.CreateRuntime(
                    "tutorial.arc",
                    Text("tutorial.arc"),
                    "battle.aim-area",
                    TutorialStepTrigger.OnMissionStart,
                    TutorialCompletionType.OnTriggerObserved),
                TutorialStepDefinition.CreateRuntime(
                    "tutorial.power",
                    Text("tutorial.power"),
                    "battle.power-meter",
                    TutorialStepTrigger.OnAimStarted,
                    TutorialCompletionType.OnTriggerObserved,
                    new[] { "tutorial.arc" }),
                TutorialStepDefinition.CreateRuntime(
                    "tutorial.arc-review",
                    Text("tutorial.arc-review"),
                    "battle.status",
                    TutorialStepTrigger.OnShotFired,
                    TutorialCompletionType.OnTriggerObserved,
                    new[] { "tutorial.power" }),
                TutorialStepDefinition.CreateRuntime(
                    "tutorial.cover-target",
                    Text("tutorial.cover-target"),
                    "battle.aim-area",
                    TutorialStepTrigger.OnMissionStart,
                    TutorialCompletionType.OnTriggerObserved),
                TutorialStepDefinition.CreateRuntime(
                    "tutorial.radius",
                    Text("tutorial.radius"),
                    "battle.impact",
                    TutorialStepTrigger.OnShotFired,
                    TutorialCompletionType.OnTriggerObserved,
                    new[] { "tutorial.cover-target" }),
                TutorialStepDefinition.CreateRuntime(
                    "tutorial.close-range",
                    Text("tutorial.close-range"),
                    "battle.aim-area",
                    TutorialStepTrigger.OnMissionStart,
                    TutorialCompletionType.OnTriggerObserved),
                TutorialStepDefinition.CreateRuntime(
                    "tutorial.weapon-switch",
                    Text("tutorial.weapon-switch"),
                    "battle.weapon-strip",
                    TutorialStepTrigger.OnMissionStart,
                    TutorialCompletionType.OnTriggerObserved),
                TutorialStepDefinition.CreateRuntime(
                    "tutorial.area-choice",
                    Text("tutorial.area-choice"),
                    "battle.weapon-strip",
                    TutorialStepTrigger.OnMissionStart,
                    TutorialCompletionType.OnTriggerObserved),
                TutorialStepDefinition.CreateRuntime(
                    "tutorial.precision",
                    Text("tutorial.precision"),
                    "battle.aim-area",
                    TutorialStepTrigger.OnAimStarted,
                    TutorialCompletionType.OnTriggerObserved),
                TutorialStepDefinition.CreateRuntime(
                    "tutorial.review",
                    Text("tutorial.review"),
                    "battle.status",
                    TutorialStepTrigger.OnMissionStart,
                    TutorialCompletionType.OnTriggerObserved)
            };
        }

        private static string Text(string key) =>
            Game.Core.Localization.GameText.Localized("tutorialSteps." + key, string.Empty);
    }
}
