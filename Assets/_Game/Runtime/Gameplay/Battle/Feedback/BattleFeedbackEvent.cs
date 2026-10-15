using UnityEngine;

namespace Game.Gameplay.Battle.Feedback
{
    public readonly struct BattleFeedbackEvent
    {
        public BattleFeedbackEvent(
            BattleFeedbackEventType eventType,
            string sourceId,
            string targetId,
            Vector2 position,
            int amount)
        {
            EventType = eventType;
            SourceId = sourceId ?? string.Empty;
            TargetId = targetId ?? string.Empty;
            Position = position;
            Amount = amount;
        }

        public BattleFeedbackEventType EventType { get; }
        public string SourceId { get; }
        public string TargetId { get; }
        public Vector2 Position { get; }
        public int Amount { get; }
    }
}
