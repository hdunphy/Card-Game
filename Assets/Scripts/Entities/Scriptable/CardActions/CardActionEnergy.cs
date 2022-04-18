using System;
using UnityEngine;

namespace Assets.Scripts.Entities.Scriptable.CardActions
{
    [CreateAssetMenu(fileName = "Add Energy", menuName = "Data/Card Action/Create Action Add Energy")]
    public class CardActionEnergy : CardAction
    {
        private const int SCORE_PER_ENERGY = 10;

        [Header("Energy")]
        [SerializeField] private int EnergyAdded;

        public override int ActionScore => EnergyAdded * SCORE_PER_ENERGY;

        public override void InvokeAction(MingmingBattleLogic source, MingmingBattleLogic target, Card card)
        {
            target.AddEnergy(EnergyAdded);
            base.InvokeAction(source, target, card);
        }

        public override Action<GameObject, GameObject> PerformAnimation
            => (_, target) =>
            {
                var destination = new Vector3(2, 1, 1);
                LeanTween.moveLocal(target, destination, durationSeconds).setEaseInBounce().setLoopPingPong(1);
            };
    }
}