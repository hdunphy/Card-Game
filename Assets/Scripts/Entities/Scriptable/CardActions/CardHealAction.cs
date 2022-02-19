using Assets.Scripts.References;
using UnityEngine;

namespace Assets.Scripts.Entities.Scriptable
{
    [CreateAssetMenu(fileName = "CardHeal", menuName = "Data/Card Action/Create Card Heal")]
    public class CardHealAction : CardAction
    {
        public override void InvokeAction(Monster source, Monster target, Card card)
        {
            float heal = Rules.Instance.GetAttackDamage(source, target, card);
            heal = -0.5f * Mathf.Clamp(heal, 0, target.MissingHealth);
            target.TakeDamage(Mathf.FloorToInt(heal), source);
        }
    }
}