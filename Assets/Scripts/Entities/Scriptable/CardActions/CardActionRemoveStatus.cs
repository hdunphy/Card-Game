using Assets.Scripts.Entities.Mingmings;
using System;
using UnityEngine;

namespace Assets.Scripts.Entities.Scriptable.CardActions
{
    [CreateAssetMenu(fileName = "Remove Status", menuName = "Data/Card Action/Create Card Remove Status")]
    public class CardActionRemoveStatus : CardAction
    {
        [Header("Status Parameters")]
        [SerializeField] private BaseStatus Status;

        public override void InvokeAction(MingmingBattleLogic source, MingmingBattleLogic target, CardAlignment cardAlignment)
        {
            Status.RemoveStatus(target);

            base.InvokeAction(source, target, cardAlignment);
        }

        public override Action<GameObject, GameObject> PerformAnimation => null;
            //no animation move animations to statuses

            //var destination = new Vector3(1, -2, 1);
            //LeanTween.moveLocal(target.gameObject, destination, durationSeconds).setEaseInBounce().setLoopPingPong(1);
    }
}