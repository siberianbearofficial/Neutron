using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Enemy : MonoBehaviour
{
    private GameObject map;
    private GameObject player;
    private Map all_walls_ref;
    private int area_size = 5;
    private UIController ui_manager;
    private bool playerInCollider = false;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("player");
        map = GameObject.Find("Map");
        all_walls_ref = map.GetComponent<Map>();
        ui_manager = new UIController();
    }

    public static void print_from_other_classes(string x) { print(x); }

    private bool SeePlayer()
    {
        Line lineRight = new Line((transform.position.x, transform.position.y), (transform.position.x + area_size, transform.position.y - area_size));
        Line hypotenuse = new Line((transform.position.x + area_size, transform.position.y - area_size), (transform.position.x + area_size, transform.position.y + area_size));
        (double, double) player_A = (player.transform.position.x - player.transform.localScale.x / 2, player.transform.position.y - player.transform.localScale.y / 2);
        (double, double) player_B = (player.transform.position.x + player.transform.localScale.x / 2, player.transform.position.y + player.transform.localScale.y / 2);
        (double, double) playerCenter = Line.Center(new Line(player_A, player_B));
        Line toPlayer = new Line((transform.position.x, transform.position.y), playerCenter);
        double angleToPlayerRight = Line.Angle(toPlayer, lineRight);
        //print(angleToPlayerRight);
        (bool, (double, double)) player_intersects = Line.Intersects(toPlayer, hypotenuse);
        //if (player_intersects.Item1)
            //{
            // Будем проверять, находится игрок перед стеной или за ней, для этого проверим, пересекаются ли отрезки стен и отрезок от врага до игрока. В flag2 запишем true, если перед игроком нет ни одной стены, тогда сразу return true
            bool flag2 = false;
            List<Line> walls_to_ignore = new List<Line>();
            foreach (((double, double), (double, double)) wall in GetAllWalls())
            {
                (double, double) a_wall = wall.Item1, b_wall = wall.Item2;
                //Line lineLeft = new Line((transform.position.x, transform.position.y), (transform.position.x + area_size, transform.position.y + area_size));
                Line wall_line = new Line(a_wall, b_wall);
                Line toWallA = new Line((transform.position.x, transform.position.y), a_wall);
                Line toWallB = new Line((transform.position.x, transform.position.y), b_wall);

                if (Line.Intersects(toPlayer, wall_line).Item1)
                {
                    flag2 = true;
                }

                double angleLeft = Line.Angle(toWallA, lineRight);
                double angleRight = Line.Angle(toWallB, lineRight);

                if ((angleRight < (Mathf.PI / 2)) && (angleLeft > 0))
                {
                    // Стена попадает в обзор противника
                    // Проецируем стену на гипотенузу

                    if (angleRight < 0)
                    {
                        // Переносим точку B в пограничный случай
                        b_wall = (transform.position.x + area_size, transform.position.y - area_size);
                        toWallB = new Line((transform.position.x, transform.position.y), b_wall);
                    }
                    if (angleLeft > (Mathf.PI / 2))
                    {
                        // Переносим точку A в пограничный случай
                        a_wall = (transform.position.x + area_size, transform.position.y + area_size);
                        toWallA = new Line((transform.position.x, transform.position.y), a_wall);
                    }
                    (bool, (double, double)) a_intersection = Line.Intersects(toWallA, hypotenuse);
                    (bool, (double, double)) b_intersection = Line.Intersects(toWallB, hypotenuse);
                    walls_to_ignore.Add(new Line(a_intersection.Item2, b_intersection.Item2));
                }

            }
            if (!flag2)
            {
                // Перед игроком нет ни одной стены, значит, нет смысла проецировать их на гипотенузу, сразу возвращаем true
                return true;
            }
            bool flag = true;
            foreach (Line wall in walls_to_ignore)
            {
                double px = player_intersects.Item2.Item1, py = player_intersects.Item2.Item2;
                //print(((px, py), (wall.x1, wall.y1), (wall.x2, wall.y2)));
                if ((wall.x1 <= px && px <= wall.x2) && (wall.y2 <= py && py <= wall.y1))
                {
                    //print("Стена закрывает игрока");
                    flag = false;
                }
                if (!flag) break;
            }
            return flag;
        //}
        //return false;
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

    private void Update()
    {
        if (playerInCollider)
        {
            if (SeePlayer())
            {
                //print("Функция выдает true");
                ui_manager.DisplayEnemiesVision(true);
            }
            else
            {
                //print("Функция выдает false");
                ui_manager.DisplayEnemiesVision(false);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "player")
        {
            playerInCollider = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "player")
        {
            playerInCollider = false;
            ui_manager.DisplayEnemiesVision(false);
        }
    }
}