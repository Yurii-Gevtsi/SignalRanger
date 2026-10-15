using Game.Core.Modes;
using UnityEngine;

namespace Game.Gameplay.Battle.Net
{
    /// <summary>
    /// A short fingerprint of a turn boundary, used to notice that two clients have
    /// drifted apart without sending the whole state to find out.
    /// <para>
    /// Positions are quantised to a centimetre before hashing. Float maths on
    /// different devices can differ in the last bits, and a hash that trips on that
    /// would cry divergence on every turn; a centimetre is far below anything the
    /// player can see or the terrain grid can resolve, so a mismatch here means a
    /// real disagreement about what happened.
    /// </para>
    /// </summary>
    public static class BattleStateHash
    {
        /// <summary>Quantisation step for positions, in world units.</summary>
        public const float PositionQuantum = 0.01f;

        public static uint Compute(BattleTurnSnapshot snapshot)
        {
            if (snapshot == null)
            {
                return DeterministicSeed.Hash(string.Empty);
            }

            var hash = DeterministicSeed.Hash("battle.turn", snapshot.TurnNumber.ToString(), snapshot.ActiveUnitId);
            for (var i = 0; i < snapshot.Units.Count; i++)
            {
                var unit = snapshot.Units[i];
                hash = Combine(hash, DeterministicSeed.Hash(
                    unit.UnitId,
                    Quantise(unit.Position.x).ToString(),
                    Quantise(unit.Position.y).ToString(),
                    unit.Health.ToString(),
                    unit.FacingDirectionX.ToString()));
            }

            for (var i = 0; i < snapshot.Craters.Count; i++)
            {
                var crater = snapshot.Craters[i];
                hash = Combine(hash, DeterministicSeed.Hash(
                    Quantise(crater.Center.x).ToString(),
                    Quantise(crater.Center.y).ToString(),
                    Quantise(crater.Radius).ToString()));
            }

            return hash;
        }

        /// <summary>World value as a whole number of quanta.</summary>
        public static int Quantise(float value)
        {
            return Mathf.RoundToInt(value / PositionQuantum);
        }

        private static uint Combine(uint hash, uint value)
        {
            unchecked
            {
                return (hash ^ value) * 16777619;
            }
        }
    }
}
