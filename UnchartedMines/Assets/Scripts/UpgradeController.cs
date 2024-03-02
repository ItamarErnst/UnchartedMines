using System;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeController : MonoBehaviour
{
    private ResourceController resourceController;
    public Button upgradeButton;

    public int upgrade_cost = 50;
    private int upgrade_level = 1;

    private void Awake()
    {
        resourceController = ResourceController.GetObject();
    }
    
    private void Start()
    {
        upgradeButton.onClick.AddListener(UpgradePlayerSTR);
    }

    private void UpgradePlayerSTR()
    {
        if (resourceController.CanPurchase(upgrade_cost * upgrade_level))
        {
            resourceController.RemoveResource(upgrade_cost * upgrade_level);
            PlayerStats.AddPlayerSTR();
            upgrade_level++;
            Debug.LogError("UPGRADE " + PlayerStats.GetPlayerSTR());
        }
        Debug.LogError(resourceController.GetResource());
    }
}