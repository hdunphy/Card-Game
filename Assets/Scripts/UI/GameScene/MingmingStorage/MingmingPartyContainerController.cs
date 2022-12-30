using Assets.Scripts.Entities.Player;
using UnityEngine;

namespace Assets.Scripts.UI.GameScene.MingmingStorage
{
    public class MingmingPartyContainerController : MonoBehaviour
    {
        [SerializeField] private Transform partyContainerTransform;
        [SerializeField] private MingmingPartyElementController mingmingPartyElementPrefab;

        public void Setup(PlayerMingmingHolder mingmingHolder)
        {
            mingmingHolder.Party.ForEach(mingming =>
            {
                var partyElement = Instantiate(mingmingPartyElementPrefab, partyContainerTransform);
                partyElement.Setup(mingming);
            });
        }
    }
}