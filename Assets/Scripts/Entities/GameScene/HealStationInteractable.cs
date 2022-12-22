using Assets.Scripts.Entities.Player;
using UnityEngine;

namespace Assets.Scripts.GameScene.Entities
{
    public class HealStationInteractable : MonoBehaviour, IPlayerInteractable
    {
        public void Interact(PlayerController player)
        {
            player.DevController.HealParty();
        }
    }
}
