using Assets.Scripts.Entities;
using Assets.Scripts.References;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class MonsterController : MonoBehaviour
{
    [SerializeField] private Mingming monsterPrefab;
    [SerializeField] private DeckHandler deckController;

    public List<Mingming> Monsters { get; private set; }
    private int CardDraw;

    //Turn
    private TurnStateEnum CurrentTurnState;
    private Dictionary<TurnStateEnum, ITurnStateMachine> TurnStateMachine;

    [SerializeField] private List<TurnStateEvent> stateEvents;

    private void Awake()
    {
        Monsters = new List<Mingming>();
        CurrentTurnState = TurnStateEnum.PostTurn;
        TurnStateMachine = new Dictionary<TurnStateEnum, ITurnStateMachine>
        {
            { TurnStateEnum.PreTurn, new PreTurnState() },
            { TurnStateEnum.AttackTurn, new AttackTurnState() },
            { TurnStateEnum.PostTurn, new PostTurnState() },
            { TurnStateEnum.End, new EndTurnState() }
        };

        foreach (TurnStateEnum _enum in TurnStateMachine.Keys)
        {
            TurnStateEvent _event = stateEvents.Find(x => x.StateEnum.Equals(_enum));
            if (_event != null)
                TurnStateMachine[_enum].NewStateAlert = _event.Event;
        }
    }

    private void Start()
    {
        EventManager.Instance.MonsterDied += Instance_MonsterDied;
        EventManager.Instance.BattleOver += Instance_BattleOver;
    }

    private void OnDestroy()
    {
        EventManager.Instance.MonsterDied -= Instance_MonsterDied;
        EventManager.Instance.BattleOver -= Instance_BattleOver;
    }

    private void Instance_MonsterDied(Mingming _monster)
    {
        if (HasMonster(_monster))
        {
            if(!Monsters.Any(m => m.IsInPlay))
            { //if all monsters are not in play
                EventManager.Instance.OnBattleOverTrigger(this);
            }
        }
    }

    private void Instance_BattleOver(MonsterController obj)
    {
        CurrentTurnState = TurnStateEnum.End;
    }

    public void BattleSetUp(IEnumerable<MingmingInstance> datas, List<CardData> deck, bool isWildDeck = false)
    {
        CardDraw = 0;

        foreach (Mingming _monster in Monsters)
        {
            Destroy(_monster);
        }

        Monsters.Clear();

        int index = 0; //for loop doesn't work with IEnumerable ?
        foreach (MingmingInstance _data in datas)
        {
            Mingming _monster = Instantiate(monsterPrefab, transform);
            _monster.SetUp(_data);
            Monsters.Add(_monster);

            //Refactor adding listeners
            TurnStateMachine[TurnStateEnum.PreTurn].NewStateAlert.AddListener(_monster.StartTurn);
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

    public void AddListenerToTurnStateMachine(TurnStateEnum turnState, UnityAction call)
    {
        TurnStateMachine[turnState].NewStateAlert.AddListener(call);
    }

    public void RemoveListenerToTurnStateMachine(TurnStateEnum turnState, UnityAction call)
    {
        TurnStateMachine[turnState].NewStateAlert.RemoveListener(call);
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

    public bool HasMonster(Mingming _monster)
    {
        return Monsters.Contains(_monster);
    }

    public void DrawCards(int numberOfCards) => deckController.DrawCards(numberOfCards);

    public List<Card> GetHand() { return deckController.GetHand(); }
}
