using System;
using Game.Core.Localization;
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
            public Planet(string mapResource, Vector2 position, string chapterId, string localizationKey, string arenaId = "")
            {
                MapResource = mapResource;
                Position = position;
                ChapterId = chapterId;
                LocalizationKey = localizationKey;
                ArenaId = arenaId ?? string.Empty;
            }

            public string MapResource { get; }

            /// <summary>Local position inside the open cluster's node.</summary>
            public Vector2 Position { get; }
            public string ChapterId { get; }
            public string LocalizationKey { get; }
            public string ArenaId { get; }
        }

        public readonly struct Sector
        {
            public Sector(string backgroundResource, string localizationKey, Planet[] planets)
            {
                BackgroundResource = backgroundResource;
                LocalizationKey = localizationKey;
                Planets = planets;
            }

            public string BackgroundResource { get; }
            public string LocalizationKey { get; }
            public Planet[] Planets { get; }
        }

        public static readonly Planet[] Planets =
        {
            new("campaign_planet_12", new Vector2(-390f, 60f), "chapter.basics", "campaign.planet.kerion", "arena.training-meadow"),
            new("campaign_planet_10", new Vector2(-130f, -70f), "chapter.tactical-tools", "campaign.planet.valtara", "arena.bones"),
            new("campaign_planet_03", new Vector2(150f, 50f), "chapter.city", "campaign.planet.gale-foundry", "arena.monument"),
            new("campaign_planet_01", new Vector2(410f, -70f), "chapter.grib", "campaign.planet.cryo-relay", "arena.forest"),
            new("campaign_planet_05", new Vector2(-390f, 60f), "chapter.laguna", "campaign.planet.hailis", "arena.laguna"),
            new("campaign_planet_06", new Vector2(-130f, -70f), "chapter.monument", "campaign.planet.ferrum", "arena.monument"),
            new("campaign_planet_07", new Vector2(150f, 50f), "chapter.kraken", "campaign.planet.pyra", "arena.kraken"),
            new("campaign_planet_08", new Vector2(410f, -70f), "chapter.vines", "campaign.planet.noctara", "arena.vines"),
            new("campaign_planet_09", new Vector2(-390f, 60f), "chapter.robots", "campaign.planet.selene", "arena.robots")
        };

        public static readonly Sector[] Sectors =
        {
            new("campaign_sector_01_background", "campaign.sector.1", new[]
            {
                new Planet("campaign_planet_12", new Vector2(-390f, 60f), "chapter.basics", "campaign.planet.kerion", "arena.training-meadow"),
                new Planet("campaign_planet_10", new Vector2(-130f, -70f), "chapter.tactical-tools", "campaign.planet.valtara", "arena.bones"),
                new Planet("campaign_planet_03", new Vector2(150f, 50f), "chapter.city", "campaign.planet.gale-foundry", "arena.monument"),
                new Planet("campaign_planet_01", new Vector2(410f, -70f), "chapter.grib", "campaign.planet.cryo-relay", "arena.forest")
            }),
            new("campaign_sector_02_background", "campaign.sector.2", new[]
            {
                new Planet("campaign_planet_05", new Vector2(-390f, 60f), "chapter.laguna", "campaign.planet.hailis", "arena.laguna"),
                new Planet("campaign_planet_06", new Vector2(-130f, -70f), "chapter.monument", "campaign.planet.ferrum", "arena.monument"),
                new Planet("campaign_planet_07", new Vector2(150f, 50f), "chapter.kraken", "campaign.planet.pyra", "arena.kraken"),
                new Planet("campaign_planet_08", new Vector2(410f, -70f), "chapter.vines", "campaign.planet.noctara", "arena.vines")
            }),
            new("campaign_sector_03_background", "campaign.sector.3", new[]
            {
                new Planet("campaign_planet_09", new Vector2(-390f, 60f), "chapter.robots", "campaign.planet.selene", "arena.robots"),
                new Planet("campaign_planet_10", new Vector2(-130f, -70f), string.Empty, "campaign.planet.pelagos"),
                new Planet("campaign_planet_11", new Vector2(150f, 50f), string.Empty, "campaign.planet.cinder"),
                new Planet("campaign_planet_12", new Vector2(410f, -70f), string.Empty, "campaign.planet.asteron")
            }),
            new("campaign_sector_04_background", "campaign.sector.4", new[]
            {
                new Planet("campaign_planet_01", new Vector2(-260f, 40f), string.Empty, "campaign.planet.viridia"),
                new Planet("campaign_planet_02", new Vector2(0f, -70f), string.Empty, "campaign.planet.dione"),
                new Planet("campaign_planet_03", new Vector2(280f, 55f), string.Empty, "campaign.planet.jovaris")
            })
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
                    return GameText.Localized(Planets[i].LocalizationKey, string.Empty);
                }
            }

            return string.Empty;
        }
    }
}
