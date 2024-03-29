﻿using Assets.Scripts.Entities.Mingmings;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Entities.Scriptable
{
    [CreateAssetMenu(fileName = "Target Side", menuName = "Data/Card Target/Create Target Side")]
    public class CardTargetSide : CardTarget
    {
        public override string TooltipText => "Can target a whole team. Select one Mingming and card effects apply to other Mingming on that team";

        public override int ScoreModifier => 5;

        public override void InvokeAction(CardAction cardAction, MingmingBattleLogic source, MingmingBattleLogic target, Card card)
        {
            var controller = FindObjectsOfType<PartyController>().First(m => m.HasMingming(target)); //should never be null
            foreach (var mingming in controller.Mingmings.Where(m => m.IsInPlay))
            {
                cardAction.InvokeAction(source, mingming.Logic, card.CardAlignment);
            }
        }
    }
}


