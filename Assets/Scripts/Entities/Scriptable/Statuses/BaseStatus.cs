using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Entities.Scriptable
{
    public abstract class BaseStatus : ScriptableObject
    {
        [SerializeField] private Sprite Sprite;

        public virtual void ApplyStatus(Monster monster)
        {
            monster.ApplyStatus(this);
        }

        public virtual void RemoveStatus(Monster monster)
        {
        }

        public virtual int GetCount() => 1;

        public abstract void DoEffect(Monster monster, int count);

    }
}
