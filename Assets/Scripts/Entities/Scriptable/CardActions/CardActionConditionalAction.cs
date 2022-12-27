using Assets.Scripts.Entities.Mingmings;
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

        public override void InvokeAction(MingmingBattleLogic source, MingmingBattleLogic target, CardAlignment cardAlignment)
        {
            OnInvoked?.Invoke();

            if (Constraint.MingmingMeetsConstraint(source))
            {
                Action.InvokeAction(source, target, cardAlignment);
            }
        }

        public override Action<GameObject, GameObject> PerformAnimation => null;
    }
}