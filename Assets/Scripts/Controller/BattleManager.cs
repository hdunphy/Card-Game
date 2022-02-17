using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Assets.Scripts.Entities;

public enum PlayerTurn { PlayerOne, PlayerTwo }

public class BattleManager : MonoBehaviour
{

    [SerializeField] private MonsterController PlayerLoader;
    [SerializeField] private MonsterController EnemyLoader;

    //Change later
    public List<MonsterData> PlayerData;
    public List<MonsterData> EnemyData;
    public List<CardData> Deck;

    //private variables
    private MonsterController ActiveController { get { return playerTurn == PlayerTurn.PlayerTwo ? EnemyLoader : PlayerLoader; } }
    private Monster SelectedMonster;
    private Card SelectedCard;
    private PlayerTurn playerTurn;

    private void Start()
    {
        EventManager.Instance.SelectMonster += SetSelectedMonster;
        EventManager.Instance.SelectCard += SetSelectedCard;
        EventManager.Instance.GetNextTurnState += GetNextTurnState;

        StartCoroutine(StartBattle());
    }

    private void OnDestroy()
    {
        EventManager.Instance.SelectMonster -= SetSelectedMonster;
        EventManager.Instance.SelectCard -= SetSelectedCard;
        EventManager.Instance.GetNextTurnState -= GetNextTurnState;
    }

    private IEnumerator StartBattle()
    {
        /* -- Set up Battle -- */
        LoadTeams(PlayerData.Select(x => { return new MonsterInstance(x, 10); }), EnemyData.Select(x => { return new MonsterInstance(x, 10); }));

        playerTurn = PlayerTurn.PlayerOne;

        yield return new WaitForSeconds(1);

        /* -- Initiate Battle -- */
        //Start players turn
        ActiveController.StartTurn();
    }

    private void GetNextTurnState()
    {
        ActiveController.GetNextTurnState();
    }

    public void EndTurn()
    {
        if (playerTurn == PlayerTurn.PlayerTwo)
        {
            playerTurn = PlayerTurn.PlayerOne;
        }
        else if (playerTurn == PlayerTurn.PlayerOne)
        {
            playerTurn = PlayerTurn.PlayerTwo;
        }
        GetNextTurnState();
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

    public void ResetSelected()
    {
        if (SelectedCard != null) SetSelectedCard(SelectedCard);
        if (SelectedMonster != null) SetSelectedMonster(SelectedMonster);
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
