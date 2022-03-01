using UnityEngine;

namespace Assets.Scripts.UI.Controller
{
    public class Quantity : MonoBehaviour
    {
        [SerializeField] private TMPro.TMP_Text Text;
        [SerializeField] private GameObject DisabledCoverPrefab;

        private GameObject DisabledCover;
        private SelectableCard Card;
        private int _count;
        public int Count
        {
            get => _count;
            set
            {
                _count = value;
                Text.text = _count.ToString();
                //Should trigger an event or something
                if(DisabledCover != null)
                {
                    bool _isActive = _count <= 0;
                    DisabledCover.SetActive(_isActive);
                    Card.enabled = !_isActive;
                }
            }
        }

        private void OnDestroy()
        {
            if(Card != null)
                Card.OnSelected -= DecrementCount;
        }

        public void Setup(int count, SelectableCard card)
        {
            Card = card;
            DisabledCover = Instantiate(DisabledCoverPrefab, Card.transform);
            Count = count;

            Card.OnSelected += DecrementCount;
        }

        private void DecrementCount(SelectableCard card) => Count -= 1;
    }
}