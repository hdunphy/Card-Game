using Assets.Scripts.Entities;
using Assets.Scripts.Entities.Drops;
using Assets.Scripts.References;
using Assets.Scripts.UI.Controller;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class RandomEncounter : MonoBehaviour, IEncounter
{
    [Header("Drop Table")]
    [SerializeField] private DropTableInstance DropTable;
    [SerializeField, Range(0, 3)] private int MaxMonsters;
    [SerializeField, Range(1, 98)] private int MinMonsterLevel;
    [SerializeField, Range(2, 99)] private int MaxMonsterLevel;

    [Header("Components")]
    [SerializeField] private RewardsController RewardsController;

    [Header("Events")]
    [SerializeField] private UnityEvent OnStartEncounter;

    List<MonsterInstance> monsters;

    private void Start()
    {
        monsters = new List<MonsterInstance>();
    }

    public void GetEncounter()
    {
        monsters.Clear();

        int monsterSpawns = Rules.GetRandomInt(0, MaxMonsters + 1);
        int monsterLevel = Rules.GetRandomInt(MinMonsterLevel, MaxMonsterLevel + 1);

        for (int i = 0; i < monsterSpawns; i++)
        {
            var _drop = DropTable.GetDrop();

            if (_drop != null)
            {
                var _monster = new MonsterInstance((MonsterData)_drop, monsterLevel);
                _monster.Name = "Wild " + _monster.Name;
                monsters.Add(_monster);
            }
        }

        if (monsters.Any())
        {
            OnStartEncounter?.Invoke();
            var player = FindObjectOfType<PlayerController>();
            GameSceneController.Singleton.LoadBattleScene(player.TrainerController.PlayableMonsters, monsters, 
                player.TrainerController.DeckHolder.CurrentDeck, new List<CardData>(), this);
        }
    }

    public List<CardData> GetRewards()
    {
        return monsters.Select(x => x.GetCardDrop()).ToList();
    }
}

public interface IEncounter
{
    public void GetEncounter();

    public List<CardData> GetRewards();
}