using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Entities.Scriptable.CardActions
{
    [CreateAssetMenu(fileName = "CardApplySelf", menuName = "Data/Card Action/Create Self Status Action")]
    public class CardActionSelfStatus : CardAction
    {
        [SerializeField] private BaseStatus Status;
        [SerializeField] private int Count;
        public override void InvokeAction(Monster source, Monster target, Card card)
        {
            Status.ApplyStatus(source, Count);
        }
    }
}