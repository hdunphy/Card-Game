using Assets.Scripts.Entities.Interfaces;
using System.Collections.Generic;

namespace Assets.Scripts.Entities.Player
{
    public class PlayerDeckHolder : IDeckHolder
    {
        public List<CardData> AllCards { get; private set; }
        public List<Deck> Decks { get; private set; }
        private int CurrentDeckIndex { get; set; }
        public List<CardData> CurrentDeck => Decks[CurrentDeckIndex].Cards;

        public PlayerDeckHolder(List<CardData> allCards)
        {
            AllCards = allCards;
            Decks = new List<Deck> {
            new Deck{
                Cards = new List<CardData>(),
                Name = "Deck 1"
            }
        };
            CurrentDeckIndex = 0;
        }

        public PlayerDeckHolder(List<CardData> allCards, List<Deck> decks, int currentDeck = 0)
        {
            AllCards = allCards;
            Decks = decks;
            CurrentDeckIndex = currentDeck;
        }
    }
}
