using System.Collections.Generic;
using Game.Gameplay.Battle.Net;
using NUnit.Framework;
using UnityEngine;

namespace Game.Tests.EditMode.Modes
{
    public sealed class BattleCommandAuthorityTests
    {
        private sealed class RecordingTarget : IBattleCommandTarget
        {
            public readonly List<string> Applied = new();

            public void SelectWeapon(int weaponIndex) => Applied.Add($"weapon:{weaponIndex}");
            public void SetWalkIntent(int direction) => Applied.Add($"walk:{direction}");
            public void Jump(int direction) => Applied.Add($"jump:{direction}");
            public void Fire(Vector2 origin, Vector2 direction, float power) => Applied.Add($"fire:{power}");
            public void SetProjectileInput(Vector2 direction) => Applied.Add("projectile");
            public void EndTurn() => Applied.Add("end");
        }

        [Test]
        public void ACommanderControlsOnlyTheirOwnUnits()
        {
            var authority = new BattleUnitAuthority();
            authority.Assign("player.one", "unit.a");
            authority.Assign("player.two", "unit.b");

            Assert.That(authority.CanControl("player.one", "unit.a"), Is.True);
            Assert.That(authority.CanControl("player.one", "unit.b"), Is.False);
            Assert.That(authority.ResolveOwner("unit.b"), Is.EqualTo("player.two"));
        }

        [Test]
        public void AnUnclaimedUnitIsRefusedRatherThanShared()
        {
            var authority = new BattleUnitAuthority();
            authority.Assign("player.one", "unit.a");

            Assert.That(authority.CanControl("player.one", "unit.unmapped"), Is.False);
        }

        [Test]
        public void OneCommanderCanOwnSeveralUnitsForTeamBattles()
        {
            var authority = new BattleUnitAuthority();
            authority.Assign("player.one", "unit.a1");
            authority.Assign("player.one", "unit.a2");
            authority.Assign("player.two", "unit.b1");

            Assert.That(authority.ResolveUnits("player.one"), Is.EqualTo(new[] { "unit.a1", "unit.a2" }));
            Assert.That(authority.CanControl("player.one", "unit.a2"), Is.True);
            Assert.That(authority.CanControl("player.two", "unit.a2"), Is.False);
        }

        [Test]
        public void ACommandForSomeoneElsesUnitIsRejected()
        {
            var target = new RecordingTarget();
            var dispatcher = CreateDispatcher(target);

            var applied = dispatcher.TryApply(
                BattleCommand.Jump(1, 1, "unit.b", 1), "player.one", out var rejection);

            Assert.That(applied, Is.False);
            Assert.That(rejection, Is.EqualTo(BattleCommandRejection.NotYourUnit));
            Assert.That(target.Applied, Is.Empty);
        }

        [Test]
        public void ACommandFromAFinishedTurnIsDropped()
        {
            var target = new RecordingTarget();
            var dispatcher = CreateDispatcher(target);
            dispatcher.CurrentTurnNumber = 5;

            var applied = dispatcher.TryApply(
                BattleCommand.Walk(4, 1, "unit.a", 1), "player.one", out var rejection);

            Assert.That(applied, Is.False);
            Assert.That(rejection, Is.EqualTo(BattleCommandRejection.StaleTurn));
        }

        [Test]
        public void AResentCommandNeverActsTwice()
        {
            var target = new RecordingTarget();
            var dispatcher = CreateDispatcher(target);
            var command = BattleCommand.Jump(1, 7, "unit.a", 1);

            Assert.That(dispatcher.TryApply(command, "player.one", out _), Is.True);
            Assert.That(dispatcher.TryApply(command, "player.one", out var rejection), Is.False);
            Assert.That(rejection, Is.EqualTo(BattleCommandRejection.Duplicate));
            Assert.That(target.Applied.Count, Is.EqualTo(1));
        }

        [Test]
        public void AnOutOfOrderArrivalDoesNotUndoALaterCommand()
        {
            var target = new RecordingTarget();
            var dispatcher = CreateDispatcher(target);

            Assert.That(dispatcher.TryApply(BattleCommand.Walk(1, 5, "unit.a", 1), "player.one", out _), Is.True);
            Assert.That(dispatcher.TryApply(BattleCommand.Walk(1, 3, "unit.a", -1), "player.one", out var rejection), Is.False);
            Assert.That(rejection, Is.EqualTo(BattleCommandRejection.Duplicate));
            Assert.That(target.Applied, Is.EqualTo(new[] { "walk:1" }));
        }

        [Test]
        public void EachUnitKeepsItsOwnOrdering()
        {
            var target = new RecordingTarget();
            var authority = new BattleUnitAuthority();
            authority.Assign("player.one", "unit.a1");
            authority.Assign("player.one", "unit.a2");
            var dispatcher = new BattleCommandDispatcher(target, authority);

            Assert.That(dispatcher.TryApply(BattleCommand.Walk(1, 9, "unit.a1", 1), "player.one", out _), Is.True);
            // A lower sequence on a different unit is not a duplicate: ordering is
            // per unit, which is what team battles need.
            Assert.That(dispatcher.TryApply(BattleCommand.Walk(1, 2, "unit.a2", -1), "player.one", out _), Is.True);
            Assert.That(target.Applied, Is.EqualTo(new[] { "walk:1", "walk:-1" }));
        }

        [Test]
        public void EveryCommandKindReachesTheBattle()
        {
            var target = new RecordingTarget();
            var dispatcher = CreateDispatcher(target);

            dispatcher.TryApply(BattleCommand.SelectWeapon(1, 1, "unit.a", 2), "player.one", out _);
            dispatcher.TryApply(BattleCommand.Walk(1, 2, "unit.a", -1), "player.one", out _);
            dispatcher.TryApply(BattleCommand.Jump(1, 3, "unit.a", 1), "player.one", out _);
            dispatcher.TryApply(BattleCommand.Fire(1, 4, "unit.a", Vector2.zero, Vector2.right, 15f), "player.one", out _);
            dispatcher.TryApply(BattleCommand.ProjectileInput(1, 5, "unit.a", Vector2.up), "player.one", out _);
            dispatcher.TryApply(BattleCommand.EndTurn(1, 6, "unit.a"), "player.one", out _);

            Assert.That(target.Applied, Is.EqualTo(new[]
            {
                "weapon:2", "walk:-1", "jump:1", "fire:15", "projectile", "end"
            }));
        }

        private static BattleCommandDispatcher CreateDispatcher(IBattleCommandTarget target)
        {
            var authority = new BattleUnitAuthority();
            authority.Assign("player.one", "unit.a");
            authority.Assign("player.two", "unit.b");
            return new BattleCommandDispatcher(target, authority);
        }
    }
}
