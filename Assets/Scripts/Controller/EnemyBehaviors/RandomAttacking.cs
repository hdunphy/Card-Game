using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
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
            Monster attacker = GetAttacker(SelfMonsters, minCardEnergy);
            Card _card = GetCard(attacker);
            Monster defender = GetDefender(OtherMonsters);

            EventManager.Instance.OnSelectMonsterTrigger(attacker);
            EventManager.Instance.OnSelectCardTrigger(_card);
            EventManager.Instance.OnSelectMonsterTrigger(defender);

            EventManager.Instance.OnSelectMonsterTrigger(attacker); //to deselect

            hasAttack = CanAttack(out _);
        }

        return hasAttack;
    }

    private bool CanAttack(out int minCardEnergy)
    {
        SelfMonsters = SelfMonsters.Where(x => x.IsActive && x.EnergyAvailable > 0).ToList();
        OtherMonsters = OtherMonsters.Where(x => x.IsActive).ToList();
        int maxMonsterEnergy = SelfMonsters.Any() ? SelfMonsters.Max(x => x.EnergyAvailable) : 0;
        minCardEnergy = Hand.Any() ? Hand.Min(x => x.EnergyCost) : int.MaxValue;

        return SelfMonsters.Count() > 0 && OtherMonsters.Count() > 0 && maxMonsterEnergy >= minCardEnergy;
    }

    private Monster GetDefender(List<Monster> availableOponents)
    {
        return availableOponents[Random.Range(0, availableOponents.Count())];
    }

    private Card GetCard(Monster attacker)
    {
        var possiblecards = Hand.Where(x => x.EnergyCost <= attacker.EnergyAvailable).ToList();
        return possiblecards[Random.Range(0, possiblecards.Count())];
    }

    private Monster GetAttacker(List<Monster> availableAttackers, int minCardEnergy)
    {
        int RandIndex = Random.Range(0, availableAttackers.Count());
        Monster attacker = availableAttackers[RandIndex];

        if(attacker.EnergyAvailable < minCardEnergy)
        {
            availableAttackers.Remove(attacker);
            attacker = GetAttacker(availableAttackers, minCardEnergy);
        }

        return attacker;
    }

    public void SetTurnStategy(List<Card> hand, IEnumerable<Monster> selfMonsters, IEnumerable<Monster> otherMonsters)
    {
        Hand = hand;
        SelfMonsters = selfMonsters.ToList();
        OtherMonsters = otherMonsters.ToList();
    }
}
