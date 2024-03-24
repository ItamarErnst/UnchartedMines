using System.Collections.Generic;
using UnityEngine;

public static class WallTypeGenerator
{
    public static List<Vector2Int> GenerateCenterCells(int gridSize,Vector2Int center_cell)
    {
        List<Vector2Int> center_cells = new List<Vector2Int>();

        // Populate center_cells using nested loops
        for (int x = -gridSize; x <= gridSize; x++)
        {
            for (int y = -gridSize; y <= gridSize; y++)
            {
                // Check if the current position is on the most outer layer
                WallType? currentType = GetWallType(x + center_cell.x, y + center_cell.y, gridSize);

                if (currentType.HasValue)
                {
                    center_cells.Add(new Vector2Int(x + center_cell.x, y + center_cell.y));
                }
            }
        }

        return center_cells;
    }

    public static WallType? GetWallType(int x, int y, int gridSize,int layers = 2)
    {
        if (Mathf.Abs(x + layers) >= gridSize || Mathf.Abs(x - layers) >= gridSize || Mathf.Abs(y + layers) >= gridSize || Mathf.Abs(y - layers) >= gridSize)
        {
            if (Mathf.Abs(x) == Mathf.Abs(y) && Mathf.Abs(x) == gridSize && Mathf.Abs(y) == gridSize)
            {
                return null;
            }
            
            return WallType.Dig;
        }
        
        if (x == 0 && y == 0)
        {
            return WallType.Torch;
        }
        
        return WallType.Floor;
    }
    
    public static bool IsFogged(int x, int y, int gridSize)
    {
        if (Mathf.Abs(x + 1) >= gridSize || Mathf.Abs(x - 1) >= gridSize || Mathf.Abs(y + 1) >= gridSize || Mathf.Abs(y - 1) >= gridSize)
        {
            return true;
        }

        if (Mathf.Abs(x) == Mathf.Abs(y) && Mathf.Abs(y) == 2) return true;

        return false;
    }
}