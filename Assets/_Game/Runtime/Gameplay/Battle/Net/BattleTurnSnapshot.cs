using System;
using System.Collections.Generic;
using Game.Gameplay.Battle.Domain;
using UnityEngine;

namespace Game.Gameplay.Battle.Net
{
    /// <summary>One unit's authoritative state at a turn boundary.</summary>
    public readonly struct BattleUnitSnapshot
    {
        public BattleUnitSnapshot(string unitId, Vector2 position, int health, int facingDirectionX)
        {
            UnitId = unitId ?? string.Empty;
            Position = position;
            Health = health;
            FacingDirectionX = facingDirectionX >= 0 ? 1 : -1;
        }

        public string UnitId { get; }
        public Vector2 Position { get; }
        public int Health { get; }
        public int FacingDirectionX { get; }
    }

    /// <summary>A hole punched in the terrain during the turn.</summary>
    public readonly struct BattleCraterSnapshot
    {
        public BattleCraterSnapshot(Vector2 center, float radius)
        {
            Center = center;
            Radius = radius;
        }

        public Vector2 Center { get; }
        public float Radius { get; }
    }

    /// <summary>
    /// The host's authoritative picture at the end of a turn, and the only thing
    /// clients reconcile against.
    /// <para>
    /// Terrain travels as the craters cut this turn rather than as a mask. The mask
    /// for a shipped arena is roughly 2240x1260 cells - hundreds of kilobytes, which
    /// no turn budget can carry - while a turn cuts a handful of circles, and
    /// <c>TerrainGrid.CarveCircle</c> reproduces each one exactly from a centre and a
    /// radius.
    /// </para>
    /// <para>
    /// A turn boundary is also the cheapest possible moment to correct: projectiles
    /// are gone and units have settled, so a correction moves nothing that the eye is
    /// tracking.
    /// </para>
    /// </summary>
    public sealed class BattleTurnSnapshot
    {
        public BattleTurnSnapshot(
            int turnNumber,
            string activeUnitId,
            IReadOnlyList<BattleUnitSnapshot> units,
            IReadOnlyList<BattleCraterSnapshot> craters)
        {
            TurnNumber = turnNumber;
            ActiveUnitId = activeUnitId ?? string.Empty;
            Units = units ?? Array.Empty<BattleUnitSnapshot>();
            Craters = craters ?? Array.Empty<BattleCraterSnapshot>();
        }

        public int TurnNumber { get; }

        /// <summary>Whose turn it is once this snapshot has been applied.</summary>
        public string ActiveUnitId { get; }
        public IReadOnlyList<BattleUnitSnapshot> Units { get; }
        public IReadOnlyList<BattleCraterSnapshot> Craters { get; }

        /// <summary>Captures the live battle state; craters are collected by the caller.</summary>
        public static BattleTurnSnapshot Capture(
            BattleState state,
            IReadOnlyList<BattleCraterSnapshot> craters)
        {
            if (state == null)
            {
                return new BattleTurnSnapshot(0, string.Empty, Array.Empty<BattleUnitSnapshot>(), craters);
            }

            var units = new List<BattleUnitSnapshot>();
            for (var teamIndex = 0; teamIndex < state.Teams.Count; teamIndex++)
            {
                var team = state.Teams[teamIndex];
                for (var unitIndex = 0; unitIndex < team.Units.Count; unitIndex++)
                {
                    var unit = team.Units[unitIndex];
                    if (unit == null)
                    {
                        continue;
                    }

                    units.Add(new BattleUnitSnapshot(unit.UnitId, unit.Position, unit.Health, unit.FacingDirectionX));
                }
            }

            units.Sort((left, right) => string.CompareOrdinal(left.UnitId, right.UnitId));
            return new BattleTurnSnapshot(state.Turn.TurnNumber, state.Turn.ActiveUnitId, units, craters);
        }
    }
}
