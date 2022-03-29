using UnityEngine;

namespace Assets.Scripts.Entities.Sound
{
    public class InGameSoundManager : MonoBehaviour
    {
        [SerializeField] private SoundEventManager SoundEventManager;

        private void Awake()
        {
            SoundEventManager.AddToGameObject(gameObject);
        }

        private void Start()
        {
            SoundEventManager.PlayStart();
        }

        private void OnDestroy()
        {
            SoundEventManager.RemoveFromGameObject();
        }
    }
}
