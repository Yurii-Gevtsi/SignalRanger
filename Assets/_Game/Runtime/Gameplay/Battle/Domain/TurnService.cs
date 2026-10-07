using System.Collections.Generic;
using System.Linq;

namespace Game.Gameplay.Battle.Domain
{
    /// <summary>
    /// Teams take turns, and inside a team the droids take turns in rotation: A1 → B1 → A2 → B2 → A1 …
    /// The service lives as long as the battle and remembers which droid of each team acted last.
    /// </summary>
    public sealed class TurnService : ITurnService
    {
        private readonly Dictionary<BattleTeamId, string> _lastActedUnitByTeam = new();

        public void StartFirstTurn(BattleState state, BattleTeamId startingTeam)
        {
            var activeTeam = HasLivingUnits(state, startingTeam)
                ? startingTeam
                : GetOpposingTeam(startingTeam);

            state.SetTurn(CreateTurn(state, activeTeam, 1));
        }

        public void SpendAction(BattleState state)
        {
            if (!state.Turn.ActionSpent)
            {
                state.SetTurn(state.Turn.SpendAction());
            }
        }

        public void AdvanceTurn(BattleState state)
        {
            var nextTeam = GetOpposingTeam(state.Turn.ActiveTeam);
            if (!HasLivingUnits(state, nextTeam))
            {
                nextTeam = state.Turn.ActiveTeam;
            }

            state.SetTurn(CreateTurn(state, nextTeam, state.Turn.TurnNumber + 1));
        }

        public bool TransferTurn(BattleState state, string unitId)
        {
            var unit = state.GetUnit(unitId);
            if (unit == null || unit.IsDefeated || unit.TeamId != state.Turn.ActiveTeam
                || unit.UnitId == state.Turn.ActiveUnitId)
            {
                return false;
            }

            _lastActedUnitByTeam[unit.TeamId] = unit.UnitId;
            state.SetTurn(new TurnState(state.Turn.ActiveTeam, unit.UnitId, state.Turn.TurnNumber, false));
            return true;
        }

        private TurnState CreateTurn(BattleState state, BattleTeamId activeTeam, int turnNumber)
        {
            var activeUnit = SelectNextUnit(state, activeTeam);
            if (activeUnit != null)
            {
                _lastActedUnitByTeam[activeTeam] = activeUnit.UnitId;
            }

            return new TurnState(
                activeTeam,
                activeUnit?.UnitId ?? string.Empty,
                turnNumber,
                false);
        }

        /// <summary>The living droid after the one that acted last in the team's own order (the first one at the start).</summary>
        private BattleUnitState SelectNextUnit(BattleState state, BattleTeamId team)
        {
            var units = state.GetTeam(team).Units;
            if (units.Count == 0)
            {
                return null;
            }

            var start = 0;
            if (_lastActedUnitByTeam.TryGetValue(team, out var lastId))
            {
                for (var index = 0; index < units.Count; index++)
                {
                    if (units[index] != null && units[index].UnitId == lastId)
                    {
                        start = index + 1;
                        break;
                    }
                }
            }

            for (var offset = 0; offset < units.Count; offset++)
            {
                var candidate = units[(start + offset) % units.Count];
                if (candidate != null && !candidate.IsDefeated)
                {
                    return candidate;
                }
            }

            return null;
        }

        private static bool HasLivingUnits(BattleState state, BattleTeamId team)
        {
            return state.GetTeam(team).HasLivingUnits;
        }

        private static BattleTeamId GetOpposingTeam(BattleTeamId team)
        {
            return team == BattleTeamId.Player
                ? BattleTeamId.Opponent
                : BattleTeamId.Player;
        }
    }
}
