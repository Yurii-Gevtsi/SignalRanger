using System.Collections.Generic;
using Game.Core.Modes;
using Game.Gameplay.Battle.Net;
using Game.Infrastructure;
using NUnit.Framework;
using UnityEngine;

namespace Game.Tests.EditMode.Modes
{
    public sealed class LoopbackPvpSessionTests
    {
        [Test]
        public void AMessageReachesTheOtherSideAndNotTheSender()
        {
            var (host, guest, hostInbox, guestInbox) = CreateMatch();

            host.Send(PvpMessageKind.Command, BattleCommandCodec.Encode(
                BattleCommand.Walk(1, 1, "unit.a", 1)));
            Pump(host, guest);

            Assert.That(guestInbox.Count, Is.EqualTo(1));
            Assert.That(hostInbox, Is.Empty);
            Assert.That(guestInbox[0].SenderPlayerId, Is.EqualTo("player.host"));
            Assert.That(guestInbox[0].Kind, Is.EqualTo(PvpMessageKind.Command));
        }

        [Test]
        public void ACommandArrivesIntactAcrossTheChannel()
        {
            var (host, guest, _, guestInbox) = CreateMatch();
            var sent = BattleCommand.Fire(3, 8, "unit.a", new Vector2(1f, 2f), Vector2.right, 22.5f);

            host.Send(PvpMessageKind.Command, BattleCommandCodec.Encode(sent));
            Pump(host, guest);

            Assert.That(BattleCommandCodec.TryDecode(guestInbox[0].Payload, out var received), Is.True);
            Assert.That(received.Sequence, Is.EqualTo(sent.Sequence));
            Assert.That(received.Power, Is.EqualTo(sent.Power));
            Assert.That(received.ActorUnitId, Is.EqualTo(sent.ActorUnitId));
        }

        [Test]
        public void MessagesArriveInTheOrderTheyWereSent()
        {
            var (host, guest, _, guestInbox) = CreateMatch();

            for (var i = 1; i <= 4; i++)
            {
                host.Send(PvpMessageKind.Command, BattleCommandCodec.Encode(
                    BattleCommand.Walk(1, i, "unit.a", 1)));
            }

            Pump(host, guest);

            Assert.That(guestInbox.Count, Is.EqualTo(4));
            for (var i = 0; i < 4; i++)
            {
                Assert.That(BattleCommandCodec.TryDecode(guestInbox[i].Payload, out var command), Is.True);
                Assert.That(command.Sequence, Is.EqualTo(i + 1));
            }
        }

        [Test]
        public void LatencyHoldsAMessageBackWithoutLosingIt()
        {
            var switchboard = new LoopbackPvpSessionService.Switchboard { DeliveryDelaySeconds = 0.25f };
            var (host, guest, _, guestInbox) = CreateMatch(switchboard);

            host.Send(PvpMessageKind.Heartbeat, new byte[] { 1 });
            Pump(host, guest, 0.1f);
            Assert.That(guestInbox, Is.Empty);

            Pump(host, guest, 0.2f);
            Assert.That(guestInbox.Count, Is.EqualTo(1));
        }

        [Test]
        public void ADisconnectTellsTheOtherSideWhoLeft()
        {
            var switchboard = new LoopbackPvpSessionService.Switchboard();
            var (host, guest, _, _) = CreateMatch(switchboard);
            var departed = new List<string>();
            host.PlayerDisconnected += playerId => departed.Add(playerId);

            guest.Disconnect();

            Assert.That(departed, Is.EqualTo(new[] { "player.guest" }));
            Assert.That(guest.IsConnected, Is.False);
        }

        [Test]
        public void ADisconnectedEndpointNeitherSendsNorReceives()
        {
            var (host, guest, _, guestInbox) = CreateMatch();
            guest.Disconnect();

            host.Send(PvpMessageKind.Heartbeat, new byte[] { 1 });
            Pump(host, guest);

            Assert.That(guestInbox, Is.Empty);
        }

        [Test]
        public void TheTwoSidesAgreeOnTheirRoles()
        {
            var (host, guest, _, _) = CreateMatch();

            Assert.That(host.Role, Is.EqualTo(PvpSessionRole.Host));
            Assert.That(guest.Role, Is.EqualTo(PvpSessionRole.Guest));
            Assert.That(host.IsConnected, Is.True);
            Assert.That(guest.IsConnected, Is.True);
        }

        private static (LoopbackPvpSessionService Host,
            LoopbackPvpSessionService Guest,
            List<PvpMessage> HostInbox,
            List<PvpMessage> GuestInbox) CreateMatch(LoopbackPvpSessionService.Switchboard switchboard = null)
        {
            switchboard ??= new LoopbackPvpSessionService.Switchboard();
            var host = new LoopbackPvpSessionService(switchboard);
            var guest = new LoopbackPvpSessionService(switchboard);
            var hostInbox = new List<PvpMessage>();
            var guestInbox = new List<PvpMessage>();
            host.MessageReceived += message => hostInbox.Add(message);
            guest.MessageReceived += message => guestInbox.Add(message);
            host.Connect("match.1", "player.host", PvpSessionRole.Host, _ => { });
            guest.Connect("match.1", "player.guest", PvpSessionRole.Guest, _ => { });
            return (host, guest, hostInbox, guestInbox);
        }

        private static void Pump(
            LoopbackPvpSessionService host,
            LoopbackPvpSessionService guest,
            float deltaSeconds = 0.016f)
        {
            host.Tick(deltaSeconds);
            guest.Tick(deltaSeconds);
        }
    }
}
