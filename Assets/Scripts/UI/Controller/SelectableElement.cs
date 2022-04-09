using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Highlightable : MonoBehaviour
{
    [SerializeField] private Image Highlight;
    public bool IsSelected { get; private set; }

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
