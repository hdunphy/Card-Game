using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float SecondsBetweenAttack;

    [SerializeField] private MonsterController SelfController;
    [SerializeField] private MonsterController OtherController;
    private List<Card> Hand;

    [SerializeField] private EnemyAttackBehaviorEnum EnemyAttackBehaviorEnum;

    private IEnemyAttackBehavior attackBehavior;

    private void Start()
    {
        switch (EnemyAttackBehaviorEnum)
        {
            case EnemyAttackBehaviorEnum.Random:
                attackBehavior = new RandomAttacking();
                break;
        }

        //EventManager.Instance.NewTurn += Instance_NewTurn;
    }

    private void OnDestroy()
    {
        //EventManager.Instance.NewTurn -= Instance_NewTurn;
    }

    public void StartTurn()
    {
        SetUpTurn(SelfController.GetHand());
    }

    private void SetUpTurn(List<Card> hand)
    {
        Hand = hand;

        attackBehavior.SetTurnStategy(Hand, SelfController.monsters, OtherController.monsters);

        StartCoroutine(AttackPhase());
    }

    private IEnumerator AttackPhase()
    {
        while (GetNextAttack())
        {
            yield return new WaitForSeconds(SecondsBetweenAttack);
        }

        EventManager.Instance.OnGetNextTurnStateTrigger();
    }

    public bool GetNextAttack()
    {
        return attackBehavior.GetNextAttack();
    }
}
