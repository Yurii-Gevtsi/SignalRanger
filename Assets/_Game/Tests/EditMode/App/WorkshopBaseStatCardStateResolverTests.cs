using Game.App;
using NUnit.Framework;

namespace Game.Tests.EditMode.App
{
    public sealed class WorkshopBaseStatCardStateResolverTests
    {
        [Test]
        public void UnpurchasedStat_ShowsItsOwnFirstLevelInsteadOfWeaponRank()
        {
            var state = WorkshopBaseStatCardStateResolver.Resolve(0, 4);

            Assert.That(state.CurrentLevel, Is.EqualTo(0));
            Assert.That(state.NextLevel, Is.EqualTo(1));
            Assert.That(state.IsAtMaximum, Is.False);
            Assert.That(state.IsSelectable, Is.True);
            Assert.That(state.ShowsPurchaseControls, Is.True);
        }

        [Test]
        public void MaximumStat_RemainsSelectableAndShowsItsFinalLevel()
        {
            var state = WorkshopBaseStatCardStateResolver.Resolve(4, 4);

            Assert.That(state.CurrentLevel, Is.EqualTo(4));
            Assert.That(state.NextLevel, Is.EqualTo(4));
            Assert.That(state.IsAtMaximum, Is.True);
            Assert.That(state.IsVisible, Is.True);
            Assert.That(state.IsSelectable, Is.True);
            Assert.That(state.ShowsPurchaseControls, Is.False);
        }

        [Test]
        public void StatWithoutAnyConfiguredLevels_RemainsVisibleButDisabled()
        {
            var state = WorkshopBaseStatCardStateResolver.Resolve(0, 0);

            Assert.That(state.IsVisible, Is.True);
            Assert.That(state.IsDisabled, Is.True);
            Assert.That(state.IsSelectable, Is.False);
            Assert.That(state.ShowsPurchaseControls, Is.False);
        }
    }
}
