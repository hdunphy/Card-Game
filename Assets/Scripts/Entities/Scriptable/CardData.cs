using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "CardData", menuName = "Data/Create Card Data")]
public class CardData : ScriptableObject
{
    [SerializeField] private string cardName;
    [SerializeField] private string cardDescription;
    [SerializeField] private Sprite cardSprite;
    [SerializeField] private int energyCost;
    [SerializeField] private TargetType targetType;
    [SerializeField] private CardAlignment cardAlignment;
    [SerializeField, Range(0, 1.5f)] private float attackModifier;

    public string CardName { get => cardName; }
    public string CardDescription { get => cardDescription; }
    public Sprite CardSprite { get => cardSprite; }
    public int EnergyCost { get => energyCost; }
    public TargetType TargetType { get => targetType; }
    public CardAlignment CardAlignment { get => cardAlignment; }
    public float AttackModifier { get => attackModifier * 100; } //Float to real percent
}

public enum TargetType { Self, Enemy, Ally, All }
public enum CardAlignment { Fire, Water, Earth, Air, Nature, Ice, Light, Darkness, None }