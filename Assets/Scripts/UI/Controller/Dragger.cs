using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Dragger : MonoBehaviour
{
    private LineRenderer LineRenderer;
    private Transform DragTarget;
    public static Dragger Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Debug.Log("instance already exists, destroying object!");
            Destroy(this);
        }
    }

    private void Start()
    {
        LineRenderer = GetComponent<LineRenderer>();
        LineRenderer.enabled = false;
    }

    private void Update()
    {
        if (LineRenderer.enabled && DragTarget != null)
        {
            LineRenderer.SetPosition(0, transform.position);
            LineRenderer.SetPosition(1, DragTarget.position);
        }
    }

    public void StartDragging(Transform _dragTarget)
    {
        DragTarget = _dragTarget;
        transform.position = DragTarget.position;
        LineRenderer.enabled = true;
        LineRenderer.SetPosition(0, transform.position);
    }

    public void EndDragging()
    {
        LineRenderer.enabled = false;
        DragTarget = null;
    }
}
