using Assets.Scripts.Entities;
using Assets.Scripts.Entities.Mingmings;
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

    public event Action<Mingming> SelectMingming;
    public event Action<Mingming, Card> SelectTarget;
    public event Action<Mingming> UpdateSelectedMingming;
    public event Action<Mingming> MingmingDied;
    public event Action<Card> DiscardCard;
    public event Action GetNextTurnState;
    public event Action ResetSelected;
    public event Action<PartyController> BattleOver;

    public void OnSelectMingmingTrigger(Mingming _mingming)
    {
        SelectMingming?.Invoke(_mingming);
    }

    public void OnUpdateSelectedMingmingTrigger(Mingming _mingming)
    {
        UpdateSelectedMingming?.Invoke(_mingming);
    }

    public void OnMingmingDiedTrigger(Mingming _mingming)
    {
        MingmingDied?.Invoke(_mingming);
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

    public void OnSelectTargetTrigger(Mingming target, Card card)
    {
        SelectTarget?.Invoke(target, card);
    }

    public void OnBattleOverTrigger(PartyController partyController)
    {
        BattleOver?.Invoke(partyController);
    }
}
