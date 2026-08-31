using System;

namespace Game.Core.Battle
{
    /// <summary>
    /// Battle HUD contract. The scene renders a <see cref="BattleHudState"/> each frame
    /// and reacts to input events, without knowing the concrete UI toolkit.
    /// </summary>
    public interface IBattleHud
    {
        event Action FireRequested;
        event Action<int> WeaponSelected;
        event Action<bool> ArsenalVisibilityChanged;
        event Action WeaponResetRequested;
        event Action SettingsRequested;
        event Action MoveLeftStarted;
        event Action MoveLeftStopped;
        event Action MoveRightStarted;
        event Action MoveRightStopped;
        event Action JumpLeftRequested;
        event Action JumpRightRequested;
        event Action RestartRequested;
        event Action ContinueRequested;
        event Action MenuRequested;
        event Action TutorialSkipRequested;

        void Render(in BattleHudState state);
    }
}
