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

        public override void InvokeAction(Mingming source, Mingming target, Card card)
        {
            base.InvokeAction(source, target, card);

            if (Constraint.CanUseCard(source.Simulation, card))
            {
                Action.InvokeAction(source, target, card);
            }
        }

        public override void PerformAnimation(Mingming source, Mingming target)
        {
            // no animation
        }
    }
}