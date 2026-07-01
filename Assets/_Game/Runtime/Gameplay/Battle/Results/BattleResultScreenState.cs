using Game.Gameplay.Battle.Domain;

namespace Game.Gameplay.Battle.Results
{
    public readonly struct BattleResultScreenState
    {
        public BattleResultScreenState(
            BattleResult result,
            string title,
            string subtitle,
            BattleRewardPreview rewardPreview,
            string retryLabel,
            string continueLabel)
        {
            Result = result;
            Title = title ?? string.Empty;
            Subtitle = subtitle ?? string.Empty;
            RewardPreview = rewardPreview;
            RetryLabel = retryLabel ?? "Повторити";
            ContinueLabel = continueLabel ?? "Далі";
        }

        public BattleResult Result { get; }
        public string Title { get; }
        public string Subtitle { get; }
        public BattleRewardPreview RewardPreview { get; }
        public string RetryLabel { get; }
        public string ContinueLabel { get; }
        public bool CanContinue => Result.Outcome == BattleOutcome.PlayerWon || Result.Outcome == BattleOutcome.Draw;
    }
}
