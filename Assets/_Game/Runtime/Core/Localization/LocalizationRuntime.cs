namespace Game.Core.Localization
{
    public static class LocalizationRuntime
    {
        public static ILocalizationService Service { get; set; }

        public static string Get(string key, string fallback, params LocalizationArgument[] arguments)
        {
            if (Service == null)
            {
                return fallback;
            }

            var value = Service.Translate(key, arguments);
            return value.StartsWith("[", System.StringComparison.Ordinal) ? fallback : value;
        }
    }
}
