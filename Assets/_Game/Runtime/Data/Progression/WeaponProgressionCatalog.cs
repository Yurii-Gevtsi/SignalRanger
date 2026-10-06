using System;
using System.Collections.Generic;
using Game.Data.Save;

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

    /// <summary>
    /// The four player-facing upgrade choices. A choice is recorded for every
    /// purchased weapon rank, so the player can select a different parameter on
    /// every level-up instead of following a fixed branch.
    /// </summary>
    public enum WeaponUpgradePath
    {
        None = 0,
        Core = 1,
        Aim = 2,
        Loadout = 3,
        Radius = 4
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
            Rank = rank < 1 ? 1 : rank > 60 ? 60 : rank;
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
        public bool IsEvolution => (Rank == 5 || Rank == 10) && !string.IsNullOrWhiteSpace(MechanicId);
    }

    /// <summary>
    /// A paid, rank-specific weapon evolution. Unlike the legacy A/B branch this
    /// is an independent choice at each milestone, so rank 10 may replace rank 5.
    /// </summary>
    public readonly struct WeaponEvolutionOptionDefinition
    {
        public WeaponEvolutionOptionDefinition(WeaponUpgradeNodeDefinition node)
        {
            Rank = node.Rank;
            Id = node.MechanicId ?? string.Empty;
            LegacyBranch = node.Branch;
            EvolutionName = node.EvolutionName ?? string.Empty;
        }

        public int Rank { get; }
        public string Id { get; }
        public WeaponUpgradeBranch LegacyBranch { get; }
        public string EvolutionName { get; }
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
            WeaponUpgradeNodeDefinition[] nodes,
            int coreUpgradeLimit = 4,
            int aimUpgradeLimit = 4,
            int loadoutUpgradeLimit = 4,
            int radiusUpgradeLimit = 4,
            int special5PurchaseCount = 0,
            int special10PurchaseCount = 0)
        {
            WeaponId = weaponId ?? string.Empty;
            RequiredCommanderLevel = requiredCommanderLevel < 1 ? 1 : requiredCommanderLevel;
            CreditPrice = creditPrice < 0 ? 0 : creditPrice;
            PremiumPrice = premiumPrice < 0 ? 0 : premiumPrice;
            GrantedAtStart = grantedAtStart;
            BranchAId = branchAId ?? string.Empty;
            BranchBId = branchBId ?? string.Empty;
            _nodes = nodes ?? Array.Empty<WeaponUpgradeNodeDefinition>();
            CoreUpgradeLimit = Math.Max(0, coreUpgradeLimit);
            AimUpgradeLimit = Math.Max(0, aimUpgradeLimit);
            LoadoutUpgradeLimit = Math.Max(0, loadoutUpgradeLimit);
            RadiusUpgradeLimit = Math.Max(0, radiusUpgradeLimit);
            Special5PurchaseCount = Math.Max(0, special5PurchaseCount);
            Special10PurchaseCount = Math.Max(0, special10PurchaseCount);
        }

        public string WeaponId { get; }
        public int RequiredCommanderLevel { get; }
        public int CreditPrice { get; }
        public int PremiumPrice { get; }
        public bool GrantedAtStart { get; }
        public string BranchAId { get; }
        public string BranchBId { get; }
        public int CoreUpgradeLimit { get; }
        public int AimUpgradeLimit { get; }
        public int LoadoutUpgradeLimit { get; }
        public int RadiusUpgradeLimit { get; }
        public int Special5PurchaseCount { get; }
        public int Special10PurchaseCount { get; }
        public int TotalBaseUpgradeLimit => CoreUpgradeLimit + AimUpgradeLimit + LoadoutUpgradeLimit + RadiusUpgradeLimit;
        public int MaxRank => 1 + TotalBaseUpgradeLimit + Special5PurchaseCount + Special10PurchaseCount;
        public IReadOnlyList<WeaponUpgradeNodeDefinition> Nodes => _nodes;
        public bool HasDevelopment => TotalBaseUpgradeLimit > 0 || Special5PurchaseCount > 0 || Special10PurchaseCount > 0;

        public int GetSpecialPurchaseLimit(int milestoneRank)
        {
            return milestoneRank switch
            {
                5 => Special5PurchaseCount,
                10 => Special10PurchaseCount,
                _ => 0
            };
        }

        public int GetSpecialPurchaseCount(SaveWeaponProgressData progress, int milestoneRank)
        {
            if (progress == null || GetSpecialPurchaseLimit(milestoneRank) <= 0)
            {
                return 0;
            }

            if (progress.specialUpgrades != null)
            {
                for (var index = 0; index < progress.specialUpgrades.Count; index++)
                {
                    var upgrade = progress.specialUpgrades[index];
                    if (upgrade != null && upgrade.rank == milestoneRank)
                    {
                        return Math.Clamp(upgrade.purchaseCount, 0, GetSpecialPurchaseLimit(milestoneRank));
                    }
                }
            }

            // Saves created before repeatable special steps existed already retain the
            // selected option. Treat that one-time choice as the first paid step.
            return WeaponEvolutionSelectionResolver.TryGetChoice(this, progress, milestoneRank, out _)
                ? 1
                : 0;
        }

        public int GetBaseUpgradeLimit(WeaponUpgradePath path)
        {
            return path switch
            {
                WeaponUpgradePath.Core => CoreUpgradeLimit,
                WeaponUpgradePath.Aim => AimUpgradeLimit,
                WeaponUpgradePath.Loadout => LoadoutUpgradeLimit,
                WeaponUpgradePath.Radius => RadiusUpgradeLimit,
                _ => 0
            };
        }

        public bool CanPurchaseBaseUpgrade(SaveWeaponProgressData progress, WeaponUpgradePath path)
        {
            if (CountBaseUpgrades(progress, path) >= GetBaseUpgradeLimit(path)) return false;
            return WeaponId != "weapon.titanium-barrier" || path != WeaponUpgradePath.Radius
                || progress?.rank >= 5 && WeaponEvolutionSelectionResolver.TryGetChoice(this, progress, 5, out var option)
                    && option.Id == "size-unlock" && GetSpecialPurchaseCount(progress, 5) > 0;
        }

        public int CountBaseUpgrades(SaveWeaponProgressData progress, WeaponUpgradePath path,
            int throughRank = 60)
        {
            if (progress == null || !WeaponUpgradeEconomy.IsValidPath(path))
            {
                return 0;
            }

            var counts = new int[5];
            var availablePaths = new List<WeaponUpgradePath>(4);
            for (var index = 1; index <= 4; index++)
            {
                var availablePath = (WeaponUpgradePath)index;
                if (GetBaseUpgradeLimit(availablePath) > 0)
                {
                    availablePaths.Add(availablePath);
                }
            }
            var baseOrdinal = 0;
            var unrecordedSpecialRanks = progress.upgradeChoices?.Count > 0
                ? GetSpecialPurchaseCount(progress, 5) + GetSpecialPurchaseCount(progress, 10)
                : 0;
            for (var rank = 2; rank <= Math.Min(progress.rank, throughRank); rank++)
            {
                if (TryGetRecordedPath(progress, rank, out var recordedPath))
                {
                    counts[(int)recordedPath]++;
                    baseOrdinal++;
                    continue;
                }

                // Mortar paid-special steps occupy ranks without a TTH record.
                // Explicit modern saves must not receive inferred unpaid base upgrades.
                if (WeaponId == "weapon.mortira" && progress.upgradeChoices?.Count > 0)
                    continue;

                // This weapon reaches rank five with its four Loadout purchases.
                // Rank six is the paid size feature, not an unrecorded Radius step.
                if (WeaponId == "weapon.titanium-barrier" && rank == 6
                    && GetSpecialPurchaseCount(progress, 5) > 0)
                {
                    continue;
                }

                if (TryGetNode(rank, (WeaponUpgradeBranch)progress.branch, out var node))
                {
                    if (!node.IsEvolution)
                    {
                        var legacyPath = ResolveBasePath(node.Stat);
                        if (WeaponUpgradeEconomy.IsValidPath(legacyPath))
                        {
                            counts[(int)legacyPath]++;
                            baseOrdinal++;
                        }
                    }
                    continue;
                }

                if (rank == 5 && Special5PurchaseCount > 0
                    || rank == 10 && Special10PurchaseCount > 0)
                {
                    continue;
                }

                // Explicit saves record every base purchase, so an unrecorded rank is a paid
                // special step. It must not be mistaken for an unpaid base upgrade.
                if (unrecordedSpecialRanks > 0)
                {
                    unrecordedSpecialRanks--;
                    continue;
                }

                // Rank-only opponent loadouts and older saves have no recorded TTH
                // choices. Allocate their base ranks deterministically within the
                // same four caps so they receive real battle stats and cannot buy
                // past a full path after migration.
                for (var offset = 0; offset < availablePaths.Count; offset++)
                {
                    var candidate = availablePaths[(baseOrdinal + offset) % availablePaths.Count];
                    if (counts[(int)candidate] >= GetBaseUpgradeLimit(candidate))
                    {
                        continue;
                    }

                    counts[(int)candidate]++;
                    break;
                }
                baseOrdinal++;
            }

            return counts[(int)path];
        }

        private static bool TryGetRecordedPath(SaveWeaponProgressData progress, int rank,
            out WeaponUpgradePath path)
        {
            if (progress.upgradeChoices != null)
            {
                for (var index = 0; index < progress.upgradeChoices.Count; index++)
                {
                    var choice = progress.upgradeChoices[index];
                    if (choice != null && choice.rank == rank
                        && Enum.IsDefined(typeof(WeaponUpgradePath), choice.path)
                        && choice.path != (int)WeaponUpgradePath.None)
                    {
                        path = (WeaponUpgradePath)choice.path;
                        return true;
                    }
                }
            }

            path = WeaponUpgradePath.None;
            return false;
        }

        private static WeaponUpgradePath ResolveBasePath(WeaponUpgradeStat stat)
        {
            return stat switch
            {
                WeaponUpgradeStat.Damage => WeaponUpgradePath.Core,
                WeaponUpgradeStat.Accuracy => WeaponUpgradePath.Aim,
                WeaponUpgradeStat.Range => WeaponUpgradePath.Aim,
                WeaponUpgradeStat.Ammo => WeaponUpgradePath.Loadout,
                WeaponUpgradeStat.ProjectileCount => WeaponUpgradePath.Loadout,
                WeaponUpgradeStat.ImpactRadius => WeaponUpgradePath.Radius,
                _ => WeaponUpgradePath.None
            };
        }

        public bool HasEvolutionOptions
        {
            get
            {
                for (var index = 0; index < _nodes.Length; index++)
                {
                    if (_nodes[index].IsEvolution && !string.IsNullOrWhiteSpace(_nodes[index].MechanicId))
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public IReadOnlyList<WeaponEvolutionOptionDefinition> GetEvolutionOptions(int rank)
        {
            var options = new List<WeaponEvolutionOptionDefinition>();
            for (var index = 0; index < _nodes.Length; index++)
            {
                var node = _nodes[index];
                if (node.Rank == rank && node.IsEvolution && !string.IsNullOrWhiteSpace(node.MechanicId))
                {
                    options.Add(new WeaponEvolutionOptionDefinition(node));
                }
            }
            return options;
        }

        public bool TryGetEvolutionOption(int rank, string optionId, out WeaponEvolutionOptionDefinition option)
        {
            var options = GetEvolutionOptions(rank);
            for (var index = 0; index < options.Count; index++)
            {
                if (string.Equals(options[index].Id, optionId, StringComparison.Ordinal))
                {
                    option = options[index];
                    return true;
                }
            }
            option = default;
            return false;
        }

        public bool HasBranch(WeaponUpgradeBranch branch)
        {
            return branch switch
            {
                WeaponUpgradeBranch.A => !string.IsNullOrWhiteSpace(BranchAId),
                WeaponUpgradeBranch.B => !string.IsNullOrWhiteSpace(BranchBId),
                _ => false
            };
        }

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
        public WeaponUpgradeCost(int rank, int credits, int blueprints, int crystals = 0)
        {
            Rank = rank;
            Credits = credits < 0 ? 0 : credits;
            Blueprints = blueprints < 0 ? 0 : blueprints;
            Crystals = crystals < 0 ? 0 : crystals;
        }

        public int Rank { get; }
        public int Credits { get; }
        public int Blueprints { get; }
        public int Crystals { get; }
    }

    public static class WeaponUpgradeEconomy
    {
        private static readonly int[] CommanderLevelOffsets =
        {
            0, 1, 2, 3, 5, 7, 9, 12, 15
        };

        private static readonly WeaponUpgradeCost[] Costs =
        {
            new(2, 300, 10, 3),
            new(3, 500, 15, 5),
            new(4, 800, 25, 8),
            new(5, 1200, 40, 12),
            new(6, 1800, 60, 18),
            new(7, 2500, 80, 25),
            new(8, 3500, 110, 35),
            new(9, 5000, 150, 50),
            new(10, 7000, 220, 70)
        };

        public static IReadOnlyList<WeaponUpgradeCost> All => Costs;

        public static bool TryGetCost(int targetRank, out WeaponUpgradeCost cost)
        {
            if (targetRank < 2)
            {
                cost = default;
                return false;
            }

            for (var index = 0; index < Costs.Length; index++)
            {
                if (Costs[index].Rank == targetRank)
                {
                    cost = Costs[index];
                    return true;
                }
            }

            var plateau = Costs[Costs.Length - 1];
            cost = new WeaponUpgradeCost(targetRank, plateau.Credits, plateau.Blueprints, plateau.Crystals);
            return true;
        }

        /// <summary>
        /// Returns the cost for a specific TTH choice. Core and Loadout are the
        /// premium upgrade paths, Aim is the baseline, and Radius is the cheaper
        /// path. The base rank curve remains the single source of truth for pacing.
        /// </summary>
        public static bool TryGetCost(int targetRank, WeaponUpgradePath path, out WeaponUpgradeCost cost)
        {
            if (!TryGetCost(targetRank, out var baseCost) || !IsValidPath(path))
            {
                cost = default;
                return false;
            }

            cost = baseCost;
            return true;
        }

        public static bool TryGetSpecialCost(int targetRank, int milestoneRank, bool isInitialChoice,
            out WeaponUpgradeCost cost)
        {
            if (!TryGetCost(targetRank, out var baseCost))
            {
                cost = default;
                return false;
            }

            var multiplier = isInitialChoice
                ? milestoneRank == 10 ? 2.5f : 1.8f
                : 1f;
            cost = new WeaponUpgradeCost(targetRank,
                (int)Math.Ceiling(baseCost.Credits * multiplier),
                (int)Math.Ceiling(baseCost.Blueprints * multiplier),
                (int)Math.Ceiling(baseCost.Crystals * multiplier));
            return true;
        }

        public static bool IsValidPath(WeaponUpgradePath path)
        {
            return path >= WeaponUpgradePath.Core && path <= WeaponUpgradePath.Radius;
        }

        /// <summary>
        /// Sum of credits/blueprints spent reaching <paramref name="currentRank"/> from
        /// rank 1, used to compute a reset refund.
        /// </summary>
        public static (int Credits, int Blueprints) GetCumulativeCost(int currentRank)
        {
            var credits = 0;
            var blueprints = 0;
            for (var targetRank = 2; targetRank <= currentRank; targetRank++)
            {
                if (TryGetCost(targetRank, out var cost))
                {
                    credits += cost.Credits;
                    blueprints += cost.Blueprints;
                }
            }

            return (credits, blueprints);
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

        public static int GetFreeChoiceRequiredCommanderLevel(
            WeaponProgressionDefinition definition,
            int targetRank)
        {
            if (definition == null)
            {
                return 1;
            }

            return definition.RequiredCommanderLevel + Math.Max(0, targetRank - 1);
        }

        /// <summary>
        /// A weapon starts at rank one when it is unlocked. Every later commander
        /// level banks one more rank for that weapon, so a player may catch up a
        /// weapon that was left untouched without losing the earned opportunity.
        /// </summary>
        public static int GetMaxRankForCommanderLevel(WeaponProgressionDefinition definition, int commanderLevel)
        {
            if (definition == null || commanderLevel < definition.RequiredCommanderLevel)
            {
                return 1;
            }

            return Math.Min(definition.MaxRank, Math.Max(1, commanderLevel - definition.RequiredCommanderLevel + 1));
        }

        private static int Scale(int value, int numerator, int denominator)
        {
            return (value * numerator + denominator / 2) / denominator;
        }
    }

    public static class WeaponProgressionCatalog
    {
        private static readonly WeaponProgressionDefinition[] Definitions =
        {
            Catalog("weapon.bur", 1, 0, true, 0, 0, 0, 4),
            Catalog("weapon.impulse", 1, 0, true, 4, 4, 0, 4, 3, 0, "double-impulse"),
            Catalog("weapon.granata", 1, 0, true, 4, 4, 4, 4, 1, 1, "target-pull", string.Empty, "gravity-pull"),
            Catalog("weapon.rope", 2, 600, false, 0, 4, 0, 4),
            Catalog("weapon.plasma-sword", 3, 900, false, 4, 0, 0, 0),
            Catalog("weapon.ion-shotgun", 3, 900, false, 4, 0, 4, 4),
            Catalog("weapon.c4", 6, 1800, false, 4, 0, 2, 4),
            Catalog("weapon.mine", 6, 1800, false, 4, 0, 0, 4, 1, 1, "ally-safe", string.Empty, "ally-immune"),
            Catalog("weapon.heal", 6, 1800, false, 4, 0, 2, 0, 3, 0, "protective-repair", "regenerative-repair"),
            Catalog("weapon.titanium-disc", 9, 3000, false, 4, 4, 4, 4),
            Catalog("weapon.bur-rocket", 9, 3000, false, 4, 4, 4, 4),
            Catalog("weapon.titanium-barrier", 9, 3000, false, 0, 0, 4, 4, 1, 0, "size-unlock"),
            Catalog("weapon.mortira", 12, 5000, false, 4, 4, 2, 4, 5, 0, "cascade-mortar"),
            Catalog("weapon.sticky-grenade", 12, 5000, false, 4, 4, 2, 4, 4, 0, "toxic-release"),
            Catalog("weapon.teleport", 12, 5000, false, 0, 4, 4, 0, 1, 1, "instant-touch", string.Empty, "early-trigger"),
            Catalog("weapon.rail-rifle", 15, 7500, false, 4, 0, 4, 0, 4, 0, "terrain-pierce"),
            Catalog("weapon.cryo-grenade", 15, 7500, false, 4, 4, 2, 4),
            Catalog("weapon.hopping-grenade", 20, 11000, false, 4, 4, 4, 4),
            Catalog("weapon.incendiary-grenade", 20, 11000, false, 4, 4, 2, 4),
            Catalog("weapon.repair-jammer", 25, 16000, false, 0, 0, 2, 2),
            Catalog("weapon.missle", 30, 22000, false, 4, 4, 0, 4),
            Catalog("weapon.orbital-rocket", 36, 30000, false, 4, 0, 2, 4),
            Catalog("weapon.guardian-drone", 6, 0, false, 4, 0, 2, 4, 1, 0, "stealth"),
            Catalog("weapon.emp", 15, 0, false, 4, 4, 2, 4, 2, 2, "aim-jam", string.Empty, "movement-lock"),
            Catalog("weapon.turn-transfer", 15, 0, false, 0, 0, 1, 0),
            Catalog("weapon.electric-harpoon", 20, 0, false, 4, 0, 2, 4, 1, 1, "impact-pull", string.Empty, "double-grab"),
            Catalog("weapon.swap-jump", 25, 0, false, 0, 4, 0, 4),
            Catalog("weapon.grounder", 30, 0, false, 4, 0, 0, 4),
            Catalog("weapon.force-field", 42, 0, false, 0, 0, 0, 4),
            Catalog("weapon.boost", 6, 0, false, 0, 0, 4, 4),
            Catalog("weapon.sky-hook", 9, 0, false, 4, 0, 0, 0),
            Catalog("weapon.spider-mine", 12, 0, false, 4, 0, 2, 4, 4, 0, "element-choice"),
            Catalog("weapon.virus", 25, 0, false, 4, 0, 0, 0),
            Catalog("weapon.bomber", 30, 0, false, 4, 0, 0, 4),
            Catalog("weapon.ufo-carrier", 30, 0, false, 0, 0, 0, 4),
            Catalog("weapon.self-destruct", 36, 0, false, 4, 0, 0, 4),
            Catalog("weapon.nuclear-charge", 42, 0, false, 4, 0, 0, 4),
            Catalog("weapon.laser-fence", 12, 0, false, 4, 4, 4, 4),
            Catalog("weapon.freeze", 15, 0, false, 4, 4, 2, 4, 2, 3, "full-freeze", string.Empty, "cold-bonus"),
            Catalog("weapon.fire-gun", 20, 0, false, 4, 4, 2, 4, 2, 3, "movement-lock-burn", string.Empty, "burn-bonus"),
            Catalog("weapon.toxic", 25, 0, false, 4, 4, 2, 4, 2, 3, "movement-poison", string.Empty, "poison-bonus")
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

        private static WeaponProgressionDefinition Catalog(
            string weaponId,
            int requiredLevel,
            int creditPrice,
            bool grantedAtStart,
            int core,
            int aim,
            int loadout,
            int radius,
            int special5Steps = 0,
            int special10Steps = 0,
            string special5A = "",
            string special5B = "",
            string special10A = "",
            string special10B = "")
        {
            var nodes = new List<WeaponUpgradeNodeDefinition>();
            if (special5Steps > 0)
            {
                nodes.Add(Node(5, WeaponUpgradeBranch.A, WeaponUpgradeStat.None, 0, special5A));
                if (!string.IsNullOrWhiteSpace(special5B))
                {
                    nodes.Add(Node(5, WeaponUpgradeBranch.B, WeaponUpgradeStat.None, 0, special5B));
                }
            }

            if (special10Steps > 0)
            {
                nodes.Add(Node(10, WeaponUpgradeBranch.A, WeaponUpgradeStat.None, 0, special10A));
                if (!string.IsNullOrWhiteSpace(special10B))
                {
                    nodes.Add(Node(10, WeaponUpgradeBranch.B, WeaponUpgradeStat.None, 0, special10B));
                }
            }

            return new WeaponProgressionDefinition(
                weaponId,
                requiredLevel,
                creditPrice,
                0,
                grantedAtStart,
                special5A,
                special5B,
                nodes.ToArray(),
                core,
                aim,
                loadout,
                radius,
                special5Steps,
                special10Steps);
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
            string branchBCapstoneName = "",
            int coreUpgradeLimit = 4,
            int aimUpgradeLimit = 4,
            int loadoutUpgradeLimit = 4,
            int radiusUpgradeLimit = 4)
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
                },
                coreUpgradeLimit,
                aimUpgradeLimit,
                loadoutUpgradeLimit,
                radiusUpgradeLimit);
        }

        private static WeaponProgressionDefinition SingleTrack(
            string weaponId,
            WeaponUpgradeStat rank2,
            WeaponUpgradeStat rank3,
            WeaponUpgradeStat rank4)
        {
            return new WeaponProgressionDefinition(
                weaponId,
                1,
                0,
                0,
                true,
                string.Empty,
                string.Empty,
                new[]
                {
                    Node(2, WeaponUpgradeBranch.None, rank2, 1),
                    Node(3, WeaponUpgradeBranch.None, rank3, 1),
                    Node(4, WeaponUpgradeBranch.None, rank4, 1),
                    Node(5, WeaponUpgradeBranch.A, WeaponUpgradeStat.None, 0),
                    Node(6, WeaponUpgradeBranch.A, rank2, 1),
                    Node(7, WeaponUpgradeBranch.A, rank3, 1),
                    Node(8, WeaponUpgradeBranch.A, rank4, 1),
                    Node(9, WeaponUpgradeBranch.A, rank2, 1),
                    Node(10, WeaponUpgradeBranch.A, WeaponUpgradeStat.None, 0)
                },
                coreUpgradeLimit: 0,
                aimUpgradeLimit: 4,
                loadoutUpgradeLimit: 0,
                radiusUpgradeLimit: 4);
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
