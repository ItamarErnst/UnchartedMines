using System.Collections.Generic;
using UnityEngine;

public static class WallTypeGenerator
{
    public static List<Vector2Int> GenerateCenterCells(int gridSize)
    {
        List<Vector2Int> center_cells = new List<Vector2Int>();

        // Populate center_cells using nested loops
        for (int x = -gridSize; x <= gridSize; x++)
        {
            for (int y = -gridSize; y <= gridSize; y++)
            {
                // Check if the current position is on the most outer layer
                WallType? currentType = GetWallType(x, y, gridSize);

                if (currentType.HasValue)
                {
                    center_cells.Add(new Vector2Int(x, y));
                }
            }
        }

        return center_cells;
    }

    public static WallType? GetWallType(int x, int y, int gridSize)
    {
        // Check if the current position is on the most outer layer
        if (Mathf.Abs(x) == gridSize || Mathf.Abs(y) == gridSize)
        {
            // Check if the current position is exactly on the corner
            if ((x == gridSize && y == gridSize) || (x == -gridSize && y == -gridSize) || (x == gridSize && y == -gridSize) || (x == -gridSize && y == gridSize))
            {
                return null; // Null for the corners
            }
            else
            {
                return WallType.Dig; // Dig for the edges
            }
        }
        else
        {
            return WallType.Floor; // Floor for the inner area
        }
    }
}