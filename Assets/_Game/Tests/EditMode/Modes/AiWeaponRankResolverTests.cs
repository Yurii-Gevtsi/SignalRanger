using Game.Data.Progression;
using NUnit.Framework;

namespace Game.Tests.EditMode.Modes
{
    public sealed class AiWeaponRankResolverTests
    {
        [Test]
        public void UnknownWeapon_StaysAtRankOne()
        {
            Assert.That(AiWeaponRankResolver.ResolveEconomicallyPlausibleRank("weapon.nothing", 50), Is.EqualTo(1));
        }

        [Test]
        public void WeaponNotUnlockedYet_StaysAtRankOne()
        {
            // weapon.fpv-drone unlocks at commander level 15.
            Assert.That(AiWeaponRankResolver.ResolveEconomicallyPlausibleRank("weapon.fpv-drone", 3), Is.EqualTo(1));
        }

        [Test]
        public void RankLadder_MatchesTheAuthoredLevelOffsets()
        {
            // weapon.impulse unlocks at level 1, so the ladder is
            // rank 2 at +0, 3 at +1, 4 at +2, 5 at +3, 6 at +5,
            // 7 at +7, 8 at +9, 9 at +12, 10 at +15.
            Assert.That(AiWeaponRankResolver.ResolveEconomicallyPlausibleRank("weapon.impulse", 1), Is.EqualTo(2));
            Assert.That(AiWeaponRankResolver.ResolveEconomicallyPlausibleRank("weapon.impulse", 2), Is.EqualTo(3));
            Assert.That(AiWeaponRankResolver.ResolveEconomicallyPlausibleRank("weapon.impulse", 3), Is.EqualTo(4));
            Assert.That(AiWeaponRankResolver.ResolveEconomicallyPlausibleRank("weapon.impulse", 4), Is.EqualTo(5));
            Assert.That(AiWeaponRankResolver.ResolveEconomicallyPlausibleRank("weapon.impulse", 5), Is.EqualTo(5));
            Assert.That(AiWeaponRankResolver.ResolveEconomicallyPlausibleRank("weapon.impulse", 6), Is.EqualTo(6));
            Assert.That(AiWeaponRankResolver.ResolveEconomicallyPlausibleRank("weapon.impulse", 8), Is.EqualTo(7));
            Assert.That(AiWeaponRankResolver.ResolveEconomicallyPlausibleRank("weapon.impulse", 10), Is.EqualTo(8));
            Assert.That(AiWeaponRankResolver.ResolveEconomicallyPlausibleRank("weapon.impulse", 13), Is.EqualTo(9));
            Assert.That(AiWeaponRankResolver.ResolveEconomicallyPlausibleRank("weapon.impulse", 16), Is.EqualTo(10));
        }

        [Test]
        public void RankNeverExceedsTheCapstone()
        {
            Assert.That(AiWeaponRankResolver.ResolveEconomicallyPlausibleRank("weapon.impulse", 999), Is.EqualTo(10));
        }

        [Test]
        public void EveryCatalogWeapon_ReachesRankTenAtItsCapstoneLevel()
        {
            foreach (var definition in WeaponProgressionCatalog.All)
            {
                var capstoneLevel = WeaponUpgradeEconomy.GetRequiredCommanderLevel(definition, 10);

                Assert.That(
                    AiWeaponRankResolver.ResolveEconomicallyPlausibleRank(definition.WeaponId, capstoneLevel),
                    Is.EqualTo(10),
                    definition.WeaponId);
                Assert.That(
                    AiWeaponRankResolver.ResolveEconomicallyPlausibleRank(definition.WeaponId, capstoneLevel - 1),
                    Is.EqualTo(9),
                    definition.WeaponId);
            }
        }

        [Test]
        public void EncounterRank_IsHeldUnderTheCapWhateverTheBonus()
        {
            Assert.That(AiWeaponRankResolver.ResolveEncounterRank("weapon.impulse", 30, 3, 7), Is.EqualTo(7));
        }

        [Test]
        public void EncounterRank_NeverDropsBelowOne()
        {
            Assert.That(AiWeaponRankResolver.ResolveEncounterRank("weapon.fpv-drone", 1, -1, 3), Is.EqualTo(1));
        }

        [Test]
        public void EasyPressure_ShiftsOneRankDown()
        {
            var plain = AiWeaponRankResolver.ResolveEncounterRank("weapon.granata", 12, 0, 10);
            var easy = AiWeaponRankResolver.ResolveEncounterRank("weapon.granata", 12, -1, 10);

            Assert.That(easy, Is.EqualTo(plain - 1));
        }
    }
}
