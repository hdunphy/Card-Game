using System;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Entities.Scriptable
{
    [CreateAssetMenu(fileName = "Card Draw", menuName = "Data/Card Action/Create Card Draw")]
    public class CardDrawAction : CardAction
    {
        [Header("Card Draw")]
        [SerializeField] private int numberOfCards;

        public override void InvokeAction(MingmingBattleLogic source, MingmingBattleLogic target, Card card)
        {
            FindObjectsOfType<PartyController>().First(m => m.HasMingming(source)).DrawCards(numberOfCards);

            base.InvokeAction(source, target, card);
        }

        public override Action<GameObject, GameObject> PerformAnimation => null;
    }
}