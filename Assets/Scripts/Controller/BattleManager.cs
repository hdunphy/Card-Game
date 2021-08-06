using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum BattleState { PlayerTurn, EnemyTurn }

public class BattleManager : MonoBehaviour
{

    [SerializeField] private MonsterController PlayerLoader;
    [SerializeField] private MonsterController EnemyLoader;

    //Change later
    public List<MonsterData> PlayerData;
    public List<MonsterData> EnemyData;
    public List<CardData> Deck;

    //private variables
    private MonsterController ActiveController { get { return battleState == BattleState.EnemyTurn ? EnemyLoader : PlayerLoader; } }
    private Monster SelectedMonster;
    private Card SelectedCard;
    private BattleState battleState;

    private void Start()
    {
        EventManager.Instance.SelectMonster += SetSelectedMonster;
        EventManager.Instance.SelectCard += SetSelectedCard;

        StartCoroutine(StartBattle());
    }

    private void OnDestroy()
    {
        EventManager.Instance.SelectMonster -= SetSelectedMonster;
        EventManager.Instance.SelectCard -= SetSelectedCard;
    }

    private IEnumerator StartBattle()
    {
        LoadTeams(PlayerData.Select(x => { return new MonsterInstance(x, 10); }), EnemyData.Select(x => { return new MonsterInstance(x, 10); }));

        battleState = BattleState.PlayerTurn;

        yield return new WaitForSeconds(1);

        ActiveController.StartTurn();
        //EventManager.Instance.OnNewTurnTrigger(ActiveController);
    }

    public void EndTurn()
    {
        ActiveController.EndTurn();

        if (battleState == BattleState.EnemyTurn)
        {
            battleState = BattleState.PlayerTurn;
        }
        else if (battleState == BattleState.PlayerTurn)
        {
            battleState = BattleState.EnemyTurn;
        }
        ActiveController.StartTurn();
        //EventManager.Instance.OnNewTurnTrigger(ActiveController);
    }

    public void LoadTeams(IEnumerable<MonsterInstance> playerData, IEnumerable<MonsterInstance> enemyData)
    {
        PlayerLoader.BattleSetUp(playerData, Deck);
        EnemyLoader.BattleSetUp(enemyData);
    }

    public void SetSelectedMonster(Monster _monster)
    {
        //add null check if for active controller if no one can do anything during other states
        if (ActiveController.HasMonster(_monster)) 
        {
            SelectedMonster = (Monster)SetSelectable(SelectedMonster, _monster);
            EventManager.Instance.OnUpdateSelectedMonsterTrigger(SelectedMonster);
        }
        else
        {
            if (SelectedCard != null && SelectedMonster != null)
            {
                SelectedMonster.AttackMonster(_monster, SelectedCard);
                SetSelectedCard(SelectedCard);
            }
        }
    }

    public void SetSelectedCard(Card _card)
    {
        SelectedCard = (Card)SetSelectable(SelectedCard, _card);
        EventManager.Instance.OnUpdateSelectedCardTrigger(SelectedCard);
    }

    private SelectableElement SetSelectable(SelectableElement _current, SelectableElement _selected)
    {
        SelectableElement result = _selected;

        if (_current != null)
        {
            if (_current == _selected)
            {
                result = null;
                _selected.SetSelected(false);
            }
            else
            {
                _current.SetSelected(false);
                _selected.SetSelected(true);
            }
        }
        else
        {
            _selected.SetSelected(true);
        }

        return result;
    }
}
