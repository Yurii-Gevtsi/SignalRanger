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
            if (weapon == null || weapon.Tags.Count == 0) return "зброя";
            if (weapon.Tags[0] == "straight-close" && HasTag(weapon, "engineering"))
            {
                return "пряма ближня / інженерна";
            }

            return weapon.Tags[0] switch
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
            return weapon?.WeaponId switch
            {
                "weapon.impulse" => "Базова енергетична гармата з навісною траєкторією. Надійна стартова зброя для стабільної шкоди.",
                "weapon.blaster-caset" => "Має три постріли малими зарядами. Добре підходить для ураження кількох цілей або добивання.",
                "weapon.bur" => "Промисловий лазер, що пропалює землю під собою або перед собою. Завдає малої шкоди, але допомагає вибратися з пастки чи пробити шлях.",
                "weapon.mortira" => "Запускає великий плазмовий заряд по дузі. Після вибуху він розпадається на менші уламки, які також завдають шкоди.",
                "weapon.granata" => "Класична метальна граната з вибухом у зоні попадання. Проста, зрозуміла й ефективна зброя.",
                "weapon.teleport" => "Переміщує дроїда в місце попадання сфери. Корисна для втечі, зміни позиції або обходу перешкод.",
                "weapon.c4" => "Потужна наземна вибухівка, яку можна встановити поруч із дроном. Завдає великої шкоди в зоні вибуху.",
                "weapon.mine" => "Наземна пастка, що детонує при наближенні ворога. Добре підходить для контролю проходів і захисту позиції.",
                "weapon.bur-rocket" => "Ракета з буровим наконечником, що проходить крізь перешкоди. Дозволяє атакувати цілі за укриттям.",
                "weapon.heal" => "Відновлює частину міцності корпусу дроїда. Допомагає протриматися довше в бою.",
                "weapon.bonus-box" => "Дає випадковий бонус: ремонт, ресурс, креслення або тимчасову зброю. Вміст залежить від прогресу гравця.",
                "weapon.toxic" => "Випускає токсичних наномітів, які завдають шкоди одразу й отруюють ціль на 3 ходи.",
                "weapon.emp" => "Стріляє електромагнітним зарядом, який порушує роботу систем ворога на кілька ходів: рух, щит або прицілювання.",
                "weapon.freeze" => "Завдає шкоди й заморожує ціль. Спочатку блокує рух, а потім тимчасово сповільнює дроїда.",
                "weapon.fire-gun" => "Стріляє згустками розпеченої плазми. Завдає шкоди й пропалює землю у зоні влучання протягом кількох секунд.",
                "weapon.missle" => "Самонавідна ракета, що летить у заданому напрямку й шукає ціль. Якщо не знаходить її за кілька секунд, самознищується.",
                "weapon.fpv-drone" => "Керований гравцем бойовий дрон. Дозволяє вручну довести заряд до цілі або облетіти перешкоди, детонує при зіткненні.",
                _ => string.Empty
            };
        }

        public static string DescribeBattleHint(WeaponDefinition weapon) => Describe(weapon);
    }
}
