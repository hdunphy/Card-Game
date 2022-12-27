using Assets.Scripts.Entities.Interfaces;
using System.Collections.Generic;

namespace Assets.Scripts.Entities.Trainers
{
    public class TrainerDeckHolder : IDeckHolder
    {
        public List<CardData> AllCards { get; private set; }

        public List<CardData> CurrentDeck => AllCards;

        public TrainerDeckHolder(List<CardData> allCards)
        {
            AllCards = allCards;
        }
    }
}
