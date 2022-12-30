using Assets.Scripts.Entities.Mingmings;
using Assets.Scripts.References;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.GameScene.MingmingStorage
{
    public class MingmingStorageController : MonoBehaviour
    {
        [SerializeField] private Image mingmingSprite;
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private TMP_Text levelText;
        [SerializeField] private Image primaryAlignment;
        [SerializeField] private Image secondaryAlignment;
        [SerializeField] private RectTransform experienceTransform;
        [SerializeField] private Transform statContainerTransform;
        [SerializeField] private StatContainerController statContainerPrefab;

        public void Setup(MingmingInstance mingming)
        {
            mingmingSprite.sprite = mingming.Sprite;
            nameText.text= mingming.Name;
            levelText.text = mingming.Level.ToString();
            experienceTransform.localScale = new Vector3(mingming.GetExperiencePercentage(), 1, 1);

            primaryAlignment.sprite = SpriteReferenceDictionary.Instance.GetSpriteFromEnum(mingming.MingmingAlignment.Primary);
            if (mingming.MingmingAlignment.Secondary != CardAlignment.None)
            {
                secondaryAlignment.enabled = true;
                secondaryAlignment.sprite = SpriteReferenceDictionary.Instance.GetSpriteFromEnum(mingming.MingmingAlignment.Secondary);
            }
            else
                secondaryAlignment.enabled = false;

            var attackStatContainer = Instantiate(statContainerPrefab, statContainerTransform);
            attackStatContainer.Setup(new("ATK", mingming.Attack, Rules.CalculateStat(mingming.BaseData.Attack, MingmingInstance.MAX_MODIFIER, mingming.Level)));
            var defenseStatContainer = Instantiate(statContainerPrefab, statContainerTransform);
            defenseStatContainer.Setup(new("DEF", mingming.Defense, Rules.CalculateStat(mingming.BaseData.Defense, MingmingInstance.MAX_MODIFIER, mingming.Level)));
            var healthStatContainer = Instantiate(statContainerPrefab, statContainerTransform);
            healthStatContainer.Setup(new("HP", mingming.Health, Rules.CalculateStat(mingming.BaseData.Health, MingmingInstance.MAX_MODIFIER, mingming.Level, true)));
            var energyStatContainer = Instantiate(statContainerPrefab, statContainerTransform);
            energyStatContainer.Setup(new("NRG", mingming.Energy, MingmingData.MAX_ENERGY));
            var cardDrawStatContainer = Instantiate(statContainerPrefab, statContainerTransform);
            cardDrawStatContainer.Setup(new("DRW", mingming.CardDraw, MingmingData.MAX_CARD_DRAW));
        }
    }
}