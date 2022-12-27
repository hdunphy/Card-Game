using Assets.Scripts.Entities.Mingmings;
using Assets.Scripts.Helpers;
using System;
using UnityEngine;

namespace Assets.Scripts.Entities.Scriptable.CardActions
{
    [CreateAssetMenu(fileName = "CardApplySelf", menuName = "Data/Card Action/Create Self Status Action")]
    public class CardActionSelfStatus : CardAction
    {
        [SerializeField] private float rotationAmountDegrees = 25f;

        [Header("Status Parameters")]
        [SerializeField] private BaseStatus Status;
        [SerializeField] private int Count;
        public override void InvokeAction(MingmingBattleLogic source, MingmingBattleLogic target, CardAlignment cardAlignment)
        {
            Status.ApplyStatus(source, Count);
            
            base.InvokeAction(source, target, cardAlignment);
        }

        public override Action<GameObject, GameObject> PerformAnimation
            => (source, _) =>
            {
                LeanTweenAnimations.RotateBackAndForth(source, rotationAmountDegrees, durationSeconds / 4);
            };
    }
}