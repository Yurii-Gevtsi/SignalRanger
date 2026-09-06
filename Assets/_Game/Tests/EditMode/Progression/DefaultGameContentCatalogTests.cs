using System.Linq;
using Game.App;
using Game.App.Content;
using Game.Gameplay.Battle.Arenas;
using NUnit.Framework;
using UnityEngine;

namespace Game.Tests.EditMode.Progression
{
    public sealed class DefaultGameContentCatalogTests
    {
        [Test]
        public void DefaultCatalog_HasValidCrossReferences()
        {
            var content = DefaultGameContentCatalog.Create();
            var report = ContentValidationReport.Validate(content);

            Assert.That(report.Errors, Is.Empty, string.Join("\n", report.Errors));
            Assert.That(content.Missions.Length, Is.EqualTo(18));
            Assert.That(content.Arenas.Length, Is.GreaterThanOrEqualTo(10));
            Assert.That(content.Chapters.Length, Is.EqualTo(9));
            Assert.That(content.EnemyLoadouts.Length, Is.GreaterThanOrEqualTo(3));
            Assert.That(content.DifficultyProfiles.Select(profile => profile.ProfileId).Distinct().Count(),
                Is.EqualTo(content.DifficultyProfiles.Length));
        }

        [Test]
        public void DefaultCatalog_OrganizesCurrentCampaignIntoTeachingChapters()
        {
            var content = DefaultGameContentCatalog.Create();

            Assert.That(content.TryGetChapter("chapter.basics", out var basicsChapter), Is.True);
            Assert.That(basicsChapter.MissionIds.Length, Is.EqualTo(5));
            Assert.That(basicsChapter.FeaturedWeaponIds, Contains.Item("weapon.impulse"));
            Assert.That(basicsChapter.FeaturedWeaponIds, Contains.Item("weapon.bur"));

            Assert.That(content.TryGetChapter("chapter.tactical-tools", out var toolsChapter), Is.True);
            Assert.That(toolsChapter.MissionIds.Length, Is.EqualTo(3));
            Assert.That(toolsChapter.FeaturedWeaponIds, Contains.Item("weapon.granata"));
            Assert.That(toolsChapter.FeaturedWeaponIds, Contains.Item("weapon.heal"));
        }

        [Test]
        public void FirstKerionMission_UsesKerionsBattleArena()
        {
            var content = DefaultGameContentCatalog.Create();

            Assert.That(content.TryGetMission("mission.training.01", out var firstMission), Is.True);
            Assert.That(firstMission.ArenaId, Is.EqualTo("arena.desert-sci-fi"));
            Assert.That(content.ResolveArenaIndex(firstMission), Is.EqualTo(1));
        }

        [Test]
        public void KerionCampaignMissions_AfterTheTutorial_UseDesertSciFiArena()
        {
            var content = DefaultGameContentCatalog.Create();

            foreach (var missionId in new[]
                     {
                         "mission.training.02", "mission.training.03",
                         "mission.training.04", "mission.training.05"
                     })
            {
                Assert.That(content.TryGetMission(missionId, out var mission), Is.True);
                Assert.That(mission.ArenaId, Is.EqualTo("arena.desert-sci-fi"));
                Assert.That(content.ResolveArenaIndex(mission), Is.EqualTo(1));
            }
        }

        [Test]
        public void SecondPlanetCampaignMissions_UseForestArena()
        {
            var content = DefaultGameContentCatalog.Create();

            foreach (var missionId in new[]
                     {
                         "mission.training.06", "mission.training.07", "mission.training.08"
                     })
            {
                Assert.That(content.TryGetMission(missionId, out var mission), Is.True);
                Assert.That(mission.ChapterId, Is.EqualTo("chapter.tactical-tools"));
                Assert.That(mission.ArenaId, Is.EqualTo("arena.forest"));
                Assert.That(content.ResolveArenaIndex(mission), Is.EqualTo(2));
            }
        }

        [TestCase("chapter.city", "mission.training.09", "mission.training.10", "arena.city", 3)]
        [TestCase("chapter.grib", "mission.training.11", "mission.training.12", "arena.grib", 4)]
        [TestCase("chapter.laguna", "mission.training.13", "mission.training.14", "arena.laguna", 5)]
        public void NewPlanetCampaignMissions_UseTheirDedicatedArena(
            string chapterId, string firstMissionId, string secondMissionId, string arenaId, int arenaIndex)
        {
            var content = DefaultGameContentCatalog.Create();

            foreach (var missionId in new[] { firstMissionId, secondMissionId })
            {
                Assert.That(content.TryGetMission(missionId, out var mission), Is.True);
                Assert.That(mission.ChapterId, Is.EqualTo(chapterId));
                Assert.That(mission.ArenaId, Is.EqualTo(arenaId));
                Assert.That(content.ResolveArenaIndex(mission), Is.EqualTo(arenaIndex));
            }
        }

        [Test]
        public void ForestArena_FillsTheZoomOutFrameAndStartsAtItsUpperEdge()
        {
            var forest = PrototypeArenaLibrary.CreateAll().Single(arena => arena.ArenaId == "arena.forest");

            Assert.That(forest.LimitZoomOutToSpriteBounds, Is.False);
            Assert.That(forest.SpriteArenaVerticalAnchor, Is.EqualTo(1f));
            Assert.That(forest.SpriteArenaVerticalScreenOffset, Is.EqualTo(0.15f));
            Assert.That(forest.LockCameraAtZoomOutLimit, Is.True);
            Assert.That(Resources.Load<Texture2D>("Visuals/Arenas/Backgrounds/Cosmos/cosmos"), Is.Not.Null);
        }

        [TestCase("chapter.monument", "mission.placeholder.monument", "arena.monument", 6)]
        [TestCase("chapter.kraken", "mission.placeholder.kraken", "arena.kraken", 7)]
        [TestCase("chapter.vines", "mission.placeholder.vines", "arena.vines", 8)]
        [TestCase("chapter.robots", "mission.placeholder.robots", "arena.robots", 9)]
        public void ImplementedPlanetPlaceholder_UsesThePlanetArena(string chapterId, string missionId, string arenaId, int arenaIndex)
        {
            var content = DefaultGameContentCatalog.Create();

            Assert.That(content.TryGetChapter(chapterId, out var chapter), Is.True);
            Assert.That(chapter.MissionIds, Is.EquivalentTo(new[] { missionId }));
            Assert.That(content.TryGetMission(missionId, out var mission), Is.True);
            Assert.That(mission.ArenaId, Is.EqualTo(arenaId));
            Assert.That(content.ResolveArenaIndex(mission), Is.EqualTo(arenaIndex));
        }

        [Test]
        public void EveryImplementedCampaignPlanet_OwnsOnlyMissionsForItsArena()
        {
            var content = DefaultGameContentCatalog.Create();

            foreach (var sector in CampaignPlanetCatalog.Sectors)
            {
                foreach (var planet in sector.Planets)
                {
                    if (string.IsNullOrWhiteSpace(planet.ArenaId))
                    {
                        continue;
                    }

                    Assert.That(content.TryGetChapter(planet.ChapterId, out var chapter), Is.True, planet.ChapterId);
                    Assert.That(chapter.MissionIds, Is.Not.Empty, planet.ChapterId);
                    foreach (var missionId in chapter.MissionIds)
                    {
                        Assert.That(content.TryGetMission(missionId, out var mission), Is.True, missionId);
                        Assert.That(mission.ArenaId, Is.EqualTo(planet.ArenaId), missionId);
                    }
                }
            }
        }
    }
}
