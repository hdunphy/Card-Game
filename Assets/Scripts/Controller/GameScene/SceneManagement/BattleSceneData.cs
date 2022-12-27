using Assets.Scripts.Entities;
using Assets.Scripts.Entities.Player;
using System.Collections.Generic;

namespace Assets.Scripts.GameScene.Controller.SceneManagement
{
    public class DevBattleSceneInfo
    {
        public IEnumerable<CardData> Cards { get; private set; }
        public IEnumerable<MingmingInstance> Mingmings { get; private set; }
        public IInventory Inventory { get; private set; }

        public DevBattleSceneInfo(IEnumerable<CardData> cards, IEnumerable<MingmingInstance> mingmings, IInventory inventory)
        {
            Cards = cards;
            Mingmings = mingmings;
            Inventory = inventory;
        }
    }

    public class BattleSceneData : ISceneData
    {
        public string SceneName { get; private set; }
        public DevBattleSceneInfo PlayerInfo { get; private set; }
        public DevBattleSceneInfo EnemyInfo { get; private set; }
        public LevelSceneData PreviousLevel { get; private set; }

        public BattleSceneData(string sceneName, LevelSceneData previousLevel, DevBattleSceneInfo playerInfo, DevBattleSceneInfo enemyInfo)
        {
            SceneName = sceneName;
            PreviousLevel = previousLevel;
            PlayerInfo = playerInfo;
            EnemyInfo = enemyInfo;
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
