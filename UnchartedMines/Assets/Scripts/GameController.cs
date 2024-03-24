using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameController : MonoBehaviour
{
    private AudioManager audio_manager;
    private GridManager GridManager;
    private DiggerManager diggerManager;
    private ResourceController resourceController;
    private ParticleManager particleManager;

    private Digger selected_digger = null;
    public DiggerPathRenderer diggerPathRenderer;

    private void Awake()
    {
        audio_manager = AudioManager.GetObject();
        resourceController = ResourceController.GetObject();
        GridManager = GridManager.GetObject();
        diggerManager = DiggerManager.GetObject();
        particleManager= ParticleManager.GetObject();
    }

    void Start()
    {
        GameEvent.OnCellClick.AddListener(HandleCellClick);
        GameEvent.OnDiggerDig.AddListener(DigInCell);
        GameEvent.OnRightClickCell.AddListener(HandleRightClick);
        GameEvent.OnCameraMove.AddListener(OnCameraMove);
        GameEvent.OnDigComplete.AddListener(OnDigComplete);
    }

    void OnDigComplete(WallType type,Vector2Int cell)
    {
        if (type == WallType.Dig)
        {
            resourceController.AddResource(16,ResourceType.Dirt);
        }
        
        if (type == WallType.Copper)
        {
            resourceController.AddResource(3,ResourceType.Copper);
            particleManager.InstateItem(GridManager.GetWorldPosition(cell));
        }

        particleManager.PlayExplosionParticle(GridManager.GetWorldPosition(cell));
        
        audio_manager.OnBlockDestroy(new Vector3(cell.x,cell.y));
    }
    
    void HandleCellClick(Vector2Int clickedCell)
    {
        if (selected_digger != null)
        {
            diggerPathRenderer.gameObject.SetActive(false);
            selected_digger.SetTargetAndGo(clickedCell);
            selected_digger = null;
        }
        
        if (MapData.GetMapData().TryGetValue(clickedCell, out WallData wallData))
        {
            if (wallData != null)
            {
                if (wallData.wallType == WallType.Floor || wallData.wallType == WallType.Torch)
                {
                    SelectDiggerOnCell(clickedCell);
                }
                else
                {
                    audio_manager.OnDigBlock(new Vector3(clickedCell.x,clickedCell.y));
                    GridManager.UpdateWall(wallData);
                }
            }
        }
    }
    
    void DigInCell(Vector2Int cell)
    {
        if (MapData.GetMapData().TryGetValue(cell, out WallData wallData))
        {
            if (wallData != null)
            {
                GridManager.UpdateWall(wallData);

                if (wallData.fogged)
                {
                    //audio_manager.OnCantDigBlock(new Vector3(cell.x,cell.y));
                }
                else
                {
                    if(wallData.wallType == WallType.Dig || wallData.wallType == WallType.Copper)
                    {
                        //audio_manager.OnDigBlock(new Vector3(cell.x,cell.y));
                    }
                }
            }
        }
    }

    void HandleRightClick(Vector2Int clickedCell)
    {
        if (selected_digger != null)
        {
            diggerPathRenderer.gameObject.SetActive(false);
            selected_digger.OnDeselect();
            selected_digger = null;
        }
        
        if (MapData.GetMapData().TryGetValue(clickedCell, out WallData wallData))
        {
            if(wallData == null) return;
            
            if (wallData.wallType == WallType.Floor)
            {
                GridManager.ReplaceWall(clickedCell,WallType.Torch,false);
                audio_manager.OnPlaceTorch(new Vector3(clickedCell.x,clickedCell.y));
            }
            else if (wallData.wallType == WallType.Torch)
            {
                GridManager.ReplaceWall(clickedCell,WallType.Floor,false);
                audio_manager.OnRemoveTorch(new Vector3(clickedCell.x,clickedCell.y));

            }
        }
    }

    void OnCameraMove()
    {
        GridManager.UpdateGridDisplay();
    }

    void SelectDiggerOnCell(Vector2Int cell)
    {
        selected_digger = diggerManager.GetDiggerAtCell(cell);
        if (selected_digger)
        {
            if (selected_digger.IsInProgress())
            {
                selected_digger.StopDigging();
                selected_digger = null;
            }
            else
            {
                selected_digger.OnSelect();
                diggerPathRenderer.startPoint = selected_digger.transform;
                diggerPathRenderer.gameObject.SetActive(true);
            }
        }
    }
}
