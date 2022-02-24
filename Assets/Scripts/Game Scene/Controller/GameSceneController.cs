using Assets.Scripts.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneController : MonoBehaviour
{
    public static GameSceneController Singleton { get; private set; }

    [SerializeField] private string BattleSceneName;
    [SerializeField] private string LevelSceneName; //TODO will need anotherway to get/set this with more levels

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

    public void LoadBattleScene(IEnumerable<MonsterInstance> playerMonsters, IEnumerable<MonsterInstance> enemyMonsters)
    {
        FindObjectOfType<PlayerController>().gameObject.SetActive(false);
        FindObjectOfType<CameraController>().gameObject.SetActive(false);

        SceneManager.UnloadSceneAsync(LevelSceneName);

        StartCoroutine(LoadSceneAndThen(BattleSceneName, LoadSceneMode.Additive, () =>
            BattleManager.Singleton.StartBattle(playerMonsters, enemyMonsters)));
    }

    private IEnumerator LoadSceneAndThen(string sceneName, LoadSceneMode mode, Action action)
    {
        yield return SceneManager.LoadSceneAsync(sceneName, mode);

        action.Invoke();
    }
}

