using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.GameScene.Entities
{
    public class InteractionTrigger : MonoBehaviour, IPlayerInteractable
    {
        [SerializeField] private GameObject CanInteractDisplay;
        [SerializeField] private UnityEvent<PlayerController> OnInteractionEvent;

        private void Start()
        {
            CanInteractDisplay.SetActive(false);
        }

        public void Interact(PlayerController player)
        {
            OnInteractionEvent?.Invoke(player);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out PlayerController player))
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
}