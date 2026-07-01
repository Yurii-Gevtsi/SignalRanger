using Game.Core;
using Game.Data.Save;
using Game.Tutorial;

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

        public string ReportTrigger(string triggerKey)
        {
            if (_tutorial == null || !TryMap(triggerKey, out var trigger))
            {
                return string.Empty;
            }

            var step = _tutorial.FindNext(trigger);
            if (step == null)
            {
                return string.Empty;
            }

            var message = step.Message;
            if (_tutorial.CompleteForTrigger(trigger))
            {
                _saveService?.Save(_saveService.Current);
            }

            return message;
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
