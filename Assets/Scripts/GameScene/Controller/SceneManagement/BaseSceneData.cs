namespace Assets.Scripts.GameScene.Controller.SceneManagement
{
    public class BaseSceneData : ISceneData
    {
        public string SceneName { get; set; }

        public void OnLoad() { }

        public void UnLoad() { }
    }
}
