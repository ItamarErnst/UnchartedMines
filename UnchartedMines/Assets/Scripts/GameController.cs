using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameController : MonoBehaviour
{
    public GridManager GridManager;
    public GameObject torch;
    public Transform torch_container;
    
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
                GameObject torch = Instantiate(this.torch,torch_container);
                torch.transform.position = GridManager.GetWorldPosition(clickedCell);
            }
        }
    }

    void OnCameraMove()
    {
        GridManager.UpdateGridDisplay();
    }
}
