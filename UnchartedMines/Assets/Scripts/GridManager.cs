using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GridManager : MonoBehaviour
{
    public CameraController camera_controller;
    public WallObjectPool wallObjectPool;
    
    private Dictionary<Vector2Int, BaseWallDisplay> wallDisplayDict = new();
    private List<Vector2Int> displayed_walls = new List<Vector2Int>();

    public float cellSize = 1.25f;
    
    private IEnumerator Start()
    {
        while (!wallObjectPool.IsInitialized())
        {
            yield return null;
        }
        
        InitializeMap();
    }
    
    void InitializeMap()
    {
        foreach (Vector2Int cellPosition in camera_controller.GetCellsOnScreen())
        {
            UpdateWallDisplay(cellPosition, MapData.GetMapDataToCell(cellPosition));
        }
    }

    public void UpdateWall(WallData wallData)
    {
        if(wallData.wallType == WallType.Floor) return;
        Vector2Int cell = new Vector2Int(wallData.x, wallData.y);

        if (!wallDisplayDict.TryGetValue(cell, out BaseWallDisplay wallDisplay))
        {
            return;
        }

        if (!wallDisplay.TryGetComponent(out WallDisplay dig_display))
        {
            return;
        }
        
        dig_display.Dig(++wallData.currentHits);

        if (wallData.currentHits >= wallData.hitsRequired)
        {
            ReplaceWallWithFloor(cell, wallDisplay);
            CreateWalls(cell);
        }
        else
        {
            MapData.UpdateMapData(cell,wallData);
        }
        
    }
    
    public void UpdateGridDisplay()
    {
        List<Vector2Int> currentCellsOnScreen = camera_controller.GetCellsOnScreen();
    
        foreach (Vector2Int cell in currentCellsOnScreen)
        {
            UpdateWallDisplay(cell, MapData.GetMapDataToCell(cell));
        }
        
        //Remove out of screen displays
        foreach (Vector2Int cell in displayed_walls)
        {
            if (!currentCellsOnScreen.Contains(cell))
            {
                if(wallDisplayDict.TryGetValue(cell,out BaseWallDisplay wall))
                {
                    ReturnDisplayFromCellToPool(cell, wall);
                }
            }
        }

        displayed_walls = currentCellsOnScreen;
    }
    
    void UpdateWallDisplay(Vector2Int cell, WallData wallData)
    {
        if(wallData == null) return;
        
        if (!wallDisplayDict.TryGetValue(cell, out BaseWallDisplay display))
        {
            BaseWallDisplay newDisplay = wallObjectPool.GetDisplay(wallData.wallType);
            Vector3 worldPosition = GetWorldPosition(cell);
            newDisplay.transform.position = worldPosition;
            
            wallDisplayDict.Add(cell, newDisplay);

            if (wallData.wallType == WallType.Dig)
            {
                newDisplay.SetDisplay(wallData);
            }
            
            newDisplay.gameObject.SetActive(true);
        }
    }

    void CreateWalls(Vector2Int center)
    {
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                Vector2Int position = new Vector2Int(center.x + x, center.y + y);

                if ((Mathf.Abs(x) == 1 && Mathf.Abs(y) == 1) || (x == 0 && y == 0)) //Skip corners and center
                    continue;

                WallData data = MapData.GetMapDataToCell(position);
                if (data == null)
                {
                    data = new WallData()
                    {
                        wallType = WallType.Dig,
                        currentHits = 0,
                        x = position.x,
                        y = position.y
                    };
                    
                    MapData.UpdateMapData(position,data);
                    UpdateWallDisplay(position, data);
                }

            }
        }
    }

    private void ReplaceWallWithFloor(Vector2Int cell,BaseWallDisplay wallDisplay)
    {
        ReturnDisplayFromCellToPool(cell, wallDisplay);
        
        WallData data = new WallData()
        {
            wallType = WallType.Floor,
            x = cell.x,
            y = cell.y
        };
                    
        MapData.UpdateMapData(cell,data);
        UpdateWallDisplay(cell, data);
    }

    private void ReturnDisplayFromCellToPool(Vector2Int cell,BaseWallDisplay wallDisplay)
    {
        wallDisplayDict.Remove(cell);
        wallObjectPool.ReturnWallToPool(wallDisplay,WallType.Dig);
    }
    
    public Vector2Int GetGridCell(Vector3 position)
    {
        int x = Mathf.FloorToInt(position.x / cellSize);
        int y = Mathf.FloorToInt(position.y / cellSize);

        return new Vector2Int(x, y);
    }
    
    public Vector3 GetWorldPosition(Vector2Int gridCell)
    {
        float x = gridCell.x * cellSize + cellSize / 2f;
        float y = gridCell.y * cellSize + cellSize / 2f;

        return new Vector3(x, y, 0f);
    }
}