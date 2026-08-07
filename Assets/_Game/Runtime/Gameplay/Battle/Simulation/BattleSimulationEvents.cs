using Game.Gameplay.Battle.Domain;
using Game.Gameplay.Battle.Weapons;
using UnityEngine;

namespace Game.Gameplay.Battle.Simulation
{
    /// <summary>Why a <see cref="BattleSimulation.TurnAdvanced"/> notification fired.</summary>
    public enum TurnAdvanceReason
    {
        ProjectileImpactResolved,
        InstantActionCompleted,
        PlayerTimerExpired,
        OpponentSkippedNoUsableWeapon
    }

    /// <summary>Why a <see cref="BattleSimulation.BattleFinished"/> notification fired.</summary>
    public enum BattleFinishReason
    {
        ProjectileImpactResolved,
        UnitMotionSettled,
        InstantActionCompleted
    }

    /// <summary>Outcome of <see cref="BattleSimulation.TryFire"/>.</summary>
    public enum BattleFireOutcome
    {
        /// <summary>Battle finished, a projectile is already in flight, or a unit is mid-motion.</summary>
        Blocked,
        NoActiveUnit,
        TurnTimerExpired,
        AimRequired,
        OutOfAmmo,
        NoWeaponSelected,
        Fired,
        BurstFired,
        UtilityUsed
    }

    public readonly struct BattleFireResult
    {
        public BattleFireResult(BattleFireOutcome outcome, BattleTeamId team, WeaponDefinition weapon, Vector2 effectPoint = default, int hitCount = 0)
        {
            Outcome = outcome;
            Team = team;
            Weapon = weapon;
            EffectPoint = effectPoint;
            HitCount = hitCount;
        }

        public BattleFireOutcome Outcome { get; }
        public BattleTeamId Team { get; }
        public WeaponDefinition Weapon { get; }

        /// <summary>Valid for <see cref="BattleFireOutcome.BurstFired"/>: where the melee burst landed.</summary>
        public Vector2 EffectPoint { get; }

        /// <summary>Valid for <see cref="BattleFireOutcome.BurstFired"/>: how many targets it hit.</summary>
        public int HitCount { get; }
    }

    public enum BattleWalkOutcome
    {
        /// <summary>Movement is not allowed right now (turn state, motion in progress, etc.).</summary>
        NotAllowed,
        None,
        Moved,
        Blocked,
        StartedFalling
    }

    public enum BattleJumpOutcome
    {
        NotAllowed,
        NotStable,
        Jumped
    }

    public readonly struct ImpactResolvedEvent
    {
        public ImpactResolvedEvent(Vector2 point, WeaponDefinition weapon, bool terrainChanged)
        {
            Point = point;
            Weapon = weapon;
            TerrainChanged = terrainChanged;
        }

        public Vector2 Point { get; }
        public WeaponDefinition Weapon { get; }
        public bool TerrainChanged { get; }
    }

    public readonly struct UnitDefeatedEvent
    {
        public UnitDefeatedEvent(string unitId, string reason)
        {
            UnitId = unitId;
            Reason = reason;
        }

        public string UnitId { get; }
        public string Reason { get; }
    }

    public readonly struct CoverDeployedEvent
    {
        public CoverDeployedEvent(Vector2 position, Vector2 size, WeaponDefinition weapon)
        {
            Position = position;
            Size = size;
            Weapon = weapon;
        }

        public Vector2 Position { get; }
        public Vector2 Size { get; }
        public WeaponDefinition Weapon { get; }
    }

    public readonly struct HealthRestoredEvent
    {
        public HealthRestoredEvent(Vector2 point, int amount, BattleTeamId team)
        {
            Point = point;
            Amount = amount;
            Team = team;
        }

        public Vector2 Point { get; }
        public int Amount { get; }
        public BattleTeamId Team { get; }
    }

    public readonly struct MineDeployedEvent
    {
        public MineDeployedEvent(int mineId, Vector2 point, WeaponDefinition weapon, BattleTeamId team)
        {
            MineId = mineId;
            Point = point;
            Weapon = weapon;
            Team = team;
        }

        public int MineId { get; }
        public Vector2 Point { get; }
        public WeaponDefinition Weapon { get; }
        public BattleTeamId Team { get; }
    }

    public readonly struct ChargeDeployedEvent
    {
        public ChargeDeployedEvent(int chargeId, Vector2 origin, Vector2 point, float dropDurationSeconds, WeaponDefinition weapon, BattleTeamId team)
        {
            ChargeId = chargeId;
            Origin = origin;
            Point = point;
            DropDurationSeconds = dropDurationSeconds;
            Weapon = weapon;
            Team = team;
        }

        public int ChargeId { get; }
        public Vector2 Origin { get; }
        public Vector2 Point { get; }
        public float DropDurationSeconds { get; }
        public WeaponDefinition Weapon { get; }
        public BattleTeamId Team { get; }
    }

    public readonly struct GroundObjectRepositionedEvent
    {
        public GroundObjectRepositionedEvent(int objectId, Vector2 point)
        {
            ObjectId = objectId;
            Point = point;
        }

        public int ObjectId { get; }
        public Vector2 Point { get; }
    }

    public readonly struct LinearWeaponEffectEvent
    {
        public LinearWeaponEffectEvent(
            WeaponDefinition weapon,
            BattleTeamId team,
            Vector2 origin,
            Vector2 direction,
            float range,
            float growthDurationSeconds,
            float lifetimeSeconds)
        {
            Weapon = weapon;
            Team = team;
            Origin = origin;
            Direction = direction;
            Range = range;
            GrowthDurationSeconds = growthDurationSeconds;
            LifetimeSeconds = lifetimeSeconds;
        }

        public WeaponDefinition Weapon { get; }
        public BattleTeamId Team { get; }
        public Vector2 Origin { get; }
        public Vector2 Direction { get; }
        public float Range { get; }
        public float GrowthDurationSeconds { get; }
        public float LifetimeSeconds { get; }
    }

    public readonly struct FireBlobEvent
    {
        public FireBlobEvent(
            int puddleId,
            WeaponDefinition weapon,
            BattleTeamId team,
            Vector2 origin,
            Vector2 initialVelocity,
            Vector2 gravity,
            Vector2 impactPoint,
            float travelSeconds,
            float lifetimeSeconds)
        {
            PuddleId = puddleId;
            Weapon = weapon;
            Team = team;
            Origin = origin;
            InitialVelocity = initialVelocity;
            Gravity = gravity;
            ImpactPoint = impactPoint;
            TravelSeconds = travelSeconds;
            LifetimeSeconds = lifetimeSeconds;
        }

        /// <summary>Identity of the burning puddle this blob turns into; used by
        /// <see cref="BattleSimulation.FirePuddleMoved"/> and
        /// <see cref="BattleSimulation.FirePuddleExtinguished"/>.</summary>
        public int PuddleId { get; }
        public WeaponDefinition Weapon { get; }
        public BattleTeamId Team { get; }
        public Vector2 Origin { get; }
        public Vector2 InitialVelocity { get; }
        public Vector2 Gravity { get; }
        public Vector2 ImpactPoint { get; }
        public float TravelSeconds { get; }
        public float LifetimeSeconds { get; }
    }
}
