using Assets.Scripts.Entities.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Entities.Player
{
    public class MingmingBluePrintItem : IItem { 
        public int MaxRequired { get; }
        public int Current { get; private set; }
        public string Name { get; }
        public float PercentComplete => (float)Current / MaxRequired;

        public MingmingBluePrintItem(int maxRequired, int current, string name)
        {
            MaxRequired = maxRequired;
            Current = current;
            Name = name;
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

    public class PlayerInventory
    {
        private readonly Dictionary<int, MingmingBluePrintItem> _blueprints;

        public PlayerInventory()
        {
            _blueprints = new();
        }
    }
}