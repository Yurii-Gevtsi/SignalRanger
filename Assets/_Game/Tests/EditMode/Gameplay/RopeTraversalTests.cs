using Game.Gameplay.Battle.Motion;
using Game.Gameplay.Battle.Terrain;
using NUnit.Framework;
using UnityEngine;

namespace Game.Tests.EditMode.Gameplay
{
    public sealed class RopeTraversalTests
    {
        [Test]
        public void Constructor_ClampsHorizontalAimToTheUpwardSector()
        {
            var rope = new RopeTraversal(
                "unit",
                Vector2.zero,
                Vector2.right,
                maximumLength: 8f,
                hookSpeed: 18f,
                sectorDegrees: 45f,
                swingAcceleration: 8f);

            Assert.That(Vector2.Angle(Vector2.up, rope.Direction), Is.EqualTo(22.5f).Within(0.01f));
            Assert.That(rope.Direction.x, Is.GreaterThan(0f));
        }

        [Test]
        public void Hook_AnchorsOnTheFirstSolidTerrainCell()
        {
            var terrain = BattleTerrain.CreateProcedural(new Rect(-4f, -2f, 8f, 8f));
            terrain.StampSolidRect(new Vector2(0f, 2f), new Vector2(1f, 0.25f));
            var rope = new RopeTraversal(
                "unit",
                Vector2.zero,
                Vector2.up,
                maximumLength: 6f,
                hookSpeed: 18f,
                sectorDegrees: 45f,
                swingAcceleration: 8f);

            for (var i = 0; i < 30 && !rope.IsAttached; i++)
            {
                rope.StepHook(terrain, 1f / 60f);
            }

            Assert.That(rope.IsAttached, Is.True);
            Assert.That(rope.Anchor.y, Is.GreaterThan(1.8f));
            Assert.That(rope.Anchor.y, Is.LessThan(2.2f));
        }

        [Test]
        public void Hook_ExpiresWhenItsMaximumLengthIsSpent()
        {
            var terrain = BattleTerrain.CreateProcedural(new Rect(-4f, -2f, 8f, 8f));
            var rope = new RopeTraversal(
                "unit",
                Vector2.zero,
                Vector2.up,
                maximumLength: 1f,
                hookSpeed: 18f,
                sectorDegrees: 45f,
                swingAcceleration: 8f);

            var remainsActive = true;
            for (var i = 0; i < 10 && remainsActive; i++)
            {
                remainsActive = rope.StepHook(terrain, 1f / 60f);
            }

            Assert.That(remainsActive, Is.False);
            Assert.That(rope.IsAttached, Is.False);
        }
    }
}
