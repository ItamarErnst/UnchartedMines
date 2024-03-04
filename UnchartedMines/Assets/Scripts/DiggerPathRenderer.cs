using System;
using UnityEngine;

public class DiggerPathRenderer : MonoBehaviour
{
    private GridManager gridManager;
    public LineRenderer lineRenderer;
    public Transform startPoint;

    public GameObject end_target;
    
    private void Awake()
    {
        gridManager = GridManager.GetObject();
    }

    void OnDisable()
    {
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, Vector3.zero);
        lineRenderer.SetPosition(1, Vector3.zero);
    }

    void Update()
    {
        if (startPoint == null)
        {
            return;
        }

        Vector2Int mousePosition = gridManager.GetGridCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        UpdateLineRenderer(gridManager.GetWorldPosition(mousePosition));
    }

    void UpdateLineRenderer(Vector3 targetPosition)
    {
        end_target.transform.position = targetPosition;

        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, startPoint.position);

        float xDifference = targetPosition.x - startPoint.position.x;
        float yDifference = targetPosition.y - startPoint.position.y;

        Vector3 middlePoint;

        if (Mathf.Abs(xDifference) >= Mathf.Abs(yDifference))
        {
            middlePoint = new Vector3(targetPosition.x, startPoint.position.y, 0f);
        }
        else
        {
            middlePoint = new Vector3(startPoint.position.x, targetPosition.y, 0f);
        }

        lineRenderer.positionCount = 3;
        lineRenderer.SetPosition(1, middlePoint);
        lineRenderer.SetPosition(2, new Vector3(targetPosition.x, targetPosition.y, 0f));
    }
}