using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Смысловая нагрузка:
    public int social_status; // В самом начале игры равен нулю. Чем больше, тем лучше.
    public Dictionary<int, int> npc_reactions;
    public bool in_attack;
    public bool good_choice;
    public bool can_speak;
    public bool is_speaking;
    private int npc_to_speak;

    private int last_npc_to_speak;

    Save save;
    UIController ui_manager;
    private Game gameSession;

    public float speed;
    private Rigidbody2D rb;
    private Vector2 move;
    private List<Line> current_walls;
    private float lastHorizontalAxisVal;
    // Start is called before the first frame update
    void Start()
    {
        last_npc_to_speak = -1;
        npc_to_speak = -1;
        good_choice = false;
        in_attack = false;
        can_speak = false;
        is_speaking = false;

        ui_manager = Camera.main.GetComponent<UIController>();
        gameSession = Camera.main.GetComponent<Game>();

        save = Camera.main.GetComponent<Save>();
        Settings gameSettings = save.GetGameSettings();
        npc_reactions = gameSettings.npc_reactions;
        social_status = gameSettings.social_status;
        lastHorizontalAxisVal = 1;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        ui_manager.DisplaySocialStatus(social_status);

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
        if (can_speak)
        {
            // Игрок находится в коллаидре какого-то npc и может произнести фразу
            if (last_npc_to_speak != npc_to_speak)
            {
                string[] dialog_options = GetDialogOptions();
                ui_manager.ShowDialogOptions(dialog_options);
                last_npc_to_speak = npc_to_speak;
            }
        }
        if (is_speaking)
        {
            gameSession.Pause();
        }
    }

    private string[] GetDialogOptions()
    {
        string[] dialog_options = new string[3];
        switch (npc_to_speak)
        {
            case 0:
                {
                    dialog_options[0] = "Пошел вон";
                    dialog_options[1] = "Здравия желаю, товарищ полицай!";
                    dialog_options[2] = "Не бейте пж";
                    break;
                }
            default:
                {
                    break;
                }
        }
        return dialog_options;
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
    }

    public void CanSpeak(int npc_index)
    {
        can_speak = true;
        last_npc_to_speak = npc_to_speak;
        npc_to_speak = npc_index;
    }
}
