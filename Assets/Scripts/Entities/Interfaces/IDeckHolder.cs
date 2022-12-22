using System.Collections.Generic;

namespace Assets.Scripts.Entities.Interfaces
{
    public interface IDeckHolder
    {
        public List<CardData> AllCards { get; }
        public List<CardData> CurrentDeck { get; }
    }
    public class Deck
    {
        public string Name { get; set; }
        public List<CardData> Cards { get; set; }
    }
}