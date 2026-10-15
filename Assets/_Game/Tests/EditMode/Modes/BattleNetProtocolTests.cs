using System.Collections.Generic;
using Game.Gameplay.Battle.Net;
using NUnit.Framework;
using UnityEngine;

namespace Game.Tests.EditMode.Modes
{
    public sealed class BattleNetProtocolTests
    {
        [Test]
        public void ACommandSurvivesTheWireUnchanged()
        {
            var original = BattleCommand.Fire(
                7, 42, "player.commander", new Vector2(1.25f, -3.5f), new Vector2(0.6f, 0.8f), 18.75f);

            Assert.That(BattleCommandCodec.TryDecode(BattleCommandCodec.Encode(original), out var decoded), Is.True);
            Assert.That(decoded.TurnNumber, Is.EqualTo(original.TurnNumber));
            Assert.That(decoded.Sequence, Is.EqualTo(original.Sequence));
            Assert.That(decoded.ActorUnitId, Is.EqualTo(original.ActorUnitId));
            Assert.That(decoded.Kind, Is.EqualTo(original.Kind));
            Assert.That(decoded.Origin, Is.EqualTo(original.Origin));
            Assert.That(decoded.Direction, Is.EqualTo(original.Direction));
            Assert.That(decoded.Power, Is.EqualTo(original.Power));
        }

        [Test]
        public void EveryCommandKindRoundTrips()
        {
            var commands = new[]
            {
                BattleCommand.SelectWeapon(1, 1, "unit.a", 3),
                BattleCommand.Walk(1, 2, "unit.a", -1),
                BattleCommand.Jump(1, 3, "unit.a", 1),
                BattleCommand.Fire(1, 4, "unit.a", Vector2.one, Vector2.right, 12f),
                BattleCommand.ProjectileInput(1, 5, "unit.a", Vector2.up),
                BattleCommand.EndTurn(1, 6, "unit.a")
            };

            foreach (var command in commands)
            {
                Assert.That(BattleCommandCodec.TryDecode(BattleCommandCodec.Encode(command), out var decoded), Is.True);
                Assert.That(decoded.Kind, Is.EqualTo(command.Kind), command.Kind.ToString());
                Assert.That(decoded.Value, Is.EqualTo(command.Value), command.Kind.ToString());
            }
        }

        [Test]
        public void ACommandStaysSmallEnoughToSendPerInput()
        {
            var bytes = BattleCommandCodec.Encode(
                BattleCommand.Fire(1, 1, "player.commander", Vector2.one, Vector2.right, 20f));

            Assert.That(bytes.Length, Is.LessThan(64));
        }

        [Test]
        public void GarbageAndTruncatedPayloadsAreRefusedRatherThanGuessedAt()
        {
            Assert.That(BattleCommandCodec.TryDecode(null, out _), Is.False);
            Assert.That(BattleCommandCodec.TryDecode(new byte[] { 9, 9, 9 }, out _), Is.False);

            var valid = BattleCommandCodec.Encode(BattleCommand.Walk(1, 1, "unit.a", 1));
            var wrongVersion = (byte[])valid.Clone();
            wrongVersion[0] = 99;
            Assert.That(BattleCommandCodec.TryDecode(wrongVersion, out _), Is.False);
        }

        [Test]
        public void ASnapshotSurvivesTheWireUnchanged()
        {
            var snapshot = new BattleTurnSnapshot(
                12,
                "unit.b",
                new List<BattleUnitSnapshot>
                {
                    new("unit.a", new Vector2(-4.5f, 1.25f), 63, -1),
                    new("unit.b", new Vector2(6.75f, 0.5f), 100, 1)
                },
                new List<BattleCraterSnapshot>
                {
                    new(new Vector2(2f, 0.5f), 1.4f)
                });

            Assert.That(BattleTurnSnapshotCodec.TryDecode(BattleTurnSnapshotCodec.Encode(snapshot), out var decoded), Is.True);
            Assert.That(decoded.TurnNumber, Is.EqualTo(12));
            Assert.That(decoded.ActiveUnitId, Is.EqualTo("unit.b"));
            Assert.That(decoded.Units.Count, Is.EqualTo(2));
            Assert.That(decoded.Units[0].Position, Is.EqualTo(new Vector2(-4.5f, 1.25f)));
            Assert.That(decoded.Units[0].Health, Is.EqualTo(63));
            Assert.That(decoded.Units[0].FacingDirectionX, Is.EqualTo(-1));
            Assert.That(decoded.Craters.Count, Is.EqualTo(1));
            Assert.That(decoded.Craters[0].Radius, Is.EqualTo(1.4f));
        }

        [Test]
        public void ATurnBoundaryStaysFarSmallerThanTheTerrainItDescribes()
        {
            // The shipped arena's collision mask is ~2240x1260 cells; sending it would
            // be hundreds of kilobytes a turn. The craters that produced it are not.
            var snapshot = new BattleTurnSnapshot(
                3,
                "unit.a",
                new List<BattleUnitSnapshot>
                {
                    new("unit.a", Vector2.zero, 100, 1),
                    new("unit.b", Vector2.one, 100, -1)
                },
                new List<BattleCraterSnapshot>
                {
                    new(Vector2.zero, 1f),
                    new(Vector2.one, 1.2f),
                    new(new Vector2(3f, 2f), 0.8f)
                });

            Assert.That(BattleTurnSnapshotCodec.Encode(snapshot).Length, Is.LessThan(256));
        }

        [Test]
        public void TheSameTurnHashesTheSameOnBothSides()
        {
            var left = Snapshot(new Vector2(1f, 2f), 80);
            var right = Snapshot(new Vector2(1f, 2f), 80);

            Assert.That(BattleStateHash.Compute(right), Is.EqualTo(BattleStateHash.Compute(left)));
        }

        [Test]
        public void SubCentimetreDriftDoesNotCountAsDivergence()
        {
            var left = Snapshot(new Vector2(1f, 2f), 80);
            var right = Snapshot(new Vector2(1.0009f, 2.0009f), 80);

            Assert.That(BattleStateHash.Compute(right), Is.EqualTo(BattleStateHash.Compute(left)));
        }

        [Test]
        public void ARealDisagreementDoesCountAsDivergence()
        {
            var left = Snapshot(new Vector2(1f, 2f), 80);

            Assert.That(BattleStateHash.Compute(Snapshot(new Vector2(1.4f, 2f), 80)),
                Is.Not.EqualTo(BattleStateHash.Compute(left)));
            Assert.That(BattleStateHash.Compute(Snapshot(new Vector2(1f, 2f), 79)),
                Is.Not.EqualTo(BattleStateHash.Compute(left)));
        }

        private static BattleTurnSnapshot Snapshot(Vector2 position, int health)
        {
            return new BattleTurnSnapshot(
                5,
                "unit.a",
                new List<BattleUnitSnapshot> { new("unit.a", position, health, 1) },
                new List<BattleCraterSnapshot>());
        }
    }
}
