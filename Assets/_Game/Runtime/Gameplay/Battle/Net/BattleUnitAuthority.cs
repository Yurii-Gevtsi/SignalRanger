using System;
using System.Collections.Generic;

namespace Game.Gameplay.Battle.Net
{
    /// <summary>
    /// Which commander controls which units. In a duel this is trivial - one unit
    /// each - but the whole point of storing it as a map is that 2v2 and 3v3 need it
    /// and a protocol without it cannot be extended to them.
    /// <para>
    /// This is the rule that stops a client moving a unit that is not theirs. It is
    /// checked by whoever is authoritative, never by the sender.
    /// </para>
    /// </summary>
    public sealed class BattleUnitAuthority
    {
        private readonly Dictionary<string, string> _ownerByUnitId = new(StringComparer.Ordinal);
        private readonly Dictionary<string, List<string>> _unitsByOwnerId = new(StringComparer.Ordinal);

        public static BattleUnitAuthority Empty { get; } = new();

        /// <summary>Local commander id, used to decide which commands to originate.</summary>
        public string LocalPlayerId { get; private set; } = string.Empty;

        public void SetLocalPlayer(string playerId)
        {
            LocalPlayerId = playerId ?? string.Empty;
        }

        public void Assign(string playerId, string unitId)
        {
            if (string.IsNullOrWhiteSpace(playerId) || string.IsNullOrWhiteSpace(unitId))
            {
                return;
            }

            _ownerByUnitId[unitId] = playerId;
            if (!_unitsByOwnerId.TryGetValue(playerId, out var units))
            {
                units = new List<string>();
                _unitsByOwnerId.Add(playerId, units);
            }

            if (!units.Contains(unitId))
            {
                units.Add(unitId);
            }
        }

        /// <summary>Owner of the unit, or an empty string when nobody claims it.</summary>
        public string ResolveOwner(string unitId)
        {
            return unitId != null && _ownerByUnitId.TryGetValue(unitId, out var owner) ? owner : string.Empty;
        }

        public IReadOnlyList<string> ResolveUnits(string playerId)
        {
            return playerId != null && _unitsByOwnerId.TryGetValue(playerId, out var units)
                ? units
                : Array.Empty<string>();
        }

        /// <summary>
        /// Whether this command may be applied. Unclaimed units are refused rather
        /// than allowed: an unmapped unit means the match was set up wrong, and
        /// failing closed keeps that from turning into a player moving someone
        /// else's commander.
        /// </summary>
        public bool CanControl(string playerId, string unitId)
        {
            if (string.IsNullOrWhiteSpace(playerId) || string.IsNullOrWhiteSpace(unitId))
            {
                return false;
            }

            return _ownerByUnitId.TryGetValue(unitId, out var owner)
                   && string.Equals(owner, playerId, StringComparison.Ordinal);
        }

        public bool IsLocallyControlled(string unitId)
        {
            return CanControl(LocalPlayerId, unitId);
        }
    }
}
