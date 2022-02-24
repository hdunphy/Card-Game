using Assets.Scripts.Entities.Drops;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MonsterData", menuName = "Data/Create Monster Data")]
public class MonsterData : IDropScriptableObject
{
    [SerializeField] private Sprite sprite;
    [SerializeField] private int health;
    [SerializeField] private int attack;
    [SerializeField] private int defense;
    [SerializeField] private int energy;
    [SerializeField] private int cardDraw;
    [SerializeField] private MonsterAlignment monsterAlignments;
    [SerializeField] private List<CardData> wildCards;
    [SerializeField] private CardData level30Card;
    [SerializeField] private CardData level50Card;

    public Sprite Sprite { get => sprite; }
    public int Health { get => health; }
    public int Attack { get => attack; }
    public int Defense { get => defense; }
    public int Energy { get => energy; }
    public int CardDraw { get => cardDraw; }
    public MonsterAlignment MonsterAlignments { get => monsterAlignments; }
    public List<CardData> WildCards { get => wildCards; }
    public CardData Level30Card => level30Card;
    public CardData Level50Card => level50Card;
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