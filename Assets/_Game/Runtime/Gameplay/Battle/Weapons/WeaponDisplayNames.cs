using System;
using System.Collections.Generic;
using System.Linq;

namespace Game.Gameplay.Battle.Weapons
{
    public static class WeaponDisplayNames
    {
        private static readonly Dictionary<string, string> Names = new()
        {
            { "weapon.impulse", "Імпульсна гармата" },
            { "weapon.blaster-caset", "Касетний бластер" },
            { "weapon.bur", "Буровий лазер" },
            { "weapon.mortira", "Мортира" },
            { "weapon.granata", "Граві-граната" },
            { "weapon.teleport", "Телепорт-сфера" },
            { "weapon.c4", "Плазмовий заряд" },
            { "weapon.mine", "Сенсорна міна" },
            { "weapon.bur-rocket", "Бурова ракета" },
            { "weapon.heal", "Ремонтний комплект" },
            { "weapon.bonus-box", "Контейнер підтримки" },
            { "weapon.toxic", "Нанотоксичний заряд" },
            { "weapon.emp", "EMP-гармата" },
            { "weapon.freeze", "Кріо-гармата" },
            { "weapon.fire-gun", "Лавова пушка" },
            { "weapon.missle", "Ракета-наводчик" },
            { "weapon.fpv-drone", "FPV-дрон" }
        };

        public static string Get(string weaponId)
        {
            if (string.IsNullOrWhiteSpace(weaponId)) return "Зброя";
            if (Names.TryGetValue(weaponId, out var name)) return name;

            var trimmed = weaponId.StartsWith("weapon.", StringComparison.Ordinal) ? weaponId.Substring(7) : weaponId;
            return string.Join(" ", trimmed.Replace('-', ' ').Replace('.', ' ').Split(' ')
                .Where(part => part.Length > 0).Select(part => char.ToUpperInvariant(part[0]) + part.Substring(1)));
        }
    }
}
