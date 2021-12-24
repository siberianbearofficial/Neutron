using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Save : MonoBehaviour
{
    private const string GAME_SETTINGS = "GAME_SETTINGS";
    Settings settings;

    [SerializeField] private int npc_count; // Общее количество возможных вариантов npc

    private void Start()
    {
        GetGameSettings();
    }

    public void SaveGameSettings()
    {
        string settings_string = JsonUtility.ToJson(settings);
        print(settings_string);
        PlayerPrefs.SetString(GAME_SETTINGS, settings_string);
        PlayerPrefs.Save();
    }

    public void SetSettings(int social_status, Dictionary<int, int> npc_reactions)
    {
        settings.social_status = social_status;
        settings.SetNpcReactions(npc_reactions);
    }

    public Settings GetGameSettings()
    {
        if (!PlayerPrefs.HasKey(GAME_SETTINGS))
        {
            SetStartSettings();
            SaveGameSettings();
        }
        string settings_string = PlayerPrefs.GetString(GAME_SETTINGS);
        settings = JsonUtility.FromJson<Settings>(settings_string);
        print(settings_string);
        if (settings.GetNpcReactions() == null)
        {
            print("Получили значение null, создаем новый словарь");
            settings.SetNpcReactions(new Dictionary<int, int>());
        }
        return settings;
    }

    private void SetStartSettings()
    {
        settings = new Settings
        {
            social_status = 0
        };
        Dictionary<int, int> npc_reactions = new Dictionary<int, int>();
        for (int i = 0; i < npc_count; i++)
        {
            npc_reactions.Add(i, 0);
        }
        settings.SetNpcReactions(npc_reactions);
    }
}

public class Settings
{
    public int social_status;
    private Dictionary<int, int> npc_reactions;
    public string[] npc_reactions_list;
    public void SetNpcReactions(Dictionary<int, int> got_npc_reactions)
    {
        npc_reactions_list = new string[got_npc_reactions.Count];
        int i = 0;
        foreach(KeyValuePair<int, int> kvp in got_npc_reactions)
        {
            npc_reactions_list[i] = (kvp.Key.ToString() + ":" + kvp.Value.ToString());
            i++;
        }
    }

    public Dictionary<int, int> GetNpcReactions()
    {
        npc_reactions = new Dictionary<int, int>();
        if (npc_reactions_list != null)
        {
            if (npc_reactions_list.Length > 0)
            {
                foreach (string kvp in npc_reactions_list)
                {
                    string[] a = kvp.Split(':');
                    npc_reactions.Add(int.Parse(a[0]), int.Parse(a[1]));
                }
            }
        }
        return npc_reactions;
    }
    public Settings() {}
}

// Список NPC по номерам:
// 0 - Policeman
// 1 - Poor