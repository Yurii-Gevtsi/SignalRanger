using Game.Gameplay.Battle.Weapons;
using NUnit.Framework;
using UnityEngine;

namespace Game.Tests.EditMode.Gameplay
{
    public sealed class WeaponExecutorTests
    {
        private sealed class RecordingSpawner : IProjectileSpawner
        {
            public int SpawnCount { get; private set; }
            public ProjectileSpawnRequest LastRequest { get; private set; }

            public void Spawn(in ProjectileSpawnRequest request)
            {
                SpawnCount++;
                LastRequest = request;
            }
        }

        private static WeaponDefinition Weapon(string id, int ammo)
        {
            return WeaponDefinition.CreateRuntime(
                id, id + ".name", WeaponRole.DirectProjectile,
                40, 1.2f, 0.12f, 0.7f, 6f, 8f, 20f, ammo, null, null);
        }

        private static WeaponFireContext Context(WeaponDefinition weapon, IProjectileSpawner spawner)
        {
            return new WeaponFireContext(
                weapon,
                "unit.player.01",
                Vector2.zero,
                Vector2.right,
                12f,
                null,
                spawner);
        }

        [Test]
        public void TryFireSelected_SpawnsProjectileFromWeaponDefinition()
        {
            var weapon = Weapon("weapon.exec.unlimited", -1);
            var inventory = new WeaponInventoryRuntime(new[] { weapon });
            var spawner = new RecordingSpawner();
            var executor = new WeaponExecutor();

            var result = executor.TryFireSelected(inventory, Context(weapon, spawner));

            Assert.That(result, Is.EqualTo(WeaponFireResult.Fired));
            Assert.That(spawner.SpawnCount, Is.EqualTo(1));
            Assert.That(spawner.LastRequest.OwnerUnitId, Is.EqualTo("unit.player.01"));
            Assert.That(spawner.LastRequest.Velocity, Is.EqualTo(Vector2.right * 12f));
            Assert.That(spawner.LastRequest.Definition.Damage, Is.EqualTo(weapon.Damage));
            Assert.That(spawner.LastRequest.Definition.ImpactRadius, Is.EqualTo(weapon.ImpactRadius).Within(0.0001f));
        }

        [Test]
        public void TryFireSelected_StopsWhenAmmoDepleted()
        {
            var weapon = Weapon("weapon.exec.single", 1);
            var inventory = new WeaponInventoryRuntime(new[] { weapon });
            var spawner = new RecordingSpawner();
            var executor = new WeaponExecutor();

            Assert.That(executor.TryFireSelected(inventory, Context(weapon, spawner)), Is.EqualTo(WeaponFireResult.Fired));
            Assert.That(executor.TryFireSelected(inventory, Context(weapon, spawner)), Is.EqualTo(WeaponFireResult.OutOfAmmo));
            Assert.That(spawner.SpawnCount, Is.EqualTo(1));
        }

        [Test]
        public void TryFireSelected_ReturnsNoWeaponForEmptyInventory()
        {
            var inventory = new WeaponInventoryRuntime(null);
            var spawner = new RecordingSpawner();
            var executor = new WeaponExecutor();

            var result = executor.TryFireSelected(inventory, Context(null, spawner));

            Assert.That(result, Is.EqualTo(WeaponFireResult.NoWeapon));
            Assert.That(spawner.SpawnCount, Is.EqualTo(0));
        }

        [Test]
        public void RegisteredEffect_OverridesDefaultForRole()
        {
            var weapon = Weapon("weapon.exec.custom", -1);
            var inventory = new WeaponInventoryRuntime(new[] { weapon });
            var executor = new WeaponExecutor();
            var custom = new CountingEffect();
            executor.RegisterEffect(WeaponRole.DirectProjectile, custom);

            var result = executor.TryFireSelected(inventory, Context(weapon, new RecordingSpawner()));

            Assert.That(result, Is.EqualTo(WeaponFireResult.Fired));
            Assert.That(custom.FireCount, Is.EqualTo(1));
        }

        [Test]
        public void RegisteredWeaponEffect_OverridesRoleEffectForSpecificWeapon()
        {
            var weapon = Weapon("weapon.exec.weapon-specific", -1);
            var inventory = new WeaponInventoryRuntime(new[] { weapon });
            var executor = new WeaponExecutor();
            var roleEffect = new CountingEffect();
            var weaponEffect = new CountingEffect();
            executor.RegisterEffect(WeaponRole.DirectProjectile, roleEffect);
            executor.RegisterEffect(weapon.WeaponId, weaponEffect);

            var result = executor.TryFireSelected(inventory, Context(weapon, new RecordingSpawner()));

            Assert.That(result, Is.EqualTo(WeaponFireResult.Fired));
            Assert.That(roleEffect.FireCount, Is.EqualTo(0));
            Assert.That(weaponEffect.FireCount, Is.EqualTo(1));
        }

        private sealed class CountingEffect : IWeaponEffect
        {
            public int FireCount { get; private set; }

            public void Fire(in WeaponFireContext context)
            {
                FireCount++;
            }
        }
    }
}
