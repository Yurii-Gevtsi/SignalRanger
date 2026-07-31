using Game.Core;
using Game.Core.Localization;
using Game.Gameplay.Battle.Domain;

namespace Game.Gameplay.Battle.Assembly
{
    /// <summary>
    /// Mandatory first-battle lesson. The controller validates input while this
    /// state describes the current teaching step and its HUD target.
    /// </summary>
    public sealed class FirstBattleGuideState
    {
        public enum GuidePhase
        {
            None = 0,
            MoveLeft = 1,
            MoveRight = 2,
            JumpLeft = 3,
            JumpRight = 4,
            ChooseFirstWeapon = 5,
            FirstAim = 6,
            FirstFire = 7,
            OpponentTurn = 8,
            FinalInstruction = 9,
            WaitingForRetry = 10,
            Done = 11
        }

        public const string FirstWeaponId = "weapon.impulse";
        private const string FirstMissionId = "mission.training.01";

        public static bool IsFirstMission(string missionId) => missionId == FirstMissionId && !GameSessionContext.IsTrainingSkirmish;

        public FirstBattleGuideState(string missionId)
        {
            CurrentPhase = IsFirstMission(missionId) ? GuidePhase.MoveLeft : GuidePhase.None;
        }

        public GuidePhase CurrentPhase { get; private set; }
        public bool IsActive => CurrentPhase != GuidePhase.None && CurrentPhase != GuidePhase.Done;
        public bool IsFinalInstruction => CurrentPhase == GuidePhase.FinalInstruction;
        public bool PausesPlayerTimer => CurrentPhase == GuidePhase.MoveLeft
            || CurrentPhase == GuidePhase.MoveRight
            || CurrentPhase == GuidePhase.JumpLeft
            || CurrentPhase == GuidePhase.JumpRight
            || CurrentPhase == GuidePhase.ChooseFirstWeapon
            || CurrentPhase == GuidePhase.FirstAim
            || CurrentPhase == GuidePhase.FirstFire
            || CurrentPhase == GuidePhase.WaitingForRetry;
        public bool ShowTurnTimer => !PausesPlayerTimer;
        public bool AllowsAimInput => CurrentPhase == GuidePhase.FirstAim
            || CurrentPhase == GuidePhase.FirstFire
            || IsFinalInstruction
            || CurrentPhase == GuidePhase.Done;
        public bool AllowsArsenal => CurrentPhase == GuidePhase.ChooseFirstWeapon
            || IsFinalInstruction
            || CurrentPhase == GuidePhase.Done;
        public bool AllowsFire => CurrentPhase == GuidePhase.FirstFire
            || IsFinalInstruction
            || CurrentPhase == GuidePhase.Done;
        public bool BlocksWeaponSwitch => CurrentPhase == GuidePhase.ChooseFirstWeapon;
        public bool BlocksCancelAim => CurrentPhase == GuidePhase.FirstAim || CurrentPhase == GuidePhase.FirstFire;
        public bool BlocksRestartShortcut => false;

        public string TargetId => CurrentPhase switch
        {
            GuidePhase.MoveLeft => "battle.move-left",
            GuidePhase.MoveRight => "battle.move-right",
            GuidePhase.JumpLeft => "battle.jump-left",
            GuidePhase.JumpRight => "battle.jump-right",
            GuidePhase.ChooseFirstWeapon => "battle.arsenal-impulse",
            GuidePhase.FirstAim => "battle.aim-area",
            GuidePhase.FirstFire => "battle.fire-button",
            GuidePhase.OpponentTurn => "battle.turn-label",
            _ => string.Empty
        };

        public string Message => CurrentPhase switch
        {
            GuidePhase.MoveLeft => Text("tutorial.firstBattle.moveLeft", "Navigation: hold the highlighted left arrow and drive left."),
            GuidePhase.MoveRight => Text("tutorial.firstBattle.moveRight", "Now hold the right arrow and drive right."),
            GuidePhase.JumpLeft => Text("tutorial.firstBattle.jumpLeft", "Obstacle on the left: press the jump button. Reactive boosters let the droid jump even on tracks."),
            GuidePhase.JumpRight => Text("tutorial.firstBattle.jumpRight", "Jump over the obstacle on the right. Reactive boosters help the droid clear the gap."),
            GuidePhase.ChooseFirstWeapon => Text("tutorial.firstBattle.chooseFirstWeapon", "First shot\nOpen the arsenal and select the Impulse Cannon to fire."),
            GuidePhase.FirstAim => Text("tutorial.firstBattle.firstAim", "Aiming: touch an open area and drag. Drag direction sets the angle; drag length sets shot power. Release to lock the arc."),
            GuidePhase.FirstFire => Text("tutorial.firstBattle.firstFire", "Trajectory locked. Press the highlighted fire button."),
            GuidePhase.OpponentTurn => Text("tutorial.firstBattle.opponentTurn", "Advance: Sigma always seeks to destroy you and preserve control of the Galaxy. Watch its turn and prepare to respond."),
            GuidePhase.FinalInstruction => Text("tutorial.firstBattle.finalInstruction", "You have learned navigation, jumping, and aiming. Finish the battle by destroying Sigma."),
            GuidePhase.WaitingForRetry => Text("tutorial.firstBattle.retry", "The turn ended. Wait for the next turn; the lesson will restart with navigation."),
            _ => string.Empty
        };

