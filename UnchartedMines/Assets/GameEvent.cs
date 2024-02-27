using UnityEngine;
using UnityEngine.Events;

public static class GameEvent
{
    public static UnityEvent<Vector2Int> OnCellClick = new();
    public static UnityEvent OnCameraMove = new(); // Event to signal camera movement

    public static UnityEvent ReadyToPlay = new();

}