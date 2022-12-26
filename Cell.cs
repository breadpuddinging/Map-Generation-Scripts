using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell
{
    List<Vector2Int> guarenteedPathTiles = new List<Vector2Int>();
    List<Vector2Int> border = new List<Vector2Int>();
    public int cellPosX; // Position in the cell 2D array
    public int cellPosY;
    public int cellId;
    int bottomLeftCornerX; // Actual position, in tile coordinates, of the bottom left tile of the cell
    int bottomLeftCornerY;

    int[][] validNeighbors;

    List<int> possibleCellTypes = new List<int>();

    public Cell(int cellPosX, int cellPosY, int cellId = -1) 
    {
        this.cellPosX = cellPosX;
        this.cellPosY = cellPosY;
        this.cellId = cellId;
        bottomLeftCornerX = cellPosX * TilemapData.cellDim;
        bottomLeftCornerY = cellPosY * TilemapData.cellDim;
        
        // If the cellId is not -1, it means it has been manually collapsed, so the possibility list should be empty
        if (cellId == -1)
        {
            // Fill the array with the id's of each possible cell described in the switch
            for (int i = 0; i < 12; i++)
            {
                possibleCellTypes.Add(i);
            }

            // If the cell is a left or a right wall, remove the possibility of it generating as a cell that could let the player out of the map
            if (cellPosX == 0)
            {
                int[] illegalCells = new int[] {2, 4, 6, 8, 9, 10, 11};
                foreach (int illegalCell in illegalCells)
                {
                    possibleCellTypes.Remove(illegalCell);
                }
            }
            if (cellPosX == TilemapData.numCells - 1)
            {
                int[] illegalCells = new int[] {2, 3, 5, 7, 9, 10, 11};
                foreach (int illegalCell in illegalCells)
                {
                    possibleCellTypes.Remove(illegalCell);
                }
            }
            if (cellPosY == 0)
            {
                int[] illegalCells = new int[] {2, 5, 6, 10};
                foreach (int illegalCell in illegalCells)
                {
                    possibleCellTypes.Remove(illegalCell);
                }
            }
        }

    }

    public void Collapse()
    {   
        if (possibleCellTypes.Count == 0) // Bug-catching code
        {
            Debug.Log("Impossible configuration");
            cellId = 0;
            return;
        }

        cellId = possibleCellTypes[Random.Range(0, possibleCellTypes.Count)];

        int x = bottomLeftCornerX;
        int y = bottomLeftCornerY;

        switch(cellId) 
        {
        case 0:
            
            validNeighbors = new int[][]
            {
                new int[] {0, 2, 5, 6, 10}, 
                new int[] {0, 1, 3, 5, 7}, 
                new int[] {0, 2, 3, 4, 9}, 
                new int[] {0, 1, 4, 6, 8}
            };

            break;
        case 1: // |
            for (int i = -1; i < (TilemapData.cellDim + 1); i++)
            {
                guarenteedPathTiles.Add(new Vector2Int(x + TilemapData.cellDim / 2, y + i));
            }   
            
            validNeighbors = new int[][]
            {
                new int[] {1, 3, 4, 7, 8, 9, 11}, 
                new int[] {0, 1, 3, 5, 7}, 
                new int[] {1, 5, 6, 7, 8, 10, 11}, 
                new int[] {0, 1, 4, 6, 8}
            };

            break;
        case 2: // __
            for (int i = -1; i < (TilemapData.cellDim + 1); i++)
            {
                guarenteedPathTiles.Add(new Vector2Int(x + i, y + TilemapData.cellDim / 2));
            }   
            
            validNeighbors = new int[][]
            {
                new int[] {0, 2, 5, 6, 10}, 
                new int[] {2, 4, 6, 8, 9, 10, 11}, 
                new int[] {0, 2, 3, 4, 9}, 
                new int[] {2, 3, 5, 7, 9, 10, 11}
            };

            break;
        case 3: // r
            for (int i = -1; i < (TilemapData.cellDim + 2) / 2; i++)
            {
                guarenteedPathTiles.Add(new Vector2Int(x + TilemapData.cellDim / 2, y + i)); // ^
                guarenteedPathTiles.Add(new Vector2Int(x + TilemapData.cellDim / 2 + i, y + TilemapData.cellDim / 2)); // ->
            }    
            
            validNeighbors = new int[][]
            {
                new int[] {0, 2, 5, 6, 10}, 
                new int[] {2, 4, 6, 8, 9, 10, 11}, 
                new int[] {1, 5, 6, 7, 8, 10, 11}, 
                new int[] {0, 1, 4, 6, 8}
            };

            break;
        case 4: // 7
            for (int i = -1; i < (TilemapData.cellDim + 2) / 2; i++)
            {
                guarenteedPathTiles.Add(new Vector2Int(x + TilemapData.cellDim / 2, y + i)); // ^
                guarenteedPathTiles.Add(new Vector2Int(x + TilemapData.cellDim / 2 - i, y + TilemapData.cellDim / 2)); // <-
            }    
            
            validNeighbors = new int[][]
            {
                new int[] {0, 2, 5, 6, 10}, 
                new int[] {0, 1, 3, 5, 7}, 
                new int[] {1, 5, 6, 7, 8, 10, 11}, 
                new int[] {2, 3, 5, 7, 9, 10, 11}
            };

            break;
        case 5: // L
            for (int i = -1; i < (TilemapData.cellDim + 2) / 2; i++)
            {
                guarenteedPathTiles.Add(new Vector2Int(x + TilemapData.cellDim / 2, y + TilemapData.cellDim - i)); // \/
                guarenteedPathTiles.Add(new Vector2Int(x + TilemapData.cellDim / 2 + i, y + TilemapData.cellDim / 2)); // ->
            }    
            
            validNeighbors = new int[][]
            {
                new int[] {1, 3, 4, 7, 8, 9, 11}, 
                new int[] {2, 4, 6, 8, 9, 10, 11}, 
                new int[] {0, 2, 3, 4, 9}, 
                new int[] {0, 1, 4, 6, 8}
            };

            break;
        case 6: // j
            for (int i = -1; i < (TilemapData.cellDim + 2) / 2; i++)
            {
                guarenteedPathTiles.Add(new Vector2Int(x + TilemapData.cellDim / 2, y + TilemapData.cellDim - i)); // \/
                guarenteedPathTiles.Add(new Vector2Int(x + TilemapData.cellDim / 2 - i, y + TilemapData.cellDim / 2)); // <-
            }    
            
            validNeighbors = new int[][]
            {
                new int[] {1, 3, 4, 7, 8, 9, 11}, 
                new int[] {0, 1, 3, 5, 7}, 
                new int[] {0, 2, 3, 4, 9}, 
                new int[] {2, 3, 5, 7, 9, 10, 11}
            };

            break;
        case 7: // |-
            for (int i = -1; i < TilemapData.cellDim + 1; i++) // |
            {
                guarenteedPathTiles.Add(new Vector2Int(x + TilemapData.cellDim / 2, y + i));
            } 
            for (int i = -1; i < (TilemapData.cellDim + 2) / 2; i++) // ->
            {
                guarenteedPathTiles.Add(new Vector2Int(x + TilemapData.cellDim / 2 + i, y + TilemapData.cellDim / 2));
            } 
            
            validNeighbors = new int[][]
            {
                new int[] {1, 3, 4, 7, 8, 9, 11}, 
                new int[] {2, 4, 6, 8, 9, 10, 11}, 
                new int[] {1, 5, 6, 7, 8, 10, 11}, 
                new int[] {0, 1, 4, 6, 8}
            };

            break;
        case 8: // -|
            for (int i = -1; i < TilemapData.cellDim + 1; i++) // |
            {
                guarenteedPathTiles.Add(new Vector2Int(x + TilemapData.cellDim / 2, y + i));
            } 
            for (int i = -1; i < (TilemapData.cellDim + 2) / 2; i++) // <-
            {
                guarenteedPathTiles.Add(new Vector2Int(x + TilemapData.cellDim / 2 - i, y + TilemapData.cellDim / 2));
            } 
            
            validNeighbors = new int[][]
            {
                new int[] {1, 3, 4, 7, 8, 9, 11}, 
                new int[] {0, 1, 3, 5, 7}, 
                new int[] {1, 5, 6, 7, 8, 10, 11}, 
                new int[] {2, 3, 5, 7, 9, 10, 11}
            };

            break;
        case 9: // T
            for (int i = -1; i < TilemapData.cellDim + 1; i++) // __
            {
                guarenteedPathTiles.Add(new Vector2Int(x + i, y + TilemapData.cellDim / 2));
            }  
            for (int i = -1; i < (TilemapData.cellDim + 2) / 2; i++) // \/
            {
                guarenteedPathTiles.Add(new Vector2Int(x + TilemapData.cellDim / 2 , y + TilemapData.cellDim / 2 - i));
            } 
            
            validNeighbors = new int[][]
            {
                new int[] {0, 2, 5, 6, 10}, 
                new int[] {2, 4, 6, 8, 9, 10, 11}, 
                new int[] {1, 5, 6, 7, 8, 10, 11}, 
                new int[] {2, 3, 5, 7, 9, 10, 11}
            };

            break;
        case 10: // _|_
            for (int i = -1; i < TilemapData.cellDim + 1; i++) // __
            {
                guarenteedPathTiles.Add(new Vector2Int(x + i, y + TilemapData.cellDim / 2));
            }  
            for (int i = -1; i < (TilemapData.cellDim + 2) / 2; i++) // ^
            {
                guarenteedPathTiles.Add(new Vector2Int(x + TilemapData.cellDim / 2 , y + TilemapData.cellDim / 2 + i));
            } 
            
            validNeighbors = new int[][]
            {
                new int[] {1, 3, 4, 7, 8, 9, 11}, 
                new int[] {2, 4, 6, 8, 9, 10, 11}, 
                new int[] {0, 2, 3, 4, 9}, 
                new int[] {2, 3, 5, 7, 9, 10, 11}
            };

            break;
        case 11: // +
            for (int i = -1; i < TilemapData.cellDim + 1; i++) // __
            {
                guarenteedPathTiles.Add(new Vector2Int(x + i, y + TilemapData.cellDim / 2));
            }  
            for (int i = -1; i < TilemapData.cellDim + 1; i++) // |
            {
                guarenteedPathTiles.Add(new Vector2Int(x + TilemapData.cellDim / 2, y + i));
            } 
            
            validNeighbors = new int[][]
            {
                new int[] {1, 3, 4, 7, 8, 9, 11}, 
                new int[] {2, 4, 6, 8, 9, 10, 11}, 
                new int[] {1, 5, 6, 7, 8, 10, 11}, 
                new int[] {2, 3, 5, 7, 9, 10, 11}
            };

            break;
        }
    }

    public List<int> GetPossibleTypes()
    {
        return possibleCellTypes;
    }

    public int GetEntropy()
    {
        return possibleCellTypes.Count;
    }

    public int[] GetValidTopNeighbors()
    {
        return validNeighbors[0];
    }
    public int[] GetValidRightNeighbors()
    {
        return validNeighbors[1];
    }
    public int[] GetValidBottomNeighbors()
    {
        return validNeighbors[2];
    }
    public int[] GetValidLeftNeighbors()
    {
        return validNeighbors[3];
    }

    public void PrefabCellOverride(int[,] tilemap)
    {
        for (int x = 0; x < TilemapData.cellDim; x++)
        {
            for (int y = 0; y < TilemapData.cellDim; y++)
            {
                int tileId = tilemap[x, y];
                if (tileId != 0)
                {
                    TilemapData.tilemapInstructions[x + bottomLeftCornerX, y + bottomLeftCornerY] = tileId;
                }
            }
        }
    }

    public void Generate()
    {   
        if (cellId == 0)
        {
            FillCell();
        } else {
            // FillCell(); // TEMP
            GenerateBorder();
            GeneratePath();
        }
    }

    void FillCell()
    {
        for (int x = 0; x < TilemapData.cellDim; x++)
        {
            for (int y = 0; y < TilemapData.cellDim; y++)
            {
                TilemapData.tilemapInstructions[x + bottomLeftCornerX, y + bottomLeftCornerY] = 1;
            }
        }
    }

    void GenerateBorder()
    {
        int startX = cellPosX * TilemapData.cellDim;
        int startY = cellPosY * TilemapData.cellDim;

        for (int i = startX; i < startX + TilemapData.cellDim; i++) 
        {
            border.Add(new Vector2Int(i, startY));
            border.Add(new Vector2Int(i, startY + TilemapData.cellDim - 1));
        }

        for (int i = startY; i < startY + TilemapData.cellDim; i++) 
        {
            border.Add(new Vector2Int(startX, i));
            border.Add(new Vector2Int(startX + TilemapData.cellDim - 1, i));
        }
        
        RandomWalkPathGenerator.RunProceduralGeneration(border, 5, 3, true, 1);
    }

    void GeneratePath()
    {   
        RandomWalkPathGenerator.RunProceduralGeneration(guarenteedPathTiles, 10, 6, true, 0);
    }
}
