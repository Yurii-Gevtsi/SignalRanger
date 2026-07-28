using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using UnityEngine;

namespace Game.Core.Localization
{
    /// <summary>
    /// Loads one structured JSON dictionary per locale from Resources/Localization.
    /// Keys are flattened internally, so JSON sections remain convenient for writers
    /// while game code uses stable dotted keys such as "battle.ammo".
    /// </summary>
    public sealed class JsonLocalizationService : ILocalizationService
    {
        public const string DefaultLanguageCode = "uk";

        private static readonly PreviewLanguage[] PreviewLanguages =
        {
            new("uk", "Українська"),
            new("en", "English"),
            new("pl", "Polski"),
            new("de", "Deutsch"),
            new("es", "Español"),
            new("fr", "Français"),
            new("it", "Italiano"),
            new("pt-BR", "Português (Brasil)"),
            new("ja", "Japanese"),
            new("ko", "Korean"),
            new("tr", "Türkçe"),
            new("ar", "Arabic"),
            new("zh-Hans", "Chinese (Simplified)")
        };

        private readonly Dictionary<string, Dictionary<string, string>> _translations = new(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<string, string> _languageNames = new(StringComparer.OrdinalIgnoreCase);
        private readonly List<string> _availableLanguageCodes = new();

        public JsonLocalizationService(string savedLanguageCode)
        {
            LoadAllDictionaries();
            LanguageCode = ResolveLanguageCode(savedLanguageCode);
        }

        public string LanguageCode { get; private set; }
        public IReadOnlyList<string> AvailableLanguageCodes => _availableLanguageCodes;
        public event Action LanguageChanged;

        public string Translate(string key, params LocalizationArgument[] arguments)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return string.Empty;
            }

            var value = FindTranslation(LanguageCode, key)
                ?? FindTranslation(DefaultLanguageCode, key)
                ?? $"[{key}]";

            if (arguments == null)
            {
                return value;
            }

            foreach (var argument in arguments)
            {
                if (string.IsNullOrWhiteSpace(argument.Name))
                {
                    continue;
                }

                var replacement = Convert.ToString(argument.Value, CultureInfo.InvariantCulture) ?? string.Empty;
                value = value.Replace("{" + argument.Name + "}", replacement, StringComparison.Ordinal);
            }

            return value;
        }

