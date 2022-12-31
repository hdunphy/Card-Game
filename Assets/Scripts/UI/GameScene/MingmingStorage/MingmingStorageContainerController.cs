using Assets.Scripts.Entities.Mingmings;
using Assets.Scripts.Entities.Player;
using Assets.Scripts.Helpers;
using System;
using UnityEngine;

namespace Assets.Scripts.UI.GameScene.MingmingStorage
{
    public class MingmingStorageContainerController : MonoBehaviour
    {
        [SerializeField] private Transform mingmingStorageTransform;
        [Header("Prefabs")]
        [SerializeField] private GameObject horizontalRowPrefab;
        [SerializeField] private StoredMingmingController storedMingmingPrefab;

        public event Action<MingmingInstance> OnTryToAddPartyElement;

        public void Setup(PlayerMingmingHolder mingmingHolder)
        {
            mingmingStorageTransform.DestroyAllChildren();

            var rows = mingmingHolder.StorageMingmings.ChunkBy(3);
            rows.ForEach(row => {
                var horizontalRow = Instantiate(horizontalRowPrefab, mingmingStorageTransform);
                row.ForEach(mingming =>
                {
                    var storedMingming = Instantiate(storedMingmingPrefab, horizontalRow.transform);
                    storedMingming.Setup(mingming, OnTryToAddPartyElement);
                });
            });
        }
    }
}