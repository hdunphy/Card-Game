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
    [SerializeField] private TMP_Text HealthText;
    [SerializeField] private EnergyHolder EnergyHolder;

    private MonsterInstance Data;
    private int CurrentHealth;
    private int EnergyAvailable;

    public int Level => Data.Level;
    public int Attack => Data.Attack;
    public int Defense => Data.Defense;
    private int TotalEnergy => Data.Energy;
    

    public PlayerTeam Team { get; private set; }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log($"selecting: {this.GetInstanceID()}");
        EventManager.Instance.OnSelectMonsterTrigger(this);
    }

    public void SetUp(MonsterInstance _data, PlayerTeam _playerTeam)
    {
        Data = _data;
        Team = _playerTeam;
        MonsterSprite.sprite = Data.Sprite;
        CurrentHealth = Data.Health;
        EnergyAvailable = Data.Energy;

        SetEnergy();
        UpdateHealthText();
        gameObject.AddComponent<TooltipTrigger>().SetText($"Level: {Data.Level}\nAttack: {Data.Attack}\nDefense: {Data.Defense}\nExp: {Data.Experiance}", "Stats");
    }


    public void AttackMonster(Monster target, Card selectedCard)
    {
        EventManager.Instance.OnDiscardCardTrigger(selectedCard);
        float damage = Rules.Instance.GetAttackDamage(this, target, selectedCard);
        target.TakeDamage(Mathf.FloorToInt(damage));
        EnergyAvailable -= selectedCard.EnergyCost;
        SetEnergy();
    }

    private void TakeDamage(int damage)
    {
        Debug.Log($"{GetInstanceID()} takes {damage} damage");
        float startPercent = (float) CurrentHealth / Data.Health;
        float finalPercent = (float) (CurrentHealth -= damage) / Data.Health;
        StartCoroutine(UpdateHealthUI(startPercent, finalPercent));
    }

    private IEnumerator UpdateHealthUI(float startPercent, float finalPercent)
    {
        float currentPercent = startPercent;
        int _currentHealth = CurrentHealth;

        while(Mathf.Abs(currentPercent - finalPercent) > 0.0001f)
        {
            currentPercent = Mathf.Lerp(currentPercent, finalPercent, 0.01f);
            HealthTransform.localScale = new Vector3(currentPercent, 1, 1);
            CurrentHealth = Mathf.CeilToInt(currentPercent * Data.Health);
            UpdateHealthText();

            if(currentPercent <= 0.0001f)
            {
                gameObject.SetActive(false);
                break;
            }
            yield return null;
        }
        CurrentHealth = _currentHealth;
        UpdateHealthText();
    }

    private void SetEnergy()
    {
        EnergyHolder.SetEnergy(EnergyAvailable, TotalEnergy);
    }

    private void UpdateHealthText()
    {
        HealthText.text = $"{CurrentHealth}/{Data.Health}";
    }

    public MonsterAlignment GetMonsterAlignment()
    {
        return Data.MonsterAlignment;
    }
}