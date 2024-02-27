using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public BlockDataProvider BlockDataProvider;
    public CameraController camera_controller;
    public WallObjectPool wallObjectPool;
    
    private Dictionary<Vector2Int, WallDisplay> wallDisplayDict = new();
    private List<Vector2Int> displayed_walls = new List<Vector2Int>();

    private IEnumerator Start()
    {
        while (!wallObjectPool.isReady)
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
        if(wallData.wallType == WallType.Empty) return;
        
        Vector2Int cell = new Vector2Int(wallData.x, wallData.y);

        if (!wallDisplayDict.TryGetValue(cell, out WallDisplay wallDisplay))
        {
            return;
        }
        
        wallDisplay.SetAsWall();
        wallDisplay.Shrink(wallData.currentHits++);

        if (wallData.currentHits >= wallData.hitsRequired)
        {
            Debug.Log($"Wall of type {wallData.wallType} at position ({wallData.x}, {wallData.y}) destroyed!");
            wallData.wallType = WallType.Empty;
            wallDisplay.SetAsEmpty();
            //wallDisplayDict.Remove(cell);
            //wallObjectPool.ReturnWallToPool(wallDisplay);

            CreateWalls(cell);
        }
        else
        {
            Debug.Log($"Wall of type {wallData.wallType} at position ({wallData.x}, {wallData.y}) hit {wallData.currentHits} times.");
        }
        
        MapData.UpdateMapData(cell,wallData);
    }
    
    public void UpdateGridDisplay()
    {
        List<Vector2Int> currentCellsOnScreen = camera_controller.GetCellsOnScreen();
    
        foreach (Vector2Int cell in currentCellsOnScreen)
        {
            UpdateWallDisplay(cell, MapData.GetMapDataToCell(cell));
        }

        foreach (Vector2Int cell in displayed_walls)
        {
            if (!currentCellsOnScreen.Contains(cell))
            {
                if(wallDisplayDict.TryGetValue(cell,out WallDisplay wall))
                {
                    wall.SetAsEmpty();
                    //wallDisplayDict.Remove(cell);
                    //wallObjectPool.ReturnWallToPool(wall);
                }
            }
        }

        displayed_walls = currentCellsOnScreen;
    }
    
    void UpdateWallDisplay(Vector2Int cell, WallData wallData)
    {
        if(wallData == null) return;
        
        if (!wallDisplayDict.TryGetValue(cell, out WallDisplay display))
        {
            WallDisplay newDisplay = wallObjectPool.GetWallFromPool(cell);
            wallDisplayDict.Add(cell, newDisplay);

            if (wallData.wallType == WallType.Wall)
            {
                newDisplay.SetAsWall();
                newDisplay.Shrink(wallData.currentHits);
            }
            else
            {
                newDisplay.SetAsEmpty();
            }
            
            newDisplay.gameObject.SetActive(true);
        }
        else
        {
            //if (wallData.wallType == WallType.Empty)
            //{
            //    wallDisplayDict.Remove(cell);
            //    wallObjectPool.ReturnWallToPool(display);
            //}
        }
    }

    void CreateWalls(Vector2Int center)
    {
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                Vector2Int position = new Vector2Int(center.x + x, center.y + y);

                if (position == center)
                    continue;

                WallData data = MapData.GetMapDataToCell(position);
                if (data == null)
                {
                    data = new WallData()
                    {
                        wallType = WallType.Wall,
                        x = position.x,
                        y = position.y
                    };
                    
                    MapData.UpdateMapData(position,data);
                    UpdateWallDisplay(position, data);
                }

            }
        }
    }
}