using UnityEngine;

public class Game : MonoBehaviour
{
    public bool game_paused;
    private void Start()
    {
        game_paused = false;
    }
    public void Pause()
    {
        game_paused = true;
    }
    public void Play()
    {
        game_paused = false;
    }
}