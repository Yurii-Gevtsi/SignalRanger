using Game.Gameplay.Battle.Domain;
using Game.Gameplay.Battle.Terrain;
using UnityEngine;

namespace Game.Gameplay.Battle.Net
{
    /// <summary>What reconciling one turn boundary changed, if anything.</summary>
    public readonly struct BattleReconcileResult
    {
        public BattleReconcileResult(bool diverged, int correctedUnits, int appliedCraters)
        {
            Diverged = diverged;
            CorrectedUnits = correctedUnits;
            AppliedCraters = appliedCraters;
        }

        /// <summary>True when the local picture disagreed with the host's.</summary>
        public bool Diverged { get; }
        public int CorrectedUnits { get; }
        public int AppliedCraters { get; }
    }

    /// <summary>
    /// Brings a client's battle back in line with the host's at a turn boundary.
    /// <para>
    /// This is what makes host authority workable without bit-exact maths: each
    /// client plays its own turn locally for responsiveness, and once the turn is
    /// over the host's picture wins. The correction lands while projectiles are gone
    /// and units have settled, so in practice it is invisible even when the numbers
    /// differed slightly.
    /// </para>
    /// </summary>
    public static class BattleTurnReconciler
    {
        /// <summary>Distance below which a position difference is left alone, in world units.</summary>
        public const float PositionTolerance = 0.02f;

        public static BattleReconcileResult Apply(
            BattleState state,
            BattleTerrain terrain,
            BattleTurnSnapshot snapshot)
        {
            if (state == null || snapshot == null)
            {
                return new BattleReconcileResult(false, 0, 0);
            }

            var correctedUnits = 0;
            for (var i = 0; i < snapshot.Units.Count; i++)
            {
                if (ApplyUnit(state, snapshot.Units[i]))
                {
                    correctedUnits++;
                }
            }

            var appliedCraters = 0;
            if (terrain != null)
            {
                for (var i = 0; i < snapshot.Craters.Count; i++)
                {
                    var crater = snapshot.Craters[i];
                    if (terrain.Grid.CarveCircle(crater.Center, crater.Radius, out _))
                    {
                        appliedCraters++;
                    }
                }
            }

            var localHash = BattleStateHash.Compute(BattleTurnSnapshot.Capture(state, snapshot.Craters));
            var diverged = localHash != BattleStateHash.Compute(snapshot);
            return new BattleReconcileResult(diverged || correctedUnits > 0, correctedUnits, appliedCraters);
        }

        private static bool ApplyUnit(BattleState state, BattleUnitSnapshot authoritative)
        {
            BattleUnitState unit = null;
            for (var teamIndex = 0; teamIndex < state.Teams.Count && unit == null; teamIndex++)
            {
                var team = state.Teams[teamIndex];
                for (var unitIndex = 0; unitIndex < team.Units.Count; unitIndex++)
                {
                    if (string.Equals(team.Units[unitIndex].UnitId, authoritative.UnitId, System.StringComparison.Ordinal))
                    {
                        unit = team.Units[unitIndex];
                        break;
                    }
                }
            }

            if (unit == null)
            {
                return false;
            }

            var corrected = false;
            if (Vector2.Distance(unit.Position, authoritative.Position) > PositionTolerance)
            {
                unit.MoveTo(authoritative.Position);
                corrected = true;
            }

            if (unit.FacingDirectionX != authoritative.FacingDirectionX)
            {
                unit.SetFacing(authoritative.FacingDirectionX);
                corrected = true;
            }

            // Health is the host's to state. Damage is applied rather than assigned so
            // the unit keeps its own clamping rules; a client that somehow ended up
            // healthier than the host is brought down, never quietly healed up.
            var difference = unit.Health - authoritative.Health;
            if (difference > 0)
            {
                unit.ApplyDamage(difference, "reconcile");
                corrected = true;
            }
            else if (difference < 0)
            {
                unit.RestoreHealth(-difference);
                corrected = true;
            }

            return corrected;
        }
    }
}
