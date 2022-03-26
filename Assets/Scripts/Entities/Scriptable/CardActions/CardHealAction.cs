using Assets.Scripts.References;
using UnityEngine;

namespace Assets.Scripts.Entities.Scriptable
{
    [CreateAssetMenu(fileName = "CardHeal", menuName = "Data/Card Action/Create Card Heal")]
    public class CardHealAction : CardAction
    {
        public override void InvokeAction(Mingming source, Mingming target, Card card)
        {
            float heal = Rules.Instance.GetAttackDamage(source, target, card);
            heal = -0.5f * Mathf.Clamp(heal, 0, target.TotalHealth - target.CurrentHealth);
            target.TakeDamage(Mathf.FloorToInt(heal), source);

            base.InvokeAction(source, target, card);
        }
    }
}