        public bool AllowsWalkDirection(int direction) => CurrentPhase switch
        {
            GuidePhase.MoveLeft => direction < 0,
            GuidePhase.MoveRight => direction > 0,
            GuidePhase.FinalInstruction => direction != 0,
            GuidePhase.Done => direction != 0,
            _ => false
        };

        public bool AllowsJumpDirection(int direction) => CurrentPhase switch
        {
            GuidePhase.JumpLeft => direction < 0,
            GuidePhase.JumpRight => direction > 0,
            GuidePhase.FinalInstruction => direction != 0,
            GuidePhase.Done => direction != 0,
            _ => false
        };

        public bool AllowsWeapon(string weaponId) => CurrentPhase == GuidePhase.ChooseFirstWeapon
            ? weaponId == FirstWeaponId
            : !IsActive || IsFinalInstruction;

        public void ObserveWalk(int direction)
        {
            if (CurrentPhase == GuidePhase.MoveLeft && direction < 0)
            {
                CurrentPhase = GuidePhase.MoveRight;
            }
            else if (CurrentPhase == GuidePhase.MoveRight && direction > 0)
            {
                CurrentPhase = GuidePhase.JumpLeft;
            }
            else if (IsFinalInstruction)
            {
                CurrentPhase = GuidePhase.Done;
            }
        }

        public void ObserveJump(int direction)
        {
            if (CurrentPhase == GuidePhase.JumpLeft && direction < 0)
            {
                CurrentPhase = GuidePhase.JumpRight;
            }
            else if (CurrentPhase == GuidePhase.JumpRight && direction > 0)
            {
                CurrentPhase = GuidePhase.ChooseFirstWeapon;
            }
            else if (IsFinalInstruction)
            {
                CurrentPhase = GuidePhase.Done;
            }
        }

        public void ObserveWeaponSelected(string weaponId)
        {
            if (CurrentPhase == GuidePhase.ChooseFirstWeapon && weaponId == FirstWeaponId)
            {
                CurrentPhase = GuidePhase.FirstAim;
            }
            else if (IsFinalInstruction)
            {
                CurrentPhase = GuidePhase.Done;
            }
        }

        public void ObserveAimState(bool hasAimInput)
        {
            if (!hasAimInput)
            {
                return;
            }

            if (CurrentPhase == GuidePhase.FirstAim)
            {
                CurrentPhase = GuidePhase.FirstFire;
            }
            else if (IsFinalInstruction)
            {
                CurrentPhase = GuidePhase.Done;
            }
        }

        public void ObservePlayerShotFired()
        {
            if (CurrentPhase == GuidePhase.FirstFire)
            {
                CurrentPhase = GuidePhase.OpponentTurn;
            }
            else if (IsFinalInstruction)
            {
                CurrentPhase = GuidePhase.Done;
            }
        }

        public void ObserveProjectileImpact()
        {
        }

        public void ObservePlayerTurnExpired()
        {
            if (IsActive && PausesPlayerTimer)
            {
                CurrentPhase = GuidePhase.WaitingForRetry;
            }
        }

        public void ObserveTurnAdvanced(BattleTeamId activeTeam)
        {
            if (CurrentPhase == GuidePhase.WaitingForRetry && activeTeam == BattleTeamId.Player)
            {
                CurrentPhase = GuidePhase.MoveLeft;
            }
            else if (CurrentPhase == GuidePhase.OpponentTurn && activeTeam == BattleTeamId.Player)
            {
                CurrentPhase = GuidePhase.FinalInstruction;
            }
        }

        public void ObserveBattleFinished()
        {
            if (CurrentPhase != GuidePhase.None)
            {
                CurrentPhase = GuidePhase.Done;
            }
        }

        private static string Text(string key, string fallback) => GameText.Localized(key, fallback);
    }
}
