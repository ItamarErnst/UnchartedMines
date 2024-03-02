using UnityEngine;

public class ResourceController : MonoBehaviour
{
    public int dirt;

    public static ResourceController GetObject()
    {
        return GameObject.Find("ResourceController").GetComponent<ResourceController>();
    }

    public bool CanPurchase(int amount)
    {
        return dirt >= amount;
    }
    
    public int GetResource()
    {
        return dirt;
    }

    public void AddResource(int amount)
    {
        dirt += amount;
    }
    
    public void RemoveResource(int amount)
    {
        dirt -= amount;
    }
}