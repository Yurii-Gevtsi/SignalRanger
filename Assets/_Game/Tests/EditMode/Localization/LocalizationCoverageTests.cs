using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Game.App.Content;
using Game.Progression;
using NUnit.Framework;

namespace Game.Tests.EditMode.Localization
{
    /// <summary>
    /// Guards the project's hard rule: every string a player sees goes through the
    /// dictionaries, in every language. A literal in <c>GameText</c> is only ever a
    /// fallback for a key that does not exist yet - and a key that never gets its
    /// entry means every language quietly shows Ukrainian.
    /// <para>
    /// That failure is invisible in play: the fallback renders, nothing throws, and
    /// it only surfaces when a player of another language sees it. So it is checked
    /// here instead.
    /// </para>
    /// </summary>
    public sealed class LocalizationCoverageTests
    {
        private static readonly string[] Languages =
        {
            "uk", "en", "pl", "de", "es", "fr", "it", "pt-BR", "ja", "ko", "tr", "ar", "zh-Hans"
        };

        private const string BaseLanguage = "uk";

        private static readonly string[] CampaignSectorAndPlanetKeys =
        {
            "campaign.sector.1", "campaign.sector.2", "campaign.sector.3", "campaign.sector.4",
            "campaign.planet.kerion", "campaign.planet.valtara", "campaign.planet.gale-foundry",
            "campaign.planet.cryo-relay", "campaign.planet.hailis", "campaign.planet.ferrum",
            "campaign.planet.pyra", "campaign.planet.noctara", "campaign.planet.selene",
            "campaign.planet.pelagos", "campaign.planet.cinder", "campaign.planet.asteron",
            "campaign.planet.viridia", "campaign.planet.dione", "campaign.planet.jovaris"
        };

        [Test]
        public void EveryGameTextKeyExistsInEveryDictionary()
        {
            var keys = ReadGameTextKeys();
            Assert.That(keys, Is.Not.Empty, "No GameText keys were found - has the file moved?");

            var failures = new List<string>();
            foreach (var language in Languages)
            {
                var entries = ReadDictionary(language);
                var missing = keys.Where(key => !entries.ContainsKey(key)).ToArray();
                if (missing.Length > 0)
                {
                    failures.Add($"{language}: {string.Join(", ", missing)}");
                }
            }

            Assert.That(failures, Is.Empty,
                "GameText keys without a dictionary entry - add them to every language:"
                + Environment.NewLine + string.Join(Environment.NewLine, failures));
        }

        [Test]
        public void EveryLanguageCarriesTheSameKeysAsTheBaseDictionary()
        {
            var baseEntries = ReadDictionary(BaseLanguage);

            var failures = new List<string>();
            foreach (var language in Languages)
            {
                if (language == BaseLanguage)
                {
                    continue;
                }

                var entries = ReadDictionary(language);
                var missing = baseEntries.Keys.Where(key => !entries.ContainsKey(key)).ToArray();
                var unexpected = entries.Keys.Where(key => !baseEntries.ContainsKey(key)).ToArray();
                if (missing.Length > 0 || unexpected.Length > 0)
                {
                    failures.Add($"{language} missing {missing.Length}: {string.Join(", ", missing.Take(10))}; "
                        + $"unexpected {unexpected.Length}: {string.Join(", ", unexpected.Take(10))}");
                }
            }

            Assert.That(failures, Is.Empty,
                "Languages behind the base dictionary:" + Environment.NewLine
                + string.Join(Environment.NewLine, failures));
        }

        [Test]
        public void NoDictionaryEntryIsLeftEmpty()
        {
            var failures = new List<string>();
            foreach (var language in Languages)
            {
                var blank = ReadDictionary(language)
                    .Where(pair => string.IsNullOrWhiteSpace(pair.Value))
                    .Select(pair => pair.Key)
                    .ToArray();
                if (blank.Length > 0)
                {
                    failures.Add($"{language}: {string.Join(", ", blank)}");
                }
            }

            Assert.That(failures, Is.Empty,
                "Empty translations render as nothing at all:" + Environment.NewLine
                + string.Join(Environment.NewLine, failures));
        }

