using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Tooltips
{
    public class AlignmentTooltipComponent : MonoBehaviour, ITooltipComponent
    {
        [SerializeField] private bool isPrimary;
        [SerializeField] private Image iconImage;

        public void SetData(ScriptableObject data)
        {
            CardAlignment alignment = CardAlignment.None;
            if (data is CardData cardData)
            {
                gameObject.AddComponent<TooltipTrigger>().SetText(cardData.CardAlignment.ToString());
                alignment = cardData.CardAlignment;
            }
            else if (data is MingmingData mingmingData)
            {
                alignment = isPrimary ? mingmingData.MingmingAlignment.Primary : mingmingData.MingmingAlignment.Secondary;
                if (!isPrimary && alignment is CardAlignment.None)
                {
                    gameObject.SetActive(false);
                }
                else
                {
                    string header = isPrimary ? "Primary" : "Secondary";
                    gameObject.AddComponent<TooltipTrigger>().SetText(alignment.ToString(), $"{header} Alignment");
                }
            }

            iconImage.sprite = SpriteReferenceDictionary.Instance.GetSpriteFromEnum(alignment);
        }
    }
}
