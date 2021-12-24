using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public bool game_paused;
    private List<int> all_npc;
    private void Start()
    {
        all_npc = new List<int>();
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

    public int AddNpc()
    {
        int id = 0;
        if (all_npc.Count > 0)
        {
            id = all_npc[all_npc.Count - 1] + 1;
        }
        all_npc.Add(id);
        return id;
    }
}