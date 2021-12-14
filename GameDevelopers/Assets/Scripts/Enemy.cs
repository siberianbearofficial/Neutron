using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Enemy : MonoBehaviour
{
    private GameObject map;
    private GameObject player;
    private Map all_walls_ref;
    private int area_size = 5;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("player");
        map = GameObject.Find("Map");
        all_walls_ref = map.GetComponent<Map>();
    }

    // Update is called once per frame
    void Update()
    {
        foreach (((double, double), (double, double)) wall in GetAllWalls()) {
            (double, double) a_wall = wall.Item1, b_wall = wall.Item2;
            Line lineLeft = new Line((transform.position.x, transform.position.y), (transform.position.x + area_size, transform.position.y + area_size));
            Line lineRight = new Line((transform.position.x, transform.position.y), (transform.position.x + area_size, transform.position.y - area_size));
            Line wall_line = new Line(a_wall, b_wall);
            //print(lineRight.k);
            bool crosses_left = Line.Intersects(lineLeft, wall_line);
            bool crosses_right = Line.Intersects(lineRight, wall_line);
            print(crosses_right);
        }
    }

    List<((double, double), (double, double))> GetAllWalls()
    {
        List<((double, double), (double, double))> list_of_walls = new List<((double, double), (double, double))>();
        
        foreach (Line wall in all_walls_ref.GetAllWalls())
        {
            ((double, double), (double, double)) current_wall = ((wall.x1, wall.y1), (wall.x1, wall.y2));
            //print(current_wall);
            list_of_walls.Add(current_wall);
        }
        return list_of_walls;
    }
}