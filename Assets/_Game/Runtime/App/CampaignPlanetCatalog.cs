using System;
using UnityEngine;

namespace Game.App
{
    /// <summary>
    /// The planets of the campaign map and the mission chapter each one owns. Shared so
    /// the map, the loading screen and anything else naming a planet agree - a planet is
    /// identified by its chapter, never by a position in the mission list.
    /// </summary>
    public static class CampaignPlanetCatalog
    {
        /// <summary>
        /// Colour identifies the planet rather than its state; an empty chapter id marks
        /// a planet held for future content.
        /// </summary>
        public readonly struct Planet
        {
            public Planet(string name, string starResource, Vector2 position, string chapterId)
            {
                Name = name;
                StarResource = starResource;
                Position = position;
                ChapterId = chapterId;
            }

            public string Name { get; }
            public string StarResource { get; }

            /// <summary>Local position inside the open cluster's node.</summary>
            public Vector2 Position { get; }
            public string ChapterId { get; }
        }

        public static readonly Planet[] Planets =
        {
            new("Керіон", "star_blue", new Vector2(15f, 120f), "chapter.basics"),
            new("Вальтара", "star_red", new Vector2(130f, 165f), "chapter.tactical-tools"),
            new("Орн-9", "star_yellow", new Vector2(220f, 100f), string.Empty),
            new("Тайрен", "star_blue", new Vector2(330f, 140f), string.Empty)
        };

        /// <summary>Planet owning a chapter, or an empty string when nothing claims it.</summary>
        public static string GetPlanetName(string chapterId)
        {
            if (string.IsNullOrWhiteSpace(chapterId))
            {
                return string.Empty;
            }

            for (var i = 0; i < Planets.Length; i++)
            {
                if (string.Equals(Planets[i].ChapterId, chapterId, StringComparison.Ordinal))
                {
                    return Planets[i].Name;
                }
            }

            return string.Empty;
        }
    }
}
