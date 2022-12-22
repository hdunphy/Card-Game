using Assets.Scripts.Entities.SaveSystem;
using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.GameScene.Controller
{
    public class TrainerController : MonoBehaviour
    {
        public new string name;
        [SerializeField] private DevController devController;

        public bool CanBattle { get; private set; }

        public DevController DevController => devController;

        private void Start()
        {
            devController.SetDeckHolder(null);
            devController.SetMingmings(null);
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
