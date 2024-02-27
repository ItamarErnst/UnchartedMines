using UnityEngine;

public class WallDataProvider : MonoBehaviour
{
    public GameObject regularWallPrefab; // Add more prefabs for different wall types as needed

    public GameObject GetWallPrefab(WallType wallType)
    {
        switch (wallType)
        {
            case WallType.Regular:
                return regularWallPrefab;
            // Add more cases for other wall types
            default:
                return null; // Default to null if no matching wall type
        }
    }
}