using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeckHandler : DeckHandler
{
    protected override Card AddCardToHand(Card _card, Transform transform)
    {
        //Do nothing with the card
        return _card;
    }

    protected override void DiscardCardImpl(Card _card)
    {
        AddToDiscardPile(_card);
    }

    protected override void UpdateHandUI(List<Card> CardsInHand)
    {
        //Don't show cards
    }
}
