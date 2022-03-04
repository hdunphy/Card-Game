using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Entities.SaveSystem
{
    [Serializable]
    public class MonsterSaveModel
    {
        public string MonsterDataName { get; set; }
        public int Level { get; set; }
        public int CurrentHealth { get; set; }
        public int Experience { get; set; }
        public int AttackModifier { get; set; }
        public int DefenseModifier { get; set; }
        public int HealthModifier { get; set; }

        public MonsterSaveModel(MonsterInstance monsterInstance)
        {
            MonsterDataName = monsterInstance.DataName;
            Level = monsterInstance.Level;
            CurrentHealth = monsterInstance.CurrentHealth;
            Experience = monsterInstance.Experience;
            AttackModifier = monsterInstance.AttackModifier;
            DefenseModifier = monsterInstance.DefenseModifier;
            HealthModifier = monsterInstance.HealthModifier;
        }
    }
}
