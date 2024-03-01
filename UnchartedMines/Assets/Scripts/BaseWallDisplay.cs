using UnityEngine;

public class BaseWallDisplay : MonoBehaviour
{
    protected WallData wallData = null;
    
    public virtual void SetDisplay(WallData data)
    {
        wallData = data;
    }
}