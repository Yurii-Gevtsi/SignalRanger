using Game.Core.Battle;
using NUnit.Framework;

namespace Game.Tests.EditMode.Gameplay
{
    public sealed class BattleTutorialCueTests
    {
        [Test]
        public void EmptyCue_ReportsAsEmpty()
        {
            Assert.That(BattleTutorialCue.Empty.IsEmpty, Is.True);
        }

        [Test]
        public void Cue_WithMessage_IsNotEmpty()
        {
            var cue = new BattleTutorialCue("Aim here", "battle.aim-area");

            Assert.That(cue.IsEmpty, Is.False);
            Assert.That(cue.UiTargetId, Is.EqualTo("battle.aim-area"));
        }
    }
}
