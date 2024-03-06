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

    public GameObject display_selected;
    private Coroutine digging_corutine = null;
    
    Vector2Int path_to_target = Vector2Int.zero;

    public Animator animator;
    public ParticleSystem start_digging_pr;
    public GameObject visuals_holder;

    public bool random_path = true;
    
    private void Awake()
    {
        gridManager = GridManager.GetObject();
    }
    
    public void SetDigger(Vector2Int cell)
    {
        this.cell = cell;
        transform.position = gridManager.GetWorldPosition(cell);
    }

    public void OnSelect()
    {
        display_selected.SetActive(true);
    }
    
    public void OnDeselect()
    {
        display_selected.SetActive(false);
    }

    public void SetTargetAndGo(Vector2Int target)
    {
        if(inProgress) return;
        
        start_digging_pr.Play();
        target_cell = target;
        OnDeselect();
        digging_corutine = StartCoroutine(GotoTargetCell());
    }

    public void StopDigging()
    {
        if (digging_corutine != null)
        {
            inProgress = false;
            animator.SetBool("Digging", false);
            animator.SetBool("Walking", false);
            StopCoroutine(digging_corutine);
        }
    }

    private IEnumerator GotoTargetCell()
    {
        inProgress = true;
        
        path_to_target = CalculatePath(transform.position, gridManager.GetWorldPosition(target_cell));
        
        while (cell != target_cell)
        {
            Vector2Int direction = CalculateDirection(transform.position, gridManager.GetWorldPosition(target_cell));
            
            WallData wallData = MapData.GetMapDataToCell(cell + direction);
            if (wallData != null)
            {
                if (wallData.wallType == WallType.Floor || wallData.wallType == WallType.Torch)
                {
                    animator.SetBool("Walking", true);
                    animator.SetBool("Digging", false);
                    
                    MoveToCell(direction);
                    GameEvent.OnDiggerMove.Invoke(this);
                }
                else if (wallData.wallType == WallType.Dig || wallData.wallType == WallType.Copper)
                {
                    animator.SetBool("Digging", true);
                    animator.SetBool("Walking", false);
                    
                    GameEvent.OnDiggerDig.Invoke(cell + direction);
                }
            }
            else
            {
                inProgress = false;
                transform.position = gridManager.GetWorldPosition(target_cell);
                animator.SetBool("Digging", false);
                animator.SetBool("Walking", false);
                break;
            }
            
            yield return new WaitForSeconds(0.5f);
        }

        transform.position = gridManager.GetWorldPosition(target_cell);
        inProgress = false;

        if (random_path)
        {
            SetTargetAndGo(GetRandomCell(cell,5));
        }
    }

    private Vector2Int CalculateDirection(Vector3 currentPos, Vector3 targetPos)
    {
        Vector2Int current_path = CalculatePath(currentPos, targetPos);
        
        int xClamp = Mathf.Clamp(path_to_target.x, -1, 1);
        int yClamp = Mathf.Clamp(path_to_target.y, -1, 1);

        int xDiff = Mathf.Abs(path_to_target.x);
        int yDiff = Mathf.Abs(path_to_target.y);

        if (xDiff == yDiff && current_path.x != 0)
        {
            return new Vector2Int(xClamp, 0);
        }
        else if (xDiff > yDiff)
        {
            return (current_path.x != 0) ? 
                new Vector2Int(xClamp, 0) : new Vector2Int(0, yClamp);
        }
        else
        {
            return (current_path.y != 0) ? 
                new Vector2Int(0, yClamp) : new Vector2Int(xClamp, 0);
        }
    }

    private Vector2Int CalculatePath(Vector3 currentPos, Vector3 targetPos)
    {
        Vector2Int currentCell = gridManager.GetGridCell(currentPos);
        Vector2Int targetCell = gridManager.GetGridCell(targetPos);
        
        return new Vector2Int(targetCell.x - currentCell.x, targetCell.y - currentCell.y);
    }
    
    private void MoveToCell(Vector2Int direction)
    {
        Vector2Int nextCell = new Vector2Int(cell.x + direction.x, cell.y + direction.y);
        
        FlipToDirection(direction);
        transform.position = gridManager.GetWorldPosition(nextCell);
        
        cell = nextCell;
    }

    private void FlipToDirection(Vector2Int direction)
    {
        float flip = visuals_holder.transform.localScale.x;
        if (direction.x != 0)
        {
            flip = Mathf.Clamp(direction.x, -1, 1) * -1;
        }
        visuals_holder.transform.localScale = new Vector3(flip, 1, 0);
    }

    public Vector2Int GetCell()
    {
        return cell;
    }

    public bool IsInProgress()
    {
        return inProgress;
    }

    private Vector2Int GetRandomCell(Vector2Int cell,int range)
    {
        return new Vector2Int(cell.x + Random.Range(-range, range), cell.y + Random.Range(-range, range));
    }
}
