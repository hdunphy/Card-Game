using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RewardsController : MonoBehaviour
{
    [SerializeField] private Transform CardListParent;
    [SerializeField] private CardReward CardRewardPrefab;
    [SerializeField] private GameObject RewardCanvas;

    private CardData SelectedCard;
    private List<CardReward> CardRewards;

    private void Start()
    {
        RewardCanvas.SetActive(false);
        CardRewards = new List<CardReward>();
    }

    public void Show(List<CardData> cards)
    {
        foreach (var reward in CardRewards)
        {
            Destroy(reward.gameObject);
        }
        CardRewards.Clear();

        RewardCanvas.SetActive(true);
        foreach (var _cardData in cards)
        {
            var cardReward = Instantiate(CardRewardPrefab, CardListParent);
            cardReward.SetCardData(_cardData, this);
            CardRewards.Add(cardReward);
        }
    }

    public void SelectCard(CardReward selected)
    {
        foreach (var reward in CardRewards)
        {
            reward.IsSelected = (selected == reward) && !reward.IsSelected;
        }
    }

    public void OnContinueClicked()
    {
        var SelectedCards = CardRewards.Where(c => c.IsSelected).Select(c => c.CardData);
        FindObjectOfType<PlayerController>().AddCards(SelectedCard);

        RewardCanvas.SetActive(false);
    }
}
