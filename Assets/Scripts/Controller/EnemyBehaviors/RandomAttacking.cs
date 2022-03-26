using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

public class RandomAttacking : IEnemyAttackBehavior
{
    private List<Mingming> SelfMonsters;
    private List<Mingming> OtherMonsters;
    private List<Card> Hand;

    public bool GetNextAttack()
    {
        bool hasAttack = CanAttack(out int minCardEnergy);

        if (hasAttack)
        {
            Mingming source = GetSource(SelfMonsters, minCardEnergy);
            Card _card = GetCard(source);
            Mingming target = GetTarget(OtherMonsters);

            List<Mingming> availableMonsters = new List<Mingming>(OtherMonsters);
            while(!CheckIsCardValid(_card, source, target))
            {
                availableMonsters.Remove(target);
                if(availableMonsters.Count == 0)
                {
                    target = source;
                    break;
                }

                target = GetTarget(availableMonsters);
            }

            if (CheckIsCardValid(_card, source, target))
            {
                UserMessage.Instance.CanSendMessage = true;

                EventManager.Instance.OnSelectMingmingTrigger(source);
                EventManager.Instance.OnSelectTargetTrigger(target, _card);
                EventManager.Instance.OnSelectMingmingTrigger(target);

                EventManager.Instance.OnSelectMingmingTrigger(source); //to deselect

                //disable user message so not to get bombarded by failed attempts
                UserMessage.Instance.CanSendMessage = false;
            }
            else
            {
                Hand.Remove(_card);
            }

            hasAttack = CanAttack(out _);
        }

        return hasAttack;
    }

    private bool CheckIsCardValid(Card card, Mingming source, Mingming target) =>
        card.IsValidAction(source, target) && source.EnergyAvailable >= card.EnergyCost;

    private bool CanAttack(out int minCardEnergy)
    {
        SelfMonsters = SelfMonsters.Where(x => x.IsInPlay && x.EnergyAvailable > 0).ToList();
        OtherMonsters = OtherMonsters.Where(x => x.IsInPlay).ToList();
        int maxMonsterEnergy = SelfMonsters.Any() ? SelfMonsters.Max(x => x.EnergyAvailable) : 0;
        minCardEnergy = Hand.Any() ? Hand.Min(x => x.EnergyCost) : int.MaxValue;

        return SelfMonsters.Count() > 0 && OtherMonsters.Count() > 0 && maxMonsterEnergy >= minCardEnergy;
    }

    private Mingming GetTarget(List<Mingming> availableOponents)
    {
        return availableOponents[Random.Range(0, availableOponents.Count())];
    }

    private Card GetCard(Mingming attacker)
    {
        var possiblecards = Hand.Where(x => x.EnergyCost <= attacker.EnergyAvailable).ToList();
        return possiblecards[Random.Range(0, possiblecards.Count())];
    }

    private Mingming GetSource(List<Mingming> availableSources, int minCardEnergy)
    {
        int RandIndex = Random.Range(0, availableSources.Count());
        Mingming source = availableSources[RandIndex];

        if(source.EnergyAvailable < minCardEnergy)
        {
            availableSources.Remove(source);
            source = GetSource(availableSources, minCardEnergy);
        }

        return source;
    }

    public void SetTurnStategy(List<Card> hand, IEnumerable<Mingming> selfMonsters, IEnumerable<Mingming> otherMonsters)
    {
        Hand = hand;
        SelfMonsters = selfMonsters.ToList();
        OtherMonsters = otherMonsters.ToList();
    }
}
