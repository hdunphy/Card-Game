using UnityEngine;

namespace Assets.Scripts.Entities.Scriptable.CardActions
{
    [CreateAssetMenu(fileName = "Add Energy", menuName = "Data/Card Action/Create Action Add Energy")]
    public class CardActionEnergy : CardAction
    {
        [SerializeField] private int EnergyAdded;

        public override void InvokeAction(Mingming source, Mingming target, Card card)
        {
            target.AddEnergy(EnergyAdded);
            base.InvokeAction(source, target, card);
        }
    }
}