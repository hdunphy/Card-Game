using UnityEngine;

namespace Assets.Scripts.GameScene.Entities
{

    public class PlayerEnterInteractable : MonoBehaviour
    {
        private ITriggerable Triggerable;

        private void Start()
        {
            Triggerable = GetComponent<ITriggerable>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out PlayerController player))
            {
                Triggerable.SetIsOn(true, player);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out PlayerController _))
            {
                Triggerable.SetIsOn(false, null);
            }
        }
    }

    public interface ITriggerable
    {
        void SetIsOn(bool _isOn, PlayerController _player);
    }

}