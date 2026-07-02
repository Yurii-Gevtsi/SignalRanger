using System;
using System.Collections.Generic;
using System.Linq;

namespace Game.Gameplay.Battle.Weapons
{
    /// <summary>
    /// Ukrainian display names for prototype weapons. Temporary until a real
    /// localization layer lands; keeps raw weapon IDs out of the UI.
    /// </summary>
    public static class WeaponDisplayNames
    {
        private static readonly Dictionary<string, string> Names = new()
        {
            { "weapon.steady-orb", "Стійка сфера" },
            { "weapon.pulse-shell", "Пульс-снаряд" },
            { "weapon.close-pop", "Ближній удар" },
            { "weapon.long-lance", "Довгий спис" },
            { "weapon.scatter-arc", "Розсіювач" }
        };

        public static string Get(string weaponId)
        {
            if (string.IsNullOrWhiteSpace(weaponId))
            {
                return "Зброя";
            }

            if (Names.TryGetValue(weaponId, out var name))
            {
                return name;
            }

            var trimmed = weaponId.StartsWith("weapon.", StringComparison.Ordinal) ? weaponId.Substring(7) : weaponId;
            trimmed = trimmed.Replace('-', ' ').Replace('.', ' ');
            return string.Join(" ", trimmed.Split(' ')
                .Where(part => part.Length > 0)
                .Select(part => char.ToUpperInvariant(part[0]) + part.Substring(1)));
        }
    }
}
