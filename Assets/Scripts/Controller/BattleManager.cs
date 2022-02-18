using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Assets.Scripts.Entities;
using System;

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
    private PlayerTurn playerTurn;

    private void Start()
    {
        EventManager.Instance.SelectMonster += SetSelectedMonster;
        EventManager.Instance.SelectTarget += TargetSelected;
        EventManager.Instance.GetNextTurnState += GetNextTurnState;
        EventManager.Instance.ResetSelected += ResetSelected;

        StartCoroutine(StartBattle());
    }

    private void OnDestroy()
    {
        EventManager.Instance.SelectMonster -= SetSelectedMonster;
        EventManager.Instance.GetNextTurnState -= GetNextTurnState;
        EventManager.Instance.ResetSelected -= ResetSelected;
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
        EnemyLoader.BattleSetUp(enemyData, new List<CardData>(), true);
    }

    public void SetSelectedMonster(Monster _monster)
    {
        if (ActiveController.HasMonster(_monster))
        {
            SelectedMonster = (Monster)SetSelectable(SelectedMonster, _monster);
            EventManager.Instance.OnUpdateSelectedMonsterTrigger(SelectedMonster);
        }
    }

    private void TargetSelected(Monster target, Card card)
    {
        if(card.IsValidAction(SelectedMonster, target))
        {
            card.InvokeAction(SelectedMonster, target);
            
        }
        else
        {
            //give message and return card to normal position
        }
    }

    public void ResetSelected()
    {
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
