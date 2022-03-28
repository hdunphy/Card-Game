namespace Assets.Scripts.GameScene.Controller.SceneManagement
{
    public interface ISceneData
    {
        public string SceneName { get; }

        void OnLoad();
        void UnLoad();
    }
}
