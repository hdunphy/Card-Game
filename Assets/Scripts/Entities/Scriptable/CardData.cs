using Assets.Scripts.Entities.Drops;
using Assets.Scripts.Entities.Scriptable;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "CardData", menuName = "Data/Create Card Data")]
public class CardData : IDropScriptableObject
{
    [SerializeField] private string cardName;
    [SerializeField] private string cardDescription;
    [SerializeField] private Sprite cardSprite;
    [SerializeField] private int energyCost;
    [SerializeField] private CardTarget targetType;
    [SerializeField] private CardAlignment cardAlignment;
    [SerializeField, Range(0, 1.5f)] private float attackModifier;
    [SerializeField] private CardAction cardAction;
    [SerializeField] private BaseConstraint cardConstraint;

    public string CardName { get => cardName; }
    public string CardDescription { get => cardDescription; }
    public Sprite CardSprite { get => cardSprite; }
    public int EnergyCost { get => energyCost; }
    public CardTarget TargetType { get => targetType; }
    public CardAlignment CardAlignment { get => cardAlignment; }
    public float AttackModifier { get => attackModifier * 100; } //Float to real percent
    public BaseConstraint CardConstraint { get => cardConstraint; }

    public void InvokeAction(Monster source, Monster target, Card card) =>
        TargetType.InvokeAction(cardAction, source, target, card);
}

public enum CardAlignment { Fire, Water, Earth, Air, Nature, Ice, Light, Darkness, None }