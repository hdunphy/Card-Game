using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardReward : MonoBehaviour
{
    [SerializeField] private CardUIController UIController;

    public void SetCardData(CardData cardData)
    {
        UIController.SetCardData(cardData);
    }
}
