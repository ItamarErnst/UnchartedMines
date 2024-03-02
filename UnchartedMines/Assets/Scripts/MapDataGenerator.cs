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
            if (Mathf.Abs(x) == Mathf.Abs(y))
            {
                return null;
            }
            else
            {
                return WallType.FogOfWall; // Dig for the edges
            }
        }
        else
        {
            // Check if the adjacent cell is on the outer layer
            if (Mathf.Abs(x + 1) == gridSize || Mathf.Abs(x - 1) == gridSize || Mathf.Abs(y + 1) == gridSize || Mathf.Abs(y - 1) == gridSize)
            {
                if (Mathf.Abs(x) == Mathf.Abs(y))
                {
                    return WallType.FogOfWall;
                }
                return WallType.Dig; // FogOfWall for the cells one layer away from Dig
            }
            else
            {
                if (x == 0 && y == 0)
                {
                    return WallType.Torch;
                }
                
                return WallType.Floor; // Floor for the inner area
            }
        }
    }
}