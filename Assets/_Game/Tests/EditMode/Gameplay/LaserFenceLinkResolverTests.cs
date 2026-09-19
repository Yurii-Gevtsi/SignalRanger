using System.Collections.Generic;
using Game.Gameplay.Battle.Domain;
using Game.Gameplay.Battle.Simulation;
using NUnit.Framework;
using UnityEngine;

namespace Game.Tests.EditMode.Gameplay
{
    public sealed class LaserFenceLinkResolverTests
    {
        [Test]
        public void FourNearbyNodes_FormAllSixSquareAndDiagonalLinks()
        {
            var nodes = new List<LaserFenceNodeDeployedEvent>
            {
                Node(1, "player.1", new Vector2(0f, 0f)),
                Node(2, "player.1", new Vector2(1.2f, 0f)),
                Node(3, "player.1", new Vector2(0f, 1.2f)),
                Node(4, "player.1", new Vector2(1.2f, 1.2f))
            };

            var links = LaserFenceLinkResolver.Resolve(nodes, 1.8f);

            Assert.That(links, Has.Count.EqualTo(6));
        }

        [Test]
        public void NodesOfDifferentOwners_AndNodesOutsideRange_DoNotLink()
        {
            var nodes = new List<LaserFenceNodeDeployedEvent>
            {
                Node(1, "player.1", Vector2.zero),
                Node(2, "opponent.1", new Vector2(0.5f, 0f)),
                Node(3, "player.1", new Vector2(2f, 0f))
            };

            var links = LaserFenceLinkResolver.Resolve(nodes, 1.8f);

            Assert.That(links, Is.Empty);
        }

        private static LaserFenceNodeDeployedEvent Node(int id, string ownerId, Vector2 point)
        {
            return new LaserFenceNodeDeployedEvent(id, ownerId, point, null, BattleTeamId.Player);
        }
    }
}
