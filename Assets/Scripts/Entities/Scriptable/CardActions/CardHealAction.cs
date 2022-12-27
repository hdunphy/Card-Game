using Assets.Scripts.Entities.Mingmings;
using Assets.Scripts.References;
using System;
using UnityEngine;

namespace Assets.Scripts.Entities.Scriptable
{
    [CreateAssetMenu(fileName = "CardHeal", menuName = "Data/Card Action/Create Card Heal")]
    public class CardHealAction : CardAction
    {
        [SerializeField] private float animationHeightMovement;
        [Range(0, 1.5f)]
        [SerializeField] private float healPower;

        public float HealPower => 100 * healPower;

        public override void InvokeAction(MingmingBattleLogic source, MingmingBattleLogic target, CardAlignment cardAlignment)
        {
            int healAmount = Rules.GetHealAmount(source, target, HealPower);
            target.TakeDamage(-healAmount, source);

            base.InvokeAction(source, target, cardAlignment);
        }

        public override Action<GameObject, GameObject> PerformAnimation
            => (_, target) =>
            {
                LeanTween.moveLocalY(target, animationHeightMovement, durationSeconds / 2).setLoopPingPong(3);
            };
    }
}