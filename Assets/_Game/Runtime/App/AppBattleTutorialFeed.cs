using System;
using Game.Core;
using Game.Core.Battle;
using Game.Data.Save;
using Game.Tutorial;
using System.Linq;

namespace Game.App
{
    /// <summary>
    /// App-side implementation of <see cref="IBattleTutorialFeed"/>. Maps battle event
    /// keys to tutorial triggers, returns the matching step's coaching message, completes
    /// the step, and persists tutorial progress.
    /// </summary>
    public sealed class AppBattleTutorialFeed : IBattleTutorialFeed
    {
        private readonly TutorialService _tutorial;
        private readonly ISaveService _saveService;

        public AppBattleTutorialFeed(TutorialService tutorial, ISaveService saveService)
        {
            _tutorial = tutorial;
            _saveService = saveService;
        }

        public BattleTutorialCue ReportTrigger(string triggerKey)
        {
            if (_tutorial == null || !TryMap(triggerKey, out var trigger))
            {
                return BattleTutorialCue.Empty;
            }

            var activeStepIds = GameSessionContext.SelectedTutorialStepIds?.Where(stepId => !string.IsNullOrWhiteSpace(stepId));
            var step = _tutorial.FindNext(trigger, activeStepIds);
            if (step == null)
            {
                return BattleTutorialCue.Empty;
            }

            var cue = new BattleTutorialCue(step.Message, step.UiTargetId);
            if (_tutorial.CompleteForTrigger(trigger, activeStepIds))
            {
                _saveService?.Save(_saveService.Current);
            }

            return cue;
        }

        public void SkipActiveMissionTutorial()
        {
            if (_tutorial == null)
            {
                return;
            }

            var changed = false;
            foreach (var stepId in GameSessionContext.SelectedTutorialStepIds ?? Array.Empty<string>())
            {
                changed |= _tutorial.Complete(stepId);
            }

            if (changed)
            {
                _saveService?.Save(_saveService.Current);
            }
        }

        public bool IsActiveMissionTutorialCompleted()
        {
            var stepIds = GameSessionContext.SelectedTutorialStepIds;
            return _tutorial != null
                && stepIds != null
                && stepIds.Length > 0
                && stepIds.All(_tutorial.IsCompleted);
        }

        private static bool TryMap(string triggerKey, out TutorialStepTrigger trigger)
        {
            switch (triggerKey)
            {
                case BattleTutorialBridge.MissionStart:
                    trigger = TutorialStepTrigger.OnMissionStart;
                    return true;
                case BattleTutorialBridge.AimStarted:
                    trigger = TutorialStepTrigger.OnAimStarted;
                    return true;
                case BattleTutorialBridge.ShotFired:
                    trigger = TutorialStepTrigger.OnShotFired;
                    return true;
                case BattleTutorialBridge.ProjectileImpact:
                    trigger = TutorialStepTrigger.OnProjectileImpact;
                    return true;
                case BattleTutorialBridge.BattleWon:
                    trigger = TutorialStepTrigger.OnBattleWon;
                    return true;
                default:
                    trigger = default;
                    return false;
            }
        }
    }
}
