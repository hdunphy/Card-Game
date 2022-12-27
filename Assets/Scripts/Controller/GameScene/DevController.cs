using Assets.Scripts.Entities.Interfaces;
using Assets.Scripts.Entities.Mingmings;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.GameScene.Controller
{
    public class DevController : MonoBehaviour
    {
        public IEnumerable<MingmingInstance> PlayableMingmings => MingmingHolder.Party.Where(m => m.CurrentHealth > 0);

        public IMingmingHolder MingmingHolder { get; private set; }

        public IDeckHolder DeckHolder { get; private set; }

        public void HealParty() => MingmingHolder.Party.ForEach((mingming) => mingming.CurrentHealth = mingming.Health);

        public void SetDeckHolder(IDeckHolder _deckHolder) => DeckHolder = _deckHolder;

        public void SetMingmingHolder(IMingmingHolder mingmingHolder) => MingmingHolder = mingmingHolder;
    }
}