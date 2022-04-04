using UnityEngine;

namespace Assets.Scripts.Entities.Scriptable.CardActions
{
    [CreateAssetMenu(fileName = "Add Energy", menuName = "Data/Card Action/Create Action Add Energy")]
    public class CardActionEnergy : CardAction
    {
        [Header("Energy")]
        [SerializeField] private int EnergyAdded;

        public override void InvokeAction(Mingming source, Mingming target, Card card)
        {
            target.AddEnergy(EnergyAdded);
            base.InvokeAction(source, target, card);
        }

        public override void PerformAnimation(Mingming source, Mingming target)
        {
            var destination = new Vector3(2, 1, 1);
            LeanTween.moveLocal(source.gameObject, destination, durationSeconds).setEaseInBounce().setLoopPingPong(1);
        }
    }
}