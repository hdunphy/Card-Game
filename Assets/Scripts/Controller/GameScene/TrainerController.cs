using Assets.Scripts.Entities.GameScene;
using Assets.Scripts.Entities.SaveSystem;
using Assets.Scripts.Entities.Trainers;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.GameScene.Controller
{
    public class TrainerController : MonoBehaviour
    {
        public new string name;
        [SerializeField] private DevController devController;
        [SerializeField] private DevStartingInfo startingInfo;

        public bool CanBattle { get; private set; }

        public DevController DevController => devController;

        private void Start()
        {
            devController.SetDeckHolder(new TrainerDeckHolder(startingInfo.Deck.ToList()));
            devController.SetMingmings(startingInfo.Mingmings.ToList());
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

        //TODO: implement trainer loading & saving for rebattling
    }
}
