using Assets.Scripts.Entities;
using Assets.Scripts.References;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Monster : SelectableElement, IPointerDownHandler
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

    public SpriteRenderer _renderer;

    private TooltipTrigger TooltipTrigger;
    private MonsterInstance Data;
    private bool isTurn;
    private int CurrentHealth;

    private int TotalEnergy => Data.Energy;
    public bool IsActive => CurrentHealth > 0;
    public int EnergyAvailable { get; private set; }
    public int Level => Data.Level;
    public int Attack => Data.Attack;
    public int Defense => Data.Defense;

    private void Start()
    {
        EventManager.Instance.UpdateSelectedCard += Instance_UpdateSelectedCard;
        //EventManager.Instance.NewTurn += Instance_StartTurn;
        isTurn = false;
    }

    private void OnDestroy()
    {
        EventManager.Instance.UpdateSelectedCard -= Instance_UpdateSelectedCard;
        //EventManager.Instance.NewTurn -= Instance_StartTurn;
    }

    private void Instance_UpdateSelectedCard(Card _card)
    {
        if (!isTurn) return; //Don't run if it is not this monsters turn


        DisableCover.gameObject.SetActive(false);
        if (_card != null)
        {
            DisableCover.gameObject.SetActive(EnergyAvailable < _card.EnergyCost);
        }
    }

    public void StartTurn()
    {
        EnergyAvailable = TotalEnergy;
        SetEnergy();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!DisableCover.gameObject.activeSelf)
            EventManager.Instance.OnSelectMonsterTrigger(this);
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

        TooltipTrigger = gameObject.AddComponent<TooltipTrigger>();

        SetEnergy();
        UpdateHealthText();
        UpdateTooltip();
    }

    public void AttackMonster(Monster target, Card selectedCard)
    {
        EventManager.Instance.OnDiscardCardTrigger(selectedCard);
        float damage = Rules.Instance.GetAttackDamage(this, target, selectedCard);

        target.TakeDamage(Mathf.FloorToInt(damage), this);

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

    public void SetIsTurn(bool _isTurn) { isTurn = _isTurn; }

    private void TakeDamage(int damage, Monster source)
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
                source.UpdateExperienceUI(GetDeathExp());
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
}