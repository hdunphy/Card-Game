using Assets.Scripts.Entities;
using Assets.Scripts.Entities.Player;
using Assets.Scripts.Entities.Scriptable;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.GameScene.Controller
{
    public class DevController : MonoBehaviour
    {
        public IEnumerable<MingmingInstance> PlayableMingmings { get => Mingming.Where(m => m.CurrentHealth > 0); }
        //Replace
        public List<MingmingLevelData> MingmingData;
        [SerializeField] private List<CardData> StartingDeck;

        public List<MingmingInstance> Mingming { get; private set; }

        public IDeckHolder DeckHolder { get; private set; }

        public void HealParty() => Mingming.ForEach((mingming) => mingming.CurrentHealth = mingming.Health);

        public void SetDeckHolder(IDeckHolder _deckHolder)
        {
            DeckHolder = _deckHolder ?? 
                new PlayerDeckHolder(StartingDeck, new List<Deck> { new Deck { Cards = new List<CardData>(StartingDeck) } });
        }

        public void SetMingmings(List<MingmingInstance> mingmingInstances)
        {
            Mingming = mingmingInstances ?? MingmingData.Select(d => new MingmingInstance(d.MingMingData, d.Level)).ToList();
        }
    }
}