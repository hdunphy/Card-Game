using UnityEngine;
using UnityEngine.Events;

public class InteractionTrigger : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject CanInteractDisplay;
    [SerializeField] private UnityEvent OnInteractionEvent;

    private void Start()
    {
        CanInteractDisplay.SetActive(false);
    }

    public void GetInteraction()
    {
        OnInteractionEvent?.Invoke();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out PlayerController player))
        {
            player.SetInteraction(this);
            CanInteractDisplay.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerController player))
        {
            player.SetInteraction(null);
            CanInteractDisplay.SetActive(false);
        }
    }
}

public interface IInteractable
{
    public void GetInteraction();
}