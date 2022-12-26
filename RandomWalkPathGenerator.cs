using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Tilemaps;

public static class RandomWalkPathGenerator
{
    public static void RunProceduralGeneration(List<Vector2Int> startPositions, int iterations, int walkLength, bool startRandomlyEachIteration, int tileId)
    {
        foreach (Vector2Int startPosition in startPositions)
        {
            HashSet<Vector2Int> floorPositions = RunRandomWalk(startPosition, iterations, walkLength, startRandomlyEachIteration);
            foreach (var position in floorPositions)
            {
                // This if statement makes sure that the index being accessed is within the array. 
                // A better solution should be designed for the future?
                if (position.x >= 0 && position.x < 140 
                    && position.y >= 0 && position.y < 140) // 140 is temp. Later, this method should know how large the tilemap is
                    {
                        TilemapData.tilemapInstructions[position.x, position.y] = tileId;
                    }
            }
        }
    }

    static HashSet<Vector2Int> RunRandomWalk(Vector2Int startPosition, int iterations, int walkLength, bool startRandomlyEachIteration)
    {
        var currentPosition = startPosition;
        HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>();
        for (int i = 0; i < iterations; i++)
        {
            var path = RandomWalk.SimpleRandomWalk(currentPosition, walkLength);
            floorPositions.UnionWith(path);
            if (startRandomlyEachIteration)
            {
                currentPosition = floorPositions.ElementAt(Random.Range(0, floorPositions.Count));
            }
        }
        return floorPositions;
    }
}
