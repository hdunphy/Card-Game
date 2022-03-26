using Assets.Scripts.Controller.References;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.Entities.SaveSystem
{
    [System.Serializable]
    public class DeckSaveModel
    {
        public string Name { get; set; }
        public List<string> CardDataNames { get; set; }

        public DeckSaveModel(Deck deck)
        {
            Name = deck.Name;
            CardDataNames = deck.Cards.Select(c => c.name).ToList();
        }

        public Deck GetPlayerDeckHolder() => new Deck
        {
            Cards = CardDataNames.Select(n => ScriptableObjectReferenceSingleton.Singleton.GetScriptableObject<CardData>(n)).ToList(),
            Name = Name
        };
    }
}