using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{
    private GameObject player;
    public GameObject toCenter;
    public GameObject toRightCorner;
    public GameObject toLeftCorner;
    private Rigidbody2D rb;
    private int area_size = 5;
    private UIController ui_manager;
    private bool playerInCollider = false;
    private Player player_script;
    public int speed;
    private float deltaWaitTime;
    public float waitTime;
    public bool ignore;
    private Game gameSession;

    public int npc_index;
    private int npc_id;
    public float health;

    // Логика:
    public bool have_to_catch = false;

    // Start is called before the first frame update
    public void Start()
    {
        health = 6000;
        ignore = false;
        gameSession = Camera.main.GetComponent<Game>();
        deltaWaitTime = waitTime;
        player = GameObject.Find("player");
        player_script = player.GetComponent<Player>();
        ui_manager = Camera.main.GetComponent<UIController>();
        rb = GetComponent<Rigidbody2D>();
        npc_id = gameSession.AddNpc();
    }

    public static void print_from_other_classes(string x) { print(x); }

    private bool SeePlayer()
    {
        //Line lineRight = new Line((transform.position.x, transform.position.y), (transform.position.x + area_size, transform.position.y - area_size));
        //Line hypotenuse = new Line((transform.position.x + area_size, transform.position.y - area_size), (transform.position.x + area_size, transform.position.y + area_size));
        //Line lineRight = new Line((transform.position.x, transform.position.y), (toRightCorner.transform.position.x, toRightCorner.transform.position.y));
        Line hypotenuse = new Line((toLeftCorner.transform.position.x, toLeftCorner.transform.position.y), (toRightCorner.transform.position.x, toRightCorner.transform.position.y));
        (double, double) player_A = (player.transform.position.x - player.transform.localScale.x / 2, player.transform.position.y - player.transform.localScale.y / 2);
        (double, double) player_B = (player.transform.position.x + player.transform.localScale.x / 2, player.transform.position.y + player.transform.localScale.y / 2);
        (double, double) playerCenter = Line.Center(new Line(player_A, player_B));
        Line toPlayer = new Line((transform.position.x, transform.position.y), playerCenter);
        //double angleToPlayerRight = Line.Angle(toPlayer, lineRight);
        //print(angleToPlayerRight);
        (bool, (double, double)) player_intersects = Line.Intersects(toPlayer, hypotenuse);

        Vector2 enemyPosition = new Vector2(transform.position.x, transform.position.y);
        Vector2 rightCornerPosition = new Vector2(toRightCorner.transform.position.x, toRightCorner.transform.position.y);
        Vector2 vectorRight = rightCornerPosition - enemyPosition;
        //Vector2 leftCornerPosition = new Vector2(toLeftCorner.transform.position.x, toLeftCorner.transform.position.y);
        //Vector2 vectorLeftRight = rightCornerPosition - leftCornerPosition;
        Vector2 playerPosition = new Vector2((float)playerCenter.Item1, (float)playerCenter.Item2);
        //Vector2 vectorToPlayer = playerPosition - enemyPosition;


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

            Vector2 wallPosA = new Vector2((float)a_wall.Item1, (float)a_wall.Item2);
            Vector2 wallPosB = new Vector2((float)b_wall.Item1, (float)b_wall.Item2);
            Vector2 wallVector = wallPosB - wallPosA;

            Vector2 vectorToWallA = wallPosA - enemyPosition;
            Vector2 vectorToWallB = wallPosB - enemyPosition;

                if (Line.Intersects(toPlayer, wall_line).Item1)
                {
                    flag2 = true;
                }

            //double angleLeft = Line.Angle(toWallA, lineRight);
            //double angleRight = Line.Angle(toWallB, lineRight);
            double angleLeft = Vector2.Angle(vectorToWallA, vectorRight);
            double angleRight = Vector2.Angle(vectorToWallB, vectorRight);

                if ((angleRight < 90) && (angleLeft > 0))
                {
                    // Стена попадает в обзор противника
                    // Проецируем стену на гипотенузу

                    if (angleRight < 0)
                    {
                        // Переносим точку B в пограничный случай
                        b_wall = (transform.position.x + area_size, transform.position.y - area_size);
                        toWallB = new Line((transform.position.x, transform.position.y), b_wall);
                    }
                    if (angleLeft > 90)
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
        
        foreach (Line wall in player.GetComponent<Player>().GetAllWalls())
        {
            ((double, double), (double, double)) current_wall = ((wall.x1, wall.y1), (wall.x1, wall.y2));
            //print(current_wall);
            list_of_walls.Add(current_wall);
        }
        
        return list_of_walls;
    }

    private void FollowPlayer()
    {
        //(double, double) player_A = (player.transform.position.x - player.transform.localScale.x / 2, player.transform.position.y - player.transform.localScale.y / 2);
        //(double, double) player_B = (player.transform.position.x + player.transform.localScale.x / 2, player.transform.position.y + player.transform.localScale.y / 2);
        //(double, double) playerCenter = Line.Center(new Line(player_A, player_B));
        //Line toPlayer = new Line((transform.position.x, transform.position.y), playerCenter);
        //Line fromCenter = new Line((transform.position.x, transform.position.y), (toCenter.transform.position.x, toCenter.transform.position.y));
        //double angleToPlayer = Line.Angle(toPlayer, fromCenter);
        //double angleToPlayer2 = Vector2.Angle(new Vector2((float)toPlayer.x2, (float)toPlayer.y2) - new Vector2((float)toPlayer.x1, (float)toPlayer.y1), new Vector2((float)fromCenter.x2, (float)fromCenter.y2) - new Vector2((float)fromCenter.x1, (float)fromCenter.y1));
        //print(angleToPlayer2);
        //transform.Rotate(new Vector3(0, 0, (float) angleToPlayer * Mathf.Deg2Rad));
        rb.MovePosition(Vector2.MoveTowards(rb.position, new Vector2(player.transform.position.x, player.transform.position.y), speed * Time.fixedDeltaTime));
        deltaWaitTime = waitTime;
    }

    private bool needToMoveRandomly = false, needToMirror = false;
    private float random_x, random_y;
    private void RandomMovement()
    {
        //print(deltaWaitTime);
        if (deltaWaitTime < 0)
        {
            needToMoveRandomly = true;
            needToMirror = Random.Range(0, 2) == 0;
            bool go_right = transform.localScale.x > 0;
            if (needToMirror)
            {
                go_right = !go_right;
            }
            random_y = rb.position.y + Random.Range(-3, 4);
            if (go_right)
            {
                random_x = rb.position.x + Random.Range(0, 4);
            }
            else
            {
                random_x = rb.position.x - Random.Range(0, 4);
            }
            deltaWaitTime = waitTime;
        }
        if (needToMoveRandomly)
        {
            DoRandomMove();
        }
        deltaWaitTime -= Time.fixedDeltaTime;
    }

    private void DoRandomMove()
    {
        if (needToMirror)
        {
            needToMirror = false;
            Mirror();
        }
        rb.MovePosition(Vector2.MoveTowards(rb.position, new Vector2(random_x, random_y), speed * Time.fixedDeltaTime));
    }

    private void Mirror()
    {
        transform.localScale = new Vector3(transform.localScale.x * (-1), transform.localScale.y, transform.localScale.z);
    }

    private bool started_attack = false;
    private void PlayerAttacks()
    {
        if (!started_attack)
        {
            switch (npc_index)
            {
                case 0:
                    {
                        gameObject.GetComponent<Policeman>().OnAttack();
                        break;
                    }
                case 1:
                    {
                        gameObject.GetComponent<Poor>().OnAttack();
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
            started_attack = true;
        }
    }

    private bool started_speach;

    private void PlayerSpeaks()
    {
        if (!started_speach)
        {
            switch (npc_index)
            {
                case 0:
                    {
                        gameObject.GetComponent<Policeman>().OnSpeach();
                        break;
                    }
                case 1:
                    {
                        gameObject.GetComponent<Poor>().OnSpeach();
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
            started_speach = true;
        }
    }

    private void FixedUpdate()
    {
        if (!gameSession.game_paused)
        {
            if (health < 0)
            {
                Destroy(gameObject);
            }
            if (playerInCollider && !ignore)
            {
                if (player_script.npc_to_speak == npc_id)
                {
                    if (player_script.in_attack)
                    {
                        PlayerAttacks();
                    }
                    if (player_script.good_choice)
                    {
                        PlayerSpeaks();
                    }
                }
                if (SeePlayer())
                {
                    if (have_to_catch)
                    {
                        needToMoveRandomly = false;
                        //print("Функция выдает true");
                        //ui_manager.DisplayEnemiesVision(true);
                        FollowPlayer();
                    } else
                    {
                        RandomMovement();
                    }
                }
                else
                {
                    //print("Функция выдает false");
                    //ui_manager.DisplayEnemiesVision(false);
                    RandomMovement();
                }
            }
            else
            {
                RandomMovement();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "player")
        {
            if (!player_script.can_speak)
            {
                switch (npc_index)
                {
                    case 0:
                        {
                            gameObject.GetComponent<Policeman>().WhatToDo();
                            player_script.CanSpeak(npc_id, 0);
                            break;
                        }
                    case 1:
                        {
                            gameObject.GetComponent<Poor>().WhatToDo();
                            player_script.CanSpeak(npc_id, 1);
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }
            }
            playerInCollider = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "player")
        {
            playerInCollider = false;
            switch (npc_index)
            {
                case 0:
                    {
                        gameObject.GetComponent<Policeman>().ActionFinished();
                        break;
                    }
                case 1:
                    {
                        gameObject.GetComponent<Poor>().ActionFinished();
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
            //ui_manager.DisplayEnemiesVision(false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "player")
        {
            print("Collision with player");
            
            switch (npc_index)
            {
                case 0:
                    {
                        gameObject.GetComponent<Policeman>().ActionFinished();
                        player.GetComponent<SpriteRenderer>().color = Color.yellow;
                        StartCoroutine(restartScene());
                        break;
                    }
                case 1:
                    {
                        gameObject.GetComponent<Poor>().ActionFinished();
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }
    }

    private IEnumerator restartScene()
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}