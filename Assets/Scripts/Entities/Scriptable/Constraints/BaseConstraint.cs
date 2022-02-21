using UnityEngine;

namespace Assets.Scripts.Entities.Scriptable
{
    public abstract class BaseConstraint : ScriptableObject
    {
        public virtual bool CheckConstraint(Monster source, Card card) => source.EnergyAvailable >= card.EnergyCost;
    }
}