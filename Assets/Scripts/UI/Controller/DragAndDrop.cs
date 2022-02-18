using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CanvasGroup))]
public class DragAndDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] private Transform m_transform;
    [SerializeField] private UnityEvent OnBeginDragEvent;
    [SerializeField] private UnityEvent OnEndDragEvent;

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
        var position = Camera.main.WorldToScreenPoint(m_transform.position);
        position += (Vector3)eventData.delta;
        m_transform.position = Camera.main.ScreenToWorldPoint(position);
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
