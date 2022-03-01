using Assets.Scripts.Entities;
using Assets.Scripts.UI.Controller;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneController : MonoBehaviour
{
    public static GameSceneController Singleton { get; private set; }

    [SerializeField] private PlayerController Player;
    [SerializeField] private CameraController LevelCamera;
    [SerializeField] private string BattleSceneName;
    [SerializeField] private string LevelSceneName; //TODO will need anotherway to get/set this with more levels

    private IEncounter EncounterCaller;

    private void Awake()
    {
        //Singleton pattern On Awake set the singleton to this.
        //There should only be one GameLayer that can be accessed statically
        if (Singleton == null)
        {
            Singleton = this;
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

    public void LoadLevelScene(float seconds, bool didPlayerOneWin)
    {
        StartCoroutine(LoadLevelSceneWait(seconds, didPlayerOneWin));
    }

    private IEnumerator LoadLevelSceneWait(float seconds, bool didPlayerOneWin)
    {
        yield return new WaitForSeconds(seconds);

        SceneManager.UnloadSceneAsync(BattleSceneName);

        StartCoroutine(LoadSceneAndThen(LevelSceneName, LoadSceneMode.Additive, () => LoadLevelScene(didPlayerOneWin)));
    }

    private void ToggleLevelSceneObjects(bool _isActive)
    {
        Player.gameObject.SetActive(_isActive);
        LevelCamera.gameObject.SetActive(_isActive);
    }

    private void LoadLevelScene(bool didPlayerOneWin)
    {
        ToggleLevelSceneObjects(true);
        if (didPlayerOneWin)
        {
            FindObjectOfType<RewardsController>().Show(EncounterCaller.GetRewards());
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

