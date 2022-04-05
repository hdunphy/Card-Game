using Assets.Scripts.Entities.Drops;
using Assets.Scripts.Entities.Scriptable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardData", menuName = "Data/Create Card Data")]
public class CardData : IDropScriptableObject
{
    [SerializeField] private new string name;
    [SerializeField] private string cardDescription;
    [SerializeField] private Sprite cardSprite;
    [SerializeField] private int energyCost;
    [SerializeField] private CardTarget targetType;
    [SerializeField] private CardAlignment cardAlignment;
    [SerializeField, Range(0, 1.5f)] private float attackModifier;
    [SerializeField] private List<CardAction> cardActions;
    [SerializeField] private BaseConstraint cardConstraint;

    public string CardName { get => name; }
    public string CardDescription { get => cardDescription; }
    public Sprite CardSprite { get => cardSprite; }
    public int EnergyCost { get => energyCost; }
    public CardTarget TargetType { get => targetType; }
    public CardAlignment CardAlignment { get => cardAlignment; }
    public float AttackModifier { get => attackModifier * 100; } //Float to real percent
    public BaseConstraint CardConstraint { get => cardConstraint; }

    public IEnumerator InvokeAction(Mingming source, Mingming target, Card card)
    {
        foreach(var cardAction in cardActions)
        {
            TargetType.InvokeAction(cardAction, source, target, card);

            yield return new WaitForSeconds(cardAction.DurationSeconds); //parameterize this somewhere
        }

        //TODO: improve this below. Move into TargetType
        (source ?? target).PlayCard(card); //use null check for self targeting. 
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (!string.IsNullOrEmpty(name))
        {
            string thisFileNewName = name;
            string assetPath = UnityEditor.AssetDatabase.GetAssetPath(this.GetInstanceID());
            UnityEditor.AssetDatabase.RenameAsset(assetPath, thisFileNewName);
        }
    }
#endif
}

public enum CardAlignment { Fire, Water, Earth, Air, Nature, Ice, Light, Darkness, None }