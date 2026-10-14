using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SpawnOnBeach.Classes;

public static class BeachPoints
{
    private const float YOffset = 2f;
    private static List<Vector3> _cachedPoints = [];

    public static void ClearCache()
    {
        _cachedPoints.Clear();
    }

    public static bool TryGetRandomBeachPoint(out Vector3 point)
    {
        point = default;

        var mapGen = MapGeneration.instance;
        if (mapGen == null || mapGen.nodes == null || mapGen.Edges == null)
            return false;

        // If not yet cached, get beach points from the map gen nodes
        List<Vector3> pointsToEvaluate;
        if (_cachedPoints is { Count: > 0 })
        {
            pointsToEvaluate = _cachedPoints;
        }
        else
        {
            var validPoints = new List<Vector3>();
            var visitedNodes = new HashSet<IntVec2>();
            foreach (var edge in mapGen.Edges)
            {
                if (edge == null || !edge.IsCoast())
                    continue;

                AddNodeBeachPoint(mapGen.nodes, edge.edgeStart, visitedNodes, validPoints);
                AddNodeBeachPoint(mapGen.nodes, edge.edgeEnd, visitedNodes, validPoints);
            }

            if (validPoints.Count == 0)
            {
                SpawnOnBeach.Logger.LogWarning("No valid beach points found.");
                return false;
            }

            _cachedPoints = validPoints;
            pointsToEvaluate = validPoints;
        }

        // Get top 3 candidates furthest from danger
        var candidateCount = Math.Min(3, pointsToEvaluate.Count);
        var topCandidates = pointsToEvaluate
            .OrderByDescending(Helper.GetClosestDanger)
            .Take(candidateCount)
            .ToList();

        // Then pick one of the top 3 candidates randomly
        var selectedPoint = topCandidates[UnityEngine.Random.Range(0, topCandidates.Count)];
        point = selectedPoint + Vector3.up * YOffset;

        return true;
    }

    private static void AddNodeBeachPoint(GridNode[,] nodes, IntVec2 gridPos, HashSet<IntVec2> visitedNodes,
        List<Vector3> validPoints)
    {
        if (!visitedNodes.Add(gridPos))
            return;

        // Check if the grid pos is not out of bounds
        if (gridPos.x < 0 || gridPos.x >= nodes.GetLength(0) ||
            gridPos.y < 0 || gridPos.y >= nodes.GetLength(1))
            return;

        var node = nodes[gridPos.x, gridPos.y];
        if (node?.beachPoints == null || node.beachPoints.Count < 2)
            return;

        // Beach points index explanations:
        // Index 0 is the side of the road position near the beach
        // Index 1 is the position in the beach where you start to touch the water in the feet
        var edgeRoad = node.beachPoints[0];
        var edgeWater = node.beachPoints[1];
        var midPoint = (edgeRoad + edgeWater) * 0.5f;
        validPoints.Add(midPoint);
    }
}
