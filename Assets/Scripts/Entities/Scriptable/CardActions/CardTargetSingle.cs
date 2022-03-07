using UnityEngine;

namespace Assets.Scripts.Entities.Scriptable
{
    [CreateAssetMenu(fileName = "Target Single", menuName = "Data/Card Target/Create Target Single")]
    public class CardTargetSingle : CardTarget
    {
        public override void InvokeAction(CardAction cardAction, Monster source, Monster target, Card card)
        {
            cardAction.InvokeAction(source, target, card);
        }
    }
}


