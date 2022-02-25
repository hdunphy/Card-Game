using Assets.Scripts.Entities;
using Assets.Scripts.Entities.Drops;
using Assets.Scripts.References;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RandomEncounter : MonoBehaviour
{
    [SerializeField] private DropTableInstance DropTable;
    [SerializeField, Range(0, 3)] private int MaxMonsters;
    [SerializeField, Range(1, 98)] private int MinMonsterLevel;
    [SerializeField, Range(2, 99)] private int MaxMonsterLevel;

    public void GetEncounter()
    {
        List<MonsterInstance> monsters = new List<MonsterInstance>();

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
            GameSceneController.Singleton.LoadBattleScene(FindObjectOfType<PlayerController>().Monsters, monsters);
        }
    }
}
