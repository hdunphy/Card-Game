using Assets.Scripts.Entities;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Controller
{
    public class CardInputController : MonoBehaviour
    {
        [SerializeField] private Image disableCover;
        [SerializeField] private HoverEffect hoverEffect;
        [SerializeField] private DragAndDrop dragAndDrop;
        [SerializeField] private Card card;

        private int _siblingIndex { get; set; }

        // Use this for initialization
        void Start()
        {
            dragAndDrop.SetRectTransform(Dragger.Instance.GetComponent<RectTransform>());
            
            EventManager.Instance.UpdateSelectedMingming += UpdateSelectedMingmingListener;
            card.ToggleInteractions += ToggleInteractions;
            card.OnSiblingIndexChanged += SetSiblingIndex;
        }
        private void OnDestroy()
        {
            EventManager.Instance.UpdateSelectedMingming -= UpdateSelectedMingmingListener;
            card.ToggleInteractions -= ToggleInteractions;
            card.OnSiblingIndexChanged -= SetSiblingIndex;
        }

        private void UpdateSelectedMingmingListener(Mingming mingming)
        {
            disableCover.gameObject.SetActive(false);
            if (mingming != null)
            {
                UserMessage.Instance.CanSendMessage = false;

                bool isCardPlayable = card.CanUseCard(mingming.Logic);
                disableCover.gameObject.SetActive(!isCardPlayable);
                dragAndDrop.enabled = isCardPlayable;

                UserMessage.Instance.CanSendMessage = true;
            }
        }

        public void OnBeginDrag()
        {
            ToggleOtherCardInteractions(false);

            hoverEffect.enabled = false;
            Dragger.Instance.StartDragging(transform);
        }

        public void OnEndDrag()
        {
            Dragger.Instance.EndDragging();
            if (!card.PlayedThisTurn)
            {
                hoverEffect.enabled = true;
                hoverEffect.ReturnToNormalPosition();
            }
            ToggleOtherCardInteractions(true);
        }

        private void ToggleOtherCardInteractions(bool isDisabled)
        {
            var cards = FindObjectsOfType<CardInputController>().Where(c => c != this && c.gameObject.activeSelf);
            foreach (var _card in cards)
            {
                _card.ToggleInteractions(isDisabled);
            }
        }

        public void ToggleInteractions(bool isEnabled)
        {
            hoverEffect.enabled = isEnabled;
            dragAndDrop.enabled = isEnabled;
        }

        public void SetSiblingIndex(int i) => _siblingIndex = i;

        //invoked by unity event
        public void OnHoverStart() => transform.SetAsLastSibling();

        //invoked by unity event
        public void OnHoverEnd() => transform.SetSiblingIndex(_siblingIndex);

    }
}