using System;
using System.Collections.Generic;
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

        public bool HasAnyProgress()
        {
            return _saveData.tutorial.completedStepIds.Count > 0;
        }

        public TutorialStepDefinition FindNext(TutorialStepTrigger trigger, IEnumerable<string> allowedStepIds = null)
        {
            var allowed = ToAllowedSet(allowedStepIds);
            return _steps.FirstOrDefault(step =>
                step.Trigger == trigger
                && (allowed == null || allowed.Contains(step.StepId))
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

        public bool CompleteForTrigger(TutorialStepTrigger trigger, IEnumerable<string> allowedStepIds = null)
        {
            var step = FindNext(trigger, allowedStepIds);
            if (step == null || step.CompletionType == TutorialCompletionType.Manual)
            {
                return false;
            }

            return Complete(step.StepId);
        }

        private static HashSet<string> ToAllowedSet(IEnumerable<string> allowedStepIds)
        {
            if (allowedStepIds == null)
            {
                return null;
            }

            var set = new HashSet<string>(allowedStepIds.Where(id => !string.IsNullOrWhiteSpace(id)), StringComparer.Ordinal);
            return set.Count > 0 ? set : null;
        }
    }
}
