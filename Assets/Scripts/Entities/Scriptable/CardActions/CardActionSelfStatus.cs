using UnityEngine;

namespace Assets.Scripts.Entities.Scriptable.CardActions
{
    [CreateAssetMenu(fileName = "CardApplySelf", menuName = "Data/Card Action/Create Self Status Action")]
    public class CardActionSelfStatus : CardAction
    {
        [SerializeField] private BaseStatus Status;
        [SerializeField] private int Count;
        public override void InvokeAction(Mingming source, Mingming target, Card card)
        {
            Status.ApplyStatus(source, Count);
            base.InvokeAction(source, target, card);
        }

        public override void PerformAnimation(Mingming source, Mingming target)
        {
            //no animation move animations to statuses

            //var destination = new Vector3(1, 2, 1);
            //LeanTween.moveLocal(target.gameObject, destination, durationSeconds).setEaseInBounce().setLoopPingPong(1);
        }
    }
}