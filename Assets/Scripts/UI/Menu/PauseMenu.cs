using UnityEngine;

namespace Assets.Scripts.UI.Menu
{
    public class PauseMenu: MonoBehaviour
    {
        [SerializeField] private GameObject menu;

        private void Start()
        {
            GameSceneController.Singleton.OnPaused += OnPaused;
        }

        private void OnDestroy()
        {
            GameSceneController.Singleton.OnPaused -= OnPaused;
        }

        private void OnPaused(bool isPaused)
        {
            menu.SetActive(isPaused);
        }

        public void OnQuit() => GameSceneController.Singleton.LoadMainMenu();

        public void OnResume() => GameSceneController.Singleton.PauseGame(false);
    }
}
