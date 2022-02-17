using UnityEngine;

namespace Assets.Scripts.Entities.Scriptable
{
    [CreateAssetMenu(fileName = "CardHeal", menuName = "Data/Create Card Heal")]
    public class CardHealAction : CardAction
    {
        public override void InvokeAction(Monster source, Monster target, Card card)
        {
            source.HealMonster(target, card);
        }
    }
}