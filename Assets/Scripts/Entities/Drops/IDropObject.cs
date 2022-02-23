using System;
using UnityEngine;

namespace Assets.Scripts.Entities.Drops
{
    public abstract class IDropObject : MonoBehaviour
    {
        public abstract void AddScriptable(IDropScriptableObject dropScriptable);
        protected abstract void AddDropObjectToPlayer(PlayerController playerController);

    }

    public interface IDropScriptableObject { }


    [Serializable]
    public struct DropChance
    {
        [Range(0, 1)]
        public float RollChance;
        public bool IsEmpty;

        public IDropScriptableObject DropObject;
    }
}
