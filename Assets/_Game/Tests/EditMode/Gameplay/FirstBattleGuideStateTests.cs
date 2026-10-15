using Game.Gameplay.Battle.Assembly;
using Game.Gameplay.Battle.Domain;
using NUnit.Framework;

namespace Game.Tests.EditMode.Gameplay
{
    public sealed class FirstBattleGuideStateTests
    {
        [Test]
        public void FirstMission_StartsWithLeftNavigationAndPausedTimer()
        {
            var guide = new FirstBattleGuideState("mission.training.01");

            Assert.That(guide.CurrentPhase, Is.EqualTo(FirstBattleGuideState.GuidePhase.MoveLeft));
            Assert.That(guide.TargetId, Is.EqualTo("battle.move-left"));
            Assert.That(guide.PausesPlayerTimer, Is.True);
            Assert.That(guide.ShowTurnTimer, Is.False);
        }

        [Test]
        public void FirstMission_TeachesNavigationJumpsWeaponAndFirstShot()
        {
            var guide = new FirstBattleGuideState("mission.training.01");

            guide.ObserveWalk(-1);
            guide.ObserveWalk(1);
            guide.ObserveJump(-1);
            Assert.That(guide.CurrentPhase, Is.EqualTo(FirstBattleGuideState.GuidePhase.JumpRight));

            guide.ObserveJump(1);
            Assert.That(guide.CurrentPhase, Is.EqualTo(FirstBattleGuideState.GuidePhase.ChooseFirstWeapon));
            Assert.That(guide.AllowsWeapon(FirstBattleGuideState.FirstWeaponId), Is.True);
            Assert.That(guide.AllowsWeapon("weapon.mortira"), Is.False);

            guide.ObserveWeaponSelected(FirstBattleGuideState.FirstWeaponId);
            guide.ObserveAimState(true);
            Assert.That(guide.CurrentPhase, Is.EqualTo(FirstBattleGuideState.GuidePhase.FirstFire));

            guide.ObservePlayerShotFired();
            Assert.That(guide.CurrentPhase, Is.EqualTo(FirstBattleGuideState.GuidePhase.OpponentTurn));
            Assert.That(guide.PausesPlayerTimer, Is.False);
            Assert.That(guide.ShowTurnTimer, Is.True);
        }

        [Test]
        public void OpponentTurn_LeadsToFreeFinalInstruction()
        {
            var guide = new FirstBattleGuideState("mission.training.01");
            guide.ObserveWalk(-1);
            guide.ObserveWalk(1);
            guide.ObserveJump(-1);
            guide.ObserveJump(1);
            guide.ObserveWeaponSelected(FirstBattleGuideState.FirstWeaponId);
            guide.ObserveAimState(true);
            guide.ObservePlayerShotFired();
            guide.ObserveTurnAdvanced(BattleTeamId.Player);

            Assert.That(guide.IsFinalInstruction, Is.True);
            Assert.That(guide.AllowsWalkDirection(-1), Is.True);
            Assert.That(guide.AllowsJumpDirection(1), Is.True);
            Assert.That(guide.AllowsArsenal, Is.True);

            guide.ObserveWeaponSelected("weapon.mortira");
            Assert.That(guide.IsActive, Is.False);
        }

        [Test]
        public void NonTutorialMission_DoesNotActivateGuide()
        {
            var guide = new FirstBattleGuideState("mission.training.03");

            Assert.That(guide.IsActive, Is.False);
            Assert.That(guide.CurrentPhase, Is.EqualTo(FirstBattleGuideState.GuidePhase.None));
        }

        [Test]
        public void Skip_DeactivatesTheGuideImmediately()
        {
            var guide = new FirstBattleGuideState("mission.training.01");

            guide.Skip();

            Assert.That(guide.IsActive, Is.False);
            Assert.That(guide.CurrentPhase, Is.EqualTo(FirstBattleGuideState.GuidePhase.Done));
            Assert.That(guide.AllowsWalkDirection(-1), Is.True);
        }
    }
}
