using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameController : MonoBehaviour
{
    public GridManager GridManager;
    private ResourceController resourceController;

    private void Awake()
    {
        resourceController = ResourceController.GetObject();
    }

    void Start()
    {
        GameEvent.OnCellClick.AddListener(HandleCellClick);
        GameEvent.OnRightClickCell.AddListener(HandleRightClick);
        GameEvent.OnCameraMove.AddListener(OnCameraMove);
        GameEvent.OnDigComplete.AddListener(OnDigComplete);
    }

    void OnDigComplete(WallType type)
    {
        if (type == WallType.Dig)
        {
            resourceController.AddResource(16,ResourceType.Dirt);
        }
        
        if (type == WallType.Copper)
        {
            resourceController.AddResource(3,ResourceType.Copper);
        }
    }
    
    void HandleCellClick(Vector2Int clickedCell)
    {
        if (MapData.GetMapData().TryGetValue(clickedCell, out WallData wallData))
        {
            if (wallData != null)
            {
                GridManager.UpdateWall(wallData);
            }
        }
    }

    void HandleRightClick(Vector2Int clickedCell)
    {
        if (MapData.GetMapData().TryGetValue(clickedCell, out WallData wallData))
        {
            if(wallData == null) return;
            
            if (wallData.wallType == WallType.Floor)
            {
                GridManager.ReplaceWall(clickedCell,WallType.Torch,false);
            }
        }
    }

    void OnCameraMove()
    {
        GridManager.UpdateGridDisplay();
    }
}
