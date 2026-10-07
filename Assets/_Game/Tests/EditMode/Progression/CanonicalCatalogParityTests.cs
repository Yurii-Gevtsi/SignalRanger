using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Game.Data.Progression;
using NUnit.Framework;
using UnityEngine;

namespace Game.Tests.EditMode.Progression
{
    /// <summary>
    /// End-to-end check of all 41 weapons: the runtime registry must match the canonical catalog table
    /// (unlock level, TTH limits, presence and step count of both features, shop price of core weapons).
    /// </summary>
    public sealed class CanonicalCatalogParityTests
    {
        private static readonly Dictionary<string, string> RuntimeIds = new()
        {
            ["drill_laser"] = "weapon.bur", ["impulse_cannon"] = "weapon.impulse", ["grenade"] = "weapon.granata",
            ["rope"] = "weapon.rope", ["plasma_sword"] = "weapon.plasma-sword", ["ion_shotgun"] = "weapon.ion-shotgun",
            ["dynamite"] = "weapon.c4", ["sensor_mine"] = "weapon.mine", ["repair_kit"] = "weapon.heal",
            ["titanium_disc"] = "weapon.titanium-disc", ["drill_rocket"] = "weapon.bur-rocket",
            ["titanium_barrier"] = "weapon.titanium-barrier", ["mortar"] = "weapon.mortira",
            ["sticky_grenade"] = "weapon.sticky-grenade", ["teleport_sphere"] = "weapon.teleport",
            ["rail_rifle"] = "weapon.rail-rifle", ["cryo_grenade"] = "weapon.cryo-grenade",
            ["hopping_grenade"] = "weapon.hopping-grenade", ["incendiary_grenade"] = "weapon.incendiary-grenade",
            ["repair_jammer"] = "weapon.repair-jammer", ["homing_rocket"] = "weapon.missle",
            ["orbital_rocket"] = "weapon.orbital-rocket", ["guardian_drone"] = "weapon.guardian-drone",
            ["emp_cannon"] = "weapon.emp", ["turn_transfer"] = "weapon.turn-transfer",
            ["electric_harpoon"] = "weapon.electric-harpoon", ["swap_jump"] = "weapon.swap-jump",
            ["grounder"] = "weapon.grounder", ["force_field"] = "weapon.force-field", ["boost"] = "weapon.boost",
            ["sky_hook"] = "weapon.sky-hook", ["spider_mine"] = "weapon.spider-mine", ["virus"] = "weapon.virus",
            ["bomber"] = "weapon.bomber", ["ufo_carrier"] = "weapon.ufo-carrier", ["self_destruct"] = "weapon.self-destruct",
            ["nuclear_charge"] = "weapon.nuclear-charge", ["laser_fence"] = "weapon.laser-fence",
            ["cryo_cannon"] = "weapon.freeze", ["lava_gun"] = "weapon.fire-gun", ["nanotoxic_charge"] = "weapon.toxic"
        };

        private static string ReadCatalog()
        {
            var path = Path.GetFullPath(Path.Combine(Application.dataPath, "..", "docs", "design", "weapon-catalog-canonical.md"));
            Assert.That(File.Exists(path), Is.True, path);
            return File.ReadAllText(path);
        }

        private static List<string[]> ReadRegistryRows(string catalog)
        {
            return catalog.Split('\n')
                .Where(line => Regex.IsMatch(line, @" \| (starter|coreShop|arenaShop|dungeonShop|bossReward) / "))
                .Select(line => line.Split('|').Skip(1).Select(cell => cell.Trim()).ToArray())
                .ToList();
        }

        /// <summary>The feature step counts the catalog's own economy check uses (special5, special10).</summary>
        private static Dictionary<string, int[]> ReadSpecialCounts(string catalog)
        {
            var block = Regex.Match(catalog, @"const specialCounts = \{(?<body>[\s\S]*?)\};").Groups["body"].Value;
            return Regex.Matches(block, @"(?<id>[a-z_]+):\[(?<a>\d+),(?<b>\d+)\]")
                .Cast<Match>()
                .ToDictionary(m => m.Groups["id"].Value,
                    m => new[] { int.Parse(m.Groups["a"].Value), int.Parse(m.Groups["b"].Value) });
        }

        [Test]
        public void EveryCatalogWeaponMatchesTheRuntimeRegistry()
        {
            var catalog = ReadCatalog();
            var rows = ReadRegistryRows(catalog);
            var specials = ReadSpecialCounts(catalog);
            Assert.That(rows, Has.Count.EqualTo(41));
            var problems = new List<string>();
            foreach (var row in rows)
            {
                var designId = row[0].Trim('`');
                if (!RuntimeIds.TryGetValue(designId, out var runtimeId))
                {
                    problems.Add($"{designId}: no runtime id mapping");
                    continue;
                }

                if (!WeaponProgressionCatalog.TryGet(runtimeId, out var definition))
                {
                    problems.Add($"{designId}: missing in the runtime registry ({runtimeId})");
                    continue;
                }

                var unlockLevel = int.Parse(row[2].Split('/')[1].Trim());
                if (definition.RequiredCommanderLevel != unlockLevel)
                    problems.Add($"{designId}: unlock {definition.RequiredCommanderLevel} != catalog {unlockLevel}");

                var tth = row[3].Split('/').Select(value => int.Parse(value.Trim())).ToArray();
                var runtimeTth = new[] { definition.CoreUpgradeLimit, definition.AimUpgradeLimit,
                    definition.LoadoutUpgradeLimit, definition.RadiusUpgradeLimit };
                if (!tth.SequenceEqual(runtimeTth))
                    problems.Add($"{designId}: TTH {string.Join("/", runtimeTth)} != catalog {row[3]}");

                var expected = specials.TryGetValue(designId, out var counts) ? counts : new[] { 0, 0 };
                if (definition.Special5PurchaseCount != expected[0] || definition.Special10PurchaseCount != expected[1])
                    problems.Add($"{designId}: feature steps {definition.Special5PurchaseCount}+{definition.Special10PurchaseCount}"
                        + $" != catalog {expected[0]}+{expected[1]}");

                var hasFirst = row[6] != "—";
                var hasSecond = row[7] != "—";
                if (hasFirst != definition.GetEvolutionOptions(5).Count > 0)
                    problems.Add($"{designId}: rank-5 feature presence differs from the catalog");
                if (hasSecond != definition.GetEvolutionOptions(10).Count > 0)
                    problems.Add($"{designId}: rank-10 feature presence differs from the catalog");
            }

            Assert.That(problems, Is.Empty, string.Join("\n", problems));
        }

        [Test]
        public void CoreShopPricesFollowTheUnlockLevelTable()
        {
            var prices = new Dictionary<int, int>
            {
                [2] = 600, [3] = 900, [6] = 1800, [9] = 3000, [12] = 5000,
                [15] = 7500, [20] = 11000, [25] = 16000, [30] = 22000, [36] = 30000
            };
            var problems = new List<string>();
            foreach (var row in ReadRegistryRows(ReadCatalog()).Where(r => r[2].StartsWith("coreShop", StringComparison.Ordinal)))
            {
                var designId = row[0].Trim('`');
                WeaponProgressionCatalog.TryGet(RuntimeIds[designId], out var definition);
                var unlockLevel = int.Parse(row[2].Split('/')[1].Trim());
                if (definition.CreditPrice != prices[unlockLevel])
                    problems.Add($"{designId}: price {definition.CreditPrice} != {prices[unlockLevel]}");
            }

            Assert.That(problems, Is.Empty, string.Join("\n", problems));
        }
    }
}
