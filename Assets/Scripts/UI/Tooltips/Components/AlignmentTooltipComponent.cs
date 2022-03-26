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
            else if(data is MingmingData monsterData)
            {
                CardAlignment alignment = isPrimary ? monsterData.MingmingAlignment.Primary : monsterData.MingmingAlignment.Secondary;
                string header = isPrimary ? "Primary" : "Secondary";
                gameObject.AddComponent<TooltipTrigger>().SetText(alignment.ToString(), $"{header} Alignment");
            }
        }
    }
}
