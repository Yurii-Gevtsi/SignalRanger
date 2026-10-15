using Game.Core.Localization;

namespace Game.Gameplay.Battle.Weapons
{
    public static class WeaponDisplayNames
    {
        public static string Get(string weaponId)
        {
            return string.IsNullOrWhiteSpace(weaponId)
                ? GameText.Weapon
                : GameText.WeaponName(weaponId, string.Empty);
        }
    }
}
