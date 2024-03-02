using System;
using TMPro;
using UnityEngine;

public enum ResourceType
{
    Dirt = 0,
    Copper = 1,
}

[Serializable]
public class ResourceData
{
    public ResourceType type;
    public int amount;
}

public class ResourceController : MonoBehaviour
{
    public ResourceData dirt;
    public ResourceData copper;
    
    public TextMeshProUGUI text_display;
    public TextMeshProUGUI text_display_copper;

    public static ResourceController GetObject()
    {
        return GameObject.Find("ResourceController").GetComponent<ResourceController>();
    }

    private void Start()
    {
        UpdateDisplay();
    }

    public bool CanPurchase(int amount,ResourceType type)
    {
        if (type == ResourceType.Dirt)
        {
            return dirt.amount >= amount;
        }
        else
        {
            return copper.amount >= amount;
        }
    }

    public void AddResource(int amount,ResourceType type)
    {
        if (type == ResourceType.Dirt)
        {
            dirt.amount += amount;
        }
        else
        {
            copper.amount += amount;
        }
        
        UpdateDisplay();
    }
    
    public void RemoveResource(int amount,ResourceType type)
    {
        if (type == ResourceType.Dirt)
        {
            dirt.amount -= amount;
        }
        else
        {
            copper.amount -= amount;
        }
        
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        text_display.text = $"X{dirt.amount}";
        text_display_copper.text = $"X{copper.amount}";
    }
}