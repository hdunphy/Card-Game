using Assets.Scripts.UI.Tooltips;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Controller
{
    public class CardUIController : MonoBehaviour, ICardUI
    {
        [Header("UI")]
        [SerializeField] private TMP_Text CardName;
        [SerializeField] private TMP_Text CardDescription;
        [SerializeField] private TMP_Text EnergyCostText;
        [SerializeField] private Image CardSprite;
        [SerializeField] private Image TargetTypeIcon;
        [SerializeField] private Image CardAlignmentIcon;

        private CardData Data;

        public void SetCardData(CardData _data)
        {
            Data = _data;

            CardName.text = Data.CardName;
            CardDescription.text = Data.CardDescription;
            EnergyCostText.text = Data.EnergyCost.ToString();
            CardSprite.sprite = Data.CardSprite;
            TargetTypeIcon.sprite = Data.TargetType.Sprite;
            CardAlignmentIcon.sprite = SpriteReferenceDictionary.Instance.GetSpriteFromEnum(Data.CardAlignment);

            var tooltipComponents = GetComponentsInChildren<ITooltipComponent>();
            foreach (var tooltipComponent in tooltipComponents)
            {
                tooltipComponent.SetData(Data);
            }
        }
    }
}