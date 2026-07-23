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
        public float TerrainDrillTimeRemaining { get; private set; }
        public float TerrainDrillSpeed { get; private set; }
        public Vector2 TerrainDrillDirection { get; private set; } = Vector2.right;
        public bool IsTraversingTerrain { get; private set; }
        public bool IsRestingFuse { get; private set; }
        public float RestingFuseRemaining { get; private set; }

        private Vector2 _lastTunnelCarvePoint;
        private bool _hasCarvedTunnel;

        /// <summary>
        /// The projectile has left its owner's hitbox at least once; from then
        /// on the owner is a valid target (reference behaviour - you can shoot
        /// yourself with a shot that comes back or clips you).
        /// </summary>
        public bool HasClearedOwner { get; private set; }

        internal void MarkClearedOwner()
        {
            HasClearedOwner = true;
        }

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

        internal void StartRestingFuse(Vector2 position, float durationSeconds)
        {
            Position = position;
            Velocity = Vector2.zero;
            IsRestingFuse = true;
            RestingFuseRemaining = Mathf.Max(0f, durationSeconds);
        }

        internal void ReduceRestingFuse(float deltaTime)
        {
            RestingFuseRemaining = Mathf.Max(0f, RestingFuseRemaining - deltaTime);
        }

        internal void ConfigureTerrainDrill(float durationSeconds)
        {
            TerrainDrillTimeRemaining = Mathf.Max(0f, durationSeconds);
        }

        internal void EnterTerrain(float speedRetain)
        {
            if (IsTraversingTerrain)
            {
                return;
            }

            IsTraversingTerrain = true;
            Velocity *= Mathf.Clamp01(speedRetain);
            TerrainDrillSpeed = Velocity.magnitude;
            TerrainDrillDirection = Velocity.sqrMagnitude > 0.0001f ? Velocity.normalized : Vector2.right;
        }

        internal void ExitTerrain()
        {
            IsTraversingTerrain = false;
        }

        internal void ConsumeTerrainDrillTime(float deltaTime)
        {
            TerrainDrillTimeRemaining = Mathf.Max(0f, TerrainDrillTimeRemaining - deltaTime);
        }

        /// <summary>
        /// Rate-limits tunnel carving by travelled distance: returns true (and
        /// records the point) only when the projectile moved at least
        /// <paramref name="spacing"/> since the previous carve.
        /// </summary>
        internal bool ShouldCarveTunnelAt(Vector2 position, float spacing)
        {
            if (_hasCarvedTunnel && (position - _lastTunnelCarvePoint).sqrMagnitude < spacing * spacing)
            {
                return false;
            }

            _hasCarvedTunnel = true;
            _lastTunnelCarvePoint = position;
            return true;
        }
    }
}
