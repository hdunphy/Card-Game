using Assets.Scripts.Entities.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.Entities.Trainers
{
    public class TrainerDeckHolder : IDeckHolder
    {
        public List<CardData> AllCards { get; private set; }

        public List<CardData> CurrentDeck => AllCards;

        public TrainerDeckHolder(IEnumerable<CardData> allCards)
        {
            AllCards = allCards.ToList();
        }
    }
}
