using UnityEngine;
using UnityEngine.UI;

public abstract class Highlightable : MonoBehaviour
{
    [SerializeField] private Image Highlight;

    private void Awake()
    {
        Highlight.enabled = false;
    }

    public void SetHighlighted(bool _isHighlighted)
    {
        Highlight.enabled = _isHighlighted;
    }
}
