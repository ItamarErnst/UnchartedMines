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
        int gridSize = 4;
        List<Vector2Int> center_cells = WallTypeGenerator.GenerateCenterCells(gridSize, new Vector2Int(0,0));

        foreach (Vector2Int cellPosition in center_cells)
        {
            WallType? currentType = WallTypeGenerator.GetWallType(cellPosition.x, cellPosition.y, gridSize);
            bool fogged = WallTypeGenerator.IsFogged(cellPosition.x, cellPosition.y, gridSize);

            if (currentType.HasValue)
            {
                WallData new_building = new WallData
                {
                    wallType = currentType.Value,
                    fogged = fogged,
                    x = cellPosition.x,
                    y = cellPosition.y
                };

                DataMap.Add(new Vector2Int(new_building.x, new_building.y), new_building);
            }
        }

        CreateEventRoom(new Vector2Int(9,9),2);
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

    public static void CreateEventRoom(Vector2Int cell,int gridSize)
    {
        List<Vector2Int> center_cells = WallTypeGenerator.GenerateCenterCells(gridSize, cell);

        foreach (Vector2Int cellPosition in center_cells)
        {
            Vector2Int normalized_cell = cellPosition - cell;
            WallType? currentType = WallTypeGenerator.GetWallType(normalized_cell.x, normalized_cell.y, gridSize, 0);

            if (currentType.HasValue)
            {
                if (currentType.Value == WallType.Torch)
                {
                    currentType = WallType.Event;
                }
                else if (Mathf.Abs(normalized_cell.x) <= 1 && Mathf.Abs(normalized_cell.y) <= 1)
                {
                    currentType = WallType.Blocked;
                }
                
                WallData new_building = new WallData
                {
                    wallType = currentType.Value,
                    fogged = true,
                    x = cellPosition.x,
                    y = cellPosition.y
                };

                DataMap.Add(new Vector2Int(new_building.x, new_building.y), new_building);
            }
        }
    }
}