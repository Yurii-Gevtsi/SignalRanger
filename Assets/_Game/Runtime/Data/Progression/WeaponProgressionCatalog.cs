using System;
using System.Collections.Generic;

namespace Game.Data.Progression
{
    public enum WeaponUpgradeBranch
    {
        None = 0,
        A = 1,
        B = 2
    }

    public enum WeaponUpgradeStat
    {
        None = 0,
        Damage,
        ImpactRadius,
        Range,
        Ammo,
        ProjectileCount,
        Duration,
        Knockback,
        TerrainDamage,
        Control,
        Healing,
        Fuse,
        Accuracy,
        Homing,
        Penetration,
        Mobility,
        Charge
    }

    public readonly struct WeaponUpgradeNodeDefinition
    {
        public WeaponUpgradeNodeDefinition(
            int rank,
            WeaponUpgradeBranch branch,
            WeaponUpgradeStat stat,
            int amount,
            string mechanicId = "",
            string evolutionName = "")
        {
            Rank = rank < 1 ? 1 : rank > 10 ? 10 : rank;
            Branch = branch;
            Stat = stat;
            Amount = amount;
            MechanicId = mechanicId ?? string.Empty;
            EvolutionName = evolutionName ?? string.Empty;
        }

        public int Rank { get; }
        public WeaponUpgradeBranch Branch { get; }
        public WeaponUpgradeStat Stat { get; }
        public int Amount { get; }
        public string MechanicId { get; }
        public string EvolutionName { get; }
        public bool IsEvolution => Rank == 5 || Rank == 10;
    }

    public sealed class WeaponProgressionDefinition
    {
        private readonly WeaponUpgradeNodeDefinition[] _nodes;

        public WeaponProgressionDefinition(
            string weaponId,
            int requiredCommanderLevel,
            int creditPrice,
            int premiumPrice,
            bool grantedAtStart,
            string branchAId,
            string branchBId,
            WeaponUpgradeNodeDefinition[] nodes)
        {
            WeaponId = weaponId ?? string.Empty;
            RequiredCommanderLevel = requiredCommanderLevel < 1 ? 1 : requiredCommanderLevel;
            CreditPrice = creditPrice < 0 ? 0 : creditPrice;
            PremiumPrice = premiumPrice < 0 ? 0 : premiumPrice;
            GrantedAtStart = grantedAtStart;
            BranchAId = branchAId ?? string.Empty;
            BranchBId = branchBId ?? string.Empty;
            _nodes = nodes ?? Array.Empty<WeaponUpgradeNodeDefinition>();
        }

        public string WeaponId { get; }
        public int RequiredCommanderLevel { get; }
        public int CreditPrice { get; }
        public int PremiumPrice { get; }
        public bool GrantedAtStart { get; }
        public string BranchAId { get; }
        public string BranchBId { get; }
        public IReadOnlyList<WeaponUpgradeNodeDefinition> Nodes => _nodes;

        public bool TryGetNode(int rank, WeaponUpgradeBranch branch, out WeaponUpgradeNodeDefinition node)
        {
            for (var index = 0; index < _nodes.Length; index++)
            {
                var candidate = _nodes[index];
                if (candidate.Rank != rank)
                {
                    continue;
                }

                if (candidate.Branch == WeaponUpgradeBranch.None || candidate.Branch == branch)
                {
                    node = candidate;
                    return true;
                }
            }

            node = default;
            return false;
        }
    }

    public readonly struct WeaponUpgradeCost
    {
        public WeaponUpgradeCost(int rank, int credits, int blueprints)
        {
            Rank = rank;
            Credits = credits < 0 ? 0 : credits;
            Blueprints = blueprints < 0 ? 0 : blueprints;
        }

        public int Rank { get; }
        public int Credits { get; }
        public int Blueprints { get; }
    }

    public static class WeaponUpgradeEconomy
    {
        private static readonly int[] CommanderLevelOffsets =
        {
            0,  // Rank 2: learn the weapon's first refinement at its unlock level.
            1,  // Rank 3
            2,  // Rank 4
            3,  // Rank 5: choose an evolution.
            5,  // Rank 6
            7,  // Rank 7
            9,  // Rank 8
            12, // Rank 9
            15  // Rank 10: capstone evolution.
        };

        private static readonly WeaponUpgradeCost[] Costs =
        {
            new(2, 300, 10),
            new(3, 500, 15),
            new(4, 800, 25),
            new(5, 1200, 40),
            new(6, 1800, 60),
            new(7, 2500, 80),
            new(8, 3500, 110),
            new(9, 5000, 150),
            new(10, 7000, 220)
        };

