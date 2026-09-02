namespace Game.Gameplay.Battle.Weapons
{
    public static class WeaponPresentationText
    {
        public static string DescribeRole(WeaponDefinition weapon) => DescribeCategory(weapon);

        public static bool IsDropOnly(WeaponDefinition weapon)
        {
            return weapon?.WeaponId == "weapon.bonus-box";
        }

        public static string DescribeCategory(WeaponDefinition weapon)
        {
            if (weapon == null || weapon.Tags.Count == 0) return Game.Core.Localization.GameText.RoleFallback();
            if (weapon.Tags[0] == "straight-close" && HasTag(weapon, "engineering"))
            {
                return Game.Core.Localization.GameText.WeaponCategory("straight-close-engineering", string.Empty);
            }

            return Game.Core.Localization.GameText.WeaponCategory(weapon.Tags[0], string.Empty);
        }

        private static bool HasTag(WeaponDefinition weapon, string tag)
        {
            for (var index = 0; index < weapon.Tags.Count; index++)
            {
                if (weapon.Tags[index] == tag)
                {
                    return true;
                }
            }

            return false;
        }

        public static string Describe(WeaponDefinition weapon)
        {
            var weaponId = weapon?.WeaponId;
            return Game.Core.Localization.GameText.WeaponDescription(weaponId, string.Empty);
        }

        public static string DescribeBattleHint(WeaponDefinition weapon) => Describe(weapon);
    }
}
