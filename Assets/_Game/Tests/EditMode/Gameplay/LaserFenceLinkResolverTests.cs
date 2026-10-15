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
        public void FourNodesInASquare_FormItsOutlineWithoutTheDiagonals()
        {
            var nodes = new List<LaserFenceNodeDeployedEvent>
            {
                Node(1, "player.1", new Vector2(0f, 0f)),
                Node(2, "player.1", new Vector2(1.2f, 0f)),
                Node(3, "player.1", new Vector2(0f, 1.2f)),
                Node(4, "player.1", new Vector2(1.2f, 1.2f))
            };

            var links = LaserFenceLinkResolver.Resolve(nodes, 1.8f);

            Assert.That(links, Has.Count.EqualTo(4));
            Assert.That(links.Exists(l => (l.FirstNodeId == 1 && l.SecondNodeId == 4) || (l.FirstNodeId == 4 && l.SecondNodeId == 1)),
                Is.False, "no diagonal 1-4");
            Assert.That(links.Exists(l => (l.FirstNodeId == 2 && l.SecondNodeId == 3) || (l.FirstNodeId == 3 && l.SecondNodeId == 2)),
                Is.False, "no diagonal 2-3");
        }

        [Test]
        public void EveryNodeCarriesAtMostTwoBeamsToItsNearestNeighbours()
        {
            var nodes = new List<LaserFenceNodeDeployedEvent>();
            for (var i = 0; i < 5; i++) nodes.Add(Node(i + 1, "player.1", new Vector2(i * 1f, 0f)));
            nodes.Add(Node(6, "player.1", new Vector2(2f, 0.4f)));     // crowds around the middle node

            var links = LaserFenceLinkResolver.Resolve(nodes, 5f);

            var degree = new Dictionary<int, int>();
            foreach (var link in links)
            {
                degree[link.FirstNodeId] = degree.TryGetValue(link.FirstNodeId, out var a) ? a + 1 : 1;
                degree[link.SecondNodeId] = degree.TryGetValue(link.SecondNodeId, out var b) ? b + 1 : 1;
            }

            Assert.That(degree.Values, Has.All.LessThanOrEqualTo(2));
            Assert.That(links.Exists(l => (l.FirstNodeId == 3 && l.SecondNodeId == 6) || (l.FirstNodeId == 6 && l.SecondNodeId == 3)),
                Is.True, "the closest pair is always linked");
        }

        [Test]
        public void TheLinkRangeIsResolvedPerOwner()
        {
            var nodes = new List<LaserFenceNodeDeployedEvent>
            {
                Node(1, "player.1", new Vector2(0f, 0f)),
                Node(2, "player.1", new Vector2(6f, 0f)),
                Node(3, "opponent.1", new Vector2(0f, 2f)),
                Node(4, "opponent.1", new Vector2(6f, 2f))
            };

            var links = LaserFenceLinkResolver.Resolve(nodes, owner => owner == "player.1" ? 7.56f : 5.4f);

            Assert.That(links, Has.Count.EqualTo(1));
            Assert.That(links[0].FirstNodeId, Is.EqualTo(1));
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
