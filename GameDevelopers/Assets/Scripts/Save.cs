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
        PlayerPrefs.SetString(GAME_SETTINGS, settings_string);
        PlayerPrefs.Save();
    }

    public void SetSettings(int social_status, Dictionary<int, int> npc_reactions)
    {
        settings.social_status = social_status;
        settings.npc_reactions = npc_reactions;
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
        if (settings.npc_reactions == null)
        {
            print("Получили значение null, создаем новый словарь");
            settings.npc_reactions = new Dictionary<int, int>();
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
        settings.npc_reactions = npc_reactions;
    }
}

public class Settings
{
    public int social_status;
    public Dictionary<int, int> npc_reactions;
    public Settings() {}
}

// Список NPC по номерам:
// 0 - Policeman