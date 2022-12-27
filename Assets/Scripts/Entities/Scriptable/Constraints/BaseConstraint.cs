using Assets.Scripts.Entities.Mingmings;
using UnityEngine;

namespace Assets.Scripts.Entities.Scriptable
{
    public abstract class BaseConstraint : ScriptableObject
    {
        public virtual bool MingmingMeetsConstraint(MingmingBattleLogic source) => true;

        public virtual bool CanUseCard(MingmingBattleLogic source, Card card)
        {
            bool hasEnergy = source.EnergyAvailable >= card.EnergyCost;

            if (!hasEnergy)
            {
                UserMessage.Instance.SendMessageToUser($"{source.Name} does not have the required energy. Needs {card.EnergyCost}");
            }
            return hasEnergy && MingmingMeetsConstraint(source);
        }
    }
}