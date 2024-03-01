using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallObjectPool : MonoBehaviour
{
    public BlockDataProvider BlockDataProvider;

    public Transform wallContainer;

    public int poolSize = 110;
    public int FloorpoolSize = 110;

    private Queue<BaseWallDisplay> wallPool = new Queue<BaseWallDisplay>();
    private Queue<BaseWallDisplay> floorPool = new Queue<BaseWallDisplay>();
    public bool isReady = false;
    void Awake()
    {
        StartCoroutine(InitializePool());
    }

    IEnumerator InitializePool()
    {
        while (!BlockDataProvider.IsInitialized())
        {
            yield return null;
        }
        
        wallPool = new Queue<BaseWallDisplay>();

        for (int i = 0; i < poolSize; i++)
        {
            BaseWallDisplay wall = Instantiate(GetPrefab(WallType.Dig), Vector3.zero, Quaternion.identity,wallContainer);
            wall.gameObject.SetActive(false);
            wallPool.Enqueue(wall);
        }
        
        for (int i = 0; i < FloorpoolSize; i++)
        {
            BaseWallDisplay floor = Instantiate(GetPrefab(WallType.Floor), Vector3.zero, Quaternion.identity,wallContainer);
            floor.gameObject.SetActive(false);
            floorPool.Enqueue(floor);
        }

        yield return null;
        isReady = true;
    }

    public bool IsInitialized()
    {
        return isReady;
    }
    
    public BaseWallDisplay GetDisplay(WallType type)
    {
        if (type == WallType.Dig)
        {
            return GetWallFromPool();
        }
        else if (type == WallType.Floor)
        {
            return GetFloorFromPool();
        }

        return GetWallFromPool();
    }
    
    public BaseWallDisplay GetWallFromPool()
    {
        if (wallPool.Count == 0)
        {
            ExpandWallPool();
        }

        BaseWallDisplay wall = wallPool.Dequeue();
        return wall;
    }
    
    public BaseWallDisplay GetFloorFromPool()
    {
        if (floorPool.Count == 0)
        {
            ExpandFloorPool();
        }

        BaseWallDisplay floor = floorPool.Dequeue();
        return floor;
    }

    public void ReturnWallToPool(BaseWallDisplay wall,WallType type)
    {
        wall.gameObject.SetActive(false);

        if (type == WallType.Dig)
        {
            wallPool.Enqueue(wall);
        }
        else if (type == WallType.Floor)
        {
            floorPool.Enqueue(wall);
        }
    }

    void ExpandWallPool()
    {
        BaseWallDisplay wall = Instantiate(GetPrefab(WallType.Dig), Vector3.zero, Quaternion.identity,wallContainer);
        wall.gameObject.SetActive(false);
        wallPool.Enqueue(wall);
    }
    
    void ExpandFloorPool()
    {
        BaseWallDisplay floor = Instantiate(GetPrefab(WallType.Floor), Vector3.zero, Quaternion.identity,wallContainer);
        floor.gameObject.SetActive(false);
        floorPool.Enqueue(floor);
    }

    BaseWallDisplay GetPrefab(WallType type)
    {
        return BlockDataProvider.GetConfig(type).prefab;
    }
}