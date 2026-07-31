using Game.Data.Save;
using Game.Inventory;
using NUnit.Framework;

namespace Game.Tests.EditMode.Inventory
{
    public sealed class InventoryServiceTests
    {
        [Test]
        public void UnlockWeapon_AddsWeaponOnce()
        {
            var saveData = SaveData.CreateNew();
            var service = new InventoryService(saveData);

            Assert.That(service.UnlockWeapon("weapon.test.new"), Is.True);
            Assert.That(service.UnlockWeapon("weapon.test.new"), Is.False);
            Assert.That(saveData.inventory.unlockedWeaponIds, Contains.Item("weapon.test.new"));
        }

        [Test]
        public void TrySetLoadout_AcceptsOnlyUnlockedWeapons()
        {
            var saveData = SaveData.CreateNew();
            var service = new InventoryService(saveData);

            Assert.That(service.TrySetLoadout(new[] { "weapon.test.locked" }), Is.False);
            Assert.That(service.TrySetLoadout(new[] { "weapon.impulse", "weapon.blaster-caset" }), Is.True);
            Assert.That(saveData.inventory.selectedLoadoutWeaponIds, Is.EqualTo(new[] { "weapon.impulse", "weapon.blaster-caset" }));
        }

        [Test]
        public void TrySetLoadout_RemovesDuplicates()
        {
            var saveData = SaveData.CreateNew();
            var service = new InventoryService(saveData);

            Assert.That(service.TrySetLoadout(new[] { "weapon.impulse", "weapon.impulse" }), Is.True);

            Assert.That(saveData.inventory.selectedLoadoutWeaponIds, Is.EqualTo(new[] { "weapon.impulse" }));
        }
    }
}
