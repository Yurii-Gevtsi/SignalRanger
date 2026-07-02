using Game.Gameplay.Battle.Domain;
using NUnit.Framework;
using UnityEngine;

namespace Game.Tests.EditMode.Gameplay
{
    public sealed class AimCalculatorTests
    {
        [Test]
        public void Calculate_ReturnsFallback_WhenPointerTooClose()
        {
            var settings = new AimSettings(5f, 15f, 2f, 1f, Vector2.up);

            var aim = AimCalculator.Calculate(Vector2.zero, new Vector2(0.1f, 0.1f), settings);

            Assert.IsFalse(aim.HasInput);
            Assert.AreEqual(Vector2.up, aim.Direction);
            Assert.AreEqual(5f, aim.Power);
        }

        [Test]
        public void Calculate_NormalizesDirection()
        {
            var aim = AimCalculator.Calculate(Vector2.zero, new Vector2(3f, 4f), AimSettings.Default);

            Assert.IsTrue(aim.HasInput);
            Assert.That(Vector2.Distance(new Vector2(0.6f, 0.8f), aim.Direction), Is.LessThan(0.001f));
        }

        [Test]
        public void Calculate_ScalesPowerWithDragLength()
        {
            // minInput 0, maxInput 10, power range 0..10 => power equals drag distance.
            var settings = new AimSettings(0f, 10f, 10f, 0f, Vector2.right);

            var aim = AimCalculator.Calculate(Vector2.zero, new Vector2(5f, 0f), settings);

            Assert.That(aim.Power, Is.EqualTo(5f).Within(0.001f));
        }

        [Test]
        public void Calculate_ClampsPowerAtMaxInputDistance()
        {
            var settings = new AimSettings(5f, 10f, 10f, 0.1f, Vector2.right);

            var aim = AimCalculator.Calculate(Vector2.zero, new Vector2(100f, 0f), settings);

            Assert.AreEqual(10f, aim.Power);
        }

        [Test]
        public void Calculate_PowerIsIndependentOfTargetDistanceBeyondMax()
        {
            var settings = new AimSettings(8f, 20f, 5f, 0.35f, Vector2.right);

            var near = AimCalculator.Calculate(Vector2.zero, new Vector2(6f, 0f), settings);
            var far = AimCalculator.Calculate(Vector2.zero, new Vector2(12f, 0f), settings);

            // Both drags exceed MaxInputDistance, so both saturate to MaxPower.
            Assert.AreEqual(20f, near.Power);
            Assert.AreEqual(20f, far.Power);
        }
    }
}