        public static IReadOnlyList<WeaponUpgradeCost> All => Costs;

        public static bool TryGetCost(int targetRank, out WeaponUpgradeCost cost)
        {
            for (var index = 0; index < Costs.Length; index++)
            {
                if (Costs[index].Rank == targetRank)
                {
                    cost = Costs[index];
                    return true;
                }
            }

            cost = default;
            return false;
        }

        public static int GetRequiredCommanderLevel(WeaponProgressionDefinition definition, int targetRank)
        {
            if (definition == null)
            {
                return 1;
            }

            var offsetIndex = targetRank - 2;
            if (offsetIndex < 0 || offsetIndex >= CommanderLevelOffsets.Length)
            {
                return definition.RequiredCommanderLevel;
            }

            return definition.RequiredCommanderLevel + CommanderLevelOffsets[offsetIndex];
        }
    }

    public static class WeaponProgressionCatalog
    {
        private static readonly WeaponProgressionDefinition[] Definitions =
        {
            Track("weapon.impulse", 1, 0, 0, true, "double-impulse", "impact-impulse",
                WeaponUpgradeStat.Damage, WeaponUpgradeStat.ImpactRadius, WeaponUpgradeStat.Range,
                WeaponUpgradeStat.Damage, WeaponUpgradeStat.Knockback, "triple-impulse", "assault-impulse",
                "Гармата подвійного імпульсу", "Ударна імпульсна гармата",
                "Гармата потрійного імпульсу", "Штурмова імпульсна гармата"),
            Track("weapon.blaster-caset", 1, 0, 0, true, "quad-blaster", "ricochet-blaster",
                WeaponUpgradeStat.Accuracy, WeaponUpgradeStat.Damage, WeaponUpgradeStat.Range,
                WeaponUpgradeStat.Damage, WeaponUpgradeStat.ProjectileCount, "five-shot-blaster", "double-ricochet-blaster",
                "Касетний бластер чотирьох зарядів", "Рикошетний касетний бластер",
                "Касетний бластер п’яти зарядів", "Касетний бластер подвійного рикошету"),
            Track("weapon.bur", 2, 800, 16, false, "long-range-drill", "universal-drill",
                WeaponUpgradeStat.Duration, WeaponUpgradeStat.TerrainDamage, WeaponUpgradeStat.Damage,
                WeaponUpgradeStat.Damage, WeaponUpgradeStat.Range, "deep-range-drill", "pioneer-drill",
                "Далекобійний буровий лазер", "Універсальний буровий лазер",
                "Глибокобійний буровий лазер", "Прохідницький буровий лазер"),
            Track("weapon.granata", 3, 1000, 20, false, "sticky-grenade", "gravity-grenade",
                WeaponUpgradeStat.Damage, WeaponUpgradeStat.Fuse, WeaponUpgradeStat.ImpactRadius,
                WeaponUpgradeStat.Homing, WeaponUpgradeStat.Range, "magnetic-sticky-grenade", "collapse-grenade",
                "Липуча граната", "Гравітаційна граната",
                "Магнітна липуча граната", "Колапс-граната"),
            Track("weapon.mine", 4, 1500, 24, false, "four-mines", "recognition-mine",
                WeaponUpgradeStat.Damage, WeaponUpgradeStat.Control, WeaponUpgradeStat.ImpactRadius,
                WeaponUpgradeStat.ProjectileCount, WeaponUpgradeStat.Control, "five-mines", "friendly-mine",
                "4 міни за хід", "Розпізнавальна міна",
                "5 мін за хід", "Дружня міна"),
            Track("weapon.c4", 5, 1800, 28, false, "enhanced-explosive-charge", "impulse-explosive-charge",
                WeaponUpgradeStat.Damage, WeaponUpgradeStat.Fuse, WeaponUpgradeStat.ImpactRadius,
                WeaponUpgradeStat.TerrainDamage, WeaponUpgradeStat.Knockback, "breaching-explosive-charge", "shockwave-charge",
                "Посилений вибуховий заряд", "Імпульсний вибуховий заряд",
                "Проломний вибуховий заряд", "Ударно-хвильовий заряд"),
            Track("weapon.heal", 6, 2000, 32, false, "protective-repair", "regenerative-repair",
                WeaponUpgradeStat.Healing, WeaponUpgradeStat.Range, WeaponUpgradeStat.Charge,
                WeaponUpgradeStat.Duration, WeaponUpgradeStat.Duration, "enhanced-protective-repair", "long-regenerative-repair",
                "Захисний ремонт", "Відновлювальний ремонт",
                "Посилений захисний ремонт", "Тривалий відновлювальний ремонт"),
            Track("weapon.teleport", 7, 3000, 36, false, "exchange-teleport", "impulse-teleport",
                WeaponUpgradeStat.Control, WeaponUpgradeStat.ImpactRadius, WeaponUpgradeStat.Charge,
                WeaponUpgradeStat.Control, WeaponUpgradeStat.ImpactRadius, "tactical-exchange", "spatial-shift",
                "Телепорт обміну", "Імпульсний телепорт",
                "Тактичний обмін", "Просторовий зсув"),
            Track("weapon.bur-rocket", 8, 3500, 40, false, "long-range-drill-rocket", "universal-drill-rocket",
                WeaponUpgradeStat.Damage, WeaponUpgradeStat.Penetration, WeaponUpgradeStat.Range,
                WeaponUpgradeStat.Penetration, WeaponUpgradeStat.TerrainDamage, "deep-range-drill-rocket", "all-terrain-drill-rocket",
                "Далекобійна бурова ракета", "Універсальна бурова ракета",
                "Глибокобійна бурова ракета", "Всепрохідна бурова ракета"),
            Track("weapon.mortira", 9, 4000, 44, false, "fragment-mortar", "cascade-mortar",
                WeaponUpgradeStat.Damage, WeaponUpgradeStat.Range, WeaponUpgradeStat.ImpactRadius,
                WeaponUpgradeStat.ProjectileCount, WeaponUpgradeStat.Penetration, "assault-mortar", "multi-cascade-mortar",
                "Осколкова мортира", "Каскадна мортира",
                "Штурмова мортира", "Багатокаскадна мортира"),
            Track("weapon.emp", 10, 4000, 48, false, "overload", "disruption",
                WeaponUpgradeStat.Damage, WeaponUpgradeStat.Range, WeaponUpgradeStat.Control,
                WeaponUpgradeStat.Damage, WeaponUpgradeStat.Duration, "shield-break", "system-noise"),
            Track("weapon.freeze", 11, 4000, 52, false, "cryo", "slick",
                WeaponUpgradeStat.Damage, WeaponUpgradeStat.ImpactRadius, WeaponUpgradeStat.Duration,
                WeaponUpgradeStat.Control, WeaponUpgradeStat.ImpactRadius, "hard-freeze", "ice-patch"),
            Track("weapon.fire-gun", 12, 4500, 56, false, "stream", "pool",
                WeaponUpgradeStat.Damage, WeaponUpgradeStat.Duration, WeaponUpgradeStat.Range,
                WeaponUpgradeStat.Duration, WeaponUpgradeStat.TerrainDamage, "long-stream", "burning-ground"),
            Track("weapon.toxic", 13, 4500, 60, false, "corrosion", "cloud",
                WeaponUpgradeStat.Damage, WeaponUpgradeStat.Duration, WeaponUpgradeStat.Range,
                WeaponUpgradeStat.Damage, WeaponUpgradeStat.ImpactRadius, "armor-corrosion", "lingering-cloud"),
            Track("weapon.missle", 14, 5000, 64, false, "ballistic-missile", "maneuvering-missile",
                WeaponUpgradeStat.Homing, WeaponUpgradeStat.Damage, WeaponUpgradeStat.Range,
                WeaponUpgradeStat.Homing, WeaponUpgradeStat.Knockback, "cluster-ballistic-missile", "tactical-homing-missile",
                "Балістична ракета", "Маневрова самонавідна ракета",
                "Касетна балістична ракета", "Тактична самонавідна ракета"),
            Track("weapon.fpv-drone", 15, 7000, 68, false, "endurance", "agility",
                WeaponUpgradeStat.Control, WeaponUpgradeStat.Damage, WeaponUpgradeStat.Range,
                WeaponUpgradeStat.Duration, WeaponUpgradeStat.Mobility, "long-link", "snap-turn")
        };

