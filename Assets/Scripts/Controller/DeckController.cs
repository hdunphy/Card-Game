using System.Collections.Generic;
using UnityEngine;

public abstract class DeckController : MonoBehaviour
{
    [SerializeField] protected HandManager HandManager;
    [Header("Prefabs")]
    [SerializeField] protected Card CardPrefab;
    [Header("Stats")]
    [SerializeField] private int StartTurnDraw;
    [Header("UI Locations")]
    [SerializeField] private GameObject GO_DrawPile;

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
        //EventManager.Instance.DrawCards += DrawCards;
        EventManager.Instance.DiscardCard += DiscardCard;
    }

    private void OnDestroy()
    {
        //EventManager.Instance.DrawCards -= DrawCards;
        EventManager.Instance.DiscardCard -= DiscardCard;
    }

    public void StartTurn()
    {
        //Pre draw actions like status effects

        //Draw
        DrawCards(StartTurnDraw);

        //Begin Turn
    }

    protected abstract Card AddCardToHand(Card _card, Transform transform);
    protected abstract void UpdateHandUI(List<Card> CardsInHand);
    protected abstract void DiscardCardImpl(Card _card);

    private void DrawCards(int _numberOfCards)
    {
        int cardsToDraw = DrawPile.Count > _numberOfCards ? _numberOfCards : DrawPile.Count;
        int cardsRemaining = _numberOfCards - cardsToDraw;

        for (int i = 0; i < cardsToDraw; i++)
        {
            CardsInHand.Add(AddCardToHand(DrawPile[i], HandManager.HandContainer.transform));
        }

        DrawPile.RemoveRange(0, cardsToDraw);
        //Add logic if all cards are drawn
        UpdateCardCounts();
        UpdateHandUI(CardsInHand);

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

    public void EndTurn()
    {
        while (CardsInHand.Count > 0)
        {
            DiscardCard(CardsInHand[0]);
        }
    }

    protected void AddToDiscardPile(Card _card)
    {
        DiscardPile.Add(_card);
        UpdateCardCounts();
    }

    private void DiscardCard(Card _card)
    {
        //Add check for Battle State
        if (CardsInHand.Remove(_card)) //might not be in this hand
        {
            DiscardCardImpl(_card);
            UpdateHandUI(CardsInHand);
        }
        else
            UserMessage.Instance.SendMessageToUser($"Could not remove card!\nInstanceId: {_card.GetInstanceID()}");
    }

    private void ShuffleDiscardIntoDrawpile()
    {
        DrawPile.AddRange(DiscardPile);

        foreach (Card _card in DrawPile)
        { //Can move into HandManager ?
            _card.transform.position = GO_DrawPile.transform.position;
        }
        DiscardPile.Clear();
        UpdateCardCounts();
    }

    protected void UpdateCardCounts()
    {
        HandManager.UpdateCardCounts(DrawPile.Count, DiscardPile.Count, CardsInHand.Count);
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
        UpdateCardCounts();
    }
}
