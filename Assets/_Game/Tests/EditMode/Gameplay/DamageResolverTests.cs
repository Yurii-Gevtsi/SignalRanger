using Game.Gameplay.Battle.Domain;
using NUnit.Framework;
using UnityEngine;

namespace Game.Tests.EditMode.Gameplay
{
    public sealed class DamageResolverTests
    {
        [Test]
        public void ResolveRadialDamage_AppliesMoreDamageNearImpact()
        {
            var near = new BattleUnitState("near", BattleTeamId.Player, Vector2.zero, 100);
            var far = new BattleUnitState("far", BattleTeamId.Opponent, new Vector2(1.5f, 0f), 100);
            var settings = new RadialDamageSettings(50, 2f, 1f);

            DamageResolver.ResolveRadialDamage(new[] { near, far }, Vector2.zero, string.Empty, "test", settings);

            Assert.Less(near.Health, far.Health);
        }

        [Test]
        public void ResolveRadialDamage_IgnoresUnitsOutsideRadius()
        {
            var unit = new BattleUnitState("outside", BattleTeamId.Opponent, new Vector2(5f, 0f), 100);
            var settings = new RadialDamageSettings(50, 2f, 1f);

            var events = DamageResolver.ResolveRadialDamage(new[] { unit }, Vector2.zero, string.Empty, "test", settings);

            Assert.AreEqual(0, events.Count);
            Assert.AreEqual(100, unit.Health);
        }

        [Test]
        public void ResolveRadialDamage_AppliesSelfDamageMultiplier()
        {
            var owner = new BattleUnitState("owner", BattleTeamId.Player, Vector2.zero, 100);
            var target = new BattleUnitState("target", BattleTeamId.Opponent, Vector2.zero, 100);
            var settings = new RadialDamageSettings(50, 2f, 0.5f);

            DamageResolver.ResolveRadialDamage(new[] { owner, target }, Vector2.zero, "owner", "test", settings);

            Assert.AreEqual(75, owner.Health);
            Assert.AreEqual(50, target.Health);
        }
    }
}
