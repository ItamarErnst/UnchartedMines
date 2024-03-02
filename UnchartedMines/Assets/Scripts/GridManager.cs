using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GridManager : MonoBehaviour
{
    public BlockDataProvider BlockDataProvider;
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
        if(wallData.wallType != WallType.Dig) return;
        
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
            ReplaceWall(cell, WallType.Floor);
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
        
        WallType type = wallData.wallType;

        if (!wallDisplayDict.TryGetValue(cell, out BaseWallDisplay display))
        {
            BaseWallDisplay newDisplay = wallObjectPool.GetDisplay(type);
            newDisplay.ChangeColors(BlockDataProvider.GetConfig(type).prefab);
            newDisplay.SetDisplay(wallData);
            
            newDisplay.gameObject.SetActive(true);
            
            newDisplay.transform.position = GetWorldPosition(cell);
            
            wallDisplayDict.Add(cell, newDisplay);
        }
    }

    void CreateWalls(Vector2Int center)
    {
        int dig_range = 1;
        int fog_range = 2;

        for (int x = -fog_range; x <= fog_range; x++)
        {
            for (int y = -fog_range; y <= fog_range; y++)
            {
                Vector2Int position = new Vector2Int(center.x + x, center.y + y);

                WallData data = MapData.GetMapDataToCell(position);

                if (Mathf.Abs(x) <= dig_range && Mathf.Abs(y) <= dig_range
                    && !(Mathf.Abs(x) == dig_range && Mathf.Abs(y) == dig_range))
                {
                    if (data == null)
                    {
                        CreateWallDataAtCellAndUpdate(position, WallType.Dig);
                    }
                    else if (data.wallType == WallType.FogOfWall)
                    {
                        ReplaceWall(position,WallType.Dig);
                    }
                }
                else if (data == null
                         && !(Mathf.Abs(x) == fog_range && Mathf.Abs(y) == fog_range))
                {
                    CreateWallDataAtCellAndUpdate(position, WallType.FogOfWall);
                }
            }
        }
    }

    private void CreateWallDataAtCellAndUpdate(Vector2Int cell, WallType type)
    {
        WallData data = new WallData()
        {
            wallType = type,
            currentHits = 0,
            x = cell.x,
            y = cell.y
        };
        
        MapData.UpdateMapData(cell,data);
        UpdateWallDisplay(cell, data);
    }

    private void ReplaceWall(Vector2Int cell,WallType new_type)
    {
        if(!wallDisplayDict.TryGetValue(cell,out BaseWallDisplay wallDisplay)) return;
        
        ReturnDisplayFromCellToPool(cell, wallDisplay);
        
        WallData data = new WallData()
        {
            wallType = new_type,
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