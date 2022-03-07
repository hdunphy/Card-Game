using Assets.Scripts.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserMessage : MonoBehaviour
{
    public static UserMessage Instance;

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
    }

    public void SendMessageToUser(string _msg)
    {
        DisplayLog.AddMessageToLog(_msg);
        Debug.LogWarning(_msg);
    }
}
