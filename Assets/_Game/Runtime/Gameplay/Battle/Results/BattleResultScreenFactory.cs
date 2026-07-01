using Game.Gameplay.Battle.Domain;

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
                    "Перемога",
                    $"Бій завершено за {turnNumber} ходів.",
                    new BattleRewardPreview(25, "Відкрито попередній доступ до наступної арени"),
                    "Повторити",
                    "Далі"),
                BattleOutcome.OpponentWon => new BattleResultScreenState(
                    result,
                    "Поразка",
                    $"Суперник переміг після {turnNumber} ходів.",
                    new BattleRewardPreview(0, string.Empty),
                    "Повторити",
                    "Далі"),
                BattleOutcome.Draw => new BattleResultScreenState(
                    result,
                    "Нічия",
                    $"Обидві команди вибули після {turnNumber} ходів.",
                    new BattleRewardPreview(10, string.Empty),
                    "Повторити",
                    "Далі"),
                _ => new BattleResultScreenState(
                    result,
                    "Бій триває",
                    string.Empty,
                    new BattleRewardPreview(0, string.Empty),
                    "Повторити",
                    "Далі")
            };
        }
    }
}
