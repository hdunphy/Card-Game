using Assets.Scripts.Entities;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeckHandler : DeckHandler
{
    [SerializeField] private GameObject GO_DiscardPile;
    protected override Card AddCardToHand(Card _card, Transform transform)
    {
        //Do nothing with the card
        return _card.DrawCard(transform);
    }

    protected override void DiscardCardImpl(Card _card)
    {
        _card.DiscardCard(GO_DiscardPile.transform.position, Vector3.one * .33f, HandManager.CardMovementTiming, () => { AddToDiscardPile(_card); });
    }

    protected override void UpdateHandUI(List<Card> CardsInHand)
    {
        HandManager.UpdateCardPositions(CardsInHand);
    }
}
