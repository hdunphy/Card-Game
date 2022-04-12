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
        //private MingmingInstance Data;
        public int Direction { get; private set; } //1 or -1
        //private int CurrentHealth => Data.CurrentHealth;

        public void SetUp(MingmingInstance data, bool isFacingRight)
        {
            //Data = _data;
            NameText.text = data.Name;
            name = data.Name;
            Direction = isFacingRight ? 1 : -1;

            MingmingSprite.sprite = data.Sprite;

            PrimaryAlignment.sprite = SpriteReferenceDictionary.Instance.GetSpriteFromEnum(data.MingmingAlignment.Primary);
            if (data.MingmingAlignment.Secondary != CardAlignment.None)
            {
                SecondaryAlignment.enabled = true;
                SecondaryAlignment.sprite = SpriteReferenceDictionary.Instance.GetSpriteFromEnum(data.MingmingAlignment.Secondary);
            }
            else
                SecondaryAlignment.enabled = false;

            HealthBar.rectTransform.localScale = new Vector3((float)data.CurrentHealth / data.Health, 1, 1);
            ExperienceTransform.localScale = new Vector3(data.GetExperiencePercentage(), 1, 1);

            UpdateHealthText(data.CurrentHealth, data.Health);
        }

        public void SetHealthBar(int currentHealth, int totalHealth)
        {
            float currentPercent = (float)currentHealth / totalHealth;
            HealthBar.rectTransform.localScale = new Vector3(currentPercent, 1, 1);
            UpdateHealthText(currentHealth, totalHealth);
        }

        public void AddExperience(int levelsGained, float xpPercentage)
        {
            if (levelsGained > 0)
            {
                LeanTween.scale(ExperienceTransform, new Vector3(1, 1), .75f)
                        .setOnComplete(() => { ExperienceTransform.localScale = new Vector3(0, 1, 1); }).setLoopCount(levelsGained)
                        .setOnComplete(() =>
                        {
                            ExperienceTransform.localScale = new Vector3(0, 1, 1);
                            SetExpBar(xpPercentage);
                        });
            }
            else
            {
                SetExpBar(xpPercentage);
            }
        }

        private void SetExpBar(float xpPercentage)
        {
            LeanTween.scale(ExperienceTransform, new Vector3(xpPercentage, 1), .75f);
        }

        private void UpdateHealthText(int currentHealth, int totalHealth)
        {
            HealthText.text = $"{currentHealth}/{totalHealth}";
            HealthBar.color = HealthGradient.Evaluate((float)currentHealth / totalHealth);
        }
    }
}