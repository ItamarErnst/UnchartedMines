using System.Collections.Generic;
using UnityEngine;

public static class MapData
{
    private static Dictionary<Vector2Int, WallData> DataMap = new Dictionary<Vector2Int, WallData>();

    private static List<Vector2Int> center_cells = new List<Vector2Int>()
    {
        new Vector2Int(-2, -2), new Vector2Int(-2, -1), new Vector2Int(-2, 0), new Vector2Int(-2, 1), new Vector2Int(-2, 2),
        new Vector2Int(-1, -2), new Vector2Int(-1, -1), new Vector2Int(-1, 0), new Vector2Int(-1, 1), new Vector2Int(-1, 2),
        new Vector2Int(0, -2), new Vector2Int(0, -1), new Vector2Int(0, 0), new Vector2Int(0, 1), new Vector2Int(0, 2),
        new Vector2Int(1, -2), new Vector2Int(1, -1), new Vector2Int(1, 0), new Vector2Int(1, 1), new Vector2Int(1, 2),
        new Vector2Int(2, -2), new Vector2Int(2, -1), new Vector2Int(2, 0), new Vector2Int(2, 1), new Vector2Int(2, 2),
    };

    private static List<Vector2Int> worldEdge = new();
    
    public static Dictionary<Vector2Int, WallData> GetMapData()
    {
        return DataMap;
    }
    
    public static WallData GetMapDataToCell(Vector2Int cellPosition)
    {
        if (DataMap.TryGetValue(cellPosition, out WallData wallData))
        {
            return wallData;
        }
        
        if (center_cells.Contains(cellPosition))
        {
            WallData new_building = new WallData
            {
                wallType = WallType.Empty,
                x = cellPosition.x,
                y = cellPosition.y
            };
            
            UpdateMapData(cellPosition,new_building);
            return new_building;
        }

        return null;
    }

    // Method to update WallData when a wall is cleared
    public static void UpdateMapData(Vector2Int cellPosition, WallData wallData)
    {
        DataMap[cellPosition] = wallData;

        if (wallData.wallType == WallType.Empty)
        {
            UpdateWorldEdge(cellPosition);
        }
    }

    public static void UpdateWorldEdge(Vector2Int cell)
    {
        // If the list is empty, add the cell as both min and max
        if (worldEdge.Count == 0)
        {
            worldEdge.Add(cell);
            worldEdge.Add(cell);
            return;
        }

        // Update X values
        worldEdge[0] = new Vector2Int(Mathf.Min(worldEdge[0].x, cell.x), worldEdge[0].y);
        worldEdge[1] = new Vector2Int(Mathf.Max(worldEdge[1].x, cell.x), worldEdge[1].y);

        // Update Y values
        worldEdge[0] = new Vector2Int(worldEdge[0].x, Mathf.Min(worldEdge[0].y, cell.y));
        worldEdge[1] = new Vector2Int(worldEdge[1].x, Mathf.Max(worldEdge[1].y, cell.y));
    }


    public static List<Vector2Int> GetWorldEdge()
    {
        return worldEdge;
    }
}