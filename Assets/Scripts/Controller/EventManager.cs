using System;
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
    public event Action<Monster, Card> SelectTarget;
    public event Action<Monster> UpdateSelectedMonster;
    public event Action<Card> DiscardCard;
    public event Action GetNextTurnState;
    public event Action ResetSelected;

    public void OnSelectMonsterTrigger(Monster _monster)
    {
        SelectMonster?.Invoke(_monster);
    }

    public void OnUpdateSelectedMonsterTrigger(Monster _monster)
    {
        UpdateSelectedMonster?.Invoke(_monster);
    }

    public void OnDiscardCardTrigger(Card _card)
    {
        DiscardCard?.Invoke(_card);
    }

    public void OnGetNextTurnStateTrigger()
    {
        GetNextTurnState?.Invoke();
    }

    public void OnResetSelectedTrigger()
    {
        ResetSelected?.Invoke();
    }

    public void OnSelectTargetTrigger(Monster target, Card card)
    {
        SelectTarget?.Invoke(target, card);
    }
}
