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
            Instantiate(currentMap, cursor.transform.position, Quaternion.identity);
            already_spawned.Add(got_pos);
        }
    }

    /*private bool Contains((float, float) what)
    {
        foreach((float, float) curr in already_spawned)
        {
            if ((Mathf.Abs(curr.Item1 - what.Item1) < 0.1f) && (Mathf.Abs(curr.Item2 - curr.Item2) < 0.1f))
            {
                return true;
            }
        }
        return false;
    }*/

    private void MoveCursor((float, float) got_pos)
    {
        cursor.transform.position = new Vector2(got_pos.Item1, got_pos.Item2);
    }

    public GameObject GetCurrentMap()
    {
        return currentMap;
    }
}