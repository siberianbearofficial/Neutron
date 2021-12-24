using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    public List<GameObject> physical_walls;
    public List<Line> all_walls;

    // Для спавна npc:
    [SerializeField] private GameObject[] npc_spawn_points;
    private Vector2[] nsp;


    void Start()
    {
        all_walls = GetAllWalls();
    }

    public Vector2[] GetNsp()
    {
        nsp = new Vector2[npc_spawn_points.Length];
        for (int i = 0; i < npc_spawn_points.Length; i++)
        {
            nsp[i] = npc_spawn_points[i].transform.position;
        }
        return nsp;
    }

    public List<Line> GetAllWalls()
    {
        all_walls = new List<Line>();
        foreach (GameObject wall in physical_walls)
        {
            double x1 = wall.transform.position.x - (wall.transform.localScale.x / 2), y1 = wall.transform.position.y + (wall.transform.localScale.y / 2), x2 = wall.transform.position.x + (wall.transform.localScale.x / 2), y2 = wall.transform.position.y - (wall.transform.localScale.y / 2);
            Line top = new Line((x1, y1), (x2, y1));
            Line bottom = new Line((x1, y2), (x2, y2));
            Line left = new Line((x1, y1), (x1, y2));
            Line right = new Line((x2, y1), (x2, y2));
            //Line wall_line = new Line((x1, y1), (x2, y2));
            all_walls.Add(top);
            all_walls.Add(bottom);
            all_walls.Add(left);
            all_walls.Add(right);
        }
        return all_walls;
    }
}
