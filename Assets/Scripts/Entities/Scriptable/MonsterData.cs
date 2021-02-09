using System;
using UnityEngine;

[CreateAssetMenu(fileName = "MonsterData", menuName = "Data/Create Monster Data")]
public class MonsterData : ScriptableObject
{
    [SerializeField] private Sprite sprite;
    [SerializeField] private int health;
    [SerializeField] private int attack;
    [SerializeField] private int defense;
    [SerializeField] private int energy;
    [SerializeField] private MonsterAlignment monsterAlignments;

    public Sprite Sprite { get => sprite; }
    public int Health { get => health; }
    public int Attack { get => attack; }
    public int Defense { get => defense; }
    public int Energy { get => energy; }
    public MonsterAlignment MonsterAlignments { get => monsterAlignments; }

}

[Serializable]
public class MonsterAlignment
{
    public CardAlignment Primary;
    public CardAlignment Secondary = CardAlignment.None;

    public bool Contains(CardAlignment alignment)
    {
        return alignment.Equals(Primary) || (Secondary != CardAlignment.None && alignment.Equals(Secondary));
    }
}