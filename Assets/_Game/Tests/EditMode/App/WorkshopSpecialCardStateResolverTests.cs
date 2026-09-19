using Game.App;
using NUnit.Framework;

namespace Game.Tests.EditMode.App
{
    public sealed class WorkshopSpecialCardStateResolverTests
    {
        [Test]
        public void Resolve_KeepsSpecialLockedBeforeItsUnlockRank()
        {
            var state = WorkshopSpecialCardStateResolver.Resolve(
                playerRank: 4, nextPurchaseRank: 5, hasPurchasedSpecial: false);

            Assert.That(state.IsAvailable, Is.False);
            Assert.That(state.IsPurchased, Is.False);
        }

        [Test]
        public void Resolve_UnlocksTheNextPurchasableSpecial()
        {
            var state = WorkshopSpecialCardStateResolver.Resolve(
                playerRank: 5, nextPurchaseRank: 5, hasPurchasedSpecial: false);

            Assert.That(state.IsAvailable, Is.True);
            Assert.That(state.IsPurchased, Is.False);
        }

        [Test]
        public void Resolve_KeepsACompletedOneTimeSpecialAvailableForInspection()
        {
            var state = WorkshopSpecialCardStateResolver.Resolve(
                playerRank: 5, nextPurchaseRank: 0, hasPurchasedSpecial: true);

            Assert.That(state.IsAvailable, Is.True);
            Assert.That(state.IsPurchased, Is.True);
        }
    }
}
