using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Policeman : MonoBehaviour
{
    public int npc_index; // Номер этого NPC в общем списке
    int condition;
    public int neutralMinLimit; // Минимальное значение, которое нужно набрать, чтобы охранник был настроен нейтрально
    public int ifNegative;
    public int ifNeutral;
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
        int onPolicemanReactions;
        
        if (player.npc_reactions.ContainsKey(npc_index)) {
            onPolicemanReactions = player.npc_reactions[npc_index];
        } else
        {
            onPolicemanReactions = 0;
        }
        if (onPolicemanReactions > neutralMinLimit)
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
                // Хороший сценарий, можно спокойно уйти
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
            }
            else
            {
                player.social_status += ifNeutral;
            }
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
            condition += 1;
            WhatToDo();
        }
    }
}
