using Assets.Scripts.Controller.References;
using Assets.Scripts.Entities.Interfaces;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.Entities.Player
{
    [Serializable]
    public class MingmingBluePrintItem : IItem { 
        public int MaxRequired { get; }
        public int Current { get; private set; }
        public float PercentComplete => (float)Current / MaxRequired;

        public MingmingBluePrintItem(int maxRequired, int current)
        {
            MaxRequired = maxRequired;
            Current = current;
        }

        public int Add(int value)
        {
            if(Current + value > MaxRequired)
            {
                Current = MaxRequired;
            }
            else
            {
                Current += value;
            }

            return Current;
        }
    }

    public interface IInventory
    {
        int AddBlueprint(string name, int count);
    }

    [Serializable]
    public class PlayerInventory : IInventory
    {
        private readonly Dictionary<string, MingmingBluePrintItem> _blueprints;

        public PlayerInventory()
        {
            _blueprints = new();
        }

        public int AddBlueprint(string name, int count)
        {
            int currentCount = count;
            if (_blueprints.ContainsKey(name)){
                currentCount = _blueprints[name].Add(count);
            }
            else
            {
                var mingming = ScriptableObjectReferenceSingleton.Singleton.GetScriptableObject<MingmingData>(name);
                _blueprints.Add(name, new MingmingBluePrintItem(mingming.MaxBlueprintsRequired, count));
            }

            return currentCount;
        }
    }
}