using Assets.Scripts.Entities.Scriptable;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.Entities
{
    public class MingmingBattleLogic
    {

        /* --Private Properties-- */
        private readonly MingmingInstance _data;
        private readonly Dictionary<BaseStatus, int> _statuses;
        private int _energyAvailable;

        /* --Public Properties-- */
        public string Name { get; private set; }
        public float AttackModifier { get; set; }
        public float DefenseModifier { get; set; }
        public int TotalEnergy => _data.Energy;
        public int TotalHealth => _data.Health;
        public int Level => _data.Level;
        public float Attack => _data.Attack * AttackModifier;
        public float Defense => _data.Defense * DefenseModifier;
        public int EnergyAvailable
        {
            get => _energyAvailable;
            private set
            {
                _energyAvailable = value;
                OnEnergyChanged?.Invoke(value);
            }
        }
        public int CurrentHealth
        {
            get => _data.CurrentHealth;
            private set => _data.CurrentHealth = value;
        }
        public MingmingAlignment GetMingmingAlignment => _data.MingmingAlignment;
        public string GetTooltipInfo() => $"Level: {_data.Level}\nAttack: {_data.Attack}\nDefense: {_data.Defense}\nExp: {_data.Experience}";

        /* --Events-- */
        public event Action OnCardPlayed;
        public event Action<int> OnEnergyChanged;
        public event Action<int, MingmingBattleLogic> OnTakeDamage; //Damage, source
        public event Action<BaseStatus, int> OnStatusUpdated;
        public event Action<BaseStatus, int> OnStatusAdded;
        public event Action<BaseStatus> OnStatusRemoved;

        public MingmingBattleLogic(MingmingBattleLogic simulation)
        {
            _data = new MingmingInstance(simulation._data);
            AttackModifier = simulation.AttackModifier;
            DefenseModifier = simulation.DefenseModifier;
            EnergyAvailable = simulation.EnergyAvailable;
            _statuses = simulation._statuses;
            Name = simulation.Name;
        }

        public MingmingBattleLogic(MingmingInstance data, string name)
        {
            _data = data;
            Name = name;
            EnergyAvailable = TotalEnergy;
            AttackModifier = 1;
            DefenseModifier = 1;

            _statuses = new Dictionary<BaseStatus, int>();
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
            EventManager.Instance.OnDiscardCardTrigger(selectedCard);
            EnergyAvailable -= selectedCard.EnergyCost;
        }

        public void TakeDamage(int damage, MingmingBattleLogic source)
        {
            CurrentHealth -= damage;
            OnTakeDamage?.Invoke(damage, source);
        }
        #endregion

        #region Exp
        public int GetDeathExp() => _data.GetDeathExp();

        public float GetExperiencePercentage() => _data.GetExperiencePercentage();

        public int AddExperience(int expGained) => _data.AddExperience(expGained);
        #endregion

        #region Statuses
        public void RemoveStatus(BaseStatus status)
        {
            _statuses.Remove(status);
            OnStatusRemoved?.Invoke(status);
        }

        public void ApplyStatus(BaseStatus status, int count)
        {
            if (HasStatus(status))
            {
                int _count = _statuses[status] += count;
                if (_count == 0)
                {
                    status.RemoveStatus(this);
                }
                else
                {
                    OnStatusUpdated?.Invoke(status, _count);
                }
            }
            else
            {
                _statuses.Add(status, count);
                OnStatusAdded?.Invoke(status, count);
            }
        }

        public void RemoveAllStatuses()
        {
            var keys = _statuses.Keys;
            foreach (var status in keys)
            {
                status.RemoveStatus(this);
            }
        }

        public bool HasStatus(BaseStatus status) => _statuses.ContainsKey(status);

        public int GetStatusCount(BaseStatus status) => HasStatus(status) ? _statuses[status] : 0;
        #endregion
    }
}