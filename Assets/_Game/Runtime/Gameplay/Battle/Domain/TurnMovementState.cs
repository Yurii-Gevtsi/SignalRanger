using UnityEngine;

namespace Game.Gameplay.Battle.Domain
{
    /// <summary>
    /// Tracks the active turn timer. Movement and jumps are free while time remains;
    /// only firing spends the turn action.
    /// </summary>
    public sealed class TurnMovementState
    {
        public TurnMovementState(float turnDurationSeconds)
        {
            Reset(turnDurationSeconds);
        }

        public float TurnDurationSeconds { get; private set; }
        public float RemainingTurnSeconds { get; private set; }
        public bool HasTimeRemaining => RemainingTurnSeconds > 0f;

        public void Reset(float turnDurationSeconds)
        {
            TurnDurationSeconds = Mathf.Max(0f, turnDurationSeconds);
            RemainingTurnSeconds = TurnDurationSeconds;
        }

        public void Tick(float deltaTime)
        {
            if (deltaTime <= 0f || RemainingTurnSeconds <= 0f)
            {
                return;
            }

            RemainingTurnSeconds = Mathf.Max(0f, RemainingTurnSeconds - deltaTime);
        }
    }
}
