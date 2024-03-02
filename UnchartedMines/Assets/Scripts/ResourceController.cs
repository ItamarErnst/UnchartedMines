using System;
using TMPro;
using UnityEngine;

public class ResourceController : MonoBehaviour
{
    public TextMeshProUGUI text_display;
    public int dirt;

    public static ResourceController GetObject()
    {
        return GameObject.Find("ResourceController").GetComponent<ResourceController>();
    }

    private void Start()
    {
        UpdateDisplay();
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
        UpdateDisplay();
    }
    
    public void RemoveResource(int amount)
    {
        dirt -= amount;
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        text_display.text = $"X{dirt}";

    }
}