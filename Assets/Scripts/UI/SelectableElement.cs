using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class SelectableElement : MonoBehaviour
{
    [SerializeField] private Image Highlight;
    private bool IsSelected;

    private void Awake()
    {
        Highlight.enabled = false;
    }

    public void SetSelected(bool _isSelected)
    {
        IsSelected = _isSelected;
        Highlight.enabled = IsSelected;
    }
}
