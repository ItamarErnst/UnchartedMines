using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Digger : MonoBehaviour
{
    private GridManager gridManager;
    private Vector2Int cell = Vector2Int.one;
    public Vector2Int target_cell = Vector2Int.zero;
    private bool inProgress = false;
    private bool startWithX = false;
    private void Awake()
    {
        gridManager = GridManager.GetObject();
    }
    
    public void SetDigger(Vector2Int cell)
    {
        this.cell = cell;
        transform.position = gridManager.GetWorldPosition(cell);
    }

    public void SetTargetAndGo(Vector2Int target)
    {
        if(inProgress) return;
        target_cell = target;
        StartCoroutine(GotoTargetCell());
    }

    private IEnumerator GotoTargetCell()
    {
        inProgress = true;
        startWithX = Random.Range(0, 2) == 0;
        
        while (cell != target_cell)
        {
            Vector2Int direction = CalculateDirection(transform.position, gridManager.GetWorldPosition(target_cell));

            WallData wallData = MapData.GetMapDataToCell(cell + direction);
            if (wallData != null)
            {
                if (wallData.wallType == WallType.Floor || wallData.wallType == WallType.Torch)
                {
                    MoveToCell(direction);
                    GameEvent.OnDiggerMove.Invoke(this);
                }
                else if (wallData.wallType == WallType.Dig || wallData.wallType == WallType.Copper)
                {
                    GameEvent.OnDiggerDig.Invoke(cell + direction);
                }
            }
            else
            {
                inProgress = false;
                transform.position = gridManager.GetWorldPosition(target_cell);
                break;
            }
            
            yield return new WaitForSeconds(0.5f);
        }

        transform.position = gridManager.GetWorldPosition(target_cell);
        inProgress = false;
    }

    private Vector2Int CalculateDirection(Vector3 currentPos, Vector3 targetPos)
    {
        Vector2Int currentCell = gridManager.GetGridCell(currentPos);
        Vector2Int targetCell = gridManager.GetGridCell(targetPos);

        int xDiff = targetCell.x - currentCell.x;
        int yDiff = targetCell.y - currentCell.y;

        if (startWithX)
        {
            if (xDiff != 0)
            {
                return new Vector2Int(Mathf.Clamp(xDiff, -1, 1), 0);
            }
            else
            {
                return new Vector2Int(0, Mathf.Clamp(yDiff, -1, 1));
            }
        }
        else
        {
            if (yDiff != 0)
            {
                return new Vector2Int(0, Mathf.Clamp(yDiff, -1, 1));
            }
            else
            {
                return new Vector2Int(Mathf.Clamp(xDiff, -1, 1), 0);
            }
        }
    }

    private void MoveToCell(Vector2Int direction)
    {
        Vector2Int nextCell = new Vector2Int(cell.x + direction.x, cell.y + direction.y);
        
        transform.position = gridManager.GetWorldPosition(nextCell);
        cell = nextCell;
    }

    public Vector2Int GetCell()
    {
        return cell;
    }
}
