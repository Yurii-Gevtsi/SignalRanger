using System.Collections.Generic;
using System.Linq;

namespace Game.Gameplay.Battle.Domain
{
    public sealed class BattleTeamState
    {
        private readonly List<BattleUnitState> _units = new();

        public BattleTeamState(BattleTeamId teamId, IEnumerable<BattleUnitState> units)
        {
            TeamId = teamId;
            _units.AddRange(units.Where(unit => unit.TeamId == teamId));
        }

        public BattleTeamId TeamId { get; }
        public IReadOnlyList<BattleUnitState> Units => _units;
        public bool HasLivingUnits => _units.Any(unit => !unit.IsDefeated);
    }
}
