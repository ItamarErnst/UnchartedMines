using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeController : MonoBehaviour
{
    private ResourceController resourceController;
    public Button upgradeButton;
    public TextMeshProUGUI text_display;


    public int upgrade_cost = 50;
    private int upgrade_level = 1;
    private float multiplayer = 1.3f;

    private void Awake()
    {
        resourceController = ResourceController.GetObject();
    }
    
    private void Start()
    {
        upgradeButton.onClick.AddListener(UpgradePlayerSTR);
        text_display.text = $"PRICE X{GetPriceToLevel()}";
    }

    private void UpgradePlayerSTR()
    {
        if (resourceController.CanPurchase(GetPriceToLevel()))
        {
            resourceController.RemoveResource(GetPriceToLevel());
            PlayerStats.AddPlayerSTR();
            upgrade_level++;
            
            text_display.text = $"PRICE X{GetPriceToLevel()}";
        }
    }

    private int GetPriceToLevel()
    {
        return Mathf.RoundToInt((upgrade_cost * upgrade_level) * multiplayer);
    }
}