using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class HoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private float Movement;
    [SerializeField] private float Scaling;
    [SerializeField] private float Timing;
    [SerializeField] private UnityEvent OnHoverStart;
    [SerializeField] private UnityEvent OnHoverEnd;

    LTDescr move = null;
    LTDescr scale = null;

    private void OnDisable()
    {
        if(move != null)
            LeanTween.cancel(move.id);
        if(scale != null)
            LeanTween.cancel(scale.id);
    }

    public void ReturnToNormalPosition()
    {
        OnHoverEnd.Invoke();
        move = LeanTween.moveLocalY(gameObject, 0, Timing).setFrom(Movement);
        scale = LeanTween.scale(gameObject, Vector3.one, Timing);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ReturnToNormalPosition();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //transform.SetAsLastSibling();
        OnHoverStart.Invoke();
        move = LeanTween.moveLocalY(gameObject, Movement, Timing).setFrom(0);
        scale = LeanTween.scale(gameObject, Vector3.one * Scaling, Timing);
    }
}
