﻿using Assets.Scripts.Controller.References;
using Assets.Scripts.Entities.Interfaces;
using Assets.Scripts.Entities.Mingmings;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.Entities.Player
{

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