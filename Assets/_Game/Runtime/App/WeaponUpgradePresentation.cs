using Game.Core.Localization;
using Game.Data.Progression;
using Game.Gameplay.Battle.Weapons;
using UnityEngine;

namespace Game.App
{
    /// <summary>
    /// Concise player-facing text for a concrete workshop node. Gameplay stays data-driven;
    /// this class only explains the selected improvement in the workshop UI. Every string
    /// returned to callers is routed through <see cref="GameText"/> — the literals here are
    /// fallback text only, used when a locale has no override for the key.
    /// </summary>
    public static class WeaponUpgradePresentation
    {
        public static string GetTitle(WeaponProgressionDefinition definition, int rank, WeaponUpgradeBranch branch)
        {
            if (rank == 1)
            {
                return GameText.UpgradeBaseConfigTitle();
            }

            if (!definition.TryGetNode(rank, branch, out var node))
            {
                return GameText.UpgradeGenericTitle();
            }

            if (node.IsEvolution)
            {
                if (!string.IsNullOrWhiteSpace(node.EvolutionName))
                {
                    return GameText.WeaponEvolutionName(node.MechanicId, node.EvolutionName);
                }

                return rank == 5 ? GameText.UpgradeEvolutionChoiceTitle() : GameText.UpgradeFinalEvolutionTitle();
            }

            return GameText.UpgradeStatBonusTitle(GetStatName(node.Stat), node.Amount);
        }

        public static string GetDescription(WeaponProgressionDefinition definition, int rank, WeaponUpgradeBranch branch)
        {
            if (rank == 1 || !definition.TryGetNode(rank, branch, out var node))
            {
                return string.Empty;
            }

            if (!node.IsEvolution)
            {
                return GameText.UpgradeStatImprovement(GetStatName(node.Stat), node.Amount);
            }

            return GetMechanicDescription(definition.WeaponId, node.MechanicId);
        }

        /// <summary>
        /// Flavor text for the rank/branch the player has actually reached: the
        /// mechanic unlocked at rank 5 or 10, or nothing below rank 5. Meant to be
        /// appended to the base weapon description on the "current rank" card.
        /// </summary>
        public static string GetActiveEvolutionText(WeaponProgressionDefinition definition, int rank, WeaponUpgradeBranch branch)
        {
            if (definition == null || rank < 5 || branch == WeaponUpgradeBranch.None)
            {
                return string.Empty;
            }

            var evolvedRank = rank >= 10 ? 10 : 5;
            return GetDescription(definition, evolvedRank, branch);
        }

        /// <summary>
        /// Current damage after applying every purchased Damage-stat node up to
        /// <paramref name="rank"/> on the matching branch (or branch-less nodes).
        /// Only Damage is resolved here - the battle simulation doesn't yet consume
        /// the other stat modifiers, so there is nothing else to reflect in the UI.
        /// </summary>
        public static int ResolveCurrentDamage(WeaponDefinition weapon, WeaponProgressionDefinition definition, int rank, WeaponUpgradeBranch branch)
        {
            if (weapon == null)
            {
                return 0;
            }

            if (definition == null)
            {
                return weapon.Damage;
            }

            var bonusPercent = 0;
            var nodes = definition.Nodes;
            for (var i = 0; i < nodes.Count; i++)
            {
                var node = nodes[i];
                if (node.Rank <= rank
                    && node.Stat == WeaponUpgradeStat.Damage
                    && (node.Branch == WeaponUpgradeBranch.None || node.Branch == branch))
                {
                    bonusPercent += node.Amount;
                }
            }

            return Mathf.RoundToInt(weapon.Damage * (1f + bonusPercent / 100f));
        }

        public static string GetWeaponName(WeaponProgressionDefinition definition, int rank,
            WeaponUpgradeBranch branch, string fallbackName)
        {
            if (definition == null || rank < 5 || branch == WeaponUpgradeBranch.None)
            {
                return fallbackName;
            }

            var evolutionRank = rank >= 10 ? 10 : 5;
            return definition.TryGetNode(evolutionRank, branch, out var node)
                && !string.IsNullOrWhiteSpace(node.EvolutionName)
                    ? GameText.WeaponEvolutionName(node.MechanicId, node.EvolutionName)
                    : fallbackName;
        }

