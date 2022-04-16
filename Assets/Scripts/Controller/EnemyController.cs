using Assets.Scripts.Controller.EnemyBehaviors;
using System.Collections;
using System.Linq;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float SecondsBetweenAttack;

    [SerializeField] private PartyController SelfController;
    [SerializeField] private PartyController OtherController;

    [SerializeField] private EnemyAttackBehaviorEnum EnemyAttackBehaviorEnum;

    private IEnemyAttackBehavior attackBehavior;

    private void Start()
    {
        switch (EnemyAttackBehaviorEnum)
        {
            case EnemyAttackBehaviorEnum.Random:
                attackBehavior = new RandomAttacking();
                break;
            case EnemyAttackBehaviorEnum.MaxTurnAttack:
                attackBehavior = new MaxTurnAttack();
                break;
        }
    }

    //called by UnityEvent
    public void StartTurn()
    {
        attackBehavior.SetTurnStategy(SelfController.GetHand(), SelfController.Mingmings, OtherController.Mingmings);
    }

    //Called by unityEvent
    public void StartAttackPhase() => StartCoroutine(AttackPhaseCoroutine());

    private IEnumerator AttackPhaseCoroutine()
    {
        //disable user message so not to get bombarded by failed attempts
        UserMessage.Instance.CanSendMessage = false;

        //TODO change this to animation duration
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
