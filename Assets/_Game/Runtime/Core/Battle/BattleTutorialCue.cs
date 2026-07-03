namespace Game.Core.Battle
{
    public readonly struct BattleTutorialCue
    {
        public BattleTutorialCue(string message, string uiTargetId)
        {
            Message = message ?? string.Empty;
            UiTargetId = uiTargetId ?? string.Empty;
        }

        public string Message { get; }
        public string UiTargetId { get; }
        public bool IsEmpty => string.IsNullOrWhiteSpace(Message);

        public static BattleTutorialCue Empty => new(string.Empty, string.Empty);
    }
}
