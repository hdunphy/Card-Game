using Assets.Scripts.Entities;
using Assets.Scripts.Entities.Scriptable;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Monster : SelectableElement, IPointerDownHandler, IDropHandler
{
    [SerializeField] private Image MonsterSprite;
    [SerializeField] private Image DisableCover;
    [SerializeField] private Image HealthBar;
    [SerializeField] private Image PrimaryAlignment;
    [SerializeField] private Image SecondaryAlignment;
    [SerializeField] private RectTransform ExperienceTransform;
    [SerializeField] private TMP_Text HealthText;
    [SerializeField] private TMP_Text MonsterName;
    [SerializeField] private EnergyHolder EnergyHolder;
    [SerializeField] private Gradient HealthGradient;
    [SerializeField] private Transform StatusParent;
    [SerializeField] private StatusIcon StatusIconPrefab;

    private TooltipTrigger TooltipTrigger;
    private MonsterInstance Data;

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
    private Dictionary<BaseStatus, StatusIcon> Statuses;

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
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!DisableCover.gameObject.activeSelf)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                EventManager.Instance.OnSelectMonsterTrigger(this);
            }
            else if (eventData.button == PointerEventData.InputButton.Right)
            {
                EventManager.Instance.OnResetSelectedTrigger();
            }
        }
    }

    public void SetUp(MonsterInstance _data)
    {
        Data = _data;
        CurrentHealth = Data.Health;
        EnergyAvailable = Data.Energy;
        MonsterName.text = Data.Name;


        MonsterSprite.sprite = Data.Sprite;
        //MonsterSprite.rectTransform.localScale = new Vector3(-1, 1, 1); //flipping the sprite but also flips other alignments
        PrimaryAlignment.sprite = SpriteReferenceDictionary.Instance.GetSpriteFromEnum(Data.MonsterAlignment.Primary);
        if (Data.MonsterAlignment.Secondary != CardAlignment.None)
        {
            SecondaryAlignment.enabled = true;
            SecondaryAlignment.sprite = SpriteReferenceDictionary.Instance.GetSpriteFromEnum(Data.MonsterAlignment.Secondary);
        }
        else
            SecondaryAlignment.enabled = false;

        HealthBar.rectTransform.localScale = new Vector3((float)CurrentHealth / Data.Health, 1, 1);
        ExperienceTransform.localScale = new Vector3(Data.GetExperiencePercentage(), 1, 1);

        Statuses = new Dictionary<BaseStatus, StatusIcon>();

        TooltipTrigger = gameObject.AddComponent<TooltipTrigger>();

        SetEnergy();
        UpdateHealthText();
        UpdateTooltip();
    }

    public void PlayCard(Card selectedCard)
    {
        EventManager.Instance.OnDiscardCardTrigger(selectedCard);
        EnergyAvailable -= selectedCard.EnergyCost;
        SetEnergy();
    }

    private int GetDeathExp() => Data.GetDeathExp();

    public void ApplyStatus(BaseStatus status, int _count)
    {
        if (Statuses.ContainsKey(status))
        {
            var count = Statuses[status].AddCount(_count);
            if (count == 0)
            {
                RemoveStatus(status);
            }
        }
        else
        {
            var icon = Instantiate(StatusIconPrefab, StatusParent);
            icon.SetStatus(status, _count);
            Statuses.Add(status, icon);
        }
    }

    public bool HasStatus(BaseStatus status) => Statuses.ContainsKey(status);

    public void GetStatusEffect(BaseStatus status) => status.DoEffect(this, Statuses[status].Count);

    public void RemoveStatus(BaseStatus status)
    {
        Destroy(Statuses[status].gameObject);
        Statuses.Remove(status);
        status.RemoveStatus(this);
    }

    private void SetDead()
    {
        var _statuses = Statuses.Keys.ToList();
        //remove events
        foreach (var _status in _statuses)
        {
            RemoveStatus(_status);
        }

        EventManager.Instance.OnMonsterDiedTrigger(this);
        TooltipSystem.Hide();
        gameObject.SetActive(false);
    }

    public void SetIsTurn(bool _isTurn) { IsTurn = _isTurn; }

    public void TakeDamage(int damage, Monster source)
    {
        float startPercent = (float)CurrentHealth / Data.Health;
        float finalPercent = (float)(CurrentHealth -= damage) / Data.Health;
        StartCoroutine(UpdateHealthUI(startPercent, finalPercent, source));
    }

    private IEnumerator UpdateHealthUI(float startPercent, float finalPercent, Monster source)
    {
        float currentPercent = startPercent;

        while (Mathf.Abs(currentPercent - finalPercent) > 0.0001f)
        {
            currentPercent = Mathf.Lerp(currentPercent, finalPercent, 0.01f);
            HealthBar.rectTransform.localScale = new Vector3(currentPercent, 1, 1);
            CurrentHealth = Mathf.FloorToInt((float)currentPercent * Data.Health);

            UpdateHealthText();

            if (CurrentHealth <= 0)
            {
                source?.UpdateExperienceUI(GetDeathExp()); //TODO null propagation for Status deaths
                SetDead();
                break;
            }
            yield return null;
        }
        UpdateHealthText();
    }

    private void UpdateExperienceUI(int expGained)
    {
        int levelUps = Data.AddExperience(expGained);
        Debug.Log($"LevelUps: {levelUps}");
        if (levelUps > 0)
        {
            LeanTween.scale(ExperienceTransform, new Vector3(1, 1), .75f)
                    .setOnComplete(() => { ExperienceTransform.localScale = new Vector3(0, 1, 1); }).setLoopCount(levelUps)
                    .setOnComplete(() =>
                    {
                        ExperienceTransform.localScale = new Vector3(0, 1, 1);
                        LeanTween.scale(ExperienceTransform, new Vector3(Data.GetExperiencePercentage(), 1), .75f);
                    });
        }
        else
        {
            LeanTween.scale(ExperienceTransform, new Vector3(Data.GetExperiencePercentage(), 1), 0.75f);
        }


        UpdateTooltip();
    }

    private void SetEnergy()
    {
        EnergyHolder.SetEnergy(EnergyAvailable, TotalEnergy);
        if (IsSelected)
            EventManager.Instance.OnUpdateSelectedMonsterTrigger(this);
    }

    private void UpdateHealthText()
    {
        HealthText.text = $"{CurrentHealth}/{Data.Health}";
        HealthBar.color = HealthGradient.Evaluate((float)CurrentHealth / Data.Health);
    }

    private void UpdateTooltip()
    {
        TooltipTrigger.SetText($"Level: {Data.Level}\nAttack: {Data.Attack}\nDefense: {Data.Defense}\nExp: {Data.Experiance}", "Stats");
    }

    public MonsterAlignment GetMonsterAlignment()
    {
        return Data.MonsterAlignment;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerPress.TryGetComponent(out Card card))
        {
            EventManager.Instance.OnSelectTargetTrigger(this, card);
        }
    }
}