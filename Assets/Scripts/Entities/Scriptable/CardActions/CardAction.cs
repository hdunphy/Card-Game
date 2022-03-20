using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Entities.Scriptable
{
    public abstract class CardAction : ScriptableObject
    {
        [SerializeField] private UnityEvent OnInvoked;
        
        public virtual void InvokeAction(Monster source, Monster target, Card card)
        {
            OnInvoked?.Invoke();
        }
    }
}