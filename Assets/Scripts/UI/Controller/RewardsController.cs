using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.UI.Controller
{
    public class RewardsController : MonoBehaviour
    {
        [SerializeField] private Transform CardListParent;
        [SerializeField] private SelectableCard CardRewardPrefab;
        [SerializeField] private GameObject RewardCanvas;

        private List<SelectableCard> CardRewards;

        private void Start()
        {
            RewardCanvas.SetActive(false);
            CardRewards = new List<SelectableCard>();
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
                cardReward.SetCardData(_cardData);
                cardReward.OnSelected += SelectCard;
                CardRewards.Add(cardReward);
            }
        }

        public void SelectCard(SelectableCard selected)
        {
            foreach (var reward in CardRewards)
            {
                reward.IsSelected = (selected == reward) && !reward.IsSelected;
            }
        }

        public void OnContinueClicked()
        {
            var SelectedCards = CardRewards.Where(c => c.IsSelected).Select(c => c.CardData);
            FindObjectOfType<PlayerController>().DeckHolder.AllCards.AddRange(SelectedCards);

            RewardCanvas.SetActive(false);
        }

        private void OnDestroy()
        {
            foreach(var reward in CardRewards)
            {
                reward.OnSelected -= SelectCard;
            }
        }
    }
}