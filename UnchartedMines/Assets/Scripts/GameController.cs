using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameController : MonoBehaviour
{
    private AudioController audio_controller;
    private GridManager GridManager;
    private DiggerManager diggerManager;
    private ResourceController resourceController;
    private ParticleManager particleManager;

    private Digger selected_digger = null;
    public DiggerPathRenderer diggerPathRenderer;

    private void Awake()
    {
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
        }

        particleManager.PlayExplosionParticle(GridManager.GetWorldPosition(cell));
        
        audio_controller.OnDigBlock();
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
                    audio_controller.OnCantDigBlock();
                }
                else
                {
                    if(wallData.wallType == WallType.Dig || wallData.wallType == WallType.Copper)
                    {
                        audio_controller.OnDigBlock();
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
                audio_controller.OnPlaceTorch();
            }
            else if (wallData.wallType == WallType.Torch)
            {
                GridManager.ReplaceWall(clickedCell,WallType.Floor,false);
                audio_controller.OnPlaceTorch();

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
