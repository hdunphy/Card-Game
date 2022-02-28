using UnityEngine;
using UnityEngine.EventSystems;

public class CardReward : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private CardUIController UIController;
    [SerializeField] private GameObject Highlight;

    private RewardsController RewardsController;

    private bool _isSelected;
    public bool IsSelected
    {
        get => _isSelected; 
        set
        {
            _isSelected = value;
            Highlight.SetActive(_isSelected);
        }
    }
    public CardData CardData { get; private set; }

    private void Start()
    {
        Highlight.SetActive(false);
        IsSelected = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        RewardsController.SelectCard(this);
    }

    public void SetCardData(CardData cardData, RewardsController rewardsController)
    {
        CardData = cardData;
        RewardsController = rewardsController;
        UIController.SetCardData(cardData);
    }
}
