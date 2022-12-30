using Assets.Scripts.Entities.Drops;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Create Mingming Data")]
public class MingmingData : IDropScriptableObject
{
    public const int MAX_ENERGY = 10;
    public const int MAX_CARD_DRAW = 10;

    [SerializeField] private Sprite sprite;
    [SerializeField] private int health;
    [SerializeField] private int attack;
    [SerializeField] private int defense;
    [SerializeField, Range(1, MAX_ENERGY)] private int energy;
    [SerializeField, Range(1, MAX_CARD_DRAW)] private int cardDraw;
    [SerializeField] private MingmingAlignment mingmingAlignments;
    [SerializeField] private List<CardData> wildCards;
    [SerializeField] private CardData level30Card;
    [SerializeField] private CardData level50Card;
    [SerializeField] private int maxBlueprintsRequired;

    public Sprite Sprite => sprite;
    public int Health => health;
    public int Attack => attack;
    public int Defense => defense;
    public int Energy => energy;
    public int CardDraw => cardDraw;
    public MingmingAlignment MingmingAlignment => mingmingAlignments;
    public List<CardData> WildCards => wildCards;
    public CardData Level30Card => level30Card;
    public CardData Level50Card => level50Card;
    public int MaxBlueprintsRequired => maxBlueprintsRequired;
}

[Serializable]
public class MingmingAlignment
{
    public CardAlignment Primary;
    public CardAlignment Secondary = CardAlignment.None;

    public bool Contains(CardAlignment alignment)
    {
        return alignment.Equals(Primary) || (Secondary != CardAlignment.None && alignment.Equals(Secondary));
    }
}