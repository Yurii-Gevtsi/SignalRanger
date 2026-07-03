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
            Assert.That(basicsChapter.FeaturedWeaponIds, Contains.Item("weapon.steady-orb"));
            Assert.That(basicsChapter.FeaturedWeaponIds, Contains.Item("weapon.long-lance"));

            Assert.That(content.TryGetChapter("chapter.tactical-tools", out var toolsChapter), Is.True);
            Assert.That(toolsChapter.MissionIds.Length, Is.EqualTo(3));
            Assert.That(toolsChapter.FeaturedWeaponIds, Contains.Item("weapon.rampart-kit"));
            Assert.That(toolsChapter.FeaturedWeaponIds, Contains.Item("weapon.guard-pulse"));
        }
    }
}
