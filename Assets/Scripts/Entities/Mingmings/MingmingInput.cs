using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Entities.Mingmings
{
    public class MingmingInput : MonoBehaviour, IPointerDownHandler
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
    }
}