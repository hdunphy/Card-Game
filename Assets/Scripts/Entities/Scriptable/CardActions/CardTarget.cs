using System;
using UnityEngine;

namespace Assets.Scripts.Entities.Scriptable
{
    public abstract class CardTarget : ScriptableObject
    {
        [SerializeField] public Sprite Sprite;

        public abstract string TooltipText { get; }

        public virtual bool IsValidTarget(Mingming source, Mingming target, Card card)
        {
            bool notNullSource = source != null;
            bool notNullTarget = target != null;
            if (!notNullSource) {
                UserMessage.Instance.SendMessageToUser("Source Mingming is null");
            }
            else if(!notNullTarget){
                UserMessage.Instance.SendMessageToUser("Target Mingming is null");
            }
            else if (!source.IsTurn)
            {
                UserMessage.Instance.SendMessageToUser($"It is not {source.name}'s turn!");
            }
            return notNullSource && notNullTarget && source.IsTurn;
        }

        public abstract void InvokeAction(CardAction cardAction, MingmingBattleLogic source, MingmingBattleLogic target, Card card);
    }
}


