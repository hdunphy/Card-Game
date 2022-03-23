using Assets.Scripts.GameScene.Controller;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.GameScene.Entities
{
    public class TrainerEncounter : MonoBehaviour, IEncounter
    {
        [SerializeField] private SharedController TrainerController;
        [SerializeField] private List<CardData> CardRewards;
        [Header("Events")]
        [SerializeField] private UnityEvent OnStartEncounter;

        PlayerController player;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.TryGetComponent(out PlayerController _player))
            {
                player = _player;
                GetEncounter();
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out PlayerController _player))
            {
                player = null;
            }
        }

        public void GetEncounter()
        {
            OnStartEncounter?.Invoke();
            GameSceneController.Singleton.LoadBattleScene(player.SharedController.PlayableMonsters, TrainerController.PlayableMonsters,
                player.SharedController.DeckHolder.CurrentDeck, TrainerController.DeckHolder.CurrentDeck, this);
        }

        public List<CardData> GetRewards() => CardRewards;
    }
}
