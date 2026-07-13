using Game.Gameplay.Battle.Domain;

namespace Game.Gameplay.Battle.AI
{
    /// <summary>
    /// Easy-difficulty planner: wraps the lightweight <see cref="SimpleAiShooter"/>
    /// heuristic (no trajectory probing, its own built-in aim error) behind the
    /// shared planner contract so the turn controller treats every difficulty the
    /// same way. Used for the training arena where the opponent should feel
    /// forgiving and never reposition.
    /// </summary>
    public sealed class HeuristicShotPlanner : IBotShotPlanner
    {
        private readonly SimpleAiShooter _shooter;
        private readonly AiDifficulty _difficulty;
        private int _shotIndex;

        public HeuristicShotPlanner(AiDifficulty difficulty)
        {
            _shooter = new SimpleAiShooter();
            _difficulty = difficulty;
        }

        public BotShotPlan? PlanShot(in BotShotContext context)
        {
            var shot = _shooter.CreateShot(context.Origin, context.AimTarget, context.AimSettings, _difficulty, _shotIndex++);
            var aim = new AimState(context.Origin, shot.Direction, shot.Power, true);
            return new BotShotPlan(aim, context.Weapon != null ? context.Weapon.WeaponId : string.Empty, 0f);
        }
    }
}