        [Test]
        public void CampaignSectorAndPlanetNamesExistInEveryDictionary()
        {
            var failures = new List<string>();
            foreach (var language in Languages)
            {
                var entries = ReadDictionary(language);
                var missingOrBlank = CampaignSectorAndPlanetKeys
                    .Where(key => !entries.TryGetValue(key, out var value) || string.IsNullOrWhiteSpace(value))
                    .ToArray();
                if (missingOrBlank.Length > 0)
                {
                    failures.Add($"{language}: {string.Join(", ", missingOrBlank)}");
                }
            }

            Assert.That(failures, Is.Empty,
                "Campaign sector and planet names must be localized in every dictionary:"
                + Environment.NewLine + string.Join(Environment.NewLine, failures));
        }

        [Test]
        public void UkrainianDictionaryUsesStrictUtf8WithoutLossMarkers()
        {
            var path = Path.Combine(
                ResolveAssetsRoot(), "_Game", "Content", "Resources", "Localization", BaseLanguage + ".json");
            var text = File.ReadAllText(path, new UTF8Encoding(false, true));

            Assert.That(text.Contains("?"), Is.False, "A question mark in Ukrainian text signals a lost character.");
            Assert.That(text.Contains("\uFFFD"), Is.False, "The Unicode replacement character signals invalid UTF-8.");
        }

        /// <summary>
        /// Keys that are built at runtime from content ids, which the literal scan
        /// above cannot see. These are exactly where the rule slipped before: a quest
        /// or mission added to a catalog silently rendered its Ukrainian fallback in
        /// every language, because nothing checked that its entry existed.
        /// </summary>
        [Test]
        public void EveryQuestAndMissionHasItsTextInEveryDictionary()
        {
            var expected = new List<string>();
            foreach (var quest in ActivityQuestCatalog.AllQuests)
            {
                expected.Add("rewardsScreen.quest." + quest.Id);
            }

            foreach (var mission in DefaultGameContentCatalog.Create().Missions)
            {
                if (mission == null)
                {
                    continue;
                }

                expected.Add("missionText." + mission.MissionId + ".title");
                expected.Add("missionText." + mission.MissionId + ".description");
            }

            var failures = new List<string>();
            foreach (var language in Languages)
            {
                var entries = ReadDictionary(language);
                var missing = expected.Where(key => !entries.ContainsKey(key)).ToArray();
                if (missing.Length > 0)
                {
                    failures.Add($"{language}: {string.Join(", ", missing)}");
                }
            }

            Assert.That(failures, Is.Empty,
                "Content text without a dictionary entry:" + Environment.NewLine
                + string.Join(Environment.NewLine, failures));
        }

        /// <summary>
        /// Keys named by a literal in a <c>T("key", ...)</c> call. Keys composed at
        /// runtime (<c>"content.weapon." + weaponId</c>) end in a dot and are skipped:
        /// their entries are per-content and covered by the content catalog tests.
        /// </summary>
        private static string[] ReadGameTextKeys()
        {
            var path = Path.Combine(ResolveAssetsRoot(), "_Game", "Runtime", "Core", "Localization", "GameText.cs");
            if (!File.Exists(path))
            {
                Assert.Ignore($"GameText.cs not found at {path}.");
            }

            return Regex.Matches(File.ReadAllText(path), "T\\(\\s*\"([^\"]+)\"")
                .Select(match => match.Groups[1].Value)
                .Where(key => !key.EndsWith(".", StringComparison.Ordinal))
                .Distinct()
                .OrderBy(key => key, StringComparer.Ordinal)
                .ToArray();
        }

        /// <summary>
        /// The project's Assets folder, found by walking up from the working
        /// directory. Deliberately not <c>Application.dataPath</c>: that needs the
        /// Unity runtime, and this check is worth running from a plain test host too.
        /// </summary>
        private static string ResolveAssetsRoot()
        {
            var directory = new DirectoryInfo(Directory.GetCurrentDirectory());
            while (directory != null)
            {
                var candidate = Path.Combine(directory.FullName, "Assets");
                if (Directory.Exists(Path.Combine(candidate, "_Game", "Content", "Resources", "Localization")))
                {
                    return candidate;
                }

                directory = directory.Parent;
            }

            Assert.Ignore("Project Assets folder not found from the working directory.");
            return string.Empty;
        }

