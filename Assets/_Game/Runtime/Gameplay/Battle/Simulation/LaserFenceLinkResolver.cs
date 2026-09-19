using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Gameplay.Battle.Simulation
{
    /// <summary>Pairs compatible laser-fence nodes that are close enough to form a beam.</summary>
    public static class LaserFenceLinkResolver
    {
        public static List<LaserFenceLink> Resolve(
            IReadOnlyList<LaserFenceNodeDeployedEvent> nodes,
            float maximumDistance)
        {
            var links = new List<LaserFenceLink>();
            var maximumDistanceSquared = Mathf.Max(0f, maximumDistance) * Mathf.Max(0f, maximumDistance);
            if (nodes == null || maximumDistanceSquared <= 0f)
            {
                return links;
            }

            for (var firstIndex = 0; firstIndex < nodes.Count - 1; firstIndex++)
            {
                var first = nodes[firstIndex];
                for (var secondIndex = firstIndex + 1; secondIndex < nodes.Count; secondIndex++)
                {
                    var second = nodes[secondIndex];
                    if (!string.Equals(first.OwnerUnitId, second.OwnerUnitId, StringComparison.Ordinal)
                        || (first.Point - second.Point).sqrMagnitude > maximumDistanceSquared)
                    {
                        continue;
                    }

                    links.Add(new LaserFenceLink(first.NodeId, second.NodeId));
                }
            }

            return links;
        }
    }

    public readonly struct LaserFenceLink
    {
        public LaserFenceLink(int firstNodeId, int secondNodeId)
        {
            FirstNodeId = firstNodeId;
            SecondNodeId = secondNodeId;
        }

        public int FirstNodeId { get; }
        public int SecondNodeId { get; }
    }
}
