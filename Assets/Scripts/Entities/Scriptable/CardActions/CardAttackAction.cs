using Assets.Scripts.Entities.Mingmings;
using Assets.Scripts.References;
using UnityEngine;

namespace Assets.Scripts.Entities.Scriptable
{
    [CreateAssetMenu(fileName = "CardBaseAttack", menuName = "Data/Card Action/Create Card Attack")]
    public class CardAttackAction : CardAction
    {
        [Range(0, 1.5f)]
        [SerializeField] private float attackPower;

        public float AttackPower => 100 * attackPower;
        public override void InvokeAction(MingmingBattleLogic source, MingmingBattleLogic target, CardAlignment cardAlignment)
        {
            float damage = Rules.Instance.GetAttackDamage(source, target, cardAlignment, AttackPower);
            target.TakeDamage(Mathf.FloorToInt(damage), source);

            base.InvokeAction(source, target, cardAlignment);
        }
    }
}