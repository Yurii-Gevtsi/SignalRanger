using System;
using System.Collections.Generic;

namespace Game.App.Content
{
    /// <summary>
    /// Canonical pacing and reserved-planet data. Reserved entries are planning/runtime
    /// metadata until their mission and arena assets are activated in the content catalog.
    /// </summary>
    public readonly struct ReservedPlanetDefinition
    {
        public ReservedPlanetDefinition(string planetName, string chapterId, int missionCount,
            int recommendedLevelMin, int recommendedLevelMax, string environmentModifier,
            int firstMissionCreditsMin, int finalMissionCreditsMax)
        {
            PlanetName = planetName ?? string.Empty;
            ChapterId = chapterId ?? string.Empty;
            MissionCount = missionCount < 1 ? 1 : missionCount;
            RecommendedLevelMin = recommendedLevelMin < 1 ? 1 : recommendedLevelMin;
            RecommendedLevelMax = recommendedLevelMax < RecommendedLevelMin
                ? RecommendedLevelMin
                : recommendedLevelMax;
            EnvironmentModifier = environmentModifier ?? string.Empty;
            FirstMissionCreditsMin = firstMissionCreditsMin < 0 ? 0 : firstMissionCreditsMin;
            FinalMissionCreditsMax = finalMissionCreditsMax < FirstMissionCreditsMin
                ? FirstMissionCreditsMin
                : finalMissionCreditsMax;
        }

        public string PlanetName { get; }
        public string ChapterId { get; }
        public int MissionCount { get; }
        public int RecommendedLevelMin { get; }
        public int RecommendedLevelMax { get; }
        public string EnvironmentModifier { get; }
        public int FirstMissionCreditsMin { get; }
        public int FinalMissionCreditsMax { get; }
    }

    public static class CampaignPacingTargets
    {
        public const int FirstNewWeaponMinutesMin = 30;
        public const int FirstNewWeaponMinutesMax = 60;
        public const int Rank3HoursMin = 1;
        public const int Rank3HoursMax = 2;
        public const int Rank5HoursMin = 2;
        public const int Rank5HoursMax = 4;
        public const int Rank10HoursMin = 8;
        public const int Rank10HoursMax = 12;
        public const int FullCatalogHoursMin = 40;
        public const int FullCatalogHoursMax = 60;
    }

    public static class CampaignContentPlan
    {
        private static readonly ReservedPlanetDefinition[] ReservedPlanets =
        {
            new ReservedPlanetDefinition("Gale Foundry", "chapter.gale-foundry", 6, 8, 13,
                "variable-wind", 250, 850),
            new ReservedPlanetDefinition("Cryo Relay", "chapter.cryo-relay", 7, 14, 21,
                "slippery-freeze", 400, 1200)
        };

        public static IReadOnlyList<ReservedPlanetDefinition> AllReservedPlanets => ReservedPlanets;

        public static bool TryGetReservedPlanet(string chapterId, out ReservedPlanetDefinition definition)
        {
            for (var index = 0; index < ReservedPlanets.Length; index++)
            {
                if (string.Equals(ReservedPlanets[index].ChapterId, chapterId, StringComparison.Ordinal))
                {
                    definition = ReservedPlanets[index];
                    return true;
                }
            }

            definition = default;
            return false;
        }
    }
}
