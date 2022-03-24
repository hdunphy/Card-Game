using Assets.Scripts.Controller.SaveSystem;
using Assets.Scripts.Entities;
using Assets.Scripts.Entities.SaveSystem;
using Assets.Scripts.UI.Controller;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneController : MonoBehaviour
{
    public static GameSceneController Singleton { get; private set; }

    [SerializeField] private PlayerController PlayerPrefab;
    [SerializeField] private CameraController CameraPrefab;
    [SerializeField] private Transform EssentialObjectTransform; //Object to parent instantiated objects
    [SerializeField] private string InitialSceneToLoad; //Name of scene to load on start
    [SerializeField] private Vector3 StartPosition; //Start position of the player
    [SerializeField] private string BattleSceneName;
    [SerializeField] private string LevelSceneName; //TODO will need anotherway to get/set this with more levels

    public GameState CurrentGameState { get; private set; }
    public static string MainMenuScene { get => "MainMenu"; }

    public enum GameState { InGame, Paused, Menu }

    private IEncounter EncounterCaller;
    private PlayerController player;
    private CameraController cam;

    private void Awake()
    {
        //Singleton pattern On Awake set the singleton to this.
        //There should only be one GameSceneController that can be accessed statically
        if (Singleton == null)
        {
            Singleton = this;
            CurrentGameState = GameState.Menu; //initialize to Menu since first loads when at main menu
        }
        else
        { //if GameSceneController already exists then destory this. We don't want duplicates
            Destroy(this);
        }
    }

    public void LoadBattleScene(IEnumerable<MonsterInstance> playerMonsters, IEnumerable<MonsterInstance> enemyMonsters, 
        List<CardData> playerCards, List<CardData> enemyCards, IEncounter _encounterCaller)
    {
        EncounterCaller = _encounterCaller;
        ToggleLevelSceneObjects(false);

        SceneManager.UnloadSceneAsync(LevelSceneName);

        StartCoroutine(LoadSceneAndThen(BattleSceneName, LoadSceneMode.Additive, () =>
            BattleManager.Singleton.StartBattle(playerMonsters, enemyMonsters, playerCards, enemyCards)));
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
            cam.transform.position = /*StartPosition +*/ (Vector3.back * 10);
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

    public void LoadLevelScene(float seconds, bool didPlayerOneWin)
    {
        StartCoroutine(LoadLevelSceneWait(seconds, didPlayerOneWin));
    }

    private IEnumerator LoadLevelSceneWait(float seconds, bool didPlayerOneWin)
    {
        yield return new WaitForSeconds(seconds);

        SceneManager.UnloadSceneAsync(BattleSceneName);

        yield return LoadSceneAndThen(LevelSceneName, LoadSceneMode.Additive, () => LoadLevelScene(didPlayerOneWin));
    }

    private void ToggleLevelSceneObjects(bool _isActive)
    {
        player.gameObject.SetActive(_isActive);
        cam.gameObject.SetActive(_isActive);
    }

    private void LoadLevelScene(bool didPlayerOneWin)
    {
        ToggleLevelSceneObjects(true);

        if (didPlayerOneWin)
        {
            FindObjectOfType<RewardsController>().Show(EncounterCaller.GetRewards());
        }
        else
        {
            player.EnterRoom(SaveData.Current.PlayerPosition);
        }
    }

    private IEnumerator LoadSceneAndThen(string sceneName, LoadSceneMode mode, Action action)
    {
        yield return SceneManager.LoadSceneAsync(sceneName, mode);

        //wait for two frames
        yield return null;
        yield return null;

        action.Invoke();
    }
}

