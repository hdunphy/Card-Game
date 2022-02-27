using Assets.Scripts.Entities;
using Assets.Scripts.Entities.Drops;
using Assets.Scripts.References;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RandomEncounter : MonoBehaviour, IEncounter
{
    [Header("Drop Table")]
    [SerializeField] private DropTableInstance DropTable;
    [SerializeField, Range(0, 3)] private int MaxMonsters;
    [SerializeField, Range(1, 98)] private int MinMonsterLevel;
    [SerializeField, Range(2, 99)] private int MaxMonsterLevel;

    [Header("Components")]
    [SerializeField] private RewardsController RewardsController;

    List<MonsterInstance> monsters;

    private void Start()
    {
        monsters = new List<MonsterInstance>();
    }

    public void GetEncounter()
    {
        monsters.Clear();

        int monsterSpawns = Rules.GetRandomInt(0, MaxMonsters);
        int monsterLevel = Rules.GetRandomInt(MinMonsterLevel, MaxMonsterLevel);

        for (int i = 0; i < monsterSpawns; i++)
        {
            var _drop = DropTable.GetDrop();

            if (_drop != null)
            {
                monsters.Add(new MonsterInstance((MonsterData)_drop, monsterLevel));
            }
        }

        if (monsters.Any())
        {
            GameSceneController.Singleton.LoadBattleScene(FindObjectOfType<PlayerController>().Monsters, monsters, this);
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