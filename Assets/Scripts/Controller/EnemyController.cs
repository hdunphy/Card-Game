using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float SecondsBetweenAttack;

    [SerializeField] private PartyController SelfController;
    [SerializeField] private PartyController OtherController;
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
    }

    public void StartTurn()
    {
        SetUpTurn(SelfController.GetHand());
    }

    private void SetUpTurn(List<Card> hand)
    {
        Hand = hand;

        attackBehavior.SetTurnStategy(Hand, SelfController.Mingmings, OtherController.Mingmings);

        StartCoroutine(AttackPhase());
    }

    private IEnumerator AttackPhase()
    {
        //disable user message so not to get bombarded by failed attempts
        UserMessage.Instance.CanSendMessage = false;

        yield return new WaitForSeconds(SecondsBetweenAttack);

        while (GetNextAttack())
        {
            yield return new WaitForSeconds(SecondsBetweenAttack);
        }

        UserMessage.Instance.CanSendMessage = true;

        if (OtherController.Mingmings.Any(m => m.IsInPlay))
        {
            EventManager.Instance.OnGetNextTurnStateTrigger();
        }
    }

    public bool GetNextAttack()
    {
        return attackBehavior.GetNextAttack();
    }
}
