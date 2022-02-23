using Assets.Scripts.Entities.Drops;
using Assets.Scripts.References;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomEncounter : MonoBehaviour
{
    [SerializeField] private DropTable DropTable;
    [SerializeField, Range(0, 3)] private int MaxMonsters;

    public List<MonsterData> GetEncounter()
    {
        List<MonsterData> monsters = new List<MonsterData>();

        int monsterSpawns = Rules.GetRandomInt(0, MaxMonsters);

        for(int i = 0; i < monsterSpawns; i++)
        {
            var _drop = DropTable.GetDrop();

            if(_drop != null)
            {
                monsters.Add((MonsterData)_drop);
            }
        }

        return monsters;
    }
}
