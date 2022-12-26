using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilemapGenerator : MonoBehaviour
{

    Dictionary<int, GameObject> tileset;
    Dictionary<int, GameObject> tileGroups;

    public GameObject[] tiles = new GameObject[2];

    int cellWidth = 40;
    int cellHeight = 40;

    List<List<int>> noiseGrid = new List<List<int>>();
    List<List<GameObject>> tileGrid = new List<List<GameObject>>();
    
    float magnification = 8.0f;
    int xOffset = 0;
    int yOffset = 0;

    // Start is called before the first frame update
    void Start()
    {
        GenerateTileset();
        GenerateTileGroups();
        GenerateMap();
    }

    void GenerateTileset() 
    {
        tileset = new Dictionary<int, GameObject>();
        for (int i = 0; i < tiles.Length; i++) 
        { 
            tileset.Add(i, tiles[i]);
        }
    }

    void GenerateTileGroups()
    {
        tileGroups = new Dictionary<int, GameObject>();
        foreach(KeyValuePair<int, GameObject> prefabPair in tileset)
        {
            GameObject tileGroup = new GameObject(prefabPair.Value.name);
            tileGroup.transform.parent = gameObject.transform;
            tileGroup.transform.localPosition = new Vector3(0, 0, 0);
            tileGroups.Add(prefabPair.Key, tileGroup);
        }
    }

    void GenerateMap()
    {
        for(int x = 0; x < cellWidth; x++)
        {
            noiseGrid.Add(new List<int>());
            tileGrid.Add(new List<GameObject>());

            for(int y = 0; y < cellHeight; y++)
            {
                int tileId = GetIdUsingPerlin(x, y);
                noiseGrid[x].Add(tileId);
                CreateTile(tileId, x, y);
            }
        }
    }

    int GetIdUsingPerlin(int x, int y)
    {
        float rawPerlin = Mathf.PerlinNoise((x - xOffset) / magnification, (y - yOffset) / magnification);
        float clampedPerlin = Mathf.Clamp01(rawPerlin);
        float scaledPerlin = clampedPerlin * tiles.Length;
        if (scaledPerlin == tiles.Length) {
            scaledPerlin = tiles.Length - 1;
        }

        return Mathf.FloorToInt(scaledPerlin);
    }

    void CreateTile(int tileId, int x, int y) {
        GameObject tilePrefab = tileset[tileId];
        GameObject tileGroup = tileGroups[tileId];
        GameObject tile = Instantiate(tilePrefab, tileGroup.transform);

        tile.name =  string.Format("tileX{0}Y{1}", x, y);
        tile.transform.localPosition = new Vector3(x, y, 0);

        tileGrid[x].Add(tile);
    }

    
}
