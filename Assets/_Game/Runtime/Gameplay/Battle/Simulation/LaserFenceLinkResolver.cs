using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Gameplay.Battle.Simulation
{
    /// <summary>
    /// Pairs laser-fence nodes of the same owner into beams. Each node carries at most two beams, to its
    /// nearest neighbours within the owner's link range: candidate pairs are taken shortest first and a
    /// pair is skipped once either node already has two beams, so four nodes in a square form its outline
    /// without the diagonals.
    /// </summary>
    public static class LaserFenceLinkResolver
    {
        public const int MaximumLinksPerNode = 2;

        public static List<LaserFenceLink> Resolve(
            IReadOnlyList<LaserFenceNodeDeployedEvent> nodes,
            float maximumDistance)
        {
            return Resolve(nodes, _ => maximumDistance);
        }

        public static List<LaserFenceLink> Resolve(
            IReadOnlyList<LaserFenceNodeDeployedEvent> nodes,
            Func<string, float> maximumDistanceForOwner)
        {
            var links = new List<LaserFenceLink>();
            if (nodes == null || maximumDistanceForOwner == null)
            {
                return links;
            }

            var candidates = new List<(float DistanceSquared, LaserFenceNodeDeployedEvent First, LaserFenceNodeDeployedEvent Second)>();
            for (var firstIndex = 0; firstIndex < nodes.Count - 1; firstIndex++)
            {
                var first = nodes[firstIndex];
                var range = Mathf.Max(0f, maximumDistanceForOwner(first.OwnerUnitId));
                var rangeSquared = range * range;
                for (var secondIndex = firstIndex + 1; secondIndex < nodes.Count; secondIndex++)
                {
                    var second = nodes[secondIndex];
                    var distanceSquared = (first.Point - second.Point).sqrMagnitude;
                    if (!string.Equals(first.OwnerUnitId, second.OwnerUnitId, StringComparison.Ordinal)
                        || rangeSquared <= 0f || distanceSquared > rangeSquared)
                    {
                        continue;
                    }

                    candidates.Add((distanceSquared, first, second));
                }
            }

            candidates.Sort((left, right) =>
            {
                var byDistance = left.DistanceSquared.CompareTo(right.DistanceSquared);
                if (byDistance != 0) return byDistance;
                var byFirst = Math.Min(left.First.NodeId, left.Second.NodeId)
                    .CompareTo(Math.Min(right.First.NodeId, right.Second.NodeId));
                return byFirst != 0
                    ? byFirst
                    : Math.Max(left.First.NodeId, left.Second.NodeId).CompareTo(Math.Max(right.First.NodeId, right.Second.NodeId));
            });

            var degree = new Dictionary<int, int>();
            foreach (var candidate in candidates)
            {
                degree.TryGetValue(candidate.First.NodeId, out var firstDegree);
                degree.TryGetValue(candidate.Second.NodeId, out var secondDegree);
                if (firstDegree >= MaximumLinksPerNode || secondDegree >= MaximumLinksPerNode)
                {
                    continue;
                }

                degree[candidate.First.NodeId] = firstDegree + 1;
                degree[candidate.Second.NodeId] = secondDegree + 1;
                links.Add(new LaserFenceLink(candidate.First.NodeId, candidate.Second.NodeId));
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
