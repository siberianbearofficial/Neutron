using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

public class MapGenerator : MonoBehaviour
{
    public GameObject[] map_prefabs;
    private GameObject cursor;
    private GameObject currentMap;
    public float map_size_x, map_size_y;
    public List<(float, float)> already_spawned;

    // Для спавна npc:
    public GameObject[] npc_prefabs;

    public void Start()
    {
        already_spawned = new List<(float, float)>();
        cursor = GameObject.Find("MapGeneratorCursor");
        SpawnMap((cursor.transform.position.x, cursor.transform.position.y));
    }
    
    private GameObject GenerateRandomMap()
    {
        return map_prefabs[Random.Range(0, map_prefabs.Length)];
    }

    public void SpawnMap((float, float) got_pos)
    {
        if (!already_spawned.Contains(got_pos))
        {
            currentMap = GenerateRandomMap();
            MoveCursor(got_pos);
            currentMap = Instantiate(currentMap, cursor.transform.position, Quaternion.identity);
            already_spawned.Add(got_pos);
            SpawnEnemy();
        }
    }

    public void SpawnEnemy()
    {
        if (Random.Range(0, 2) == 1)
        {
            Vector2[] nsp = currentMap.GetComponent<Map>().GetNsp();
            /*print((nsp[0].transform.position.x, nsp[0].transform.position.y));*/
           Instantiate(npc_prefabs[Random.Range(0, npc_prefabs.Length)], nsp[Random.Range(0, nsp.Length)], Quaternion.identity);
        }
    }

    private void MoveCursor((float, float) got_pos)
    {
        cursor.transform.position = new Vector2(got_pos.Item1, got_pos.Item2);
    }

    public GameObject GetCurrentMap()
    {
        return currentMap;
    }
}