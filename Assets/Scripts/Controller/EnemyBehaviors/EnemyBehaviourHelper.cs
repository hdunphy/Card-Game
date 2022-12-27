using Assets.Scripts.Entities;
using Assets.Scripts.Entities.Mingmings;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.Controller.EnemyBehaviors
{
    public static class EnemyBehaviourHelper
    {
        public static bool CanAttack(IEnumerable<Mingming> OwnedParty, IEnumerable<Mingming> OtherParty, IEnumerable<Card> Hand)
        {
            OwnedParty = OwnedParty.Where(x => x.IsInPlay && x.Logic.EnergyAvailable > 0).ToList();
            OtherParty = OtherParty.Where(x => x.IsInPlay).ToList();

            var targets = new List<Mingming>(OwnedParty);
            targets.AddRange(new List<Mingming>(OtherParty));

            Hand = Hand.Where(card => OwnedParty.Any(own => targets.Any(target => card.IsValidAction(own, target)))).ToList();

            return OwnedParty.Any() && OtherParty.Any() && Hand.Any();
        }

        public static int GetCurrentStateScore(this MingmingBattleLogic mingming)
        {
            int statusScore = mingming.GetStatuses().Aggregate(0, (score, pair) => score += pair.Key.GetScore(pair.Value));
            return mingming.CurrentHealth * 2 + statusScore;
        }
    }
}