using System;
using System.Collections.Generic;

namespace Game.Core.Localization
{
    public interface ILocalizationService
    {
        string LanguageCode { get; }
        IReadOnlyList<string> AvailableLanguageCodes { get; }
        event Action LanguageChanged;

        string Translate(string key, params LocalizationArgument[] arguments);
        bool SetLanguage(string languageCode);
        string GetLanguageDisplayName(string languageCode = null);
    }

    public readonly struct LocalizationArgument
    {
        public LocalizationArgument(string name, object value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; }
        public object Value { get; }
    }
}