        private static readonly Dictionary<string, WeaponProgressionDefinition> ById = BuildIndex();

        public static IReadOnlyList<WeaponProgressionDefinition> All => Definitions;

        public static bool TryGet(string weaponId, out WeaponProgressionDefinition definition)
        {
            return ById.TryGetValue(weaponId ?? string.Empty, out definition);
        }

        private static Dictionary<string, WeaponProgressionDefinition> BuildIndex()
        {
            var index = new Dictionary<string, WeaponProgressionDefinition>(StringComparer.Ordinal);
            for (var definitionIndex = 0; definitionIndex < Definitions.Length; definitionIndex++)
            {
                var definition = Definitions[definitionIndex];
                index[definition.WeaponId] = definition;
            }

            return index;
        }

        private static WeaponProgressionDefinition Track(
            string weaponId,
            int requiredLevel,
            int creditPrice,
            int premiumPrice,
            bool grantedAtStart,
            string branchAId,
            string branchBId,
            WeaponUpgradeStat rank2,
            WeaponUpgradeStat rank3,
            WeaponUpgradeStat rank4,
            WeaponUpgradeStat branchAPrimary,
            WeaponUpgradeStat branchBPrimary,
            string branchACapstone,
            string branchBCapstone,
            string branchAEvolutionName = "",
            string branchBEvolutionName = "",
            string branchACapstoneName = "",
            string branchBCapstoneName = "")
        {
            return new WeaponProgressionDefinition(
                weaponId,
                requiredLevel,
                creditPrice,
                premiumPrice,
                grantedAtStart,
                branchAId,
                branchBId,
                new[]
                {
                    Node(2, WeaponUpgradeBranch.None, rank2, 2),
                    Node(3, WeaponUpgradeBranch.None, rank3, 2),
                    Node(4, WeaponUpgradeBranch.None, rank4, 3),
                    Node(5, WeaponUpgradeBranch.A, WeaponUpgradeStat.None, 0, branchAId, branchAEvolutionName),
                    Node(5, WeaponUpgradeBranch.B, WeaponUpgradeStat.None, 0, branchBId, branchBEvolutionName),
                    Node(6, WeaponUpgradeBranch.A, branchAPrimary, 2),
                    Node(7, WeaponUpgradeBranch.A, branchAPrimary, 3),
                    Node(8, WeaponUpgradeBranch.A, CompensationFor(branchAPrimary), 4),
                    Node(9, WeaponUpgradeBranch.A, branchAPrimary, 3),
                    Node(10, WeaponUpgradeBranch.A, WeaponUpgradeStat.None, 0, branchACapstone, branchACapstoneName),
                    Node(6, WeaponUpgradeBranch.B, branchBPrimary, 2),
                    Node(7, WeaponUpgradeBranch.B, branchBPrimary, 3),
                    Node(8, WeaponUpgradeBranch.B, CompensationFor(branchBPrimary), 4),
                    Node(9, WeaponUpgradeBranch.B, branchBPrimary, 3),
                    Node(10, WeaponUpgradeBranch.B, WeaponUpgradeStat.None, 0, branchBCapstone, branchBCapstoneName)
                });
        }

