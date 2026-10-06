using Game.Gameplay.Battle.Domain;
using UnityEngine;

namespace Game.Gameplay.Battle.Runtime
{
    public sealed class BattleUnitPresenter
    {
        private const float MinimumHorizontalMovementDelta = 0.001f;
        private static readonly Color PlayerHealthColor = new(0.06f, 1f, 0.10f, 1f);
        private static readonly Color OpponentHealthColor = new(1f, 0.05f, 0.03f, 1f);

        private readonly BattleUnitState _state;
        private readonly BattleUnitView _view;
        private Vector2 _lastPresentedPosition;
        private bool _hasPresentedPosition;

        public BattleUnitPresenter(BattleUnitState state, BattleUnitView view)
        {
            _state = state;
            _view = view;
        }

        public BattleUnitState State => _state;
        public BattleUnitView View => _view;

        public void SetFacing(int facingDirectionX)
        {
            _state.SetFacing(facingDirectionX);
            _view.SetFacing(_state.FacingDirectionX);
        }

        public void SetEquippedWeapon(Sprite sprite, bool sourceFacesRight)
        {
            _view.SetEquippedWeapon(sprite, sourceFacesRight);
        }

        public void SetWeaponPresentation(Vector2 scaleMultiplier, Vector2 localPosition, float aimAngleOffset)
        {
            _view.SetWeaponPresentation(scaleMultiplier, localPosition, aimAngleOffset);
        }

        public void ClearEquippedWeapon()
        {
            _view.ClearEquippedWeapon();
        }

        public void SetVirusActive(bool isActive)
        {
            _view.SetVirusActive(isActive);
        }

        public void SetRepairJammerActive(bool isActive)
        {
            _view.SetRepairJammerActive(isActive);
        }

        public void SetWeaponAim(Vector2 direction)
        {
            _view.SetWeaponAim(direction);
        }

        public void Initialize(Color teamColor)
        {
            _view.transform.position = _state.Position;
            _lastPresentedPosition = _state.Position;
            _hasPresentedPosition = true;
            _view.SetColor(teamColor);
            _view.SetHealthColor(_state.TeamId == BattleTeamId.Player ? PlayerHealthColor : OpponentHealthColor);
            _view.SetFacing(_state.FacingDirectionX);
            _view.SetMoving(false);
            Refresh(false);
        }

        public void Refresh(bool isActive)
        {
            Refresh(isActive, false, 0f);
        }

        public void Refresh(bool isActive, bool isJumping)
        {
            Refresh(isActive, isJumping, isJumping ? 1f : 0f);
        }

        public void Refresh(bool isActive, bool isJumping, float jumpTrailScale)
        {
            var isMoving = _hasPresentedPosition
                && Mathf.Abs(_state.Position.x - _lastPresentedPosition.x) > MinimumHorizontalMovementDelta;
            _view.transform.position = _state.Position;
            _lastPresentedPosition = _state.Position;
            _hasPresentedPosition = true;
            _view.SetFacing(_state.FacingDirectionX);
            _view.SetHealth(_state.Health, _state.MaxHealth);
            _view.SetDefeated(_state.IsDefeated);
            _view.SetMoving(isMoving && !_state.IsDefeated);
            _view.SetJetTrailActive(isJumping && !_state.IsDefeated, jumpTrailScale);
            _view.SetActiveTurn(isActive && !_state.IsDefeated);
        }
    }
}
