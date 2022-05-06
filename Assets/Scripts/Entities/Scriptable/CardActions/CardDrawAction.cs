using System;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Entities.Scriptable
{
    [CreateAssetMenu(fileName = "Card Draw", menuName = "Data/Card Action/Create Card Draw")]
    public class CardDrawAction : CardAction
    {
        private const int SCORE_PER_CARD_DRAW = 15;

        [Header("Card Draw")]
        [SerializeField] private int numberOfCards;

        public override int ActionScore => numberOfCards * SCORE_PER_CARD_DRAW;

        public override void InvokeAction(MingmingBattleLogic source, MingmingBattleLogic target, CardAlignment cardAlignment)
        {
            FindObjectsOfType<PartyController>().First(m => m.HasMingming(source)).DrawCards(numberOfCards);

            base.InvokeAction(source, target, cardAlignment);
        }

        public override Action<GameObject, GameObject> PerformAnimation => null;
    }
}