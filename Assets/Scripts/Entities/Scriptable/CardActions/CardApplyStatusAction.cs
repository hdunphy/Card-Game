using Assets.Scripts.Helpers;
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

        public override void InvokeAction(MingmingBattleLogic source, MingmingBattleLogic target, Card card)
        {
            //Status.ApplyStatus(target, Count);
            base.InvokeAction(source, target, card);
        }

        public override void PerformAnimation(Mingming source, Mingming target)
        {
            LeanTweenAnimations.RotateBackAndForth(target.gameObject, rotationAmountDegrees, durationSeconds / 4);
        }
    }
}