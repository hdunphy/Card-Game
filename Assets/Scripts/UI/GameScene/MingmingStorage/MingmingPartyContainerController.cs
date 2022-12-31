using Assets.Scripts.Entities.Mingmings;
using Assets.Scripts.Entities.Player;
using Assets.Scripts.Helpers;
using System;
using UnityEngine;

namespace Assets.Scripts.UI.GameScene.MingmingStorage
{
    public class MingmingPartyContainerController : MonoBehaviour
    {
        [SerializeField] private Transform partyContainerTransform;
        [SerializeField] private MingmingPartyElementController mingmingPartyElementPrefab;

        public event Action<MingmingInstance> OnTryToRemovePartyElement;

        public void Setup(PlayerMingmingHolder mingmingHolder)
        {
            partyContainerTransform.DestroyAllChildren();

            mingmingHolder.Party.ForEach(mingming =>
            {
                var partyElement = Instantiate(mingmingPartyElementPrefab, partyContainerTransform);
                partyElement.Setup(mingming, OnTryToRemovePartyElement);
            });
        }
    }
}