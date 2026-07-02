using UnityEngine;

namespace Game.Core.Battle
{
    /// <summary>Builds a battle HUD attached to the given parent transform.</summary>
    public interface IBattleHudFactory
    {
        IBattleHud Create(Transform parent);
    }

    /// <summary>
    /// Static bridge so the battle scene can request a HUD without a compile-time
    /// dependency on any UI assembly. The app layer sets <see cref="Factory"/> during
    /// startup with an implementation that constructs the concrete uGUI view.
    /// </summary>
    public static class BattleHudFactoryBridge
    {
        public static IBattleHudFactory Factory { get; set; }

        public static IBattleHud Create(Transform parent)
        {
            return Factory != null ? Factory.Create(parent) : null;
        }
    }
}
