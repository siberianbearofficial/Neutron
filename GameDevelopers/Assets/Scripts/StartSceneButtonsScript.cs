using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartSceneButtonsScript : MonoBehaviour
{
    Button playButton;
    void Start()
    {
        playButton = GameObject.Find("PlayButton").GetComponent<Button>();
        playButton.onClick.AddListener(OnPlayButtonClick);
    }

    void OnPlayButtonClick()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
