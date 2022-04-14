using Assets.Scripts.Entities;
using Assets.Scripts.Entities.Scriptable;
using Assets.Scripts.GameScene.Controller;
using Assets.Scripts.GameScene.Controller.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Controller
{
    public class TestBattleManager : MonoBehaviour
    {
        [SerializeField] private string sceneName = "BattleScene";
        [SerializeField] private List<CardData> playerCards;
        [SerializeField] private List<CardData> enemyCards;
        [SerializeField] private List<MingmingLevelData> playerMinmings;
        [SerializeField] private List<MingmingLevelData> enemyMinmings;

        // Use this for initialization
        void Start()
        {
            if(GameSceneController.Singleton is null)
            {                
                var _playerMingmings = playerMinmings.Select(m => new MingmingInstance(m.MingMingData, m.Level));
                var _enemyMingmings = enemyMinmings.Select(m => new MingmingInstance(m.MingMingData, m.Level));

                var battleScene = new BattleSceneData(sceneName, playerCards, enemyCards, null, _playerMingmings, _enemyMingmings);

                BattleManager.Singleton.StartBattle(battleScene);
            }
        }
    }
}