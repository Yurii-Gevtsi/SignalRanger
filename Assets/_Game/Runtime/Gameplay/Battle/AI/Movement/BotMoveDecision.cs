using UnityEngine;

namespace Game.Gameplay.Battle.AI.Movement
{
    public enum BotMoveKind
    {
        Stay,
        Walk,
        Jump
    }

    /// <summary>
    /// How the bot should reach its chosen firing position this turn: stay put,
    /// walk to an X, or jump in a direction. The controller turns this into the
    /// matching movement command.
    /// </summary>
    public readonly struct BotMoveDecision
    {
        private BotMoveDecision(BotMoveKind kind, Vector2 position, int jumpDirection)
        {
            Kind = kind;
            Position = position;
            JumpDirection = jumpDirection;
        }

        public BotMoveKind Kind { get; }
        public Vector2 Position { get; }
        public int JumpDirection { get; }

        public static BotMoveDecision Stay(Vector2 position) => new(BotMoveKind.Stay, position, 0);
        public static BotMoveDecision Walk(Vector2 position) => new(BotMoveKind.Walk, position, 0);
        public static BotMoveDecision Jump(Vector2 landing, int direction) => new(BotMoveKind.Jump, landing, direction > 0 ? 1 : -1);
    }
}
