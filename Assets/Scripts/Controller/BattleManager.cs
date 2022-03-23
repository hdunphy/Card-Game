using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Entities;
using UnityEngine.UI;
using UnityEngine.Events;

public enum PlayerTurn { PlayerOne, PlayerTwo }

public class BattleManager : MonoBehaviour
{

    [SerializeField] private MonsterController PlayerLoader;
    [SerializeField] private MonsterController EnemyLoader;
    [SerializeField] private Button EndButton;

    [Header("Events")]
    [SerializeField] private UnityEvent OnPlayerWin;
    [SerializeField] private UnityEvent OnPlayerLose;
    [SerializeField] private UnityEvent OnCardError;
    [SerializeField] private UnityEvent OnEndTurn;

    //Change later
    public List<CardData> Deck;

    //private variables
    private MonsterController ActiveController { get { return playerTurn == PlayerTurn.PlayerTwo ? EnemyLoader : PlayerLoader; } }
    private Monster SelectedMonster;
    private PlayerTurn playerTurn;

    public static BattleManager Singleton { get; private set; }

    private void Awake()
    {
        //Singleton pattern On Awake set the singleton to this.
        //There should only be one GameLayer that can be accessed statically
        if (Singleton == null)
        {
            Singleton = this;
        }
        else
        { //if BattleManager already exists then destory this. We don't want duplicates
            Destroy(this);
        }
    }

    private void Start()
    {
        EventManager.Instance.SelectMonster += SetSelectedMonster;
        EventManager.Instance.SelectTarget += TargetSelected;
        EventManager.Instance.GetNextTurnState += GetNextTurnState;
        EventManager.Instance.ResetSelected += ResetSelected;
        EventManager.Instance.BattleOver += BattleOver;
    }

    private void OnDestroy()
    {
        EventManager.Instance.SelectMonster -= SetSelectedMonster;
        EventManager.Instance.GetNextTurnState -= GetNextTurnState;
        EventManager.Instance.ResetSelected -= ResetSelected;
        EventManager.Instance.BattleOver -= BattleOver;
    }

    public void StartBattle(IEnumerable<MonsterInstance> playerData, IEnumerable<MonsterInstance> enemyData, 
        List<CardData> playerCards, List<CardData> enemyCards)
    {
        /* -- Set up Battle -- */
        PlayerLoader.BattleSetUp(playerData, playerCards);
        EnemyLoader.BattleSetUp(enemyData, enemyCards, true);

        playerTurn = PlayerTurn.PlayerOne;

        StartCoroutine(StartTurnAfterSeconds(1));
    }

    private IEnumerator StartTurnAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        /* -- Initiate Battle -- */
        //Start players turn
        ActiveController.StartTurn();

        EndButton.enabled = true;
    }

    /// <summary>
    /// Triggered when battle has been lost
    /// </summary>
    /// <param name="_controller">The controller of the losing side</param>
    private void BattleOver(MonsterController _controller)
    {
        bool didPlayerOneWin = _controller == EnemyLoader;
        
        var _event = didPlayerOneWin ? OnPlayerWin : OnPlayerLose;
        _event?.Invoke();
        
        string message = didPlayerOneWin ? "Player 1" : "Player 2";
        EndButton.enabled = false;
        UserMessage.Instance.SendMessageToUser(message + " Has won");
        
        GameSceneController.Singleton.LoadLevelScene(2, didPlayerOneWin);
    }

    private void GetNextTurnState()
    {
        ActiveController.GetNextTurnState();
    }

    //Called from button
    public void EndTurn()
    {
        OnEndTurn?.Invoke();

        if (playerTurn == PlayerTurn.PlayerTwo)
        {
            playerTurn = PlayerTurn.PlayerOne;
        }
        else if (playerTurn == PlayerTurn.PlayerOne)
        {
            playerTurn = PlayerTurn.PlayerTwo;
        }

        ResetSelected();

        GetNextTurnState();
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
            string source = SelectedMonster?.name ?? target.name;
            UserMessage.Instance.SendMessageToUser($"{source} used {card.name} on {target.name}");
            
            StartCoroutine(card.InvokeAction(SelectedMonster, target));
        }
        else
        {
            //give message and return card to normal position
            OnCardError?.Invoke();
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
