using Assets.Scripts.Entities.Mingmings;
using Assets.Scripts.Entities.Player;
using Assets.Scripts.GameScene.Controller;
using UnityEngine;

namespace Assets.Scripts.UI.GameScene.MingmingStorage
{
    public class MingmingManagerUIController : MonoBehaviour
    {
        [SerializeField] private Canvas mingmingMangerCanvas;
        [SerializeField] private MingmingStorageContainerController storageContainer;
        [SerializeField] private MingmingPartyContainerController partyContainer;

        private PlayerMingmingHolder _mingmingHolder;
        private PlayerInputController _playerInputController;

        private void Start()
        {
            mingmingMangerCanvas.gameObject.SetActive(false);
            _playerInputController = FindObjectOfType<PlayerInputController>();

            partyContainer.OnTryToRemovePartyElement += HandleTryToRemovePartyElement;
            storageContainer.OnTryToAddPartyElement += HandleTryToAddPartyElement;
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

        public void Show(PlayerController playerController)
        {
            mingmingMangerCanvas.gameObject.SetActive(true);
            _playerInputController.enabled = false;
            Setup((PlayerMingmingHolder)playerController.DevController.MingmingHolder);
        }

        public void Hide()
        {
            mingmingMangerCanvas.gameObject.SetActive(false);
            _playerInputController.enabled = true;
        }

        public void Setup(PlayerMingmingHolder mingmingHolder)
        {
            _mingmingHolder = mingmingHolder;
            storageContainer.Setup(mingmingHolder);
            partyContainer.Setup(mingmingHolder);
        }
    }
}