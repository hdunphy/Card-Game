using Assets.Scripts.Entities.Drops;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Create Mingming Data")]
public class MingmingData : IDropScriptableObject
{
    [SerializeField] private int id = -1;
    [SerializeField] private Sprite sprite;
    [SerializeField] private int health;
    [SerializeField] private int attack;
    [SerializeField] private int defense;
    [SerializeField] private int energy;
    [SerializeField] private int cardDraw;
    [SerializeField] private MingmingAlignment mingmingAlignments;
    [SerializeField] private List<CardData> wildCards;
    [SerializeField] private CardData level30Card;
    [SerializeField] private CardData level50Card;
    [SerializeField] private int maxBlueprintsRequired;

    public int ID => id;
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

    public int SetId(int currentMax)
    {
        id = currentMax + 1;
        return id;
    }
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