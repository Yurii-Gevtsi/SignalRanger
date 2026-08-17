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
                return Game.Core.Localization.GameText.WeaponCategory("straight-close-engineering", "пряма ближня / інженерна");
            }

            var fallback = weapon.Tags[0] switch
            {
                "arc" => "навісна",
                "arc-utility" => "навісна / утилітарна",
                "arc-special" => "навісна / спеціальна",
                "arc-homing" => "навісна / самонаведення",
                "straight-long" => "пряма далека",
                "straight-close" => "пряма ближня",
                "ground" => "наземна",
                "bonus" => "бонус",
                "drop" => "дроп на карті",
                "controlled" => "керована",
                _ => "зброя"
            };
            return Game.Core.Localization.GameText.WeaponCategory(weapon.Tags[0], fallback);
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
            var fallback = weaponId switch
            {
                "weapon.impulse" => "Стандартний заряд бойового реактора дроїда — летить по дузі й дістає ворога навіть за укриттям. Слабких місць немає, тому це перша зброя, яку варто брати в будь-який бій.",
                "weapon.blaster-caset" => "Касета з трьома легкими зарядами — розкидай постріли по кількох ворогах або зосередь усі на одному.",
                "weapon.bur" => "Гірничий лазер, а не бойова зброя — головна цінність не в шкоді, а в тунелі, що лишається: втечи з ями або зайди ворогу з несподіваного боку.",
                "weapon.mortira" => "Важка мортира б'є по дузі великим плазмовим зарядом, а після вибуху ще й розкидає уламки — рахуй не лише на прямий удар, а й на осколкову шкоду навколо.",
                "weapon.granata" => "Стандартна граната інженерного корпусу — летить по дузі й вибухає по колу. Без сюрпризів, зате завжди під рукою.",
                "weapon.teleport" => "Кидаєш сферу як гранату — вона відскакує й котиться рельєфом, а щойно зупиняється, телепортує дроїда в точку зупинки. Вислизни з пастки, зайди ворогу в тил або дістанься недосяжної висоти.",
                "weapon.c4" => "Важкий плазмовий заряд для підриву позиції — став його впритул до цілі й відступи: вибух не жаліє нікого в радіусі ураження.",
                "weapon.mine" => "Наземна пастка з сенсором руху — постав її в проході чи на підході до своєї позиції: вибухне, тільки коли хтось наступить. Обережно — спрацьовує й на власного дроїда, якщо забути, де вона лежить.",
                "weapon.bur-rocket" => "Ракета з буровим наконечником пробиває перешкоди на льоту — стріляй прямо крізь стіну, за якою ховається ворог.",
                "weapon.heal" => "Польовий ремкомплект латає корпус дроїда просто в бою. Не витрачай його завчасно: найбільша користь одразу після важкого обміну ударами, а не про запас.",
                "weapon.bonus-box" => "Дає випадковий бонус: ремонт, ресурс, креслення або тимчасову зброю. Вміст залежить від прогресу гравця.",
                "weapon.toxic" => "Випускає токсичних наномітів, які завдають шкоди одразу й отруюють ціль на 3 ходи.",
                "weapon.emp" => "Електромагнітний заряд не руйнує корпус, а виводить з ладу системи цілі — слабка шкода, зате може зірвати ворогу найважливіший хід.",
                "weapon.freeze" => "Завдає шкоди й заморожує ціль на 1 хід, блокуючи їй рух.",
                "weapon.fire-gun" => "Викидає струмінь розплавленої лави — крапля шкодить одразу, а розпечена земля ще секунди пропалює всіх, хто на неї ступить.",
                "weapon.missle" => "Пускаєш у напрямку ворога — головка самонаведення сама довершує наведення. Найкраще на середній і великій дистанції, де ручний приціл ненадійний.",
                "weapon.fpv-drone" => "Береш керування дроном на себе й ведеш його наживо повз укриття та пастки прямо в борт ворога. Детонує при зіткненні.",
                _ => string.Empty
            };
            return Game.Core.Localization.GameText.WeaponDescription(weaponId, fallback);
        }

        public static string DescribeBattleHint(WeaponDefinition weapon) => Describe(weapon);
    }
}
