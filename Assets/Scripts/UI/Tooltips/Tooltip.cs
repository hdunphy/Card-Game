using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[ExecuteInEditMode()]
public class Tooltip : MonoBehaviour
{
    public TextMeshProUGUI headerField;
    public TextMeshProUGUI contentField;
    public LayoutElement layoutElement;
    public Vector2 Offset;

    public int characterWrapLimit;

    private RectTransform RectTransform;

    private void Awake()
    {
        RectTransform = GetComponent<RectTransform>();
    }

    public void SetText(string content, string header = "")
    {
        if (string.IsNullOrEmpty(header))
        {
            headerField.gameObject.SetActive(false);
        }
        else
        {
            headerField.gameObject.SetActive(true);
            headerField.text = header;
        }

        contentField.text = content;

        UpdateTooltipSize();
    }

    private void Update()
    {
        if (Application.isEditor)
            UpdateTooltipSize();

        Vector2 position = Mouse.current.position.ReadValue();

        if (position.x + RectTransform.rect.width > Screen.width)
            position.x = Screen.width - RectTransform.rect.width;
        if (position.y + RectTransform.rect.height > Screen.height)
            position.y = Screen.height - RectTransform.rect.height;

        transform.position = position + Offset;
    }

    private void UpdateTooltipSize()
    {
        int headerLength = headerField.text.Length;
        int contentLength = contentField.text.Length;

        layoutElement.enabled = (headerLength > characterWrapLimit || contentLength > characterWrapLimit);
    }
}
