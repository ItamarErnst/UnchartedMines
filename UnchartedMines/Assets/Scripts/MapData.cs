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

        WallData new_building = new WallData
        {
            wallType = WallType.Regular,
            x = cellPosition.x,
            y = cellPosition.y
        };
        
        if (center_cells.Contains(cellPosition))
        {
            new_building.wallType = WallType.Empty;
        }

        UpdateMapData(cellPosition,new_building);

        return new_building;
    }

    // Method to update WallData when a wall is cleared
    public static void UpdateMapData(Vector2Int cellPosition, WallData wallData)
    {
        DataMap[cellPosition] = wallData;
    }
}