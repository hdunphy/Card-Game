using Assets.Scripts;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Monster : SelectableElement, IPointerDownHandler
{
    [SerializeField] private Image MonsterSprite;
    [SerializeField] private RectTransform HealthTransform;
    [SerializeField] private RectTransform ExperienceTransform;
    [SerializeField] private TMP_Text HealthText;
    [SerializeField] private EnergyHolder EnergyHolder;
    [SerializeField] private Image DisableCover;

    private TooltipTrigger TooltipTrigger;
    private MonsterInstance Data;
    private int CurrentHealth;
    public int EnergyAvailable { get; private set; }

    public int Level => Data.Level;
    public int Attack => Data.Attack;
    public int Defense => Data.Defense;
    private int TotalEnergy => Data.Energy;


    public PlayerTeam Team { get; private set; }

    private void Start()
    {
        EventManager.Instance.UpdateSelectedCard += Instance_UpdateSelectedCard;
    }

    private void OnDestroy()
    {
        EventManager.Instance.UpdateSelectedCard -= Instance_UpdateSelectedCard;
    }

    private void Instance_UpdateSelectedCard(Card _card)
    {
        DisableCover.gameObject.SetActive(false);
        if (_card != null)
        {
            DisableCover.gameObject.SetActive(EnergyAvailable < _card.EnergyCost);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!DisableCover.gameObject.activeSelf)
            EventManager.Instance.OnSelectMonsterTrigger(this);
    }

    public void SetUp(MonsterInstance _data, PlayerTeam _playerTeam)
    {
        Data = _data;
        Team = _playerTeam;
        MonsterSprite.sprite = Data.Sprite;
        CurrentHealth = Data.Health;
        EnergyAvailable = Data.Energy;

        HealthTransform.localScale = new Vector3((float)CurrentHealth / Data.Health, 1, 1);
        ExperienceTransform.localScale = new Vector3(Data.GetExperiencePercentage(), 1, 1);

        TooltipTrigger = gameObject.AddComponent<TooltipTrigger>();
        TooltipTrigger.SetText($"Level: {Data.Level}\nAttack: {Data.Attack}\nDefense: {Data.Defense}\nExp: {Data.Experiance}", "Stats");

        SetEnergy();
        UpdateHealthText();
        UpdateTooltip();
    }

    //private void CalculateExperience()
    //{
    //    ExperienceTransform.localScale = new Vector3(Data.GetExperiencePercentage(), 1, 1);
    //}

    public void AttackMonster(Monster target, Card selectedCard)
    {
        EventManager.Instance.OnDiscardCardTrigger(selectedCard);
        float damage = Rules.Instance.GetAttackDamage(this, target, selectedCard);

        if (target.TakeDamage(Mathf.FloorToInt(damage)))
        {
            int expGained = target.GetDeathExp();
            Debug.Log($"Exp gained: {expGained}");
            target.SetDead();
            UpdateExperienceUI(expGained);
        }

        EnergyAvailable -= selectedCard.EnergyCost;
        SetEnergy();
    }

    private int GetDeathExp()
    {
        return 100;
    }

    private void SetDead()
    {
        TooltipSystem.Hide();
        gameObject.SetActive(false);
    }

    private bool TakeDamage(int damage)
    {
        Debug.Log($"{GetInstanceID()} takes {damage} damage");
        float startPercent = (float)CurrentHealth / Data.Health;
        float finalPercent = (float)(CurrentHealth -= damage) / Data.Health;
        StartCoroutine(UpdateHealthUI(startPercent, finalPercent));

        return CurrentHealth <= 0;
    }

    private IEnumerator UpdateHealthUI(float startPercent, float finalPercent)
    {
        float currentPercent = startPercent;

        while (Mathf.Abs(currentPercent - finalPercent) > 0.0001f)
        {
            currentPercent = Mathf.Lerp(currentPercent, finalPercent, 0.01f);
            HealthTransform.localScale = new Vector3(currentPercent, 1, 1);
            //CurrentHealth = Mathf.CeilToInt(currentPercent * Data.Health);
            UpdateHealthText();

            if (currentPercent <= 0.0001f)
            {
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
        //LeanTween.scaleX(ExperienceTransform, Data.GetExperiencePercentage(), 0.5f);
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
    }

    private void UpdateTooltip()
    {
        TooltipTrigger.SetText($"Level: {Data.Level}\nAttack: {Data.Attack}\nDefense: {Data.Defense}\nExp: {Data.Experiance}", "Stats");
    }

    public MonsterAlignment GetMonsterAlignment()
    {
        return Data.MonsterAlignment;
    }
}