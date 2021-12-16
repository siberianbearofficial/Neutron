using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    public List<GameObject> physical_walls;
    public List<Line> all_walls;
    
    void Start()
    {
        all_walls = GetAllWalls();
    }

    public List<Line> GetAllWalls()
    {
        all_walls = new List<Line>();
        foreach (GameObject wall in physical_walls)
        {
            double x1 = wall.transform.position.x, y1 = wall.transform.position.y + 3, x2 = wall.transform.position.x, y2 = wall.transform.position.y - 3;
            Line wall_line = new Line((x1, y1), (x2, y2));
            all_walls.Add(wall_line);
        }
        return all_walls;
    }
}
