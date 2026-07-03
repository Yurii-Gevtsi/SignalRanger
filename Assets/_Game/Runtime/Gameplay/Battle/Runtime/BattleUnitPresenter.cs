using Game.Gameplay.Battle.Domain;
using UnityEngine;

namespace Game.Gameplay.Battle.Runtime
{
    public sealed class BattleUnitPresenter
    {
        private readonly BattleUnitState _state;
        private readonly BattleUnitView _view;

        public BattleUnitPresenter(BattleUnitState state, BattleUnitView view)
        {
            _state = state;
            _view = view;
        }

        public BattleUnitState State => _state;
        public BattleUnitView View => _view;

        public void Initialize(Color teamColor)
        {
            _view.transform.position = _state.Position;
            _view.SetColor(teamColor);
            _view.SetFacing(_state.FacingDirectionX);
            Refresh(false);
        }

        public void Refresh(bool isActive)
        {
            _view.transform.position = _state.Position;
            _view.SetFacing(_state.FacingDirectionX);
            _view.SetHealth(_state.Health, _state.MaxHealth);
            _view.SetDefeated(_state.IsDefeated);
            _view.SetActiveTurn(isActive && !_state.IsDefeated);
        }
    }
}
