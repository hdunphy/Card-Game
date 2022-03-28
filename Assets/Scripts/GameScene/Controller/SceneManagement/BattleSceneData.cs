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
            IEnumerable<MingmingInstance> playerMonsters, IEnumerable<MingmingInstance> enemyMonsters)
        {
            SceneName = sceneName;
            EnemyCards = enemyCards;
            PlayerCards = playerCards;
            EnemyMingmings = enemyMonsters;
            PlayerMingmings = playerMonsters;
            PreviousLevel = previousLevel;
        }

        public void OnLoad()
        {
            BattleManager.Singleton.StartBattle(this);
        }

        public void UnLoad()
        {
        }
    }
}
