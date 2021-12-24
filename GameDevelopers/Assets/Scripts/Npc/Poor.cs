using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poor : MonoBehaviour
{
    public int npc_index; // Номер этого NPC в общем списке
    int condition;
    public int neutralMinLimit; // Минимальное значение, которое нужно набрать, чтобы охранник был настроен нейтрально
    public int positiveMinLimit;
    public int ifNegative;
    public int ifNeutral;
    public int ifPositive;
    bool reacted; // Отреагировал ли этот npc как-либо с игроком

    Player player;
    Enemy enemy;
    void Start()
    {
        reacted = false;
        player = GameObject.Find("player").GetComponent<Player>();
        enemy = gameObject.GetComponent<Enemy>();
        GetCondition();
    }

    void GetCondition() // 0 - нейтральный, 1 - позитивный, -1 - негативный
    {
        int onPoorReactions;
        
        if (player.npc_reactions.ContainsKey(npc_index)) {
            onPoorReactions = player.npc_reactions[npc_index];
        } else
        {
            onPoorReactions = 0;
        }

        if (onPoorReactions > positiveMinLimit)
        {
            condition = 1;
        }
        else if (onPoorReactions > neutralMinLimit)
        {
            condition = 0;
        }
        else condition = -1;
    }

    public void WhatToDo()
    {
        if (!reacted)
        {
            if (condition == -1)
            {
                // Плохой сценарий, тебя начинают ловить
                enemy.have_to_catch = true;
            }
            else
            {
                // Хороший и нейтральный сценарии
                enemy.have_to_catch = false;
                enemy.ignore = true;
            }
        }
    }

    public void ActionFinished()
    {
        if (!reacted)
        {
            if (condition == -1)
            {
                player.social_status += ifNegative;
                if (player.npc_reactions.ContainsKey(npc_index))
                    player.npc_reactions[npc_index]--;
                else
                {
                    player.npc_reactions.Add(npc_index, -1);
                }
            }
            else if (condition == 0)
            {
                player.social_status += ifNeutral;
                if (!player.npc_reactions.ContainsKey(npc_index))
                    player.npc_reactions.Add(npc_index, 0);
            } else
            {
                player.social_status += ifPositive;
                if (player.npc_reactions.ContainsKey(npc_index))
                    player.npc_reactions[npc_index]++;
                else
                {
                    player.npc_reactions.Add(npc_index, 1);
                }
            }
            player.SaveGameStats();
            player.good_choice = false;
            player.can_speak = false;
            reacted = true;
        }
    }

    public void OnAttack()
    {
        if (!reacted)
        {
            condition = -1;
            WhatToDo();
        }
    }

    public void OnSpeach()
    {
        if (!reacted)
        {
            condition++;
            WhatToDo();
        }
    }
}
