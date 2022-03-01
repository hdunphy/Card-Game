using Assets.Scripts.UI.Controller;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectableCard : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private CardUIController UIController;
    [SerializeField] private GameObject Highlight;

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
    public event Action<SelectableCard> OnSelected;

    private void Start()
    {
        Highlight.SetActive(false);
        IsSelected = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnSelected?.Invoke(this);
        //CardController.SelectCard(this);
    }

    public void SetCardData(CardData cardData)
    {
        CardData = cardData;
        UIController.SetCardData(cardData);
    }
}
