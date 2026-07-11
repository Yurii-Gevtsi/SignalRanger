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
}
