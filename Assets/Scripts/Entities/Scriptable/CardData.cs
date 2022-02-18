using Assets.Scripts.Entities.Scriptable;
using System;
using System.Linq;
using UnityEngine;

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
    [SerializeField] private CardAction cardAction;

    public string CardName { get => cardName; }
    public string CardDescription { get => cardDescription; }
    public Sprite CardSprite { get => cardSprite; }
    public int EnergyCost { get => energyCost; }
    public TargetType TargetType { get => targetType; }
    public CardAlignment CardAlignment { get => cardAlignment; }
    public float AttackModifier { get => attackModifier * 100; } //Float to real percent

    public void InvokeAction(Monster source, Monster target, Card card)
    {
        switch (TargetType)
        {
            case TargetType.Self:
                cardAction.InvokeAction(target, target, card);
                break;
            case TargetType.Any:
                cardAction.InvokeAction(source, target, card);
                break;
            case TargetType.Side:
                var controller = FindObjectsOfType<MonsterController>().First(m => m.HasMonster(target)); //should never be null
                foreach (var monster in controller.monsters.Where(m => m.IsInPlay))
                {
                    cardAction.InvokeAction(source, monster, card);
                }
                break;
        }
    }

    public bool IsValidAction(Monster source, Monster target) =>
        TargetType switch
        {
            TargetType.Self => (source == null && target.IsTurn) || source == target,
            TargetType.Any => source != null && target != null,
            TargetType.Side => source != null && target != null, //handle later?
            _ => throw new NotImplementedException(),
        };
}

public enum TargetType { Self, Any, Side }
public enum CardAlignment { Fire, Water, Earth, Air, Nature, Ice, Light, Darkness, None }