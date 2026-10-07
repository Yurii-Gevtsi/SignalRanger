using Game.Gameplay.Battle.Projectiles;
using Game.Gameplay.Battle.Terrain;
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
            float remainingLifetime,
            bool suppressTurnAdvanceOnImpact = false,
            string guaranteedTargetUnitId = null,
            Vector2? sourceOrigin = null)
        {
            Id = id;
            Definition = definition;
            OwnerUnitId = ownerUnitId;
            Position = position;
            Velocity = velocity;
            RemainingLifetime = remainingLifetime;
            SuppressesTurnAdvanceOnImpact = suppressTurnAdvanceOnImpact;
            GuaranteedTargetUnitId = guaranteedTargetUnitId;
            SourceOrigin = sourceOrigin;
        }

        public int Id { get; }
        public ProjectileDefinition Definition { get; }
        public string OwnerUnitId { get; }
        public Vector2 Position { get; private set; }
        public Vector2 Velocity { get; private set; }
        public float RemainingLifetime { get; private set; }
        public bool SuppressesTurnAdvanceOnImpact { get; }
        /// <summary>A trap-fired round remains committed to this target until impact.</summary>
        public string GuaranteedTargetUnitId { get; }
        /// <summary>Throw-time origin captured by swap-jump for the enemy return destination.</summary>
        public Vector2? SourceOrigin { get; } // Captured before the owner can move.
        public int BounceCount { get; private set; }
        public float TerrainDrillTimeRemaining { get; private set; }
        public float TerrainDrillSpeed { get; private set; }
        public Vector2 TerrainDrillDirection { get; private set; } = Vector2.right;
        public TerrainMaterial TerrainDrillPenetration { get; private set; } = TerrainMaterial.Soil;
        public int MissileSplitChargeCount { get; private set; }
        public int MissileObstacleBypassesRemaining { get; private set; }
        public bool IsBallisticMissileDescending { get; private set; }
        public bool HasMissileWaypoint { get; private set; }
        public Vector2 MissileWaypoint { get; private set; }
        public bool IsTraversingTerrain { get; private set; }
        public bool IsRestingFuse { get; private set; }
        public float RestingFuseRemaining { get; private set; }
        public bool IsAttachedToUnit => !string.IsNullOrEmpty(AttachedUnitId);
        public string AttachedUnitId { get; private set; }
        public Vector2 ExternalVelocity { get; private set; }
        /// <summary>The droid a signal rocket is carrying up, or empty.</summary>
        public string SkyHookCarriedUnitId { get; private set; } = string.Empty;
        /// <summary>Facing of the owner when the signal rocket was launched: the throw direction on symmetry.</summary>
        public int SkyHookLaunchFacing { get; private set; } = 1;

        private Vector2 _lastTunnelCarvePoint;
        private bool _hasCarvedTunnel;
        private Vector2 _attachedUnitOffset;

        /// <summary>
        /// The projectile has left its owner's hitbox at least once; from then
        /// on the owner is a valid target (reference behaviour - you can shoot
        /// yourself with a shot that comes back or clips you).
        /// </summary>
        public bool HasClearedOwner { get; private set; }

        internal void SetSkyHookCarried(string unitId)
        {
            SkyHookCarriedUnitId = unitId ?? string.Empty;
        }

        internal void SetSkyHookLaunchFacing(int facing)
        {
            SkyHookLaunchFacing = facing < 0 ? -1 : 1;
        }

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

        internal void AddExternalVelocity(Vector2 velocity)
        {
            ExternalVelocity += velocity;
        }

        internal void DampenExternalVelocity(float deltaTime, float deceleration)
        {
            ExternalVelocity = Vector2.MoveTowards(
                ExternalVelocity,
                Vector2.zero,
                Mathf.Max(0f, deceleration) * Mathf.Max(0f, deltaTime));
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

        internal void ResumeFromRestingFuse(Vector2 velocity)
        {
            IsRestingFuse = false;
            RestingFuseRemaining = 0f;
            Velocity = velocity;
        }

        internal void ReduceRestingFuse(float deltaTime)
        {
            RestingFuseRemaining = Mathf.Max(0f, RestingFuseRemaining - deltaTime);
        }

        internal void AttachToUnit(string unitId, Vector2 unitPosition)
        {
            if (string.IsNullOrEmpty(unitId))
            {
                return;
            }

            AttachedUnitId = unitId;
            _attachedUnitOffset = Position - unitPosition;
        }

        internal void FollowAttachedUnit(Vector2 unitPosition)
        {
            if (IsAttachedToUnit)
            {
                Position = unitPosition + _attachedUnitOffset;
            }
        }

        internal void DetachFromUnit()
        {
            AttachedUnitId = null;
            _attachedUnitOffset = Vector2.zero;
        }

        /// <summary>Extra jumps a hopping grenade may still launch (in flight or at rest).</summary>
        public int HopsRemaining { get; private set; }
        public bool HasHopFuse { get; private set; }
        public float HopFuseRemaining { get; private set; }

        internal void ConfigureHops(int hops)
        {
            HopsRemaining = Mathf.Max(0, hops);
        }

        internal void ConsumeHop()
        {
            HopsRemaining = Mathf.Max(0, HopsRemaining - 1);
        }

        /// <summary>The fuse starts once, at the first contact, and is never restarted by later jumps.</summary>
        internal void EnsureHopFuse(float seconds)
        {
            if (HasHopFuse)
            {
                return;
            }

            HasHopFuse = true;
            HopFuseRemaining = Mathf.Max(0f, seconds);
        }

        internal void SetHopFuse(float seconds)
        {
            HopFuseRemaining = Mathf.Max(0f, seconds);
        }

        internal void ReduceHopFuse(float deltaTime)
        {
            HopFuseRemaining = Mathf.Max(0f, HopFuseRemaining - deltaTime);
        }

        private System.Collections.Generic.HashSet<string> _piercedUnitIds;

        /// <summary>True the first time this projectile passes through the given unit.</summary>
        internal bool TryMarkPierced(string unitId)
        {
            _piercedUnitIds ??= new System.Collections.Generic.HashSet<string>(System.StringComparer.Ordinal);
            return _piercedUnitIds.Add(unitId);
        }

        /// <summary>How many different units this projectile has passed through so far.</summary>
        internal int PiercedCount => _piercedUnitIds?.Count ?? 0;

        internal void ConfigureTerrainDrill(float durationSeconds, TerrainMaterial penetration)
        {
            TerrainDrillTimeRemaining = Mathf.Max(0f, durationSeconds);
            TerrainDrillPenetration = penetration;
        }

        internal void ConfigureMissileEvolution(int splitChargeCount, int obstacleBypassCount)
        {
            MissileSplitChargeCount = Mathf.Max(0, splitChargeCount);
            MissileObstacleBypassesRemaining = Mathf.Max(0, obstacleBypassCount);
        }

        internal void BeginBallisticMissileDescent()
        {
            IsBallisticMissileDescending = true;
            HasMissileWaypoint = false;
        }

        internal bool TrySetMissileWaypoint(Vector2 waypoint)
        {
            if (MissileObstacleBypassesRemaining <= 0)
            {
                return false;
            }

            MissileWaypoint = waypoint;
            HasMissileWaypoint = true;
            MissileObstacleBypassesRemaining--;
            return true;
        }

        internal void ClearMissileWaypoint()
        {
            HasMissileWaypoint = false;
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
