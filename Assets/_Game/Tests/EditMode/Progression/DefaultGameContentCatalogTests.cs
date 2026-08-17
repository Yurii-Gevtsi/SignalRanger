using System.Linq;
using Game.App.Content;
using NUnit.Framework;

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
            Assert.That(content.Missions.Length, Is.InRange(8, 12));
            Assert.That(content.Arenas.Length, Is.GreaterThanOrEqualTo(3));
            Assert.That(content.Chapters.Length, Is.GreaterThanOrEqualTo(2));
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
        public void KerionCampaignMissions_UseDesertSciFiArena()
        {
            var content = DefaultGameContentCatalog.Create();

            foreach (var missionId in new[]
                     {
                         "mission.training.01", "mission.training.02", "mission.training.03",
                         "mission.training.04", "mission.training.05"
                     })
            {
                Assert.That(content.TryGetMission(missionId, out var mission), Is.True);
                Assert.That(mission.ArenaId, Is.EqualTo("arena.desert-sci-fi"));
                Assert.That(content.ResolveArenaIndex(mission), Is.EqualTo(1));
            }
        }
    }
}
