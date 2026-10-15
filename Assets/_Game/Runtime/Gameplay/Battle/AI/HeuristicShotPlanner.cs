using Game.Gameplay.Battle.Domain;

namespace Game.Gameplay.Battle.AI
{
    /// <summary>
    /// Easy-difficulty planner: wraps the lightweight <see cref="SimpleAiShooter"/>
    /// heuristic behind the shared planner contract so the turn controller treats
    /// every difficulty the same way. Grenades use the bounce-aware planner because
    /// their final blast point cannot be inferred from the first terrain contact.
    /// </summary>
    public sealed class HeuristicShotPlanner : IBotShotPlanner
    {
        private const string GrenadeWeaponId = "weapon.granata";

        private readonly SimpleAiShooter _shooter;
        private readonly AiDifficulty _difficulty;
        private readonly BallisticShotPlanner _grenadePlanner;
        private int _shotIndex;

        public HeuristicShotPlanner(AiDifficulty difficulty, BallisticShotPlanner grenadePlanner = null)
        {
            _shooter = new SimpleAiShooter();
            _difficulty = difficulty;
            _grenadePlanner = grenadePlanner;
        }

        public BotShotPlan? PlanShot(in BotShotContext context)
        {
            if (context.Weapon?.WeaponId == GrenadeWeaponId && _grenadePlanner != null)
            {
                var grenadePlan = _grenadePlanner.PlanShot(context);
                if (grenadePlan.HasValue)
                {
                    return grenadePlan;
                }
            }

            var shot = _shooter.CreateShot(context.Origin, context.AimTarget, context.AimSettings, _difficulty, _shotIndex++);
            var aim = new AimState(context.Origin, shot.Direction, shot.Power, true);
            return new BotShotPlan(aim, context.Weapon != null ? context.Weapon.WeaponId : string.Empty, 0f);
        }
    }
}
