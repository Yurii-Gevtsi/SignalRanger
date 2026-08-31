using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Gameplay.Battle.Net
{
    /// <summary>What a battle can be told to do by a command, whoever produced it.</summary>
    public interface IBattleCommandTarget
    {
        void SelectWeapon(int weaponIndex);
        void SetWalkIntent(int direction);
        void Jump(int direction);
        void Fire(Vector2 origin, Vector2 direction, float power);
        void SetProjectileInput(Vector2 direction);
        void EndTurn();
    }

    public enum BattleCommandRejection
    {
        None = 0,

        /// <summary>The sender does not control the unit named in the command.</summary>
        NotYourUnit = 1,

        /// <summary>The command belongs to a turn that is already over.</summary>
        StaleTurn = 2,

        /// <summary>Already applied; a resend arriving twice must not act twice.</summary>
        Duplicate = 3,

        /// <summary>Nothing to do, or no battle to do it to.</summary>
        Unusable = 4
    }

    /// <summary>
    /// Applies commands to a battle, once each and only from whoever is allowed to
    /// send them.
    /// <para>
    /// Every check here exists because the transport cannot be trusted to deliver
    /// exactly once, in order, or honestly: packets are resent, arrive late, and are
    /// sent by whoever wants to. Filtering is the authoritative side's job, so this
    /// runs wherever the authoritative battle runs.
    /// </para>
    /// </summary>
    public sealed class BattleCommandDispatcher
    {
        private readonly IBattleCommandTarget _target;
        private readonly BattleUnitAuthority _authority;
        private readonly Dictionary<string, int> _lastSequenceByUnit = new(StringComparer.Ordinal);

        public BattleCommandDispatcher(IBattleCommandTarget target, BattleUnitAuthority authority)
        {
            _target = target ?? throw new ArgumentNullException(nameof(target));
            _authority = authority ?? BattleUnitAuthority.Empty;
        }

        /// <summary>Turn the battle is currently on; commands for older turns are dropped.</summary>
        public int CurrentTurnNumber { get; set; }

        public bool TryApply(in BattleCommand command, string senderPlayerId, out BattleCommandRejection rejection)
        {
            if (command.Kind == BattleCommandKind.None || string.IsNullOrEmpty(command.ActorUnitId))
            {
                rejection = BattleCommandRejection.Unusable;
                return false;
            }

            if (!_authority.CanControl(senderPlayerId, command.ActorUnitId))
            {
                rejection = BattleCommandRejection.NotYourUnit;
                return false;
            }

            if (command.TurnNumber < CurrentTurnNumber)
            {
                rejection = BattleCommandRejection.StaleTurn;
                return false;
            }

            if (_lastSequenceByUnit.TryGetValue(command.ActorUnitId, out var lastSequence)
                && command.Sequence <= lastSequence)
            {
                rejection = BattleCommandRejection.Duplicate;
                return false;
            }

            _lastSequenceByUnit[command.ActorUnitId] = command.Sequence;
            Apply(command);
            rejection = BattleCommandRejection.None;
            return true;
        }

        /// <summary>Forgets per-unit ordering, for a rematch on the same connection.</summary>
        public void Reset()
        {
            _lastSequenceByUnit.Clear();
            CurrentTurnNumber = 0;
        }

        private void Apply(in BattleCommand command)
        {
            switch (command.Kind)
            {
                case BattleCommandKind.SelectWeapon:
                    _target.SelectWeapon(command.Value);
                    break;
                case BattleCommandKind.WalkIntent:
                    _target.SetWalkIntent(command.Value);
                    break;
                case BattleCommandKind.Jump:
                    _target.Jump(command.Value);
                    break;
                case BattleCommandKind.Fire:
                    _target.Fire(command.Origin, command.Direction, command.Power);
                    break;
                case BattleCommandKind.ProjectileInput:
                    _target.SetProjectileInput(command.Direction);
                    break;
                case BattleCommandKind.EndTurn:
                    _target.EndTurn();
                    break;
            }
        }
    }
}
