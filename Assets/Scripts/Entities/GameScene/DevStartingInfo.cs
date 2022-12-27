using Assets.Scripts.Entities.Mingmings;
using Assets.Scripts.Entities.Player;
using Assets.Scripts.Entities.SaveSystem;
using Assets.Scripts.Entities.Scriptable;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Entities.GameScene
{
    [Serializable]
    public class DevStartingInfo
    {
        [SerializeField] private List<MingmingLevelData> startingMingmings;
        [SerializeField] private List<CardData> startingDeck;

        public IEnumerable<CardData> Deck => startingDeck;
        public IEnumerable<MingmingInstance> Mingmings => startingMingmings.Select(m => new MingmingInstance(m.MingMingData, m.Level));

        public (DeckHolderSaveModel, MingmingHolderSaveModel) GetSaveModels() 
            => (new (new PlayerDeckHolder(startingDeck)),
                new(new PlayerMingmingHolder(startingMingmings)));
    }
}
