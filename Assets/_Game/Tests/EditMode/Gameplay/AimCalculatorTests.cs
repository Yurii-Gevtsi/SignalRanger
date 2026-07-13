using Game.Gameplay.Battle.Domain;
using NUnit.Framework;
using UnityEngine;

namespace Game.Tests.EditMode.Gameplay
{
    public sealed class AimCalculatorTests
    {
        [Test]
        public void CalculateFromPull_ReturnsFallback_WhenPullTooShort()
        {
            var settings = new AimSettings(5f, 15f, 2f, 1f, Vector2.up);

            var aim = AimCalculator.CalculateFromPull(Vector2.zero, new Vector2(0.1f, 0.1f), settings);

            Assert.IsFalse(aim.HasInput);
            Assert.AreEqual(Vector2.up, aim.Direction);
            Assert.AreEqual(5f, aim.Power);
        }

        [Test]
        public void CalculateFromPull_LaunchesOppositeToPull()
        {
            // Pull down-left => the shot launches up-right, like a slingshot.
            var aim = AimCalculator.CalculateFromPull(Vector2.zero, new Vector2(-3f, -4f), AimSettings.Default);

            Assert.IsTrue(aim.HasInput);
            Assert.That(Vector2.Distance(new Vector2(0.6f, 0.8f), aim.Direction), Is.LessThan(0.001f));
        }

        [Test]
        public void CalculateFromPull_ScalesPowerWithPullLength()
        {
            // minInput 0, maxInput 10, power range 0..10 => power equals pull distance.
            var settings = new AimSettings(0f, 10f, 10f, 0f, Vector2.right);

            var aim = AimCalculator.CalculateFromPull(Vector2.zero, new Vector2(0f, -5f), settings);

            Assert.That(aim.Power, Is.EqualTo(5f).Within(0.001f));
        }

        [Test]
        public void CalculateFromPull_ClampsPowerAtMaxInputDistance()
        {
            var settings = new AimSettings(5f, 10f, 10f, 0.1f, Vector2.right);

            var aim = AimCalculator.CalculateFromPull(Vector2.zero, new Vector2(-100f, 0f), settings);

            Assert.AreEqual(10f, aim.Power);
        }

        [Test]
        public void CalculateFromPull_KeepsOrigin_AndProjectsTargetAlongLaunch()
        {
            // The pull is independent of the origin, so the arc still starts at the
            // firing unit while the finger rests wherever the press began.
            var settings = new AimSettings(0f, 10f, 10f, 0f, Vector2.right);
            var origin = new Vector2(2f, 1f);

            var aim = AimCalculator.CalculateFromPull(origin, new Vector2(-3f, 0f), settings);

            Assert.AreEqual(origin, aim.Origin);
            Assert.That(Vector2.Distance(new Vector2(5f, 1f), aim.Target), Is.LessThan(0.001f));
        }
    }
}
