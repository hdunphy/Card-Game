using UnityEngine;

namespace Assets.Scripts.UI.Tooltips
{
    public class TargetTooltipComponent : MonoBehaviour, ITooltipComponent
    {
        public void SetData(ScriptableObject data)
        {
            CardData cardData = data as CardData;
            gameObject.AddComponent<TooltipTrigger>().SetText(cardData.TargetType.TooltipText, cardData.TargetType.name);
        }
    }
}