        private static Dictionary<string, string> ReadDictionary(string language)
        {
            var path = Path.Combine(
                ResolveAssetsRoot(), "_Game", "Content", "Resources", "Localization", language + ".json");
            if (!File.Exists(path))
            {
                Assert.Ignore($"Dictionary not found at {path}.");
            }

            var entries = new Dictionary<string, string>(StringComparer.Ordinal);
            Flatten(SimpleJson.Parse(File.ReadAllText(path)), string.Empty, entries);
            return entries;
        }

        private static void Flatten(object node, string prefix, IDictionary<string, string> entries)
        {
            if (node is Dictionary<string, object> map)
            {
                foreach (var pair in map)
                {
                    Flatten(pair.Value, prefix.Length == 0 ? pair.Key : prefix + "." + pair.Key, entries);
                }

                return;
            }

            entries[prefix] = node as string ?? string.Empty;
        }

        /// <summary>
        /// Minimal object/string reader. Unity's JsonUtility cannot read a tree of
        /// unknown shape, and the dictionaries are exactly that.
        /// </summary>
        private static class SimpleJson
        {
            public static object Parse(string text)
            {
                var index = 0;
                return ReadValue(text, ref index);
            }

            private static object ReadValue(string text, ref int index)
            {
                SkipWhitespace(text, ref index);
                return text[index] switch
                {
                    '{' => ReadObject(text, ref index),
                    '"' => ReadString(text, ref index),
                    '[' => ReadArray(text, ref index),
                    _ => ReadLiteral(text, ref index)
                };
            }

            private static Dictionary<string, object> ReadObject(string text, ref int index)
            {
                var map = new Dictionary<string, object>(StringComparer.Ordinal);
                index++; // '{'
                SkipWhitespace(text, ref index);
                if (text[index] == '}')
                {
                    index++;
                    return map;
                }

                while (index < text.Length)
                {
                    SkipWhitespace(text, ref index);
                    var key = ReadString(text, ref index);
                    SkipWhitespace(text, ref index);
                    index++; // ':'
                    map[key] = ReadValue(text, ref index);
                    SkipWhitespace(text, ref index);
                    if (text[index] == ',')
                    {
                        index++;
                        continue;
                    }

                    index++; // '}'
                    break;
                }

                return map;
            }

            private static List<object> ReadArray(string text, ref int index)
            {
                var items = new List<object>();
                index++; // '['
                SkipWhitespace(text, ref index);
                if (text[index] == ']')
                {
                    index++;
                    return items;
                }

                while (index < text.Length)
                {
                    items.Add(ReadValue(text, ref index));
                    SkipWhitespace(text, ref index);
                    if (text[index] == ',')
                    {
                        index++;
                        continue;
                    }

                    index++; // ']'
                    break;
                }

                return items;
            }

            private static string ReadString(string text, ref int index)
            {
                index++; // opening quote
                var builder = new System.Text.StringBuilder();
                while (text[index] != '"')
                {
                    if (text[index] == '\\')
                    {
                        index++;
                        builder.Append(text[index] switch
                        {
                            'n' => '\n',
                            't' => '\t',
                            'r' => '\r',
                            'u' => (char)Convert.ToInt32(text.Substring(index + 1, 4), 16),
                            var other => other
                        });
                        index += text[index] == 'u' ? 5 : 1;
                        continue;
                    }

                    builder.Append(text[index]);
                    index++;
                }

                index++; // closing quote
                return builder.ToString();
            }

            private static object ReadLiteral(string text, ref int index)
            {
                var start = index;
                while (index < text.Length && text[index] != ',' && text[index] != '}' && text[index] != ']')
                {
                    index++;
                }

                return text.Substring(start, index - start).Trim();
            }

            private static void SkipWhitespace(string text, ref int index)
            {
                while (index < text.Length && char.IsWhiteSpace(text[index]))
                {
                    index++;
                }
            }
        }
    }
}
