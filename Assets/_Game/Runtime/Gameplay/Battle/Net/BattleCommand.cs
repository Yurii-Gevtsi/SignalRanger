using UnityEngine;

namespace Game.Gameplay.Battle.Net
{
    /// <summary>What a combatant asked the battle to do on their turn.</summary>
    public enum BattleCommandKind
    {
        None = 0,

        /// <summary>Pick a weapon by its arsenal index.</summary>
        SelectWeapon = 1,

        /// <summary>Hold a walk direction (-1, 0 or 1); the fixed tick consumes it.</summary>
        WalkIntent = 2,

        /// <summary>Start a jump toward a side.</summary>
        Jump = 3,

        /// <summary>Fire with an aim origin, direction and power.</summary>
        Fire = 4,

        /// <summary>Steer a controlled projectile such as the drone.</summary>
        ProjectileInput = 5,

        /// <summary>Give up the rest of the turn.</summary>
        EndTurn = 6
    }

    /// <summary>
    /// One addressable battle input. Deliberately addressed to a <em>unit</em> rather
    /// than to "the player": 1v1 needs only two ids, but team battles need to know
    /// which of a side's units the command is for, and retrofitting that later would
    /// mean changing every message already in flight.
    /// <para>
    /// Walk carries no duration on purpose - the simulation treats walking as a held
    /// intent consumed by its fixed tick, so a command means "hold this direction",
    /// not "move this far". That is what makes the stream frame-rate independent and
    /// safe to replay.
    /// </para>
    /// </summary>
    public readonly struct BattleCommand
    {
        public BattleCommand(
            int turnNumber,
            int sequence,
            string actorUnitId,
            BattleCommandKind kind,
            int value = 0,
            Vector2 origin = default,
            Vector2 direction = default,
            float power = 0f)
        {
            TurnNumber = turnNumber;
            Sequence = sequence;
            ActorUnitId = actorUnitId ?? string.Empty;
            Kind = kind;
            Value = value;
            Origin = origin;
            Direction = direction;
            Power = power;
        }

        /// <summary>Turn this command belongs to; a command for a finished turn is dropped.</summary>
        public int TurnNumber { get; }

        /// <summary>Per-match ordering, so a reordered transport can be put back in order.</summary>
        public int Sequence { get; }
        public string ActorUnitId { get; }
        public BattleCommandKind Kind { get; }

        /// <summary>Weapon index, walk direction or jump direction, by kind.</summary>
        public int Value { get; }
        public Vector2 Origin { get; }
        public Vector2 Direction { get; }
        public float Power { get; }

        public static BattleCommand SelectWeapon(int turnNumber, int sequence, string unitId, int weaponIndex) =>
            new(turnNumber, sequence, unitId, BattleCommandKind.SelectWeapon, weaponIndex);

        public static BattleCommand Walk(int turnNumber, int sequence, string unitId, int direction) =>
            new(turnNumber, sequence, unitId, BattleCommandKind.WalkIntent, direction);

        public static BattleCommand Jump(int turnNumber, int sequence, string unitId, int direction) =>
            new(turnNumber, sequence, unitId, BattleCommandKind.Jump, direction);

        public static BattleCommand Fire(
            int turnNumber, int sequence, string unitId, Vector2 origin, Vector2 direction, float power) =>
            new(turnNumber, sequence, unitId, BattleCommandKind.Fire, 0, origin, direction, power);

        public static BattleCommand ProjectileInput(int turnNumber, int sequence, string unitId, Vector2 direction) =>
            new(turnNumber, sequence, unitId, BattleCommandKind.ProjectileInput, 0, default, direction);

        public static BattleCommand EndTurn(int turnNumber, int sequence, string unitId) =>
            new(turnNumber, sequence, unitId, BattleCommandKind.EndTurn);
    }
}
