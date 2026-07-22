using Game.Core.Localization;
using Game.Gameplay.Battle.Domain;

namespace Game.Gameplay.Battle.Assembly
{
    public sealed class FirstBattleGuideState
    {
        public enum GuidePhase
        {
            None = 0,
            Arsenal = 1,
            Aim = 2,
            Fire = 3,
            WatchImpact = 4,
            OpponentTurn = 5,
            WaitingForRetry = 6,
            Done = 7
        }

        private const string FirstMissionId = "mission.training.01";

        /// <summary>True for the first training mission, where extra aim help (the
        /// pulsing impact-point marker) is shown.</summary>
        public static bool IsFirstMission(string missionId) => missionId == FirstMissionId;

        public FirstBattleGuideState(string missionId)
        {
            CurrentPhase = missionId == FirstMissionId ? GuidePhase.Arsenal : GuidePhase.None;
        }

        public GuidePhase CurrentPhase { get; private set; }
        public bool IsActive => CurrentPhase != GuidePhase.None && CurrentPhase != GuidePhase.Done;
        public bool BlocksWeaponSwitch => CurrentPhase != GuidePhase.Arsenal;
        public bool BlocksCancelAim => CurrentPhase == GuidePhase.Aim || CurrentPhase == GuidePhase.Fire;
        public bool BlocksRestartShortcut => false;

        public string TargetId => CurrentPhase switch
        {
            GuidePhase.Arsenal => "battle.arsenal",
            GuidePhase.Aim => "battle.aim-area",
            GuidePhase.Fire => "battle.fire-button",
            GuidePhase.WatchImpact => "battle.impact",
            GuidePhase.OpponentTurn => "battle.turn-label",
            GuidePhase.WaitingForRetry => "battle.turn-label",
            _ => string.Empty
        };

        public string Message => CurrentPhase switch
        {
            GuidePhase.Arsenal => GameText.GuideChooseFirstWeapon(),
            GuidePhase.Aim => GameText.GuideAimFirstShot(),
            GuidePhase.Fire => GameText.GuideConfirmFirstShot(),
            GuidePhase.WatchImpact => GameText.GuideWatchImpact(),
            GuidePhase.OpponentTurn => GameText.GuideWatchOpponentTurn(),
            GuidePhase.WaitingForRetry => GameText.GuideWatchOpponentTurn(),
            _ => string.Empty
        };

        public void ObserveWeaponSelected()
        {
            if (CurrentPhase == GuidePhase.Arsenal)
            {
                CurrentPhase = GuidePhase.Aim;
            }
        }

        public void ObserveWeaponReset()
        {
            if (CurrentPhase == GuidePhase.Aim || CurrentPhase == GuidePhase.Fire)
            {
                CurrentPhase = GuidePhase.Arsenal;
            }
        }

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

        public void ObservePlayerTurnExpired()
        {
            if (CurrentPhase == GuidePhase.Arsenal
                || CurrentPhase == GuidePhase.Aim
                || CurrentPhase == GuidePhase.Fire)
            {
                CurrentPhase = GuidePhase.WaitingForRetry;
            }
        }

        public void ObserveTurnAdvanced(BattleTeamId activeTeam)
        {
            if (CurrentPhase == GuidePhase.WaitingForRetry && activeTeam == BattleTeamId.Player)
            {
                CurrentPhase = GuidePhase.Arsenal;
                return;
            }

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
