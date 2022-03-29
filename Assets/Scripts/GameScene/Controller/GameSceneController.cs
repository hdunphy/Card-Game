using Assets.Scripts.Controller.SaveSystem;
using Assets.Scripts.Entities.SaveSystem;
using Assets.Scripts.GameScene.Controller.SceneManagement;
using Assets.Scripts.Helpers;
using Assets.Scripts.UI.Controller;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.GameScene.Controller
{
    public class GameSceneController : SingletonMonoBehavior<GameSceneController>
    {

        [SerializeField] private PlayerController PlayerPrefab;
        [SerializeField] private CameraController CameraPrefab;
        [SerializeField] private SceneTransitionController sceneTransitionController;
        [SerializeField] private Transform EssentialObjectTransform; //Object to parent instantiated objects
        [SerializeField] private string InitialSceneToLoad; //Name of scene to load on start
        [SerializeField] private Vector3 StartPosition; //Start position of the player
        [SerializeField] private string BattleSceneName;

        public GameState CurrentGameState { get; private set; }
        public static string MainMenuScene { get => "MainMenu"; }
        public static string BattleScene => Singleton.BattleSceneName;

        public enum GameState { InGame, Paused, Menu, Battle }

        private PlayerController player;
        private CameraController cam;
        public event Action<bool> OnPaused;

        private void Awake()
        {
            CurrentGameState = GameState.Menu; //initialize to Menu since first loads when at main menu
            OnAwake(this);
        }

        private void OnDestroy()
        {
            DestroySingleton();
        }

        public void SwapScenes(ISceneData currentScene, ISceneData nextScene)
        {
            StartCoroutine(SwapScenesCoroutine(currentScene, nextScene));
        }

        private IEnumerator SwapScenesCoroutine(ISceneData currentScene, ISceneData nextScene)
        {
            UnloadScene(currentScene);

            yield return null;

            SceneManager.UnloadSceneAsync(currentScene.SceneName);

            yield return LoadSceneAndThen(nextScene.SceneName, LoadSceneMode.Additive, () => nextScene.OnLoad());
        }

        private void UnloadScene(ISceneData currentScene)
        {
            sceneTransitionController.FadeToBlack();
            currentScene.UnLoad();
        }

        /// <summary>
        /// Start the game
        /// Can also be used to reload the game upon death
        /// </summary>
        public void StartGame()
        {
            if (CurrentGameState == GameState.Menu)
            {
                string path = Application.persistentDataPath + "/saves/" + SaveData.Current.SaveName + ".save";
                SaveData.Current = (SaveData)SerializationManager.Load(path);

                Debug.Log("Loaded");
            }

            CurrentGameState = GameState.InGame;

            InitialSceneToLoad = string.IsNullOrEmpty(SaveData.Current.PlayerSceneName) ?
                InitialSceneToLoad : SaveData.Current.PlayerSceneName;

            if (SaveData.Current.PlayerPosition != Vector3.zero)
                StartPosition = SaveData.Current.PlayerPosition;

            StartCoroutine(LoadInitialScene(InitialSceneToLoad));
        }

        public void PauseGame(bool isPaused)
        {
            CurrentGameState = isPaused ? GameState.Paused : GameState.InGame;

            OnPaused?.Invoke(isPaused);
        }

        /// <summary>
        /// Which scene to load initially and all essential objects
        /// </summary>
        /// <param name="initialSceneName">Name of the scene to load</param>
        /// <returns>IEnumerator so it can be run as a coroutine</returns>
        private IEnumerator LoadInitialScene(string initialSceneName)
        {
            cam = EssentialObjectTransform.GetComponentInChildren<CameraController>();
            if (cam == null)
            { //Make sure the camera is in the scene
                cam = Instantiate(CameraPrefab, EssentialObjectTransform);
                cam.transform.position = /*StartPosition +*/ Vector3.back * 10;
            }

            player = EssentialObjectTransform.GetComponentInChildren<PlayerController>();
            if (player == null)
            { //Make sure the player is in the scene
                player = Instantiate(PlayerPrefab, EssentialObjectTransform);
            }

            if (SceneManager.GetActiveScene().name != initialSceneName)
            { //Make sure the scene isn't already loaded
                yield return SceneManager.LoadSceneAsync(initialSceneName, LoadSceneMode.Additive);
            }

            player.OnLoad(StartPosition);
        }

        public void LoadMainMenu() => SceneManager.LoadScene(MainMenuScene);

        public void ToggleLevelSceneObjects(bool isActive)
        {
            player.gameObject.SetActive(isActive);
            cam.gameObject.SetActive(isActive);
        }

        public void SetCameraVisible(bool isVisible)
        {
            cam.gameObject.SetActive(isVisible);
        }

        private IEnumerator LoadSceneAndThen(string sceneName, LoadSceneMode mode, Action action)
        {
            float duration = 0f;
            var sceneLoading = SceneManager.LoadSceneAsync(sceneName, mode);
            
            while (!sceneLoading.isDone)
            {
                duration += Time.deltaTime;
                yield return null;
            }

            yield return sceneTransitionController.FadeToScene(duration);

            //wait for two frames
            yield return null;
            yield return null;

            action.Invoke();
        }
    }
}