        public static string GetStatName(WeaponUpgradeStat stat)
        {
            var fallback = stat switch
            {
                WeaponUpgradeStat.Damage => "Шкода",
                WeaponUpgradeStat.ImpactRadius => "Радіус вибуху",
                WeaponUpgradeStat.Range => "Дальність",
                WeaponUpgradeStat.Ammo => "Кількість у бою",
                WeaponUpgradeStat.ProjectileCount => "Кількість снарядів",
                WeaponUpgradeStat.Duration => "Тривалість дії",
                WeaponUpgradeStat.Knockback => "Відкидання",
                WeaponUpgradeStat.TerrainDamage => "Руйнування ґрунту",
                WeaponUpgradeStat.Control => "Контроль",
                WeaponUpgradeStat.Healing => "Відновлення",
                WeaponUpgradeStat.Fuse => "Затримка детонації",
                WeaponUpgradeStat.Accuracy => "Точність",
                WeaponUpgradeStat.Homing => "Наведення",
                WeaponUpgradeStat.Penetration => "Пробиття",
                WeaponUpgradeStat.Mobility => "Маневреність",
                WeaponUpgradeStat.Charge => "Заряд",
                _ => "Спецможливість"
            };
            return GameText.UpgradeStatName(stat.ToString(), fallback);
        }

        private static string GetMechanicDescription(string weaponId, string mechanicId)
        {
            var fallback = GetMechanicDescriptionFallback(weaponId, mechanicId);
            return GameText.WeaponEvolutionDescription(mechanicId, fallback);
        }

