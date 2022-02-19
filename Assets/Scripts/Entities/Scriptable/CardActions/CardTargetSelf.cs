using UnityEngine;

namespace Assets.Scripts.Entities.Scriptable
{
    [CreateAssetMenu(fileName = "Target Self", menuName = "Data/Card Target/Create Target Self")]
    public class CardTargetSelf : CardTarget
    {
        public override bool IsValidAction(Monster source, Monster target, Card card)
            => target != null && target.IsTurn && target.EnergyAvailable >= card.EnergyCost && (source == null || source == target);

        public override void InvokeAction(CardAction cardAction, Monster source, Monster target, Card card)
        {
            cardAction.InvokeAction(target, target, card);
            target.PlayCard(card);
        }
    }
}


