using System;
using System.Collections.Generic;
using Game.Core.Modes;

namespace Game.Infrastructure
{
    /// <summary>
    /// An in-process duel channel: two endpoints joined by a shared switchboard,
    /// with an optional delay so latency can be exercised without a network.
    /// <para>
    /// This is not a stand-in for the Relay adapter - it is how the duel flow is
    /// tested. Two endpoints in one process run the same host/guest code paths the
    /// real transport will, so command ordering, reconciliation and forfeits are all
    /// exercised before any package is installed.
    /// </para>
    /// </summary>
    public sealed class LoopbackPvpSessionService : IPvpSessionService
    {
        /// <summary>
        /// Routes messages between the endpoints of one match. Endpoints register
        /// themselves on connect and drop out on disconnect.
        /// </summary>
        public sealed class Switchboard
        {
            private readonly List<LoopbackPvpSessionService> _endpoints = new();

            /// <summary>Seconds every message waits before delivery; 0 delivers on the next tick.</summary>
            public float DeliveryDelaySeconds { get; set; }

            internal void Register(LoopbackPvpSessionService endpoint)
            {
                if (!_endpoints.Contains(endpoint))
                {
                    _endpoints.Add(endpoint);
                }
            }

            internal void Unregister(LoopbackPvpSessionService endpoint)
            {
                _endpoints.Remove(endpoint);
                for (var i = 0; i < _endpoints.Count; i++)
                {
                    _endpoints[i].RaisePlayerDisconnected(endpoint.LocalPlayerId);
                }
            }

            internal void Publish(LoopbackPvpSessionService sender, PvpMessage message)
            {
                for (var i = 0; i < _endpoints.Count; i++)
                {
                    var endpoint = _endpoints[i];
                    if (!ReferenceEquals(endpoint, sender))
                    {
                        endpoint.Enqueue(message, DeliveryDelaySeconds);
                    }
                }
            }
        }

        private readonly Switchboard _switchboard;
        private readonly List<PendingMessage> _inbox = new();

        public LoopbackPvpSessionService(Switchboard switchboard)
        {
            _switchboard = switchboard ?? throw new ArgumentNullException(nameof(switchboard));
        }

        public bool IsConnected { get; private set; }
        public PvpSessionRole Role { get; private set; }
        public string LocalPlayerId { get; private set; } = string.Empty;
        public string MatchId { get; private set; } = string.Empty;

        public event Action<PvpMessage> MessageReceived;
        public event Action<string> PlayerDisconnected;

        public void Connect(string matchId, string localPlayerId, PvpSessionRole role, Action<bool> onConnected)
        {
            MatchId = matchId ?? string.Empty;
            LocalPlayerId = localPlayerId ?? string.Empty;
            Role = role;
            IsConnected = true;
            _switchboard.Register(this);
            onConnected?.Invoke(true);
        }

        public void Send(PvpMessageKind kind, byte[] payload)
        {
            if (!IsConnected)
            {
                return;
            }

            _switchboard.Publish(this, new PvpMessage(LocalPlayerId, kind, payload));
        }

        public void Tick(float deltaSeconds)
        {
            if (!IsConnected || _inbox.Count == 0)
            {
                return;
            }

            // Collected first, then raised: a handler that sends in response must not
            // mutate the queue being walked.
            List<PvpMessage> ready = null;
            for (var i = _inbox.Count - 1; i >= 0; i--)
            {
                var pending = _inbox[i];
                pending.RemainingDelay -= Math.Max(0f, deltaSeconds);
                if (pending.RemainingDelay > 0f)
                {
                    _inbox[i] = pending;
                    continue;
                }

                _inbox.RemoveAt(i);
                ready ??= new List<PvpMessage>();
                ready.Add(pending.Message);
            }

            if (ready == null)
            {
                return;
            }

            // The queue is walked backwards, so restore send order before delivery.
            for (var i = ready.Count - 1; i >= 0; i--)
            {
                MessageReceived?.Invoke(ready[i]);
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
            _switchboard.Unregister(this);
        }

        private void Enqueue(PvpMessage message, float delaySeconds)
        {
            _inbox.Add(new PendingMessage { Message = message, RemainingDelay = Math.Max(0f, delaySeconds) });
        }

        private void RaisePlayerDisconnected(string playerId)
        {
            PlayerDisconnected?.Invoke(playerId);
        }

        private struct PendingMessage
        {
            public PvpMessage Message;
            public float RemainingDelay;
        }
    }
}
