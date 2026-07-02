using System;

namespace Game.Core.Battle
{
    /// <summary>
    /// Battle HUD contract. The scene renders a <see cref="BattleHudState"/> each frame
    /// and reacts to button events, without knowing the concrete UI toolkit.
    /// </summary>
    public interface IBattleHud
    {
        event Action FireRequested;
        event Action CancelRequested;
        event Action NextWeaponRequested;
        event Action RestartRequested;
        event Action ContinueRequested;
        event Action MenuRequested;

        void Render(in BattleHudState state);
    }
}
