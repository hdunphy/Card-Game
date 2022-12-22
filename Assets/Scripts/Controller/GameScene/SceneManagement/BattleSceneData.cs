using Assets.Scripts.Entities;
using System.Collections.Generic;

namespace Assets.Scripts.GameScene.Controller.SceneManagement
{
    public class BattleSceneData : ISceneData
    {
        public string SceneName { get; private set; }
        public IEnumerable<CardData> EnemyCards { get; private set; }
        public IEnumerable<CardData> PlayerCards { get; private set; }
        public IEnumerable<MingmingInstance> EnemyMingmings { get; private set; }
        public IEnumerable<MingmingInstance> PlayerMingmings { get; private set; }
        public LevelSceneData PreviousLevel { get; private set; }

        public BattleSceneData(string sceneName, IEnumerable<CardData> playerCards, IEnumerable<CardData> enemyCards, LevelSceneData previousLevel,
            IEnumerable<MingmingInstance> playerMingmings, IEnumerable<MingmingInstance> enemyMingmings)
        {
            SceneName = sceneName;
            EnemyCards = enemyCards;
            PlayerCards = playerCards;
            EnemyMingmings = enemyMingmings;
            PlayerMingmings = playerMingmings;
            PreviousLevel = previousLevel;
        }

        public void OnLoad()
        {
            GameSceneController.Singleton.ToggleLevelSceneObjects(false);

            BattleManager.Singleton.StartBattle(this);
        }

        public void UnLoad()
        {
        }
    }
}
