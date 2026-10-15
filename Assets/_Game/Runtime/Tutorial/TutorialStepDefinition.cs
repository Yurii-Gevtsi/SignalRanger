using System;
using UnityEngine;

namespace Game.Tutorial
{
    [CreateAssetMenu(menuName = "Game/Tutorial/Tutorial Step", fileName = "TutorialStepDefinition")]
    public sealed class TutorialStepDefinition : ScriptableObject
    {
        [SerializeField] private string _stepId;
        [SerializeField] private string _message;
        [SerializeField] private string _uiTargetId;
        [SerializeField] private TutorialStepTrigger _trigger;
        [SerializeField] private TutorialCompletionType _completionType;
        [SerializeField] private string[] _requiredCompletedStepIds;

        public string StepId => _stepId;
        public string Message => _message;
        public string UiTargetId => _uiTargetId;
        public TutorialStepTrigger Trigger => _trigger;
        public TutorialCompletionType CompletionType => _completionType;
        public string[] RequiredCompletedStepIds => _requiredCompletedStepIds ?? Array.Empty<string>();

        public static TutorialStepDefinition CreateRuntime(
            string stepId,
            string message,
            string uiTargetId,
            TutorialStepTrigger trigger,
            TutorialCompletionType completionType,
            string[] requiredCompletedStepIds = null)
        {
            var definition = CreateInstance<TutorialStepDefinition>();
            definition._stepId = stepId ?? string.Empty;
            definition._message = message ?? string.Empty;
            definition._uiTargetId = uiTargetId ?? string.Empty;
            definition._trigger = trigger;
            definition._completionType = completionType;
            definition._requiredCompletedStepIds = requiredCompletedStepIds ?? Array.Empty<string>();
            return definition;
        }

        public bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(_stepId)
                && !string.IsNullOrWhiteSpace(_message);
        }
    }
}
