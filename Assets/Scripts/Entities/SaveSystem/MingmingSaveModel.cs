using Assets.Scripts.Entities.Mingmings;
using System;

namespace Assets.Scripts.Entities.SaveSystem
{
    [Serializable]
    public class MingmingSaveModel
    {
        public string MingmingDataName { get; set; }
        public int Level { get; set; }
        public int CurrentHealth { get; set; }
        public int Experience { get; set; }
        public int AttackModifier { get; set; }
        public int DefenseModifier { get; set; }
        public int HealthModifier { get; set; }

        public MingmingSaveModel(MingmingInstance mingmingInstance)
        {
            MingmingDataName = mingmingInstance.DataName;
            Level = mingmingInstance.Level;
            CurrentHealth = mingmingInstance.CurrentHealth;
            Experience = mingmingInstance.Experience;
            AttackModifier = mingmingInstance.AttackModifier;
            DefenseModifier = mingmingInstance.DefenseModifier;
            HealthModifier = mingmingInstance.HealthModifier;
        }
    }
}
