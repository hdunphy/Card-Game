using Assets.Scripts.GameScene.Controller;
using Assets.Scripts.GameScene.Controller.SceneManagement;
using Assets.Scripts.UI.Controller;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.GameScene.Entities
{
    public class TrainerEncounter : MonoBehaviour, IEncounter
    {
        [SerializeField] private TrainerController trainerController;
        [SerializeField] private List<CardData> CardRewards;
        [Header("Events")]
        [SerializeField] private UnityEvent OnStartEncounter;

        private PlayerController _player;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.TryGetComponent(out PlayerController player))
            {
                _player = player;

                if (trainerController.CanBattle)
                {
                    GetEncounter();
                }
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out PlayerController _))
            {
                _player = null;
            }
        }

        public void GetEncounter()
        {
            OnStartEncounter?.Invoke();
            var thisScene = new LevelSceneData(gameObject.scene.name, this, _player);
            var battleScene = new BattleSceneData(
                GameSceneController.BattleScene, 
                _player.DevController.DeckHolder.CurrentDeck,
                trainerController.DevController.DeckHolder.CurrentDeck, 
                thisScene, 
                _player.DevController.PlayableMonsters, 
                trainerController.DevController.PlayableMonsters
            );

            GameSceneController.Singleton.SwapScenes(thisScene, battleScene);
        }

        public List<CardData> GetRewards()
        {
            trainerController.LostBattle();
            return CardRewards;
        }
    }
}
