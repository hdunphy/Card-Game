using System.Collections;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

namespace Assets.Scripts.GameScene.Controller
{
    public class PlayerInputController : MonoBehaviour
    {
        [SerializeField] private PlayerController Player;

        private void Awake()
        {
            GameSceneController.Singleton.OnPaused += OnPaused;
        }

        private void OnDestroy()
        {
            GameSceneController.Singleton.OnPaused -= OnPaused;
        }

        public void OnMove(CallbackContext callback)
        {
            Player.SetMoveDirection(callback.ReadValue<Vector2>());
        }

        public void OnInteraction(CallbackContext callback)
        {
            if (callback.started)
            {
                Player.Interact();
            }
        }

        public void OnPause(CallbackContext callback)
        {
            if (callback.started)
            {
                GameSceneController.Singleton.PauseGame(true);
            }
        }

        private void OnPaused(bool isPaused)
        {
            enabled = !isPaused;
        }
    }
}