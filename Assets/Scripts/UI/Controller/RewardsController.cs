using Assets.Scripts.GameScene.Controller;
using Assets.Scripts.Helpers;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.UI.Controller
{
    public class RewardsController : SingletonMonoBehavior<RewardsController>
    {
        [SerializeField] private Transform CardListParent;
        [SerializeField] private SelectableCard CardRewardPrefab;
        [SerializeField] private GameObject RewardCanvas;

        private List<SelectableCard> _cardRewards;
        private PlayerController _player;

        private void Awake()
        {
            OnAwake(this);
        }

        private void Start()
        {
            RewardCanvas.SetActive(false);
            _cardRewards = new List<SelectableCard>();
        }

        public void Show(List<CardData> cards, PlayerController player)
        {
            _player = player;

            foreach (var reward in _cardRewards)
            {
                Destroy(reward.gameObject);
            }
            _cardRewards.Clear();

            RewardCanvas.SetActive(true);
            foreach (var _cardData in cards)
            {
                var cardReward = Instantiate(CardRewardPrefab, CardListParent);
                cardReward.SetCardData(_cardData);
                cardReward.OnSelected += SelectCard;
                _cardRewards.Add(cardReward);
            }
        }

        public void SelectCard(SelectableCard selected)
        {
            foreach (var reward in _cardRewards)
            {
                reward.IsSelected = (selected == reward) && !reward.IsSelected;
            }
        }

        public void OnContinueClicked()
        {
            var SelectedCards = _cardRewards.Where(c => c.IsSelected).Select(c => c.CardData);
            _player.DevController.DeckHolder.AllCards.AddRange(SelectedCards);

            RewardCanvas.SetActive(false);
            _player.GetComponent<PlayerInputController>().enabled = true;
        }

        private void OnDestroy()
        {
            foreach(var reward in _cardRewards)
            {
                reward.OnSelected -= SelectCard;
            }

            DestroySingleton();
        }
    }
}