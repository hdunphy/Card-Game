using UnityEngine;

namespace Assets.Scripts.GameScene.Controller
{
    public class TrainerController : MonoBehaviour
    {
        [SerializeField] private SharedController sharedController;

        private void Start()
        {
            sharedController.SetDeckHolder(null);
            sharedController.SetMonsters(null);
        }
    }
}
