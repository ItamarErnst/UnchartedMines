using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WallData
{
    public WallType wallType;
    public bool fogged = false;
    public bool in_room = false;
    public int hitsRequired = 100;
    public int currentHits = 0;
    public int x;
    public int y;
}

[System.Serializable]
public class RoomData
{
    public RoomType roomType;
    public bool fogged = false;
    public List<Vector2Int> cells;
}