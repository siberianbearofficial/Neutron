using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    // Смысловая нагрузка:
    public int social_status; // В самом начале игры равен нулю. Чем больше, тем лучше.
    public Dictionary<int, int> npc_reactions;
    public bool in_attack;
    public bool good_choice;
    public bool can_speak;
    public bool is_speaking;
    public int npc_to_speak;
    private int right_answer;
    private int last_npc_to_speak;
    private int npc_to_speak_index;
    public float health;

    public Save save;
    UIController ui_manager;
    private Game gameSession;

    // Controls
    public bool rightButton;
    public bool leftButton;
    public bool upButton;
    public bool downButton;

    private UIButtonInfo rightButtonUI;
    private UIButtonInfo leftButtonUI;
    private UIButtonInfo upButtonUI;
    private UIButtonInfo downButtonUI;

    public float speed;
    private Rigidbody2D rb;
    private Vector2 move;
    private List<Line> current_walls;
    private float lastHorizontalAxisVal;
    // Start is called before the first frame update
    void Start()
    {
        rightButtonUI = GameObject.Find("RightButton").GetComponent<UIButtonInfo>();
        leftButtonUI = GameObject.Find("LeftButton").GetComponent<UIButtonInfo>();
        upButtonUI = GameObject.Find("UpButton").GetComponent<UIButtonInfo>();
        downButtonUI = GameObject.Find("DownButton").GetComponent<UIButtonInfo>();

        health = 10000;
        last_npc_to_speak = -1;
        npc_to_speak = -1;
        npc_to_speak_index = -1;
        good_choice = false;
        in_attack = false;
        can_speak = false;
        is_speaking = false;

        ui_manager = Camera.main.GetComponent<UIController>();
        gameSession = Camera.main.GetComponent<Game>();

        save = Camera.main.GetComponent<Save>();
        Settings gameSettings = save.GetGameSettings();
        npc_reactions = gameSettings.GetNpcReactions();
        social_status = gameSettings.social_status;
        lastHorizontalAxisVal = 1;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (rightButtonUI.isDown) rightButton = true; else rightButton = false;
        if (leftButtonUI.isDown) leftButton = true; else leftButton = false;
        if (upButtonUI.isDown) upButton = true; else upButton = false;
        if (downButtonUI.isDown) downButton = true; else downButton = false;


        ui_manager.DisplaySocialStatus(social_status);

        move = new Vector2();
        if (!gameSession.game_paused)
        {
            float gotOnHorizontalAxis = Input.GetAxisRaw("Horizontal");
            float gotOnMobileHAxis = 0f, gotOnMobileVAxis = 0f;
            if (upButton) gotOnMobileVAxis = 1f;
            if (downButton) gotOnMobileVAxis = -1f;
            if (leftButton) gotOnMobileHAxis = -1f;
            if (rightButton) gotOnMobileHAxis = 1f;

            move = new Vector2(gotOnMobileHAxis, gotOnMobileVAxis).normalized * speed;

            //move = new Vector2(gotOnHorizontalAxis, Input.GetAxisRaw("Vertical")).normalized * speed;

            if ((gotOnMobileHAxis < 0 && lastHorizontalAxisVal > 0) || (gotOnMobileHAxis > 0 && lastHorizontalAxisVal < 0))
            {
                lastHorizontalAxisVal = gotOnMobileHAxis;
                Mirror();
            }
        }
    }
    bool started_speaking = false;
    public void FixedUpdate()
    {
        if (health < 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
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
            if (is_speaking && !started_speaking)
            {
                gameSession.Pause();
                started_speaking = true;
            }
            if (!is_speaking) started_speaking = false;
        }
    }

    private string[] GetDialogOptions()
    {
        string[] dialog_options = new string[3];
        switch (npc_to_speak_index)
        {
            case 0:
                {
                    dialog_options[0] = "Пошел вон";
                    dialog_options[1] = "Здравия желаю, товарищ полицай!";
                    dialog_options[2] = "Не бейте пж";

                    right_answer = 1;
                    break;
                }
            case 1:
                {
                    dialog_options[0] = "Проваливай, нищеброд";
                    dialog_options[1] = "Я сейчас полицию вызову!";
                    dialog_options[2] = "Мира тебе, добрый человек";

                    right_answer = 2;
                    break;
                }
            default:
                {
                    break;
                }
        }
        return dialog_options;
    }
    
    public void SaveGameStats()
    {
        save.SetSettings(social_status, npc_reactions);
        save.SaveGameSettings();
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

    public void CanSpeak(int npc_id, int npc_index)
    {
        can_speak = true;
        last_npc_to_speak = npc_to_speak;
        npc_to_speak = npc_id;
        npc_to_speak_index = npc_index;
    }

    public void Answered(int answer)
    {
        if (answer == right_answer)
        {
            // Выбрал хорошую фразу
            good_choice = true;
            is_speaking = false;
        }
    }
}
