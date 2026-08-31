using System;
using System.Collections.Generic;
using Game.Core.Modes;
using Unity.Collections;
using Unity.Netcode;
using Unity.Services.Multiplayer;
using UnityEngine;

namespace Game.Infrastructure.Multiplayer
{
    /// <summary>
    /// The duel's byte pipe over an already joined Multiplayer Services session.
    /// <para>
    /// The session brings the players together and stands up the Relay-backed
    /// Netcode connection; this class only carries our messages across it, using
    /// Netcode's named messages with reliable ordered delivery. It never learns what
    /// a battle is - the payloads are opaque bytes, exactly as
    /// <see cref="IPvpSessionService"/> promises.
    /// </para>
    /// <para>
    /// Who hosts is read from the session rather than chosen by the caller: the
    /// service decides, and a client that disagreed with it would be authoritative
    /// about nothing.
    /// </para>
    /// </summary>
    public sealed class MultiplayerPvpSessionService : IPvpSessionService
    {
        /// <summary>Named-message channel every duel message travels on.</summary>
        public const string MessageChannel = "game.duel";

        private readonly Func<ISession> _sessionProvider;
        private readonly Queue<PvpMessage> _inbox = new();
        private ISession _session;
        private bool _handlerRegistered;

        /// <summary>
        /// <paramref name="sessionProvider"/> hands over the session the matchmaker
        /// already joined, so pairing and transport stay separable.
        /// </summary>
        public MultiplayerPvpSessionService(Func<ISession> sessionProvider)
        {
            _sessionProvider = sessionProvider ?? throw new ArgumentNullException(nameof(sessionProvider));
        }

        public bool IsConnected { get; private set; }
        public PvpSessionRole Role { get; private set; }
        public string LocalPlayerId { get; private set; } = string.Empty;

        public event Action<PvpMessage> MessageReceived;
        public event Action<string> PlayerDisconnected;

        public void Connect(string matchId, string localPlayerId, PvpSessionRole role, Action<bool> onConnected)
        {
            LocalPlayerId = localPlayerId ?? string.Empty;
            _session = _sessionProvider();
            if (_session == null)
            {
                onConnected?.Invoke(false);
                return;
            }

            // The session already knows who hosts; the caller's role is only a hint.
            Role = _session.IsHost ? PvpSessionRole.Host : PvpSessionRole.Guest;
            _session.PlayerLeaving += OnPlayerLeaving;
            _session.RemovedFromSession += OnRemovedFromSession;

            if (!TryRegisterMessageHandler())
            {
                onConnected?.Invoke(false);
                return;
            }

            IsConnected = true;
            onConnected?.Invoke(true);
        }

        public void Send(PvpMessageKind kind, byte[] payload)
        {
            var messaging = NetworkManager.Singleton?.CustomMessagingManager;
            if (!IsConnected || messaging == null)
            {
                return;
            }

            payload ??= Array.Empty<byte>();
            // One byte of kind, then the payload verbatim.
            var writer = new FastBufferWriter(payload.Length + sizeof(byte), Allocator.Temp);
            try
            {
                writer.WriteByteSafe((byte)kind);
                if (payload.Length > 0)
                {
                    writer.WriteBytesSafe(payload, payload.Length);
                }

                messaging.SendNamedMessageToAll(MessageChannel, writer, NetworkDelivery.ReliableSequenced);
            }
            catch (Exception exception)
            {
                Debug.LogWarning($"Duel message could not be sent: {exception.Message}");
            }
            finally
            {
                writer.Dispose();
            }
        }

        /// <summary>
        /// Named messages arrive on Netcode's own callback, which can fire at any
        /// point in the frame. They are queued there and raised here so the battle
        /// always sees them at one predictable moment.
        /// </summary>
        public void Tick(float deltaSeconds)
        {
            if (!IsConnected)
            {
                return;
            }

            while (_inbox.Count > 0)
            {
                MessageReceived?.Invoke(_inbox.Dequeue());
            }
        }

        public void Disconnect()
        {
            if (!IsConnected)
            {
                return;
            }

            IsConnected = false;
            _inbox.Clear();
            UnregisterMessageHandler();

            if (_session != null)
            {
                _session.PlayerLeaving -= OnPlayerLeaving;
                _session.RemovedFromSession -= OnRemovedFromSession;
                var leaving = _session;
                _session = null;
                // Leaving is fire-and-forget: the battle is already over for us, and
                // a failed leave is cleaned up by the service's own timeout.
                _ = leaving.LeaveAsync();
            }
        }

        private bool TryRegisterMessageHandler()
        {
            var messaging = NetworkManager.Singleton?.CustomMessagingManager;
            if (messaging == null)
            {
                Debug.LogWarning("Duel transport is unavailable: the session has no Netcode connection.");
                return false;
            }

            messaging.RegisterNamedMessageHandler(MessageChannel, OnNamedMessage);
            _handlerRegistered = true;
            return true;
        }

        private void UnregisterMessageHandler()
        {
            if (!_handlerRegistered)
            {
                return;
            }

            NetworkManager.Singleton?.CustomMessagingManager?.UnregisterNamedMessageHandler(MessageChannel);
            _handlerRegistered = false;
        }

        private void OnNamedMessage(ulong senderClientId, FastBufferReader reader)
        {
            try
            {
                reader.ReadByteSafe(out var kind);
                var remaining = reader.Length - reader.Position;
                var payload = Array.Empty<byte>();
                if (remaining > 0)
                {
                    payload = new byte[remaining];
                    reader.ReadBytesSafe(ref payload, remaining);
                }

                _inbox.Enqueue(new PvpMessage(
                    ResolveSenderPlayerId(senderClientId),
                    (PvpMessageKind)kind,
                    payload));
            }
            catch (Exception exception)
            {
                // A malformed message is a transport problem; dropping it beats
                // taking the battle down.
                Debug.LogWarning($"Duel message could not be read: {exception.Message}");
            }
        }

        /// <summary>
        /// Netcode identifies senders by client id; the battle works in player ids.
        /// With two commanders the other party is simply "not us", which is enough
        /// until team battles make the mapping worth carrying explicitly.
        /// </summary>
        private string ResolveSenderPlayerId(ulong senderClientId)
        {
            if (_session == null)
            {
                return senderClientId.ToString();
            }

            var players = _session.Players;
            for (var i = 0; i < players.Count; i++)
            {
                var playerId = players[i]?.Id;
                if (!string.IsNullOrEmpty(playerId)
                    && !string.Equals(playerId, LocalPlayerId, StringComparison.Ordinal))
                {
                    return playerId;
                }
            }

            return senderClientId.ToString();
        }

        private void OnPlayerLeaving(string playerId)
        {
            PlayerDisconnected?.Invoke(playerId);
        }

        private void OnRemovedFromSession()
        {
            PlayerDisconnected?.Invoke(LocalPlayerId);
            IsConnected = false;
        }
    }
}
