using UnityEngine;

public class WallDisplay : MonoBehaviour
{
    public void Shrink(int shrinkAmount = 1)
    {
        if (shrinkAmount <= 0)
        {
            Reset();
            return;
        }

        float scaleFactor = Mathf.Pow(0.9f, shrinkAmount);
        transform.localScale *= scaleFactor;
    }

    public void Reset()
    {
        transform.localScale = new Vector2(1,1);
    }

    public void Remove()
    {
        // Remove the visual representation (e.g., destroy the GameObject)
        Destroy(gameObject);
    }
}