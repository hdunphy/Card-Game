using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.GameScene.Entities
{
    public class HealStationInteractable : MonoBehaviour, IPlayerInteractable
    {
        [SerializeField] private GameObject CanInteractDisplay;
        [SerializeField] private UnityEvent OnHeal;

        private void Start()
        {
            CanInteractDisplay.SetActive(false);
        }

        public void Interact(PlayerController player)
        {
            player.HealMonsters();
            OnHeal?.Invoke();
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
