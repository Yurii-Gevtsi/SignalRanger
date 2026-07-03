using Game.Gameplay.Battle.Domain;
using NUnit.Framework;

namespace Game.Tests.EditMode.Gameplay
{
    public sealed class TurnMovementStateTests
    {
        [Test]
        public void Tick_ReducesRemainingTurnSeconds()
        {
            var state = new TurnMovementState(30f);

            state.Tick(4.25f);

            Assert.That(state.RemainingTurnSeconds, Is.EqualTo(25.75f).Within(0.001f));
            Assert.That(state.HasTimeRemaining, Is.True);
        }

        [Test]
        public void Tick_ClampsAtZero()
        {
            var state = new TurnMovementState(3f);

            state.Tick(10f);

            Assert.That(state.RemainingTurnSeconds, Is.EqualTo(0f).Within(0.001f));
            Assert.That(state.HasTimeRemaining, Is.False);
        }

        [Test]
        public void Reset_RestoresTurnDuration()
        {
            var state = new TurnMovementState(30f);
            state.Tick(12f);

            state.Reset(20f);

            Assert.That(state.TurnDurationSeconds, Is.EqualTo(20f).Within(0.001f));
            Assert.That(state.RemainingTurnSeconds, Is.EqualTo(20f).Within(0.001f));
            Assert.That(state.HasTimeRemaining, Is.True);
        }
    }
}
