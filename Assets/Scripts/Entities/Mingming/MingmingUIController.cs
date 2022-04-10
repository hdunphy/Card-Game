using Assets.Scripts.UI.Tooltips;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Entities
{
    public class MingmingUIController : Highlightable
    {
        [SerializeField] private Image MingmingSprite;
        [SerializeField] private Image DisableCover;
        [SerializeField] private Image HealthBar;
        [SerializeField] private Image PrimaryAlignment;
        [SerializeField] private Image SecondaryAlignment;
        [SerializeField] private RectTransform ExperienceTransform;
        [SerializeField] private TMP_Text HealthText;
        [SerializeField] private TMP_Text NameText;
        [SerializeField] private Gradient HealthGradient;
        [SerializeField] private GameObject DescriptionToolTipTrigger;

        private TooltipTrigger TooltipTrigger;
        private MingmingInstance Data;
        public int Direction { get; private set; } //1 or -1
        private int CurrentHealth => Data.CurrentHealth;

        public void SetUp(MingmingInstance _data, bool isFacingRight)
        {
            Data = _data;
            NameText.text = Data.Name;
            name = Data.Name;
            Direction = isFacingRight ? 1 : -1;

            MingmingSprite.sprite = Data.Sprite;

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

            SetUpToolTips();
            UpdateHealthText();
            UpdateTooltip();
        }

        private void SetUpToolTips()
        {
            TooltipTrigger = DescriptionToolTipTrigger.AddComponent<TooltipTrigger>();
            var tooltipComponents = GetComponentsInChildren<ITooltipComponent>();
            foreach (var tooltipComponent in tooltipComponents)
            {
                tooltipComponent.SetData(Data.BaseData);
            }
        }

        public void SetHealthBar(float currentPercent)
        {
            HealthBar.rectTransform.localScale = new Vector3(currentPercent, 1, 1);
            UpdateHealthText();
        }

        public void AddExperience(int levelsGained)
        {
            if (levelsGained > 0)
            {
                LeanTween.scale(ExperienceTransform, new Vector3(1, 1), .75f)
                        .setOnComplete(() => { ExperienceTransform.localScale = new Vector3(0, 1, 1); }).setLoopCount(levelsGained)
                        .setOnComplete(() =>
                        {
                            ExperienceTransform.localScale = new Vector3(0, 1, 1);
                            SetExpBar();
                        });
            }
            else
            {
                SetExpBar();
            }


            UpdateTooltip();
        }

        private void SetExpBar()
        {
            LeanTween.scale(ExperienceTransform, new Vector3(Data.GetExperiencePercentage(), 1), .75f);
        }

        private void UpdateHealthText()
        {
            HealthText.text = $"{CurrentHealth}/{Data.Health}";
            HealthBar.color = HealthGradient.Evaluate((float)CurrentHealth / Data.Health);
        }

        private void UpdateTooltip()
        {
            TooltipTrigger.SetText($"Level: {Data.Level}\nAttack: {Data.Attack}\nDefense: {Data.Defense}\nExp: {Data.Experience}", "Stats");
        }
    }
}