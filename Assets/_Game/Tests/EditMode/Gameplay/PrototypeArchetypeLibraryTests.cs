using System.Linq;
using Game.Gameplay.Battle.AI;
using Game.Gameplay.Battle.Units;
using NUnit.Framework;

namespace Game.Tests.EditMode.Gameplay
{
    public sealed class PrototypeArchetypeLibraryTests
    {
        [Test]
        public void CreateAll_AreValidAndHaveDistinctIds()
        {
            var archetypes = PrototypeArchetypeLibrary.CreateAll();

            Assert.That(archetypes, Has.Length.InRange(2, 3));
            Assert.That(archetypes.Select(a => a.Id).Distinct().Count(), Is.EqualTo(archetypes.Length));
            foreach (var archetype in archetypes)
            {
                Assert.That(archetype.MaxHealth, Is.GreaterThanOrEqualTo(1));
                Assert.That(archetype.BodyHeight, Is.GreaterThan(0f));
                Assert.That(archetype.WalkBudget, Is.GreaterThan(0f));
                Assert.That(archetype.WalkSpeed, Is.GreaterThan(0f));
                Assert.That(archetype.FrontJumpDistance, Is.GreaterThan(archetype.BackJumpDistance));
                Assert.That(archetype.BackJumpHeight, Is.GreaterThanOrEqualTo(archetype.FrontJumpHeight));
                Assert.That(archetype.DisplayName, Is.Not.Empty);
            }
        }

        [Test]
        public void ForDifficulty_ScalesOpponentHealthWithDifficulty()
        {
            var easy = PrototypeArchetypeLibrary.ForDifficulty(AiDifficulty.Easy);
            var normal = PrototypeArchetypeLibrary.ForDifficulty(AiDifficulty.Normal);
            var hard = PrototypeArchetypeLibrary.ForDifficulty(AiDifficulty.Hard);

            Assert.That(easy.MaxHealth, Is.LessThan(normal.MaxHealth));
            Assert.That(normal.MaxHealth, Is.LessThan(hard.MaxHealth));
        }

        [Test]
        public void CreateAll_MobilityProfilesDifferentiateRoles()
        {
            var striker = PrototypeArchetypeLibrary.Striker;
            var skirmisher = PrototypeArchetypeLibrary.Skirmisher;
            var bulwark = PrototypeArchetypeLibrary.Bulwark;

            Assert.That(skirmisher.WalkBudget, Is.GreaterThan(striker.WalkBudget));
            Assert.That(striker.WalkBudget, Is.GreaterThan(bulwark.WalkBudget));
            Assert.That(skirmisher.FrontJumpDistance, Is.GreaterThan(bulwark.FrontJumpDistance));
            Assert.That(bulwark.BackJumpHeight, Is.LessThan(skirmisher.BackJumpHeight));
        }
    }
}
