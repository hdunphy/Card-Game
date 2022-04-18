using UnityEngine;

namespace Assets.Scripts.Entities.Scriptable
{
    [CreateAssetMenu(fileName = "Target Single", menuName = "Data/Card Target/Create Target Single")]
    public class CardTargetSingle : CardTarget
    {
        public override string TooltipText => "Can target any single Mingming. Friend or Foe";

        public override int ScoreModifier => 1;

        public override void InvokeAction(CardAction cardAction, MingmingBattleLogic source, MingmingBattleLogic target, Card card)
        {
            cardAction.InvokeAction(source, target, card);
        }
    }
}


