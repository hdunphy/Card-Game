using UnityEngine;

namespace Assets.Scripts.Entities.Scriptable
{
    [CreateAssetMenu(fileName = "CardApplyStatus", menuName = "Data/Card Action/Create Card Apply Status")]
    public class CardApplyStatusAction : CardAction
    {
        [SerializeField] private BaseStatus Status;
        [SerializeField] private int Count;
        public override void InvokeAction(Monster source, Monster target, Card card)
        {
            Status.ApplyStatus(target, Count);
        }
    }
}