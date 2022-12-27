using Assets.Scripts.Entities;
using Assets.Scripts.Entities.Interfaces;
using Assets.Scripts.Entities.Mingmings;
using Assets.Scripts.Entities.Scriptable;
using Assets.Scripts.GameScene.Controller.SceneManagement;
using Assets.Scripts.References;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PartyController : MonoBehaviour
{
    [SerializeField] private Mingming mingmingPrefab;
    [SerializeField] private DeckHandler deckController;
    [SerializeField] private bool isFacingRight;

    public List<Mingming> Mingmings { get; private set; }
    private int CardDraw;
    private IInventory _inventory;

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
                TurnStateMachine[_enum].SetEvent(_event.Event);
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
        { //Mingming is on this team
            if(!Mingmings.Any(m => m.IsInPlay))
            { //if all mingmings are not in play
                EventManager.Instance.OnBattleOverTrigger(this);
            }
        }
        else
        {
            //Add exp
            int totalXP = _mingming.Logic.GetDeathExp();
            int xpPerMingming = Mathf.CeilToInt((float)totalXP / Mingmings.Count( m => m.IsInPlay ));
            
            foreach(var mingming in Mingmings)
            {
                mingming.AddExperience(xpPerMingming);
            }

            //Add blueprints to inventory
            _inventory.AddBlueprint(_mingming.DataName, 1);
        }
    }

    private void Instance_BattleOver(PartyController obj)
    {
        CurrentTurnState = TurnStateEnum.End;
    }

    public void BattleSetUp(DevBattleSceneInfo battleSceneInfo)
    {
        var deck = battleSceneInfo.Cards.ToList();
        var datas = battleSceneInfo.Mingmings.ToList();
        _inventory = battleSceneInfo.Inventory;

        bool isWildDeck = deck.Count == 0;
        CardDraw = 0;

        foreach (Mingming _mingming in Mingmings)
        {
            Destroy(_mingming);
        }

        Mingmings.Clear();

        int index = 0; //for loop doesn't work with IEnumerable ?
        foreach (MingmingInstance _data in datas)
        {
            Mingming _mingming = Instantiate(mingmingPrefab, transform);
            _mingming.SetData(_data, isFacingRight);
            Mingmings.Add(_mingming);

            _mingming.Logic.OnStatusAdded += (status, _) => Logic_OnStatusAdded(_mingming, status);
            _mingming.Logic.OnStatusRemoved += (status) => Logic_OnStatusRemoved(_mingming, status);
            
            //Refactor adding listeners
            TurnStateMachine[TurnStateEnum.PreTurn].AddListener(_mingming.StartTurn);
            TurnStateMachine[TurnStateEnum.PostTurn].AddListener(delegate { _mingming.SetIsTurn(false); });

            CardDraw += _data.CardDraw - index;
            if (isWildDeck)
                deck.AddRange(_data.WildDeck);

            index++;
        }

        CardDraw = Mathf.Clamp(CardDraw, 0, Rules.HAND_MAX);
        deckController.SetCardDraw(CardDraw);
        deckController.AddCardsToDeck(deck);
    }

    private void Logic_OnStatusAdded(Mingming mingming, BaseStatus status)
    {
        var effectStatus = status as EffectStatus;
        if (effectStatus != null)
        {
            TurnStateMachine[effectStatus.TurnState].AddListener(effectStatus.GetEffect(mingming.Logic));
        }
    }

    private void Logic_OnStatusRemoved(Mingming mingming, BaseStatus status)
    {
        var effectStatus = status as EffectStatus;
        if (effectStatus != null)
        {
            TurnStateMachine[effectStatus.TurnState].RemoveListener(effectStatus.GetEffect(mingming.Logic));
        }
    }

    public void GetNextTurnState()
    {
        CurrentTurnState = TurnStateMachine[CurrentTurnState].GetNextState();
        TurnStateMachine[CurrentTurnState].Invoke();
    }

    public void StartTurn()
    {
        CurrentTurnState = TurnStateEnum.PreTurn;
        TurnStateMachine[CurrentTurnState].Invoke();
    }

    public void SetAsLastSibling()
    {
        transform.SetAsLastSibling();
    }

    public bool HasMingming(Mingming mingming) => Mingmings.Contains(mingming);

    public bool HasMingming(MingmingBattleLogic mingmingSimulation) => Mingmings.Any(m => m.Logic == mingmingSimulation);

    public void DrawCards(int numberOfCards) => deckController.DrawCards(numberOfCards);

    public List<Card> GetHand() { return deckController.GetHand(); }
}
