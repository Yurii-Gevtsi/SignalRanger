using System;

namespace Game.Gameplay.Battle.Net
{
    /// <summary>
    /// How a command names the unit it acts on while in flight.
    /// <para>
    /// Local unit ids cannot travel: every client builds its own side as
    /// <c>player.*</c> and the other as <c>opponent.*</c>, so the same string means
    /// opposite commanders on the two machines. A command therefore names its actor
    /// as the owning player plus their slot, and each side resolves that back to
    /// whichever local unit it happens to be.
    /// </para>
    /// <para>
    /// The slot is what makes this work for team battles: in a duel it is always 0,
    /// but with two or three units a side it is the only thing distinguishing them.
    /// </para>
    /// </summary>
    public static class BattleUnitReference
    {
        private const char Separator = '#';

        public static string Encode(string playerId, int slot)
        {
            return $"{playerId}{Separator}{(slot < 0 ? 0 : slot)}";
        }

        public static bool TryDecode(string reference, out string playerId, out int slot)
        {
            playerId = string.Empty;
            slot = 0;
            if (string.IsNullOrEmpty(reference))
            {
                return false;
            }

            var separatorIndex = reference.LastIndexOf(Separator);
            if (separatorIndex <= 0 || separatorIndex == reference.Length - 1)
            {
                return false;
            }

            playerId = reference.Substring(0, separatorIndex);
            return int.TryParse(reference.Substring(separatorIndex + 1), out slot) && slot >= 0;
        }

        /// <summary>
        /// The local unit a reference points at, or an empty string when this client
        /// has no such unit - a mismatched roster, which is refused rather than
        /// guessed at.
        /// </summary>
        public static string ResolveLocalUnitId(BattleUnitAuthority authority, string reference)
        {
            if (authority == null || !TryDecode(reference, out var playerId, out var slot))
            {
                return string.Empty;
            }

            var units = authority.ResolveUnits(playerId);
            return slot < units.Count ? units[slot] : string.Empty;
        }

        /// <summary>Reference for a unit this client owns, for putting on the wire.</summary>
        public static string ResolveReference(BattleUnitAuthority authority, string localUnitId)
        {
            if (authority == null || string.IsNullOrEmpty(localUnitId))
            {
                return string.Empty;
            }

            var ownerId = authority.ResolveOwner(localUnitId);
            if (string.IsNullOrEmpty(ownerId))
            {
                return string.Empty;
            }

            var units = authority.ResolveUnits(ownerId);
            for (var i = 0; i < units.Count; i++)
            {
                if (string.Equals(units[i], localUnitId, StringComparison.Ordinal))
                {
                    return Encode(ownerId, i);
                }
            }

            return string.Empty;
        }
    }
}
