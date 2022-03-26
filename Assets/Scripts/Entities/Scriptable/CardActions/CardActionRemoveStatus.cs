using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Entities.Scriptable.CardActions
{
    [CreateAssetMenu(fileName = "Remove Status", menuName = "Data/Card Action/Create Card Remove Status")]
    public class CardActionRemoveStatus : CardAction
    {
        [SerializeField] private BaseStatus Status;
        public override void InvokeAction(Mingming source, Mingming target, Card card)
        {
            Status.RemoveStatus(target);

            base.InvokeAction(source, target, card);
        }
    }
}