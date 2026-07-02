using Game.Gameplay.Battle.Domain;
using Game.Core.Localization;

namespace Game.Gameplay.Battle.Results
{
    public static class BattleResultScreenFactory
    {
        public static BattleResultScreenState Create(BattleResult result, int turnNumber)
        {
            return result.Outcome switch
            {
                BattleOutcome.PlayerWon => new BattleResultScreenState(
                    result,
                    GameText.VictoryTitle(),
                    GameText.VictorySubtitle(turnNumber),
                    new BattleRewardPreview(25, GameText.NextArenaPreview()),
                    GameText.Restart,
                    GameText.Continue),
                BattleOutcome.OpponentWon => new BattleResultScreenState(
                    result,
                    GameText.DefeatTitle(),
                    GameText.DefeatSubtitle(turnNumber),
                    new BattleRewardPreview(0, string.Empty),
                    GameText.Restart,
                    GameText.Continue),
                BattleOutcome.Draw => new BattleResultScreenState(
                    result,
                    GameText.DrawTitle(),
                    GameText.DrawSubtitle(turnNumber),
                    new BattleRewardPreview(10, string.Empty),
                    GameText.Restart,
                    GameText.Continue),
                _ => new BattleResultScreenState(
                    result,
                    GameText.OngoingBattleTitle(),
                    string.Empty,
                    new BattleRewardPreview(0, string.Empty),
                    GameText.Restart,
                    GameText.Continue)
            };
        }
    }
}
