using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardsController : MonoBehaviour
{
    [SerializeField] private Transform CardListParent;
    [SerializeField] private CardReward CardRewardPrefab;
    [SerializeField] private GameObject RewardCanvas;

    private void Start()
    {
        RewardCanvas.SetActive(false);
    }

    public void Show(List<CardData> cards)
    {
        RewardCanvas.SetActive(true);
        foreach(var _cardData in cards)
        {
            var cardReward = Instantiate(CardRewardPrefab, CardListParent);
            cardReward.SetCardData(_cardData);
        }
    }
}
