using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HandManager : MonoBehaviour
{

    [SerializeField] public GameObject HandContainer;
    [SerializeField] private TMP_Text DrawCardCount;
    [SerializeField] private TMP_Text HandCardCount;
    [SerializeField] private TMP_Text DiscardCardCount;

    public float CardOffset;
    public float CardMovementTiming = 0.5f;

    public void UpdateCardPositions(List<Card> CardsInHand)
    {
        float handContainerSize = HandContainer.GetComponent<RectTransform>().rect.width;

        float currentOffset = CardOffset * CardsInHand.Count > handContainerSize ? handContainerSize / CardsInHand.Count : CardOffset;

        float halfIndex = (float)(CardsInHand.Count - 1) / 2;
        int cardIndex = CardsInHand.Count;
        for (int i = 0; i < CardsInHand.Count; i++)
        {
            CardsInHand[i].SetSiblingIndex(i);

            Vector2 position = new Vector2((halfIndex - i) * currentOffset, 0);
            CardsInHand[--cardIndex].MoveToHandPosition(position, Vector3.one, CardMovementTiming);
        }
    }

    public void UpdateCardCounts(int DrawPileCount, int DiscardPileCount, int HandCount)
    {
        DrawCardCount.text = DrawPileCount.ToString();
        DiscardCardCount.text = DiscardPileCount.ToString();
        if(HandCardCount != null)
            HandCardCount.text = HandCount.ToString();
    }
}
