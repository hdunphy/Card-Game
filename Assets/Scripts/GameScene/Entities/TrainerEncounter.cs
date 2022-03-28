﻿using Assets.Scripts.GameScene.Controller;
using Assets.Scripts.GameScene.Controller.SceneManagement;
using Assets.Scripts.UI.Controller;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.GameScene.Entities
{
    public class TrainerEncounter : MonoBehaviour, IEncounter
    {
        [SerializeField] private TrainerController TrainerController;
        [SerializeField] private List<CardData> CardRewards;
        [Header("Events")]
        [SerializeField] private UnityEvent OnStartEncounter;

        PlayerController player;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.TryGetComponent(out PlayerController _player))
            {
                player = _player;

                if (TrainerController.CanBattle)
                {
                    GetEncounter();
                }
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
            var thisScene = new LevelSceneData(gameObject.scene.name, this, FindObjectOfType<RewardsController>(), player);
            var battleScene = new BattleSceneData(
                GameSceneController.BattleScene, 
                player.DevController.DeckHolder.CurrentDeck,
                TrainerController.DevController.DeckHolder.CurrentDeck, 
                thisScene, 
                player.DevController.PlayableMonsters, 
                TrainerController.DevController.PlayableMonsters
            );

            GameSceneController.Singleton.SwapScenes(thisScene, battleScene);
        }

        public List<CardData> GetRewards()
        {
            TrainerController.LostBattle();
            return CardRewards;
        }
    }
}
