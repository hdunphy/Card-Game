using Assets.Scripts.Entities.Sound;
using Assets.Scripts.References;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DeckHandler : MonoBehaviour
{
    [SerializeField] protected HandManager HandManager;
    [Header("Sounds")]
    [SerializeField] protected SoundEventManager CardSoundManager;
    [SerializeField] protected string ShuffleCardsSound;
    [SerializeField] protected string DiscardCardSound;
    [SerializeField] protected string DrawCardSound;
    [Header("Prefabs")]
    [SerializeField] protected Card CardPrefab;
    [Header("UI Locations")]
    [SerializeField] private GameObject GO_DrawPile;

    private List<Card> DrawPile;
    private List<Card> CardsInHand;
    private List<Card> DiscardPile;

    private int CardDraw;

    private void Awake()
    {
        DrawPile = new List<Card>();
        CardsInHand = new List<Card>();
        DiscardPile = new List<Card>();
    }

    private void Start()
    {
        EventManager.Instance.DiscardCard += DiscardCard;
        EventManager.Instance.BattleOver += Instance_BattleOver;
    }

    private void OnDestroy()
    {
        EventManager.Instance.DiscardCard -= DiscardCard;
        EventManager.Instance.BattleOver -= Instance_BattleOver;
    }

    public void StartTurn()
    {
        //Pre draw actions like status effects

        //Draw
        DrawCards(CardDraw);

        //Begin Turn
    }

    private void Instance_BattleOver(MonsterController obj) => DiscardHand();

    protected abstract Card AddCardToHand(Card _card, Transform transform);
    protected abstract void UpdateHandUI(List<Card> CardsInHand);
    protected abstract void DiscardCardImpl(Card _card);

    public void DrawCards(int _numberOfCards)
    {
        int cardsToDraw = DrawPile.Count > _numberOfCards ? _numberOfCards : DrawPile.Count;
        int cardsRemaining = _numberOfCards - cardsToDraw;

        for (int i = 0; i < cardsToDraw; i++)
        {
            CardsInHand.Add(AddCardToHand(DrawPile[i], HandManager.HandContainer.transform));
            CardSoundManager.PlaySoundAfterDelay(DrawCardSound, 0.1f * i);
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
                CardSoundManager.PlaySound(ShuffleCardsSound);
                DrawCards(cardsRemaining);
            }
            else
            {
                UserMessage.Instance.SendMessageToUser($"Not enough Cards to draw {cardsRemaining} more");
            }
        }
    }

    public void SetCardDraw(int cardDraw)
    {
        CardDraw = cardDraw;
    }

    public void DiscardHand()
    {
        StartCoroutine(DiscardHand_Coroutine());
    }

    private IEnumerator DiscardHand_Coroutine()
    {
        while (CardsInHand.Count > 0)
        {
            DiscardCard(CardsInHand[0]);
            yield return null;
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
            CardSoundManager.PlaySound(DiscardCardSound);
        }
        else
            Debug.LogWarning($"Could not remove card!\nInstanceId: {_card.GetInstanceID()}");
    }

    private void ShuffleDiscardIntoDrawpile()
    {
        DrawPile.AddRange(DiscardPile);
        DrawPile.Shuffle();

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
            DrawPile.Shuffle();
        }
        UpdateCardCounts();
    }

    public List<Card> GetHand()
    {
        return CardsInHand;
    }
}
