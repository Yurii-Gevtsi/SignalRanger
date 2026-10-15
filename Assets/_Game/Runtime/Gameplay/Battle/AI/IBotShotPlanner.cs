using System.Collections.Generic;
using Game.Gameplay.Battle.AI.Ballistics;
using Game.Gameplay.Battle.Domain;
using Game.Gameplay.Battle.Weapons;
using UnityEngine;

namespace Game.Gameplay.Battle.AI
{
    /// <summary>
    /// Inputs for planning a single shot from one candidate firing position. The
    /// aim target is already scattered by the turn controller, so a planner just
    /// finds the best way to reach it.
    /// </summary>
    public readonly struct BotShotContext
    {
        public BotShotContext(
            Vector2 origin,
            Vector2 aimTarget,
            WeaponDefinition weapon,
            AimSettings aimSettings,
            IReadOnlyList<TrajectoryUnitBody> bodies)
        {
            Origin = origin;
            AimTarget = aimTarget;
            Weapon = weapon;
            AimSettings = aimSettings;
            Bodies = bodies;
        }

        /// <summary>Fire point (the muzzle offset is added by the weapon effect).</summary>
        public Vector2 Origin { get; }

        /// <summary>Already-scattered world target for this turn.</summary>
        public Vector2 AimTarget { get; }

        public WeaponDefinition Weapon { get; }
        public AimSettings AimSettings { get; }

        /// <summary>All collidable bodies except the firing unit; target flagged.</summary>
        public IReadOnlyList<TrajectoryUnitBody> Bodies { get; }
    }

    /// <summary>A chosen shot, ready to feed straight into <c>BattleSimulation.TryFire</c>.</summary>
    public readonly struct BotShotPlan
    {
        public BotShotPlan(AimState aim, string weaponId, float score)
        {
            Aim = aim;
            WeaponId = weaponId;
            Score = score;
        }

        public AimState Aim { get; }
        public string WeaponId { get; }
        public float Score { get; }
    }

    public interface IBotShotPlanner
    {
        /// <summary>
        /// Returns the best shot from the context's origin, or null when no shot
        /// is possible at all (e.g. the target is unreachable at every power).
        /// </summary>
        BotShotPlan? PlanShot(in BotShotContext context);
    }
}
