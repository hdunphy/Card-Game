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
                if (_current == null)
                {
                    _current = new SaveData();
                }
                return _current;
            }
            set { _current = value; }
        }

        private string saveName; //The name of the current save
        private Vector3 playerPosition; //The current player position on last save
        private string playerSceneName; //The current scene the player is on last save
        private List<MonsterSaveModel> playerMonsters; //list of player's monsters
        private DeckHolderSaveModel deckHolder;  //holds player cards and deck list
        private System.Random random; //hold the random state so we can continue

        public Vector3 PlayerPosition { get => playerPosition; set => playerPosition = value; } 
        public string SaveName { get => saveName; set => saveName = value; }
        public string PlayerSceneName { get => playerSceneName; set => playerSceneName = value; }
        public List<MonsterSaveModel> PlayerMonsters { get => playerMonsters; set => playerMonsters = value; }
        public DeckHolderSaveModel DeckHolder { get => deckHolder; set => deckHolder = value; }
        public System.Random Random { get => random; set => random = value; }

        public SaveData()
        {
            SaveName = "save1"; //Set here for now, but for multiple saves will need to set somewhere else
        }
    }
}
