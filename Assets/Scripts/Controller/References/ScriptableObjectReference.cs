using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Controller.References
{
    public class ScriptableObjectReference : MonoBehaviour
    {
        [SerializeField] private List<CardData> Cards;
        [SerializeField] private List<MonsterData> Monsters;

        public static ScriptableObjectReference Singleton;

        private void Awake()
        {
            if (Singleton == null)
            {
                Singleton = this;
            }
            else if (Singleton != this)
            {
                Debug.Log("instance already exists, destroying object!");
                Destroy(this);
            }
        }

        public T GetScriptableObject<T>(string name) where T : ScriptableObject
            => typeof(T).Name switch
            {
                nameof(CardData) => (T)(ScriptableObject)Cards.FirstOrDefault(c => c.name == name),
                nameof(MonsterData) => (T)(ScriptableObject)Monsters.FirstOrDefault(m => m.name == name),
                _ => default,
            };
    }
}
