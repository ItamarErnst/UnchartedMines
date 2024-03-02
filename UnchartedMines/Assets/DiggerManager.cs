using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiggerManager : MonoBehaviour
{
    private GridManager gridManager;

    public Transform digger_container;
    public Digger digger_prefab;
    
    private Dictionary<Vector2Int, Digger> cell_to_digger = new();
    private Dictionary<Digger,Vector2Int > digger_to_cell = new();
    
    public static DiggerManager GetObject()
    {
        return GameObject.Find("DiggerManager").GetComponent<DiggerManager>();
    }
    
    private void Awake()
    {
        gridManager = GridManager.GetObject();
    }

    private void Start()
    {
        GameEvent.OnDiggerMove.AddListener(UpdateDiggerCell);
        SpawnDiggerAt(new Vector2Int(-1, 0));
    }

    public void SpawnDiggerAt(Vector2Int cell)
    {
        Digger digger = Instantiate(digger_prefab, digger_container);
        digger.SetDigger(cell);
        digger.gameObject.SetActive(true);
        
        cell_to_digger.Add(cell,digger);
        digger_to_cell.Add(digger,cell);
    }

    public void UpdateDiggerCell(Digger digger)
    {
        Vector2Int last_cell = digger_to_cell[digger];
        cell_to_digger.Remove(last_cell);
        
        cell_to_digger.Add(digger.GetCell(),digger);
        digger_to_cell[digger] = digger.GetCell();
    }

    public Digger GetDiggerAtCell(Vector2Int cell)
    {
        if (cell_to_digger.TryGetValue(cell, out Digger digger))
        {
            return digger;
        }

        return null;
    }
}
