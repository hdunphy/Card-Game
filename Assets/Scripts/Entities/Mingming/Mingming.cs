using Assets.Scripts.Entities.Scriptable;
using Assets.Scripts.UI.Tooltips;
using System.Collections;
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
        public MingmingBattleSimulation Simulation { get; private set; }

        #region Getters and Setters
        public bool IsTurn { get; private set; }

        public bool IsInPlay => Simulation.CurrentHealth > 0;

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
            TooltipTrigger.SetText(Simulation.GetTooltipInfo(), "Stats");
        }
        #endregion

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
            Simulation = new MingmingBattleSimulation(data);
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
            bool applied = true;

            if (Simulation.HasStatus(status))
            {
                var count = Simulation.AddCount(status, _count);
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
                Simulation.AddStatus(status, icon);
            }

            if (applied)
            {
                UserMessage.Instance.SendMessageToUser($"{status.GetTooltipHeader(Simulation.GetStatusCount(status))} was applied to {name}");
            }
        }

        public void RemoveStatus(BaseStatus status)
        {
            if (IsInPlay)
            {
                UserMessage.Instance.SendMessageToUser($"{name} lost the {status.name} status");
            }
            var icon = Simulation.RemoveStatus(status);
            status.RemoveStatus(this);

            Destroy(icon.gameObject);
        }

        public void GetStatusEffect(BaseStatus status) => status.DoEffect(this, Simulation.GetStatusCount(status));
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
                Simulation.TakeDamage(damage);
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