using Game.Gameplay.Battle.Domain;
using Game.Gameplay.Battle.Results;
using NUnit.Framework;

namespace Game.Tests.EditMode.Gameplay
{
    public sealed class BattleResultScreenFactoryTests
    {
        [Test]
        public void Create_PlayerWon_ReturnsVictoryStateWithReward()
        {
            var state = BattleResultScreenFactory.Create(BattleResult.Win(BattleTeamId.Player), 3);

            Assert.That(state.Title, Is.EqualTo("Перемога"));
            Assert.That(state.CanContinue, Is.True);
            Assert.That(state.RewardPreview.SoftCurrency, Is.GreaterThan(0));
        }

        [Test]
        public void Create_OpponentWon_DisablesContinue()
        {
            var state = BattleResultScreenFactory.Create(BattleResult.Win(BattleTeamId.Opponent), 4);

            Assert.That(state.Title, Is.EqualTo("Поразка"));
            Assert.That(state.CanContinue, Is.False);
            Assert.That(state.RewardPreview.SoftCurrency, Is.EqualTo(0));
        }

        [Test]
        public void Create_Draw_AllowsContinueWithSmallReward()
        {
            var state = BattleResultScreenFactory.Create(BattleResult.Draw, 5);

            Assert.That(state.Title, Is.EqualTo("Нічия"));
            Assert.That(state.CanContinue, Is.True);
            Assert.That(state.RewardPreview.SoftCurrency, Is.GreaterThan(0));
        }
    }
}
