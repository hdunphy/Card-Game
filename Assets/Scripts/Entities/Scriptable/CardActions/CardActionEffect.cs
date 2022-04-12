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
        //class to add a reoccuring effect to a mingming

        [Header("Card Effect Parameters")]
        [SerializeField] private CardAction Action;
        [SerializeField] private BaseConstraint Constraint;
        [SerializeField] private TurnStateEnum TurnState;

        private readonly Dictionary<Mingming, UnityAction> MingmingActions = new Dictionary<Mingming, UnityAction>();

        public override void InvokeAction(Mingming source, Mingming target, Card card)
        {
            UnityAction _unityAction = delegate
            {
                DoEffect(source, target, card);
            };

            MingmingActions.Add(target, _unityAction);

            FindObjectsOfType<PartyController>().First(m => m.HasMingming(target))
                .AddListenerToTurnStateMachine(TurnState, _unityAction);
        }

        private void DoEffect(Mingming source, Mingming target, Card card)
        {
            if (Constraint.CanUseCard(source.Simulation, card))
            {
                Action.InvokeAction(source, target, card);
            }
            else
            {
                FindObjectsOfType<PartyController>().First(m => m.HasMingming(target))
                    .RemoveListenerToTurnStateMachine(TurnState, MingmingActions[target]);

                MingmingActions.Remove(target);
            }
        }

        public override void PerformAnimation(Mingming source, Mingming target)
        {
            //no animation
        }
    }
}