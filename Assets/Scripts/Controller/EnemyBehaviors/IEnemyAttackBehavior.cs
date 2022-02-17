using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyAttackBehaviorEnum { Random };

public interface IEnemyAttackBehavior
{
    void SetTurnStategy(List<Card> hand, IEnumerable<Monster> selfMonsters, IEnumerable<Monster> otherMonsters);
    bool GetNextAttack();
}
