using UnityEngine;
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
    private Game gameSession;
    private bool isDisplayed = false;

    private Player player;

    void Start()
    {
        gameSession = Camera.main.GetComponent<Game>();
        player = GameObject.Find("player").GetComponent<Player>();
        enemiesVisionTextbox = GameObject.Find("enemiesVisionTextbox").GetComponent<Text>();
        socialStatusTextbox = GameObject.Find("socialStatusTextbox").GetComponent<Text>();
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
        socialStatusTextbox.text = "Ваш социальный статус: " + social_status;
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
        gameSession.Play();
    }

    void OnDialogChoice1()
    {
        OnDialogChoice(1);
    }
    void OnDialogChoice2()
    {
        OnDialogChoice(2);
    }
    void OnDialogChoice3()
    {
        OnDialogChoice(3);
    }
}