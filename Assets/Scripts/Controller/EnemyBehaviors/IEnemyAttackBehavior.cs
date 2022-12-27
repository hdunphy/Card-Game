using Assets.Scripts.Entities;
using Assets.Scripts.Entities.Mingmings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyAttackBehaviorEnum { Random, MaxTurnAttack };

public interface IEnemyAttackBehavior
{
    void SetTurnStrategy(List<Card> hand, IEnumerable<Mingming> ownedParty, IEnumerable<Mingming> otherParty);
    bool GetNextAttack();
}
