using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Entities.Scriptable
{
    [CreateAssetMenu(fileName = "Card Draw", menuName = "Data/Card Action/Create Card Draw")]
    public class CardDrawAction : CardAction
    {
        [Header("Card Draw")]
        [SerializeField] private int numberOfCards;

        public override void InvokeAction(Mingming source, Mingming target, Card card)
        {
            FindObjectsOfType<MingmingController>().First(m => m.HasMingming(source)).DrawCards(numberOfCards);

            base.InvokeAction(source, target, card);
        }

        public override void PerformAnimation(Mingming source, Mingming target)
        {
            //no animation
        }
    }
}