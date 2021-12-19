using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;
    private Rigidbody2D rb;
    private Vector2 move;
    private List<Line> current_walls;
    private float lastHorizontalAxisVal;
    // Start is called before the first frame update
    void Start()
    {
        lastHorizontalAxisVal = 1;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        /*if (Input.GetKey("s"))
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
        }*/

        float gotOnHorizontalAxis = Input.GetAxisRaw("Horizontal");
        move = new Vector2(gotOnHorizontalAxis, Input.GetAxisRaw("Vertical")).normalized * speed;
        
        if ((gotOnHorizontalAxis < 0 && lastHorizontalAxisVal > 0) || (gotOnHorizontalAxis > 0 && lastHorizontalAxisVal < 0))
        {
            lastHorizontalAxisVal = gotOnHorizontalAxis;
            Mirror();
        }
    }
    public void FixedUpdate()
    {
        rb.MovePosition(rb.position + move * Time.fixedDeltaTime);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Map"))
        {
            current_walls = collision.gameObject.GetComponent<Map>().GetAllWalls();
        }
    }

    public List<Line> GetAllWalls()
    {
        return current_walls;
    }

    private void Mirror()
    {
        transform.localScale = new Vector3(transform.localScale.x * (-1), transform.localScale.y, transform.localScale.z);
        //SpriteRenderer sr = gameObject.GetComponent<SpriteRenderer>();
        //sr.flipX = !sr.flipX;
    }
}
