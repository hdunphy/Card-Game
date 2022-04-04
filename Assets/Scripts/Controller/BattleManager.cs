using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Assets.Scripts.GameScene.Controller;
using Assets.Scripts.GameScene.Controller.SceneManagement;
using System.Linq;
using Assets.Scripts.Helpers;
using System;

public enum PlayerTurn { PlayerOne, PlayerTwo }

public class BattleManager : SingletonMonoBehavior<BattleManager>
{

    [SerializeField] private MingmingController PlayerLoader;
    [SerializeField] private MingmingController EnemyLoader;
    [SerializeField] private Button EndButton;

    [Header("Events")]
    [SerializeField] private UnityEvent OnPlayerWin;
    [SerializeField] private UnityEvent OnPlayerLose;
    [SerializeField] private UnityEvent OnCardError;
    [SerializeField] private UnityEvent OnEndTurn;

    //private variables
    private MingmingController _activeController { get { return _playerTurn == PlayerTurn.PlayerTwo ? EnemyLoader : PlayerLoader; } }
    private Mingming _selectedMingming;
    private PlayerTurn _playerTurn;
    private LevelSceneData _previousLevel;

    private void Awake()
    {
        OnAwake(this);
    }

    private void Start()
    {
        EventManager.Instance.SelectMingming += SetSelectedMingming;
        EventManager.Instance.SelectTarget += TargetSelected;
        EventManager.Instance.GetNextTurnState += GetNextTurnState;
        EventManager.Instance.ResetSelected += ResetSelected;
        EventManager.Instance.BattleOver += BattleOver;
    }

    private void OnDestroy()
    {
        EventManager.Instance.SelectMingming -= SetSelectedMingming;
        EventManager.Instance.GetNextTurnState -= GetNextTurnState;
        EventManager.Instance.ResetSelected -= ResetSelected;
        EventManager.Instance.BattleOver -= BattleOver;

        DestroySingleton();
    }

    public void StartBattle(BattleSceneData battleSceneData)
    {
        _previousLevel = battleSceneData.PreviousLevel;
        /* -- Set up Battle -- */
        PlayerLoader.BattleSetUp(battleSceneData.PlayerMingmings, battleSceneData.PlayerCards.ToList());
        EnemyLoader.BattleSetUp(battleSceneData.EnemyMingmings, battleSceneData.EnemyCards.ToList(), true);

        _playerTurn = PlayerTurn.PlayerOne;

        StartCoroutine(StartTurnAfterSeconds(1));
    }

    private IEnumerator StartTurnAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        /* -- Initiate Battle -- */
        //Start players turn
        _activeController.StartTurn();

        EndButton.enabled = true;
    }

    /// <summary>
    /// Triggered when battle has been lost
    /// </summary>
    /// <param name="_controller">The controller of the losing side</param>
    private void BattleOver(MingmingController _controller)
    {
        bool didPlayerOneWin = _controller == EnemyLoader;
        
        var _event = didPlayerOneWin ? OnPlayerWin : OnPlayerLose;
        _event?.Invoke();
        
        string message = didPlayerOneWin ? "Player 1" : "Player 2";
        EndButton.enabled = false;
        UserMessage.Instance.SendMessageToUser(message + " Has won");

        _previousLevel.DidPlayerOneWin = didPlayerOneWin;

        FindObjectOfType<EnemyController>().StopAllCoroutines();

        StartCoroutine(BattleOverCoroutine());
    }

    private IEnumerator BattleOverCoroutine()
    {
        yield return new WaitForSeconds(2);

        void unLoadAction() => GameSceneController.Singleton.SetCameraVisible(true); //TODO: is this necessary
        BaseSceneData currentScene = new BaseSceneData(gameObject.scene.name);
        GameSceneController.Singleton.SwapScenes(currentScene, _previousLevel);
    }

    private void GetNextTurnState()
    {
        _activeController.GetNextTurnState();
    }

    //Called from button
    public void EndTurn()
    {
        OnEndTurn?.Invoke();

        if (_playerTurn == PlayerTurn.PlayerTwo)
        {
            _playerTurn = PlayerTurn.PlayerOne;
        }
        else if (_playerTurn == PlayerTurn.PlayerOne)
        {
            _playerTurn = PlayerTurn.PlayerTwo;
        }

        ResetSelected();

        GetNextTurnState();
    }

    public void SetSelectedMingming(Mingming _mingming)
    {
        if (_activeController.HasMingming(_mingming))
        {
            _selectedMingming = (Mingming)SetSelectable(_selectedMingming, _mingming);
            EventManager.Instance.OnUpdateSelectedMingmingTrigger(_selectedMingming);
        }
    }

    private void TargetSelected(Mingming target, Card card)
    {
        if(card.IsValidAction(_selectedMingming, target))
        {
            string source = _selectedMingming?.name ?? target.name;
            UserMessage.Instance.SendMessageToUser($"{source} used {card.name} on {target.name}");
            
            StartCoroutine(card.InvokeAction(_selectedMingming, target));
        }
        else
        {
            //give message and return card to normal position
            OnCardError?.Invoke();
        }
    }

    public void ResetSelected()
    {
        if (_selectedMingming != null) SetSelectedMingming(_selectedMingming);
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