        public bool SetLanguage(string languageCode)
        {
            var resolved = ResolveLanguageCode(languageCode);
            if (string.Equals(LanguageCode, resolved, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            LanguageCode = resolved;
            LanguageChanged?.Invoke();
            return true;
        }

        public string GetLanguageDisplayName(string languageCode = null)
        {
            var code = ResolveLanguageCode(languageCode ?? LanguageCode);
            return _languageNames.TryGetValue(code, out var displayName) ? displayName : code;
        }

        private string ResolveLanguageCode(string requestedCode)
        {
            if (!string.IsNullOrWhiteSpace(requestedCode) && _translations.ContainsKey(requestedCode))
            {
                return requestedCode;
            }

            return _translations.ContainsKey(DefaultLanguageCode)
                ? DefaultLanguageCode
                : _translations.Count > 0 ? new List<string>(_translations.Keys)[0] : DefaultLanguageCode;
        }

        private string FindTranslation(string languageCode, string key)
        {
            return _translations.TryGetValue(languageCode, out var language)
                   && language.TryGetValue(key, out var translation)
                ? translation
                : null;
        }

        private void LoadAllDictionaries()
        {
            var dictionaries = Resources.LoadAll<TextAsset>("Localization");
            foreach (var dictionary in dictionaries)
            {
                if (dictionary == null || string.IsNullOrWhiteSpace(dictionary.name))
                {
                    continue;
                }

                try
                {
                    var parsed = StructuredJson.ParseObject(dictionary.text);
                    var flattened = new Dictionary<string, string>(StringComparer.Ordinal);
                    Flatten(parsed, string.Empty, flattened);
                    var languageCode = dictionary.name.Trim();
                    _translations[languageCode] = flattened;
                    _availableLanguageCodes.Add(languageCode);
                    if (flattened.TryGetValue("meta.languageName", out var displayName))
                    {
                        _languageNames[languageCode] = displayName;
                    }
                }
                catch (Exception exception)
                {
                    Debug.LogError($"[Localization] Cannot parse dictionary '{dictionary.name}': {exception.Message}");
                }
            }

            AddPreviewLanguages();

            if (_translations.Count == 0)
            {
                Debug.LogError("[Localization] No dictionaries found in Resources/Localization.");
            }
            else
            {
                _availableLanguageCodes.Sort(StringComparer.OrdinalIgnoreCase);
            }
        }

        private void AddPreviewLanguages()
        {
            foreach (var locale in PreviewLanguages)
            {
                if (_translations.ContainsKey(locale.Code))
                {
                    continue;
                }

                // Preview locales intentionally fall back to Ukrainian until a full
                // dictionary for their code is added to Resources/Localization.
                _translations.Add(locale.Code, new Dictionary<string, string>(StringComparer.Ordinal));
                _languageNames[locale.Code] = locale.DisplayName;
                _availableLanguageCodes.Add(locale.Code);
            }
        }

        private static void Flatten(Dictionary<string, object> source, string prefix, Dictionary<string, string> target)
        {
            foreach (var pair in source)
            {
                var key = string.IsNullOrEmpty(prefix) ? pair.Key : prefix + "." + pair.Key;
                if (pair.Value is Dictionary<string, object> nested)
                {
                    Flatten(nested, key, target);
                }
                else if (pair.Value is string value)
                {
                    target[key] = value;
                }
            }
        }

        private static class StructuredJson
        {
            public static Dictionary<string, object> ParseObject(string json)
            {
                var parser = new Parser(json);
                var result = parser.ReadObject();
                parser.RequireEnd();
                return result;
            }

            private sealed class Parser
            {
                private readonly string _source;
                private int _position;

                public Parser(string source) => _source = source ?? string.Empty;

                public Dictionary<string, object> ReadObject()
                {
                    SkipWhitespace();
                    Expect('{');
                    var result = new Dictionary<string, object>(StringComparer.Ordinal);
                    SkipWhitespace();
                    if (TryConsume('}')) return result;

                    while (true)
                    {
                        SkipWhitespace();
                        var key = ReadString();
                        SkipWhitespace();
                        Expect(':');
                        SkipWhitespace();
                        result[key] = Peek() == '{' ? ReadObject() : ReadString();
                        SkipWhitespace();
                        if (TryConsume('}')) return result;
                        Expect(',');
                    }
                }

                public void RequireEnd()
                {
                    SkipWhitespace();
                    if (_position != _source.Length) throw new FormatException("Unexpected data after the root object.");
                }

                private string ReadString()
                {
                    Expect('\"');
                    var builder = new StringBuilder();
                    while (_position < _source.Length)
                    {
                        var character = _source[_position++];
                        if (character == '\"') return builder.ToString();
                        if (character != '\\')
                        {
                            builder.Append(character);
                            continue;
                        }

                        if (_position >= _source.Length) throw new FormatException("Incomplete escape sequence.");
                        var escaped = _source[_position++];
                        builder.Append(escaped switch
                        {
                            '\"' => '\"', '\\' => '\\', '/' => '/', 'b' => '\b', 'f' => '\f', 'n' => '\n', 'r' => '\r', 't' => '\t',
                            'u' => ReadUnicodeCharacter(),
                            _ => throw new FormatException($"Unsupported escape sequence \\{escaped}.")
                        });
                    }

                    throw new FormatException("Unterminated string.");
                }

                private char ReadUnicodeCharacter()
                {
                    if (_position + 4 > _source.Length) throw new FormatException("Incomplete Unicode escape sequence.");
                    var hex = _source.Substring(_position, 4);
                    _position += 4;
                    return (char)Convert.ToInt32(hex, 16);
                }

                private char Peek() => _position < _source.Length ? _source[_position] : '\0';
                private bool TryConsume(char character)
                {
                    if (Peek() != character) return false;
                    _position++;
                    return true;
                }

                private void Expect(char character)
                {
                    if (!TryConsume(character)) throw new FormatException($"Expected '{character}' at position {_position}.");
                }

                private void SkipWhitespace()
                {
                    while (_position < _source.Length && char.IsWhiteSpace(_source[_position])) _position++;
                }
            }
        }

        private readonly struct PreviewLanguage
        {
            public PreviewLanguage(string code, string displayName)
            {
                Code = code;
                DisplayName = displayName;
            }

            public string Code { get; }
            public string DisplayName { get; }
        }
    }
}
