using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_Text CardName;
    [SerializeField] private TMP_Text CardDescription;
    [SerializeField] private TMP_Text EnergyCostText;
    [SerializeField] private Image CardSprite;
    [SerializeField] private Image TargetTypeIcon;
    [SerializeField] private Image CardAlignmentIcon;
    [SerializeField] private Image DisableCover;
    [SerializeField] private HoverEffect HoverEffect;
    [SerializeField] private DragAndDrop DragAndDrop;

    private CardData Data;
    private int siblingIndex;
    public float Power => Data.AttackModifier;
    public int EnergyCost => Data.EnergyCost;

    private void Start()
    {
        DragAndDrop.SetRectTransform(Dragger.Instance.GetComponent<RectTransform>());
        EventManager.Instance.UpdateSelectedMonster += Instance_UpdateSelectedMonster;
    }

    private void OnDestroy()
    {
        EventManager.Instance.UpdateSelectedMonster -= Instance_UpdateSelectedMonster;
    }

    public void OnBeginDrag()
    {
        HoverEffect.enabled = false;
        Dragger.Instance.StartDragging(transform);
    }

    public void OnEndDrag()
    {
        Dragger.Instance.EndDragging();
        HoverEffect.enabled = true;
        HoverEffect.ReturnToNormalPosition();
    }

    public void SetSiblingIndex(int i) => siblingIndex = i;

    public void OnHoverStart() => transform.SetAsLastSibling();

    public void OnHoverEnd() => transform.SetSiblingIndex(siblingIndex);

    public CardAlignment CardAlignment => Data.CardAlignment;

    public void InvokeAction(Monster source, Monster target) => Data.InvokeAction(source, target, this);

    public bool IsValidAction(Monster source, Monster target) => Data.TargetType.IsValidAction(source, target, this);

    private void Instance_UpdateSelectedMonster(Monster _monster)
    {
        DisableCover.gameObject.SetActive(false);
        if (_monster != null)
        {
            DisableCover.gameObject.SetActive(_monster.EnergyAvailable < EnergyCost);
        }
    }

    public void SetCardData(CardData _data)
    {
        Data = _data;

        CardName.text = Data.CardName;
        CardDescription.text = Data.CardDescription;
        EnergyCostText.text = Data.EnergyCost.ToString();
        CardSprite.sprite = Data.CardSprite;
        TargetTypeIcon.sprite = Data.TargetType.Sprite;
        CardAlignmentIcon.sprite = SpriteReferenceDictionary.Instance.GetSpriteFromEnum(Data.CardAlignment);

        gameObject.AddComponent<TooltipTrigger>().SetText($"Card has power: {Power}");
    }

    public void DiscardCard(Vector3 position, Vector3 scale, float CardMovementTiming, Action onComplete)
    {
        transform.SetAsLastSibling();
        LeanTween.move(gameObject, position, CardMovementTiming);
        LeanTween.scale(gameObject, scale, CardMovementTiming).setOnComplete(() => { onComplete.Invoke(); SetInactive(); });
    }

    private void SetInactive()
    {
        transform.SetParent(null);
        gameObject.SetActive(false);
    }

    public Card DrawCard(Transform parentLocation)
    {
        transform.SetParent(parentLocation);
        gameObject.SetActive(true);

        return this;
    }

    public void MoveToHandPosition(Vector2 position, Vector3 scale, float CardMovementTiming)
    {
        HoverEffect.enabled = false;
        RectTransform cardTransform = GetComponent<RectTransform>();
        LeanTween.move(cardTransform, position, CardMovementTiming);
        LeanTween.scale(cardTransform, scale, CardMovementTiming)
            .setOnComplete(SetCardInHand);
    }

    void SetCardInHand()
    {
        HoverEffect.enabled = true;
    }
}
