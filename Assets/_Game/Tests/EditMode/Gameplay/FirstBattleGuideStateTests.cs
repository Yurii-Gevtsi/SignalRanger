using Game.Gameplay.Battle.Domain;
using Game.Gameplay.Battle.Assembly;
using NUnit.Framework;

namespace Game.Tests.EditMode.Gameplay
{
    public sealed class FirstBattleGuideStateTests
    {
        [Test]
        public void FirstMission_StartsInArsenalPhase()
        {
            var guide = new FirstBattleGuideState("mission.training.01");

            Assert.That(guide.IsActive, Is.True);
            Assert.That(guide.CurrentPhase, Is.EqualTo(FirstBattleGuideState.GuidePhase.Arsenal));
            Assert.That(guide.TargetId, Is.EqualTo("battle.arsenal"));
        }

        [Test]
        public void NonTutorialMission_DoesNotActivateGuide()
        {
            var guide = new FirstBattleGuideState("mission.training.03");

            Assert.That(guide.IsActive, Is.False);
            Assert.That(guide.CurrentPhase, Is.EqualTo(FirstBattleGuideState.GuidePhase.None));
        }

        [Test]
        public void FirstMission_TransitionsAcrossFirstTurn()
        {
            var guide = new FirstBattleGuideState("mission.training.01");

            guide.ObserveWeaponSelected();
            Assert.That(guide.CurrentPhase, Is.EqualTo(FirstBattleGuideState.GuidePhase.Aim));

            guide.ObserveAimState(true);
            Assert.That(guide.CurrentPhase, Is.EqualTo(FirstBattleGuideState.GuidePhase.Fire));

            guide.ObservePlayerShotFired();
            Assert.That(guide.CurrentPhase, Is.EqualTo(FirstBattleGuideState.GuidePhase.WatchImpact));

            guide.ObserveProjectileImpact();
            Assert.That(guide.CurrentPhase, Is.EqualTo(FirstBattleGuideState.GuidePhase.OpponentTurn));

            guide.ObserveTurnAdvanced(BattleTeamId.Player);
            Assert.That(guide.CurrentPhase, Is.EqualTo(FirstBattleGuideState.GuidePhase.Done));
            Assert.That(guide.IsActive, Is.False);
        }

        [Test]
        public void FirePhase_RevertsToAimWhenInputIsLost()
        {
            var guide = new FirstBattleGuideState("mission.training.01");

            guide.ObserveWeaponSelected();
            guide.ObserveAimState(true);
            guide.ObserveAimState(false);

            Assert.That(guide.CurrentPhase, Is.EqualTo(FirstBattleGuideState.GuidePhase.Aim));
        }

        [Test]
        public void WeaponReset_ReturnsAimGuideToArsenal()
        {
            var guide = new FirstBattleGuideState("mission.training.01");
            guide.ObserveWeaponSelected();
            guide.ObserveAimState(true);

            guide.ObserveWeaponReset();

            Assert.That(guide.CurrentPhase, Is.EqualTo(FirstBattleGuideState.GuidePhase.Arsenal));
            Assert.That(guide.BlocksWeaponSwitch, Is.False);
        }

        [TestCase(FirstBattleGuideState.GuidePhase.Arsenal)]
        [TestCase(FirstBattleGuideState.GuidePhase.Aim)]
        [TestCase(FirstBattleGuideState.GuidePhase.Fire)]
        public void PlayerTimerExpired_RestartsGuideFromArsenalOnNextPlayerTurn(
            FirstBattleGuideState.GuidePhase expiredPhase)
        {
            var guide = new FirstBattleGuideState("mission.training.01");
            if (expiredPhase >= FirstBattleGuideState.GuidePhase.Aim)
            {
                guide.ObserveWeaponSelected();
            }

            if (expiredPhase >= FirstBattleGuideState.GuidePhase.Fire)
            {
                guide.ObserveAimState(true);
            }

            guide.ObservePlayerTurnExpired();

            Assert.That(guide.CurrentPhase, Is.EqualTo(FirstBattleGuideState.GuidePhase.WaitingForRetry));
            Assert.That(guide.TargetId, Is.EqualTo("battle.turn-label"));

            guide.ObserveTurnAdvanced(BattleTeamId.Player);

            Assert.That(guide.CurrentPhase, Is.EqualTo(FirstBattleGuideState.GuidePhase.Arsenal));
            Assert.That(guide.BlocksWeaponSwitch, Is.False);
            Assert.That(guide.IsActive, Is.True);
        }
    }
}
