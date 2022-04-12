using Assets.Scripts.Entities.Scriptable;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.Entities
{
    public class MingmingBattleSimulation
    {
        private MingmingInstance data { get; set; }
        private Dictionary<BaseStatus, StatusIcon> statuses { get; set; }

        public string Name { get; private set; }
        public int EnergyAvailable { get; private set; }
        public float AttackModifier { get; set; }
        public float DefenseModifier { get; set; }
        public int TotalEnergy => data.Energy;
        public int TotalHealth => data.Health;
        public int CurrentHealth
        {
            get => data.CurrentHealth;
            private set => data.CurrentHealth = value;
        }
        public int Level => data.Level;
        public float Attack => data.Attack * AttackModifier;
        public float Defense => data.Defense * DefenseModifier;

        public MingmingAlignment GetMingmingAlignment => data.MingmingAlignment;

        public string GetTooltipInfo() => $"Level: {data.Level}\nAttack: {data.Attack}\nDefense: {data.Defense}\nExp: {data.Experience}";

        public MingmingBattleSimulation(MingmingBattleSimulation simulation)
        {
            data = new MingmingInstance( simulation.data);
            AttackModifier = simulation.AttackModifier;
            DefenseModifier = simulation.DefenseModifier;
            EnergyAvailable = simulation.EnergyAvailable;
            statuses = simulation.statuses;
            Name = simulation.Name;
        }

        public MingmingBattleSimulation(MingmingInstance data, string name)
        {
            this.data = data;
            Name = name;
            EnergyAvailable = TotalEnergy;
            AttackModifier = 1;
            DefenseModifier = 1;

            statuses = new Dictionary<BaseStatus, StatusIcon>();
        }

        #region Game Logic
        public void StartTurn()
        {
            EnergyAvailable = TotalEnergy;
        }

        public void AddEnergy(int energy)
        {
            EnergyAvailable += energy;
        }

        public void PlayCard(Card selectedCard)
        {
            EnergyAvailable -= selectedCard.EnergyCost;
        }

        public void TakeDamage(int damage)
        {
            CurrentHealth -= damage;
        }
        #endregion

        #region Exp
        public int GetDeathExp() => data.GetDeathExp();

        public float GetExperiencePercentage() => data.GetExperiencePercentage();

        public int AddExperience(int expGained) => data.AddExperience(expGained);
        #endregion

        #region Statuses
        public int AddCount(BaseStatus status, int _count) => statuses[status].AddCount(_count);

        public StatusIcon RemoveStatus(BaseStatus status)
        {
            var icon = statuses[status];
            statuses.Remove(status);

            return icon;
        }

        public void AddStatus(BaseStatus status, StatusIcon icon) => statuses.Add(status, icon);

        public bool HasStatus(BaseStatus status) => statuses.ContainsKey(status);

        public int GetStatusCount(BaseStatus status) => HasStatus(status) ? statuses[status].Count : 0;

        public List<BaseStatus> GetStatusList() => statuses.Keys.ToList();
        #endregion
    }
}