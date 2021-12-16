using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("s"))
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x, transform.position.y - speed), speed * Time.deltaTime);
        }
        if (Input.GetKey("w"))
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x, transform.position.y + speed), speed * Time.deltaTime);
        }
        if (Input.GetKey("d"))
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x + speed, transform.position.y), speed * Time.deltaTime);
        }
        if (Input.GetKey("a"))
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x - speed, transform.position.y), speed * Time.deltaTime);
        }
    }
}
