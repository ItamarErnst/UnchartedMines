using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RoomType
{
    Empty,
    Fairy,
    Ore,
    
}

[Serializable]
public class RoomConfig
{
    public string name;
    public RoomType room_type;
    public BaseWallDisplay prefab;
}

public class RoomDataProvider : MonoBehaviour
{
    public List<RoomConfig> room_config_list = new List<RoomConfig>();
    private Dictionary<RoomType, RoomConfig> room_type_to_config = new();

    private bool isReady = false;
    public static RoomDataProvider GetObject()
    {
        return GameObject.Find("RoomDataProvider").GetComponent<RoomDataProvider>();
    }
    
    void Awake()
    {
        StartCoroutine(InitializePool());
    }

    IEnumerator InitializePool()
    {
        foreach (RoomConfig config in room_config_list)
        {
            room_type_to_config.TryAdd(config.room_type, config);
        }
        
        yield return null;
        isReady = true;
    }

    public bool IsInitialized()
    {
        return isReady;
    }

    public RoomConfig GetRoomConfig(RoomType type)
    {
        if (room_type_to_config.TryGetValue(type, out RoomConfig config))
        {
            return config;
        }

        return room_config_list[0];
    }
}
