using Game.Data.Progression;
using NUnit.Framework;

namespace Game.Tests.EditMode.Modes
{
    public sealed class OpponentLoadoutBuilderTests
    {
        [Test]
        public void SameSeed_AlwaysProducesTheSameLoadout()
        {
            var first = OpponentLoadoutBuilder.Build(12, 0, 5, "match.42");
            var second = OpponentLoadoutBuilder.Build(12, 0, 5, "match.42");

            Assert.That(second.WeaponIds, Is.EqualTo(first.WeaponIds));
            Assert.That(second.WeaponRanks, Is.EqualTo(first.WeaponRanks));
        }

        [Test]
        public void LoadoutAlwaysEndsWithTheUnlimitedFallback()
        {
            var loadout = OpponentLoadoutBuilder.Build(20, 0, 7, "seed");

            Assert.That(loadout.WeaponIds, Contains.Item(OpponentWeaponRoleTable.FallbackWeaponId));
        }

        [Test]
        public void LevelOneOpponent_StillHasALegalMove()
        {
            var loadout = OpponentLoadoutBuilder.Build(1, 0, 3, "seed");

            Assert.That(loadout.IsEmpty, Is.False);
            Assert.That(loadout.WeaponIds.Length, Is.EqualTo(loadout.WeaponRanks.Length));
        }

        [Test]
        public void NoWeaponAboveTheCommandersLevelIsEverPicked()
        {
            var loadout = OpponentLoadoutBuilder.Build(6, 0, 5, "seed");

            foreach (var weaponId in loadout.WeaponIds)
            {
                Assert.That(WeaponProgressionCatalog.TryGet(weaponId, out var definition), Is.True, weaponId);
                Assert.That(definition.RequiredCommanderLevel, Is.LessThanOrEqualTo(6), weaponId);
            }
        }

        [Test]
        public void RanksRespectTheEncounterCap()
        {
            var loadout = OpponentLoadoutBuilder.Build(40, 2, 4, "seed");

            foreach (var rank in loadout.WeaponRanks)
            {
                Assert.That(rank, Is.InRange(1, 4));
            }
        }

        [Test]
        public void AHighLevelOpponentBringsThreeDistinctSlots()
        {
            var loadout = OpponentLoadoutBuilder.Build(15, 0, 7, "seed");

            Assert.That(loadout.WeaponIds.Length, Is.EqualTo(3));
            Assert.That(loadout.WeaponIds, Is.Unique);
        }

        [Test]
        public void ResolveRanks_MatchesTheLoadoutIndexes()
        {
            var weaponIds = new[] { "weapon.granata", "weapon.mine" };

            var ranks = OpponentLoadoutBuilder.ResolveRanks(weaponIds, 12, 0, 10);

            Assert.That(ranks.Length, Is.EqualTo(2));
            Assert.That(ranks[0], Is.EqualTo(AiWeaponRankResolver.ResolveEconomicallyPlausibleRank("weapon.granata", 12)));
            Assert.That(ranks[1], Is.EqualTo(AiWeaponRankResolver.ResolveEconomicallyPlausibleRank("weapon.mine", 12)));
        }

        [Test]
        public void EveryCatalogWeaponHasAnAuthoredSlot()
        {
            foreach (var definition in WeaponProgressionCatalog.All)
            {
                Assert.That(
                    OpponentWeaponRoleTable.TryGetSlot(definition.WeaponId, out _),
                    Is.True,
                    definition.WeaponId);
            }
        }
    }
}
