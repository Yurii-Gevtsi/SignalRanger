using Game.Gameplay.Battle.Domain;
using Game.Gameplay.Battle.Assembly;
using NUnit.Framework;

namespace Game.Tests.EditMode.Gameplay
{
    public sealed class FirstBattleGuideStateTests
    {
        [Test]
        public void FirstMission_StartsInAimPhase()
        {
            var guide = new FirstBattleGuideState("mission.training.01");

            Assert.That(guide.IsActive, Is.True);
            Assert.That(guide.CurrentPhase, Is.EqualTo(FirstBattleGuideState.GuidePhase.Aim));
            Assert.That(guide.TargetId, Is.EqualTo("battle.aim-area"));
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

            guide.ObserveAimState(true);
            guide.ObserveAimState(false);

            Assert.That(guide.CurrentPhase, Is.EqualTo(FirstBattleGuideState.GuidePhase.Aim));
        }
    }
}
