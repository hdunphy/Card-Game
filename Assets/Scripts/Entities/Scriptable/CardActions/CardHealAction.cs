using Assets.Scripts.References;
using UnityEngine;

namespace Assets.Scripts.Entities.Scriptable
{
    [CreateAssetMenu(fileName = "CardHeal", menuName = "Data/Card Action/Create Card Heal")]
    public class CardHealAction : CardAction
    {
        [SerializeField] private float animationHeightMovement;

        public override void InvokeAction(Mingming source, Mingming target, Card card)
        {
            float heal = Rules.Instance.GetAttackDamage(source.Simulation, target.Simulation, card);
            //TODO: move clamp out of here
            heal = -0.5f * Mathf.Clamp(heal, 0, target.Simulation.TotalHealth - target.Simulation.CurrentHealth);
            target.TakeDamage(Mathf.FloorToInt(heal), source);

            base.InvokeAction(source, target, card);
        }

        public override void PerformAnimation(Mingming source, Mingming target)
        {
            LeanTween.moveLocalY(target.gameObject, animationHeightMovement, durationSeconds / 2).setLoopPingPong(3);
        }
    }
}