        private static string GetMechanicDescriptionFallback(string weaponId, string mechanicId)
        {
            return weaponId switch
            {
                "weapon.impulse" => mechanicId switch
                {
                    "double-impulse" => "Два послідовні постріли за раз.",
                    "triple-impulse" => "Три послідовні постріли за раз.",
                    "impact-impulse" => "Один постріл із помітним відкиданням.",
                    "assault-impulse" => "Один постріл із сильним відкиданням.",
                    _ => string.Empty
                },
                "weapon.blaster-caset" => mechanicId switch
                {
                    "quad-blaster" => "Чотири послідовні прямі постріли.",
                    "five-shot-blaster" => "П’ять послідовних прямих пострілів.",
                    "ricochet-blaster" => "Три постріли; кожен може один раз відбитись від поверхні.",
                    "double-ricochet-blaster" => "Три постріли; кожен може відбитись від поверхні до двох разів.",
                    _ => string.Empty
                },
                "weapon.bur" => mechanicId switch
                {
                    "long-range-drill" => "Збільшує дальність прорізання звичайного ґрунту.",
                    "deep-range-drill" => "Ще сильніше збільшує дальність прорізання звичайного ґрунту.",
                    "universal-drill" => "Прорізає звичайний ґрунт і тонкі металеві конструкції; дальність менша, ніж у гілки A.",
                    "pioneer-drill" => "Проходить крізь міцніші металеві та титанові перешкоди; дальність менша, ніж у гілки A.",
                    _ => string.Empty
                },
                "weapon.granata" => mechanicId switch
                {
                    "sticky-grenade" => "Після контакту прилипає до дроїда або поверхні й не скочується.",
                    "magnetic-sticky-grenade" => "Липуча граната додатково притягується до дроїдів і металевих конструкцій поруч.",
                    "gravity-grenade" => "Після зупинки до вибуху стягує незакріплені цілі до себе.",
                    "collapse-grenade" => "Те саме поле притягування, але з більшим радіусом дії.",
                    _ => string.Empty
                },
                "weapon.mine" => mechanicId switch
                {
                    "four-mines" => "Дозволяє встановити до 4 мін за один хід.",
                    "five-mines" => "Дозволяє встановити до 5 мін за один хід.",
                    "recognition-mine" => "Не детонує від союзників: спрацьовує лише коли поруч ворог.",
                    "friendly-mine" => "Спрацьовує лише на ворогів, а вибух не завдає шкоди й не відкидає союзників.",
                    _ => string.Empty
                },
                "weapon.heal" => mechanicId switch
                {
                    "protective-repair" => "Після лікування зменшує вхідну шкоду дроїду на 50% протягом наступного ходу.",
                    "enhanced-protective-repair" => "Після лікування зменшує вхідну шкоду дроїду на 50% протягом двох наступних ходів.",
                    "regenerative-repair" => "Якщо дроїд протримається хід без жодної шкоди, наноботи виконують ще один цикл лікування.",
                    "long-regenerative-repair" => "Наноботи виконують додатковий цикл лікування за кожен із двох наступних ходів без шкоди.",
                    _ => string.Empty
                },
                "weapon.c4" => mechanicId switch
                {
                    "enhanced-explosive-charge" => "Більший радіус вибуху, більше шкоди по зоні та більша вирва.",
                    "breaching-explosive-charge" => "Ще сильніше руйнує ґрунт і пробиває найміцніші укриття, лишаючи по-справжньому глибоку вирву.",
                    "impulse-explosive-charge" => "Звичайна або трохи менша вирва, зате значно сильніше відкидання дроїдів.",
                    "shockwave-charge" => "Ще сильніша ударна хвиля: ураженого дроїда жбурляє ще далі, навіть якщо він не в самому епіцентрі.",
                    _ => string.Empty
                },
                "weapon.teleport" => mechanicId switch
                {
                    "exchange-teleport" => "Телепорт обміну: міняється місцями з ворожим дроном біля точки прибуття.",
                    "tactical-exchange" => "Тактичний обмін: дозволяє мінятися також із союзним дроном, міною або зарядом.",
                    "impulse-teleport" => "Імпульсний телепорт: після прибуття слабко відштовхує поруч незакріплені об'єкти й дрони без шкоди.",
                    "spatial-shift" => "Просторовий зсув: значно збільшує радіус імпульсу після телепортації.",
                    _ => string.Empty
                },
                "weapon.bur-rocket" => mechanicId switch
                {
                    "long-range-drill-rocket" => "Ракета проходить далі крізь звичайний ґрунт і базові матеріали.",
                    "deep-range-drill-rocket" => "Ракета проходить значно далі крізь звичайний ґрунт і базові матеріали.",
                    "universal-drill-rocket" => "Ракета проходить крізь ґрунт, тонкий метал, бетонні плити й легкі сплави без збільшення дальності.",
                    "all-terrain-drill-rocket" => "Ракета також проходить крізь товстий метал, титан і спеціальні укріплення; дальність базова.",
                    _ => string.Empty
                },
                "weapon.mortira" => mechanicId switch
                {
                    "fragment-mortar" => "Після вибуху основного заряду вилітають чотири малих додаткових заряди.",
                    "assault-mortar" => "Після вибуху основного заряду вилітають п’ять малих додаткових зарядів.",
                    "cascade-mortar" => "У створену вирву скочується заряд із 50% шкоди та тим самим розміром вирви.",
                    "multi-cascade-mortar" => "Після другого вибуху ще менший заряд скочується глибше: 50%, потім 25% шкоди, з повною вирвою.",
                    _ => string.Empty
                },
                "weapon.emp" => mechanicId switch
                {
                    "overload" => "Збій систем: уражена ціль пропускає можливість вистрілити на свій наступний хід.",
                    "disruption" => "Пошкодження прицілу: уражена ціль кілька ходів завдає меншої шкоди своєю зброєю.",
                    "shield-break" => "Повний параліч: той самий збій додатково блокує й рух цілі на той хід.",
                    "system-noise" => "Системний колапс: послаблення шкоди сильніше і триває довше.",
                    _ => string.Empty
                },
                "weapon.freeze" => mechanicId switch
                {
                    "cryo" => "Кріо: заморожує ціль на довше, ніж базовий постріл.",
                    "slick" => "Повторна заморозка: кріо-постріл по вже замороженій цілі на наступний хід завдає бонусної шкоди.",
                    "hard-freeze" => "Глибока заморозка: блокування руху триває ще довше.",
                    "ice-patch" => "Каскадна заморозка: бонус шкоди за повторне влучання більший.",
                    _ => string.Empty
                },
                "weapon.fire-gun" => mechanicId switch
                {
                    "stream" => "Збільшені паливні баки: за один постріл випускається більше розпеченої лави.",
                    "ignite" => "Запалювання: ціль після влучання горить і отримує шкоду на початку своїх ходів.",
                    "long-stream" => "Тривале горіння: розлита лава горить значно довше.",
                    "wildfire" => "Термічний резонанс: повторне влучання по палаючій цілі завдає більше шкоди й оновлює горіння.",
                    _ => string.Empty
                },
                "weapon.toxic" => mechanicId switch
                {
                    "corrosion" => "Корозійний токсин: отрута додатково послаблює броню цілі на кілька ходів.",
                    "cloud" => "Рухомий токсин: отруєна ціль отримує шкоду на початку ходу; під час руху — додаткову шкоду щосекунди.",
                    "armor-corrosion" => "Концентрований корозійний токсин: послаблення броні сильніше і триває довше.",
                    "lingering-cloud" => "Агресивний токсин: шкода під час руху вища, а ефект отруєння триває на хід довше.",
                    _ => string.Empty
                },
                "weapon.missle" => mechanicId switch
                {
                    "ballistic-missile" => "Ракета набирає висоту, обирає ціль і розділяється на два менші некеровані заряди.",
                    "cluster-ballistic-missile" => "Ракета набирає висоту, обирає ціль і розділяється на три менші некеровані заряди.",
                    "maneuvering-missile" => "Самонаведення може облетіти одну суттєву перешкоду на шляху до цілі.",
                    "tactical-homing-missile" => "Самонаведення може прокласти складніший маршрут і обійти до двох перешкод.",
                    _ => string.Empty
                },
                "weapon.fpv-drone" => mechanicId switch
                {
                    "endurance" => "Витривалість: керований дрон довше тримає зв'язок.",
                    "agility" => "Ударний заряд: важча вибухівка — більший радіус і шкода вибуху дрона.",
                    "long-link" => "Далекий зв'язок: час ручного керування суттєво збільшується.",
                    "snap-turn" => "Направлений вибух: радіус і шкода вибуху ще більші.",
                    _ => string.Empty
                },
                _ => string.Empty
            };
        }
    }
}
