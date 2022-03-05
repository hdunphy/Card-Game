using System;
using UnityEngine;

namespace Assets.Scripts.Entities.Scriptable
{
    public abstract class CardTarget : ScriptableObject
    {
        [SerializeField] public Sprite Sprite;

        public virtual bool IsValidAction(Monster source, Monster target, Card card)
        {
            bool notNullSource = source != null;
            bool notNullTarget = target != null;
            if (!notNullSource) {
                UserMessage.Instance.SendMessageToUser("Source Monster is null");
            }
            else if(!notNullTarget){
                UserMessage.Instance.SendMessageToUser("Target Monster is null");
            }
            else if (!source.IsTurn)
            {
                UserMessage.Instance.SendMessageToUser($"It is not {source.name}'s turn!");
            }
            return notNullSource && notNullTarget && source.IsTurn && card.CheckConstraints(source);
        }

        public abstract void InvokeAction(CardAction cardAction, Monster source, Monster target, Card card);
    }
}


