using UnityEngine;

namespace Assets.Scripts.Entities.Scriptable
{
    [CreateAssetMenu(fileName = "CardApplyStatus", menuName = "Data/Card Action/Create Card Apply Status")]
    public class CardApplyStatusAction : CardAction
    {
        [SerializeField] private BaseStatus Status;
        [SerializeField] private int Count;
        public override void InvokeAction(Mingming source, Mingming target, Card card)
        {
            Status.ApplyStatus(target, Count);
            base.InvokeAction(source, target, card);
        }
    }
}