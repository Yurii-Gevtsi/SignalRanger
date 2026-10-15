using System;
using Game.Core.Modes;
using UnityEngine;

namespace Game.Gameplay.Battle.Net
{
    /// <summary>
    /// Runs one duel over the wire: sends what the local commander does, applies what
    /// the other one sends, and - on the host - publishes the authoritative state at
    /// every turn boundary for the other side to reconcile against.
    /// <para>
    /// This is the model chosen for the mode: both sides simulate locally so a shot
    /// appears the instant it is fired, and the host's picture settles any
    /// disagreement once the turn is over and everything has come to rest.
    /// </para>
    /// </summary>
    public sealed class BattleDuelCoordinator
    {
        private readonly IPvpSessionService _session;
        private readonly BattleCommandDispatcher _dispatcher;
        private readonly BattleUnitAuthority _authority;
        private readonly Func<BattleTurnSnapshot> _captureSnapshot;
        private readonly Action<BattleTurnSnapshot> _reconcile;
        private int _sequence;

        public BattleDuelCoordinator(
            IPvpSessionService session,
            IBattleCommandTarget target,
            BattleUnitAuthority authority,
            Func<BattleTurnSnapshot> captureSnapshot,
            Action<BattleTurnSnapshot> reconcile)
        {
            _session = session ?? throw new ArgumentNullException(nameof(session));
            _authority = authority ?? throw new ArgumentNullException(nameof(authority));
            _captureSnapshot = captureSnapshot ?? throw new ArgumentNullException(nameof(captureSnapshot));
            _reconcile = reconcile ?? throw new ArgumentNullException(nameof(reconcile));
            _dispatcher = new BattleCommandDispatcher(
                target ?? throw new ArgumentNullException(nameof(target)),
                authority);
            _session.MessageReceived += OnMessageReceived;
            _session.PlayerDisconnected += OnPlayerDisconnected;
        }

        /// <summary>Raised when the other commander drops out of the battle.</summary>
        public event Action<string> OpponentLeft;

        /// <summary>
        /// Divergence noticed at the last reconciled boundary, for telemetry. A duel
        /// that reports this often means the two sides are drifting more than the
        /// per-turn correction can quietly absorb.
        /// </summary>
        public bool LastTurnDiverged { get; private set; }

        public bool IsHost => _session.Role == PvpSessionRole.Host;

        /// <summary>
        /// False while a command from the other side is being applied. Local input
        /// hooks read this before sending, so replaying a remote command never
        /// echoes it straight back.
        /// </summary>
        public bool IsApplyingRemote { get; private set; }

        public void Tick(float deltaSeconds, int currentTurnNumber)
        {
            _dispatcher.CurrentTurnNumber = currentTurnNumber;
            _session.Tick(deltaSeconds);
        }

        /// <summary>
        /// Sends what the local commander just did. Called after the action has
        /// already been applied locally - the local player never waits on the network
        /// to see their own move.
        /// </summary>
        public void SendLocalCommand(BattleCommandKind kind, string unitId, int value = 0,
            Vector2 origin = default, Vector2 direction = default, float power = 0f)
        {
            if (IsApplyingRemote || !_session.IsConnected || !_authority.IsLocallyControlled(unitId))
            {
                return;
            }

            // The wire never carries a local unit id - see BattleUnitReference.
            var reference = BattleUnitReference.ResolveReference(_authority, unitId);
            if (string.IsNullOrEmpty(reference))
            {
                return;
            }

            var command = new BattleCommand(
                _dispatcher.CurrentTurnNumber, ++_sequence, reference, kind, value, origin, direction, power);
            _session.Send(PvpMessageKind.Command, BattleCommandCodec.Encode(command));
        }

        /// <summary>
        /// Publishes the authoritative turn boundary. Only the host does this; on the
        /// guest it is a no-op, so the caller can invoke it unconditionally.
        /// </summary>
        public void PublishTurnBoundary()
        {
            if (!IsHost || !_session.IsConnected)
            {
                return;
            }

            _session.Send(PvpMessageKind.TurnSnapshot, BattleTurnSnapshotCodec.Encode(_captureSnapshot()));
        }

        public void Dispose()
        {
            _session.MessageReceived -= OnMessageReceived;
            _session.PlayerDisconnected -= OnPlayerDisconnected;
        }

        private void OnMessageReceived(PvpMessage message)
        {
            switch (message.Kind)
            {
                case PvpMessageKind.Command:
                    ApplyRemoteCommand(message);
                    break;
                case PvpMessageKind.TurnSnapshot:
                    ApplyTurnSnapshot(message);
                    break;
                case PvpMessageKind.Forfeit:
                    OpponentLeft?.Invoke(message.SenderPlayerId);
                    break;
            }
        }

        private void ApplyRemoteCommand(PvpMessage message)
        {
            if (!BattleCommandCodec.TryDecode(message.Payload, out var command))
            {
                return;
            }

            // Translate the sender's reference into whichever unit it is on this
            // machine before the authority check runs against it.
            var localUnitId = BattleUnitReference.ResolveLocalUnitId(_authority, command.ActorUnitId);
            if (string.IsNullOrEmpty(localUnitId))
            {
                return;
            }

            var localCommand = new BattleCommand(
                command.TurnNumber, command.Sequence, localUnitId, command.Kind,
                command.Value, command.Origin, command.Direction, command.Power);

            IsApplyingRemote = true;
            try
            {
                _dispatcher.TryApply(localCommand, message.SenderPlayerId, out _);
            }
            finally
            {
                IsApplyingRemote = false;
            }
        }

        private void ApplyTurnSnapshot(PvpMessage message)
        {
            // The host owns the state; a snapshot arriving on the host would be an
            // impostor, so it is ignored rather than trusted.
            if (IsHost || !BattleTurnSnapshotCodec.TryDecode(message.Payload, out var snapshot))
            {
                return;
            }

            IsApplyingRemote = true;
            try
            {
                _reconcile(snapshot);
            }
            finally
            {
                IsApplyingRemote = false;
            }
        }

        private void OnPlayerDisconnected(string playerId)
        {
            OpponentLeft?.Invoke(playerId);
        }

        /// <summary>Records the divergence verdict from the last reconcile.</summary>
        public void ReportReconcileResult(BattleReconcileResult result)
        {
            LastTurnDiverged = result.Diverged;
        }
    }
}
