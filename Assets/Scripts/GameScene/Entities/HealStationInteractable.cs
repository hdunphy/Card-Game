using UnityEngine;

namespace Assets.Scripts.GameScene.Entities
{
    public class HealStationInteractable : MonoBehaviour, IPlayerInteractable
    {
        public void Interact(PlayerController player)
        {
            player.SharedController.HealMonsters();
        }
    }
}
