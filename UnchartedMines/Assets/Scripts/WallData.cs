﻿[System.Serializable]
public class WallData
{
    public WallType wallType;
    public bool fogged = false;
    public int hitsRequired = 100;
    public int currentHits = 0;
    public int x;
    public int y;
}