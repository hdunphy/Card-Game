﻿using Assets.Scripts.Entities;
using Assets.Scripts.Entities.Scriptable;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.GameScene.Controller
{
    public class SharedController : MonoBehaviour
    {
        public IEnumerable<MingmingInstance> PlayableMonsters { get => Monsters.Where(m => m.CurrentHealth > 0); }
        //Replace
        public List<MingmingLevelData> MonsterData;
        [SerializeField] private List<CardData> StartingDeck;

        public List<MingmingInstance> Monsters { get; private set; }

        public IDeckHolder DeckHolder { get; private set; }

        public void HealMonsters() => Monsters.ForEach((monster) => monster.CurrentHealth = monster.Health);

        public void SetDeckHolder(IDeckHolder _deckHolder)
        {
            DeckHolder = _deckHolder ?? 
                new PlayerDeckHolder(StartingDeck, new List<Deck> { new Deck { Cards = new List<CardData>(StartingDeck) } });
        }

        public void SetMonsters(List<MingmingInstance> monsterInstances)
        {
            Monsters = monsterInstances ?? MonsterData.Select(d => new MingmingInstance(d.MingMingData, d.Level)).ToList();
        }
    }
}