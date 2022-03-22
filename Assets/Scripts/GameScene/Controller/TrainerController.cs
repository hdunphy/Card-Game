using Assets.Scripts.Entities;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.GameScene.Controller
{
    public class TrainerController : MonoBehaviour
    {
        public IEnumerable<MonsterInstance> PlayableMonsters { get => Monsters.Where(m => m.CurrentHealth > 0); }
        //Replace
        public List<MonsterData> MonsterData;
        [SerializeField] private List<CardData> StartingDeck;

        public List<MonsterInstance> Monsters { get; private set; }

        public IDeckHolder DeckHolder { get; private set; }

        public void HealMonsters() => Monsters.ForEach((monster) => monster.CurrentHealth = monster.Health);

        public void SetDeckHolder(IDeckHolder _deckHolder)
        {
            DeckHolder = _deckHolder ?? 
                new PlayerDeckHolder(StartingDeck, new List<Deck> { new Deck { Cards = new List<CardData>(StartingDeck) } });
        }

        public void SetMonsters(List<MonsterInstance> monsterInstances)
        {
            Monsters = monsterInstances ?? MonsterData.Select(d => new MonsterInstance(d, 10)).ToList();
        }
    }
}