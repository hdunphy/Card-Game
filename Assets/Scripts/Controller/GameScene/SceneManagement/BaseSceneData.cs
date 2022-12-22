using System;

namespace Assets.Scripts.GameScene.Controller.SceneManagement
{
    public class BaseSceneData : ISceneData
    {
        public string SceneName { get; set; }
        private readonly Action _onLoadAction;
        private readonly Action _unLoadAction;

        public BaseSceneData(string sceneName, Action onLoadAction = null, Action unLoadAction = null)
        {
            SceneName = sceneName;
            _onLoadAction = onLoadAction;
            _unLoadAction = unLoadAction;
        }

        public void OnLoad() => _onLoadAction?.Invoke();

        public void UnLoad() => _unLoadAction?.Invoke();
    }
}
