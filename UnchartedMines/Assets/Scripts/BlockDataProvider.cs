using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WallConfig
{
    public WallType type;
    public BaseWallDisplay prefab;
}

public class BlockDataProvider : MonoBehaviour
{
    public List<WallConfig> wall_config_list = new();
    private Dictionary<WallType, WallConfig> type_to_config = new();
    private bool isReady = false;
    
    void Awake()
    {
        StartCoroutine(InitializePool());
    }

    IEnumerator InitializePool()
    {
        foreach (WallConfig config in wall_config_list)
        {
            type_to_config.Add(config.type,config);
        }
        
        yield return null;
        isReady = true;
    }

    public bool IsInitialized()
    {
        return isReady;
    }
    
    public WallConfig GetConfig(WallType type)
    {
        if (type_to_config.TryGetValue(type, out WallConfig config))
        {
            return config;
        }

        return type_to_config[WallType.Dig];
    }

    public bool CanDig(WallType type)
    {
        return type == WallType.Dig || type == WallType.Copper;
    }
}