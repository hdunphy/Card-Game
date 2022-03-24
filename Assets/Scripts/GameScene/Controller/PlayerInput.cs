using UnityEngine;

namespace Assets.Scripts.GameScene.Controller
{
    public class PlayerInput : MonoBehaviour
    {
        [SerializeField] private PlayerController Player;

        Vector2 movementVector;

        private void Awake()
        {
            GameSceneController.Singleton.OnPaused += OnPaused;
        }

        private void OnDestroy()
        {
            GameSceneController.Singleton.OnPaused -= OnPaused;
        }

        // Update is called once per frame
        void Update()
        {
            movementVector.x = Input.GetAxisRaw("Horizontal");
            movementVector.y = Input.GetAxisRaw("Vertical");
            Player.SetMoveDirection(movementVector);

            if (Input.GetKeyDown(KeyCode.E))
            {
                Player.Interact();
            }

            if (Input.GetKeyDown(KeyCode.Escape))
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
