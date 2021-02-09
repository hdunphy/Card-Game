using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum PlayerTeam { Player, Enemy }

public class BattleManager : MonoBehaviour
{

    [SerializeField] private MonsterLoader PlayerLoader;
    [SerializeField] private MonsterLoader EnemyLoader;
    [SerializeField] private HandController HandController;

    //Change later
    public List<MonsterData> PlayerData;
    public List<MonsterData> EnemyData;
    public List<CardData> Deck;
    public int cardDraw;

    //private variables
    private Monster SelectedMonster;
    private Card SelectedCard;
    private PlayerTeam CurrentTeam;

    private void Start()
    {
        EventManager.Instance.SelectMonster += SetSelectedMonster;
        EventManager.Instance.SelectCard += SetSelectedCard;

        LoadTeams(PlayerData.Select(x => { return new MonsterInstance(x, 10); }), EnemyData.Select(x => { return new MonsterInstance(x, 10); }));
        CurrentTeam = PlayerTeam.Player;

        HandController.AddCardsToDeck(Deck);
    }

    private void OnDestroy()
    {
        EventManager.Instance.SelectMonster -= SetSelectedMonster;
        EventManager.Instance.SelectCard -= SetSelectedCard;
    }

    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.Space))
            EventManager.Instance.OnDrawCardTrigger(cardDraw);
    }

    public void LoadTeams(IEnumerable<MonsterInstance> playerData, IEnumerable<MonsterInstance> enemyData)
    {
        PlayerLoader.LoadMonsters(playerData, PlayerTeam.Player);
        EnemyLoader.LoadMonsters(enemyData, PlayerTeam.Enemy);
    }

    public void SetSelectedMonster(Monster _monster)
    {
        if (_monster.Team == CurrentTeam)
        {
            SelectedMonster = (Monster)SetSelectable(SelectedMonster, _monster);
            EventManager.Instance.OnUpdateSelectedMonsterTrigger(SelectedMonster);
        }
        else
        {
            if(SelectedCard != null && SelectedMonster != null)
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
