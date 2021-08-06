using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Debug.Log("instance already exists, destroying object!");
            Destroy(this);
        }
    }

    public event Action<Monster> SelectMonster;
    public event Action<Card> SelectCard;
    public event Action<Monster> UpdateSelectedMonster;
    public event Action<Card> UpdateSelectedCard;
    public event Action<Card> DiscardCard;
    public event Action<MonsterController> NewTurn;


    public void OnSelectMonsterTrigger(Monster _monster)
    {
        SelectMonster?.Invoke(_monster);
    }

    public void OnSelectCardTrigger(Card _card)
    {
        SelectCard?.Invoke(_card);
    }

    public void OnUpdateSelectedMonsterTrigger(Monster _monster)
    {
        UpdateSelectedMonster?.Invoke(_monster);
    }

    public void OnUpdateSelectedCardTrigger(Card _card)
    {
        UpdateSelectedCard?.Invoke(_card);
    }

    //public void OnDrawCardTrigger(int _cardsToDraw)
    //{
    //    DrawCards?.Invoke(_cardsToDraw);
    //}

    public void OnDiscardCardTrigger(Card _card)
    {
        DiscardCard?.Invoke(_card);
    }

    public void OnNewTurnTrigger(MonsterController activeController)
    {
        NewTurn?.Invoke(activeController);
    }
}
