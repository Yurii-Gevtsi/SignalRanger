using System.Collections.Generic;
using Game.Data.Progression;
using Game.Data.Save;
using Game.Gameplay.Battle.Weapons;
using NUnit.Framework;

namespace Game.Tests.EditMode.Modes
{
    public sealed class BattleWeaponProgressionTests
    {
        [Test]
        public void AnUndevelopedSideHasNoBonusesAtAll()
        {
            var progression = BattleWeaponProgression.Empty;

            Assert.That(progression.IsEmpty, Is.True);
            Assert.That(progression.ResolveRank("weapon.granata"), Is.EqualTo(1));
            Assert.That(progression.ResolveBranch("weapon.granata"), Is.EqualTo(0));
            Assert.That(progression.ResolveModifiers("weapon.granata").DamagePercent, Is.EqualTo(0));
            Assert.That(progression.ResolveHighestRank(), Is.EqualTo(1));
        }

        [Test]
        public void AnOpponentsRanksAndBranchesComeFromItsOwnLoadout()
        {
            var weapons = WeaponsById();

            var progression = BattleWeaponProgression.FromRanks(
                weapons,
                new[] { "weapon.granata", "weapon.impulse" },
                new[] { 6, 3 },
                new[] { (int)WeaponUpgradeBranch.B, 0 });

            Assert.That(progression.ResolveRank("weapon.granata"), Is.EqualTo(6));
            Assert.That(progression.ResolveBranch("weapon.granata"), Is.EqualTo((int)WeaponUpgradeBranch.B));
            Assert.That(progression.ResolveRank("weapon.impulse"), Is.EqualTo(3));
            Assert.That(progression.ResolveHighestRank(), Is.EqualTo(6));
        }

        [Test]
        public void ADevelopedOpponentActuallyGetsTheStatBonusesItsRankBuys()
        {
            var weapons = WeaponsById();

            var undeveloped = BattleWeaponProgression.FromRanks(
                weapons, new[] { "weapon.impulse" }, new[] { 1 }, null);
            var developed = BattleWeaponProgression.FromRanks(
                weapons, new[] { "weapon.impulse" }, new[] { 4 }, null);

            Assert.That(undeveloped.ResolveModifiers("weapon.impulse").DamagePercent, Is.EqualTo(0));
            Assert.That(developed.ResolveModifiers("weapon.impulse").DamagePercent, Is.GreaterThan(0));
        }

        [Test]
        public void ARankOneEntryIsNotRecordedBecauseItChangesNothing()
        {
            var progression = BattleWeaponProgression.FromRanks(
                WeaponsById(), new[] { "weapon.impulse" }, new[] { 1 }, null);

            Assert.That(progression.IsEmpty, Is.True);
        }

        [Test]
        public void AMissingRankLeavesTheWeaponAtBase()
        {
            var progression = BattleWeaponProgression.FromRanks(
                WeaponsById(), new[] { "weapon.granata", "weapon.impulse" }, new[] { 7 }, null);

            Assert.That(progression.ResolveRank("weapon.granata"), Is.EqualTo(7));
            Assert.That(progression.ResolveRank("weapon.impulse"), Is.EqualTo(1));
        }

        [Test]
        public void AWeaponTheBattleDoesNotKnowIsIgnoredRatherThanFatal()
        {
            var progression = BattleWeaponProgression.FromRanks(
                WeaponsById(), new[] { "weapon.nothing" }, new[] { 8 }, null);

            Assert.That(progression.IsEmpty, Is.True);
        }

        [Test]
        public void ThePlayersSideStillComesFromTheirOwnSavedProgress()
        {
            var weapons = PrototypeWeaponLibrary.CreateAll();
            var saved = new Dictionary<string, SaveWeaponProgressData>
            {
                { "weapon.granata", new SaveWeaponProgressData { weaponId = "weapon.granata", rank = 5, branch = 1 } }
            };

            var progression = BattleWeaponProgression.FromResolver(
                weapons,
                weaponId => saved.TryGetValue(weaponId, out var progress) ? progress : null);

            Assert.That(progression.ResolveRank("weapon.granata"), Is.EqualTo(5));
            Assert.That(progression.ResolveRank("weapon.impulse"), Is.EqualTo(1));
        }

        [Test]
        public void BranchesAreOnlyChosenOnceAWeaponHasEvolved()
        {
            var ids = new[] { "weapon.granata", "weapon.impulse" };
            var ranks = new[] { OpponentLoadoutBuilder.EvolutionRank, OpponentLoadoutBuilder.EvolutionRank - 1 };

            var branches = OpponentLoadoutBuilder.ResolveBranches(ids, ranks, "seed");

            Assert.That(
                branches[0],
                Is.EqualTo((int)WeaponUpgradeBranch.A).Or.EqualTo((int)WeaponUpgradeBranch.B));
            Assert.That(branches[1], Is.EqualTo((int)WeaponUpgradeBranch.None));
        }

        [Test]
        public void BranchesAreStableForASeedAndVaryAcrossSeeds()
        {
            var ids = new[] { "weapon.granata" };
            var ranks = new[] { 7 };

            Assert.That(
                OpponentLoadoutBuilder.ResolveBranches(ids, ranks, "seed.a"),
                Is.EqualTo(OpponentLoadoutBuilder.ResolveBranches(ids, ranks, "seed.a")));

            var seen = new HashSet<int>();
            for (var i = 0; i < 40; i++)
            {
                seen.Add(OpponentLoadoutBuilder.ResolveBranches(ids, ranks, $"seed.{i}")[0]);
            }

            Assert.That(seen.Count, Is.EqualTo(2));
        }

        private static Dictionary<string, WeaponDefinition> WeaponsById()
        {
            var byId = new Dictionary<string, WeaponDefinition>();
            foreach (var weapon in PrototypeWeaponLibrary.CreateAll())
            {
                if (weapon != null && !byId.ContainsKey(weapon.WeaponId))
                {
                    byId.Add(weapon.WeaponId, weapon);
                }
            }

            return byId;
        }
    }
}
