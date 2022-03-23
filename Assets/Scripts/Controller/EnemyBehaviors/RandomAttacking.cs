using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

public class RandomAttacking : IEnemyAttackBehavior
{
    private List<Monster> SelfMonsters;
    private List<Monster> OtherMonsters;
    private List<Card> Hand;

    public bool GetNextAttack()
    {
        bool hasAttack = CanAttack(out int minCardEnergy);

        if (hasAttack)
        {
            Monster source = GetSource(SelfMonsters, minCardEnergy);
            Card _card = GetCard(source);
            Monster target = GetTarget(OtherMonsters);

            List<Monster> availableMonsters = new List<Monster>(OtherMonsters);
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

                EventManager.Instance.OnSelectMonsterTrigger(source);
                EventManager.Instance.OnSelectTargetTrigger(target, _card);
                EventManager.Instance.OnSelectMonsterTrigger(target);

                EventManager.Instance.OnSelectMonsterTrigger(source); //to deselect

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

    private bool CheckIsCardValid(Card card, Monster source, Monster target) =>
        card.IsValidAction(source, target) && source.EnergyAvailable >= card.EnergyCost;

    private bool CanAttack(out int minCardEnergy)
    {
        SelfMonsters = SelfMonsters.Where(x => x.IsInPlay && x.EnergyAvailable > 0).ToList();
        OtherMonsters = OtherMonsters.Where(x => x.IsInPlay).ToList();
        int maxMonsterEnergy = SelfMonsters.Any() ? SelfMonsters.Max(x => x.EnergyAvailable) : 0;
        minCardEnergy = Hand.Any() ? Hand.Min(x => x.EnergyCost) : int.MaxValue;

        return SelfMonsters.Count() > 0 && OtherMonsters.Count() > 0 && maxMonsterEnergy >= minCardEnergy;
    }

    private Monster GetTarget(List<Monster> availableOponents)
    {
        return availableOponents[Random.Range(0, availableOponents.Count())];
    }

    private Card GetCard(Monster attacker)
    {
        var possiblecards = Hand.Where(x => x.EnergyCost <= attacker.EnergyAvailable).ToList();
        return possiblecards[Random.Range(0, possiblecards.Count())];
    }

    private Monster GetSource(List<Monster> availableSources, int minCardEnergy)
    {
        int RandIndex = Random.Range(0, availableSources.Count());
        Monster source = availableSources[RandIndex];

        if(source.EnergyAvailable < minCardEnergy)
        {
            availableSources.Remove(source);
            source = GetSource(availableSources, minCardEnergy);
        }

        return source;
    }

    public void SetTurnStategy(List<Card> hand, IEnumerable<Monster> selfMonsters, IEnumerable<Monster> otherMonsters)
    {
        Hand = hand;
        SelfMonsters = selfMonsters.ToList();
        OtherMonsters = otherMonsters.ToList();
    }
}
