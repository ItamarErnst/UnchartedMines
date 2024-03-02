using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameController : MonoBehaviour
{
    public GridManager GridManager;
    
    void Start()
    {
        GameEvent.OnCellClick.AddListener(HandleCellClick);
        GameEvent.OnRightClickCell.AddListener(HandleRightClick);
        GameEvent.OnCameraMove.AddListener(OnCameraMove);
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
                GridManager.ReplaceWall(clickedCell,WallType.Torch);
            }
        }
    }

    void OnCameraMove()
    {
        GridManager.UpdateGridDisplay();
    }
}
