using Assets.Scripts.Entities;
using Assets.Scripts.UI.Controller;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    [SerializeField] private Image DisableCover;
    [SerializeField] private HoverEffect HoverEffect;
    [SerializeField] private DragAndDrop DragAndDrop;

    private ICardUI UIController;
    private CardData Data { get; set; }
    private int siblingIndex;
    public bool PlayedThisTurn { get; private set; }
    public float Power => Data.AttackModifier;
    public int EnergyCost => Data.EnergyCost;

    private void Awake()
    {
        UIController = GetComponentInChildren<ICardUI>();
    }

    private void Start()
    {
        DragAndDrop.SetRectTransform(Dragger.Instance.GetComponent<RectTransform>());
        EventManager.Instance.UpdateSelectedMingming += Instance_UpdateSelectedMingming;
    }

    private void OnDestroy()
    {
        EventManager.Instance.UpdateSelectedMingming -= Instance_UpdateSelectedMingming;
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
        if (!PlayedThisTurn)
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

    //invoked by unity event
    public void OnHoverStart() => transform.SetAsLastSibling();

    //invoked by unity event
    public void OnHoverEnd() => transform.SetSiblingIndex(siblingIndex);

    public CardAlignment CardAlignment => Data.CardAlignment;

    public IEnumerator InvokeActionCoroutine(Mingming source, Mingming target)
    {
        HoverEffect.enabled = false;
        PlayedThisTurn = true;

        yield return null;
        DragAndDrop.enabled = false;

        yield return InvokeActions(source.Logic, target.Logic);

        EventManager.Instance.OnDiscardCardTrigger(this);
    }

    public IEnumerator InvokeActions(MingmingBattleLogic source, MingmingBattleLogic target) => Data.InvokeActionCoroutine(source, target, this);

    public bool CanUseCard(MingmingBattleLogic source) => !PlayedThisTurn && Data.CardConstraint.CanUseCard(source, this);

    public bool IsValidTarget(Mingming source, Mingming target) => Data.TargetType.IsValidTarget(source, target, this);

    public bool IsValidAction(Mingming source, Mingming target) => IsValidTarget(source, target) && CanUseCard(source.Logic);

    private void Instance_UpdateSelectedMingming(Mingming _mingming)
    {
        DisableCover.gameObject.SetActive(false);
        if (_mingming != null)
        {
            UserMessage.Instance.CanSendMessage = false;

            bool isCardPlayable = CanUseCard(_mingming.Logic);
            DisableCover.gameObject.SetActive(!isCardPlayable);
            DragAndDrop.enabled = isCardPlayable;

            UserMessage.Instance.CanSendMessage = true;
        }
    }

    public void SetCardData(CardData _data)
    {
        Data = _data;
        name = Data.CardName;
        UIController.SetCardData(_data);
    }

    public void DiscardCard(Vector3 position, Vector3 scale, float CardMovementTiming, Action onComplete)
    {
        ToggleInteractions(false);

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
        DragAndDrop.enabled = true;

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
        PlayedThisTurn = false;
    }
}
