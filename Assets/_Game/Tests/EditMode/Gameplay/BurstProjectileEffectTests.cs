using System.Collections.Generic;
using Game.Gameplay.Battle.Weapons;
using NUnit.Framework;
using UnityEngine;

namespace Game.Tests.EditMode.Gameplay
{
    public sealed class BurstProjectileEffectTests
    {
        private sealed class RecordingSpawner : IProjectileSpawner
        {
            public List<ProjectileSpawnRequest> Requests { get; } = new();

            public void Spawn(in ProjectileSpawnRequest request)
            {
                Requests.Add(request);
            }
        }

        private static WeaponDefinition Weapon()
        {
            return WeaponDefinition.CreateRuntime(
                "weapon.test.burst",
                "weapon.test.burst.name",
                WeaponRole.ShortRangeBurst,
                30, 1.2f, 0.14f, 1f, 6f, 6f, 16f, -1, null, null);
        }

        [Test]
        public void Fire_SpawnsRequestedShotCount()
        {
            var spawner = new RecordingSpawner();
            var effect = new BurstProjectileEffect(shotCount: 3);
            var context = new WeaponFireContext(Weapon(), "unit.player.01", Vector2.zero, Vector2.right, 12f, null, spawner);

            effect.Fire(context);

            Assert.That(spawner.Requests, Has.Count.EqualTo(3));
        }

        [Test]
        public void Fire_DirectionsAreSymmetricAroundAim()
        {
            var spawner = new RecordingSpawner();
            var effect = new BurstProjectileEffect(shotCount: 3, spreadDegrees: 30f);
            var context = new WeaponFireContext(Weapon(), "unit.player.01", Vector2.zero, Vector2.right, 10f, null, spawner);

            effect.Fire(context);

            var middle = spawner.Requests[1].Velocity.normalized;
            var first = spawner.Requests[0].Velocity.normalized;
            var last = spawner.Requests[2].Velocity.normalized;

            Assert.That(middle.x, Is.EqualTo(1f).Within(0.02f), "Middle shot should follow the aim direction.");
            Assert.That(first.y + last.y, Is.EqualTo(0f).Within(0.02f), "Outer shots should be mirrored around the aim.");
        }

        [Test]
        public void Fire_ClampsToAtLeastOneShot()
        {
            var spawner = new RecordingSpawner();
            var effect = new BurstProjectileEffect(shotCount: 0);
            var context = new WeaponFireContext(Weapon(), "unit.player.01", Vector2.zero, Vector2.right, 10f, null, spawner);

            effect.Fire(context);

            Assert.That(spawner.Requests, Has.Count.EqualTo(1));
        }

        [Test]
        public void Fire_SplitsDamageAcrossPellets()
        {
            var spawner = new RecordingSpawner();
            var effect = new BurstProjectileEffect(shotCount: 3);
            var context = new WeaponFireContext(Weapon(), "unit.player.01", Vector2.zero, Vector2.right, 10f, null, spawner);

            effect.Fire(context);

            Assert.That(spawner.Requests[0].Definition.Damage, Is.EqualTo(10));
            Assert.That(spawner.Requests[1].Definition.Damage, Is.EqualTo(10));
            Assert.That(spawner.Requests[2].Definition.Damage, Is.EqualTo(10));
        }
    }
}
