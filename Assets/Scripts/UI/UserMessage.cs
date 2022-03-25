using Assets.Scripts.UI;
using UnityEngine;

public class UserMessage : MonoBehaviour
{
    public static UserMessage Instance;

    [HideInInspector]
    public bool CanSendMessage;

    IDisplayLog DisplayLog;

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
        DisplayLog = GetComponent<IDisplayLog>();

        CanSendMessage = true;
    }

    public void SendMessageToUser(string _msg)
    {
        if (!CanSendMessage) return;

        DisplayLog.AddMessageToLog(_msg);
        Debug.LogWarning(_msg);
    }
}
