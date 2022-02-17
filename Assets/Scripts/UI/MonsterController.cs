﻿using Assets.Scripts.Entities;
using Assets.Scripts.References;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    [SerializeField] private Monster monsterPrefab;
    [SerializeField] private DeckHandler deckController;

    public List<Monster> monsters { get; private set; }
    private int CardDraw;

    //Turn
    private TurnStateEnum CurrentTurnState;
    private Dictionary<TurnStateEnum, ITurnStateMachine> TurnStateMachine;

    [SerializeField] private List<TurnStateEvent> stateEvents;

    private void Awake()
    {
        monsters = new List<Monster>();
        CurrentTurnState = TurnStateEnum.PostTurn;
        TurnStateMachine = new Dictionary<TurnStateEnum, ITurnStateMachine>
        {
            { TurnStateEnum.PreTurn, new PreTurnState() },
            { TurnStateEnum.AttackTurn, new AttackTurnState() },
            { TurnStateEnum.PostTurn, new PostTurnState() }
        };

        foreach (TurnStateEnum _enum in TurnStateMachine.Keys)
        {
            TurnStateEvent _event = stateEvents.Find(x => x.StateEnum.Equals(_enum));
            if (_event != null)
                TurnStateMachine[_enum].NewStateAlert = _event.Event;
        }
    }

    public void BattleSetUp(IEnumerable<MonsterInstance> datas, List<CardData> deck, bool isWildDeck = false)
    {
        CardDraw = 0;

        foreach (Monster _monster in monsters)
        {
            Destroy(_monster);
        }

        monsters.Clear();

        int index = 0; //for loop doesn't work with IEnumerable ?
        foreach (MonsterInstance _data in datas)
        {
            Monster _monster = Instantiate(monsterPrefab, transform);
            _monster.SetUp(_data);
            monsters.Add(_monster);

            //Refactor adding listeners
            TurnStateMachine[TurnStateEnum.PreTurn].NewStateAlert.AddListener(_monster.StartTurn);
            TurnStateMachine[TurnStateEnum.PreTurn].NewStateAlert.AddListener(delegate { _monster.SetIsTurn(true); });
            TurnStateMachine[TurnStateEnum.PostTurn].NewStateAlert.AddListener(delegate { _monster.SetIsTurn(false); });

            CardDraw += _data.CardDraw - index;
            if (isWildDeck)
                deck.AddRange(_data.WildDeck);

            index++;
        }

        CardDraw = Mathf.Clamp(CardDraw, 0, Rules.HAND_MAX);
        deckController.SetCardDraw(CardDraw);
        deckController.AddCardsToDeck(deck);

        //Add end of preturn event trigger
        TurnStateMachine[TurnStateEnum.PreTurn].NewStateAlert.AddListener(EventManager.Instance.OnGetNextTurnStateTrigger);
    }

    public void GetNextTurnState()
    {
        CurrentTurnState = TurnStateMachine[CurrentTurnState].GetNextState();
        TurnStateMachine[CurrentTurnState].NewStateAlert.Invoke();
    }

    public void StartTurn()
    {
        //deckController.StartTurn(CardDraw);
        CurrentTurnState = TurnStateEnum.PreTurn;
        TurnStateMachine[CurrentTurnState].NewStateAlert.Invoke();
    }

    public bool HasMonster(Monster _monster)
    {
        return monsters.Contains(_monster);
    }

    public List<Card> GetHand() { return deckController.GetHand(); }
}
