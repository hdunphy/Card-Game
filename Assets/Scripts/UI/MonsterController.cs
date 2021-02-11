using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    [SerializeField] private Monster monsterPrefab;
    [SerializeField] private DeckController deckController;
    private List<Monster> monsters;

    private void Awake()
    {
        monsters = new List<Monster>();
    }

    public void BattleSetUp(IEnumerable<MonsterInstance> datas, List<CardData> _deck = null)
    {
        bool isWildDeck = _deck == null;
        List<CardData> Deck = isWildDeck ? new List<CardData>() : _deck;

        foreach (Monster _monster in monsters)
        {
            Destroy(_monster);
        }

        monsters.Clear();

        foreach (MonsterInstance _data in datas)
        {
            Monster _monster = Instantiate(monsterPrefab, transform);
            _monster.SetUp(_data);
            monsters.Add(_monster);
            if (isWildDeck)
                Deck.AddRange(_data.WildDeck);
        }

        deckController.AddCardsToDeck(Deck);
    }

    public void StartTurn()
    {
        deckController.StartTurn();

        foreach(Monster monster in monsters)
        {
            monster.StartTurn();
        }
    }

    public void EndTurn()
    {
        deckController.EndTurn();
    }

    public bool HasMonster(Monster _monster)
    {
        return monsters.Contains(_monster);
    }
}
