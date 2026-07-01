using System;
using System.Linq;
using Game.Data.Save;

namespace Game.Tutorial
{
    public sealed class TutorialService
    {
        private readonly SaveData _saveData;
        private readonly TutorialStepDefinition[] _steps;

        public TutorialService(SaveData saveData, TutorialStepDefinition[] steps)
        {
            _saveData = saveData ?? throw new ArgumentNullException(nameof(saveData));
            _saveData.EnsureDefaults();
            _steps = steps?.Where(step => step != null && step.IsValid()).ToArray()
                ?? Array.Empty<TutorialStepDefinition>();
        }

        public bool IsCompleted(string stepId)
        {
            return !string.IsNullOrWhiteSpace(stepId)
                && _saveData.tutorial.completedStepIds.Contains(stepId);
        }

        public TutorialStepDefinition FindNext(TutorialStepTrigger trigger)
        {
            return _steps.FirstOrDefault(step =>
                step.Trigger == trigger
                && !IsCompleted(step.StepId)
                && step.RequiredCompletedStepIds.All(IsCompleted));
        }

        public bool Complete(string stepId)
        {
            if (string.IsNullOrWhiteSpace(stepId) || IsCompleted(stepId))
            {
                return false;
            }

            _saveData.tutorial.completedStepIds.Add(stepId);
            return true;
        }

        public bool CompleteForTrigger(TutorialStepTrigger trigger)
        {
            var step = FindNext(trigger);
            if (step == null || step.CompletionType == TutorialCompletionType.Manual)
            {
                return false;
            }

            return Complete(step.StepId);
        }
    }
}
