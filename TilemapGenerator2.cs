using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;

public class TilemapGenerator2 : MonoBehaviour
{   
    // Some of these serialized fields will later just be passed by the class that uses this one to generate levels.

    // Maps onto which the generator can draw
    [SerializeField] Tilemap topMap;
    [SerializeField] Tilemap botMap;

    // Tiles available to the generator
    [SerializeField] protected Tile[] tileSet = new Tile[2];

    // Number and dimension of each cell
    [SerializeField] int cellDim = 35;
    [SerializeField] int numCells = 4;

    // Blueprint of cells and map
    Cell[,] cellPattern;
    int[,] tilemapInstructions;

    void Start()
    {
        TilemapData.mapDim = cellDim * numCells;
        TilemapData.cellPattern = new Cell[numCells, numCells];
        TilemapData.cellDim = cellDim;
        TilemapData.numCells = numCells;
        TilemapData.mapDim = numCells * cellDim;
        TilemapData.tilemapInstructions = new int[TilemapData.mapDim, TilemapData.mapDim];
    }

    // Temporary method for testing
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            topMap.ClearAllTiles();
            botMap.ClearAllTiles();
            GenerateMap();
        }
    }

    // This will later need to work so that another class which creates levels can call it and customize it.
    void GenerateMap()
    {   
        // TilemapData.mapDim = cellDim * numCells;
        // TilemapData.cellPattern = new Cell[numCells, numCells];
        // TilemapData.cellDim = cellDim;
        // TilemapData.numCells = numCells;
        // TilemapData.mapDim = numCells * cellDim;
        // TilemapData.tilemapInstructions = new int[TilemapData.mapDim, TilemapData.mapDim];

        GenerateCliffs();
        GenerateCells();
        RenderMap();
    }

    void GenerateCliffs()
    {
        CliffGenerator cliffGenerator = new CliffGenerator();
        cliffGenerator.GenerateCliffs(0, 0);
    }

    void GenerateCells()
    {
        CellWFC waveformCollapse = new CellWFC();
        waveformCollapse.GenerateCellPattern();

        for (int x = 0; x < numCells; x++) 
        {
            for (int y = 0; y < numCells; y++) 
            {
                TilemapData.cellPattern[x, y].Generate();
                FillBotmap(x * cellDim, y * cellDim);
            }
        }
    }
    
    // Fills a cell with base grass. TODO: Improve
    void FillBotmap(int startX, int startY)
    {
        for (int x = startX; x < startX + cellDim; x++) 
        {
            for (int y = startY; y < startY + cellDim; y++) 
            {
                botMap.SetTile(new Vector3Int(x, y, 0), tileSet[0]);
            }
        }
    }

    // Turn the tilemap instructions into a tilemap and display it
    void RenderMap()
    {
        topMap.ClearAllTiles(); // TEMP

        for (int x = 0; x < TilemapData.mapDim; x++)
        {
            for (int y = 0; y < TilemapData.mapDim; y++)
            {
                if (TilemapData.tilemapInstructions[x, y] == 1)
                {
                    topMap.SetTile(new Vector3Int(x, y, 0), tileSet[1]);
                }
            }
        }
    }
}

public static class TilemapData
{
    public static int mapDim;
    public static int numCells;
    public static int cellDim;
    public static Cell[,] cellPattern;
    public static int[,] tilemapInstructions;
} 