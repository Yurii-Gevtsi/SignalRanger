using Game.Core.Localization;

namespace Game.Gameplay.Battle.Weapons
{
    public static class WeaponPresentationText
    {
        public static string DescribeRole(WeaponDefinition weapon)
        {
            if (weapon == null)
            {
                return GameText.RoleFallback();
            }

            return weapon.Role switch
            {
                WeaponRole.DirectProjectile => GameText.RoleDirect(),
                WeaponRole.ArcingExplosive => GameText.RoleArc(),
                WeaponRole.ShortRangeBurst => GameText.RoleBurst(),
                WeaponRole.CoverTool => GameText.RoleCover(),
                WeaponRole.DefensiveTool => GameText.RoleDefense(),
                WeaponRole.MovementTool => GameText.RoleMovement(),
                _ => GameText.RoleFallback()
            };
        }

        public static string DescribeBattleHint(WeaponDefinition weapon)
        {
            if (weapon == null)
            {
                return string.Empty;
            }

            return weapon.WeaponId switch
            {
                "weapon.steady-orb" => GameText.WeaponHintSteadyOrb(),
                "weapon.pulse-shell" => GameText.WeaponHintPulseShell(),
                "weapon.close-pop" => GameText.WeaponHintClosePop(),
                "weapon.long-lance" => GameText.WeaponHintLongLance(),
                "weapon.scatter-arc" => GameText.WeaponHintScatterArc(),
                "weapon.rampart-kit" => GameText.WeaponHintRampartKit(),
                "weapon.guard-pulse" => GameText.WeaponHintGuardPulse(),
                "weapon.relay-step" => GameText.WeaponHintRelayStep(),
                _ => GameText.WeaponHintFallback()
            };
        }
    }
}
