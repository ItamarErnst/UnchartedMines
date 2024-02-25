using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameController : MonoBehaviour
{
    public CameraController camera_controller;
    public WallObjectPool wallObjectPool;
    
    private Dictionary<Vector2Int, WallDisplay> wallDisplayDict = new();
    private List<Vector2Int> displayed_walls = new List<Vector2Int>();
    
    IEnumerator Start()
    {
        while (!wallObjectPool.isReady)
        {
            yield return null;
        }
        
        InitializeMap();
        
        GameEvent.OnCellClick.AddListener(HandleCellClick);
        GameEvent.OnCameraMove.AddListener(DeactivateWallsOutsideCamera);
    }

    void InitializeMap()
    {
        foreach (Vector2Int cellPosition in camera_controller.GetCellsOnScreen())
        {
            UpdateWallDisplay(cellPosition, MapData.GetMapDataToCell(cellPosition));
        }
    }
    
    void UpdateWallDisplay(Vector2Int cell, WallData wallData)
    {
        if (!wallDisplayDict.TryGetValue(cell, out WallDisplay display))
        {
            if (wallData.wallType == WallType.Regular)
            {
                WallDisplay newDisplay = wallObjectPool.GetWallFromPool(cell);
                wallDisplayDict.Add(cell, newDisplay);
                
                newDisplay.Shrink(wallData.currentHits);
                newDisplay.gameObject.SetActive(true);
            }
        }
        else
        {
            if (wallData.wallType == WallType.Empty)
            {
                wallDisplayDict.Remove(cell);
                wallObjectPool.ReturnWallToPool(display);
            }
        }
    }
    
    void HandleCellClick(Vector2Int clickedCell)
    {
        if (MapData.GetMapData().TryGetValue(clickedCell, out WallData wallData))
        {
            if (wallData != null)
            {
                HandleWallClick(wallData);
            }
        }
    }

    void HandleWallClick(WallData wallData)
    {
        if(wallData.wallType == WallType.Empty) return;
        
        Vector2Int cell = new Vector2Int(wallData.x, wallData.y);

        if (!wallDisplayDict.TryGetValue(cell, out WallDisplay wallDisplay))
        {
            return;
        }
        
        wallDisplay.Shrink(wallData.currentHits++);

        if (wallData.currentHits >= wallData.hitsRequired)
        {
            Debug.Log($"Wall of type {wallData.wallType} at position ({wallData.x}, {wallData.y}) destroyed!");
            wallData.wallType = WallType.Empty;
            wallDisplayDict.Remove(cell);
            wallObjectPool.ReturnWallToPool(wallDisplay);
        }
        else
        {
            Debug.Log($"Wall of type {wallData.wallType} at position ({wallData.x}, {wallData.y}) hit {wallData.currentHits} times.");
        }
        
        MapData.UpdateMapData(cell,wallData);
    }
    
    void DeactivateWallsOutsideCamera()
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
                    wallDisplayDict.Remove(cell);
                    wallObjectPool.ReturnWallToPool(wall);
                }
            }
        }

        displayed_walls = currentCellsOnScreen;
    }
    
}
