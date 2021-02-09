using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HandController : MonoBehaviour
{

    [SerializeField] private GameObject HandContainer;
    [SerializeField] private GameObject DrawPileGO;
    [SerializeField] private GameObject DiscardPileGO;
    [SerializeField] private Card CardPrefab;
    [SerializeField] private TMP_Text DrawCardCount;
    [SerializeField] private TMP_Text DiscardCardCount;

    public float CardOffset;
    public float CardMovementTiming = 0.5f;

    private List<Card> DrawPile;
    private List<Card> CardsInHand;
    private List<Card> DiscardPile;


    private void Awake()
    {
        DrawPile = new List<Card>();
        CardsInHand = new List<Card>();
        DiscardPile = new List<Card>();
    }

    private void Start()
    {
        EventManager.Instance.DrawCards += DrawCards;
        EventManager.Instance.DiscardCard += Instance_DiscardCard;
    }

    private void OnDestroy()
    {
        EventManager.Instance.DrawCards -= DrawCards;
        EventManager.Instance.DiscardCard -= Instance_DiscardCard;
    }

    private void DrawCards(int _numberOfCards)
    {
        int cardsToDraw = DrawPile.Count > _numberOfCards ? _numberOfCards : DrawPile.Count;
        int cardsRemaining = _numberOfCards - cardsToDraw;

        for (int i = 0; i < cardsToDraw; i++)
        {
            CardsInHand.Add(DrawPile[i].DrawCard(HandContainer.transform));
        }

        DrawPile.RemoveRange(0, cardsToDraw);
        //Add logic if all cards are drawn
        UpdateCardCounts();

        UpdateCardPositions();

        if (cardsRemaining > 0)
        {
            if (DiscardPile.Count > 0)
            {
                ShuffleDiscardIntoDrawpile();
                DrawCards(cardsRemaining);
            }
            else
            {
                UserMessage.Instance.SendMessageToUser($"Not enough Cards to draw {cardsRemaining} more");
            }
        }
    }

    private void UpdateCardPositions()
    {
        float handContainerSize = HandContainer.GetComponent<RectTransform>().rect.width;

        float currentOffset = CardOffset * CardsInHand.Count > handContainerSize ? handContainerSize / CardsInHand.Count : CardOffset;

        float halfIndex = (float)(CardsInHand.Count - 1) / 2;
        int cardIndex = CardsInHand.Count;
        for (int i = 0; i < CardsInHand.Count; i++)
        {
            CardsInHand[i].SetSiblingIndex(i);

            Vector2 position = new Vector2((halfIndex - i) * currentOffset, 0);
            RectTransform cardTransform = CardsInHand[--cardIndex].GetComponent<RectTransform>();
            LeanTween.move(cardTransform, position, CardMovementTiming);
            LeanTween.scale(cardTransform, Vector3.one, CardMovementTiming);
            //CardsInHand[--cardIndex].GetComponent<RectTransform>().anchoredPosition = position;
        }
    }

    private void ShuffleDiscardIntoDrawpile()
    {
        Debug.Log(DrawPileGO.transform.position);
        DrawPile.AddRange(DiscardPile);
        foreach (Card _card in DrawPile)
        {
            _card.transform.position = DrawPileGO.transform.position;
        }
        DiscardPile.Clear();
        UpdateCardCounts();
    }

    private void Instance_DiscardCard(Card _card)
    {
        if (!CardsInHand.Remove(_card))
            Debug.Log($"Could not remove card!\nInstanceId: {_card.GetInstanceID()}");

        _card.gameObject.transform.SetAsLastSibling();
        LeanTween.move(_card.gameObject, DiscardPileGO.transform.position, CardMovementTiming);
        LeanTween.scale(_card.gameObject, Vector3.one * .33f, CardMovementTiming)
            .setOnComplete(() =>
            {
                _card.DiscardCard();
                DiscardPile.Add(_card);
                UpdateCardCounts();
            });

        UpdateCardPositions();
    }

    public void AddCardsToDeck(List<CardData> cards)
    {
        foreach (CardData data in cards)
        {
            Card _card = Instantiate(CardPrefab, DrawPileGO.transform);
            _card.GetComponent<RectTransform>().localScale = Vector3.one * .333f;
            _card.SetCardData(data);
            _card.gameObject.SetActive(false);
            DrawPile.Add(_card);
        }
        UpdateCardCounts();
    }

    private void UpdateCardCounts()
    {
        DrawCardCount.text = DrawPile.Count.ToString();
        DiscardCardCount.text = DiscardPile.Count.ToString();
    }
}
