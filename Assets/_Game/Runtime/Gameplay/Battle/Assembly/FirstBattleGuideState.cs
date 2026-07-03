using Game.Core.Localization;
using Game.Gameplay.Battle.Domain;

namespace Game.Gameplay.Battle.Assembly
{
    public sealed class FirstBattleGuideState
    {
        public enum GuidePhase
        {
            None = 0,
            Aim = 1,
            Fire = 2,
            WatchImpact = 3,
            OpponentTurn = 4,
            Done = 5
        }

        private const string FirstMissionId = "mission.training.01";

        public FirstBattleGuideState(string missionId)
        {
            CurrentPhase = missionId == FirstMissionId ? GuidePhase.Aim : GuidePhase.None;
        }

        public GuidePhase CurrentPhase { get; private set; }
        public bool IsActive => CurrentPhase != GuidePhase.None && CurrentPhase != GuidePhase.Done;
        public bool BlocksWeaponSwitch => CurrentPhase == GuidePhase.Aim || CurrentPhase == GuidePhase.Fire;
        public bool BlocksCancelAim => CurrentPhase == GuidePhase.Aim || CurrentPhase == GuidePhase.Fire;
        public bool BlocksRestartShortcut => false;

        public string TargetId => CurrentPhase switch
        {
            GuidePhase.Aim => "battle.aim-area",
            GuidePhase.Fire => "battle.fire-button",
            GuidePhase.WatchImpact => "battle.impact",
            GuidePhase.OpponentTurn => "battle.turn-label",
            _ => string.Empty
        };

        public string Message => CurrentPhase switch
        {
            GuidePhase.Aim => GameText.GuideAimFirstShot(),
            GuidePhase.Fire => GameText.GuideConfirmFirstShot(),
            GuidePhase.WatchImpact => GameText.GuideWatchImpact(),
            GuidePhase.OpponentTurn => GameText.GuideWatchOpponentTurn(),
            _ => string.Empty
        };

        public void ObserveAimState(bool hasAimInput)
        {
            if (CurrentPhase == GuidePhase.Aim && hasAimInput)
            {
                CurrentPhase = GuidePhase.Fire;
                return;
            }

            if (CurrentPhase == GuidePhase.Fire && !hasAimInput)
            {
                CurrentPhase = GuidePhase.Aim;
            }
        }

        public void ObservePlayerShotFired()
        {
            if (CurrentPhase == GuidePhase.Aim || CurrentPhase == GuidePhase.Fire)
            {
                CurrentPhase = GuidePhase.WatchImpact;
            }
        }

        public void ObserveProjectileImpact()
        {
            if (CurrentPhase == GuidePhase.WatchImpact)
            {
                CurrentPhase = GuidePhase.OpponentTurn;
            }
        }

        public void ObserveTurnAdvanced(BattleTeamId activeTeam)
        {
            if (CurrentPhase == GuidePhase.OpponentTurn && activeTeam == BattleTeamId.Player)
            {
                CurrentPhase = GuidePhase.Done;
            }
        }

        public void ObserveBattleFinished()
        {
            if (CurrentPhase != GuidePhase.None)
            {
                CurrentPhase = GuidePhase.Done;
            }
        }
    }
}
