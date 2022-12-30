using Assets.Scripts.Entities.Player;
using Assets.Scripts.Entities.Scriptable;
using Assets.Scripts.Helpers;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.UI.GameScene.MingmingStorage
{
    public class MingmingStorageContainerController : MonoBehaviour
    {
        [SerializeField] private Transform mingmingStorageTransform;
        [Header("Prefabs")]
        [SerializeField] private GameObject horizontalRowPrefab;
        [SerializeField] private StoredMingmingController storedMingmingPrefab;

        [SerializeField] public List<MingmingLevelData> Mingmings;

        private void Start()
        {
            Setup(new(Mingmings));
        }

        public void Setup(PlayerMingmingHolder mingmingHolder)
        {
            var rows = mingmingHolder.AllMingmings.ChunkBy(3);
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