using UnityEngine;

public class DeckBuilderUI : MonoBehaviour
{
    [SerializeField] private GameObject DeckBuilderCanvas;

    private void Start()
    {
        DeckBuilderCanvas.gameObject.SetActive(false);
    }

    public void Show()
    {
        DeckBuilderCanvas.gameObject.SetActive(true);
    }
}
