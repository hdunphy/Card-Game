using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Controller.References
{
    [CreateAssetMenu(menuName = "Data/Scriptable Object Reference")]
    public class ScriptableObjectReference : ScriptableObject
    {
        [SerializeField] private List<CardData> Cards;
        [SerializeField] private List<MingmingData> Mingmings;

        public T GetScriptableObject<T>(string name) where T : ScriptableObject
            => typeof(T).Name switch
            {
                nameof(CardData) => (T)(ScriptableObject)Cards.FirstOrDefault(c => c.name == name),
                nameof(MingmingData) => (T)(ScriptableObject)Mingmings.FirstOrDefault(m => m.name == name),
                _ => default,
            };
    }
}
