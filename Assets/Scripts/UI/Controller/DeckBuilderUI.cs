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
        private IDeckHolder DeckHolder;

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
            DeckHolder = FindObjectOfType<PlayerController>().SharedController.DeckHolder;

            GetAvailableCards();
            GetCurrentCards();
        }

        public void Hide()
        {
            ClearCards();
            DeckBuilderCanvas.gameObject.SetActive(false);
        }

        private void ClearCards()
        {
            AvailableCardsParent.transform.Clear();
            CurrentDeckParent.transform.Clear();

            AvailableCards.Clear();
            CurrentCards.Clear();
        }

        private void GetAvailableCards()
        {
            var _allCards = DeckHolder.AllCards.GroupBy(c => c).ToList();
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
                _card.OnSelected += AddCardToCurrentDeck;
                AvailableCards.Add(_card);

                //Create quantity
                int count = _allCards[i].Count() - DeckHolder.CurrentDeck.Count(c => c == _allCards[i].Key);
                var qty = Instantiate(QuantityPrefab, _card.transform);
                qty.Setup(count, _card);
            }

            var rtransform = AvailableCardsParent.GetComponent<RectTransform>();
            rtransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, AvailableCardsParent.childCount * cardRow.GetComponent<RectTransform>().rect.height);
        }

        private void GetCurrentCards()
        {
            var currentCards = DeckHolder.CurrentDeck.GroupBy(c => c).ToList();
            for (int i = 0; i < currentCards.Count; i++)
            {
                var cardGroup = currentCards[i];
                AddCurrentCard(cardGroup.Key, cardGroup.Count());
            }

            if (CurrentCards.Any())
            {
                var rtransform = CurrentDeckParent.GetComponent<RectTransform>();
                rtransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, CurrentDeckParent.childCount * (CurrentCards[0].GetComponent<RectTransform>().rect.height + 15));
            }
        }

        private void AddCurrentCard(CardData card, int count)
        {
            //Create selectable
            var _cardGO = Instantiate(CurrentCardPrefab, CurrentDeckParent);
            _cardGO.SetCardData(card);
            CurrentCards.Add(_cardGO);

            //Create quantity
            var qty = Instantiate(QuantityPrefab, _cardGO.transform);
            var rect = qty.GetComponent<RectTransform>();
            rect.anchorMax = new Vector2(0, .5f);
            rect.anchorMin = new Vector2(0, .5f);
            rect.anchoredPosition = new Vector2(12.5f, 0);
            qty.Setup(count, _cardGO);
            _cardGO.OnSelected += RemoveCard; //TODO: Fix this race condition
        }

        public void AddCardToCurrentDeck(SelectableCard selectableCard)
        {
            Debug.Log($"Added a card: {selectableCard.name}");
            var _currentCard = CurrentCards.FirstOrDefault(c => c.CardData == selectableCard.CardData);
            if (_currentCard != null)
            {
                _currentCard.GetComponentInChildren<Quantity>().Count++;
            }
            else
            {
                AddCurrentCard(selectableCard.CardData, 1);
            }

            DeckHolder.CurrentDeck.Add(selectableCard.CardData);
        }

        public void RemoveCard(SelectableCard selectableCard)
        {
            Debug.Log($"Removed a card: {selectableCard.name}");
            var qty = selectableCard.GetComponentInChildren<Quantity>();
            if (qty.Count <= 0)
            {
                //TODO: Shouldn't be happening here
                var c = CurrentCards.FirstOrDefault(c => c == selectableCard);
                Destroy(c.gameObject);
            }

            var _availableCard = AvailableCards.FirstOrDefault(c => c.CardData == selectableCard.CardData);
            _availableCard.GetComponentInChildren<Quantity>().Count++;

            DeckHolder.CurrentDeck.Remove(selectableCard.CardData);
        }

        private void OnDestroy()
        {
            foreach (var card in AvailableCards)
            {
                card.OnSelected -= AddCardToCurrentDeck;
            }
            foreach (var card in CurrentCards)
            {
                card.OnSelected -= RemoveCard;
            }
        }
    }
}
