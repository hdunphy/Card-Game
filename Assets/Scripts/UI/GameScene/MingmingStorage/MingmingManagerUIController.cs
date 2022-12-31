using Assets.Scripts.Entities.Interfaces;
using Assets.Scripts.Entities.Mingmings;
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

        private PlayerMingmingHolder _mingmingHolder;

        private void Start()
        {
            partyContainer.OnTryToRemovePartyElement += HandleTryToRemovePartyElement;
            storageContainer.OnTryToAddPartyElement += HandleTryToAddPartyElement;

            Setup(new(Mingmings));
        }

        private void OnDestroy()
        {
            partyContainer.OnTryToRemovePartyElement -= HandleTryToRemovePartyElement;
            storageContainer.OnTryToAddPartyElement -= HandleTryToAddPartyElement;
        }

        private void HandleTryToRemovePartyElement(MingmingInstance mingming)
        {
            var isSuccessful = _mingmingHolder.TryRemoveFromParty(mingming);
            if (!isSuccessful)
            {
                Debug.Log("Could not Remove from party");
                return;
            }

            Setup(_mingmingHolder);
        }

        private void HandleTryToAddPartyElement(MingmingInstance mingming)
        {
            var isSuccessful = _mingmingHolder.TryAddToParty(mingming);
            if (!isSuccessful)
            {
                Debug.Log("Could not Add to party"); return;
            }

            Setup(_mingmingHolder);
        }

        public void Setup(PlayerMingmingHolder mingmingHolder)
        {
            _mingmingHolder = mingmingHolder;
            storageContainer.Setup(mingmingHolder);
            partyContainer.Setup(mingmingHolder);
        }
    }
}