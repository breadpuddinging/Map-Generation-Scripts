using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CliffGenerator
{
    int width;
    int height;

    int[,] tilemapInstructions;

    float magnification = 15.0f;

    // Let these be randomized later
    int xOffset = 0;
    int yOffset = 0;

    public CliffGenerator()
    {
        this.width = TilemapData.mapDim;
        this.height = TilemapData.mapDim;
    }

    public void GenerateCliffs(int startX, int startY)
    {
        for(int x = startX; x < startX + width; x++)
        {
            for(int y = startY; y < startY + height; y++)
            {
                if (TileIsCliff(x, y))
                {
                    TilemapData.tilemapInstructions[x, y] = 1;
                }
            }
        }
    }

    bool TileIsCliff(int x, int y)
    {
        float rawPerlin = Mathf.PerlinNoise((x - xOffset) / magnification, (y - yOffset) / magnification);
        float clampedPerlin = Mathf.Clamp01(rawPerlin);
        float scaledPerlin = clampedPerlin * 2;
        if (scaledPerlin >= 1)
        {
            return true;
        }
        return false;
    }
}
