using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallObjectPool : MonoBehaviour
{
    public Transform wallContainer;
    public WallDisplay wallPrefab;
    public int poolSize = 200;

    private Queue<WallDisplay> wallPool = new Queue<WallDisplay>();
    public bool isReady = false;
    void Awake()
    {
        StartCoroutine(InitializePool());
    }

    IEnumerator InitializePool()
    {
        wallPool = new Queue<WallDisplay>();

        for (int i = 0; i < poolSize; i++)
        {
            WallDisplay wall = Instantiate(wallPrefab, Vector3.zero, Quaternion.identity,wallContainer);
            wall.gameObject.SetActive(false);
            wallPool.Enqueue(wall);
        }

        yield return null;
        isReady = true;
    }

    public WallDisplay GetWallFromPool(Vector2 position)
    {
        if (wallPool.Count == 0)
        {
            ExpandPool();
        }

        WallDisplay wall = wallPool.Dequeue();
        wall.transform.position = position;
        return wall;
    }

    public void ReturnWallToPool(WallDisplay wall)
    {
        wall.gameObject.SetActive(false);
        wallPool.Enqueue(wall);
    }

    void ExpandPool()
    {
        WallDisplay wall = Instantiate(wallPrefab, Vector3.zero, Quaternion.identity,wallContainer);
        wall.gameObject.SetActive(false);
        wallPool.Enqueue(wall);
    }
}