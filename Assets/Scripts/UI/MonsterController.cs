using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    [SerializeField] private Monster monsterPrefab;
    [SerializeField] private DeckHandler deckController;
    public List<Monster> monsters { get; private set; }
    private int CardDraw;

    private void Awake()
    {
        monsters = new List<Monster>();
    }

    public void BattleSetUp(IEnumerable<MonsterInstance> datas, List<CardData> _deck = null)
    {
        CardDraw = 0;

        bool isWildDeck = _deck == null;
        List<CardData> Deck = isWildDeck ? new List<CardData>() : _deck;

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

            CardDraw += _data.CardDraw - index;
            if (isWildDeck)
                Deck.AddRange(_data.WildDeck);

            index++;
        }

        CardDraw = Mathf.Clamp(CardDraw, 0, Rules.HAND_MAX);
        deckController.AddCardsToDeck(Deck);
    }

    public void StartTurn()
    {
        deckController.StartTurn(CardDraw);
        EventManager.Instance.OnNewTurnTrigger(this);
    }

    public void EndTurn()
    {
        deckController.EndTurn();
    }

    public bool HasMonster(Monster _monster)
    {
        return monsters.Contains(_monster);
    }

    public List<Card> GetHand() { return deckController.GetHand(); }
}
