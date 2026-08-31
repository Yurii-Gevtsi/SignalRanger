using System;

namespace Game.Core.Modes
{
    /// <summary>Which side of a duel connection this client is.</summary>
    public enum PvpSessionRole
    {
        /// <summary>Runs the authoritative battle and publishes turn boundaries.</summary>
        Host = 0,

        /// <summary>Plays locally for responsiveness and reconciles against the host.</summary>
        Guest = 1
    }

    public enum PvpMessageKind
    {
        None = 0,

        /// <summary>A battle input addressed to one unit.</summary>
        Command = 1,

        /// <summary>The host's authoritative state at the end of a turn.</summary>
        TurnSnapshot = 2,

        /// <summary>A client's own fingerprint of that turn, for divergence detection.</summary>
        TurnHash = 3,

        /// <summary>A player gave up or left the match.</summary>
        Forfeit = 4,

        /// <summary>Liveness probe; carries no battle meaning.</summary>
        Heartbeat = 5
    }

    public readonly struct PvpMessage
    {
        public PvpMessage(string senderPlayerId, PvpMessageKind kind, byte[] payload)
        {
            SenderPlayerId = senderPlayerId ?? string.Empty;
            Kind = kind;
            Payload = payload ?? Array.Empty<byte>();
        }

        public string SenderPlayerId { get; }
        public PvpMessageKind Kind { get; }
        public byte[] Payload { get; }
    }

    /// <summary>
    /// The duel's transport. Deliberately a byte pipe: it knows about players,
    /// message kinds and connection state, and nothing at all about battles. The
    /// battle layer owns the encoding, so the wire format can change without the
    /// transport changing and vice versa.
    /// <para>
    /// Both the in-process loopback used for tests and the future Relay adapter
    /// implement exactly this, which is what lets the whole duel flow be built and
    /// tested before any networking package is installed.
    /// </para>
    /// </summary>
    public interface IPvpSessionService
    {
        bool IsConnected { get; }
        PvpSessionRole Role { get; }
        string LocalPlayerId { get; }

        /// <summary>Raised on the main thread for every message from the other side.</summary>
        event Action<PvpMessage> MessageReceived;

        /// <summary>Raised when a player drops; the match has to decide what that means.</summary>
        event Action<string> PlayerDisconnected;

        /// <summary>
        /// Joins the match's channel. <paramref name="onConnected"/> is invoked once
        /// with whether the connection came up.
        /// </summary>
        void Connect(string matchId, string localPlayerId, PvpSessionRole role, Action<bool> onConnected);

        void Send(PvpMessageKind kind, byte[] payload);

        /// <summary>Pumps the transport; the battle drives this once per frame.</summary>
        void Tick(float deltaSeconds);

        void Disconnect();
    }
}
