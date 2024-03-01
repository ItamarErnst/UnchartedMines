using System.Collections.Generic;
using UnityEngine;

public static class MapData
{
    private static Dictionary<Vector2Int, WallData> DataMap = new Dictionary<Vector2Int, WallData>();
    
    private static List<Vector2Int> worldEdge = new();
    
    public static Dictionary<Vector2Int, WallData> GetMapData()
    {
        return DataMap;
    }
    
    static MapData()
    {
        InitializeData();
    }
    
    public static WallData GetMapDataToCell(Vector2Int cellPosition)
    {
        if (DataMap.TryGetValue(cellPosition, out WallData wallData))
        {
            return wallData;
        }

        return null;
    }

    public static void UpdateMapData(Vector2Int cellPosition, WallData wallData)
    {
        DataMap[cellPosition] = wallData;

        if (wallData.wallType == WallType.Floor)
        {
            //UpdateWorldEdge(cellPosition);
        }
    }
    
    
    public static List<Vector2Int> GetWorldEdge()
    {
        return worldEdge;
    }
    
    public static void InitializeData()
    {
        int gridSize = 3;
        List<Vector2Int> center_cells = WallTypeGenerator.GenerateCenterCells(gridSize);

        foreach (Vector2Int cellPosition in center_cells)
        {
            WallType? currentType = WallTypeGenerator.GetWallType(cellPosition.x, cellPosition.y, gridSize);

            if (currentType.HasValue)
            {
                WallData new_building = new WallData
                {
                    wallType = currentType.Value,
                    x = cellPosition.x,
                    y = cellPosition.y
                };

                // Add the data to the dictionary
                DataMap.Add(new Vector2Int(new_building.x, new_building.y), new_building);
            }
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
}