using UnityEngine;

namespace Assets.Scripts.UI.Tooltips
{
    public class CardTooltipComponent : MonoBehaviour, ITooltipComponent
    {
        public void SetData(ScriptableObject data)
        {
            if (data is CardData cardData)
            {
                //TODO: use the actions here
                gameObject.AddComponent<TooltipTrigger>().SetText($"TODO");
            }
        }
    }
}
