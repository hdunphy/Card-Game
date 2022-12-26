using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Repository
{
    [CreateAssetMenu(menuName = "Data/Repository/Mingming")]
    public class MingMingRepository : ScriptableObject
    {
        [SerializeField] private List<MingmingData> mingmingDatas = new();

        private void OnValidate()
        {
            mingmingDatas = mingmingDatas.OrderBy(m => m.name).ToList();
            if (mingmingDatas.Any(m => m.ID == -1))
            {
                Debug.LogWarning("Mingmings with missing Id's");
                mingmingDatas.Select((m, i) => m.SetId(i));
            }
        }

        public MingmingData GetMingmingById(int id) => mingmingDatas.Single(m => m.ID == id);
    }
}