using Assets.Scripts.Entities;
using Assets.Scripts.References;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class PartyController : MonoBehaviour
{
    [SerializeField] private Mingming monsterPrefab;
    [SerializeField] private DeckHandler deckController;
    [SerializeField] private bool isFacingRight;

    public List<Mingming> Mingmings { get; private set; }
    private int CardDraw;

    //Turn
    private TurnStateEnum CurrentTurnState;
    private Dictionary<TurnStateEnum, ITurnStateMachine> TurnStateMachine;

    [SerializeField] private List<TurnStateEvent> stateEvents;

    private void Awake()
    {
        Mingmings = new List<Mingming>();
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
        EventManager.Instance.MingmingDied += Instance_MingmingDied;
        EventManager.Instance.BattleOver += Instance_BattleOver;
    }

    private void OnDestroy()
    {
        EventManager.Instance.MingmingDied -= Instance_MingmingDied;
        EventManager.Instance.BattleOver -= Instance_BattleOver;
    }

    private void Instance_MingmingDied(Mingming _mingming)
    {
        if (HasMingming(_mingming))
        {
            if(!Mingmings.Any(m => m.IsInPlay))
            { //if all mingmings are not in play
                EventManager.Instance.OnBattleOverTrigger(this);
            }
        }
        else
        {
            int totalXP = _mingming.GetDeathExp();
            int xpPerMingming = Mathf.CeilToInt((float)totalXP / Mingmings.Count( m => m.IsInPlay ));
            
            foreach(var mingming in Mingmings)
            {
                mingming.AddExperience(xpPerMingming);
            }
        }
    }

    private void Instance_BattleOver(PartyController obj)
    {
        CurrentTurnState = TurnStateEnum.End;
    }

    public void BattleSetUp(IEnumerable<MingmingInstance> datas, List<CardData> deck, bool isWildDeck = false)
    {
        CardDraw = 0;

        foreach (Mingming _mingming in Mingmings)
        {
            Destroy(_mingming);
        }

        Mingmings.Clear();

        int index = 0; //for loop doesn't work with IEnumerable ?
        foreach (MingmingInstance _data in datas)
        {
            Mingming _mingming = Instantiate(monsterPrefab, transform);
            _mingming.SetData(_data, isFacingRight);
            Mingmings.Add(_mingming);

            //Refactor adding listeners
            TurnStateMachine[TurnStateEnum.PreTurn].NewStateAlert.AddListener(_mingming.StartTurn);
            TurnStateMachine[TurnStateEnum.PostTurn].NewStateAlert.AddListener(delegate { _mingming.SetIsTurn(false); });

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
        CurrentTurnState = TurnStateEnum.PreTurn;
        TurnStateMachine[CurrentTurnState].NewStateAlert.Invoke();
    }

    public void SetAsLastSibling()
    {
        transform.SetAsLastSibling();
    }

    public bool HasMingming(Mingming mingming)
    {
        return Mingmings.Contains(mingming);
    }

    public void DrawCards(int numberOfCards) => deckController.DrawCards(numberOfCards);

    public List<Card> GetHand() { return deckController.GetHand(); }
}
