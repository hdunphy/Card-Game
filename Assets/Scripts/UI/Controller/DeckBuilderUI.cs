using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.UI.Controller
{
    public class DeckBuilderUI : MonoBehaviour, ISelectabaleCardController
    {
        [SerializeField] private int CardsPerRow = 4;
        [SerializeField] private GameObject DeckBuilderCanvas;
        [SerializeField] private Transform AvailableCardsParent;
        [SerializeField] private GameObject CardRowPrefab;
        [SerializeField] private SelectableCard CardRewardPrefab;
        [SerializeField] private Quantity QuantityPrefab;

        private List<SelectableCard> AvailableCards;

        private void Start()
        {
            DeckBuilderCanvas.gameObject.SetActive(false);
            AvailableCards = new List<SelectableCard>();
        }

        //called by unity event
        public void Show()
        {
            DeckBuilderCanvas.gameObject.SetActive(true);

            var player = FindObjectOfType<PlayerController>();
            var _allCards = player.DeckHolder.AllCards.GroupBy( c => c).ToList();
            Transform cardRow = null;

            for (int i = 0; i < _allCards.Count; i++)
            {
                if (i % CardsPerRow == 0)
                {
                    cardRow = Instantiate(CardRowPrefab, AvailableCardsParent).transform;
                }
                var _card = Instantiate(CardRewardPrefab, cardRow);
                _card.SetCardData(_allCards[i].Key);
                _card.OnSelected += SelectCard;
                AvailableCards.Add(_card);

                var qty = Instantiate(QuantityPrefab, _card.transform);
                qty.Setup(_allCards[i].Count(), _card);
            }

            var rtransform = AvailableCardsParent.GetComponent<RectTransform>();
            rtransform.sizeDelta = new Vector2( rtransform.rect.width, AvailableCardsParent.childCount * cardRow.GetComponent<RectTransform>().rect.height);
        }

        public void SelectCard(SelectableCard selectableCard)
        {
            Debug.Log($"Selected a card: {selectableCard.name}");
        }

        private void OnDestroy()
        {
            foreach(var card in AvailableCards)
            {
                card.OnSelected -= SelectCard;
            }
        }
    }
}
