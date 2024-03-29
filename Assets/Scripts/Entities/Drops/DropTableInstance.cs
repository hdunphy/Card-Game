﻿using Assets.Scripts.References;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Entities.Drops
{
    public class DropTableInstance : MonoBehaviour
    {
        [SerializeField] private List<DropChance> drops;

        private DropTable DropTable;

        private void Start()
        {
            DropTable = new DropTable(drops);
        }

        public IDropScriptableObject GetDrop() => DropTable.GetDrop();
    }

    public class DropTable
    {
        private readonly List<DropChance> drops;

        public DropTable(List<DropChance> drops)
        {
            this.drops = drops;
        }

        public IDropScriptableObject GetDrop()
        {
            IDropScriptableObject dropObject = null;
            float currentChance = 0;
            float roll = Rules.GetRandomFloat();

            foreach (DropChance _drop in drops)
            {
                currentChance += _drop.RollChance;
                if (roll <= currentChance)
                {
                    if (!_drop.IsEmpty)
                        dropObject = _drop.DropObject;
                    break;
                }
            }

            return dropObject;
        }
    }
}
