﻿using Assets.Scripts.Entities.Scriptable;
using Assets.Scripts.UI.Tooltips;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Entities
{
    public class Mingming : MonoBehaviour, ISelectable, IDropHandler
    {
        [SerializeField] private MingmingUIController UIController;
        [SerializeField] private EnergyHolder EnergyHolder;
        [SerializeField] private Transform StatusParent;
        [SerializeField] private StatusIcon StatusIconPrefab;
        [SerializeField] private GameObject DescriptionToolTipTrigger;

        private TooltipTrigger TooltipTrigger;

        private bool _isSelected;
        public MingmingBattleLogic Logic { get; private set; }
        private Dictionary<BaseStatus, StatusIcon> _statuses;

        #region Getters and Setters
        public bool IsTurn { get; private set; }

        public bool IsInPlay => Logic.CurrentHealth > 0;

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                UIController.SetHighlighted(value);
            }
        }

        public void SetIsTurn(bool _isTurn) { IsTurn = _isTurn; }

        private void UpdateTooltip()
        {
            TooltipTrigger.SetText(Logic.GetTooltipInfo(), "Stats");
        }
        #endregion

        #region Set Up
        private void Start()
        {
            IsTurn = false;
            _statuses = new Dictionary<BaseStatus, StatusIcon>();
        }

        private void OnDestroy()
        {
            RemoveEvents();
        }

        public void StartTurn()
        {
            Logic.StartTurn();
            SetEnergy();
            IsTurn = true;
        }

        public void SetData(MingmingInstance data, bool isFacingRight)
        {
            Logic = new MingmingBattleLogic(data, name);
            UIController.SetUp(data, isFacingRight);

            AddEvents();
            SetEnergy();
            SetUpToolTips(data);
            UpdateTooltip();
        }

        private void AddEvents()
        {
            Logic.OnEnergyChanged += SetEnergy;
            Logic.OnStatusAdded += StatusAdded;
            Logic.OnStatusUpdated += StatusUpdated;
            Logic.OnStatusRemoved += StatusRemoved;
        }

        private void RemoveEvents()
        {
            Logic.OnEnergyChanged -= SetEnergy;
            Logic.OnStatusAdded -= StatusAdded;
            Logic.OnStatusUpdated -= StatusUpdated;
            Logic.OnStatusRemoved -= StatusRemoved;
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
        private void SetEnergy(int _ = 0)
        {
            EnergyHolder.SetEnergy(Logic.EnergyAvailable, Logic.TotalEnergy);
            if (IsSelected)
                EventManager.Instance.OnUpdateSelectedMingmingTrigger(this);
        }
        #endregion

        #region Statuses
        private void StatusAdded(BaseStatus status, int count)
        {
            var icon = Instantiate(StatusIconPrefab, StatusParent);
            icon.SetStatus(status, count);

            _statuses.Add(status, icon);

            SendStatusMessage(status);
        }

        private void StatusUpdated(BaseStatus status, int count)
        {
            _statuses[status].SetCount(count);

            SendStatusMessage(status);
        }

        private void SendStatusMessage(BaseStatus status)
        {
            UserMessage.Instance.SendMessageToUser($"{status.GetTooltipHeader(Logic.GetStatusCount(status))} was applied to {name}");
        }

        public void StatusRemoved(BaseStatus status)
        {
            if (IsInPlay)
            {
                UserMessage.Instance.SendMessageToUser($"{name} lost the {status.name} status");
            }
            var icon = _statuses[status];
            _statuses.Remove(status);

            Destroy(icon.gameObject);
        }
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
                Logic.TakeDamage(damage, source.Logic);
            }
        }

        private IEnumerator TakeDamageCoroutine(int damage)
        {
            int currentHealth = Logic.CurrentHealth;
            int totalHealth = Logic.TotalHealth;

            float startPercent = (float)currentHealth / totalHealth;
            float finalPercent = (float)(currentHealth - damage) / totalHealth;

            float currentPercent = startPercent;

            while (Mathf.Abs(currentPercent - finalPercent) > 0.0001f)
            {
                currentPercent = Mathf.Lerp(currentPercent, finalPercent, 0.01f);
                currentHealth = Mathf.FloorToInt((float)currentPercent * totalHealth);
                UIController.SetHealthBar(currentHealth, totalHealth);

                if (currentHealth <= 0)
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
            Logic.RemoveAllStatuses();

            TooltipSystem.Hide();
            EventManager.Instance.OnMingmingDiedTrigger(this);
            gameObject.SetActive(false);
        }

        public void AddExperience(int expGained)
        {
            int levelsGained = Logic.AddExperience(expGained);

            string levelUpText = levelsGained < 1 ? "" : $" and gained {levelsGained} levels";
            UserMessage.Instance.SendMessageToUser($"{name} gained {expGained} xp{levelUpText}");

            UIController.AddExperience(levelsGained, Logic.GetExperiencePercentage());

            UpdateTooltip();
        }

        public void OnDrop(PointerEventData eventData)
        {
            if (eventData.pointerPress.TryGetComponent(out Card card))
            {
                EventManager.Instance.OnSelectTargetTrigger(this, card);
            }
        }
        #endregion
    }
}