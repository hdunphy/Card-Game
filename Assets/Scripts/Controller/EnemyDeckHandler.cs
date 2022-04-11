using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeckHandler : DeckHandler
{
    [SerializeField] private Transform handTransform;
    [SerializeField] private Transform discardPileTransform;
    [SerializeField] private Transform cardShowPosition;

    private const float MOVE_DURATION = .5f;
    private const float SHOW_DURATION = 1f;

    protected override Card AddCardToHand(Card _card, Transform transform)
    {
        //Do nothing with the card
        return _card;
    }

    protected override void DiscardCardImpl(Card _card)
    {
        if (_card.PlayedThisTurn)
        {
            _card.transform.position = handTransform.position;

            _card.gameObject.SetActive(true);
            LeanTween.move(_card.gameObject, cardShowPosition.position, MOVE_DURATION);
            LeanTween.scale(_card.gameObject, Vector3.one * .33f, MOVE_DURATION);

            LeanTween.delayedCall(
                SHOW_DURATION + MOVE_DURATION,
                () =>
                    {
                        LeanTween.move(_card.gameObject, discardPileTransform.position, MOVE_DURATION)
                                  .setOnComplete(
                                      () => _card.gameObject.SetActive(false)
                                  );
                        LeanTween.scale(_card.gameObject, Vector3.one, MOVE_DURATION);
                    }
            );
        }
        AddToDiscardPile(_card);
    }

    protected override void UpdateHandUI(List<Card> CardsInHand)
    {
        //Don't show cards
    }
}
