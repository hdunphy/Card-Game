using System;
using UnityEngine;

namespace Assets.Scripts.Entities.Scriptable
{
    public abstract class CardTarget : ScriptableObject
    {
        [SerializeField] public Sprite Sprite;

        public abstract bool IsValidAction(Monster source, Monster target);

        public abstract void InvokeAction(CardAction cardAction, Monster source, Monster target, Card card);
    }
}


