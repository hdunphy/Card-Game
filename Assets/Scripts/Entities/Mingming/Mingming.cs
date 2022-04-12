using Assets.Scripts.Entities.Scriptable;
using Assets.Scripts.UI.Tooltips;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Entities
{
    public class MingmingBattleSimulation
    {
        private MingmingInstance data { get; set; }

        private Dictionary<BaseStatus, StatusIcon> Statuses { get; set; }
        private string name { get; set; }
        public int EnergyAvailable { get; private set; }
        public float AttackModifier { get; set; }
        public float DefenseModifier { get; set; }
        public int TotalEnergy => data.Energy;
        public bool IsInPlay => CurrentHealth > 0;
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

        public MingmingBattleSimulation(MingmingInstance data, string name)
        {
            this.data = data;
            this.name = name;
            EnergyAvailable = TotalEnergy;
            AttackModifier = 1;
            DefenseModifier = 1;

            Statuses = new Dictionary<BaseStatus, StatusIcon>();
        }

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

        #region Exp
        public int GetDeathExp() => data.GetDeathExp();

        public float GetExperiencePercentage() => data.GetExperiencePercentage();

        public int AddExperience(int expGained) => data.AddExperience(expGained);
        #endregion

        #region Statuses
        public bool ApplyStatus(BaseStatus status, int _count, Mingming parent)
        {
            bool hasStatus = Statuses.ContainsKey(status);
            bool applied = true;

            if (hasStatus)
            {
                var count = Statuses[status].AddCount(_count);
                if (count == 0)
                {
                    RemoveStatus(status);
                    applied = false;
                }
            }
            else
            {
                //TODO: should refactor this. Maybe move it to mingming
                StatusIcon icon = parent.InstantiateStatus(status, _count);
                Statuses.Add(status, icon);
            }

            if (applied)
            {
                UserMessage.Instance.SendMessageToUser($"{status.GetTooltipHeader(Statuses[status].Count)} was applied to {name}");
            }

            return hasStatus;
        }

        public StatusIcon RemoveStatus(BaseStatus status)
        {
            if (IsInPlay)
            {
                UserMessage.Instance.SendMessageToUser($"{name} lost the {status.name} status");
            }
            Statuses.Remove(status);
            //status.RemoveStatus(this);

            return Statuses[status];
        }

        public bool HasStatus(BaseStatus status) => Statuses.ContainsKey(status);

        public int GetStatusCount(BaseStatus status) => HasStatus(status) ? Statuses[status].Count : 0;


        public List<BaseStatus> GetStatusList() => Statuses.Keys.ToList();
        #endregion
    }
    public class Mingming : MonoBehaviour, ISelectable, IDropHandler
    {
        [SerializeField] private MingmingUIController UIController;
        [SerializeField] private EnergyHolder EnergyHolder;
        [SerializeField] private Transform StatusParent;
        [SerializeField] private StatusIcon StatusIconPrefab;
        [SerializeField] private GameObject DescriptionToolTipTrigger;

        private TooltipTrigger TooltipTrigger;

        private bool _isSelected;
        public MingmingBattleSimulation Simulation { get; private set; }

        public bool IsTurn { get; private set; }
        public bool IsInPlay => Simulation.IsInPlay;

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                UIController.SetHighlighted(value);
            }
        }

        #region Set Up
        private void Start()
        {
            IsTurn = false;
        }

        public void StartTurn()
        {
            Simulation.StartTurn();
            SetEnergy();
            IsTurn = true;
        }

        public void SetData(MingmingInstance data, bool isFacingRight)
        {
            Simulation = new MingmingBattleSimulation(data, name);
            UIController.SetUp(data, isFacingRight);

            SetEnergy();
            SetUpToolTips(data);
            UpdateTooltip();
        }

        private void SetUpToolTips(MingmingInstance data)
        {
            TooltipTrigger = DescriptionToolTipTrigger.AddComponent<TooltipTrigger>();
            var tooltipComponents = GetComponentsInChildren<ITooltipComponent>();
            foreach (var tooltipComponent in tooltipComponents)
            {
                tooltipComponent.SetData(data.BaseData);
            }
        }
        #endregion

        #region Energy
        private void SetEnergy()
        {
            EnergyHolder.SetEnergy(Simulation.EnergyAvailable, Simulation.TotalEnergy);
            if (IsSelected)
                EventManager.Instance.OnUpdateSelectedMingmingTrigger(this);
        }

        public void AddEnergy(int _energy)
        {
            Simulation.AddEnergy(_energy);
            SetEnergy();
        }
        #endregion

        #region Statuses
        public void ApplyStatus(BaseStatus status, int _count)
        {
            Simulation.ApplyStatus(status, _count, this);
        }

        public void RemoveStatus(BaseStatus status)
        {
            var icon = Simulation.RemoveStatus(status);
            status.RemoveStatus(this);

            Destroy(icon.gameObject);
        }

        public void GetStatusEffect(BaseStatus status) => status.DoEffect(this, Simulation.GetStatusCount(status));
        #endregion

        #region Getters and Setters
        public void SetIsTurn(bool _isTurn) { IsTurn = _isTurn; }
        #endregion

        #region Game Logic
        public void TakeDamage(int damage, Mingming source)
        {
            if (damage != 0)
            {
                if (source != null)
                {
                    string effect = damage < 0 ? $"was healed {damage}" : $"took {damage} damage ";
                    UserMessage.Instance.SendMessageToUser($"{name} {effect} from {source.name}");
                }

                Simulation.TakeDamage(damage);
                StartCoroutine(TakeDamageCoroutine(damage));
            }
        }

        private IEnumerator TakeDamageCoroutine(int damage)
        {
            int currentHealth = Simulation.CurrentHealth;
            int totalHealth = Simulation.TotalHealth;

            float startPercent = (float)currentHealth / totalHealth;
            float finalPercent = (float)(currentHealth - damage) / totalHealth;

            float currentPercent = startPercent;

            while (Mathf.Abs(currentPercent - finalPercent) > 0.0001f)
            {
                currentPercent = Mathf.Lerp(currentPercent, finalPercent, 0.01f);
                currentHealth = Mathf.FloorToInt((float)currentPercent * totalHealth);
                UIController.SetHealthBar(currentHealth, totalHealth);

                if (currentPercent <= 0)
                {
                    SetDead();
                    break;
                }
                yield return null;
            }

            UIController.SetHealthBar(currentHealth, totalHealth);
        }

        private void SetDead()
        {
            UserMessage.Instance.SendMessageToUser($"{name} has fainted");
            var _statuses = Simulation.GetStatusList();
            //remove events
            foreach (var _status in _statuses)
            {
                RemoveStatus(_status);
            }

            TooltipSystem.Hide();
            EventManager.Instance.OnMingmingDiedTrigger(this);
            gameObject.SetActive(false);
        }

        public void PlayCard(Card selectedCard)
        {
            EventManager.Instance.OnDiscardCardTrigger(selectedCard);
            Simulation.PlayCard(selectedCard);
            SetEnergy();
        }

        public void AddExperience(int expGained)
        {
            int levelsGained = Simulation.AddExperience(expGained);

            string levelUpText = levelsGained < 1 ? "" : $" and gained {levelsGained} levels";
            UserMessage.Instance.SendMessageToUser($"{name} gained {expGained} xp{levelUpText}");

            UIController.AddExperience(levelsGained, Simulation.GetExperiencePercentage());

            UpdateTooltip();
        }
        #endregion

        public void OnDrop(PointerEventData eventData)
        {
            if (eventData.pointerPress.TryGetComponent(out Card card))
            {
                EventManager.Instance.OnSelectTargetTrigger(this, card);
            }
        }

        public StatusIcon InstantiateStatus(BaseStatus status, int _count)
        {
            var icon = Instantiate(StatusIconPrefab, StatusParent);
            icon.SetStatus(status, _count);

            return icon;
        }

        private void UpdateTooltip()
        {
            TooltipTrigger.SetText(Simulation.GetTooltipInfo(), "Stats");
        }
    }
}