using Game.Gameplay.Battle.Net;
using NUnit.Framework;

namespace Game.Tests.EditMode.Modes
{
    public sealed class BattleUnitReferenceTests
    {
        [Test]
        public void AReferenceSurvivesEncodingAndDecoding()
        {
            Assert.That(BattleUnitReference.TryDecode(
                BattleUnitReference.Encode("player.abc", 2), out var playerId, out var slot), Is.True);
            Assert.That(playerId, Is.EqualTo("player.abc"));
            Assert.That(slot, Is.EqualTo(2));
        }

        [Test]
        public void APlayerIdContainingTheSeparatorStillDecodes()
        {
            // Only the last separator counts, so an id with one inside survives.
            Assert.That(BattleUnitReference.TryDecode("odd#id#3", out var playerId, out var slot), Is.True);
            Assert.That(playerId, Is.EqualTo("odd#id"));
            Assert.That(slot, Is.EqualTo(3));
        }

        [Test]
        public void MalformedReferencesAreRefused()
        {
            Assert.That(BattleUnitReference.TryDecode(null, out _, out _), Is.False);
            Assert.That(BattleUnitReference.TryDecode("no-separator", out _, out _), Is.False);
            Assert.That(BattleUnitReference.TryDecode("#0", out _, out _), Is.False);
            Assert.That(BattleUnitReference.TryDecode("player#", out _, out _), Is.False);
            Assert.That(BattleUnitReference.TryDecode("player#notanumber", out _, out _), Is.False);
        }

        [Test]
        public void TheSameReferenceResolvesToOppositeUnitsOnTheTwoClients()
        {
            // This is the whole reason references exist: both clients call their own
            // side "player.*", so a raw unit id would name the wrong commander.
            var host = new BattleUnitAuthority();
            host.SetLocalPlayer("alice");
            host.Assign("alice", "player.striker.01");
            host.Assign("bob", "opponent.striker.01");

            var guest = new BattleUnitAuthority();
            guest.SetLocalPlayer("bob");
            guest.Assign("bob", "player.striker.01");
            guest.Assign("alice", "opponent.striker.01");

            var sent = BattleUnitReference.ResolveReference(host, "player.striker.01");

            Assert.That(sent, Is.EqualTo(BattleUnitReference.Encode("alice", 0)));
            Assert.That(BattleUnitReference.ResolveLocalUnitId(guest, sent), Is.EqualTo("opponent.striker.01"));
            Assert.That(BattleUnitReference.ResolveLocalUnitId(host, sent), Is.EqualTo("player.striker.01"));
        }

        [Test]
        public void SlotsKeepTeamMatesApart()
        {
            var authority = new BattleUnitAuthority();
            authority.Assign("alice", "player.striker.01");
            authority.Assign("alice", "player.bulwark.02");

            Assert.That(BattleUnitReference.ResolveReference(authority, "player.bulwark.02"),
                Is.EqualTo(BattleUnitReference.Encode("alice", 1)));
            Assert.That(BattleUnitReference.ResolveLocalUnitId(authority, BattleUnitReference.Encode("alice", 1)),
                Is.EqualTo("player.bulwark.02"));
        }

        [Test]
        public void AReferenceToARosterThisClientDoesNotHaveResolvesToNothing()
        {
            var authority = new BattleUnitAuthority();
            authority.Assign("alice", "player.striker.01");

            Assert.That(BattleUnitReference.ResolveLocalUnitId(authority, BattleUnitReference.Encode("alice", 4)),
                Is.Empty);
            Assert.That(BattleUnitReference.ResolveLocalUnitId(authority, BattleUnitReference.Encode("carol", 0)),
                Is.Empty);
            Assert.That(BattleUnitReference.ResolveReference(authority, "unit.unknown"), Is.Empty);
        }
    }
}
