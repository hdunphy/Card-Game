using Assets.Scripts.Entities.Scriptable;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Entities
{
    public class Mingming : MonoBehaviour, ISelectable
    {
        [SerializeField] private MingmingUIController UIController;
        [SerializeField] private EnergyHolder EnergyHolder;
        [SerializeField] private Transform StatusParent;
        [SerializeField] private StatusIcon StatusIconPrefab;

        private MingmingInstance Data;
        private bool _isSelected;

        private int TotalEnergy => Data.Energy;
        public bool IsInPlay => CurrentHealth > 0;
        public int TotalHealth => Data.Health;
        public bool IsTurn { get; private set; }
        public int CurrentHealth
        {
            get => Data.CurrentHealth;
            private set => Data.CurrentHealth = value;
        }
        public int EnergyAvailable { get; private set; }
        public float AttackModifier { get; set; }
        public float DefenseModifier { get; set; }
        public int Level => Data.Level;
        public float Attack => Data.Attack * AttackModifier;
        public float Defense => Data.Defense * DefenseModifier;

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                UIController.SetHighlighted(value);
            }
        }

        private Dictionary<BaseStatus, StatusIcon> Statuses;

        #region Set Up
        private void Start()
        {
            IsTurn = false;
            AttackModifier = 1;
            DefenseModifier = 1;
        }

        public void StartTurn()
        {
            EnergyAvailable = TotalEnergy;
            SetEnergy();
            IsTurn = true;
        }

        public void SetData(MingmingInstance data, bool isFacingRight)
        {
            Data = data;
            UIController.SetUp(data, isFacingRight);

            EnergyAvailable = Data.Energy;
            Statuses = new Dictionary<BaseStatus, StatusIcon>();

            SetEnergy();
        }
        #endregion

        #region Energy
        private void SetEnergy()
        {
            EnergyHolder.SetEnergy(EnergyAvailable, TotalEnergy);
            if (IsSelected)
                EventManager.Instance.OnUpdateSelectedMingmingTrigger(this);
        }

        public void AddEnergy(int _energy)
        {
            EnergyAvailable += _energy;
            SetEnergy();
        }
        #endregion

        #region Statuses
        public void ApplyStatus(BaseStatus status, int _count)
        {
            bool applied = true;

            if (Statuses.ContainsKey(status))
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
                var icon = Instantiate(StatusIconPrefab, StatusParent);
                icon.SetStatus(status, _count);
                Statuses.Add(status, icon);
            }

            if (applied)
            {
                UserMessage.Instance.SendMessageToUser($"{status.GetTooltipHeader(Statuses[status].Count)} was applied to {name}");
            }
        }

        public bool HasStatus(BaseStatus status) => Statuses.ContainsKey(status);

        public int GetStatusCount(BaseStatus status) => HasStatus(status) ? Statuses[status].Count : 0;

        public void GetStatusEffect(BaseStatus status) => status.DoEffect(this, Statuses[status].Count);

        public void RemoveStatus(BaseStatus status)
        {
            if (IsInPlay)
            {
                UserMessage.Instance.SendMessageToUser($"{name} lost the {status.name} status");
            }
            Destroy(Statuses[status].gameObject);
            Statuses.Remove(status);
            status.RemoveStatus(this);
        }
        #endregion

        #region Getters and Setters
        public MingmingAlignment GetMonsterAlignment() => Data.MonsterAlignment;

        public int GetDeathExp() => Data.GetDeathExp();

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

                StartCoroutine(TakeDamageCoroutine(damage));
            }
        }

        private IEnumerator TakeDamageCoroutine(int damage)
        {
            float startPercent = (float)CurrentHealth / Data.Health;
            float finalPercent = (float)(CurrentHealth -= damage) / Data.Health;

            float currentPercent = startPercent;

            while (Mathf.Abs(currentPercent - finalPercent) > 0.0001f)
            {
                currentPercent = Mathf.Lerp(currentPercent, finalPercent, 0.01f);
                CurrentHealth = Mathf.FloorToInt((float)currentPercent * Data.Health);
                UIController.SetHealthBar(currentPercent);

                if (CurrentHealth <= 0)
                {
                    SetDead();
                    break;
                }
                yield return null;
            }

            UIController.SetHealthBar(currentPercent);
        }

        private void SetDead()
        {
            UserMessage.Instance.SendMessageToUser($"{name} has fainted");
            var _statuses = Statuses.Keys.ToList();
            //remove events
            foreach (var _status in _statuses)
            {
                RemoveStatus(_status);
            }

            TooltipSystem.Hide();
            EventManager.Instance.OnMonsterDiedTrigger(this);
            gameObject.SetActive(false);
        }

        public void PlayCard(Card selectedCard)
        {
            EventManager.Instance.OnDiscardCardTrigger(selectedCard);
            EnergyAvailable -= selectedCard.EnergyCost;
            SetEnergy();
        }

        public void AddExperience(int expGained)
        {
            int levelsGained = Data.AddExperience(expGained);

            string levelUpText = levelsGained < 1 ? "" : $" and gained {levelsGained} levels";
            UserMessage.Instance.SendMessageToUser($"{name} gained {expGained} xp{levelUpText}");

            UIController.AddExperience(levelsGained);
        }
        #endregion
    }
}