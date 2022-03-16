using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Entities.Scriptable
{
    public abstract class CardAction : ScriptableObject
    {
        [SerializeField] protected AudioClip Clip;
        
        public virtual void InvokeAction(Monster source, Monster target, Card card)
        {
            //Need someway to adjust volume here. Maybe global volume settings?
            AudioSource.PlayClipAtPoint(Clip, target.gameObject.transform.position);
        }
    }
}