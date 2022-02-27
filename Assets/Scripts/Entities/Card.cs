using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    [SerializeField] private Image DisableCover;
    [SerializeField] private HoverEffect HoverEffect;
    [SerializeField] private DragAndDrop DragAndDrop;
    [SerializeField] private CardUIController UIController;

    private CardData Data;
    private int siblingIndex;
    private bool playedThisTurn;
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
        ToggleOtherCardInteractions(false);

        HoverEffect.enabled = false;
        Dragger.Instance.StartDragging(transform);
    }

    public void OnEndDrag()
    {
        Dragger.Instance.EndDragging();
        if (!playedThisTurn)
        {
            HoverEffect.enabled = true;
            HoverEffect.ReturnToNormalPosition();
        }
        ToggleOtherCardInteractions(true);
    }

    private void ToggleOtherCardInteractions(bool isDisabled)
    {
        var cards = FindObjectsOfType<Card>().Where(c => c != this && c.gameObject.activeSelf);
        foreach (var _card in cards)
        {
            _card.ToggleInteractions(isDisabled);
        }
    }

    public void ToggleInteractions(bool isDisabled)
    {
        HoverEffect.enabled = isDisabled;
        DragAndDrop.enabled = isDisabled;
    }

    public void SetSiblingIndex(int i) => siblingIndex = i;

    public void OnHoverStart() => transform.SetAsLastSibling();

    public void OnHoverEnd() => transform.SetSiblingIndex(siblingIndex);

    public CardAlignment CardAlignment => Data.CardAlignment;

    public void InvokeAction(Monster source, Monster target)
    {
        HoverEffect.enabled = false;
        playedThisTurn = true;
        Data.InvokeAction(source, target, this);
    }

    public bool CheckConstraints(Monster source) => Data.CardConstraint.CheckConstraint(source, this);

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
        UIController.SetCardData(_data);

        gameObject.AddComponent<TooltipTrigger>().SetText($"Card has power: {Power}");
    }

    public void DiscardCard(Vector3 position, Vector3 scale, float CardMovementTiming, Action onComplete)
    {
        HoverEffect.enabled = false;
        LeanTween.cancel(gameObject);
        transform.SetAsLastSibling();
        LeanTween.move(gameObject, position, CardMovementTiming).setDelay(0.5f);
        LeanTween.scale(gameObject, scale, CardMovementTiming).setDelay(0.5f).setOnComplete(() => { onComplete.Invoke(); SetInactive(); });
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
        playedThisTurn = false;
    }
}
