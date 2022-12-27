using Assets.Scripts.Entities.Mingmings;
using UnityEngine;

namespace Assets.Scripts.Entities.Scriptable
{
    [CreateAssetMenu(fileName = "Target Self", menuName = "Data/Card Target/Create Target Self")]
    public class CardTargetSelf : CardTarget
    {
        public override string TooltipText => "Can target itself (the selected Mingming)";

        public override int ScoreModifier => 1;

        public override bool IsValidTarget(Mingming source, Mingming target, Card card)
        {
            bool notNullTarget = target != null;
            bool rightSource = (source == null || source == target);

            if (!notNullTarget)
            {
                UserMessage.Instance.SendMessageToUser("Target Mingming is null");
            }
            else if (!target.IsTurn)
            {
                UserMessage.Instance.SendMessageToUser($"It is not {target.name}'s turn!");
            }
            else if (!rightSource)
            {
                UserMessage.Instance.SendMessageToUser($"Card must target itself");
            }
            return notNullTarget && target.IsTurn && rightSource;
        }

        public override void InvokeAction(CardAction cardAction, MingmingBattleLogic source, MingmingBattleLogic target, Card card)
        {
            cardAction.InvokeAction(target, target, card.CardAlignment);
        }
    }
}


