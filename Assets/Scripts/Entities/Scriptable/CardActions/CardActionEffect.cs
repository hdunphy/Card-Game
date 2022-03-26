using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Entities.Scriptable.CardActions
{
    [CreateAssetMenu(fileName = "CardEffect", menuName = "Data/Card Action/Create Card Effect")]
    public class CardActionEffect : CardAction
    {
        [SerializeField] private CardAction Action;
        [SerializeField] private BaseConstraint Constraint;
        [SerializeField] private TurnStateEnum TurnState;

        private readonly Dictionary<Mingming, UnityAction> MonsterActions = new Dictionary<Mingming, UnityAction>();

        public override void InvokeAction(Mingming source, Mingming target, Card card)
        {
            UnityAction _unityAction = delegate
            {
                DoEffect(source, target, card);
            };

            MonsterActions.Add(target, _unityAction);

            FindObjectsOfType<MonsterController>().First(m => m.HasMonster(target))
                .AddListenerToTurnStateMachine(TurnState, _unityAction);
        }

        private void DoEffect(Mingming source, Mingming target, Card card)
        {
            if (Constraint.CheckConstraint(source, card))
            {
                Action.InvokeAction(source, target, card);
            }
            else
            {
                FindObjectsOfType<MonsterController>().First(m => m.HasMonster(target))
                    .RemoveListenerToTurnStateMachine(TurnState, MonsterActions[target]);

                MonsterActions.Remove(target);
            }
        }
    }
}