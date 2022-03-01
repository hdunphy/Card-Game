using UnityEngine;

namespace Assets.Scripts.UI.Controller
{
    public class CurrentCardRow : MonoBehaviour, ICardUI
    {
        [SerializeField] private TMPro.TMP_Text NameText;

        public void SetCardData(CardData cardData)
        {
            NameText.text = cardData.CardName;
        }
    }
}
