using Assets.Scripts.References;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Entities.Drops
{
    public class DropTable : MonoBehaviour
    {
        [SerializeField] private List<DropChance> drops;

        public IDropScriptableObject GetDrop()
        {
            IDropScriptableObject dropObject = null;
            float Total = drops.Sum(x => x.RollChance);
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
