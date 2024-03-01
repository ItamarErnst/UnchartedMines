using UnityEngine;
using UnityEngine.Events;

public class InputController : MonoBehaviour
{
    public GridManager gridManager;
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2Int clickedCell = GetClickedCell();
            GameEvent.OnCellClick.Invoke(clickedCell);
        }
        
        if (Input.GetMouseButtonDown(1))
        {
            Vector2Int clickedCell = GetClickedCell();
            GameEvent.OnRightClickCell.Invoke(clickedCell);
        }
    }

    Vector2Int GetClickedCell()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

        if (hit.collider != null)
        {
            return gridManager.GetGridCell(hit.point);
        }

        return Vector2Int.zero; // Return (0, 0) if no valid cell was clicked
    }
}