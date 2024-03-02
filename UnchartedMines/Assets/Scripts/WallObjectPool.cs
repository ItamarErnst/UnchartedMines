using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class WallObjectPool : MonoBehaviour
{
    public BlockDataProvider BlockDataProvider;

    public Transform wallContainer;

    public int poolSize = 110;
    public int floorPoolSize = 110;
    public int orbPoolSize = 25;

    private Queue<BaseWallDisplay> wallPool = new Queue<BaseWallDisplay>();
    private Queue<BaseWallDisplay> floorPool = new Queue<BaseWallDisplay>();
    private Queue<BaseWallDisplay> orbPool = new Queue<BaseWallDisplay>();
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
        
        for (int i = 0; i < floorPoolSize; i++)
        {
            BaseWallDisplay floor = Instantiate(GetPrefab(WallType.Floor), Vector3.zero, Quaternion.identity,wallContainer);
            floor.gameObject.SetActive(false);
            floorPool.Enqueue(floor);
        }

        for (int i = 0; i < orbPoolSize; i++)
        {
            BaseWallDisplay orb = Instantiate(GetPrefab(WallType.Copper), Vector3.zero, Quaternion.identity,wallContainer);
            orb.gameObject.SetActive(false);
            floorPool.Enqueue(orb);
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
        else if (type == WallType.Floor || type == WallType.Torch)
        {
            return GetFloorFromPool();
        }
        else if (type == WallType.Copper)
        {
            return GetOrbFromPool();
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
    
    public BaseWallDisplay GetOrbFromPool()
    {
        if (orbPool.Count == 0)
        {
            ExpandOrbPool();
        }

        BaseWallDisplay orb = orbPool.Dequeue();
        return orb;
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
        else if (type == WallType.Copper)
        {
            orbPool.Enqueue(wall);
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
    
    void ExpandOrbPool()
    {
        BaseWallDisplay orb = Instantiate(GetPrefab(WallType.Copper), Vector3.zero, Quaternion.identity,wallContainer);
        orb.gameObject.SetActive(false);
        orbPool.Enqueue(orb);
    }

    BaseWallDisplay GetPrefab(WallType type)
    {
        return BlockDataProvider.GetConfig(type).prefab;
    }
}