using NUnit.Framework;
using UnityEngine;
using Game.Gameplay.Battle.Motion;

namespace Game.Tests.EditMode.Gameplay
{
    public sealed class ElectricHarpoonTraversalTests
    {
        [Test]
        public void Hook_FliesInStraightLineUntilMaximumLength()
        {
            var traversal = new ElectricHarpoonTraversal("owner", Vector2.zero, Vector2.right, 2f, 4f, 2f, 5f);

            Assert.IsTrue(traversal.StepHook(0.25f));
            Assert.That(traversal.HookPosition, Is.EqualTo(new Vector2(1f, 0f)));
            Assert.IsFalse(traversal.StepHook(0.3f));
            Assert.That(traversal.HookPosition.x, Is.EqualTo(2.2f).Within(0.001f));
        }

        [Test]
        public void Hook_AttachesToTargetAndStopsAdvancing()
        {
            var traversal = new ElectricHarpoonTraversal("owner", Vector2.zero, Vector2.right, 8f, 18f, 2f, 5f);
            traversal.Attach("target", new Vector2(3f, 1f));

            Assert.IsTrue(traversal.IsAttached);
            Assert.That(traversal.TargetUnitId, Is.EqualTo("target"));
            Assert.That(traversal.HookPosition, Is.EqualTo(new Vector2(3f, 1f)));
            Assert.IsTrue(traversal.StepHook(1f));
            Assert.That(traversal.HookPosition, Is.EqualTo(new Vector2(3f, 1f)));
        }
    }
}
