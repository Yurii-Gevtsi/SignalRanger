using System;
using System.Collections.Generic;
using System.Linq;

namespace Game.Gameplay.Battle.Domain
{
    public sealed class BattleState
    {
        private readonly List<BattleTeamState> _teams;

        public BattleState(IEnumerable<BattleTeamState> teams, TurnState turn)
        {
            _teams = teams.ToList();
            Turn = turn;
        }

        public IReadOnlyList<BattleTeamState> Teams => _teams;
        public TurnState Turn { get; private set; }

        public BattleTeamState GetTeam(BattleTeamId teamId)
        {
            return _teams.First(team => team.TeamId == teamId);
        }

        public BattleUnitState GetUnit(string unitId)
        {
            return _teams
                .SelectMany(team => team.Units)
                .First(unit => string.Equals(unit.UnitId, unitId, StringComparison.Ordinal));
        }

        public IReadOnlyList<BattleUnitState> GetLivingUnits(BattleTeamId teamId)
        {
            return GetTeam(teamId).Units.Where(unit => !unit.IsDefeated).ToList();
        }

        public void SetTurn(TurnState turn)
        {
            Turn = turn;
        }
    }
}
