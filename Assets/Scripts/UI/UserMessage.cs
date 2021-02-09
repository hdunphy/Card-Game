using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserMessage : MonoBehaviour
{
    public static UserMessage Instance;

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

    public void SendMessageToUser(string _msg)
    {
        Debug.LogWarning(_msg);
    }
}
