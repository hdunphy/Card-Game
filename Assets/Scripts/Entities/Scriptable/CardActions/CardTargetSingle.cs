using UnityEngine;

namespace Assets.Scripts.Entities.Scriptable
{
    [CreateAssetMenu(fileName = "Target Single", menuName = "Data/Card Target/Create Target Single")]
    public class CardTargetSingle : CardTarget
    {
        public override string TooltipText => "Can target any single monster. Friend or Foe";

        public override void InvokeAction(CardAction cardAction, Mingming source, Mingming target, Card card)
        {
            cardAction.InvokeAction(source, target, card);
        }
    }
}


