using Assets.Scripts.Entities.Player;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Entities.SaveSystem
{
    [Serializable]
    public class SaveData
    {
        //Singleton pattern
        private static SaveData _current;
        public static SaveData Current
        {
            get
            {
                _current ??= new SaveData();
                return _current;
            }
            set { _current = value; }
        }

        private string saveName; //The name of the current save
        private Vector3 playerPosition; //The current player position on last save
        private string playerSceneName; //The current scene the player is on last save
        private List<MingmingSaveModel> playerMingmings; //list of player's mingmings
        private DeckHolderSaveModel deckHolder;  //holds player cards and deck list
        private readonly Dictionary<string, bool> trainersCanBattle; //Reference to which trainers can battle at the moment
        private PlayerInventory playerInventory;

        public Vector3 PlayerPosition { get => playerPosition; set => playerPosition = value; } 
        public string SaveName { get => saveName; set => saveName = value; }
        public string PlayerSceneName { get => playerSceneName; set => playerSceneName = value; }
        public List<MingmingSaveModel> PlayerMingmings { get => playerMingmings; set => playerMingmings = value; }
        public DeckHolderSaveModel DeckHolder { get => deckHolder; set => deckHolder = value; }
        public PlayerInventory PlayerInventory { get => playerInventory; set => playerInventory = value; }

        public SaveData()
        {
            SaveName = "save1"; //Set here for now, but for multiple saves will need to set somewhere else
            trainersCanBattle = new Dictionary<string, bool>();
        }

        public bool GetTrainerCanBattle(string name) => 
            !trainersCanBattle.TryGetValue(name, out bool canBattle) || canBattle;

        public void SetTrainerCanBattle(string name, bool canBattle)
        {
            if (trainersCanBattle.ContainsKey(name))
            {
                trainersCanBattle[name] = canBattle;
            }
            else
            {
                trainersCanBattle.Add(name, canBattle);
            }
        }
    }
}
