using Game.Gameplay.Battle.AI;
using Game.Gameplay.Battle.Domain;
using NUnit.Framework;
using UnityEngine;

namespace Game.Tests.EditMode.Gameplay
{
    public sealed class SimpleAiShooterTests
    {
        [Test]
        public void CreateShot_PointsTowardTarget()
        {
            var shooter = new SimpleAiShooter();

            var plan = shooter.CreateShot(Vector2.zero, new Vector2(4f, 0f), AimSettings.Default);

            Assert.That(plan.Direction.x, Is.GreaterThan(0f));
            Assert.That(plan.Direction.y, Is.GreaterThan(0f));
        }

        [Test]
        public void CreateShot_ClampsPowerToAimSettings()
        {
            var shooter = new SimpleAiShooter();
            var settings = AimSettings.Default;

            var plan = shooter.CreateShot(Vector2.zero, new Vector2(20f, 0f), settings);

            Assert.That(plan.Power, Is.InRange(settings.MinPower, settings.MaxPower));
        }

        [Test]
        public void CreateShot_UsesMorePowerForFartherTarget()
        {
            var shooter = new SimpleAiShooter();
            var settings = AimSettings.Default;

            var close = shooter.CreateShot(Vector2.zero, new Vector2(2f, 0f), settings);
            var far = shooter.CreateShot(Vector2.zero, new Vector2(7f, 0f), settings);

            Assert.That(far.Power, Is.GreaterThanOrEqualTo(close.Power));
        }

        [Test]
        public void CreateShot_DifficultyChangesAimError()
        {
            var shooter = new SimpleAiShooter();
            var settings = AimSettings.Default;
            var origin = Vector2.zero;
            var target = new Vector2(6f, 0f);

            var normal = shooter.CreateShot(origin, target, settings, AiDifficulty.Normal, 2);
            var easy = shooter.CreateShot(origin, target, settings, AiDifficulty.Easy, 2);
            var hard = shooter.CreateShot(origin, target, settings, AiDifficulty.Hard, 2);

            var easyDelta = Vector2.Angle(normal.Direction, easy.Direction);
            var hardDelta = Vector2.Angle(normal.Direction, hard.Direction);

            Assert.That(easyDelta, Is.GreaterThan(hardDelta));
            Assert.That(easy.Power, Is.Not.EqualTo(hard.Power));
        }
    }
}
