﻿using Assets.Scripts.Controller.References;
using Assets.Scripts.Entities.Player;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.Entities.SaveSystem
{
    [Serializable]
    public class DeckHolderSaveModel
    {
        public List<string> AllCardNames { get; set; }

        public List<DeckSaveModel> Decks { get; set; }

        public DeckHolderSaveModel(PlayerDeckHolder deckHolder)
        {
            AllCardNames = deckHolder.AllCards.Select(c => c.name).ToList();
            Decks = deckHolder.Decks.Select(d => new DeckSaveModel(d)).ToList();
        }

        public PlayerDeckHolder GetDeckHolder() =>
            new(
                AllCardNames.Select(n => ScriptableObjectReferenceSingleton.Singleton.GetScriptableObject<CardData>(n)).ToList(),
                Decks.Select(n => n.GetPlayerDeckHolder()).ToList()
            );
    }
}
