using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterLoader : MonoBehaviour
{
    public Monster monsterPrefab;
    private List<Monster> monsters;

    private void Awake()
    {
        monsters = new List<Monster>();
    }

    public void LoadMonsters(IEnumerable<MonsterInstance> datas, PlayerTeam _playerTeam)
    {
        foreach (Monster _monster in monsters)
        {
            Destroy(_monster);
        }

        monsters.Clear();

        foreach (MonsterInstance _data in datas)
        {
            Monster _monster = Instantiate(monsterPrefab, transform);
            _monster.SetUp(_data, _playerTeam);
            monsters.Add(_monster);
        }
    }
}
