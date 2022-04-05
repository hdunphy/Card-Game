using UnityEngine;

namespace Assets.Scripts.Entities.Scriptable
{
    [CreateAssetMenu(fileName = "CardApplyStatus", menuName = "Data/Card Action/Create Card Apply Status")]
    public class CardApplyStatusAction : CardAction
    {
        [SerializeField] private float rotationAmountDegrees;

        [Header("Status Parameters")]
        [SerializeField] private BaseStatus Status;
        [SerializeField] private int Count;
        public override void InvokeAction(Mingming source, Mingming target, Card card)
        {
            Status.ApplyStatus(target, Count);
            base.InvokeAction(source, target, card);
        }

        public override void PerformAnimation(Mingming source, Mingming target)
        {
            LeanTween.rotateZ(source.gameObject, rotationAmountDegrees, durationSeconds / 4).setLoopPingPong(3);

            LeanTween.delayedCall(durationSeconds / 2,
                () => LeanTween.rotateZ(target.gameObject, -rotationAmountDegrees, durationSeconds / 4).setLoopPingPong(3));
        }
    }
}