using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private HandManager HandManager;
    [Header("Prefabs")]
    [SerializeField] private Card CardPrefab;
    [Header("UI Locations")]
    [SerializeField] private GameObject GO_DrawPile;
    [SerializeField] private GameObject GO_DiscardPile;

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
        EventManager.Instance.DiscardCard += DiscardCard;
    }

    private void OnDestroy()
    {
        EventManager.Instance.DrawCards -= DrawCards;
        EventManager.Instance.DiscardCard -= DiscardCard;
    }

    private void DiscardCard(Card _card)
    {
        if (!CardsInHand.Remove(_card))
            UserMessage.Instance.SendMessageToUser($"Could not remove card!\nInstanceId: {_card.GetInstanceID()}");

        _card.DiscardCard(GO_DiscardPile.transform.position, Vector3.one * .33f, HandManager.CardMovementTiming, () =>
        {
            DiscardPile.Add(_card);
            HandManager.UpdateCardCounts(DrawPile.Count, DiscardPile.Count);
        });

        HandManager.UpdateCardPositions(CardsInHand);
    }

    private void DrawCards(int _numberOfCards)
    {
        int cardsToDraw = DrawPile.Count > _numberOfCards ? _numberOfCards : DrawPile.Count;
        int cardsRemaining = _numberOfCards - cardsToDraw;

        for (int i = 0; i < cardsToDraw; i++)
        {
            CardsInHand.Add(DrawPile[i].DrawCard(HandManager.HandContainer.transform));
        }

        DrawPile.RemoveRange(0, cardsToDraw);
        //Add logic if all cards are drawn
        HandManager.UpdateCardCounts(DrawPile.Count, DiscardPile.Count);

        HandManager.UpdateCardPositions(CardsInHand);

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

    private void ShuffleDiscardIntoDrawpile()
    {
        DrawPile.AddRange(DiscardPile);
        
        foreach (Card _card in DrawPile)
        { //Can move into HandManager ?
            _card.transform.position = GO_DrawPile.transform.position;
        }
        DiscardPile.Clear();
        HandManager.UpdateCardCounts(DrawPile.Count, DiscardPile.Count);
    }

    public void AddCardsToDeck(List<CardData> cards)
    {
        foreach (CardData data in cards)
        {
            Card _card = Instantiate(CardPrefab, GO_DrawPile.transform);
            _card.GetComponent<RectTransform>().localScale = Vector3.one * .333f;
            _card.SetCardData(data);
            _card.gameObject.SetActive(false);
            DrawPile.Add(_card);
        }
        HandManager.UpdateCardCounts(DrawPile.Count, DiscardPile.Count);
    }
}
