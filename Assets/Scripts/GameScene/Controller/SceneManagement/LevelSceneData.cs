using Assets.Scripts.Entities.SaveSystem;
using Assets.Scripts.UI.Controller;

namespace Assets.Scripts.GameScene.Controller.SceneManagement
{
    public class LevelSceneData : ISceneData
    {
        public string SceneName { get; private set; }
        public bool DidPlayerOneWin { get; set; }
        private IEncounter encounter { get; set; }
        private PlayerController player { get; set; }

        public LevelSceneData(string sceneName, IEncounter encounter, PlayerController player)
        {
            SceneName = sceneName;
            this.encounter = encounter;
            this.player = player;
        }

        public void OnLoad()
        {
            GameSceneController.Singleton.ToggleLevelSceneObjects(true);

            if (DidPlayerOneWin)
            {
                RewardsController.Singleton.Show(encounter.GetRewards(), player);
            }
            else
            {
                player.EnterRoom(SaveData.Current.PlayerPosition);
                player.DevController.HealMonsters();
                player.GetComponent<PlayerInputController>().enabled = true;
            }
        }

        public void UnLoad()
        {
            player.GetComponent<PlayerInputController>().enabled = false;
        }
    }
}
