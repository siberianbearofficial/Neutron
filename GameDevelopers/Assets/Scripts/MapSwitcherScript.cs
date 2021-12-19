using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSwitcherScript : MonoBehaviour
{
    public int direction;
    public GameObject map_center;
    private MapGenerator mapGenerator;

    private void Start()
    {
        mapGenerator = GameObject.Find("MapGenerator").GetComponent<MapGenerator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") || collision.gameObject.name == "player")
        {
            float coord_x = map_center.transform.position.x, coord_y = map_center.transform.position.y;
            switch (direction)
            {
                case 1:
                    {
                        coord_y += mapGenerator.map_size_y;
                        break;
                    }
                case 2:
                    {
                        coord_y -= mapGenerator.map_size_y;
                        break;
                    }
                case 3:
                    {
                        coord_x += mapGenerator.map_size_x;
                        break;
                    }
                case 4:
                    {
                        coord_x -= mapGenerator.map_size_x;
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
            mapGenerator.SpawnMap((coord_x, coord_y));
        }
    }
}
