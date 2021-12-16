using UnityEngine;
using UnityEngine.UI;
public class UIController
{
    private Text enemiesVisionTextbox;
    private bool isDisplayed = false;

    public UIController()
    {
        OnCreate();
    }
    public void OnCreate()
    {
        enemiesVisionTextbox = GameObject.Find("enemiesVisionTextbox").GetComponent<Text>();
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
}