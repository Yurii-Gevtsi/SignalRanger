using Game.Gameplay.Battle.Weapons;
using NUnit.Framework;

namespace Game.Tests.EditMode.Gameplay
{
    public sealed class WeaponInventoryRuntimeTests
    {
        private static WeaponDefinition LimitedWeapon(string id, int ammo)
        {
            return WeaponDefinition.CreateRuntime(
                id, id + ".name", WeaponRole.DirectProjectile,
                30, 1f, 0.1f, 0.6f, 6f, 8f, 18f, ammo, null, null);
        }

        [Test]
        public void Selection_WrapsAroundAndTracksIndex()
        {
            var inventory = new WeaponInventoryRuntime(PrototypeWeaponLibrary.CreateAll());

            inventory.Select(0);
            Assert.That(inventory.SelectedIndex, Is.EqualTo(0));

            inventory.SelectPrevious();
            Assert.That(inventory.SelectedIndex, Is.EqualTo(inventory.Count - 1));

            inventory.SelectNext();
            Assert.That(inventory.SelectedIndex, Is.EqualTo(0));
        }

        [Test]
        public void UnlimitedAmmo_NeverRunsOut()
        {
            var inventory = new WeaponInventoryRuntime(PrototypeWeaponLibrary.CreateAll());

            for (var i = 0; i < 5; i++)
            {
                Assert.That(inventory.CanFireSelected, Is.True);
                Assert.That(inventory.TryConsumeSelectedAmmo(), Is.True);
            }

            Assert.That(inventory.GetSelectedRemainingAmmo(), Is.EqualTo(-1));
        }

        [Test]
        public void LimitedAmmo_DecrementsAndBlocksAtZero()
        {
            var inventory = new WeaponInventoryRuntime(new[] { LimitedWeapon("weapon.test.limited", 2) });

            Assert.That(inventory.GetSelectedRemainingAmmo(), Is.EqualTo(2));

            Assert.That(inventory.TryConsumeSelectedAmmo(), Is.True);
            Assert.That(inventory.TryConsumeSelectedAmmo(), Is.True);
            Assert.That(inventory.GetSelectedRemainingAmmo(), Is.EqualTo(0));

            Assert.That(inventory.CanFireSelected, Is.False);
            Assert.That(inventory.TryConsumeSelectedAmmo(), Is.False);
            Assert.That(inventory.GetSelectedRemainingAmmo(), Is.EqualTo(0));
        }

        [Test]
        public void EmptyInventory_HasNoSelectionAndCannotFire()
        {
            var inventory = new WeaponInventoryRuntime(null);

            Assert.That(inventory.HasWeapons, Is.False);
            Assert.That(inventory.Selected, Is.Null);
            Assert.That(inventory.CanFireSelected, Is.False);
            Assert.That(inventory.TryConsumeSelectedAmmo(), Is.False);
        }

        [Test]
        public void DuplicateWeaponIds_AreIgnored()
        {
            var weapon = LimitedWeapon("weapon.test.dup", 3);
            var inventory = new WeaponInventoryRuntime(new[] { weapon, weapon });

            Assert.That(inventory.Count, Is.EqualTo(1));
        }
    }
}
