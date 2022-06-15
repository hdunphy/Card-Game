using UnityEngine;

namespace Assets.Scripts.Entities
{
    [CreateAssetMenu(fileName = "Scrap", menuName = "Data/Items/Scrap", order = 0)]
    public class Scrap : ScriptableObject
    {
        [SerializeField]
        private Sprite Icon;
    }
}