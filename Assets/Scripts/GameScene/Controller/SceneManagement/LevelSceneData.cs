using Assets.Scripts.Entities.SaveSystem;
using Assets.Scripts.UI.Controller;

namespace Assets.Scripts.GameScene.Controller.SceneManagement
{
    public class LevelSceneData : ISceneData
    {
        public string SceneName { get; private set; }
        public bool DidPlayerOneWin { get; set; }
        private IEncounter encounter { get; set; }
        private RewardsController rewardsController { get; set; }
        private PlayerController player { get; set; }

        public LevelSceneData(string sceneName, IEncounter encounter, RewardsController rewardsController, PlayerController player)
        {
            SceneName = sceneName;
            this.encounter = encounter;
            this.rewardsController = rewardsController;
            this.player = player;
        }

        public void OnLoad()
        {
            GameSceneController.Singleton.ToggleLevelSceneObjects(true);

            if (DidPlayerOneWin)
            {
                rewardsController.Show(encounter.GetRewards());
            }
            else
            {
                player.EnterRoom(SaveData.Current.PlayerPosition);
                player.DevController.HealMonsters();
            }
        }

        public void UnLoad()
        {
            GameSceneController.Singleton.ToggleLevelSceneObjects(false);
        }
    }
}
