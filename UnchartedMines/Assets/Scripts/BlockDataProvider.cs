using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WallConfig
{
    public WallType type;
    public WallDisplay prefab;
}

public class BlockDataProvider : MonoBehaviour
{
    public List<WallConfig> wall_config_list = new();
    public Dictionary<WallType, WallConfig> type_to_config = new();

    private void Awake()
    {
        foreach (WallConfig config in wall_config_list)
        {
            type_to_config.Add(config.type,config);
        }
    }

    public WallConfig GetConfig(WallType type)
    {
        if (type_to_config.TryGetValue(type, out WallConfig config))
        {
            return config;
        }

        return type_to_config[WallType.Wall];
    }
}