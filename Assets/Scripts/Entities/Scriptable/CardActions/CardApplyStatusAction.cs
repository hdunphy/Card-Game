using Assets.Scripts.Helpers;
using System;
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

        public override void InvokeAction(MingmingBattleLogic source, MingmingBattleLogic target, CardAlignment cardAlignment)
        {
            Status.ApplyStatus(target, Count);
            base.InvokeAction(source, target, cardAlignment);
        }

        public override Action<GameObject, GameObject> PerformAnimation
            => (_, target) =>
            {
                LeanTweenAnimations.RotateBackAndForth(target.gameObject, rotationAmountDegrees, durationSeconds / 4);
            };
    }
}