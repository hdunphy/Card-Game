using System;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Entities.Scriptable.CardActions
{
    [CreateAssetMenu(menuName = "Data/Card Action/Create Conditional Action")]
    public class CardActionConditionalAction : CardAction
    {
        [Header("Conditional Parameters")]
        [SerializeField] private CardAction Action;
        [SerializeField] private BaseConstraint Constraint;

        public override void InvokeAction(MingmingBattleLogic source, MingmingBattleLogic target, Card card)
        {
            OnInvoked?.Invoke();

            if (Constraint.CanUseCard(source, card))
            {
                Action.InvokeAction(source, target, card);
            }
        }

        public override Action<GameObject, GameObject> PerformAnimation => null;
    }
}