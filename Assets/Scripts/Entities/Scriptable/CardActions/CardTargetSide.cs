using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Entities.Scriptable
{
    [CreateAssetMenu(fileName = "Target Side", menuName = "Data/Card Target/Create Target Side")]
    public class CardTargetSide : CardTarget
    {
        public override string TooltipText => "Can target a whole team. Select one monster and card effects apply to other monsters on that team";

        public override void InvokeAction(CardAction cardAction, Mingming source, Mingming target, Card card)
        {
            var controller = FindObjectsOfType<MonsterController>().First(m => m.HasMonster(target)); //should never be null
            foreach (var monster in controller.Monsters.Where(m => m.IsInPlay))
            {
                cardAction.InvokeAction(source, monster, card);
            }
        }
    }
}


