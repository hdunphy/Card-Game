using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Entities.Scriptable
{
    [CreateAssetMenu(fileName = "CardBaseAttack", menuName = "Data/Create Card Attack")]
    public class CardAttackAction : CardAction
    {
        public override void InvokeAction(Monster source, Monster target, Card card)
        {
            source.AttackMonster(target, card);
        }
    }
}