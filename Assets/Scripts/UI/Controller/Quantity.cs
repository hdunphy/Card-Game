using System;
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
                if(DisabledCover != null)
                {
                    DisabledCover.SetActive(_count <= 0);
                }
            }
        }

        private void OnDestroy()
        {
            Card.OnSelected -= DecrementCount;
        }

        public void Setup(int count, SelectableCard card)
        {
            Count = count;
            Card = card;
            DisabledCover = Instantiate(DisabledCoverPrefab, Card.transform);

            Card.OnSelected += DecrementCount;
        }

        private void DecrementCount(SelectableCard card) => Count -= 1;
    }
}