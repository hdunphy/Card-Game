using Assets.Scripts.Entities.SaveSystem;
using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.GameScene.Controller
{
    public class TrainerController : MonoBehaviour
    {
        public new string name;
        [SerializeField] private SharedController sharedController;

        public bool CanBattle { get; private set; }

        public SharedController SharedController => sharedController;

        private void Start()
        {
            sharedController.SetDeckHolder(null);
            sharedController.SetMonsters(null);
            CanBattle = false;

            StartCoroutine(GetCanBattle());
        }

        private IEnumerator GetCanBattle()
        {
            yield return new WaitForSeconds(3);


            CanBattle = SaveData.Current.GetTrainerCanBattle(name);
        }

        public void LostBattle()
        {
            SaveData.Current.SetTrainerCanBattle(name, false);
        }
    }
}
