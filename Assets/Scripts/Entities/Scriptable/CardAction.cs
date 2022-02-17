using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Entities.Scriptable
{
    //[CreateAssetMenu(fileName = "CardAction", menuName = "Data/Create Card Action")]
    public abstract class CardAction : ScriptableObject
    {
        public abstract void InvokeAction(Monster source, Monster target, Card card);
    }
}