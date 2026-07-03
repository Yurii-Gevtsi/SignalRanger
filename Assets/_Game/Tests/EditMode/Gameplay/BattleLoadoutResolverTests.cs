using Game.Gameplay.Battle.Weapons;
using NUnit.Framework;
using System.Linq;

namespace Game.Tests.EditMode.Gameplay
{
    public sealed class BattleLoadoutResolverTests
    {
        private static readonly string[] All =
        {
            "weapon.steady-orb", "weapon.pulse-shell", "weapon.close-pop",
            "weapon.long-lance", "weapon.scatter-arc", "weapon.rampart-kit",
            "weapon.guard-pulse", "weapon.relay-step"
        };

        [Test]
        public void Resolve_PrefersFullSavedLoadoutOverMissionAllowed()
        {
            var loadout = new[] { "weapon.steady-orb", "weapon.pulse-shell", "weapon.long-lance" };
            var allowed = new[] { "weapon.pulse-shell", "weapon.long-lance", "weapon.close-pop" };

            var result = BattleLoadoutResolver.Resolve(loadout, allowed, All);

            Assert.That(result, Is.EqualTo(loadout));
        }

        [Test]
        public void Resolve_UsesLoadoutWhenMissionHasNoRestriction()
        {
            var loadout = new[] { "weapon.close-pop", "weapon.steady-orb" };

            var result = BattleLoadoutResolver.Resolve(loadout, null, All);

            Assert.That(result, Is.EqualTo(loadout));
        }

        [Test]
        public void Resolve_StillUsesSavedLoadoutWhenMissionAllowedIsDifferent()
        {
            var loadout = new[] { "weapon.steady-orb" };
            var allowed = new[] { "weapon.long-lance", "weapon.scatter-arc" };

            var result = BattleLoadoutResolver.Resolve(loadout, allowed, All);

            Assert.That(result, Is.EqualTo(loadout));
        }

        [Test]
        public void Resolve_FallsBackToAllowedWhenLoadoutMissing()
        {
            var allowed = new[] { "weapon.long-lance", "weapon.scatter-arc" };

            var result = BattleLoadoutResolver.Resolve(null, allowed, All);

            Assert.That(result, Is.EqualTo(allowed));
        }

        [Test]
        public void Resolve_FallsBackToAllWhenEverythingEmpty()
        {
            var result = BattleLoadoutResolver.Resolve(null, null, All);

            Assert.That(result, Is.EqualTo(All));
        }

        [Test]
        public void Resolve_DropsBlanksAndDuplicates()
        {
            var loadout = new[] { "weapon.steady-orb", "", "weapon.steady-orb", null };

            var result = BattleLoadoutResolver.Resolve(loadout, null, All);

            Assert.That(result, Is.EqualTo(new[] { "weapon.steady-orb" }));
        }

#if UNITY_EDITOR || DEVELOPMENT_BUILD
        [Test]
        public void Resolve_InEditorKeepsFullTestLoadoutAcrossMissionRestrictions()
        {
            var allowed = new[] { "weapon.steady-orb", "weapon.pulse-shell" };

            var result = BattleLoadoutResolver.Resolve(All, allowed, All);

            Assert.That(result, Is.EqualTo(All));
        }
#endif
    }
}
