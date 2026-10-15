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
        public void ContinueFromPull_InsideDeadZone_StaysLiveAtMinPower()
        {
            // Mid-drag the dead zone must not freeze the aim: direction keeps
            // tracking the pull, power rests at the minimum.
            var settings = new AimSettings(5f, 15f, 10f, 2f, Vector2.right);
            var current = new AimState(Vector2.zero, Vector2.up, 10f, true);

            var aim = AimCalculator.ContinueFromPull(Vector2.zero, new Vector2(-1f, 0f), settings, current);

            Assert.IsTrue(aim.HasInput);
            Assert.AreEqual(5f, aim.Power);
            Assert.That(Vector2.Distance(Vector2.right, aim.Direction), Is.LessThan(0.001f));
        }

        [Test]
        public void ContinueFromPull_NearZeroPull_KeepsCurrentDirection()
        {
            var settings = new AimSettings(5f, 15f, 10f, 2f, Vector2.right);
            var current = new AimState(Vector2.zero, Vector2.up, 10f, true);

            var aim = AimCalculator.ContinueFromPull(Vector2.zero, new Vector2(0.01f, 0f), settings, current);

            Assert.IsTrue(aim.HasInput);
            Assert.AreEqual(Vector2.up, aim.Direction);
            Assert.AreEqual(5f, aim.Power);
        }

        [Test]
        public void ContinueFromPull_PastDeadZone_MatchesCalculateFromPull()
        {
            var settings = new AimSettings(0f, 10f, 10f, 0f, Vector2.right);
            var pull = new Vector2(0f, -5f);
            var current = new AimState(Vector2.zero, Vector2.up, 3f, true);

            var continued = AimCalculator.ContinueFromPull(Vector2.zero, pull, settings, current);
            var fresh = AimCalculator.CalculateFromPull(Vector2.zero, pull, settings);

            Assert.AreEqual(fresh.Direction, continued.Direction);
            Assert.That(continued.Power, Is.EqualTo(fresh.Power).Within(0.001f));
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
