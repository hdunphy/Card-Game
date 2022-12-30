using Assets.Scripts.Entities.Player;
using Assets.Scripts.Helpers;
using UnityEngine;

namespace Assets.Scripts.UI.GameScene.MingmingStorage
{
    public class MingmingStorageContainerController : MonoBehaviour
    {
        [SerializeField] private Transform mingmingStorageTransform;
        [Header("Prefabs")]
        [SerializeField] private GameObject horizontalRowPrefab;
        [SerializeField] private StoredMingmingController storedMingmingPrefab;

        public void Setup(PlayerMingmingHolder mingmingHolder)
        {
            var rows = mingmingHolder.StorageMingmings.ChunkBy(3);
            rows.ForEach(row => {
                var horizontalRow = Instantiate(horizontalRowPrefab, mingmingStorageTransform);
                row.ForEach(mingming =>
                {
                    var storedMingming = Instantiate(storedMingmingPrefab, horizontalRow.transform);
                    storedMingming.Setup(mingming);
                });
            });
        }
    }
}