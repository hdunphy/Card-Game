using Assets.Scripts.Entities.Player;
using Assets.Scripts.Entities.Scriptable;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.UI.GameScene.MingmingStorage
{
    public class MingmingManagerUIController : MonoBehaviour
    {
        [SerializeField] private MingmingStorageContainerController storageContainer;
        [SerializeField] private MingmingPartyContainerController partyContainer;

        [SerializeField] public List<MingmingLevelData> Mingmings;

        private void Start()
        {
            Setup(new(Mingmings));
        }

        public void Setup(PlayerMingmingHolder mingmingHolder)
        {
            storageContainer.Setup(mingmingHolder);
            partyContainer.Setup(mingmingHolder);
        }
    }
}