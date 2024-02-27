[System.Serializable]
public class WallData
{
    public WallType wallType;
    public int hitsRequired = 5;
    public int currentHits = 0;
    public int x;
    public int y;
}