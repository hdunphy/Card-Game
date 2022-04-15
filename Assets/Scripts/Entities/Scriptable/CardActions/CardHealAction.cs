using Assets.Scripts.References;
using System;
using UnityEngine;

namespace Assets.Scripts.Entities.Scriptable
{
    [CreateAssetMenu(fileName = "CardHeal", menuName = "Data/Card Action/Create Card Heal")]
    public class CardHealAction : CardAction
    {
        [SerializeField] private float animationHeightMovement;

        public override void InvokeAction(MingmingBattleLogic source, MingmingBattleLogic target, Card card)
        {
            int healAmount = Rules.GetHealAmount(source, target, card);
            target.TakeDamage(-healAmount, source);

            base.InvokeAction(source, target, card);
        }

        public override Action<GameObject, GameObject> PerformAnimation
            => (_, target) =>
            {
                LeanTween.moveLocalY(target, animationHeightMovement, durationSeconds / 2).setLoopPingPong(3);
            };
    }
}