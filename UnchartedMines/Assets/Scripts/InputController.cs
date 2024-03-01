using UnityEngine;
using UnityEngine.Events;

public class InputController : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2Int clickedCell = GetClickedCell();

            GameEvent.OnCellClick.Invoke(clickedCell);
        }
    }

    Vector2Int GetClickedCell()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

        if (hit.collider != null)
        {
            // Assuming your grid cells have a collider and are positioned at integer coordinates
            int x = Mathf.RoundToInt(hit.point.x);
            int y = Mathf.RoundToInt(hit.point.y);

            return new Vector2Int(x, y);
        }

        return Vector2Int.zero; // Return (0, 0) if no valid cell was clicked
    }
}