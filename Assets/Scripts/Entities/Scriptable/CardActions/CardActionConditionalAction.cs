using UnityEngine;

namespace Assets.Scripts.Entities.Scriptable.CardActions
{
    [CreateAssetMenu(menuName = "Data/Card Action/Create Conditional Action")]
    public class CardActionConditionalAction : CardAction
    {
        [SerializeField] private CardAction Action;
        [SerializeField] private BaseConstraint Constraint;

        public override void InvokeAction(Mingming source, Mingming target, Card card)
        {
            base.InvokeAction(source, target, card);

            if (Constraint.CheckConstraint(source, card))
            {
                Action.InvokeAction(source, target, card);
            }
        }
    }
}