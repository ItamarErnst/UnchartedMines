using UnityEngine;
using UnityEngine.Events;

public static class GameEvent
{
    public static UnityEvent<Vector2Int> OnCellClick = new();
    public static UnityEvent<Vector2Int> OnRightClickCell = new();
    public static UnityEvent<WallType> OnDigComplete = new();
    public static UnityEvent OnCameraMove = new(); // Event to signal camera movement

    public static UnityEvent<Vector2Int> OnDiggerDig = new();
    public static UnityEvent<Digger> OnDiggerMove = new();

    public static UnityEvent ReadyToPlay = new();

}