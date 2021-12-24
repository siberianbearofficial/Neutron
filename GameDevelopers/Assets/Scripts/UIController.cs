using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class UIController : MonoBehaviour
{
    private Text enemiesVisionTextbox;
    private Text socialStatusTextbox;
    private GameObject dialogOptionTextbox1;
    private GameObject dialogOptionTextbox2;
    private GameObject dialogOptionTextbox3;
    private GameObject dialogCanvas;
    private Button speakButton;
    public Button pauseButton;
    public Button playButton;
    public Button menuButton;
    public GameObject pauseCanvas;
    public GameObject mainCanvas;

    // Social status
    public Image socialStatusImg1;
    public Image socialStatusImg2;
    public Image socialStatusImg3;
    public Image socialStatusImg4;
    public Image socialStatusImg5;
    public Sprite socialStatusSpriteEmpty;
    public Sprite socialStatusSpriteHalf;
    public Sprite socialStatusSpriteFull;

    private Game gameSession;
    private bool isDisplayed = false;

    private Player player;

    void Start()
    {
        gameSession = Camera.main.GetComponent<Game>();
        player = GameObject.Find("player").GetComponent<Player>();
        //enemiesVisionTextbox = GameObject.Find("enemiesVisionTextbox").GetComponent<Text>();
        //socialStatusTextbox = GameObject.Find("socialStatusTextbox").GetComponent<Text>();
        dialogCanvas = GameObject.Find("DialogCanvas");
        dialogOptionTextbox1 = GameObject.Find("dialogOptionTextbox1");
        dialogOptionTextbox2 = GameObject.Find("dialogOptionTextbox2");
        dialogOptionTextbox3 = GameObject.Find("dialogOptionTextbox3");
        dialogOptionTextbox1.GetComponent<Button>().onClick.AddListener(OnDialogChoice1);
        dialogOptionTextbox2.GetComponent<Button>().onClick.AddListener(OnDialogChoice2);
        dialogOptionTextbox3.GetComponent<Button>().onClick.AddListener(OnDialogChoice3);
        dialogCanvas.SetActive(false);
        speakButton = GameObject.Find("speakButton").GetComponent<Button>();
        speakButton.onClick.AddListener(OnSpeakButtonClick);
        speakButton.gameObject.SetActive(false);

        pauseButton.onClick.AddListener(OnPauseButtonClick);
        playButton.onClick.AddListener(OnPlayButtonClick);
        menuButton.onClick.AddListener(OnMenuButtonClick);
    }

    private void OnPauseButtonClick()
    {
        gameSession.Pause();
        mainCanvas.SetActive(false);
        dialogCanvas.SetActive(false);
        pauseCanvas.SetActive(true);
    }
    private void OnPlayButtonClick()
    {
        gameSession.Play();
        mainCanvas.SetActive(true);
        dialogCanvas.SetActive(false);
        pauseCanvas.SetActive(false);
    }
    private void OnMenuButtonClick()
    {
        SceneManager.LoadScene("StartScene");
    }
    public void DisplayEnemiesVision(bool display)
    {
        if (display != isDisplayed)
        {
            if (display)
                enemiesVisionTextbox.text = "Замечен!";
            else enemiesVisionTextbox.text = "Вас никто не видит";
            isDisplayed = display;
        } 
    }

    public void DisplaySocialStatus(int social_status)
    {
        social_status = Mathf.Clamp(social_status, 0, 100);
        
        switch (social_status / 20)
        {
            case 0:
                {
                    socialStatusImg1.sprite = socialStatusSpriteEmpty;
                    socialStatusImg2.sprite = socialStatusSpriteEmpty;
                    socialStatusImg3.sprite = socialStatusSpriteEmpty;
                    socialStatusImg4.sprite = socialStatusSpriteEmpty;
                    socialStatusImg5.sprite = socialStatusSpriteEmpty;
                    break;
                }
            case 1:
                {
                    socialStatusImg1.sprite = socialStatusSpriteFull;
                    socialStatusImg2.sprite = socialStatusSpriteEmpty;
                    socialStatusImg3.sprite = socialStatusSpriteEmpty;
                    socialStatusImg4.sprite = socialStatusSpriteEmpty;
                    socialStatusImg5.sprite = socialStatusSpriteEmpty;
                    break;
                }
            case 2:
                {
                    socialStatusImg1.sprite = socialStatusSpriteFull;
                    socialStatusImg2.sprite = socialStatusSpriteFull;
                    socialStatusImg3.sprite = socialStatusSpriteEmpty;
                    socialStatusImg4.sprite = socialStatusSpriteEmpty;
                    socialStatusImg5.sprite = socialStatusSpriteEmpty;
                    break;
                }
            case 3:
                {
                    socialStatusImg1.sprite = socialStatusSpriteFull;
                    socialStatusImg2.sprite = socialStatusSpriteFull;
                    socialStatusImg3.sprite = socialStatusSpriteFull;
                    socialStatusImg4.sprite = socialStatusSpriteEmpty;
                    socialStatusImg5.sprite = socialStatusSpriteEmpty;
                    break;
                }
            case 4:
                {
                    socialStatusImg1.sprite = socialStatusSpriteFull;
                    socialStatusImg2.sprite = socialStatusSpriteFull;
                    socialStatusImg3.sprite = socialStatusSpriteFull;
                    socialStatusImg4.sprite = socialStatusSpriteFull;
                    socialStatusImg5.sprite = socialStatusSpriteEmpty;
                    break;
                }
            default:
                {
                    socialStatusImg1.sprite = socialStatusSpriteFull;
                    socialStatusImg2.sprite = socialStatusSpriteFull;
                    socialStatusImg3.sprite = socialStatusSpriteFull;
                    socialStatusImg4.sprite = socialStatusSpriteFull;
                    socialStatusImg5.sprite = socialStatusSpriteFull;
                    break;
                }
        }

        //socialStatusTextbox.text = "Ваш социальный статус: " + social_status;
    }

    public void ShowDialogOptions(string[] dialog_options)
    {
        speakButton.gameObject.SetActive(true);
        dialogOptionTextbox1.GetComponent<Text>().text = dialog_options[0];
        dialogOptionTextbox2.GetComponent<Text>().text = dialog_options[1];
        dialogOptionTextbox3.GetComponent<Text>().text = dialog_options[2];
    }

    void OnSpeakButtonClick()
    {
        // Открываем диалоги
        speakButton.gameObject.SetActive(false);
        dialogCanvas.SetActive(true);
        player.is_speaking = true;
    }

    void OnDialogChoice(int answer)
    {
        print(answer);
        dialogCanvas.SetActive(false);
        player.Answered(answer);
        gameSession.Play();
    }

    void OnDialogChoice1()
    {
        OnDialogChoice(0);
    }
    void OnDialogChoice2()
    {
        OnDialogChoice(1);
    }
    void OnDialogChoice3()
    {
        OnDialogChoice(2);
    }
}