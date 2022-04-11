using Assets.Scripts.Entities;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

public class RandomAttacking : IEnemyAttackBehavior
{
    private List<Mingming> OwnedParty;
    private List<Mingming> OtherParty;
    private List<Card> Hand;

    public bool GetNextAttack()
    {
        bool hasAttack = CanAttack();

        if (hasAttack)
        {
            int minCardEnergy = Hand.Any() ? Hand.Min(x => x.EnergyCost) : int.MaxValue;
            Mingming source = GetRandomMingming(OwnedParty);
            Mingming target = GetRandomMingming(OtherParty);
            Card _card = GetCard(source);

            List<Mingming> availableMingmings = new List<Mingming>(OtherParty);
            while(!_card.IsValidAction(source, target))
            {
                availableMingmings.Remove(target);
                if(availableMingmings.Count == 0)
                {
                    target = source;
                    break;
                }

                target = GetRandomMingming(availableMingmings);
            }

            if (_card.IsValidAction(source, target))
            {
                UserMessage.Instance.CanSendMessage = true;

                EventManager.Instance.OnSelectMingmingTrigger(source);
                EventManager.Instance.OnSelectTargetTrigger(target, _card);

                EventManager.Instance.OnSelectMingmingTrigger(source); //to deselect

                //disable user message so not to get bombarded by failed attempts
                UserMessage.Instance.CanSendMessage = false;
            }

            hasAttack = CanAttack();
        }

        return hasAttack;
    }

    private bool CanAttack()
    {
        OwnedParty = OwnedParty.Where(x => x.IsInPlay && x.EnergyAvailable > 0).ToList();
        OtherParty = OtherParty.Where(x => x.IsInPlay).ToList();

        var targets = new List<Mingming>(OwnedParty);
        targets.AddRange(new List<Mingming>(OtherParty));

        Hand = Hand.Where(card => OwnedParty.Any(own => targets.Any( target => card.IsValidAction(own, target)))).ToList();

        return OwnedParty.Any() && OtherParty.Any() && Hand.Any();
    }

    private Mingming GetRandomMingming(List<Mingming> mingmings)
    {
        return mingmings[Random.Range(0, mingmings.Count())];
    }

    private Card GetCard(Mingming attacker)
    {
        var possiblecards = Hand.Where(x => x.EnergyCost <= attacker.EnergyAvailable).ToList();
        return possiblecards[Random.Range(0, possiblecards.Count())];
    }

    public void SetTurnStategy(List<Card> hand, IEnumerable<Mingming> ownedParty, IEnumerable<Mingming> otherParty)
    {
        Hand = hand;
        OwnedParty = ownedParty.ToList();
        OtherParty = otherParty.ToList();
    }
}
