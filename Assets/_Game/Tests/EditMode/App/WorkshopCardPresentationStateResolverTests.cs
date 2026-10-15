using Game.App;
using NUnit.Framework;

namespace Game.Tests.EditMode.App
{
    public sealed class WorkshopCardPresentationStateResolverTests
    {
        [Test]
        public void UnavailableBaseStat_StaysTappableOnlyForItsExplanation()
        {
            var state = WorkshopCardPresentationStateResolver.Resolve(
                isSpecial: false, isAvailable: false, specialPurchaseLimit: 0);

            Assert.That(state.IsInteractable, Is.True);
            Assert.That(state.ShowsUnavailableExplanation, Is.True);
            Assert.That(state.ShowsProgressScale, Is.False);
        }

        [Test]
        public void PurchasedOneTimeSpecial_RemainsInspectableWithoutProgressScale()
        {
            var state = WorkshopCardPresentationStateResolver.Resolve(
                isSpecial: true, isAvailable: true, specialPurchaseLimit: 1);

            Assert.That(state.IsInteractable, Is.True);
            Assert.That(state.ShowsUnavailableExplanation, Is.False);
            Assert.That(state.ShowsProgressScale, Is.False);
        }

        [Test]
        public void PurchasableSpecial_ShowsItsProgressScale()
        {
            var state = WorkshopCardPresentationStateResolver.Resolve(
                isSpecial: true, isAvailable: true, specialPurchaseLimit: 2);

            Assert.That(state.IsInteractable, Is.True);
            Assert.That(state.ShowsProgressScale, Is.True);
        }
    }
}
