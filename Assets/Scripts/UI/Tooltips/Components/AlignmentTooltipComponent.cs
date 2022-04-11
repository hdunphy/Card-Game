using UnityEngine;

namespace Assets.Scripts.UI.Tooltips
{
    public class AlignmentTooltipComponent : MonoBehaviour, ITooltipComponent
    {
        [SerializeField]
        private bool isPrimary;

        public void SetData(ScriptableObject data)
        {
            if(data is CardData cardData)
            {
                gameObject.AddComponent<TooltipTrigger>().SetText(cardData.CardAlignment.ToString());
            }
            else if(data is MingmingData mingmingData)
            {
                CardAlignment alignment = isPrimary ? mingmingData.MingmingAlignment.Primary : mingmingData.MingmingAlignment.Secondary;
                string header = isPrimary ? "Primary" : "Secondary";
                gameObject.AddComponent<TooltipTrigger>().SetText(alignment.ToString(), $"{header} Alignment");
            }
        }
    }
}
