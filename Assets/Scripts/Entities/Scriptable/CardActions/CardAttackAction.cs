using Assets.Scripts.References;
using UnityEngine;

namespace Assets.Scripts.Entities.Scriptable
{
    [CreateAssetMenu(fileName = "CardBaseAttack", menuName = "Data/Card Action/Create Card Attack")]
    public class CardAttackAction : CardAction
    {
        public override void InvokeAction(Monster source, Monster target, Card card)
        {
            float damage = Rules.Instance.GetAttackDamage(source, target, card);
            target.TakeDamage(Mathf.FloorToInt(damage), source);

            source.PlayCard(card);
        }
    }
}