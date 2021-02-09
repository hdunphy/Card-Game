﻿using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Card : SelectableElement, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    public float HoverMovement;
    public float HoverScale;

    [Header("UI")]
    [SerializeField] private TMP_Text CardName;
    [SerializeField] private TMP_Text CardDescription;
    [SerializeField] private TMP_Text EnergyCostText;
    [SerializeField] private Image CardSprite;
    [SerializeField] private Image TargetTypeIcon;
    [SerializeField] private Image CardAlignmentIcon;
    [SerializeField] private Image DisableCover;

    private CardData Data;
    private int siblingIndex;
    public float Power => Data.AttackModifier;
    public int EnergyCost => Data.EnergyCost;

    public void SetSiblingIndex(int i) => siblingIndex = i;


    public void OnPointerDown(PointerEventData eventData)
    {
        EventManager.Instance.OnSelectCardTrigger(this);
    }

    private void Start()
    {
        EventManager.Instance.UpdateSelectedMonster += Instance_UpdateSelectedMonster;
    }

    private void OnDestroy()
    {
        EventManager.Instance.UpdateSelectedMonster -= Instance_UpdateSelectedMonster;
    }

    private void Instance_UpdateSelectedMonster(Monster _monster)
    {
        DisableCover.gameObject.SetActive(false);
        if(_monster != null)
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
        TargetTypeIcon.sprite = SpriteReferenceDictionary.Instance.GetSpriteFromEnum(Data.TargetType);
        CardAlignmentIcon.sprite = SpriteReferenceDictionary.Instance.GetSpriteFromEnum(Data.CardAlignment);

        gameObject.AddComponent<TooltipTrigger>().SetText($"Card has power: {Power}");
    }

    public CardAlignment CardAlignment => Data.CardAlignment;

    public void DiscardCard()
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

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.SetSiblingIndex(siblingIndex);
        LeanTween.moveLocalY(gameObject, 0, .5f);
        LeanTween.scale(gameObject, Vector3.one, .5f);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.SetAsLastSibling();
        LeanTween.moveLocalY(gameObject, HoverMovement, .5f);
        LeanTween.scale(gameObject, Vector3.one * HoverScale, .5f);
    }

}
