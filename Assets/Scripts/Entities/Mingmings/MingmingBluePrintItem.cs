using Assets.Scripts.Entities.Interfaces;
using System;

namespace Assets.Scripts.Entities.Mingmings
{
    [Serializable]
    public class MingmingBluePrintItem : IItem
    {
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
            if (Current + value > MaxRequired)
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
}