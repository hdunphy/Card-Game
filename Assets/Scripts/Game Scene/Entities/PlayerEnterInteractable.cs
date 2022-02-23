using UnityEngine;
using UnityEngine.Events;

public class PlayerEnterInteractable : MonoBehaviour
{
    [SerializeField] private float Timer;
    [SerializeField] private UnityEvent OnTimer;

    bool playerInZone;

    private float TimeLeft;

    private void Start()
    {
        playerInZone = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerController _))
        {
            playerInZone = true;
            TimeLeft = Timer;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerController _))
        {
            playerInZone = false;
        }
    }

    private void Update()
    {
        if (playerInZone)
        {
            TimeLeft -= Time.deltaTime;
            if (TimeLeft <= 0)
            {
                OnTimer?.Invoke();
                TimeLeft = Timer;
            }
        }
    }
}
