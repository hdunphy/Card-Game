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

    int move = -1;
    int scale = -1;

    private void OnDisable()
    {
        if(move != -1)
        {
            LeanTween.cancel(move);
            move = -1;
        }
        if(scale != -1)
        {
            LeanTween.cancel(scale);
            scale = -1;
        }
    }

    public void ReturnToNormalPosition()
    {
        OnHoverEnd.Invoke();
        move = LeanTween.moveLocalY(gameObject, 0, Timing).setFrom(Movement).id;
        scale = LeanTween.scale(gameObject, Vector3.one, Timing).id;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ReturnToNormalPosition();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //transform.SetAsLastSibling();
        OnHoverStart.Invoke();
        move = LeanTween.moveLocalY(gameObject, Movement, Timing).setFrom(0).id;
        scale = LeanTween.scale(gameObject, Vector3.one * Scaling, Timing).id;
    }
}
