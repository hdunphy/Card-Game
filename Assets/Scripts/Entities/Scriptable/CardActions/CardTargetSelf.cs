using UnityEngine;

namespace Assets.Scripts.Entities.Scriptable
{
    [CreateAssetMenu(fileName = "Target Self", menuName = "Data/Card Target/Create Target Self")]
    public class CardTargetSelf : CardTarget
    {
        public override bool IsValidAction(Monster source, Monster target, Card card)
        {
            bool notNullTarget = target != null;
            bool rightSource = (source == null || source == target);

            if (!notNullTarget)
            {
                UserMessage.Instance.SendMessageToUser("Target Monster is null");
            }
            else if (!target.IsTurn)
            {
                UserMessage.Instance.SendMessageToUser($"It is not {target.name}'s turn!");
            }
            else if (!rightSource)
            {
                UserMessage.Instance.SendMessageToUser($"Card must target itself");
            }
            return notNullTarget && target.IsTurn && card.CheckConstraints(target) && rightSource;
        }

        public override void InvokeAction(CardAction cardAction, Monster source, Monster target, Card card)
        {
            cardAction.InvokeAction(target, target, card);
        }
    }
}


