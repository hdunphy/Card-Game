using Assets.Scripts.References;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.UI.Controller
{
    public class DeckBuilderUI : MonoBehaviour
    {
        [SerializeField] private int CardsPerRow = 4;
        [SerializeField] private GameObject DeckBuilderCanvas;
        [SerializeField] private Transform AvailableCardsParent;
        [SerializeField] private Transform CurrentDeckParent;
        [SerializeField] private GameObject CardRowPrefab;
        [SerializeField] private SelectableCard CardPrefab;
        [SerializeField] private Quantity QuantityPrefab;
        [SerializeField] private SelectableCard CurrentCardPrefab;

        private List<SelectableCard> AvailableCards;
        private List<SelectableCard> CurrentCards;

        private void Start()
        {
            DeckBuilderCanvas.gameObject.SetActive(false);
            AvailableCards = new List<SelectableCard>();
            CurrentCards = new List<SelectableCard>();
        }

        //called by unity event
        public void Show()
        {
            DeckBuilderCanvas.gameObject.SetActive(true);
            var player = FindObjectOfType<PlayerController>();

            ClearCards();
            GetAvailableCards(player.DeckHolder);
            GetCurrentCards(player.DeckHolder);
        }

        private void ClearCards()
        {
            foreach(var card in AvailableCards)
            {
                Destroy(card);
            }
            foreach (var card in CurrentCards)
            {
                Destroy(card);
            }

            AvailableCardsParent.transform.Clear();

            AvailableCards.Clear();
            CurrentCards.Clear();
        }

        private void GetAvailableCards(IDeckHolder deckHolder)
        {
            var _allCards = deckHolder.AllCards.GroupBy(c => c).ToList();
            Transform cardRow = null;

            for (int i = 0; i < _allCards.Count; i++)
            {
                if (i % CardsPerRow == 0)
                {
                    cardRow = Instantiate(CardRowPrefab, AvailableCardsParent).transform;
                }

                //Create selectable
                var _card = Instantiate(CardPrefab, cardRow);
                _card.SetCardData(_allCards[i].Key);
                _card.OnSelected += AddCard;
                AvailableCards.Add(_card);

                //Create quantity
                int count = _allCards[i].Count() - deckHolder.CurrentDeck.Count(c => c == _allCards[i].Key);
                var qty = Instantiate(QuantityPrefab, _card.transform);
                qty.Setup(count, _card);
            }

            var rtransform = AvailableCardsParent.GetComponent<RectTransform>();
            rtransform.sizeDelta = new Vector2(rtransform.rect.width, AvailableCardsParent.childCount * cardRow.GetComponent<RectTransform>().rect.height);
        }

        private void GetCurrentCards(IDeckHolder deckHolder)
        {
            var currentCards = deckHolder.CurrentDeck.GroupBy(c => c).ToList();
            for (int i = 0; i < currentCards.Count; i++)
            {
                //Create selectable
                var _cardGO = Instantiate(CurrentCardPrefab, CurrentDeckParent);
                _cardGO.SetCardData(currentCards[i].Key);
                _cardGO.OnSelected += RemoveCard;
                CurrentCards.Add(_cardGO);

                //Create quantity
                int count = currentCards[i].Count();
                var qty = Instantiate(QuantityPrefab, _cardGO.transform);
                qty.Setup(count, _cardGO);
            }

            var rtransform = CurrentDeckParent.GetComponent<RectTransform>();
            rtransform.sizeDelta = new Vector2(rtransform.rect.width, CurrentDeckParent.childCount * CurrentCardPrefab.GetComponent<RectTransform>().rect.height);
        }

        public void AddCard(SelectableCard selectableCard)
        {
            Debug.Log($"Added a card: {selectableCard.name}");
        }

        public void RemoveCard(SelectableCard selectableCard)
        {
            Debug.Log($"Removed a card: {selectableCard.name}");
        }

        private void OnDestroy()
        {
            foreach(var card in AvailableCards)
            {
                card.OnSelected -= AddCard;
            }
        }
    }
}
