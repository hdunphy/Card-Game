using System;
using UnityEngine;

namespace Assets.Scripts.Entities.Scriptable
{
    public abstract class CardTarget : ScriptableObject
    {
        [SerializeField] public Sprite Sprite;

        public virtual bool IsValidAction(Monster source, Monster target, Card card)
            => source != null && target != null && source.IsTurn && source.EnergyAvailable >= card.EnergyCost;

        public abstract void InvokeAction(CardAction cardAction, Monster source, Monster target, Card card);
    }
}


