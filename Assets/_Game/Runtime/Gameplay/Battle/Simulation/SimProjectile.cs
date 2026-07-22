using Game.Gameplay.Battle.Projectiles;
using UnityEngine;

namespace Game.Gameplay.Battle.Simulation
{
    /// <summary>Runtime projectile state owned by <see cref="BattleSimulation"/>.</summary>
    public sealed class SimProjectile
    {
        public SimProjectile(
            int id,
            ProjectileDefinition definition,
            string ownerUnitId,
            Vector2 position,
            Vector2 velocity,
            float remainingLifetime)
        {
            Id = id;
            Definition = definition;
            OwnerUnitId = ownerUnitId;
            Position = position;
            Velocity = velocity;
            RemainingLifetime = remainingLifetime;
        }

        public int Id { get; }
        public ProjectileDefinition Definition { get; }
        public string OwnerUnitId { get; }
        public Vector2 Position { get; private set; }
        public Vector2 Velocity { get; private set; }
        public float RemainingLifetime { get; private set; }
        public int BounceCount { get; private set; }

        internal void MoveTo(Vector2 position)
        {
            Position = position;
        }

        internal void SetVelocity(Vector2 velocity)
        {
            Velocity = velocity;
        }

        internal void ReduceLifetime(float deltaTime)
        {
            RemainingLifetime = Mathf.Max(0f, RemainingLifetime - deltaTime);
        }

        internal void RegisterBounce()
        {
            BounceCount++;
        }
    }
}