        private static WeaponUpgradeNodeDefinition Node(
            int rank,
            WeaponUpgradeBranch branch,
            WeaponUpgradeStat stat,
            int amount,
            string mechanicId = "",
            string evolutionName = "")
        {
            return new WeaponUpgradeNodeDefinition(rank, branch, stat, amount, mechanicId, evolutionName);
        }

        private static WeaponUpgradeStat CompensationFor(WeaponUpgradeStat primary)
        {
            return primary switch
            {
                WeaponUpgradeStat.Damage => WeaponUpgradeStat.Accuracy,
                WeaponUpgradeStat.ProjectileCount => WeaponUpgradeStat.Damage,
                WeaponUpgradeStat.Duration => WeaponUpgradeStat.Control,
                WeaponUpgradeStat.Range => WeaponUpgradeStat.Accuracy,
                WeaponUpgradeStat.Penetration => WeaponUpgradeStat.Damage,
                WeaponUpgradeStat.Knockback => WeaponUpgradeStat.ImpactRadius,
                WeaponUpgradeStat.TerrainDamage => WeaponUpgradeStat.Damage,
                WeaponUpgradeStat.Homing => WeaponUpgradeStat.Damage,
                WeaponUpgradeStat.Mobility => WeaponUpgradeStat.Control,
                WeaponUpgradeStat.Control => WeaponUpgradeStat.Duration,
                WeaponUpgradeStat.Healing => WeaponUpgradeStat.Charge,
                _ => WeaponUpgradeStat.Range
            };
        }
    }
}
