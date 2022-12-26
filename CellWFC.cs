using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CellWFC
{
    List<Cell> uncollapsedCells = new List<Cell>();
    public void GenerateCellPattern()
    {
        for (int x = 0; x < TilemapData.numCells; x++) 
        {
            for (int y = 0; y < TilemapData.numCells; y++) 
            {
                Cell cell = new Cell(x, y);
                TilemapData.cellPattern[x, y] = cell;
                uncollapsedCells.Add(cell);
            }
        }

        Cell collapsedCell = TilemapData.cellPattern[0,0];

        int cellPosX;
        int cellPosY;

        while (uncollapsedCells.Count != 0)
        {
            collapsedCell.Collapse();
            uncollapsedCells.Remove(collapsedCell);

            cellPosX = collapsedCell.cellPosX;
            cellPosY = collapsedCell.cellPosY;

            List<int> possibleTypes;
            List<int> typesToRemove;

            // Cull types for left neighbor
            if (cellPosX != 0 && TilemapData.cellPattern[cellPosX - 1, cellPosY].cellId == -1)
            {
                possibleTypes = TilemapData.cellPattern[cellPosX - 1, cellPosY].GetPossibleTypes();
                typesToRemove = new List<int>();
                foreach (int type in possibleTypes)
                {
                    if (!Array.Exists<int>(collapsedCell.GetValidLeftNeighbors(), element => element == type))
                    {
                        typesToRemove.Add(type);
                    }
                }
                foreach (int type in typesToRemove)
                {
                    possibleTypes.Remove(type);
                }
            }

            // Cull types for right neighbor
            if (cellPosX != TilemapData.numCells - 1 && TilemapData.cellPattern[cellPosX + 1, cellPosY].cellId == -1)
            {
                possibleTypes = TilemapData.cellPattern[cellPosX + 1, cellPosY].GetPossibleTypes();
                typesToRemove = new List<int>();
                foreach (int type in possibleTypes)
                {
                    if (!Array.Exists<int>(collapsedCell.GetValidRightNeighbors(), element => element == type))
                    {
                        typesToRemove.Add(type);
                    }
                }
                foreach (int type in typesToRemove)
                {
                    possibleTypes.Remove(type);
                }
            }
            
            // Cull types for top neighbor
            if (cellPosY != TilemapData.numCells - 1 && TilemapData.cellPattern[cellPosX, cellPosY + 1].cellId == -1)
            {
                possibleTypes = TilemapData.cellPattern[cellPosX, cellPosY + 1].GetPossibleTypes();
                typesToRemove = new List<int>();
                foreach (int type in possibleTypes)
                {
                    if (!Array.Exists<int>(collapsedCell.GetValidTopNeighbors(), element => element == type))
                    {
                        typesToRemove.Add(type);
                    }
                }
                foreach (int type in typesToRemove)
                {
                    possibleTypes.Remove(type);
                }
            }

            // Cull types for bottom neighbor
            if (cellPosY != 0 && TilemapData.cellPattern[cellPosX, cellPosY - 1].cellId == -1)
            {
                possibleTypes = TilemapData.cellPattern[cellPosX, cellPosY - 1].GetPossibleTypes();
                typesToRemove = new List<int>();
                foreach (int type in possibleTypes)
                {
                    if (!Array.Exists<int>(collapsedCell.GetValidBottomNeighbors(), element => element == type))
                    {
                        typesToRemove.Add(type);
                    }
                }
                foreach (int type in typesToRemove)
                {
                    possibleTypes.Remove(type);
                }
            }

            if (uncollapsedCells.Count != 0)
            {
                collapsedCell = GetMinEntropyCell();
            }
        }
    }

    Cell GetMinEntropyCell()
    {
        Cell minEntropy = uncollapsedCells[0];
        for (int i = 1; i < uncollapsedCells.Count; i++)
        {
            if (uncollapsedCells[i].GetEntropy() < minEntropy.GetEntropy())
            {
                minEntropy = uncollapsedCells[i];
            }
        }
        return minEntropy;
    }
}
