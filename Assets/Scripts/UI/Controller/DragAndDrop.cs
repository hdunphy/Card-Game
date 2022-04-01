using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CanvasGroup))]
public class DragAndDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] private UnityEvent OnBeginDragEvent;
    [SerializeField] private UnityEvent OnEndDragEvent;

    private Transform m_transform;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void SetRectTransform(Transform transform)
    {
        m_transform = transform;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = false;
        OnBeginDragEvent.Invoke();
    }

    public void OnDrag(PointerEventData eventData)
    {
        var position = Camera.main.ScreenToWorldPoint(eventData.position);

        position.z = m_transform.position.z;
        m_transform.position = position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        OnEndDragEvent.Invoke();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
    }
}
