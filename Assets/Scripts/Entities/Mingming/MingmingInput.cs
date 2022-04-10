using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Entities
{
    public class MingmingInput : MonoBehaviour, IPointerDownHandler, IDropHandler
    {
        private Mingming _mingming;

        private void Start()
        {
            _mingming = GetComponent<Mingming>();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                EventManager.Instance.OnSelectMingmingTrigger(_mingming);
            }
            else if (eventData.button == PointerEventData.InputButton.Right)
            {
                EventManager.Instance.OnResetSelectedTrigger();
            }
        }

        public void OnDrop(PointerEventData eventData)
        {
            if (eventData.pointerPress.TryGetComponent(out Card card))
            {
                EventManager.Instance.OnSelectTargetTrigger(_mingming, card);
            }
        }
    }
}