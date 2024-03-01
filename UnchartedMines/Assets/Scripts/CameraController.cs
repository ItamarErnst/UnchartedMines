using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CameraController : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Camera mainCamera;
    
    private void Awake()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 moveDirection = new Vector3(horizontal, vertical, 0f);
        
        if (moveDirection != Vector3.zero)
        {
            //if (IsCameraWithinBounds(transform.position + moveDirection))
            //{
            //    transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
            //    GameEvent.OnCameraMove.Invoke();
            //}
            
            transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
            GameEvent.OnCameraMove.Invoke();
        }
    }
    
    public List<Vector2Int> GetCellsOnScreen(float cellSize = 1)
    {
        Bounds cameraBounds = CalculateCameraBounds(mainCamera);

        Vector2Int minCell = new Vector2Int(
            Mathf.FloorToInt(cameraBounds.min.x / cellSize),
            Mathf.FloorToInt(cameraBounds.min.y / cellSize)
        );

        Vector2Int maxCell = new Vector2Int(
            Mathf.CeilToInt(cameraBounds.max.x / cellSize),
            Mathf.CeilToInt(cameraBounds.max.y / cellSize)
        );

        List<Vector2Int> cellRange = new List<Vector2Int>();

        for (int x = minCell.x; x <= maxCell.x; x++)
        {
            for (int y = minCell.y; y <= maxCell.y; y++)
            {
                Vector2Int cell = new Vector2Int(x, y);
                if (!cellRange.Contains(cell))
                {
                    cellRange.Add(cell);
                }
            }
        }

        return cellRange;
    }

    Bounds CalculateCameraBounds(Camera camera)
    {
        float cameraDistance = Mathf.Abs(camera.transform.position.z);

        // Calculate the bounds of the camera's view in world coordinates
        float top = camera.ScreenToWorldPoint(new Vector3(0, camera.pixelHeight, cameraDistance)).y;
        float bottom = camera.ScreenToWorldPoint(new Vector3(0, 0, cameraDistance)).y;
        float left = camera.ScreenToWorldPoint(new Vector3(0, 0, cameraDistance)).x;
        float right = camera.ScreenToWorldPoint(new Vector3(camera.pixelWidth, 0, cameraDistance)).x;

        return new Bounds(new Vector3((left + right) / 2f, (top + bottom) / 2f, 0f),
            new Vector3(right - left, top - bottom, float.MaxValue));
    }
    
    private bool IsCameraWithinBounds(Vector3 cameraPosition)
    {
        Vector2Int cameraPosition2D = new Vector2Int((int)cameraPosition.x, (int)cameraPosition.y);

        return (cameraPosition2D.x >= MapData.GetWorldEdge()[0].x && cameraPosition2D.x <= MapData.GetWorldEdge()[1].x &&
                cameraPosition2D.y >= MapData.GetWorldEdge()[0].y && cameraPosition2D.y <= MapData.GetWorldEdge()[1].y);
    }
}