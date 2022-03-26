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

    public event Action<Mingming> SelectMonster;
    public event Action<Mingming, Card> SelectTarget;
    public event Action<Mingming> UpdateSelectedMonster;
    public event Action<Mingming> MonsterDied;
    public event Action<Card> DiscardCard;
    public event Action GetNextTurnState;
    public event Action ResetSelected;
    public event Action<MonsterController> BattleOver;

    public void OnSelectMingmingTrigger(Mingming _monster)
    {
        SelectMonster?.Invoke(_monster);
    }

    public void OnUpdateSelectedMonsterTrigger(Mingming _monster)
    {
        UpdateSelectedMonster?.Invoke(_monster);
    }

    public void OnMonsterDiedTrigger(Mingming _monster)
    {
        MonsterDied?.Invoke(_monster);
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

    public void OnBattleOverTrigger(MonsterController monsterController)
    {
        BattleOver?.Invoke(monsterController);
    }
}
